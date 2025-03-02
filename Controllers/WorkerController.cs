﻿using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using GateKeeperV1.Services.ServicoEmail;
using GateKeeperV1.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GateKeeperV1.Controllers
{
    public class WorkerController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFunctions functions;
        private readonly IEmailSender emailSender;

        public WorkerController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IFunctions functions, IEmailSender emailSender)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.functions = functions;
            this.emailSender = emailSender;
        }

        //Password generator for creating new user
        private string GenerateRandomPassword(int length)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";
            const string symbols = "!@#$%^&*()_-+=<>?";

            string allChars = letters + numbers + symbols;
            StringBuilder password = new StringBuilder();

            // Use cryptographically secure random number generator
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] data = new byte[length];
                rng.GetBytes(data);

                for (int i = 0; i < length; i++)
                {
                    int index = data[i] % allChars.Length;
                    password.Append(allChars[index]);
                }
            }

            return password.ToString();
        }

        [HttpGet]
        public async Task<IActionResult> ListWorkers()
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }


            var workers = await dbContext.WorkerProfiles
            .Where(wp => wp.CompanyId.ToString() == companyId)
            .Include(wp => wp.ApplicationUser)
            .Select(wp => new WorkerProfileWithUserViewModel
            {
                WorkerProfileId = wp.Id,
                InternalNumber = wp.InternalNumber,
                Role = wp.Role,
                ApplicationUserId = wp.ApplicationUserId,
                UserEmail = wp.ApplicationUser.Email,
                UserFullName = wp.ApplicationUser.Name + " " + wp.ApplicationUser.Surname
            })
            .ToListAsync();

            return View(workers);
        }
        [HttpGet]
        public async Task<IActionResult> WorkerDetails(Guid workerId)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var worker = await dbContext.WorkerProfiles.Include(w => w.ApplicationUser).Where(w => w.Id == workerId).FirstAsync();

            if(worker == null)
            {
                //ToDo: this
                return View("NotFound");
            }

            return View(worker);
        }


        [HttpGet]
        public async Task<IActionResult> CreateWorker()
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateWorker(CreateWorkerViewModel model)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }
            var company = await dbContext.Companies.FindAsync(Guid.Parse(companyId));

            string emailRole;
            if(model.Role == "Admin")
            {
                emailRole = "a GateKeeper administration.";
            }else if(model.Role == "HR")
            {
                emailRole = "a human resources team member.";
            }
            else
            {
                emailRole = "a worker.";
            }

            if (ModelState.IsValid)
            {
                var checkUser = await userManager.FindByEmailAsync(model.Email);
                string userId;
                if(checkUser != null)
                {
                    var message = new Message(new string[] { checkUser.Email }, "Added to a new company",
                        new TextPart("html")
                        {
                            Text = "<p>Hi " + checkUser.Name + ".</p>" +
                            "<p>We are happy to inform you that you've been added to " + company.Name + "'s organization as " + emailRole + "<p>" +
                            "<p>To acess your dashboard, log in to your GateKeeper account.<p>" +
                            "<p><p>" +
                            "<p>Best regards,<p>" +
                            "<p>GateKeeper team.<p>"
                        });

                    emailSender.SendEmail(message);

                    userId = checkUser.Id;
                }
                else
                {
                    var newUser = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Name, Surname = model.Surname };
                    var password = GenerateRandomPassword(10);
                    var result = await userManager.CreateAsync(newUser, password);

                    var message = new Message(new string[] { newUser.Email }, "Added to a new company",
                        new TextPart("html")
                        {
                            Text = "<p>Hi " + newUser.Name + ".</p>" +
                            "<p>We are happy to inform you that you've been added to " + company.Name + "'s organization as " + emailRole + "<p>" +
                            "<p>A GateKeeper account as already been create for you. You can acess it by using the email where you received this message and the " +
                            "following password: <strong>" + password + "</strong></p>" +
                            "<p><strong>WE STRONGLY RECOMMEND THAT YOU CHANGE YOUR PASSWORD UPON FIRST LOGIN</strong><p>" +
                            "<p><p>" +
                            "<p>Best regards,<p>" +
                            "<p>GateKeeper team.<p>"
                        });

                    emailSender.SendEmail(message);

                    userId = newUser.Id;
                }

                int internalNumber = await functions.GenerateInternalNumber(Guid.Parse(companyId));

                WorkerProfile workerProfile = new WorkerProfile(internalNumber, Guid.Parse(companyId), model.Notes, model.Role, userId);

                await dbContext.WorkerProfiles.AddAsync(workerProfile);

                await dbContext.SaveChangesAsync();

                if (model.Continue)
                {
                    return RedirectToAction("CreateWorker", "Worker");
                }

                return RedirectToAction("ListWorkers", "Worker");
            }
            return View(model);
        }






        public async Task<IActionResult> DeleteWorker(Guid workerId)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }
            var company = await dbContext.Companies.FindAsync(Guid.Parse(companyId));

            var worker = await dbContext.WorkerProfiles.Include(w => w.ApplicationUser).Where(w => w.Id == workerId).FirstAsync();

            if (worker != null)
            {
                if(worker.ApplicationUser.Email == user.Email)
                {
                    //ToDo
                    return View("Are you dumb?");
                }

                dbContext.WorkerProfiles.Remove(worker);
                await dbContext.SaveChangesAsync();

                var message = new Message(new string[] { worker.ApplicationUser.Email }, "Removed from a company",
                        new TextPart("html")
                        {
                            Text = "<p>Hi " + worker.ApplicationUser.Name + ".</p>" +
                            "<p>We regret to inform you that you've been removed from " + company.Name + "'s organization.<p>" +
                            "<p>If you believe this to be a mistake, please contact your company's administration.<p>" +
                            "<p><p>" +
                            "<p>Best regards,<p>" +
                            "<p>GateKeeper team.<p>"
                        });

                emailSender.SendEmail(message);

                return RedirectToAction("ListWorkers", "Worker");
            }
            return View("Error");
        }


        
        
        [HttpGet]
        public async Task<IActionResult> CreateTeam()
        {

            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var workers = await dbContext.WorkerProfiles
                .Include(w => w.ApplicationUser) 
                .Where(w => w.CompanyId.ToString() == companyId)
                .ToListAsync();


            CreateTeamViewModel model = new CreateTeamViewModel() { };

            foreach (var w in workers) 
            {
                WorkerInTeamViewModel worker = new WorkerInTeamViewModel()
                {
                    UserId = w.Id,
                    Email = w.ApplicationUser.Email,
                    Role = w.Role,
                    FullName = w.ApplicationUser.Name + " " + w.ApplicationUser.Surname,
                    isSelected = false
                };

                model.Workers.Add(worker);
            }

            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(CreateTeamViewModel model)
        {
            if (!model.Workers.Where(w => w.isSelected == true).Any() || model.Name.IsNullOrEmpty())
            {
                if(model.Name.IsNullOrEmpty()) 
                {
                    ViewBag.NameError = "O campo nome é obrigatório.";
                }
                if (!model.Workers.Where(w => w.isSelected == true).Any()) 
                {
                    ViewBag.WorkersError = "Por favor selecione pelo menos um trabalhador.";
                }
                return View(model);
            }

            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            WorkersTeam workersTeam = new WorkersTeam() 
            { 
                Name = model.Name,
                Description = model.Description,
                CompanyId = Guid.Parse(companyId),
            };

            await dbContext.WorkersTeams.AddAsync(workersTeam);

            var teamMemberships = new List<WorkerTeamMembership>();

            foreach (var wrk in model.Workers)
            {
                if (wrk.isSelected)
                {
                    teamMemberships.Add(new WorkerTeamMembership()
                    {
                        TeamId = workersTeam.Id,
                        WorkerId = wrk.UserId
                    });
                }
            }

            await dbContext.WorkerTeamMemberships.AddRangeAsync(teamMemberships);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("TeamDetails", "Worker", new { TeamId = workersTeam.Id });
        }


        [HttpGet]
        public async Task<IActionResult> TeamDetails(Guid TeamId)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var team = await dbContext.WorkersTeams.Include(wt => wt.TeamMemberships).ThenInclude(tm => tm.Worker).ThenInclude(w => w.ApplicationUser).Where(wt => wt.Id == TeamId).FirstOrDefaultAsync();

            if(team == null)
            {
                return View("Error");
            }

            return View(team);
        }

        [HttpGet]
        public async Task<IActionResult> ListTeams()
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }


            var teams = await dbContext.WorkersTeams.Include(wt => wt.TeamMemberships).Where(wt => wt.CompanyId == Guid.Parse(companyId)).ToListAsync();

            return View(teams);
        }

        public async Task<IActionResult> DeleteTeam(Guid Id)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var team = await dbContext.WorkersTeams.Include(wt => wt.TeamMemberships).Where(wt => wt.Id == Id).FirstOrDefaultAsync();

            if (team == null) 
            { 
                ViewBag.Error = "Erro ao apagar equipa";  
                return RedirectToAction("ListTeams", "Worker"); 
            }

            

            dbContext.WorkersTeams.Remove(team);
            dbContext.WorkerTeamMemberships.RemoveRange(team.TeamMemberships);

            await dbContext.SaveChangesAsync();


            return RedirectToAction("ListTeams", "Worker");
        }


        [HttpGet]
        public async Task<IActionResult> EditTeam (Guid TeamId)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var team = await dbContext.WorkersTeams
                .Include(wt => wt.TeamMemberships)
                .ThenInclude(tm => tm.Worker)
                .ThenInclude(w => w.ApplicationUser)
                .Where(wt => wt.Id == TeamId)
                .FirstOrDefaultAsync();

            if (team == null)
            {
                ViewBag.Error = "Erro ao editar equipa";
                return RedirectToAction("ListTeams", "Worker");
            }

            var workers = await dbContext.WorkerProfiles
                .Include(w => w.ApplicationUser)
                .Where(w => w.CompanyId.ToString() == companyId)
                .ToListAsync();



            EditTeamViewModel model = new EditTeamViewModel()
            {  
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
            };

            foreach(var w in team.TeamMemberships)
            {
                WorkerInTeamViewModel worker = new WorkerInTeamViewModel()
                {
                    UserId = w.Worker.Id,
                    Email = w.Worker.ApplicationUser.Email,
                    Role = w.Worker.Role,
                    FullName = w.Worker.ApplicationUser.Name + " " + w.Worker.ApplicationUser.Surname,
                    isSelected = true
                };
                model.Workers.Add(worker);
            }

            foreach (var w in workers)
            {
                WorkerInTeamViewModel worker = new WorkerInTeamViewModel()
                {
                    UserId = w.Id,
                    Email = w.ApplicationUser.Email,
                    Role = w.Role,
                    FullName = w.ApplicationUser.Name + " " + w.ApplicationUser.Surname,
                    isSelected = false
                };

                if (!model.Workers.Where(wk => wk.UserId == w.Id).Any())
                {
                    model.Workers.Add(worker);
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditTeam(EditTeamViewModel model)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin"); if (!canAcess) { return View("AcessDenied"); }

            var team = await dbContext.WorkersTeams.Include(wt => wt.TeamMemberships).Where(wt => wt.Id == model.Id).FirstOrDefaultAsync();

            team.Name = model.Name;
            team.Description = model.Description;

            dbContext.WorkerTeamMemberships.RemoveRange(team.TeamMemberships);



            var teamMemberships = new List<WorkerTeamMembership>();

            foreach (var wrk in model.Workers)
            {
                if (wrk.isSelected)
                {
                    teamMemberships.Add(new WorkerTeamMembership()
                    {
                        TeamId = model.Id,
                        WorkerId = wrk.UserId
                    });
                }
            }

            await dbContext.WorkerTeamMemberships.AddRangeAsync(teamMemberships);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("TeamDetails", "Worker", new { TeamId = model.Id });
        }
    }
}
