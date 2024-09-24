using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GateKeeperV1.Data;
using GateKeeperV1.Services;

namespace GateKeeperV1.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;

        public ClientController(ApplicationDbContext dbContext, IFunctions functions)
        {
            this.dbContext = dbContext;
            this.functions = functions;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var companies = functions.GetUserCompanys(userId);

            return View();
        }
    }
}
