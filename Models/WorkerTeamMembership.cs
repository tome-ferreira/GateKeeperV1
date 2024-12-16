namespace GateKeeperV1.Models
{
    public class WorkerTeamMembership
    {
        public Guid WorkerId { get; set; }
        public WorkerProfile Worker { get; set; } = null!;

        public Guid TeamId { get; set; }
        public WorkersTeam Team { get; set; } = null!;
    }
}
