﻿using Microsoft.AspNetCore.Authorization;
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



        //View to regist a company
        [HttpGet]
        public IActionResult RegistCompany()
        {
            return View();
        }
        //After recibing the company info, redirect to checkout
        [HttpPost]
        public IActionResult RegistCompany(RegistCompanyViewModel model)
        {
            if(ModelState.IsValid)
            {
                if(model.Plan == "Enterprise")
                {
                    return RedirectToAction("CostumizePlan", "Company", new { model = model });
                }
                if(model.Plan == "Free")
                {
                    // Generate a random salt
                    byte[] salt = GenerateSalt();

                    // Hash the password with the salt
                    byte[] hashedPassword = HashPassword(model.Password, salt);

                    Company company = new Company(model.Name, model.Description, Convert.ToBase64String(hashedPassword), Convert.ToBase64String(salt), DateTime.Now.AddYears(1000));

                    return RedirectToAction("CompanyReady", "Company", new { model = model });
                }

                return RedirectToAction("Checkout", "Company", new {model = model});
            }

            return View(model);
        }
        //Checkout recibes the info from the RegistCompany and opens the view
        [HttpGet]
        public IActionResult Checkout(RegistCompanyViewModel model) 
        {
            if(ModelState.IsValid)
            {
                CheckoutViewModel outgoingModel = new CheckoutViewModel(model);

                return View(model);
            }
            return View("Error");
        }
        //Action to create company after checkout
        public async Task<IActionResult> CreateCompany(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Generate a random salt
                byte[] salt = GenerateSalt();

                // Hash the password with the salt
                byte[] hashedPassword = HashPassword(model.company.Password, salt);

                Company company = new Company(model.company.Name, model.company.Description, Convert.ToBase64String(hashedPassword), Convert.ToBase64String(salt), model.validUntil);

                return RedirectToAction("CompanyReady", "Company", new { model = model });
            }
            return View("Error");
        }


        //Page to confrim that the company was registed
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