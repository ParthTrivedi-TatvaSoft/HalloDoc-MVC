using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography.X509Certificates;

namespace DataAccess.CustomModels
{
    public class PatientInfoModel
    {
        //[Required]
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "First Name Is Required")]
        public string firstName { get; set; }
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Please Enter Date")]
        public DateOnly dob { get; set; }

        [Required(ErrorMessage = "Please Enter The Patient's Email Address.")]
        //[RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please enter a valid email address.")]
        public string email { get; set; }
        public string? phNo  { get; set; }
        public string? street { get; set; } 
        public string? city { get; set; } 
        public string? state { get; set; } 
        public string? zipCode { get; set; } 
        public string? roomNo { get; set; }
        public string? country { get; set; }

        //[StringLength(15, MinimumLength = 4, ErrorMessage = "Password Have 4 to 15 Char")]
        public string? password { get; set; }

        [Compare("password", ErrorMessage = "Password Missmatch")]
        public string? confirmPassword { get; set; }
        public List<IFormFile>? File { get; set; }
        public int requestId { get; set; }
    }
    

    public class FamilyReqModel
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phoneNo { get; set; }
        public string? relation { get; set; }
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        public string patientFirstName { get; set; }
        public string? patientLastName { get; set; }
        public DateTime? patientDob { get; set; }
        public string? patientEmail { get; set; }
        public string? patientPhoneNo { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public string? roomNo { get; set; }

        public IFormFile? File { get; set; }



    }

    public class ConciergeReqModel
    {
        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phoneNo { get; set; }
        public string? hotelName { get; set; }
        public string? symptoms { get; set; }
        [Required(ErrorMessage = "Please Enter Patient's First Name")]
        public string patientFirstName { get; set; }
        public string? patientLastName { get; set; }
        public DateTime? patientDob { get; set; }
        public string? patientEmail { get; set; } 
        public string? patientPhoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        public string street { get; set; }

        [Required(ErrorMessage = "Please Enter Your City")]
        public string city { get; set; }

        [Required(ErrorMessage = "Please Enter Your State")]
        public string state { get; set; }
        [Required(ErrorMessage = "Please Enter Your ZipCode")]
        public string zipCode { get; set; }
        public int? roomNo { get; set; }

    }

    public class BusinessReqModel
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        public string? phoneNo { get; set; }

        [Required(ErrorMessage ="Please Enter Business/Property Name")]
        public string businessName { get; set; }
        public string? caseNo { get; set; }
        public string? symptoms { get; set; }
        public string? patientFirstName { get; set; }
        public string? patientLastName { get; set; }
        public DateTime? patientDob { get; set; }
        public string? patientEmail { get; set; }
        public string? patientPhoneNo { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public int? roomNo { get; set; }
    
    }


    public class PatientDashboard
    {
        public DateTime createdDate { get; set; }
        public string currentStatus { get; set; }
        public string document { get; set; }
    }

    public class PatientDashboardInfo
    {
        public List<PatientDashboard> patientDashboardItems { get; set; }
    }

    public class MedicalHistory
    {
        public int reqId { get; set; }
        public DateTime createdDate { get; set; }
        public string currentStatus { get; set; }
        public List<string>? document { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string PhoneNo { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string State { get; set; }

        public string Email { get; set; }
    }
    public class MedicalHistoryList
    {
        public List<MedicalHistory> medicalHistoriesList { get; set; }
    }

    public class Profile
    {

        [Required(ErrorMessage = "Please Enter The First Name")]
        public string firstName { get; set; }
        public string? lastName { get; set; }
        public DateTime? dob { get; set; }

        //[RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please enter a valid email address.")]
        public string email { get; set; }
        public string? MobileNo { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipCode { get; set; }
      
        

    }

    
}



