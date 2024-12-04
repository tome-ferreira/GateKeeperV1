namespace GateKeeperV1.Models
{
    public class ShiftDays
    {
        public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }  // Specific start time of this occurrence
        public DateTime EndDateTime { get; set; }    // Specific end time, allowing shifts that span two days


        // Foreign Key
        public Guid ShiftId { get; set; }

        // Navigation property
        public Shift Shift { get; set; }    // Each ShiftDay belongs to one Shift
    }
}
