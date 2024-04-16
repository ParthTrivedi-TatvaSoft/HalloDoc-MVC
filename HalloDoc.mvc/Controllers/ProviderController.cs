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

            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("admin_login", "Home");
        }

        public IActionResult GetCount()
        {
            var statusCountModel = _adminService.GetStatusCount();
            return PartialView("_Pallrequest", statusCountModel);
        }
        public IActionResult GetRequestsByStatus(int tabNo, int CurrentPage)
        {
            var list = _adminService.GetRequestsByStatus(tabNo, CurrentPage);

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

        public IActionResult FilterRegion(FilterModel filterModel)
        {
            var list = _adminService.GetRequestByRegion(filterModel);
            return PartialView("_Pnewrequest", list);
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
            return PartialView("_sendlink");
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

        public IActionResult ViewCase(int Requestclientid, int RequestTypeId ,int ReqId)
        {
            var model = _adminService.ViewCase(Requestclientid, RequestTypeId ,ReqId);

            return View("_Pviewcase", model);
        }
            
        [HttpPost]
        public IActionResult UpdateNotes(ViewNotesModel model)
        {
            bool isUpdated = _adminService.UpdateAdminNotes(model.AdditionalNotes, model.ReqId, 2);

            return Json(new { isUpdated, reqId = model.ReqId });
        }

        public IActionResult ViewNote(int ReqId)
        {

            ViewNotesModel data = _adminService.ViewNotes(ReqId);
            return PartialView("_Pviewnotes", data);
        }

        public IActionResult CancelCase(int reqId)
        {

            var model = _adminService.CancelCase(reqId);
            model.reqId = reqId;
            return PartialView("_cancelcase", model);
        }


        [HttpPost]
        public IActionResult SubmitCancelCase(CancelCaseModel cancelCaseModel, int reqId)
        {
            cancelCaseModel.reqId = reqId;
            bool isCancelled = _adminService.SubmitCancelCase(cancelCaseModel);

            if (isCancelled)
            {
                _notyf.Success("Cancelled successfully");
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


        public IActionResult ViewUploads(int reqId)
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
                return RedirectToAction("viewuploads", "Provider", new { reqId = rid });
            }
            bool isUploaded = _adminService.UploadFiles(model.uploadedFiles, rid);
            if (isUploaded)
            {
                _notyf.Success("Uploaded Successfully");
                return RedirectToAction("viewuploads", "Provider", new { reqId = rid });
            }
            else
            {
                _notyf.Error("Upload Failed");
                return RedirectToAction("viewuploads", "Provider", new { reqId = rid });
            }
        }

        public IActionResult DeleteFileById(int id)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteFileById(id);
            if (isDeleted)
            {
                return RedirectToAction("viewuploads", "Provider", new { reqId = rid });
            }
            else
            {
                _notyf.Error("SomeThing Went Wrong");
                return RedirectToAction("viewuploads", "Provider", new { reqId = rid });
            }
        }

        public IActionResult DeleteAllFiles(List<string> selectedFiles)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteAllFiles(selectedFiles, rid);
            if (isDeleted)
            {
                _notyf.Success("Deleted Successfully");
                return RedirectToAction("viewuploads", "Provider", new { reqId = rid });
            }
            _notyf.Error("SomeThing Went Wrong");
            return RedirectToAction("viewuploads", "Provider", new { reqId = rid });

        }

        public IActionResult SendAllFiles(List<string> selectedFiles)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");


            //var message = string.Join(", ", selectedFiles);
            SendEmail("parthtrivedi0710@gmail.com", "Documents", selectedFiles);
            _notyf.Success("Send Mail Successfully");
            return RedirectToAction("viewuploads", "Provider", new { reqId = rid });
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
        public IActionResult Order(int reqId)
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


        [HttpGet]
        public IActionResult ShowMyProfile()
        {
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (token == null || !_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                return Json("Token Expired");
            }
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            var model = _adminService.MyProfile(emailClaim.Value);
            return PartialView("_myprofile", model);
        }

        public IActionResult ProviderProfile(int phyId)
        {
            var tokenemail = GetTokenEmail();
            if (tokenemail != null)
            {

                EditProviderModel2 model = new EditProviderModel2();
                model.editPro = _adminService.EditProviderProfile(phyId, tokenemail);
                model.regions = _adminService.RegionTable();
                model.physicianregiontable = _adminService.PhyRegionTable(phyId);
                model.roles = _adminService.GetRoles();
                return PartialView("_editprovider", model);
            }
            _notyf.Error("Token Expired,Login Again");
            return RedirectToAction("admin_login", "Home");
        }


        [HttpPost]
        public IActionResult providerEditFirst(string password, int phyId, string email)
        {
            bool editProvider = _adminService.providerResetPass(email, password);
            return Json(new { indicate = editProvider, phyId = phyId });
        }
        [HttpPost]
        public IActionResult editProviderForm1(int phyId, int roleId, int statusId)
        {
            bool editProviderForm1 = _adminService.editProviderForm1(phyId, roleId, statusId);
            return Json(new { indicate = editProviderForm1, phyId = phyId });
        }
        [HttpPost]
        public IActionResult editProviderForm2(string fname, string lname, string email, string phone, string medical, string npi, string sync, int phyId, int[] phyRegionArray)
        {
            bool editProviderForm2 = _adminService.editProviderForm2(fname, lname, email, phone, medical, npi, sync, phyId, phyRegionArray);
            return Json(new { indicate = editProviderForm2, phyId = phyId });
        }
        [HttpPost]
        public IActionResult editProviderForm3(EditProviderModel2 payloadMain)
        {
            bool editProviderForm3 = _adminService.editProviderForm3(payloadMain);
            return Json(new { indicate = editProviderForm3, phyId = payloadMain.editPro.PhyID });
        }
        [HttpPost]
        public IActionResult PhysicianBusinessInfoEdit(EditProviderModel2 payloadMain)
        {
            bool editProviderForm4 = _adminService.PhysicianBusinessInfoUpdate(payloadMain);
            return Json(new { indicate = editProviderForm4, phyId = payloadMain.editPro.PhyID });


        }
        [HttpPost]
        public IActionResult UpdateOnBoarding(EditProviderModel2 payloadMain)
        {
            var editProviderForm5 = _adminService.EditOnBoardingData(payloadMain);
            return Json(new { indicate = editProviderForm5, phyId = payloadMain.editPro.PhyID });
        }
        public IActionResult editProviderDeleteAccount(int phyId)
        {
            _adminService.editProviderDeleteAccount(phyId);
            return Ok();
        }
    }
}