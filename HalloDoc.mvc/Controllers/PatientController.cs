using DataAccess.Models;
using DataAccess.CustomModels;
using HalloDoc.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using DataAccess.Data;

namespace HalloDoc.mvc.Controllers
{
    public class PatientController : Controller
    {


        private readonly ILogger<PatientController> _logger;
        private readonly ILoginService _loginService;
        private readonly IPatientService _patientService;
        private readonly IHttpContextAccessor _contx;
        private readonly INotyfService _notyf;
        private readonly ApplicationDbContext _db;



        public PatientController(ILogger<PatientController> logger, ILoginService loginService, IPatientService patientService, IHttpContextAccessor contx, INotyfService notyf, ApplicationDbContext db)
        {
            _logger = logger;
            _loginService = loginService;
            _patientService = patientService;
            _contx = _contx;
            _notyf = notyf;
            _db = db;

        }




        [HttpPost]
        public IActionResult patient_login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                //User user = _context.Users.FirstOrDefault(u => u.Aspnetuserid == item.Id);
                var user = _loginService.Login(loginModel);
                if (user.Firstname != null)
                {
                    _notyf.Success("Logged In Successfully !!");
                    return RedirectToAction("patient_dashboard", user);
                }
                else
                {
                    _notyf.Error("Invalid Credentials");

                    //ViewBag.AuthFailedMessage = "Please enter valid username and password !!";
                }
                return View();
            }
            else
            {
                return View(loginModel);
            }
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


        
        public IActionResult patient_dashboard(User user)
        {

            var infos = _patientService.GetMedicalHistory(user);
            //var viewmodel = new MedicalHistory { medicalHistoriesList = infos };
            return View(infos);
        }

        public IActionResult Edit(MedicalHistory medicalHistory)
        {
            var existingUser = _db.Users.FirstOrDefault(x => x.Email == medicalHistory.Email);
            existingUser.Firstname = medicalHistory.FirstName;


            _db.Users.Update(existingUser);
            _db.SaveChanges();
            return RedirectToAction("patient_dashboard", "Patient", existingUser);
        }


        public IActionResult GetDcoumentsById(int requestId)
        {
            var list = _patientService.GetAllDocById(requestId);
            return PartialView("_DocumentList", list.ToList());
        }
    }
}