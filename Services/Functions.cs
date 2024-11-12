using Microsoft.EntityFrameworkCore;
using GateKeeperV1.Data;
using GateKeeperV1.Models;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;
using GateKeeperV1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace GateKeeperV1.Services
{
    public interface IFunctions
    {
        Task<List<Company>> GetUserCompanys(string userdId);
        Task<int> GenerateInternalNumber(Guid CompanyId);
        Task<bool> IsUserInCompanyRole(string userId, string role);
        Task<bool> IsUserInCompanyRoleView(string userId, string role);
        Task<CheckForProblemsViewModel> CheckForProblems(Guid CompanyId);
        Task<string> PrintUserRoleView(string WorkerId);
    }

    public class Functions : IFunctions
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public Functions(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }


        //Gets all the companies of wich a user is part of
        
        public async Task<List<Company>> GetUserCompanys(string userId)
        {
            List<Company> allCompanies = new List<Company>();

            // Get the list of Company IDs for the user
            var companiesIds = await dbContext.WorkerProfiles
                .Where(wp => wp.ApplicationUserId == userId)
                .Select(wp => wp.CompanyId)
                .ToListAsync();

            // Retrieve the Company objects that match the IDs
            allCompanies = await dbContext.Companies
                .Where(c => companiesIds.Contains(c.Id))
                .ToListAsync();

            return allCompanies;
        }

        //Generate an internal number when creating a new worker profile
        public async Task<int> GenerateInternalNumber(Guid CompanyId)
        {
            var maxInternalNumber = await dbContext.WorkerProfiles
                .Where(w => w.CompanyId == CompanyId)
                .MaxAsync(w => (int?)w.InternalNumber) ?? 0;

            return maxInternalNumber + 1;
        }


        //Checks if the user is in the company and in the necessary role. For use in views (does not log)
        public async Task<bool> IsUserInCompanyRoleView(string userId, string role)
        {
            string companyId = httpContextAccessor.HttpContext.Session.GetString("companyId") ?? Guid.Empty.ToString();

            WorkerProfile? WP = await dbContext.WorkerProfiles.Where(w => w.ApplicationUserId == userId)
                .Where(w => w.CompanyId.ToString() == companyId).FirstOrDefaultAsync();
            if (WP != null)
            {
                if (WP.Role == role)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        //Checks if the user is in the company and in the necessary role, logs unauthorized acesses tries
        public async Task<bool> IsUserInCompanyRole(string userId, string role)
        {
            string companyId = httpContextAccessor.HttpContext.Session.GetString("companyId") ?? Guid.Empty.ToString();

            WorkerProfile? WP = await dbContext.WorkerProfiles.Where(w => w.ApplicationUserId == userId)
                .Where(w => w.CompanyId.ToString() ==  companyId).FirstOrDefaultAsync();
            if(WP != null)
            {
                if(WP.Role == role)
                {
                    return true;
                }
                else
                {
                    //Log unauthorized acess from a person from the company
                    return false;
                }
            }

            //log unauthorized acess from a person outside of the company
            return false;
        }

        //Check if project is able to initialize
        public async Task<CheckForProblemsViewModel> CheckForProblems(Guid CompanyId)
        {
            Company company = await dbContext.Companies
                                      .Include(c => c.Buildings) // Load Buildings collection
                                      .FirstOrDefaultAsync(c => c.Id == CompanyId);

            if (company == null)
            {
                return new CheckForProblemsViewModel(true, "CompanyNotFound");
            }
            if(company.ValidUntil < DateTime.Now)
            {
                return new CheckForProblemsViewModel(true, "CompanyError"); //Generic name so that workers get an unknow error but administrators know to renovate the license
            }
            if(company.Buildings.Count == null || !company.Buildings.Any())
            {
                return new CheckForProblemsViewModel(true, "BuildingError");
            }
            return new CheckForProblemsViewModel(false, "Ok");
        }
        
        //Print the user's role in view
        public async Task<string> PrintUserRoleView(string WorkerId)
        {
            string companyId = httpContextAccessor.HttpContext.Session.GetString("companyId") ?? Guid.Empty.ToString();
            string userRole = await dbContext.WorkerProfiles.Where(w => w.ApplicationUserId == WorkerId.ToString()).Where(w => w.CompanyId.ToString() == companyId)
                .Select(w => w.Role).FirstOrDefaultAsync();
            if (userRole.IsNullOrEmpty())
            {
                return userRole;
            }
            return "";
        }
    }
}
