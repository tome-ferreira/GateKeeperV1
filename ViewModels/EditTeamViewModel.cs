namespace GateKeeperV1.ViewModels
{
    public class EditTeamViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<WorkerInTeamViewModel> Workers { get; set; } = new List<WorkerInTeamViewModel>();
    }
}
