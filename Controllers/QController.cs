using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GateKeeperV1.Controllers
{
    public class QController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;
        private readonly UserManager<ApplicationUser> userManager;

        public QController(ApplicationDbContext dbContext, IFunctions functions, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.functions = functions;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index(string WId)
        {
            HttpContext.Session.Remove("companyId");

            var worker = await dbContext.WorkerProfiles.Where(c => c.Id.ToString() == WId).FirstAsync();
            if (worker != null)
            {
                string CId = worker.CompanyId.ToString();

                HttpContext.Session.SetString("companyId", CId);

                var user = await userManager.GetUserAsync(User);
                var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            

            
                return RedirectToAction("WorkerDetails", "Worker", worker.Id);
            }

            return View();
        }
    }
}
