using GateKeeperV1.Models;

namespace GateKeeperV1.ViewModels
{
    public class DashboardViewModel
    {
        public Company Company { get; set; }

        public DashboardViewModel(Company company)
        {
            Company = company;
        }
    }
}