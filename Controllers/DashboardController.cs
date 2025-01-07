using GateKeeperV1.Data;
using GateKeeperV1.Dto;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using GateKeeperV1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GateKeeperV1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;
        private readonly UserManager<ApplicationUser> userManager;

        public DashboardController(ApplicationDbContext dbContext, IFunctions functions, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.functions = functions;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index(Guid? CompanyId)
        {
            // Retrieve or set the company ID in the session
            string companyId = HttpContext.Session.GetString("companyId") ?? CompanyId.ToString();
            HttpContext.Session.SetString("companyId", companyId);

            // Attempt to retrieve the company
            Company company = await dbContext.Companies.FindAsync(Guid.Parse(companyId));
            
            //Checks for erros that could make the comoany profile unfit to being acessed 
            var result = await functions.CheckForProblems(Guid.Parse(companyId)); if (result.Problems){return RedirectToAction(result.Error, "CompanyError");}

            

            var user = await userManager.GetUserAsync(User);
            var worker = await functions.IsUserInCompanyRole(user.Id, "Worker");

            if (worker) 
            {
                return View("Worker");
            }

            DashboardViewModel model = new DashboardViewModel(company);

            var today = DateTime.Now;

            var movements = await dbContext.Movements
                .Include(m => m.Shift)
                .Where(m => m.Shift.CompanyId == Guid.Parse(companyId))
                .Where(m => m.DateTime.Date == today.Date).ToListAsync();

            model.EntriesToday = movements
                .Where(m => m.isEntrance)
                .Count();

            model.ExitsToday = movements
                .Where(m => !m.isEntrance)
                .Count();

            model.NewstMovements = await dbContext.Movements
                .Include(m => m.Shift)
                .Where(m => m.Shift.CompanyId == Guid.Parse(companyId))
                .Where(m => m.DateTime.Date == today.Date)
                .OrderByDescending(m => m.DateTime) 
                .Take(3)                           
                .ToListAsync();

            var shifts = movements.Select(m => m.Shift).ToList().Distinct();

            List<ShiftWorkerCount> swcs = new List<ShiftWorkerCount>();
            List<DashboardTableRow> dtrs = new List<DashboardTableRow> ();

            foreach (var shift in shifts) 
            {
                ShiftWorkerCount swc = new ShiftWorkerCount();

                swc.name = shift.Name;
                swc.value = movements.Where(m => m.Shift == shift).Count();

                swcs.Add(swc); 
                
                DashboardTableRow dtr = new DashboardTableRow();

                dtr.Shift = shift.Name;
                dtr.Entries = movements.Where(m => m.Shift == shift).Where(m => m.isEntrance).Count();
                dtr.Exits = movements.Where(m => m.Shift == shift).Where(m => !m.isEntrance).Count();
                if(dtr.Entries - dtr.Exits >= 0)
                {
                    dtr.WorkingNow = dtr.Entries - dtr.Exits;
                }
                else
                {
                    dtr.WorkingNow = 0;
                }

                dtrs.Add(dtr);
            }

            model.dashboardTableRows = dtrs;
            model.WorkersPerShift = swcs;

            return View(model);
        }
    }
}
