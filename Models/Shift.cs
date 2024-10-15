namespace GateKeeperV1.Models
{
    public class Shift
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Starts { get; set; }
        public DateTime Ends { get; set; }

        // Foreign Key
        public Guid BuildingId { get; set; }

        // Navigation properties
        public Building Building { get; set; }    // A shift belongs to one building
        public ICollection<WorkerShift> WorkerShifts { get; set; } = new List<WorkerShift>();
    }
}
