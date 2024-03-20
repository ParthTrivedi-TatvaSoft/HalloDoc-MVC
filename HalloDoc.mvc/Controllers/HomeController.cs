using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Enums;
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
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IAdminService adminService,INotyfService notyfService)
        {
            _logger = logger;
            _db = db;
            _adminService = adminService;
            _notyf = notyfService;
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

        [HttpGet]
        public IActionResult ReviewAgreement(int reqId)
        {
            var status = _adminService.GetStatusForReviewAgreement(reqId);
            if (status == (int)StatusEnum.MDEnRoute)
            {
                TempData["ReviewStatus"] = "Review Agreement is Already Accepted !!";
                return RedirectToAction("AgreementStatus");
            }
            else if (status == (int)StatusEnum.CancelledByPatient)
            {
                TempData["ReviewStatus"] = "Review Agreement is Already Cancelled by patient !!";
                return RedirectToAction("AgreementStatus");
            }
            else
            {
                AgreementModel model = new()
                {
                    reqId = reqId,
                };

                return View(model);
            }
        }

        public IActionResult AgreementStatus()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReviewAgreement(AgreementModel agreementModal)
        {
            bool isSaved = _adminService.AgreeAgreement(agreementModal);
            if (isSaved)
            {
                _notyf.Success("Agreement Accepted");
                return RedirectToAction("patient_login", "Patient");
            }
            _notyf.Error("Something went wrong");
            return RedirectToAction("ReviewAgreement", new { reqId = agreementModal.reqId });

        }

        [HttpGet]
        public IActionResult CancelAgreement(int reqId)
        {
            var model = _adminService.CancelAgreement(reqId);
            return PartialView("_cancelagreement", model);
        }

        [HttpPost]
        public IActionResult CancelAgreement(AgreementModel model)
        {
            bool isCancelled = _adminService.SubmitCancelAgreement(model);
            if (isCancelled)
            {
                _notyf.Success("Agreement Cancelled");
                return RedirectToAction("patient_login", "Patient");
            }
            _notyf.Error("Something went wrong");
            return RedirectToAction("ReviewAgreement", new { reqId = model.reqId });
        }


        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}