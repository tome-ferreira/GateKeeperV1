using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GateKeeperV1.Data;
using GateKeeperV1.ViewModels;
using GateKeeperV1.Models;
using AutoMapper;

namespace GateKeeperV1.Controllers
{
    [Authorize(Roles = "Staff, StaffManager")]
    public class PlanController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;

        public PlanController(ApplicationDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var model = await dbContext.Plans.ToListAsync();

            return View(model);
        }

        [HttpGet]
        public IActionResult CreatePlan() 
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan(CreatePlanViewModel model)
        {
            // Check if a plan with the same name already exists
            bool planExists = await dbContext.Plans.AnyAsync(p => p.Name == model.Name);

            if (planExists)
            {
                ModelState.AddModelError("Name", "A plan with this name already exists. Please choose a different name.");
            }

            if (ModelState.IsValid)
            {
                if(model.buildingsNUnlimited == true)
                {
                    model.BuildingsN = 0;
                }
                if(model.RegistsPerMonthUnlimited == true)
                {
                    model.RegistsPerMonth = 0;
                }
                if (model.WorkersUnlimited == true)
                {
                    model.Workers = 0;
                }
                if(model.DashboardAccountsUnlimited == true)
                {
                    model.DashboardAccounts = 0;
                }

                var plan = mapper.Map<Plan>(model);

                await dbContext.Plans.AddAsync(plan);
                await dbContext.SaveChangesAsync();
                
                return RedirectToAction("PlanDetails", "Plan", new { id = plan.Id });
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> PlanDetails(Guid id)
        {
            var plan = await dbContext.Plans.FindAsync(id);

            return View(plan);
        }

        public async Task<IActionResult> DeletePlan(Guid id)
        {
            var plan = await dbContext.Plans.FindAsync(id);

            if(plan != null)
            {
                if(plan.CanBeDeleted == false)
                {
                    return RedirectToAction("AccessDenied");
                }


                dbContext.Plans.Remove(plan);
                await dbContext.SaveChangesAsync();

                //TO DO: Put the old return inside of this and outside resdirect to PlanNotFound
            }

            return RedirectToAction("Index", "Plan");
        }
    }
}
