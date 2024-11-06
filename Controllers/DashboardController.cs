using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using GateKeeperV1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace GateKeeperV1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;

        public DashboardController(ApplicationDbContext dbContext, IFunctions functions)
        {
            this.dbContext = dbContext;
            this.functions = functions;
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

            DashboardViewModel model = new DashboardViewModel(company);

            return View(model);
        }
    }
}
