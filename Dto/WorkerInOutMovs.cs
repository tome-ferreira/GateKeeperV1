namespace GateKeeperV1.Dto
{
    public class WorkerInOutMovs
    {
        public string WorkerName { get; set; }
        public int WorkerNumber { get; set; }
        public Guid WorkerId { get; set; }
        public List<EntranceExitGroup> EntranceExitGroups { get; set; }
    }
}
