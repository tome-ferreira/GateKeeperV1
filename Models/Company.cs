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


        public ICollection<CompanyAdmins> CompanyAdmins { get; set; }
        public ICollection<CompanyManagers> CompanyManagers { get; set; }
        public ICollection<CompanySupervisors> CompanySupervisors { get; set; }
        public ICollection<CompanyWorkers> CompanyWorkers { get; set; }


        public Company(string name, string description, string password, string salt, DateTime validUntil)
        {
            Name = name;
            Description = description;
            Password = password;
            Salt = salt;
            ValidUntil = validUntil;
        }
    }
}
