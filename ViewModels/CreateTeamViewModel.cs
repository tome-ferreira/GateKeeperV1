﻿using GateKeeperV1.Models;

namespace GateKeeperV1.ViewModels
{
    public class CreateTeamViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<WorkerInTeamViewModel> Workers { get; set; } = new List<WorkerInTeamViewModel>();
    }
}
