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
using DataAccess.Enums;
using HalloDoc.mvc.Auth;

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

        [HttpGet]
        public IActionResult CheckEmailExists(string email)
        {
            var emailExists = _patientService.IsEmailExists(email);
            return Json(new { emailExists });
        }
        [HttpGet]
        public IActionResult CheckPasswordExists(string email)
        {
            var passwordExists = _patientService.IsPasswordExists(email);
            return Json(new { passwordExists });
        }

        public IActionResult create_account()
        {
            return View();
        }

        [HttpPost]
        public IActionResult create_account(CreateAccountModel createAccountModel)
        {
            if (ModelState.IsValid)
            {
                bool isCreated = _patientService.CreateAccount(createAccountModel);
                if (isCreated)
                {
                    _notyf.Success("Account Created Successfully !!");
                    return RedirectToAction("patient_login", "Patient");
                }
                else
                {
                    _notyf.Error("Something Went Wrong !!");
                    return RedirectToAction("create_account");
                }

            }
            else
            {
                return View(createAccountModel);
            }
        }


        public IActionResult Index()
        {
            return View();
        }



        

        [HttpPost]
        public IActionResult patient_login(LoginModel user)
        {
            if (ModelState.IsValid)
            {
                LoginResponseViewModel? result = _patientService.patient_login(user);
                if (result.Status == ResponseStatus.Success)
                {
                    HttpContext.Session.SetString("Email", user.Email);
                    Response.Cookies.Append("jwt", result.Token);
                    TempData["Success"] = "Login Successfully";
                    return RedirectToAction("patient_dashboard", "Patient");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    TempData["Error"] = result.Message;
                    return View();
                }
            }
            return View();
        }

        public IActionResult patient_login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult patient_request()
        {
            return View();
        }

        [HttpPost]
        public IActionResult patient_request(PatientInfoModel patientInfoModel)
        {


            if (patientInfoModel.password != null)
            {
                patientInfoModel.password = patientInfoModel.password;
            }
            bool isValid = _patientService.AddPatientInfo(patientInfoModel);
            if (!isValid)
            {
                _notyf.Error("Service is not available in entered Region");
                return View(patientInfoModel);
            }
            _notyf.Success("Submit Successfully !!");
            return RedirectToAction("submit_request", "Patient");

        }





        [HttpGet]
        public IActionResult friendfamily_request()
        {
            return View();
        }
        [HttpPost]
        public IActionResult friendfamily_request(FamilyReqModel familyReqModel)
        {
            //if (ModelState.IsValid)
            //{
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string createAccountLink = baseUrl + Url.Action("create_account", "Patient");
            bool isValid = _patientService.AddFamilyReq(familyReqModel, createAccountLink);
            if (!isValid)
            {
                _notyf.Error("Service is not available in entered Region");
                return View(familyReqModel);
            }
            _notyf.Success("Submit Successfully !!");
            return RedirectToAction("submit_request", "Patient");
            //}
            //else
            //{
            //    return View(familyReqModel);
            //}
        }



        [HttpGet]
        public IActionResult concierge_request()
        {
            return View();
        }


        [HttpPost]
        public IActionResult concierge_request(ConciergeReqModel conciergeReqModel)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string createAccountLink = baseUrl + Url.Action("create_account", "Patient");
            bool isValid = _patientService.AddConciergeReq(conciergeReqModel, createAccountLink);
            if (!isValid)
            {
                _notyf.Error("Service is not available in entered Region");
                return View(conciergeReqModel);
            }
            _notyf.Success("Submit Successfully !!");
            return RedirectToAction("submit_request", "Patient");
        }

        [HttpGet]
        public IActionResult business_request()
        {
            return View();
        }

        [HttpPost]
        public IActionResult business_request(BusinessReqModel businessReqModel)
        {
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string createAccountLink = baseUrl + Url.Action("create_account", "Patient");
            bool isValid = _patientService.AddBusinessReq(businessReqModel, createAccountLink);
            if (!isValid)
            {
                _notyf.Error("Service is not available in entered Region");
                return View(businessReqModel);
            }
            _notyf.Success("Submit Successfully !!");
            return RedirectToAction("submit_request", "Patient");
        }



        public IActionResult submit_request()
        {
            return View();
        }

       
        public IActionResult forgot_password()
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




        //public IActionResult patient_dashboard()
        //{
        //    int? userid = HttpContext.Session.GetInt32("UserId");

        //    var infos = _patientService.GetMedicalHistory((int)userid);

        //    return View(infos);
        //}

        [CustomAuthorize("User")]
        public IActionResult patient_dashboard()
        {
            var email = HttpContext.Session.GetString("Email");
            var userdata = _db.Users.Where(x => x.Email == email).FirstOrDefault();
            var medical = _patientService.GetMedicalHistory(userdata.Userid);


            return View(medical);
        }

        public IActionResult PatientLogout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("patient_login", "Patient");
        }

        [CustomAuthorize("User")]
        public IActionResult document_list(int reqId)
        {
            HttpContext.Session.SetInt32("rid", reqId);
            var y = _patientService.GetAllDocById(reqId);
            return View(y)
     ;
        }


        [HttpPost]
        public IActionResult UploadDocuments(DocumentModel model)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            if (model.uploadedFiles == null)
            {
                _notyf.Error("First Upload Files");
                return RedirectToAction("document_list", "Patient", new { reqId = rid });
            }
            bool isUploaded = _patientService.UploadDocuments(model.uploadedFiles, rid);
            if (isUploaded)
            {
                _notyf.Success("Uploaded Successfully");
                return RedirectToAction("document_list", "Patient", new { reqId = rid });
            }
            else
            {
                _notyf.Error("Upload Failed");
                return RedirectToAction("document_list", "Patient", new { reqId = rid });
            }
            }

        //[HttpPost]
        //public IActionResult document_list()
        //{
        //    var rid = (int)HttpContext.Session.GetInt32("rid");
        //    var file = HttpContext.Request.Form.Files.FirstOrDefault();
        //    _patientService.AddFile(file, rid);
        //    return RedirectToAction("document_list", "Patient", new { reqId = rid });
        //}



        public IActionResult ShowProfile(int userid)
        {
            HttpContext.Session.SetInt32("EditUserId", userid);
            var profile = _patientService.GetProfile(userid);
            return PartialView("_profile", profile);
        }

        public IActionResult SaveEditProfile(Profile profile)
        {
            int EditUserId = (int)HttpContext.Session.GetInt32("EditUserId");
            profile.userId = EditUserId;
            bool isEdited = _patientService.EditProfile(profile);
            if (isEdited)
            {
                _notyf.Success("Profile Edited Successfully");
                return RedirectToAction("patient_dashboard");
            }
            else
            {
                _notyf.Error("Profile Edited Failed");
                return RedirectToAction("patient_dashboard");
            }
        }
       
    }
}