using System.ComponentModel.DataAnnotations;

namespace GateKeeperV1.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }


        public string Id { get; set; }
        [Required(ErrorMessage = "Role name is mandatory")]
        public string RoleName { get; set; }

        public List<string> Users { get; set;}
     }
}
