using Microsoft.AspNetCore.Identity;

namespace GateKeeperV1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name {  get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        //Navigation properties
        public ICollection<CompanyAdmins> CompanyAdmins { get; set; }
        public ICollection<CompanyManagers> CompanyManagers { get; set; }
        public ICollection<CompanySupervisors> CompanySupervisors { get; set; }
        public ICollection<CompanyWorkers> ComapanyWorkers { get; set; }
        public ICollection<EnterpirseRequest> EnterpirseRequests { get; set; }
        public WorkerProfile WorkerProfile { get; set; }   
    }
}
