using DataAccess.Models;
using DataAccess.CustomModels;
using HalloDoc.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;

namespace HalloDoc.mvc.Controllers
{
    public class PatientController : Controller
    {


        private readonly ILogger<PatientController> _logger;
        private readonly ILoginService _loginService;
        private readonly IPatientService _patientService;

        public PatientController(ILogger<PatientController> logger, ILoginService loginService, IPatientService patientService)
        {
            _logger = logger;
            _loginService = loginService;
            _patientService = patientService;
        }




        [HttpPost]

        public IActionResult patient_login(LoginModel loginModel)
        {
            if (_loginService.Login(loginModel))
            {

                return RedirectToAction("patient_dashboard", "Patient");
            }
    

            return RedirectToAction("patient_login", "Patient");
        }


        [HttpPost]
        public IActionResult friendfamily_request(FamilyReqModel familyReqModel)
        {

            _patientService.AddFamilyReq(familyReqModel);

            return RedirectToAction("submit_request", "Patient");
        }


        [HttpPost]
        public IActionResult patient_request(PatientInfoModel patientInfoModel)
        {

            _patientService.AddPatientInfo(patientInfoModel);

            return RedirectToAction("submit_request", "Patient");
        }


        [HttpPost]
        public IActionResult concierge_request(ConciergeReqModel conciergeReqModel)
        {

            _patientService.AddConciergeReq(conciergeReqModel);

            return RedirectToAction("submit_request", "Patient");
        }

        [HttpPost]
        public IActionResult business_request(BusinessReqModel businessReqModel)
        {

            _patientService.AddBusinessReq(businessReqModel);

            return RedirectToAction("submit_request", "Patient");
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