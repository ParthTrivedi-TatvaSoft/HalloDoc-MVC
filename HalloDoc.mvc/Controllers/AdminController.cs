using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.CustomModels;
using DataAccess.Models;
using HalloDoc.mvc.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.mvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly INotyfService _notyf;
        private readonly IAdminService _adminService;
        private readonly IPatientService _patientService;
        private readonly IJwtService _jwtService;

        public AdminController(ILogger<AdminController> logger, INotyfService notyfService, IAdminService adminService, IPatientService patientService,IJwtService jwtService)
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

        public IActionResult GetCount()
        {
            var statusCountModel = _adminService.GetStatusCount();
            return PartialView("_allrequest", statusCountModel);
        }


        public IActionResult GetRequestsByStatus(int tabNo)
        {
            var list = _adminService.GetRequestsByStatus(tabNo);
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

        [CustomAuthorize("Admin")]
        public IActionResult admin_dashboard()
        {

            return View();
        }

        [CustomAuthorize("User")]
        public IActionResult viewcase(int Requestclientid, int RequestTypeId)
        {
            var obj = _adminService.ViewCaseViewModel(Requestclientid, RequestTypeId);

            return View(obj);
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

        [CustomAuthorize("User")]
        public IActionResult viewnotes(int ReqId)
        {
            HttpContext.Session.SetInt32("RNId", ReqId);
            ViewNotesModel data = _adminService.ViewNotes(ReqId);
            return View(data);
        }

        public IActionResult CancelCase(int reqId)
        {
            HttpContext.Session.SetInt32("CancelReqId", reqId);
            var model = _adminService.CancelCase(reqId);
            return PartialView("_canclecase", model);
        }

        public IActionResult SubmitCancelCase(CancelCaseModel cancelCaseModel)
        {
            cancelCaseModel.reqId = HttpContext.Session.GetInt32("CancelReqId");
            bool isCancelled = _adminService.SubmitCancelCase(cancelCaseModel);
            if (isCancelled)
            {
                _notyf.Success("Cancelled successfully");
                return RedirectToAction("admin_dashboard", "Admin");
            }
            return View();
        }


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





    }
}