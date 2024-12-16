using GateKeeperV1.Data;
using GateKeeperV1.Models;
using GateKeeperV1.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GateKeeperV1.Controllers
{
    public class QrCodeController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFunctions functions;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;

        public QrCodeController(ApplicationDbContext dbContext, IFunctions functions, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.functions = functions;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
        }

        private Bitmap GenerateQrCodeImage(Guid WorkerId, string CompanyId)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(WorkerId.ToString(), QRCodeGenerator.ECCLevel.Q);

                using (var qrCode = new QRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(20); 
                }
            }
        }

        public async Task<IActionResult> GenerateQrCode(Guid WorkerId)
        {
            string? companyId = HttpContext.Session.GetString("companyId");
            if (string.IsNullOrEmpty(companyId))
            {
                return RedirectToAction("Index", "Client");
            }

            var user = await userManager.GetUserAsync(User);
            var canAcess = await functions.IsUserInCompanyRole(user.Id, "Admin") || await functions.IsUserInCompanyRole(user.Id, "HR"); if (!canAcess) { return View("AcessDenied"); }

            var worker = await dbContext.WorkerProfiles.FindAsync(WorkerId);

            var companyPath = Path.Combine(webHostEnvironment.WebRootPath, "companies", companyId, "workerQrCodes");
            if (!Directory.Exists(companyPath))
            {
                Directory.CreateDirectory(companyPath);
            }

            var fileName = $"{WorkerId}.png"; 
            var filePath = Path.Combine(companyPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "image/png","worker_" + worker.InternalNumber + "_qrcode.png");
            }


            //Gerar código se não existir
            var qrCodeImage = GenerateQrCodeImage(WorkerId, companyId); 
            var qrCodeFilePath = Path.Combine(companyPath, fileName);

            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                imageBytes = memoryStream.ToArray();
            }

            return File(imageBytes, "image/png", "worker_" + worker.InternalNumber + "_qrcode.png");
        } 
    }
}
