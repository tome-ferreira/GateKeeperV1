namespace GateKeeperV1.ViewModels
{
    public class WorkerInTeamViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string Role {  get; set; }
        public string Email { get; set; }
        public bool isSelected { get; set; }
    }
}
