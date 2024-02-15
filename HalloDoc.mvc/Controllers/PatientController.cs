using DataAccess.Models;
using DataAccess.CustomModels;
using HalloDoc.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.mvc.Controllers
{
    public class PatientController : Controller
    {


        private readonly ILogger<PatientController> _logger;
        private readonly ILoginService _loginService;
        private readonly IPatientService _patientService;
        private readonly IHttpContextAccessor _contx;
        private readonly INotyfService _notyf;


        public PatientController(ILogger<PatientController> logger, ILoginService loginService, IPatientService patientService,IHttpContextAccessor contx, INotyfService notyf)
        {
            _logger = logger;
            _loginService = loginService;
            _patientService = patientService;
            _contx = _contx;
            _notyf = notyf;
        }




        [HttpPost]

        public IActionResult patient_login(LoginModel loginModel)
        {
            if (_loginService.Login(loginModel))
            {
                _notyf.Success("Logged In Successfully !!");
                return RedirectToAction("patient_dashboard", "Patient");
            }

            _notyf.Error("Invalid Credentials");
            return RedirectToAction("patient_login", "Patient");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult patient_request(PatientInfoModel patientInfoModel)
        {

            if (ModelState.IsValid)
            {

                _patientService.AddPatientInfo(patientInfoModel);
                _notyf.Success(" Successfully !!");
                return RedirectToAction("submit_request", "Patient");
            }
            else
            {
                _notyf.Error("Please Fill The Required Details");
                return View(patientInfoModel);
            }




        }





        [HttpPost]
        public IActionResult friendfamily_request(FamilyReqModel familyReqModel)
        {
            if (ModelState.IsValid)
            {
                _patientService.AddFamilyReq(familyReqModel);
                _notyf.Success(" Successfully !!");
                return RedirectToAction("submit_request", "Patient");

            }
            else
            {
                _notyf.Error("Please Fill The Required Details");
                return View(familyReqModel);
            }

        }




        [HttpPost]
        public IActionResult concierge_request(ConciergeReqModel conciergeReqModel)
        {
            if (ModelState.IsValid)
            {

                _patientService.AddConciergeReq(conciergeReqModel);
                _notyf.Success(" Successfully !!");
                return RedirectToAction("submit_request", "Patient");
            }
            else
            {

                _notyf.Error("Please Fill The Required Details");
                return View(conciergeReqModel);
            }

        }

        [HttpPost]
        public IActionResult business_request(BusinessReqModel businessReqModel)
        {
            if (ModelState.IsValid)
            {

                _patientService.AddBusinessReq(businessReqModel);
                _notyf.Success(" Successfully !!");
                return RedirectToAction("submit_request", "Patient");
            }
            else
            {
                _notyf.Error("Please Fill The Required Details");
                return View(businessReqModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var emailExists = await _patientService.IsEmailExists(email);
            return Json(new { emailExists });
        }





        public IActionResult Index()
        {
            return View();
        }

        public IActionResult submit_request()
        {
            return View();
        }

        public IActionResult patient_login()
        {
            return View();
        }
        public IActionResult forgot_password()
        {
            return View();
        }
        public IActionResult patient_request()
        {
            return View();
        }

        public IActionResult friendfamily_request()
        {
            return View();
        }


        public IActionResult concierge_request()
        {
            return View();
        }

       
        public IActionResult business_request()
        {
            return View();
        }


        public IActionResult patient_newrequest()
        {
            return View();
        }


        public IActionResult someoneelse_newrequest()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult patient_dashboard()
        {
            var infos = _patientService.GetPatientInfos();
            var viewmodel = new PatientDashboardInfo { patientDashboardItems = infos };
            return View(viewmodel);
        }


    }
}