namespace GateKeeperV1.ViewModels
{
    public class CreateWorkerViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Notes { get; set; }
        public string Role { get; set; }
        public bool Continue { get; set; }
    }
}
