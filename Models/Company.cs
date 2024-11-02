namespace GateKeeperV1.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime ValidUntil { get; set; }
        public DateTime CreationDate { get; set; }
        public int BuildingsN { get; set; }
        public int RegistsPerMonth { get; set; }
        public int WorkersN { get; set; }
        public int DashboardAccounts { get; set; }
        public bool HasExcel { get; set; }
        public double MonthlyPrice { get; set; }
        public double AnualPrice { get; set; }

        

        //Navigation properties
        public ICollection<WorkerProfile> Workers { get; set; }
        public ICollection<Building> Buildings { get; set; }
        public ICollection<Movement> Movements { get; set; } = new List<Movement>();
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
        public ICollection<AbsenceJustification> AbsenceJustifications { get; set; } = new List<AbsenceJustification>();
        public ICollection<OffdayVacationRequest> OffdayVacationRequests { get; set; } = new List<OffdayVacationRequest>();

        //Bob o construtor
        public Company(string name, string description, string password, string salt, DateTime validUntil, int buildingsN, int registsPerMonth, int workersN,
            int dashboardAccounts, bool hasExcel, double monthlyPrice, double anualPrice)
        { 
            Name = name;
            Description = description;
            Password = password;
            Salt = salt;
            ValidUntil = validUntil;
            CreationDate = DateTime.Now;
            BuildingsN = buildingsN;
            RegistsPerMonth = registsPerMonth;
            WorkersN = workersN;
            DashboardAccounts = dashboardAccounts;
            HasExcel = hasExcel;
            MonthlyPrice = monthlyPrice;
            AnualPrice = anualPrice;
        }
    }
}
