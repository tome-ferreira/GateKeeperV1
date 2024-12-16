namespace GateKeeperV1.Models
{
    public class WorkersTeam
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid CompanyId { get; set; } 

        public ICollection<WorkerTeamMembership> TeamMemberships { get; set; } = new List<WorkerTeamMembership>();
        public Company Company { get; set; } = null!;

    }
}
