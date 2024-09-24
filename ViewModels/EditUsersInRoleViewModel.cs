using GateKeeperV1.Models;

namespace GateKeeperV1.ViewModels
{
    public class EditUsersInRoleViewModel
    {
        public string RoleName { get; set; }
        public List<ApplicationUser> users { get; set; }
    }
}
