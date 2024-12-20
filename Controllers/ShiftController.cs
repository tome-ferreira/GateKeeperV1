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

            // Calculate the start and end dates for the current week
            /*
            DateTime today = DateTime.Today;
            int daysUntilSunday = (int)today.DayOfWeek;
            DateTime lastSunday = today.AddDays(-daysUntilSunday);
            DateTime nextSaturday = lastSunday.AddDays(6);

            // Query to get shifts from last Sunday to next Saturday
            var shifts = await dbContext.ShiftDays
                .Include(s => s.Shift)
                .Where(s => s.DayOfWeek >= lastSunday && s.DayOfWeek <= nextSaturday)
                .ToListAsync() ?? new List<ShiftDays>();*/

            return View(/*shifts*/);
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

            var company = await dbContext.Companies.Include(c => c.Buildings).Where(c => c.Id.ToString() ==  companyId).FirstAsync();

            var teams = await dbContext.WorkersTeams.Include(t => t.TeamMemberships).Where(t => t.CompanyId.ToString() == companyId).ToListAsync();
            var workers = await dbContext.WorkerProfiles.Include(t => t.ApplicationUser).Where(t => t.CompanyId.ToString() == companyId).ToListAsync();

            CreateShiftViewModel model = new CreateShiftViewModel()
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
            };

            foreach (var t in teams) 
            {
                TeamInCreateShiftViewModel tcsvm = new TeamInCreateShiftViewModel()
                {
                    Id = t.Id,
                    Name = t.Name,
                    isSelected = false
                };
                model.Teams.Add(tcsvm);
            }
            foreach(var w in workers)
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
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var shift = new Shift
            {
                Name = model.Name,
                Description = model.Description,
                Starts = model.Starts,
                Ends = model.Ends,
                IsOvernight = model.IsOvernight,  
                BuildingId = model.BuildingId,    
            };

           
            List<ShiftDays> shiftDaysList = new List<ShiftDays>();

            // Check if the shift is repetitive
            if (model.IsRepetitive)
            {
                // Loop through each selected day in the Days list 
                foreach (var day in model.Days)
                {
                    // Convert the string day (e.g., "Monday") to a DayOfWeek enum
                    var selectedDay = Enum.Parse<DayOfWeek>(day);
                    DateTime currentDate = DateTime.Now; // Start from today's date

                    // Loop through each day up to the specified EndDate in the view model
                    while (model.StartDate.ToDateTime(TimeOnly.MinValue) <= model.EndDate.ToDateTime(TimeOnly.MinValue))
                    {
                        // Check if the current date matches the selected day of the week
                        if (currentDate.DayOfWeek == selectedDay)
                        {
                            // Calculate the start date and time by combining the current date and shift start time
                            var startDateTime = currentDate.Add(model.Starts.ToTimeSpan());

                            // Calculate the end date and time:
                            // If IsOvernight is true, the end time should be on the following day
                            var endDateTime = model.IsOvernight
                                ? startDateTime.Date.AddDays(1).Add(model.Ends.ToTimeSpan())
                                : startDateTime.Date.Add(model.Ends.ToTimeSpan());

                            // Add a new ShiftDays instance for each occurrence
                            shiftDaysList.Add(new ShiftDays
                            {
                                ShiftId = shift.Id,    // Associate with the Shift
                                StartDateTime = startDateTime,
                                EndDateTime = endDateTime
                            });
                        }
                        // Move to the next day in the date range
                        currentDate = currentDate.AddDays(1);
                    }
                }
            }
            else
            {
                // If the shift is not repetitive, add a single ShiftDays entry

                // Calculate the start date and time by combining the specified EndDate and shift start time
                var startDateTime = model.EndDate.ToDateTime(model.Starts);

                // Calculate the end date and time:
                // If IsOvernight is true, the end time should be on the following day
                var endDateTime = model.IsOvernight
                    ? startDateTime.Date.AddDays(1).Add(model.Ends.ToTimeSpan())
                    : startDateTime.Date.Add(model.Ends.ToTimeSpan());

                // Add a single ShiftDays instance for the non-repetitive shift
                shiftDaysList.Add(new ShiftDays
                {
                    Id = Guid.NewGuid(),
                    ShiftId = shift.Id,    // Associate with the Shift
                    StartDateTime = startDateTime,
                    EndDateTime = endDateTime
                });
            }

            // Assign the list of ShiftDays to the Shift's ShiftDays collection
            shift.ShiftDays = shiftDaysList;

            // Add the new shift to the database context
            dbContext.Shifts.Add(shift);

            // Save the changes asynchronously
            await dbContext.SaveChangesAsync();

            // Redirect to the index page after successfully creating the shift
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


        [HttpPost]
        public async Task<IActionResult> UpdateTeams([FromBody] UpdateTablesRequestViewModel request)
        {
            var selectedWorkers = request.SelectedWorkers;
            var selectedTeams = request.SelectedTeams;

            foreach (var team in selectedTeams)
            {
                var teamWorkers = await dbContext.WorkersTeams
                    .Include(t => t.TeamMemberships)
                    .Where(t => t.Id == team.Id)
                    .SelectMany(t => t.TeamMemberships)
                    .ToListAsync();

                team.isSelected = false;
                int selectedWorkersCount= 0;

                foreach(var tw in teamWorkers)
                {
                    if(selectedWorkers.Any(sw => sw.Id == tw.WorkerId && sw.isSelected))
                    {
                        selectedWorkersCount++;
                    }
                }

                if(selectedWorkersCount == teamWorkers.Count())
                {
                    team.isSelected = true;
                }
            }

            // Return the updated partial view for teams
            return PartialView("_TeamsTable", selectedTeams);
        }

    }
}
