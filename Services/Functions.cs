using Microsoft.EntityFrameworkCore;
using GateKeeperV1.Data;
using GateKeeperV1.Models;
using System.ComponentModel.Design;
using Microsoft.AspNetCore.Mvc;

namespace GateKeeperV1.Services
{
    public interface IFunctions
    {
        Task<List<Company>> GetUserCompanys(string userdId);
        Task<int> GenerateInternalNumber(Guid CompanyId);
        Task<bool> IsUserInCompanyRole(string userId, Guid companyId, string role);
    }

    public class Functions : IFunctions
    {
        private readonly ApplicationDbContext dbContext;

        public Functions(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        //Checks if the user is in the company and in the necessary role, logs unauthorized acesses tries
        public async Task<bool> IsUserInCompanyRole(string userId, Guid companyId, string role)
        {
            WorkerProfile? WP = await dbContext.WorkerProfiles.Where(w => w.ApplicationUserId == userId)
                .Where(w => w.CompanyId ==  companyId).FirstOrDefaultAsync();
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
        /*public async Task<IActionResult> CheckCompanyRole(Guid CompanyId)
        {
            Company company = await dbContext.Companies.FindAsync(CompanyId);

            if(company == null)
            {
                return 
            }
        }*/
    }
}
