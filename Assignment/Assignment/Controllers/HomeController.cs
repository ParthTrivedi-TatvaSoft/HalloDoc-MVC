using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLogic.Service;
using BusinessLogic.Interface;
using DataAccess.Models;
using System.Collections;
using DataAccess.CustomModels;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEMService _iemsservice;
 
       
     

        public HomeController(ILogger<HomeController> logger,IEMService iemsservice)
        {
            _logger = logger;
            _iemsservice = iemsservice;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult EMS_Screen()
        {
            var obj = _iemsservice.EmsRecords();
            return View(obj);
        }

        [HttpGet]
        public IActionResult DeleteRecord(int employeeid)
        {
            var isDeleted = _iemsservice.DeleteRecord(employeeid);
            return Json(new { isDeleted = isDeleted });
        }

        public IActionResult AddEmployee()
        {
            return PartialView("_AddRecords");
        }
        [HttpPost]
        public IActionResult AddEmployee(Records model)
        {
            var obj = _iemsservice.AddEmployees(model);

            return RedirectToAction("EMS_Screen", model);
        }

        public IActionResult EditEmployeeRecord(int employeeid)
        {
            var obj = _iemsservice.EditEmp(employeeid);
            return PartialView("_EditRecords", obj);

        }



    }
}