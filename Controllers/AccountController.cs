using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GateKeeperV1.Models;
using GateKeeperV1.ViewModels;

namespace GateKeeperV1.Controllers
{ 
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }




        /// <summary>
        /// Page for creating a new account
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        /// <summary>
        /// Methode for checking if the email is already taken before triyng to create account 
        /// (Currrently does not work check Part 75 Asp.Net Core totorual by kudvenkat)
        /// </summary>
        /// <param name="email">Email that the user is trying to register</param>
        /// <returns>Json for the view</returns>
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByNameAsync(email);

            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"The email {email} is already in use");
            }
        }


        /// <summary>
        /// Post action for Register
        /// </summary>
        /// <param name="model">Name, surname, email and password of the account that is being creted.</param>
        /// <returns>If the account is created sucessfully activates the redirect system, if not, returns back to the view</returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, Surname = model.Surname};
                var result = await userManager.CreateAsync(user, model.Password);


                if(result.Succeeded)
                {
                    if(signInManager.IsSignedIn(User) && (User.IsInRole("Staff") || User.IsInRole("StaffManager")))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Redirect", "Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        /// <summary>
        /// Logs the user out. Is HttpPost for safety resons. Has to be acessed thorug a form with method="post"
        /// </summary>
        /// <returns>Redirects the user to the initial page of the application</returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Page for loging in into an account
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// Post action for Login
        /// </summary>
        /// <param name="model">Credentials inserted by the user</param>
        /// <param name="ReturnUrl">In case the user comes from a page that requires authorization, this 
        /// that page's url so that we can redirect the user back to it</param>
        /// <returns>If the login is valid either activates the redirect system or returns to the return url. If not, it goes back to the view</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);


                if (result.Succeeded)
                { 
                    if(!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Redirect", "Home");
                    }
                }

                
                ModelState.AddModelError("", "Invalid login attempt");
                
            }

            return View(model);
        }

    }
}
