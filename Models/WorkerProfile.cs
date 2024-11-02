using GateKeeperV1.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace GateKeeperV1.Models
{
    public class WorkerProfile
    {
        public Guid Id { get; set; }
        public int InternalNumber { get; set; }
        public Guid CompanyId { get; set; }
        public string? Notes { get; set; }
        public string Role { get; set; } = string.Empty;

        // One-to-one relationship with ApplicationUser
        public string ApplicationUserId { get; set; }    // Foreign key for ApplicationUser
        public ApplicationUser ApplicationUser { get; set; }  // Navigation property

        // Navigation property
        public Company Company { get; set; }  // Each worker belongs to one company
        public ICollection<Movement> Movements { get; set; } = new List<Movement>();
        // Many-to-many relationship with WorkerShift (join table)
        public ICollection<WorkerShift> WorkerShifts { get; set; } = new List<WorkerShift>();
        public ICollection<AbsenceJustification> AbsenceJustifications { get; set; } = new List<AbsenceJustification>();
        public ICollection<OffdayVacationRequest> OffdayVacationRequests { get; set; } = new List<OffdayVacationRequest>();


        public WorkerProfile() { }

        public WorkerProfile(int internalNumber, Guid companyId, string role, string id)
        {
            InternalNumber = internalNumber;
            CompanyId = companyId;
            Role = role;
            ApplicationUserId = id;
        }
    }
}
