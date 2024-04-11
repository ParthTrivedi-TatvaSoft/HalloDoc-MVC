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
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string firstName { get; set; }
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly dob { get; set; }

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits long")]
        public string? phoneNo { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public string? roomNo { get; set; }
        public string? country { get; set; }

        public string? password { get; set; }

        [Compare("password", ErrorMessage = "Password Missmatch")]
        public string? confirmPassword { get; set; }

        public List<IFormFile>? file { get; set; }


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
        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please enter a valid email address.")]
        public string patientEmail { get; set; }
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


    public class MedicalHistory
    {

        public int reqId { get; set; }
        public DateTime createdDate { get; set; }
        public int currentStatus { get; set; }
        public List<string> document { get; set; }
        public int? IntDate { get; set; }
        public string StrMonth { get; set; }
        public int? IntYear { get; set; }
        public string ConfirmationNumber { get; set; }


    }
    public class MedicalHistoryList
    {
        public List<MedicalHistory>? medicalHistoriesList { get; set; }
        public int? id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }

    }

    public class DocumentModel
    {
        public List<Requestwisefile>? files { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? ReqId { get; set; }
        public List<IFormFile>? uploadedFiles { get; set; }
        public string? ConfirmationNumber { get; set; }
    }


    public class Profile
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int? IntDate { get; set; }
        public string StrMonth { get; set; }
        public int? IntYear { get; set; }

        public int isMobileCheck { get; set; }


        public string? PhoneNo { get; set; }

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? ZipCode { get; set; }

        public string? State { get; set; }

        public string? Email { get; set; }
        public int? userId { get; set; }
    }
    

    
}



