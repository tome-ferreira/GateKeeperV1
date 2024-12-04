﻿using GateKeeperV1.Models;

namespace GateKeeperV1.ViewModels
{
    public class CreateShiftViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TimeOnly Starts { get; set; }
        public TimeOnly Ends { get; set; }
        public Guid BuildingId { get; set; }
        public bool IsOvernight { get; set; }  // New property for overnight shifts
        public List<string> Days { get; set; } = new List<string>();
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsRepetitive { get; set; }
        public bool WorksOnHolydays { get; set; }
    }
}