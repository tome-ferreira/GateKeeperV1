using GateKeeperV1.Models;

namespace GateKeeperV1.ViewModels
{
    public class CreateTeamViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<WorkerInTeamViewModel> Workers { get; set; }
    }
}
