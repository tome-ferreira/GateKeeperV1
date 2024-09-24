using System.ComponentModel.DataAnnotations;

namespace GateKeeperV1.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Compare("UserName", ErrorMessage = "User name and email have to be the same")]
        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
