namespace GateKeeperV1.ViewModels
{
    public class EnterpriseRequestViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int BuildingsN { get; set; }
        public bool BuildingsNUnlimited { get; set; }

        public int RegistsPerMonth { get; set; }
        public bool RegistsPerMonthUnlimited { get; set; }

        public int Workers { get; set; }
        public bool WorkersUnlimited { get; set; }

        public int DashboardAccounts { get; set; }
        public bool DashboardAccountsUnlimited { get; set; }

        public bool HasExcel { get; set; }
        public DateTime CreationDate { get; set; }

        //Bob o construtor
        public EnterpriseRequestViewModel(RegistCompanyViewModel registCompanyViewModel)
        {
            Name = registCompanyViewModel.Name;
            Description = registCompanyViewModel.Description;
            Password = registCompanyViewModel.Password;
        }
    }
}
