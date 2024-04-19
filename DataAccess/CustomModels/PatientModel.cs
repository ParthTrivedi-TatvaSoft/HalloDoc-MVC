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

        [Required(ErrorMessage = "First Name Is Required")]
        public string firstName { get; set; }

        [Required(ErrorMessage = "Last Name Is Required")]
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Date Of Birth Is Required")]
        public DateOnly dob { get; set; }

        [Required(ErrorMessage = "Please Enter The Patient's Email Address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please Enter A Valid Email Address.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Phone Number Is Required")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone Number Is Not Valid")]
        public string? phoneNo { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }

        [Required(ErrorMessage = "State Is Required")]
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public string? roomNo { get; set; }
        public string? country { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [Compare("password", ErrorMessage = "Password Missmatch")]
        public string? confirmPassword { get; set; }

        public List<IFormFile>? file { get; set; }


    }


    public class FamilyReqModel
    {
        [Required(ErrorMessage = "First Name Is Required")]
        public string firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone Number Is Not Valid")]
        public string? phoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter Relation")]
        public string? relation { get; set; }
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Patient First Name Is Required")]
        public string? patientFirstName { get; set; }
        public string? patientLastName { get; set; }
        public DateOnly patientDob { get; set; }

        [Required(ErrorMessage = "Please Enter The Patient's Email Address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please Enter A Valid Email Address.")]
        public string? patientEmail { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone Number Is Not Valid")]
        public string? patientPhoneNo { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        [Required(ErrorMessage = "State Is Required")]
        public string? state { get; set; }
        public string? zipCode { get; set; }
        public int? roomNo { get; set; }
        public List<IFormFile>? file { get; set; }
    }


    public class ConciergeReqModel
    {

        [Required(ErrorMessage = "First Name Is Required")]
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone Number Is Not Valid")]
        public string? phoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter Hotel/Property Name")]
        public string? hotelName { get; set; }
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Patient First Name Is Required")]
        public string? patientFirstName { get; set; }
        public string? patientLastName { get; set; }
        [Required(ErrorMessage = "Patient Date of Birth Is Required")]
        public DateOnly patientDob { get; set; }

        [Required(ErrorMessage = "Please Enter The Patient's Email Address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please Enter A Valid Email Address.")]
        public string? patientEmail { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone Number Is Not Valid")]
        public string? patientPhoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        public string street { get; set; }
        [Required(ErrorMessage = "Please Enter City")]
        public string city { get; set; }
        [Required(ErrorMessage = "Please Enter State")]
        public string state { get; set; }
        [Required(ErrorMessage = "Please Enter ZipCode")]
        public string? zipCode { get; set; }
        public int? roomNo { get; set; }


    }


    public class BusinessReqModel
    {

        [Required(ErrorMessage = "First Name Is Required")]
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? email { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone Number Is Not Valid")]
        public string? phoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter Business/Property Name")]
        public string? businessName { get; set; }
        public string? caseNo { get; set; }
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Patient First Name Is Required")]
        public string? patientFirstName { get; set; }
        public string? patientLastName { get; set; }
        [Required(ErrorMessage = "Patient Date Of Birth Is Required")]
        public DateOnly patientDob { get; set; }

        [Required(ErrorMessage = "Please Enter The Patient's Email Address.")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please Enter A Valid Email Address.")]
        public string? patientEmail { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Phone Number Is Not Valid")]
        public string? patientPhoneNo { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        [Required(ErrorMessage = "State Is Required")]
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
        public int docCount { get; set; }

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



