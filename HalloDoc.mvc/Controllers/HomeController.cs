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

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
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

        public IActionResult ReviewAgreement()
        { 
            return View();
        }
        public IActionResult AgreeAgreement(CancelAgreementModal model)
        {

            return View();
        }

        public IActionResult CancelAgreementModal(int requestClientId)
        {
            Requestclient? reqCli = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == requestClientId);
            CancelAgreementModal obj = new()
            {
                ReqClientId = requestClientId,
                PatientName = reqCli.Firstname + " " + reqCli.Lastname
            };

            return PartialView("_cancelagreement", obj);
        }
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