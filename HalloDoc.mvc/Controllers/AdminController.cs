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
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        //[HttpGet]
        //public IActionResult admin_login()
        //{
        //    return View();
        //}


        ////[CustomAuthorize]
        //[HttpPost]
        //public IActionResult admin_login(AdminLoginModel adminLoginModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var aspnetuser = _adminService.GetAspnetuser(adminLoginModel.email);
        //        if (aspnetuser != null)
        //        {
        //            adminLoginModel.password = adminLoginModel.password;
        //            if (aspnetuser.Passwordhash == adminLoginModel.password)
        //            {
        //                var jwtToken = _jwtService.GetJwtToken(aspnetuser);
        //                Response.Cookies.Append("jwt", jwtToken);
        //                _notyf.Success("Logged In Successfully");
        //                return RedirectToAction("admin_dashboard", "Admin");
        //            }
        //            else
        //            {
        //                _notyf.Error("Password Is Incorrect");

        //                return View(adminLoginModel);
        //            }
        //        }
        //        _notyf.Error("Email Is Incorrect");
        //        return View(adminLoginModel);
        //    }
        //    return View(adminLoginModel);

        //}

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction("admin_login", "Home");
        }

        public IActionResult GetCount()
        {
            var statusCountModel = _adminService.GetStatusCount();
            return PartialView("_allrequest", statusCountModel);
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
                return PartialView("_tocloserequest", list);
            }
            else if (tabNo == 6)
            {
                return PartialView("_unpaidrequest", list);
            }
            return View();
        }

        public IActionResult GetRequest(int tabNo)
        {
            var list = _adminService.Export(tabNo);
            return Json(list);
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


        [CustomAuthorize("Admin")]
        public IActionResult admin_dashboard()
        {
            var email = GetTokenEmail();
            var model = _adminService.GetLoginDetail(email);
            return View(model);
        }



        public IActionResult admin_resetpassword()
        {
            return View();
        }




        [HttpPost]
        public IActionResult UpdateNotes(ViewNotesModel model)
        {
            int? reqId = HttpContext.Session.GetInt32("RNId");
            bool isUpdated = _adminService.UpdateAdminNotes(model.AdditionalNotes, (int)reqId, 1);
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

                SendEmail(model.Email, "Review Agreement", $"Hello, Review The Agreement Properly: {reviewPathLink}");
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
                _notyf.Error("Failed To Close");
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
            //var request = HttpContext.Request;
            //var token = request.Cookies["jwt"];
            //if (token == null || !_jwtService.ValidateToken(token, out JwtSecurityToken jwtToken))
            //{
            //    return Json("Token Expired");
            //}
            //var emailClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);

            var email = GetTokenEmail();
            var model = _adminService.MyProfile(email)
;
            model.adminregions = _adminService.AdminRegionTable(email)
;
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
                return RedirectToAction("admin_dashboard");
            }
            else
            {
                _notyf.Error("Failed To Create");
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
        public IActionResult ViewCase(int Requestclientid, int RequestTypeId)
        {
            var model = _adminService.ViewCaseViewModel(Requestclientid, RequestTypeId);

            return View(model);
        }


        public IActionResult RequestSupport()
        {
            return PartialView("_requestsupport");
        }

        

        public IActionResult FilterRegion(FilterModel filterModel)
        {
            var list = _adminService.GetRequestByRegion(filterModel);
            return PartialView("_newrequest", list);
        }

        public IActionResult FilterRegionPending(FilterModel filterModel)
        {
            var list = _adminService.GetRequestByRegion(filterModel);
            return PartialView("_pendingrequest", list);
        }

        public IActionResult FilterRegionActive(FilterModel filterModel)
        {
            var list = _adminService.GetRequestByRegion(filterModel);
            return PartialView("_activerequest", list);
        }

        public IActionResult FilterRegionConclude(FilterModel filterModel)
        {
            var list = _adminService.GetRequestByRegion(filterModel);
            return PartialView("_concluderequest", list);
        }

        public IActionResult FilterRegionToClose(FilterModel filterModel)
        {
            var list = _adminService.GetRequestByRegion(filterModel);
            return PartialView("_tocloserequest", list);
        }

        public IActionResult FilterRegionUnpaid(FilterModel filterModel)
        {
            var list = _adminService.GetRequestByRegion(filterModel);
            return PartialView("_unpaidrequest", list);
        }

        [HttpGet]
        public IActionResult ShowProvider()
        {
            ProviderModel2 model = new ProviderModel2();
            model.regions = _adminService.RegionTable();
            model.providerModels = _adminService.GetProvider();
            return PartialView("_provider", model);
        }

        [HttpGet]
        public IActionResult ProviderRegionFilter(int regionId)
        {

            ProviderModel2 model = new ProviderModel2();
            model.regions = _adminService.RegionTable();
            if (regionId == 0)
            {
                model.providerModels = _adminService.GetProvider();
            }
            else
            {
                model.providerModels = _adminService.GetProviderByRegion(regionId);
            }
            return PartialView("_provider", model);
        }


        [HttpGet]
        public IActionResult ProviderContactModal(int phyId)
        {

            var model = new ProviderModel();
            model.phyId = phyId;

            return PartialView("_contactprovider", model);
        }

        [HttpPost]
        public IActionResult ContactProviderEmail(int phyId, string msg, string type)
        {
            var email = GetTokenEmail();
            if (type == "sms")
            {
                var isSmsSend = _adminService.ProviderContactSms(phyId, msg, email);
                return Json(new { isSend = isSmsSend });
            }
            else if (type == "email")
            {
                var isSend = _adminService.ProviderContactEmail(phyId, msg, email);
                return Json(new { isSend = isSend });
            }
            else
            {
                var isSmsSend = _adminService.ProviderContactSms(phyId, msg, email);
                var isSend = _adminService.ProviderContactEmail(phyId, msg, email);
                return Json(new { isSend = isSend, isSmsSend = isSmsSend });
            }

        }

        public IActionResult ProviderCheckbox(int phyId)
        {
            var isStopped = _adminService.StopNotification(phyId);
            return Json(new { isStopped = isStopped });
        }

        public IActionResult EditProvider(int phyId)
        {
            var tokenemail = GetTokenEmail();
            if (tokenemail != null)
            {

                EditProviderModel2 model = new EditProviderModel2();
                model.editPro = _adminService.EditProviderProfile(phyId, tokenemail);
                model.regions = _adminService.RegionTable();
                model.physicianregiontable = _adminService.PhyRegionTable(phyId);
                model.roles = _adminService.GetPhyRoles();
                return PartialView("_editprovider", model);
            }
            _notyf.Error("Token Expired,Login Again");
            return RedirectToAction("admin_login","Home");
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



        [HttpGet]
        public IActionResult DeleteRole(int RoleId)
        {
            var isDeleted = _adminService.DeleteRole(RoleId);
            return Json(new { isDeleted = isDeleted });
        }

        public IActionResult GetEditAccess(int accounttypeid, int roleid)
        {
            var roledata = _adminService.GetEditAccessData(roleid);
            var Accounttype = _adminService.GetAccountType();
            var menu = _adminService.GetAccountMenu(accounttypeid, roleid);
            accessModel adminAccessCm = new accessModel
            {
                Aspnetroles = Accounttype,
                AccountMenu = menu,
                CreateAccountAccess = roledata,
            };
            return PartialView("_editaccessrole", adminAccessCm);
        }
        public IActionResult FilterEditRolesMenu(int accounttypeid, int roleid)
        {
            var menu = _adminService.GetAccountMenu(accounttypeid, roleid);
            var htmlcontent = "";
            foreach (var obj in menu)
            {
                htmlcontent += $"<div class='form-check form-check-inline px-2 mx-3'><input class='form-check-input d2class' {(obj.ExistsInTable ? "checked" : "")} name='AccountMenu' type='checkbox' id='{obj.menuid}' value='{obj.menuid}'/><label class='form-check-label' for='{obj.menuid}'>{obj.name}</label></div>";
            }
            return Content(htmlcontent);
        }

        [HttpPost]
        public IActionResult SetEditAccessAccount(accessModel adminAccessCm, List<int> AccountMenu)
        {
            var sessionEmail = GetTokenEmail();
            bool isEdited = _adminService.SetEditAccessAccount(adminAccessCm.CreateAccountAccess, AccountMenu, sessionEmail);

            return Json(new { isEdited });
        }

        [HttpGet]
        public IActionResult CreateAccess()
        {
            var obj = _adminService.FetchRole(0);
            return PartialView("_createaccess", obj);
        }



        [HttpPost]
        public IActionResult CreateAccessPost(List<int> MenuIds, string RoleName, short AccountType)
        {
            var isRoleExists = _adminService.RoleExists(RoleName, AccountType);
            if (isRoleExists)
            {
                return Json(new { isRoleExists = true });
            }

            else
            {
                var isCreated = _adminService.CreateRole(MenuIds, RoleName, AccountType);
                return Json(new { isCreated = isCreated });
            }

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
        public IActionResult CreateAdminAccount()
        {
            CreateAdminAccount obj = new CreateAdminAccount();
            obj.RegionList = _adminService.RegionTable();
            obj.roles = _adminService.GetAdminRoles();
            return PartialView("_createadminaccount", obj);
        }


        [HttpPost]
        public IActionResult AdminAccount(CreateAdminAccount model, List<int> AdminRegion)
        {
            var email = GetTokenEmail();
            var isCreated = _adminService.CreateAdminAccount(model, AdminRegion, email);
            return Json(new { isCreated });
        }


        [HttpGet]
        public IActionResult ShowUserAccess(short selectedValue)
        {
            var obj = _adminService.FetchAccess(selectedValue);
            return PartialView("_useraccess", obj);
        }

        public IActionResult EditUserAccessAdmin(int adminid)
        {
            CreateAdminAccount data = new();
            //data._providerEdit = _IAdminDash.adminEditPage(adminId);
            data.adminRegions = _adminService.AdminRegionTableById(adminid);
            data.regions = _adminService.RegionTable();
            data.roles = _adminService.GetAdminRoles();

            return View("_edituseraccessadmin", data);
        }

        [HttpPost]
        public IActionResult EditAdminAccount(CreateAdminAccount model, List<int> AdminRegion)
        {
            var email = GetTokenEmail();
            var isEdited = _adminService.EditAdminDetailsDb(model, email, AdminRegion);
            return Json(new { isEdited });
        }
        public IActionResult EditUserAccessPhysician(int phyid)
        {
            var tokenemail = GetTokenEmail();
            EditProviderModel2 model = new EditProviderModel2();
            model.editPro = _adminService.EditProviderProfile(phyid, tokenemail);
            model.regions = _adminService.RegionTable();
            model.physicianregiontable = _adminService.PhyRegionTable(phyid);
            model.roles = _adminService.GetPhyRoles();
            return PartialView("_edituseraccessphysician", model);
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






        public IActionResult Scheduling(SchedulingViewModel model)
        {

            model.regions = _adminService.RegionTable().ToList();
            return PartialView("_scheduling", model);
        }

        public IActionResult LoadSchedulingPartial(string PartialName, string date, int regionid, int status)
        {
            if (PartialName == "_daytable")
            {
                var day = _adminService.GetDayTable(PartialName, date, regionid, status);
                return PartialView("_daytable", day);

            }

            else if (PartialName == "_weektable")
            {
                var week = _adminService.GetWeekTable(date, regionid, status);
                return PartialView("_weektable", week);
            }
            else if (PartialName == "_monthtable")
            {
                var month = _adminService.GetMonthTable(date, regionid, status);
                return PartialView("_monthtable", month);
            }
            else
            {
                var day = _adminService.GetDayTable(PartialName, date, regionid, status);
                return PartialView("_daytable", day);
            }
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
            return View("_viewshift", data);
        }

        public IActionResult ReturnShift(int ShiftDetailId)
        {
            var email = GetTokenEmail();
            var isReturned = _adminService.ReturnShift(ShiftDetailId, email);
            return Json(new { isReturned });
        }

        public IActionResult DeleteShift(int ShiftDetailId)
        {
            var email = GetTokenEmail();
            var isDeleted = _adminService.DeleteShift(ShiftDetailId, email);
            return Json(new { isDeleted });
        }

        [HttpPost]
        public IActionResult EditViewShift(CreateNewShift model)
        {
            var email = GetTokenEmail();
            bool isEditted = _adminService.EditShift(model, email);

            return Json(new { isEditted });
        }

        public IActionResult MdOnCallData(int region)
        {
            var data = _adminService.GetOnCallDetails(region);
            return PartialView("_provideroncall", data);
        }
        public IActionResult ShiftReview(int regionId, int callId)
        {
            ShiftReview2 schedulingCm = new ShiftReview2()
            {
                regions = _adminService.RegionTable(),
                ShiftReview = _adminService.GetShiftReview(regionId, callId),
                regionId = regionId,
                callId = callId,
            };

            return PartialView("_shiftforreview", schedulingCm);
        }

        public IActionResult ApproveShift(int[] shiftDetailsId)
        {
            var Aspid = GetLoginId();
            bool isApproved = _adminService.ApproveSelectedShift(shiftDetailsId, Aspid);

            return Json(new { isApproved });
        }

        public IActionResult DeleteSelectedShift(int[] shiftDetailsId)
        {
            var Aspid = GetLoginId();

            bool isDeleted = _adminService.DeleteShiftReview(shiftDetailsId, Aspid);

            return Json(new { isDeleted });
        }


        public IActionResult CreateProviderAccount()
        {
            CreateProviderAccount data = new();
            data.regions = _adminService.RegionTable();
            data.roles = _adminService.GetPhyRoles();
            return PartialView("_createprovideraccount", data);
        }

        [HttpPost]
        public IActionResult CreateProviderAccount(CreateProviderAccount obj, List<int> physicianregions)
        {
            CreateProviderAccount data = new();
            var createprovideraccount = _adminService.CreateProviderAccount(obj, physicianregions);
            return Json(new { flag = createprovideraccount.flag });
        }




        public IActionResult BusinessTable(string vendor, string profession)
        {
            var obj = _adminService.BusinessTable(vendor, profession);
            return PartialView("_businesstable", obj);
        }

        public IActionResult Partners()
        {
            
            return PartialView("_partners");
        }

        [HttpPost]
        public IActionResult AddBusiness(AddBusinessModel obj)
        {
            if (obj.BusinessName != null && obj.FaxNumber != null)
            {
                _adminService.AddBusiness(obj);
                _notyf.Success("Save Data!!");
                return Ok();
            }
            else
            {
                _notyf.Error("Please Enter Data");
                return BadRequest();
            }


        }

        [HttpGet]
        public IActionResult SearchVendor(string vendor, string profession)
        {
            var obj = _adminService.BusinessTable(vendor, profession);
            return PartialView("_BusinessTable", obj);
        }

        public IActionResult AddVendor()
        {
            AddBusinessModel obj = new()
           {
                RegionList = _adminService.RegionTable(),
                ProfessionList = _adminService.GetProfession()
            };
            return PartialView("_addvendor", obj);
        }

        [HttpPost]
        public IActionResult AddVendor(AddBusinessModel obj)
        {
            if (obj.BusinessName != null && obj.FaxNumber != null)
            {
                bool isAdded = _adminService.AddBusiness(obj);

                return Json(new { isAdded = isAdded });
            }
            else
            {
                _notyf.Error("Please Enter  Data");
                return BadRequest();
            }

        }


        
        public IActionResult DeleteBusiness(int VendorId)
        {
            var isDeleted = _adminService.RemoveBusiness(VendorId);
            return Json(new { isDeleted = isDeleted });
        }

        public IActionResult EditBusinessData(int VendorId)
        {
            var obj = _adminService.GetEditBusiness(VendorId);
            return PartialView("_editbusiness", obj);
        }





        [HttpGet]
        public IActionResult SearchRecords(RecordsModel recordsModel)
        {
            RecordsModel model = new RecordsModel();
            model.requestListMain = _adminService.SearchRecords(recordsModel);
            if (model.requestListMain.Count() == 0)
            {
                RequestsRecordModel rec = new RequestsRecordModel();
                rec.flag = 1;
                model.requestListMain.Add(rec);
            }

            return PartialView("_searchrecord", model);


        }

        public IActionResult recordDltBtn(int reqId)
        {
            _adminService.DeleteRecords(reqId);
            _notyf.Success("Deleted Successfully!!");
            return Ok();
        }


        public IActionResult ScheduleExportAll(RecordsModel recordsModel)
        {
            var exportAll = _adminService.GenerateExcelFile(_adminService.SearchRecords(recordsModel));
            return File(exportAll, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Requests.xlsx");
        }







        [HttpGet]
        public IActionResult PatientRecords(PatientRecordsModel patientRecordsModel)
        {
            PatientRecordsModel model = new PatientRecordsModel();
            model.users = _adminService.PatientRecords(patientRecordsModel);

            if (model.users.Count() == 0)
            {
                model.flag = 1;
            }

            return PartialView("_patientrecord", model);
        }

        public IActionResult GetPatientRecordExplore(int userId)
        {
            
            var _data = _adminService.GetPatientRecordExplore(userId);

            return PartialView("_patientrecordexplore", _data);
        }

        [HttpGet]
        public IActionResult EmailLogs(EmailSmsRecords2 recordsModel)
        {
            EmailSmsRecords2 _data = new EmailSmsRecords2();
            _data = _adminService.EmailSmsLogs((int)recordsModel.tempid, recordsModel);
            return PartialView("_emaillogs", _data);
        }

        public IActionResult BlockedHistory(BlockHistory2 blockHistory2)
        {
            BlockHistory2 _data = new BlockHistory2();
            _data.blockHistories = _adminService.BlockHistory(blockHistory2);


            return PartialView("_blockhistory", _data);
        }


        public IActionResult unblockBlockHistory(int blockId)
        {
            bool isUnblocked = _adminService.UnblockRequest(blockId);
            return Json(new { isUnblocked = isUnblocked });
        }

        public IActionResult BlockHistoryCheckBox(int blockId)
        {
            bool isActive = _adminService.IsBlockRequestActive(blockId);
            return Json(new { isActive });
        }

       

    }
}