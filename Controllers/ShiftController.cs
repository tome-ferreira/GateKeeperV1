using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using GateKeeperV1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GateKeeperV1.Controllers
{
    public class ShiftController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;
        private readonly UserManager<ApplicationUser> userManager;

        public ShiftController(ApplicationDbContext dbContext, IFunctions functions, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.functions = functions;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var shifts = await dbContext.Shifts
                .Include(s => s.WorkerShifts)
                .Include(s => s.ShiftLeader)
                .ThenInclude(sl => sl.ApplicationUser)
                .Where(s => s.CompanyId.ToString() == companyId)
                .ToListAsync();

            return View(shifts);
        }

        [HttpGet]
        public async Task<IActionResult> CreateShift()
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var company = await dbContext.Companies.Include(c => c.Buildings).Where(c => c.Id.ToString() == companyId).FirstAsync();

            //var teams = await dbContext.WorkersTeams.Include(t => t.TeamMemberships).Where(t => t.CompanyId.ToString() == companyId).ToListAsync();
            var workers = await dbContext.WorkerProfiles.Include(t => t.ApplicationUser).Where(t => t.CompanyId.ToString() == companyId).ToListAsync();

            CreateShiftViewModel model = new CreateShiftViewModel()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };
            /*
            foreach (var t in teams)
            {
                TeamInCreateShiftViewModel tcsvm = new TeamInCreateShiftViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    isSelected = false
                };
                model.Teams.Add(tcsvm);
            }*/
            foreach (var w in workers)
            {
                WorkerInCreateShiftViewModel wcsvm = new WorkerInCreateShiftViewModel()
                {
                    Id = w.Id,
                    Name = w.ApplicationUser.Name + " " + w.ApplicationUser.Surname,
                    Email = w.ApplicationUser.Email,
                    isSelected = false
                };
                model.Workers.Add(wcsvm);
            }

            ViewBag.Company = company;



            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShift(CreateShiftViewModel model)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            /*
            if (!ModelState.IsValid)
            {
                return View(model);
            }*/

            var shift = new Shift
            {
                Name = model.Name,
                Description = model.Description,
                Starts = model.Starts,
                Ends = model.Ends,
                IsOvernight = model.IsOvernight,
                BuildingId = model.BuildingId,
                CompanyId = Guid.Parse(companyId),
                ShiftLeaderId = model.LeaderId,
                StartsDate = model.StartDate.ToDateTime(TimeOnly.MinValue), 
                EndsDate = model.EndDate.ToDateTime(TimeOnly.MinValue)
            };
            await dbContext.Shifts.AddAsync(shift);

            List<ShiftDays> shiftDaysList = new List<ShiftDays>();

            // Processar os dias se o turno for repetitivo
            if (model.IsRepetitive)
            {
                List<ShiftDaysOfWeek> shiftDaysOfWeeks = new List<ShiftDaysOfWeek>();

                foreach (var day in model.Days)
                {
                    int dayNumber = day switch
                    {
                        "Sunday" => 1,
                        "Monday" => 2,
                        "Tuesday" => 3,
                        "Wednesday" => 4,
                        "Thursday" => 5,
                        "Friday" => 6,
                        "Saturday" => 7,
                    };
                    shiftDaysOfWeeks.Add(new ShiftDaysOfWeek()
                    {
                        DayOfWeek = dayNumber,
                        isOvernight = model.IsOvernight,
                        ShiftId = shift.Id
                    });
                }

                await dbContext.ShiftDaysOfWeeks.AddRangeAsync(shiftDaysOfWeeks);
            }
            else
            {
                ShiftDays shiftDay = new ShiftDays()
                {
                    Date = model.EndDate.ToDateTime(model.Starts),
                    isOvernight = model.IsOvernight,
                    ShiftId  = shift.Id
                };

                await dbContext.ShiftDays.AddAsync(shiftDay);
            }

            List<WorkerShift> workers = new List<WorkerShift> ();

            foreach(var w in model.Workers.Where(w => w.isSelected))
            {
                workers.Add(new WorkerShift()
                {
                    ShiftId = shift.Id,
                    WorkerId = w.Id
                });
            }

            
            
            await dbContext.WorkerShifts.AddRangeAsync(workers);
            await dbContext.ShiftDays.AddRangeAsync(shiftDaysList);

            
            await dbContext.SaveChangesAsync();

            
            return RedirectToAction("Index");
        }



        [HttpPost]
        public async Task<IActionResult> UpdateWorkers([FromBody] UpdateTablesRequestViewModel request)
        {

            var selectedWorkers = request.SelectedWorkers; // Contains all workers with their selection status
            var selectedTeams = request.SelectedTeams;     // Teams selected by the user

            foreach (var team in selectedTeams.Where(t => t.isSelected))
            {
                // Retrieve all workers belonging to the current team
                var teamWorkers = await dbContext.WorkersTeams
                    .Include(t => t.TeamMemberships)
                    .Where(t => t.Id == team.Id)
                    .SelectMany(t => t.TeamMemberships)
                    .ToListAsync();

                // Mark workers in the current team as selected
                foreach (var tw in teamWorkers)
                {
                    var worker = selectedWorkers.Where(sw => sw.Id == tw.WorkerId).FirstOrDefault();

                    if (worker != null)
                    {
                        worker.isSelected = true;
                    }
                }
            }
            return PartialView("_WorkersTable", selectedWorkers);
        }

        [HttpGet]
        public async Task<IActionResult> ShiftDetails(Guid id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var shift = await dbContext.Shifts
                .Include(s => s.WorkerShifts)
                .ThenInclude(ws => ws.Worker)
                .ThenInclude(w => w.ApplicationUser)
                .Include(s => s.ShiftDays)
                .Include(s => s.ShiftDaysOfWeeks)
                .Include(s => s.Building)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            return View(shift);
                
        }

        /*
        [HttpGet]
        public async Task<IActionResult> EditShift(Guid id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var shift = await dbContext.Shifts
                .Include(s => s.WorkerShifts)
                .ThenInclude(ws => ws.Worker)
                .ThenInclude(w => w.ApplicationUser)
                .Include(s => s.ShiftDays)
                .Include(s => s.ShiftDaysOfWeeks)
                .Include(s => s.Building)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (shift.ShiftDaysOfWeeks.Any())
            {
                EditShiftViewModel editShiftViewModel = new EditShiftViewModel()
                {
                    ShiftId = shift.Id,
                    Name = shift.Name,
                    Description = shift.Description,
                    Starts = shift.Starts,
                    Ends = shift.Ends,

                };
            }
        }*/


    }  
}
