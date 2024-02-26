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
using System.Net.Mail;
using System.Net;

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
        



        public PatientController(ILogger<PatientController> logger, ILoginService loginService, IPatientService patientService, IHttpContextAccessor contx, INotyfService notyf, ApplicationDbContext db )
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
                if (user!=null)
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

            //_emailSender.SendEmailAsync("hello", "hello", "hello");
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

        public IActionResult Edit(MedicalHistory medicalHistory)
        {

            var existingUser = _db.Users.FirstOrDefault(x => x.Email == medicalHistory.Email);
            existingUser.Firstname = medicalHistory.FirstName;
            existingUser.Lastname = medicalHistory.LastName;
            //existingUser.dob = medicalHistory.DateOfBirth;
            existingUser.Email = medicalHistory.Email;
            //existingUser. = medicalHistory.ContactType;
            existingUser.Mobile = medicalHistory.PhoneNo;
            existingUser.Street = medicalHistory.Street;
            existingUser.City = medicalHistory.City;
            existingUser.State = medicalHistory.State;
            existingUser.Zipcode = medicalHistory.ZipCode;

            _db.Users.Update(existingUser);
            _db.SaveChanges();
            //var id = HttpContext.Session.GetInt32("existingUser");
            return RedirectToAction("patient_dashboard", "Patient", existingUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        
        public IActionResult patient_dashboard(User user)
        {

            var infos = _patientService.GetMedicalHistory(user);
            var viewmodel = new MedicalHistoryList { medicalHistoriesList = infos };
            return View(viewmodel);
        }

        public IActionResult GetDcoumentsById(int requestId)
        {
            var list = _patientService.GetAllDocById(requestId);
            return PartialView("_DocumentList", list.ToList());
        }


        public IActionResult PatientResetPasswordEmail(Aspnetuser user)

        {

            var usr =_db.Aspnetusers.FirstOrDefault(x => x.Email == user.Email);
            if (usr != null)
            {
                string Id = _db.Aspnetusers.FirstOrDefault(x => x.Email == user.Email).Id;
                string resetPasswordUrl = GenerateResetPasswordUrl(Id);
                SendEmail(user.Email, "Reset Your Password", $"Hello, reset your password using this link: {resetPasswordUrl}");
            }
            else
            {
                _notyf.Error("Email Id Not Registered");
                return RedirectToAction("forgot_password", "Patient");
            }
           

            return RedirectToAction("patient_login");
        }

        private string GenerateResetPasswordUrl(string userId)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string resetPasswordPath = Url.Action("patient_resetpassword", new { id = userId });
            return baseUrl + resetPasswordPath;
        }

        private Task SendEmail(string email, string subject, string message)
        {
            var mail = "tatva.dotnet.parthtrivedi@outlook.com";
            var password = "Parth@70160";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
        }

        // Handle the reset password URL in the same controller or in a separate one
        public IActionResult patient_resetpassword(string id)
        {
            var aspuser = _db.Aspnetusers.FirstOrDefault(x => x.Id == id);
            return View(aspuser);
        }

        [HttpPost]
        public IActionResult patient_resetpassword(Aspnetuser aspnetuser)
        {
            var aspuser = _db.Aspnetusers.FirstOrDefault(x => x.Id == aspnetuser.Id);
            aspuser.Passwordhash = aspnetuser.Passwordhash;
            _db.Aspnetusers.Update(aspuser);
            _db.SaveChanges();
            return RedirectToAction("patient_login");
        }
    }
}