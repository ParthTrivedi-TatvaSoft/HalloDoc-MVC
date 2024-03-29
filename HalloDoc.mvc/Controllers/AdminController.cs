using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.CustomModels;
using DataAccess.Models;
using HalloDoc.mvc.Auth;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Text.Json.Nodes;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc.mvc.Controllers
{


    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly INotyfService _notyf;
        private readonly IAdminService _adminService;
        private readonly IPatientService _patientService;
        private readonly IJwtService _jwtService;

        public AdminController(ILogger<AdminController> logger, INotyfService notyfService, IAdminService adminService, IPatientService patientService, IJwtService jwtService)
        {
            _logger = logger;
            _notyf = notyfService;
            _adminService = adminService;
            _patientService = patientService;
            _jwtService = jwtService;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[CustomAuthorize]
        public IActionResult admin_login(AdminLoginModel adminLoginModel)
        {
            if (ModelState.IsValid)
            {
                var aspnetuser = _adminService.GetAspnetuser(adminLoginModel.email);
                if (aspnetuser != null)
                {
                    adminLoginModel.password = adminLoginModel.password;
                    if (aspnetuser.Passwordhash == adminLoginModel.password)
                    {
                        var jwtToken = _jwtService.GetJwtToken(aspnetuser);
                        Response.Cookies.Append("jwt", jwtToken);
                        _notyf.Success("Logged In Successfully");
                        return RedirectToAction("admin_dashboard", "Admin");
                    }
                    else
                    {
                        _notyf.Error("Password is incorrect");

                        return View(adminLoginModel);
                    }
                }
                _notyf.Error("Email is incorrect");
                return View(adminLoginModel);
            }
            return View(adminLoginModel);

        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("admin_login", "Admin");
        }

        public IActionResult GetCount()
        {
            var statusCountModel = _adminService.GetStatusCount();
            return PartialView("_allrequest", statusCountModel);
        }


        public IActionResult GetRequestsByStatus(int tabNo, int CurrentPage)
        {
            var list = _adminService.GetRequestsByStatus(tabNo, CurrentPage);
            if (tabNo == 1)
            {
                return PartialView("_newrequest", list);
            }
            else if (tabNo == 2)
            {
                return PartialView("_pendingrequest", list);
            }
            else if (tabNo == 3)
            {
                return PartialView("_activerequest", list);
            }
            else if (tabNo == 4)
            {
                return PartialView("_concluderequest", list);
            }
            else if (tabNo == 5)
            {
                return PartialView("_toCloserequest", list);
            }
            else if (tabNo == 6)
            {
                return PartialView("_unpaidrequest", list);
            }
            else if (tabNo == 0)
            {
                return Json(list);
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
                "\"Status\"," + "\"RequestTypeId\"," + "\"OtherPhone\"," + "\"Email\"," + Environment.NewLine + Environment.NewLine;

            stringbuild.Append(header);
            int count = 1;

            foreach (var item in reqList)
            {
                string content = $"\"{count}\"," + $"\"{item.firstName}\"," + $"\"{item.intDate}\"," + $"\"{item.requestorFname}\"," +
                    $"\"{item.intDate}\"," + $"\"{item.mobileNo}\"," + $"\"{item.notes}\"," + $"\"{item.street}\"," +
                    $"\"{item.lastName}\"," + $"\"{item.intDate}\"," + $"\"{item.street}\"," +
                    $"\"{item.status}\"," + $"\"{item.requestTypeId}\"," + $"\"{item.mobileNo}\"," + $"\"{item.firstName}\"," + Environment.NewLine;

                count++;
                stringbuild.Append(content);
            }

            string finaldata = stringbuild.ToString();

            return finaldata;

        }

        
        [CustomAuthorize("Admin")]
        public IActionResult admin_dashboard()
        {
            return View();
        }

        


        public IActionResult admin_resetpassword()
        {
            return View();
        }




        [HttpPost]
        public IActionResult UpdateNotes(ViewNotesModel model)
        {
            int? reqId = HttpContext.Session.GetInt32("RNId");
            bool isUpdated = _adminService.UpdateAdminNotes(model.AdditionalNotes, (int)reqId);
            if (isUpdated)
            {
                _notyf.Success("Saved Changes !!");
                return RedirectToAction("viewnotes", "Admin", new { ReqId = reqId });

            }
            return View();
        }

        public IActionResult viewnotes(int ReqId)
        {
            HttpContext.Session.SetInt32("RNId", ReqId);
            ViewNotesModel data = _adminService.ViewNotes(ReqId);
            return View(data);
        }

        public IActionResult CancelCase(int reqId)
        {

            var model = _adminService.CancelCase(reqId);
            model.reqId = reqId;
            return PartialView("_canclecase", model);
        }


        [HttpPost]
        public IActionResult SubmitCancelCase(CancelCaseModel cancelCaseModel, int reqId)
        {
            cancelCaseModel.reqId = reqId;
            bool isCancelled = _adminService.SubmitCancelCase(cancelCaseModel);

            if (isCancelled)
            {
                _notyf.Success("Cancelled successfully");
                return RedirectToAction("admin_dashboard");
            }
            _notyf.Error("Cancellation Failed");
            return RedirectToAction("admin_dashboard");
        }


        public IActionResult AssignCase(int reqId)
        {
            HttpContext.Session.SetInt32("AssignReqId", reqId);
            var model = _adminService.AssignCase(reqId);
            return PartialView("_assigncase", model);
        }

        [HttpPost]
        public IActionResult SubmitAssignCase(AssignCaseModel assignCaseModel)
        {
            assignCaseModel.ReqId = HttpContext.Session.GetInt32("AssignReqId");
            bool isAssigned = _adminService.SubmitAssignCase(assignCaseModel);
            if (isAssigned)
            {
                _notyf.Success("Assigned successfully");
                return RedirectToAction("admin_dashboard", "Admin");
            }
            return View();
        }




        public IActionResult GetPhysician(int selectRegion)
        {
            List<Physician> physicianlist = _adminService.GetPhysicianByRegion(selectRegion);
            return Json(new { physicianlist });
        }

   


        public IActionResult BlockCase(int reqId)
        {
            HttpContext.Session.SetInt32("BlockReqId", reqId);
            var model = _adminService.BlockCase(reqId);
            return PartialView("_BlockCase", model);
        }

        [HttpPost]
        public IActionResult SubmitBlockCase(BlockCaseModel blockCaseModel)
        {
            blockCaseModel.ReqId = HttpContext.Session.GetInt32("BlockReqId");
            bool isBlocked = _adminService.SubmitBlockCase(blockCaseModel);
            if (isBlocked)
            {
                _notyf.Success("Blocked Successfully");
                return RedirectToAction("admin_dashboard", "Admin");
            }

            _notyf.Error("BlockCase Failed");
            return RedirectToAction("admin_dashboard", "Admin");
        }

        [HttpGet]
        public IActionResult TransferCase(int reqId)
        {
            var model = _adminService.AssignCase(reqId);
            model.ReqId = reqId;
            return PartialView("_transfer", model);
        }

        [HttpPost]
        public IActionResult SubmitTransferCase(AssignCaseModel transferCaseModel)
        {
            bool isTransferred = _adminService.SubmitAssignCase(transferCaseModel);
            return Json(new { isTransferred = isTransferred });
        }


        [HttpPost]
        public IActionResult orders(Order order)
        {
            bool isSend = _adminService.SendOrder(order);
            return Json(new { isSend = isSend });
        }


        [HttpGet]
        public IActionResult orders(int reqId)
        {
            var order = _adminService.FetchProfession();
            order.ReqId = reqId;
            return View(order);
        }


        [HttpGet]
        public IActionResult ClearCase(int reqId)
        {
            ViewBag.ClearCaseId = reqId;
            return PartialView("_clearcase");
        }

        [HttpPost]
        public IActionResult SubmitClearCase(int reqId)
        {
            bool isClear = _adminService.ClearCase(reqId);
            if (isClear)
            {
                _notyf.Success("Cleared Successfully");
                return RedirectToAction("admin_dashboard");
            }
            _notyf.Error("Failed");
            return RedirectToAction("admin_dashboard");
        }

        [HttpGet]
        public IActionResult SendAgreement(int reqId, int reqType)
        {
            var model = _adminService.SendAgreementCase(reqId);
            model.reqType = reqType;
            return PartialView("_sendagreement", model);
        }


        public IActionResult SendAllFiles(List<string> selectedFiles)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");


            var message = string.Join(", ", selectedFiles);

            _notyf.Success("Send Mail Successfully");


            var mail = "tatva.dotnet.parthtrivedi@outlook.com";
            var password = "Parth@70160";

            var client = new SmtpClient("smtp.office365.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, password)
            };
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("tatva.dotnet.parthtrivedi@outlook.com"),
                Subject = "Document List",
                IsBodyHtml = true,

            };
            foreach (var item in selectedFiles)
            {

                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles");
                path = Path.Combine(path, item);
                Attachment attachment = new Attachment(path);
                mailMessage.Attachments.Add(attachment);
            }
            //RequestClient requestClient = _requestClientRepository.GetRequestClientByRequestId(requestId);
            //mailMessage.To.Add(requestClient.Email);
            mailMessage.To.Add("parthtrivedi0710@gmail.com");

            client.SendMailAsync(mailMessage);

            return RedirectToAction("viewuploads", "Admin", new { reqId = rid });
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


        [HttpPost]
        public IActionResult SendAgreement(SendAgreementModel model)
        {
            try
            {
                string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                string reviewPathLink = baseUrl + Url.Action("ReviewAgreement", "Home",new {reqId=model.Reqid});

                SendEmail(model.Email, "Review Agreement", $"Hello, Review the agreement properly: {reviewPathLink}");
                return Json(new { isSend = true });

            }
            catch (Exception ex)
            {
                return Json(new { isSend = false });
            }
        }

        public IActionResult CloseCase(int ReqId)
        {
            var model = _adminService.ShowCloseCase(ReqId);
            return View(model);
        }

        [HttpPost]
        public IActionResult CloseCase(CloseCaseModel model)
        {

            bool isSaved = _adminService.SaveCloseCase(model);
            if (isSaved)
            {
                _notyf.Success("Saved");
            }
            else
            {
                _notyf.Error("Failed");
            }
            return RedirectToAction("closecase", new { ReqId = model.reqid });
        }

        public IActionResult SubmitCloseCase(int ReqId)
        {
            bool isClosed = _adminService.SubmitCloseCase(ReqId);
            if (isClosed)
            {
                _notyf.Success("Closed Successfully");
                return RedirectToAction("admin_dashboard");
            }
            else
            {
                _notyf.Error("Failed to close");
                return RedirectToAction("closecase", new { ReqId = ReqId });
            }
        }




        public IActionResult viewuploads(int reqId)
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
                return RedirectToAction("viewuploads", "Admin", new { reqId = rid });
            }
            bool isUploaded = _adminService.UploadFiles(model.uploadedFiles, rid);
            if (isUploaded)
            {
                _notyf.Success("Uploaded Successfully");
                return RedirectToAction("viewuploads", "Admin", new { reqId = rid });
            }
            else
            {
                _notyf.Error("Upload Failed");
                return RedirectToAction("viewuploads", "Admin", new { reqId = rid });
            }
        }

        public IActionResult DeleteFileById(int id)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteFileById(id);
            if (isDeleted)
            {
                return RedirectToAction("viewuploads", "Admin", new { reqId = rid });
            }
            else
            {
                _notyf.Error("SomeThing Went Wrong");
                return RedirectToAction("viewuploads", "Admin", new { reqId = rid });
            }
        }

        public IActionResult DeleteAllFiles(List<string> selectedFiles)
        {
            var rid = (int)HttpContext.Session.GetInt32("rid");
            bool isDeleted = _adminService.DeleteAllFiles(selectedFiles, rid);
            if (isDeleted)
            {
                _notyf.Success("Deleted Successfully");
                return RedirectToAction("viewuploads", "Admin", new { reqId = rid });
            }
            _notyf.Error("SomeThing Went Wrong");
            return RedirectToAction("viewuploads", "Admin", new { reqId = rid });

        }

      

        [HttpGet]
        public JsonArray FetchBusiness(int proffesionId)
        {
            var result = _adminService.FetchVendors(proffesionId);
            return result;
        }

        [HttpGet]
        public Healthprofessional VendorDetails(int selectedValue)
        {
            var result = _adminService.VendorDetails(selectedValue);
            return result;
        }


        [HttpGet]
        public IActionResult Encounter(int ReqId)
        {
            var model = _adminService.EncounterForm(ReqId);
            return View(model);
        }

        [HttpPost]
        public IActionResult Encounter(EncounterFormModel model)
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
            return RedirectToAction("Encounter", new { ReqId = model.reqid });
        }


        [HttpGet]
        public IActionResult ShowMyProfile()
        {
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (token == null || !_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                return Json("token expired");
            }
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            var model = _adminService.MyProfile(emailClaim.Value);
            return PartialView("_myprofile", model);
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
        [HttpPost]
        public IActionResult ResetPassword(string resetPassword)
        {
            var tokenEmail = GetTokenEmail();
            if (tokenEmail != "")
            {
                bool isReset = _adminService.ResetPassword(tokenEmail, resetPassword);
                return Json(new { isReset = isReset });
             
            }
            return Json(new { isReset = false });
        }

        [HttpPost]
        public IActionResult SubmitAdminInfo(MyProfileModel model)
        {
            var tokenEmail = GetTokenEmail();
            if (tokenEmail != "")
            {
                bool isSubmit = _adminService.SubmitAdminInfo(model, tokenEmail);
                return Json(new { isSubmit = isSubmit });
            }
            return Json(new { isSubmit = false });
        }

        [HttpPost]
        public IActionResult SubmitBillingInfo(MyProfileModel model)
        {
            var tokenEmail = GetTokenEmail();
            if (tokenEmail != "")
            {
                var isRegionExists = _adminService.VerifyState(model.state);
                if (isRegionExists)
                {
                    bool isSubmit = _adminService.SubmitBillingInfo(model, tokenEmail);
                    return Json(new { isSubmit = isSubmit, isRegionExists = isRegionExists });
                }
                else
                {
                    return Json(new { isRegionExists = isRegionExists });
                }
            }
            return Json(new { isSubmit = false });
        }

        [HttpGet]
        public IActionResult createrequest()
        {
            return View();
        }

        [HttpGet]
        public IActionResult VerifyState(string stateMain)
        {
            if (stateMain == null || stateMain.Trim() == null)
            {
                return Json(new { isSend = false });
            }
            var isSend = _adminService.VerifyState(stateMain);
            return Json(new { isSend = isSend });
        }


        [HttpPost]
        public IActionResult createrequest(CreateRequestModel model)
        {
            var request = HttpContext.Request;
            var token = request.Cookies["jwt"];
            if (token == null || !_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            {
                _notyf.Error("Token Expired");
                return View(model);
            }
            var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            var isSaved = _adminService.CreateRequest(model, emailClaim.Value);
            if (isSaved)
            {
                _notyf.Success("Request Created");
                return RedirectToAction("admin_dashboard");
            }
            else
            {
                _notyf.Error("Failed to Create");
                return View(model);
            }
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

                SendEmail(model.email, "Create Patient Request", $"Hello, please create patient request from this link: {reviewPathLink}");
                _notyf.Success("Link Sent");
                isSend = true;
            }
            catch (Exception ex)
            {
                _notyf.Error("Failed to sent");
            }
            return Json(new { isSend = isSend });

        }
        public IActionResult viewcase(int Requestclientid, int RequestTypeId)
        {
            var model = _adminService.ViewCaseViewModel(Requestclientid, RequestTypeId);

            return View(model);
        }



        public IActionResult RequestSupport()
        {
            return PartialView("_requestsupport");
        }


        public IActionResult FilterRegion(int regionId, int tabNo)
        {
            var list = _adminService.GetRequestByRegion(regionId, tabNo);
            return PartialView("_newrequest", list);
        }

        public IActionResult FilterRegionPending(int regionId, int tabNo)
        {
            var list = _adminService.GetRequestByRegion(regionId, tabNo);
            return PartialView("_pendingrequest", list);
        }

        public IActionResult FilterRegionActive(int regionId, int tabNo)
        {
            var list = _adminService.GetRequestByRegion(regionId, tabNo);
            return PartialView("_activerequest", list);
        }

        public IActionResult FilterRegionConclude(int regionId, int tabNo)
        {
            var list = _adminService.GetRequestByRegion(regionId, tabNo);
            return PartialView("_concluderequest", list);
        }
        public IActionResult FilterRegionToClose(int regionId, int tabNo)
        {
            var list = _adminService.GetRequestByRegion(regionId, tabNo);
            return PartialView("_tocloserequest", list);
        }
        public IActionResult FilterRegionUnpaid(int regionId, int tabNo)
        {
            var list = _adminService.GetRequestByRegion(regionId, tabNo);
            return PartialView("_unpaidrequest", list);
        }

        public IActionResult ShowProvider()
        {
            var model = _adminService.GetProvider();
            return PartialView("_provider", model);
        }
        [HttpGet]
        public IActionResult DeleteRole(int RoleId)
        {
            var isDeleted = _adminService.DeleteRole(RoleId);
            return Json(new { isDeleted = isDeleted });
        }

        [HttpGet]
        public IActionResult ProviderContactModal(int phyId)
        {

            var model = new ProviderModel();
            model.phyId = phyId;

            return PartialView("_contactprovider", model);
        }

        [HttpPost]
        public IActionResult ContactProviderEmail(int phyId, string msg)
        {
            var isSend = _adminService.ProviderContactEmail(phyId, msg);
            return Json(new { isSend = isSend });
        }

        public IActionResult ProviderCheckbox(int phyId)
        {
            var isStopped = _adminService.StopNotification(phyId);
            return Json(new { isStopped = isStopped });
        }

        public IActionResult EditProvider()
        {
            EditProviderModel2 model = new EditProviderModel2();
            return PartialView("_editprovider", model);
        }

        [HttpGet]
        public IActionResult CreateAccess()
        {
            var obj = _adminService.FetchRole(0);
            return PartialView("_createaccess",obj);
        }


        [HttpPost]
        public IActionResult CreateAccessPost(List<int> MenuIds, string RoleName, short AccountType)
        {
                _adminService.CreateRole(MenuIds, RoleName, AccountType);
            return RedirectToAction("ShowAccountAccess");
        }

        [HttpGet]
        public CreateAccess FetchRoles(short selectedValue)
        {
            var obj = _adminService.FetchRole(selectedValue);
            return obj;
        }

        [HttpGet]
        public IActionResult ShowAccountAccess()
        {
            var obj = _adminService.AccountAccess();
            return PartialView("_accountaccess",obj);
        }

        [HttpGet]
        public IActionResult AdminAccount()
        {
            var obj = _adminService.RegionList();
            return PartialView("_createadminaccount",obj);
        }

        [HttpPost]
        public IActionResult AdminAccount(CreateAdminAccount model)
        {
            var email = GetTokenEmail();
            var isCreated = _adminService.CreateAdminAccount(model,email);
            if (isCreated)
            {
                _notyf.Success("Account created");
                return RedirectToAction("admin_dashboard");
            }
            else
            {
                _notyf.Error("Somethng Went Wrong!!");
                return PartialView("_createadminaccount");
            }
        }

        public IActionResult ShowUserAccess()
        {
            return PartialView("_useraccess");
        }

        public IActionResult ProviderLocation()
        {
            return PartialView("_providerlocation");
        }
        public IActionResult GetLocation()
        {
            List<Physicianlocation> getLocation = _adminService.GetPhysicianlocations();
            return Ok(getLocation);
        }




    }
}