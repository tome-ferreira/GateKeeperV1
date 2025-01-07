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
        
        public DateTime CreationDate { get; set; }


        
        

        // Parameterless constructor for EF
        public EnterpirseRequest() { }


            
    }
}
