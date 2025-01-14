using GateKeeperV1.Data;
using GateKeeperV1.Dto;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace GateKeeperV1.Controllers
{
    public class MovementController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;
        private readonly UserManager<ApplicationUser> userManager;

        public MovementController(ApplicationDbContext dbContext, IFunctions functions, UserManager<ApplicationUser> userManager)
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

            var dates = await dbContext.Movements
                .Include(m => m.Shift)
                .Where(m => m.Shift.CompanyId == Guid.Parse(companyId))
                .Select(m => m.DateTime.Date) 
                .Distinct() 
                .OrderByDescending(d => d) 
                .ToListAsync();

            return View(dates);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetMovementsTablePartial(DateTime date)
        {
            if(date == DateTime.MinValue)
            {
                 date = DateTime.Now.Date;

            }

            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            int dayOfWeek = date.DayOfWeek == DayOfWeek.Sunday ? 1 : (int)date.DayOfWeek + 1;

            var normalizedDate = DateTime.ParseExact(date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var shifts = await dbContext.Shifts
                .Include(s => s.Movements)
                .Include(s => s.WorkerShifts)
                    .ThenInclude(ws => ws.Worker)
                        .ThenInclude(w => w.ApplicationUser)
                .Where(s => s.CompanyId == Guid.Parse(companyId)).ToListAsync();

            List<Shift> shiftList = new List<Shift>();  
            

            foreach (var shift in shifts)
            {
                Shift sh = new Shift();

                sh = shift;

                sh.Movements = shift.Movements
                    .Where(m => DateOnly.FromDateTime(m.DateTime.Date).ToString("MM/dd/yyyy") == normalizedDate.ToString("dd/MM/yyyy"))
                    /*.Where(m => m.DateTime.Date == normalizedDate.Date)*/
                    .ToList();

                if(sh.Movements.Count > 0)
                {
                    shiftList.Add(sh);
                }
            }

            List<ShiftWorkers> model = new List<ShiftWorkers>();

            foreach (var shift in shiftList)
            {
                ShiftWorkers shiftWorkers = new ShiftWorkers()
                {
                    ShiftName = shift.Name,
                    ShiftId = shift.Id,
                    WorkerInOutMovs = new List<WorkerInOutMovs>()
                };

                var WorkerShifts = shift.WorkerShifts.ToList();

                foreach (var ws in WorkerShifts)
                {
                    var worker = ws.Worker;

                    WorkerInOutMovs workerInOutMovs = new WorkerInOutMovs()
                    {
                        WorkerName = worker.ApplicationUser.Name + " " + worker.ApplicationUser.Surname,
                        WorkerNumber = worker.InternalNumber,
                        WorkerId = worker.Id,
                        EntranceExitGroups = new List<EntranceExitGroup>()
                    };

                    
                    var workerMovements = shift.Movements
                        .Where(m => m.WorkerId == worker.Id)
                        .OrderBy(m => m.DateTime)
                        .ToList();

                    // Iterate through movements and form EntranceExitGroups
                    for (int i = 0; i < workerMovements.Count; i++)
                    {
                        if (i == workerMovements.Count - 1)
                        {
                            // If it's the last movement and it's an entrance, handle the "Erro" case
                            if (workerMovements[i].isEntrance)
                            {
                                workerInOutMovs.EntranceExitGroups.Add(new EntranceExitGroup
                                {
                                    Entrance = workerMovements[i].DateTime.ToString("HH:mm"),
                                    Exit = "Erro"
                                });
                            }
                            break;
                        }

                        var currentMovement = workerMovements[i];
                        var nextMovement = workerMovements[i + 1];

                        if (currentMovement.isEntrance && !nextMovement.isEntrance)
                        {
                            workerInOutMovs.EntranceExitGroups.Add(new EntranceExitGroup
                            {
                                Entrance = currentMovement.DateTime.ToString("HH:mm"),
                                Exit = nextMovement.DateTime.ToString("HH:mm")
                            });
                            i++; // Skip the next movement since it's already paired
                        }
                        else if (!currentMovement.isEntrance)
                        {
                            workerInOutMovs.EntranceExitGroups.Add(new EntranceExitGroup
                            {
                                Entrance = "Erro",
                                Exit = currentMovement.DateTime.ToString("HH:mm")
                            });
                        }
                    }

                    if(workerMovements.Count > 0)
                    {
                        shiftWorkers.WorkerInOutMovs.Add(workerInOutMovs);
                    }
                }
                model.Add(shiftWorkers);
            }

            return PartialView("_MovementsTablePartial", model);

        }
        








        /*

        [HttpGet]
        public async Task<IActionResult> GetMovementsTablePartial(DateTime date)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var movements = await dbContext.Movements
                .Include(m => m.Shift)
                .Where(m => m.Shift.CompanyId == Guid.Parse(companyId))
                .ToListAsync();

            DateOnly dateOnly = DateOnly.FromDateTime(date);

            List<Movement> movementsList = new List<Movement>();

            foreach(var m in movements)
            {
                string dataShift = DateOnly.FromDateTime(m.DateTime.Date).ToString("MM/dd/yyyy");
                string dataData = dateOnly.ToString("dd/MM/yyyy");

                if (dataShift == dataData)
                {
                    movementsList.Add(m);
                }
            }

            return PartialView("_MovementsTablePartial", movementsList);

        }
        */
    }
}
