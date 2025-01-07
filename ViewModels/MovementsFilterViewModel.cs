using GateKeeperV1.Dto;

namespace GateKeeperV1.ViewModels
{
    public class MovementsFilterViewModel
    {
        public List<DateOnly> Dates {  get; set; }
        public List<ShiftsInFilterDto> Shifts { get; set; }
        public List<BuildingInFilterDto> Buildings { get; set; }
    }
}
