using System.ComponentModel.DataAnnotations;

namespace GateKeeperV1.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role name")]
        public string RoleName {  get; set; } = string.Empty;
    }
}
