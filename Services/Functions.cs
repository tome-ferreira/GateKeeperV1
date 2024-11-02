using Microsoft.EntityFrameworkCore;
using GateKeeperV1.Data;
using GateKeeperV1.Models;
using System.ComponentModel.Design;

namespace GateKeeperV1.Services
{
    public interface IFunctions
    {
        //Task<List<Company>> GetUserCompanys(string userdId);
        Task<int> GenerateInternalNumber(Guid CompanyId);
    }

    public class Functions : IFunctions
    {
        private readonly ApplicationDbContext dbContext;

        public Functions(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        //Gets all the projects of wich a user is part of
        /*
        public async Task<List<Company>> GetUserCompanys(string userId)
        {
            // Fetch CompanyIds from different roles
            var companysWhereAdmin = await dbContext.CompanyAdmins.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();
            var companysWhereManager = await dbContext.CompanyManagers.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();
            var companysWhereSupervisor = await dbContext.CompanySupervisors.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();
            var companysWhereWorker = await dbContext.ComapnyWorkers.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();

            // Combine all company IDs and remove duplicates using Union
            var allCompanysIds = companysWhereAdmin
                .Union(companysWhereManager)
                .Union(companysWhereSupervisor)
                .Union(companysWhereWorker)
                .ToList();

            // Fetch the distinct companies from the database
            var allCompanies = await dbContext.Companies
                .Where(p => allCompanysIds.Contains(p.Id))
                .ToListAsync();

            return allCompanies;
        }*/

        public async Task<int> GenerateInternalNumber(Guid CompanyId)
        {
            var maxInternalNumber = await dbContext.WorkerProfiles
                .Where(w => w.CompanyId == CompanyId)
                .MaxAsync(w => (int?)w.InternalNumber) ?? 0;

            return maxInternalNumber + 1;
        }
    }
}
