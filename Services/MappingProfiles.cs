using AutoMapper;
using GateKeeperV1.Models;
using GateKeeperV1.ViewModels;

namespace GateKeeperV1.Services
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<CreatePlanViewModel, Plan>();
        }
    }
}
