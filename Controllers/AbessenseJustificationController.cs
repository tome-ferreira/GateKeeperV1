using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GateKeeperV1.Controllers
{
    public class AbessenseJustificationController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFunctions functions;

        public AbessenseJustificationController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IFunctions functions)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.functions = functions;
        }

        [HttpGet]
        public async  Task<IActionResult> ListAbessenceJustifications()
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }


            var justifications = await dbContext.AbsenceJustifications.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.CompanyId == Guid.Parse(companyId)).ToListAsync();
            return View(justifications);
        }


        [HttpGet]
        public async Task<IActionResult> AbssenceDetails(Guid Id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var justification = await dbContext.AbsenceJustifications.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.Id == Id).FirstOrDefaultAsync();

            return View(justification);
        }

        public async Task<IActionResult> AproveJustification(Guid Id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var justification = await dbContext.AbsenceJustifications.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.Id == Id).FirstOrDefaultAsync();

            justification.Status = "Aproved";

            await dbContext.SaveChangesAsync();

            return RedirectToAction("ListAbessenceJustifications");
        }

        public async Task<IActionResult> RejectJustification(Guid Id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var justification = await dbContext.AbsenceJustifications.Include(a => a.Worker).ThenInclude(w => w.ApplicationUser).Where(a => a.Id == Id).FirstOrDefaultAsync();

            justification.Status = "Recused";

            await dbContext.SaveChangesAsync();

            return RedirectToAction("ListAbessenceJustifications");
        }
    }
}
