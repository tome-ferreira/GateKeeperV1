using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using GateKeeperV1.ViewModels;

namespace GateKeeperV1.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;

        public CompanyController(ApplicationDbContext dbContext, IFunctions functions)
        {
            this.dbContext = dbContext;
            this.functions = functions;
        }

        // Method to generate a random salt
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[32]; // 256 bits
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Method to hash the password with the salt
        private byte[] HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password).Concat(salt).ToArray());
            }
        }



        //View to create a project
        [HttpGet]
        public IActionResult RegistCompany()
        {
            return View();
        }
        //After recibing the project info, redirect to checkout
        [HttpPost]
        public IActionResult RegistCompany(RegistCompanyViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.Plan == "Enterprise")
                {
                    return RedirectToAction("CostumizePlan", "Project", new { model = model });
                }
                if(model.Plan == "Free")
                {

                }

                return RedirectToAction("Checkout", "Project", new {model = model});
            }

            return View(model);
        }
        //Checkout recibes the info from the StartProject and opens the view
        [HttpGet]
        public IActionResult Checkout(RegistCompanyViewModel model) 
        {
            if(ModelState.IsValid)
            {
                return View(model);
            }
            return View("Error");
        }
        //Page to confrim that the project was created
        [HttpGet]
        public IActionResult CompanyReady()
        {
            return View();
        }
        //Page for the user costumise what he needs after selecting the enterprise plan
        [HttpGet]
        public IActionResult CostumizePlan()
        {
            return View();
        }
    }
}
