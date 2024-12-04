using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GateKeeperV1.Models;
using GateKeeperV1.ViewModels;

namespace GateKeeperV1.Controllers
{
    [Authorize(Roles = "Staff, StaffManager")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// Page for creating a new role
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [Authorize(Roles = "StaffManager")]
        public IActionResult CreateRole()
        {
            return View();
        }


        /// <summary>
        /// Post action for creating a new role
        /// </summary>
        /// <param name="model">Role name</param>
        /// <returns>Creates the role and redirects to a list of roles</returns>
        [HttpPost]
        [Authorize(Roles = "StaffManager")]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            IdentityRole identityRole = new IdentityRole()
            {
                Name = model.RoleName
            };
            IdentityResult result = await roleManager.CreateAsync(identityRole);
            return RedirectToAction("ListRoles", "Administration");
        }


        /// <summary>
        /// Page to list all the roles
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [Authorize(Roles = "StaffManager")]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }


        /// <summary>
        /// Page to 
        /// 
        /// 
        /// 
        /// a role
        /// </summary>
        /// <param name="id">Role to edit id</param>
        /// <returns>View</returns>
        [HttpGet]
        [Authorize(Roles = "StaffManager")]
        public async Task<IActionResult> EditRole(string id)
        {
            if (id.IsNullOrEmpty())
            {
                return RedirectToAction("ListRoles", "Administration");
            }

            var role = await roleManager.FindByIdAsync(id);

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            var users = userManager.Users.ToList();

            foreach (var user in users)
            {
                bool isInRole = await userManager.IsInRoleAsync(user, role.Name);
                if (isInRole == true)
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }


        /// <summary>
        /// Action to delete a role
        /// </summary>
        /// <param name="id">Id of the role to delete</param>
        /// <returns></returns>
        [Authorize(Roles = "StaffManager")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            var result = await roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");
            }
        }


        /// <summary>
        /// Post action for editing a role 
        /// </summary>
        /// <param name="model">Information about the role</param>
        /// <returns>Saves the changes and returns the view</returns>
        [HttpPost]
        [Authorize(Roles = "StaffManager")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);


                return RedirectToAction("ListRoles");

            }
        }


        /// <summary>
        /// Page to edit the users in a role
        /// </summary>
        /// <param name="roleId">Id of the role to to edit the users</param>
        /// <returns>View</returns>
        [HttpGet]
        [Authorize(Roles = "StaffManager")]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();


            var users = userManager.Users.ToList();
            foreach (var user in users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }


        /// <summary>
        /// Post action to edit the users in role
        /// </summary>
        /// <param name="model">List of users to save to that role</param>
        /// <param name="roleId">Id of the role to edit</param>
        /// <returns>Goes back to the generic edit role page</returns>
        [HttpPost]
        [Authorize(Roles = "StaffManager")]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }


        /// <summary>
        /// View to see all users of the application
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        [Authorize(Roles = "StaffManager")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }



        /// <summary>
        /// View to edit a user
        /// </summary>
        /// <param name="id">Id of the user to edit</param>
        /// <returns>View</returns>
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            // GetClaimsAsync retunrs the list of user Claims
            var userClaims = await userManager.GetClaimsAsync(user);
            // GetRolesAsync returns the list of user Roles
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles
            };

            return View(model);
        }





        /// <summary>
        /// Post action for editing a user
        /// </summary>
        /// <param name="model">Info of the user</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.Name = model.Name;
                user.Surname = model.Surname;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }


        /// <summary>
        /// Deletes a user. Is HttpPost for safety resons. Has to be acessed thorug a form with method="post"
        /// </summary>
        /// <param name="id">Id of the user to delete</param>
        /// <returns>Back to the list users view</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("ListUsers");
            }
        }
    }
}
