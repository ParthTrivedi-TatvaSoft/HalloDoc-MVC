using DataAccess.Models;
using DataAccess.Data;
using HalloDoc.mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics; 

namespace HalloDoc.mvc.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly  ApplicationDbContext _db;

        public PatientController(ILogger<PatientController> logger, ApplicationDbContext db) : this(logger)
        {
            _db = db;
        }
        [HttpPost]
        public async Task<IActionResult> LoginToAcc(Aspnetuser aspnetuser)
        {
            var email = aspnetuser.Email;
            var password = aspnetuser.Passwordhash;
            var user = await aspnetuser.FirstOrDefaultAsync(u => u.Email.Trim() == email && u.Passwordhash.Trim() == password);
            if (user != null)
            {
                return RedirectToAction("submit_request", "Patient");
            }
            return RedirectToAction("patient_login");
        }

        
        //public async Task<IActionResult> Login(Aspnetuser model)
        //{
        //    var email = model.Email;
        //    var password = model.Passwordhash;
        //    var user = await _db.aspnetusers.FirstOrDefaultAsync(u => u.Email.Trim() == email && u.PasswordHash.Trim() == password);
        //    if (user != null)
        //    {
        //        return RedirectToAction("submit_request", "Patient");
        //    }
        //    return RedirectToAction("patient_login");
        //}

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult submit_request()
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}