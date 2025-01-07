using GateKeeperV1.Data;
using GateKeeperV1.Dto;
using GateKeeperV1.Models;

namespace GateKeeperV1.ViewModels
{
    public class DashboardViewModel
    {
        public Company Company { get; set; }

        public int EntriesToday { get; set; }
        public int ExitsToday {  get; set; }

        public List<Movement> NewstMovements { get; set; } = new List<Movement>();

        public List<ShiftWorkerCount> WorkersPerShift { get; set; } = new List<ShiftWorkerCount>();

        public List<DashboardTableRow> dashboardTableRows { get; set; } = new List<DashboardTableRow>();
        public DashboardViewModel(Company company)
        {
            Company = company;
        }
    }
}