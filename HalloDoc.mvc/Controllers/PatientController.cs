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
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var emailExists = await _patientService.IsEmailExists(email);
            return Json(new { emailExists });
        }





        public IActionResult Index()
        {
            return View();
        }



        //[HttpPost]
        //public IActionResult patient_login(LoginModel loginModel)
        //{


        //    if (ModelState.IsValid)
        //    {
        //        string passwordhash = loginModel.Password;
        //        loginModel.Password = passwordhash;
        //        var user = _loginService.Login(loginModel);

        //        //var userId = user.Userid;
        //        HttpContext.Session.SetInt32("UserId", user.Userid);

        //        //the above data is coming from user table and storing in user object
        //        if (user != null)
        //        {
        //            TempData["username"] = user.Firstname;
        //            TempData["id"] = user.Lastname;
        //            _notyf.Success("Logged In Successfully !!");
        //            return RedirectToAction("patient_dashboard", "Patient");
        //        }
        //        else
        //        {
        //            _notyf.Error("Invalid Credentials");

        //            //ViewBag.AuthFailedMessage = "Please enter valid username and password !!";
        //        }
        //        return View();
        //    }
        //    else
        //    {
        //        return View(loginModel);
        //    }
        //}

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

        [HttpPost]
        
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



        public IActionResult submit_request()
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