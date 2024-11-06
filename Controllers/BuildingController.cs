using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using GateKeeperV1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.Security.Policy;

namespace GateKeeperV1.Controllers
{
    public class BuildingController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFunctions functions;
        private readonly ApplicationDbContext dbContext;

        public BuildingController(UserManager<ApplicationUser> userManager, IFunctions functions, ApplicationDbContext dbContext)
        {
            this.userManager = userManager;
            this.functions = functions;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public  async Task<IActionResult> CreateBuilding()
        {
            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess){return View("AcessDenied");}

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBuilding(CreateBuildingViewModel model)
        {
            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            if (ModelState.IsValid)
            {
                string? companyId = HttpContext.Session.GetString("companyId");
                if (string.IsNullOrEmpty(companyId))
                {
                    return View("Error");
                }
                Building building = new Building(model.Name, model.Description, Guid.Parse(companyId));

                await dbContext.Buildings.AddAsync(building);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("BuildingDetails", "Building", new { buildingId = building.Id });
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> BuildingDetails(Guid buildingId)
        {
            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var building = await dbContext.Buildings.FindAsync(buildingId);

            if (building != null)
            {
                return View(building);
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> EditBuilding (Guid BuildingId)
        {
            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var building = await dbContext.Buildings.FindAsync(BuildingId);

            if (building != null)
            {
                return View(building);
            }
            return View("Error");
        }
        [HttpPost]
        public async Task<IActionResult> EditBuilding(Building building)
        {
            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            
                var buil = await dbContext.Buildings.FindAsync(building.Id);
                
                buil.Name = building.Name;
                buil.Description = building.Description;

                await dbContext.SaveChangesAsync();

                return RedirectToAction("BuildingDetails", "Building", new { buildingId = building.Id });
            
        }

        public async Task<IActionResult> DeleteBuilding(Guid BuildingId)
        {
            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var building = await dbContext.Buildings.FindAsync(BuildingId);

            if (building != null)
            {
                dbContext.Buildings.Remove(building);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("ListBuildings", "Building");
            }
            return View("Error");
        }

        [HttpGet]
        public async Task<IActionResult> ListBuildings()
        {
            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return View("Error");
            }

            // Use FirstOrDefaultAsync instead of FindAsync
            var company = await dbContext.Companies.Include(c => c.Buildings).FirstOrDefaultAsync(c => c.Id.ToString() == companyId);

            return View(company);
        }
    }
}
