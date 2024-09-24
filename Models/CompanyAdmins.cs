﻿namespace GateKeeperV1.Models
{
    public class CompanyAdmins
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
