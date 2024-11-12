namespace GateKeeperV1.ViewModels
{
    public class WorkerProfileWithUserViewModel
    {
        public Guid WorkerProfileId { get; set; }
        public int InternalNumber { get; set; }
        public string Role { get; set; }
        public string ApplicationUserId { get; set; }
        public string UserEmail { get; set; }
        public string UserFullName { get; set; }
    }
}
