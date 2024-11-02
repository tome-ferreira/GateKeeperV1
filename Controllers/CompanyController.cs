using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using GateKeeperV1.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using System.Numerics;
using Microsoft.AspNetCore.Hosting;

namespace GateKeeperV1.Controllers
{
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CompanyController(ApplicationDbContext dbContext, IFunctions functions, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.functions = functions;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
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

        private async Task<WorkerProfile> CreateAdminProfile()
        {
            var user = await userManager.GetUserAsync(User);
            var userEmail = user.Email;
            var userId = user.Id;

            var company = JsonConvert.DeserializeObject<Company>(TempData["FreeCompany"].ToString());

            await dbContext.Companies.AddAsync(company);

            int internalNumber = await functions.GenerateInternalNumber(company.Id);

            WorkerProfile workerProfile = new WorkerProfile(internalNumber, company.Id, "Admin", userEmail);

            return workerProfile;
        }



        //View to regist a company
        [HttpGet]
        public IActionResult RegistCompany()
        {
            return View();
        }
        //Post for creating a company
        [HttpPost]
        public async Task<IActionResult> RegistCompany(RegistCompanyViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Plan == "Enterprise")
                {
                    TempData["RegistCompanyModel"] = JsonConvert.SerializeObject(model);
                    return RedirectToAction("CostumizePlan", "Company");
                }


                if (model.Plan == "Free")
                {
                    // Generate a random salt
                    byte[] salt = GenerateSalt();

                    // Hash the password with the salt
                    byte[] hashedPassword = HashPassword(model.Password, salt);

                    Company company = new Company(model.Name, model.Description, Convert.ToBase64String(hashedPassword), Convert.ToBase64String(salt),
                        DateTime.Now.AddYears(99), 1, 500, 5, 1, false, 0.00, 0.00);
                    await dbContext.Companies.AddAsync(company);

                    var user = await userManager.GetUserAsync(User);

                    int internalNumber = await functions.GenerateInternalNumber(company.Id);

                    WorkerProfile workerProfile = new WorkerProfile(internalNumber, company.Id, "Admin", user.Id);

                    await dbContext.WorkerProfiles.AddAsync(workerProfile);

                    await dbContext.SaveChangesAsync();

                    return View("CompanyReady", company.Id);
                }
                else if (model.Plan == "Premium")
                {

                }

            }
            return View("Error");
        }


        
                

                //Save pfp====================================================
                /*
                var companyPath = Path.Combine(webHostEnvironment.WebRootPath, "img/pfps", company.Id.ToString());
                if (!Directory.Exists(companyPath))
                {
                    Directory.CreateDirectory(companyPath);
                }
                if (profileModel.pfp != null)
                {
                    string extension = Path.GetExtension(profileModel.pfp.FileName);
                    var newFileName = $"{userId}{extension}";
                    var newFilePath = Path.Combine(companyPath, newFileName);
                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        profileModel.pfp.CopyTo(stream);
                    }
                }*/
                //============================================================





        //Page for the user costumise what he needs after selecting the enterprise plan
        [HttpGet]
        public IActionResult CostumizePlan()
        {

            // Retrieve and deserialize the model from TempData
            if (TempData["RegistCompanyModel"] != null)
            {
                var model = JsonConvert.DeserializeObject<RegistCompanyViewModel>(TempData["RegistCompanyModel"].ToString());

                EnterpriseRequestViewModel outgoingModel = new EnterpriseRequestViewModel(model);

                return View(outgoingModel);
            }

            return View("Error");
        }
        [HttpPost]
        public async Task<IActionResult> CostumizePlan(EnterpriseRequestViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View("Erro");
        }
    }
}
