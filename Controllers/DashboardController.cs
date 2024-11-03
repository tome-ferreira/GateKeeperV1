﻿using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GateKeeperV1.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public DashboardController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? CompanyId)
        {
            // Retrieve or set the company ID in the session
            string companyId = HttpContext.Session.GetString("companyId") ?? CompanyId.ToString();
            HttpContext.Session.SetString("companyId", companyId);

            // Attempt to retrieve the company
            Company company = await dbContext.Companies.FindAsync(Guid.Parse(companyId));

            DashboardViewModel model = new DashboardViewModel(company);

            return View(model);
        }
    }
}