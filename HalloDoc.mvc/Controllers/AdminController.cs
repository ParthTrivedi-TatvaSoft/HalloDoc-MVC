using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using Microsoft.AspNetCore.Mvc;

namespace HalloDoc.mvc.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly INotyfService _notyf;
        private readonly IAdminService _adminService;


        public AdminController(ILogger<AdminController> logger, INotyfService notyfService, IAdminService adminService)
        {
            _logger = logger;
            _notyf = notyfService;
            _adminService = adminService;
        }

        public IActionResult Index()
        {
            return View();
        }
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

        public IActionResult admin_dashboard()
        {

            return View();
        }


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





    }
}