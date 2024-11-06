using Microsoft.AspNetCore.Mvc;

namespace GateKeeperV1.Controllers
{
    public class CompanyErrorController : Controller
    {
        public IActionResult CompanyNotFound()
        {
            return View();
        }

        public IActionResult CompanyError()
        {
            return View();
        }

        public IActionResult BuildingError()
        {
            return View();
        }
    }
}
