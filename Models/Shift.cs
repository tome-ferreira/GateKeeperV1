namespace GateKeeperV1.Models
{
    public class Shift
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TimeOnly Starts { get; set; }
        public TimeOnly Ends { get; set; }
        public bool IsOvernight { get; set; }  // Indicates if the shift spans two days

        // Foreign Key
        public Guid BuildingId { get; set; }

        // Navigation properties
        public Building Building { get; set; }    // A shift belongs to one building
        public ICollection<WorkerShift> WorkerShifts { get; set; } = new List<WorkerShift>();
        public ICollection<ShiftDays> ShiftDays { get; set; } = new List<ShiftDays>();
    }
}
