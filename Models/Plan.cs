namespace GateKeeperV1.Models
{
    public class Plan
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public int BuildingsN { get; set; }
        public int RegistsPerMonth { get; set; }
        public int Workers { get; set; }
        public int DashboardAccounts { get; set; }
        public bool HasExcel { get; set; }
        public decimal MonthlyPrice { get; set; }
        public decimal AnualPrice { get; set; }
        public bool CanBeDeleted { get; set; }

        //Navigation property
        public ICollection<Company> Companies { get; set; } 

        // Constructor
        public Plan(string name, int buildingsN, int registsPerMonth, int workers, int dashboardAccounts, bool hasExcel, decimal monthlyPrice, decimal anualPrice)
        {
            Id = Guid.NewGuid(); 
            CreationDate = DateTime.Now;
            Name = name;
            BuildingsN = buildingsN;
            RegistsPerMonth = registsPerMonth;
            Workers = workers;
            DashboardAccounts = dashboardAccounts;
            HasExcel = hasExcel;
            MonthlyPrice = monthlyPrice;
            AnualPrice = anualPrice;
            CanBeDeleted = true;
        }
    }
}
