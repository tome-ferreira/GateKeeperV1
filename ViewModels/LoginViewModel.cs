using System.ComponentModel.DataAnnotations;

namespace GateKeeperV1.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remenber me")]
        public bool RememberMe { get; set; }   
    }
}
