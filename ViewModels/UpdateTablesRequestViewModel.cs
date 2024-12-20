namespace GateKeeperV1.ViewModels
{
    public class UpdateTablesRequestViewModel
    {
        public List<WorkerInCreateShiftViewModel> SelectedWorkers { get; set; }
        public List<TeamInCreateShiftViewModel> SelectedTeams { get; set; }
    }
}
