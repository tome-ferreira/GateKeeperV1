namespace GateKeeperV1.ViewModels
{
    public class EnterpriseRequestCostumizePlanViewModel
    {
        public int BuildingsN { get; set; }
        public bool BuildingsNUnlimited { get; set; }

        public int RegistsPerMonth { get; set; }
        public bool RegistsPerMonthUnlimited { get; set; }

        public int Workers { get; set; }
        public bool WorkersUnlimited { get; set; }

        public int DashboardAccounts { get; set; }
        public bool DashboardAccountsUnlimited { get; set; }

        public bool HasExcel { get; set; }
    }
}
