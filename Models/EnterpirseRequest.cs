using GateKeeperV1.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace GateKeeperV1.Models
{
    public class EnterpirseRequest
    {
        public Guid Id { get; set; }
        // Foreign key to User
        [ForeignKey("User")]
        public string UserId { get; set; }

        // Navigation property
        public ApplicationUser User { get; set; }
        //----------------------------------------------
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty; 
        public string Password { get; set; } = string.Empty; 
        public string Salt {  get; set; } = string.Empty;
        public int BuildingsN { get; set; } ///
        public bool BuildingsNUnlimited { get; set; } ///

        public int RegistsPerMonth { get; set; } ///
        public bool RegistsPerMonthUnlimited { get; set; } ///

        public int Workers { get; set; } ///
        public bool WorkersUnlimited { get; set; } ///

        public int DashboardAccounts { get; set; } ///
        public bool DashboardAccountsUnlimited { get; set; } ///

        public bool HasExcel { get; set; } ///
        public DateTime CreationDate { get; set; }


        //Bob o construtor
        public EnterpirseRequest(EnterpriseRequestCostumizePlanViewModel ercpvm, string User, string name, string description, string password, string salt)
        {
            //EnterpriseRequestCostumizePlanViewModel
            BuildingsN = ercpvm.BuildingsN;
            BuildingsNUnlimited = ercpvm.BuildingsNUnlimited;

            RegistsPerMonth = ercpvm.RegistsPerMonth;
            RegistsPerMonthUnlimited = ercpvm.RegistsPerMonthUnlimited;

            Workers = ercpvm.Workers;
            WorkersUnlimited = ercpvm.WorkersUnlimited;

            DashboardAccounts = ercpvm.DashboardAccounts;
            DashboardAccountsUnlimited = ercpvm.DashboardAccountsUnlimited;

            HasExcel = ercpvm.HasExcel;

            //Outros
            UserId = User;
            Name = name;
            Description = description;
            Password = password;
            Salt = salt;

            //Creation date 
            CreationDate = DateTime.Now;
        }


        // Parameterless constructor for EF
        public EnterpirseRequest() { }


            
    }
}
