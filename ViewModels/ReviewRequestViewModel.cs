namespace GateKeeperV1.ViewModels
{
    public class ReviewRequestViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Salt {  get; set; } = string.Empty;
        public string creatorEmail { get; set; } = string.Empty;
        public int BuildingsN { get; set; }
        public bool buildingsNUnlimited { get; set; }

        public int RegistsPerMonth { get; set; }
        public bool RegistsPerMonthUnlimited { get; set; }

        public int Workers { get; set; }
        public bool WorkersUnlimited { get; set; }

        public int DashboardAccounts { get; set; }
        public bool DashboardAccountsUnlimited { get; set; }

        public bool HasExcel { get; set; }
    }
}
