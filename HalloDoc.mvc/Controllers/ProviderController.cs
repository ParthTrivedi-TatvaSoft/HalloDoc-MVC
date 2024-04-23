using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Models;
using HalloDoc.mvc.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Rotativa.AspNetCore;

namespace HalloDoc.mvc.Controllers
{
    [CustomAuthorize("Physician")]
    public class ProviderController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly INotyfService _notyf;
        private readonly IAdminService _adminService;
        private readonly IPatientService _patientService;
        private readonly IJwtService _jwtService;
        private readonly IProviderService _providerService;


        public ProviderController(ILogger<AdminController> logger, INotyfService notyfService, IAdminService adminService, IPatientService patientService, IJwtService jwtService, IProviderService providerService)
        {
            _logger = logger;
            _notyf = notyfService;
            _adminService = adminService;
            _patientService = patientService;
            _jwtService = jwtService;
            _providerService = providerService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public IActionResult provider_dashboard()
        {
            var email = GetTokenEmail();
            var model = _providerService.GetLoginDetail(email);
            return View(model);
        }
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("admin_login", "Home");
        }

        public IActionResult GetCount()
        {
            var aspid = GetLoginId();
            var phyid = _providerService.GetPhysicianId(aspid);
            var statusCountModel = _providerService.GetStatusCount(phyid);
            return PartialView("_PallRequest", statusCountModel);
        }
        public IActionResult GetRequestsByStatus(int tabNo, int CurrentPage)
        {
            var aspid = GetLoginId();
            var phyid = _providerService.GetPhysicianId(aspid);
            var list = _providerService.GetRequestsByStatus(tabNo, CurrentPage, phyid);

            if (tabNo == 0)
            {
                return Json(list);
            }
            if (tabNo == 1)
            {
                return PartialView("_Pnewrequest", list);
            }
            else if (tabNo == 2)
            {
                return PartialView("_Ppendingrequest", list);
            }
            else if (tabNo == 3)
            {
                return PartialView("_Pactiverequest", list);
            }
            else if (tabNo == 4)
            {
                return PartialView("_Pconcluderequest", list);
            }

            return View();
        }


        

        [HttpPost]
        public string ExportReq(List<AdminDashTableModel> reqList)
        {
            StringBuilder stringbuild = new StringBuilder();

            string header = "\"No\"," + "\"Name\"," + "\"DateOfBirth\"," + "\"Requestor\"," +
                "\"RequestDate\"," + "\"Phone\"," + "\"Notes\"," + "\"Address\","
                 + "\"Physician\"," + "\"DateOfService\"," + "\"Region\"," +
                "\"Status\"," + "\"RequestTypeId\"," + "\"OtherPhone\"," + "\"Email\"," + "\"RequestId\"," + Environment.NewLine + Environment.NewLine;

            stringbuild.Append(header);
            int count = 1;

            foreach (var item in reqList)
            {
                string content = $"\"{count}\"," + $"\"{item.firstName}\"," + $"\"{item.intDate}\"," + $"\"{item.requestorFname}\"," +
                    $"\"{item.intDate}\"," + $"\"{item.mobileNo}\"," + $"\"{item.notes}\"," + $"\"{item.street}\"," +
                    $"\"{item.lastName}\"," + $"\"{item.intDate}\"," + $"\"{item.street}\"," +
                    $"\"{item.status}\"," + $"\"{item.requestTypeId}\"," + $"\"{item.mobileNo}\"," + $"\"{item.firstName}\"," + $"\"{item.Reqid}\"," + Environment.NewLine;

                count++;
                stringbuild.Append(content);
            }

            string finaldata = stringbuild.ToString();

            return finaldata;

        }

        [HttpGet]
        public IActionResult SendLink()
        {
            return PartialView("_Psendlink");
        }
        [HttpPost]
        public IActionResult SendLink(SendLinkModel model)
        {
            bool isSend = false;
            try
            {
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string reviewPathLink = baseUrl + Url.Action("submit_request", "Patient");

                SendEmail(model.email, "Create Patient Request", $"Hello, Please Create Patient Request From This Link: {reviewPathLink}");
                _notyf.Success("Link Sent");
                isSend = true;
            }
            catch (Exception ex)
            {
                _notyf.Error("Failed To Sent");
            }
            return Json(new { isSend = isSend });

        }
        public Task SendEmail(string email, string subject, string message)
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

        public IActionResult viewcase(int Requestclientid, int RequestTypeId)
        {
            var model = _adminService.ViewCaseViewModel(Requestclientid, RequestTypeId);

            return PartialView("_PViewCase", model);
        }

        //[HttpPost]
        //public IActionResult UpdateNotes(ViewNotesModel model)
        //{
        //    bool isUpdated = _adminService.UpdateAdminNotes(model.AdditionalNotes, model.ReqId, 2);

        //    return Json(new { isUpdated, reqId = model.ReqId });
        //}

        //public IActionResult ViewNote(int ReqId)
        //{

        //    ViewNotesModel data = _adminService.ViewNotes(ReqId);
        //    return PartialView("_Pviewnotes", data);
        //}


        [HttpPost]
        public IActionResult UpdateNotes(ViewNotesModel model)
        {
            int? reqId = HttpContext.Session.GetInt32("RNId");
            bool isUpdated = _adminService.UpdateAdminNotes(model.AdditionalNotes, (int)reqId, 2);
            if (isUpdated)
            {
                _notyf.Success("Saved Changes !!");
                return RedirectToAction("Pviewnotes", "Provider", new { ReqId = reqId });

            }
            return View();
        }

        public IActionResult Pviewnotes(int ReqId)
        {
            HttpContext.Session.SetInt32("RNId", ReqId);
            ViewNotesModel data = _adminService.ViewNotes(ReqId);
            return View(data);
        }

        public IActionResult AcceptCase(int requestId)
        {
            var loginUserId = GetLoginId();
            _providerService.AcceptCase(requestId, loginUserId);
            return Ok();
        }

        public IActionResult CancelCase(int reqId)
        {

            var model = _adminService.CancelCase(reqId);
            model.reqId = reqId;
            return PartialView("_Pcancelcase", model);
        }


        [HttpPost]
        public IActionResult SubmitCancelCase(CancelCaseModel cancelCaseModel, int reqId)
        {
            cancelCaseModel.reqId = reqId;
            bool isCancelled = _adminService.SubmitCancelCase(cancelCaseModel);

            if (isCancelled)
            {
                _notyf.Success("Cancelled Successfully");
                return RedirectToAction("provider_dashboard");
            }
            _notyf.Error("Cancellation Failed");
            return RedirectToAction("provider_dashboard");
        }

        [HttpGet]
        public IActionResult AssignCase(int reqId)
        {
            HttpContext.Session.SetInt32("AssignReqId", reqId);
            var model = _adminService.AssignCase(reqId);
            return PartialView("_assigncase", model);
        }

        public IActionResult GetPhysician(int selectRegion)
        {
            List<Physician> physicianlist = _adminService.GetPhysicianByRegion(selectRegion);
            return Json(new { physicianlist });
        }

        [HttpPost]
        public IActionResult SubmitAssignCase(AssignCaseModel assignCaseModel)
        {
            assignCaseModel.ReqId = HttpContext.Session.GetInt32("AssignReqId");
            bool isAssigned = _adminService.SubmitAssignCase(assignCaseModel);
            if (isAssigned)
            {
                _notyf.Success("Assigned Successfully");
                return RedirectToAction("provider_dashboard", "Provider");
            }
            return View();
        }

        [HttpGet]
        public IActionResult TranferRequest(int reqId)
        {

            TransferRequest model = new();
            model.ReqId = reqId;
            return PartialView("_Ptransferrequest", model);
        }

        [HttpPost]
        public IActionResult TranferRequest(TransferRequest model)
        {

            bool isTranferred = _providerService.TransferRequest(model);
            if (isTranferred)
            {
                _notyf.Success("Tranferred Successfully");
                return RedirectToAction("provider_dashboard", "Provider");
            }
            _notyf.Error("Tranferred Failed");
            return RedirectToAction("provider_dashboard", "Provider");
        }


        public IActionResult Pviewuploads(int reqId)
        {
            HttpContext.Session.SetInt32("rid", reqId);
            var model = _adminService.GetAllDocById(reqId);
            return View(model);
        }

        [HttpPost]
        public IActionResult UploadFiles(ViewUploadModel model)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            if (model.uploadedFiles == null)
            {
                _notyf.Error("First Upload Files");
                return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });
            }
            bool isUploaded = _adminService.UploadFiles(model.uploadedFiles, rid);
            if (isUploaded)
            {
                _notyf.Success("Uploaded Successfully");
                return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });
            }
            else
            {
                _notyf.Error("Upload Failed");
                return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });
            }
        }

        public IActionResult DeleteFileById(int id)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteFileById(id);
            if (isDeleted)
            {
                return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });
            }
            else
            {
                _notyf.Error("SomeThing Went Wrong");
                return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });
            }
        }

        public IActionResult DeleteAllFiles(List<string> selectedFiles)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteAllFiles(selectedFiles, rid);
            if (isDeleted)
            {
                _notyf.Success("Deleted Successfully");
                return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });
            }
            _notyf.Error("SomeThing Went Wrong");
            return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });

        }

        public IActionResult SendAllFiles(List<string> selectedFiles)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");


            //var message = string.Join(", ", selectedFiles);
            SendEmail("parthtrivedi0710@gmail.com", "Documents", selectedFiles);
            _notyf.Success("Send Mail Successfully");
            return RedirectToAction("Pviewuploads", "Provider", new { reqId = rid });
        }

        public Task SendEmail(string email, string subject, List<string> filenames)
        {
            var mail = "tatva.dotnet.parthtrivedi@outlook.com";
            var password = "Parth@70160";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };

            MailMessage mailMessage = new MailMessage();
            for (var i = 0; i < filenames.Count; i++)
            {
                string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", filenames[i]);
                Attachment attachment = new Attachment(pathname);
                mailMessage.Attachments.Add(attachment);
            }
            mailMessage.To.Add(email);
            mailMessage.From = new MailAddress(mail);

            mailMessage.Subject = subject;


            return client.SendMailAsync(mailMessage);
        }


        [HttpGet]
        public IActionResult SendAgreement(int reqId, int reqType)
        {
            var model = _adminService.SendAgreementCase(reqId);
            model.reqType = reqType;
            return PartialView("_Psendagreement", model);
        }

        [HttpPost]
        public IActionResult SendAgreement(SendAgreementModel model)
        {
            try
            {
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string reviewPathLink = baseUrl + Url.Action("ReviewAgreement", "Home", new { reqId = model.Reqid });

                SendEmail(model.Email, "Review Agreement", $"Hello, Review The Agreement Properly: {reviewPathLink}");
                return Json(new { isSend = true });

            }
            catch (Exception ex)
            {
                return Json(new { isSend = false });
            }
        }


        public IActionResult RequestAdmin()
        {
            return PartialView("_Prequestadmin");
        }

        [HttpPost]
        public IActionResult RequestAdmin(RequestAdmin model)
        {
            try
            {
                var email = GetTokenEmail();
                _providerService.RequestAdmin(model, email);
                _notyf.Success("Email Send Successfully");
                return Ok();
            }
            catch
            {
                return NotFound();
            }


        }

        [HttpGet]
        public IActionResult orders(int reqId)
        {
            var order = _adminService.FetchProfession();
            order.ReqId = reqId;
            return View(order);
        }

      

     
        public string GetTokenEmail()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (token == null || !_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                return "";
            }
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }
        public string GetLoginId()
        {
            var token = HttpContext.Request.Cookies["jwt"];
            if (token == null || !_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                return "";
            }
            var loginId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "aspNetUserId");
            return loginId.Value;
        }

        public IActionResult Scheduling(SchedulingViewModel model)
        {

            model.regions = _adminService.RegionTable().ToList();
            return PartialView("_Pmyscheduling", model);
        }
        public IActionResult LoadSchedulingPartial(string date, int regionid, int status)
        {
            var aspnetuserid = GetLoginId();
            var month = _providerService.PhysicianMonthlySchedule(date, status, aspnetuserid);
            return PartialView("_Pmonthlyschedule", month);

        }

        [HttpPost]
        public IActionResult AddShift(SchedulingViewModel model, List<int> repeatdays)
        {
            var email = GetTokenEmail();

            //var email = User.FindFirstValue(ClaimTypes.Email);
            var isAdded = _adminService.CreateShift(model, email, repeatdays);
            return Json(new { isAdded });
        }

        public IActionResult ViewShift(int ShiftDetailId)
        {
            var data = _adminService.ViewShift(ShiftDetailId);
            return View("_Pviewshift", data);
        }

        [HttpGet]
        public IActionResult MyProfile()
        {
            var userid = GetLoginId();
            var tokenemail = GetTokenEmail();
            int phyId = _providerService.GetPhysicianId(userid);
            EditProviderModel2 model = new EditProviderModel2();
            model.editPro = _adminService.EditProviderProfile(phyId, tokenemail);
            model.regions = _adminService.RegionTable();
            model.physicianregiontable = _adminService.PhyRegionTable(phyId);
            model.roles = _adminService.GetPhyRoles();
            return PartialView("_Pmyprofile", model);

        }


        [HttpPost]
        public IActionResult providerEditFirst(string password, int phyId, string email)
        {
            bool editProvider = _adminService.providerResetPass(email, password);
            return Json(new { indicate = editProvider, phyId = phyId });
        }
        

        public IActionResult pcaremodal(int reqId)
        {
            ViewBag.reqid = reqId;
            return PartialView("_Pcaremodel");
        }
        [HttpPost]
        public IActionResult EncounterTypeModalSubmit(int requestId, short encounterType)
        {
            _providerService.CallType(requestId, encounterType);
            return Ok();
        }
        [HttpGet]
   
        public IActionResult PEncounterForm(int reqId)
        {
            ViewBag.reqId = reqId;
            var form = _adminService.EncounterForm(reqId);
            return View(form);
        }
        [HttpPost]
        public IActionResult PEncounterForm(EncounterFormModel model)
        {
            bool isSaved = _adminService.SubmitEncounterForm(model);
            if (isSaved)
            {
                _notyf.Success("Saved!!");
            }
            else
            {
                _notyf.Error("Failed");
            }
            return RedirectToAction("PEncounterForm", new { ReqId = model.reqid });
        }



        [HttpPost]
        public IActionResult HouseCallSubmit(int requestId)
        {
            _providerService.housecall(requestId);
            return RedirectToAction("provider_dashboard");
        }

        public IActionResult Finalizesubmit(int reqid)
        {
            bool isFinalized = _providerService.finalizesubmit(reqid);
            return Json(new { isFinalized });
        }
        public IActionResult DownloadEncounterPopUp(int reqId)
        {
            ViewBag.reqId = reqId;
            return PartialView("_Pdownloadmodel");
        }
        public IActionResult DownloadEncounterPDF([FromQuery] int reqId)
        {
            var data = _adminService.EncounterForm(reqId);
            return new ViewAsPdf("pdfpartial", data)
            {
                FileName = "EncounterForm.pdf"
            };
            //return PartialView("_PConcludeRequest");
        }

        public IActionResult ConcludeCare(int reqId)
        {
            HttpContext.Session.SetInt32("rid", reqId);
            var model = _adminService.GetAllDocById(reqId);
            return View(model);
        }

        [HttpPost]
        public IActionResult CUploadFiles(ViewUploadModel model)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            if (model.uploadedFiles == null)
            {
                _notyf.Error("First Upload Files");
                return RedirectToAction("concludecare", "Provider", new { reqId = rid });
            }
            bool isUploaded = _adminService.UploadFiles(model.uploadedFiles, rid);
            if (isUploaded)
            {
                _notyf.Success("Uploaded Successfully");
                return RedirectToAction("concludecare", "Provider", new { reqId = rid });
            }
            else
            {
                _notyf.Error("Upload Failed");
                return RedirectToAction("concludecare", "Provider", new { reqId = rid });
            }
        }

        public IActionResult CDeleteFileById(int id)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteFileById(id);
            if (isDeleted)
            {
                return RedirectToAction("concludecare", "Provider", new { reqId = rid });
            }
            else
            {
                _notyf.Error("SomeThing Went Wrong");
                return RedirectToAction("concludecare", "Provider", new { reqId = rid });
            }
        }

        public IActionResult CDeleteAllFiles(List<string> selectedFiles)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteAllFiles(selectedFiles, rid);
            if (isDeleted)
            {
                _notyf.Success("Deleted Successfully");
                return RedirectToAction("concludecare", "Provider", new { reqId = rid });
            }
            _notyf.Error("SomeThing Went Wrong");
            return RedirectToAction("concludecare", "Provider", new { reqId = rid });

        }

        [HttpPost]
        public IActionResult ConcludeCareSubmit(int ReqId, string ProviderNote)
        {
            _providerService.concludecaresubmit(ReqId, ProviderNote);
            return RedirectToAction("provider_dashboard");
        }

        [HttpGet]
        public IActionResult Pcreaterequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Pcreaterequest(CreateRequestModel model)
        {
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (token == null || !_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                _notyf.Error("Token Expired,Login Again");
                return View(model);
            }
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string createAccountLink = baseUrl + Url.Action("create_account", "Patient");
            var email = GetTokenEmail();
            var isSaved = _adminService.CreateRequest(model, email, createAccountLink);
            if (isSaved)
            {
                _notyf.Success("Request Created");
                return RedirectToAction("admin-dashboard");
            }
            else
            {
                _notyf.Error("Failed To Create");
                return View(model);
            }
        }

        public string ExportReq(int tabNo)
        {
            var reqList = _adminService.Export(tabNo);


            StringBuilder stringbuild = new StringBuilder();

            string header = "\"No\"," + "\"Name\"," + "\"DateOfBirth\"," + "\"Requestor\"," +
                "\"RequestDate\"," + "\"Phone\"," + "\"Notes\"," + "\"Address\","
                 + "\"Physician\"," + "\"DateOfService\"," + "\"Region\"," +
                "\"Status\"," + "\"RequestTypeId\"," + "\"OtherPhone\"," + "\"Email\"," + "\"RequestId\"," + Environment.NewLine + Environment.NewLine;

            stringbuild.Append(header);
            int count = 1;

            foreach (var item in reqList)
            {
                string content = $"\"{count}\"," + $"\"{item.firstName}\"," + $"\"{item.intDate}\"," + $"\"{item.requestorFname}\"," +
                    $"\"{item.intDate}\"," + $"\"{item.mobileNo}\"," + $"\"{item.notes}\"," + $"\"{item.street}\"," +
                    $"\"{item.lastName}\"," + $"\"{item.intDate}\"," + $"\"{item.street}\"," +
                    $"\"{item.status}\"," + $"\"{item.requestTypeId}\"," + $"\"{item.mobileNo}\"," + $"\"{item.firstName}\"," + $"\"{item.Reqid}\"," + Environment.NewLine;

                count++;
                stringbuild.Append(content);
            }

            string finaldata = stringbuild.ToString();

            return finaldata;

        }




    }
}