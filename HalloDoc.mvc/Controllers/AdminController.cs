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
        public IActionResult admin_login()
        {
            return View(); 
        }

        public IActionResult viewcase(int reqClientId)
        {
            var obj = _adminService.ViewCase(reqClientId);
            return View(obj);
        }

        public IActionResult admin_resetpassword()
        {
            return View();
        }

        public IActionResult viewnotes()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult admin_login(AdminLoginModel adminLoginModel)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("admin_dashboard", "Admin");
            }
            return View(adminLoginModel);

        }
        public IActionResult admin_dashboard()
        {
            var list = _adminService.GetRequestsByStatus();
            return View(list);
        }
    }
}