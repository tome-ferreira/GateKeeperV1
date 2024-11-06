namespace GateKeeperV1.ViewModels
{
    public class CheckForProblemsViewModel
    {
        public bool Problems { get; set; }
        public string Error { get; set; }

        public CheckForProblemsViewModel(bool problems, string error)
        {
            Problems = problems;
            Error = error;
        }
    }
}
