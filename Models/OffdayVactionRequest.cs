namespace GateKeeperV1.Models
{
    public class OffdayVacationRequest
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }
        public Guid WorkerId { get; set; }
        public DateTime Starts { get; set; }
        public DateTime Ends { get; set; }
        public string? Notes { get; set; }

        // Navigation properties
        public Company Company { get; set; }      // Each OffdayVacationRequest belongs to one Company
        public WorkerProfile Worker { get; set; } // Each OffdayVacationRequest belongs to one Worker
    }
}
