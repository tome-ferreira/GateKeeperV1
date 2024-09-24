using Microsoft.EntityFrameworkCore;
using GateKeeperV1.Data;
using GateKeeperV1.Models;

namespace GateKeeperV1.Services
{
    public interface IFunctions
    {
        Task<List<Company>> GetUserCompanys(string userdId);
    }

    public class Functions : IFunctions
    {
        private readonly ApplicationDbContext dbContext;

        public Functions(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        //Gets all the projects of wich a user is part of
        public async Task<List<Company>> GetUserCompanys(string userId)
        {
            // Fetch ProjectIds from different roles
            var companysWhereAdmin = await dbContext.CompanyAdmins.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();
            var companysWhereManager = await dbContext.CompanyManagers.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();
            var companysWhereSupervisor = await dbContext.CompanySupervisors.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();
            var companysWhereWorker = await dbContext.ComapnyWorkers.Where(pa => pa.UserId == userId).Select(pa => pa.CompanyId).ToListAsync();

            // Combine all project IDs and remove duplicates using Union
            var allCompanysIds = companysWhereAdmin
                .Union(companysWhereManager)
                .Union(companysWhereSupervisor)
                .Union(companysWhereWorker)
                .ToList();

            // Fetch the distinct projects from the database
            var allProjects = await dbContext.Projects
                .Where(p => allCompanysIds.Contains(p.Id))
                .ToListAsync();

            return allProjects;
        }
    }
}
