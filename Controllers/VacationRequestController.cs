using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GateKeeperV1.Controllers
{
    public class VacationRequestController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFunctions functions;

        public VacationRequestController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IFunctions functions)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.functions = functions;
        }

        [HttpGet]
        public async Task<IActionResult> ListVacationRequests()
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }


            var requests = await dbContext.OffdayVacationRequests.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.CompanyId == Guid.Parse(companyId)).ToListAsync();
            return View(requests);
        }

        [HttpGet]
        public async Task<IActionResult> RequestDetails(Guid Id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }


            var requests = await dbContext.OffdayVacationRequests.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.Id == Id).FirstOrDefaultAsync();
            return View(requests);
        }








        public async Task<IActionResult> AproveRequest(Guid Id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var request = await dbContext.OffdayVacationRequests.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.Id == Id).FirstOrDefaultAsync();

            request.Status = "Aproved";

            await dbContext.SaveChangesAsync();

            return RedirectToAction("ListVacationRequests");
        }

        public async Task<IActionResult> RejectRequest(Guid Id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var request = await dbContext.OffdayVacationRequests.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.Id == Id).FirstOrDefaultAsync();

            request.Status = "Recused";

            await dbContext.SaveChangesAsync();

            return RedirectToAction("ListVacationRequests");
        }
    }
}
