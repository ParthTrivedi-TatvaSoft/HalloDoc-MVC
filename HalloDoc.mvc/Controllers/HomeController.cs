using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Models;
using HalloDoc.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HalloDoc.mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IAdminService _adminService;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IAdminService adminService)
        {
            _logger = logger;
            _db = db;
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ReviewAgreement(int reqId)
        {
            AgreementModal am = new AgreementModal();
            am.Reqid = reqId;
            return View(am);
        }
        public IActionResult AgreeAgreement(AgreementModal agreementModal)
        {
            var model = _adminService.IAgreeAgreement(agreementModal);
            return RedirectToAction("admin_dashboard", "Admin", model);

        }

        public IActionResult CancelAgreement(AgreementModal agreementModal)
        {
            var model = _adminService.ICancelAgreement(agreementModal);
            return PartialView("_cancelagreement", model);
        }

        [HttpPost]
        public IActionResult CancelAgreementSubmit(int ReqClientid, string Description)
        {
            AgreementModal model = new()
            {
                ReqClientId = ReqClientid,
                Reason = Description,
            };
            var obj = _adminService.SubmitCancelAgreement(model);
            return RedirectToAction("admin_dashboard", "Admin", obj);
        }




        //public IActionResult CancelAgreementModal(int requestClientId)
        //{
        //    Requestclient? reqCli = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == requestClientId);
        //    CancelAgreementModal obj = new()
        //    {
        //        ReqClientId = requestClientId,
        //        PatientName = reqCli.Firstname + " " + reqCli.Lastname
        //    };

        //    return PartialView("_cancelagreement", obj);
        //}
        //public IActionResult CancelAgreementSubmit(int ReqClientid, string Description)
        //{
        //    _authService.CancelAgreementSubmit(ReqClientid, Description);
        //    return RedirectToAction("");
        //}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}