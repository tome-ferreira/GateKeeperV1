namespace GateKeeperV1.Dto
{
    public class ShiftWorkers
    {
        public  string ShiftName { get; set; }
        public Guid ShiftId { get; set; }
        public List<WorkerInOutMovs> WorkerInOutMovs { get; set; }
    }
}
