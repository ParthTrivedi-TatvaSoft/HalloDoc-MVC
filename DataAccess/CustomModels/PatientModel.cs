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
        [RegularExpression(@"^\S.*$", ErrorMessage = "Symptoms cannot be null or whitespace.")]
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name should contain only letters")]
        public string? firstName { get; set; }

        [Required(ErrorMessage = "Last Name name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name should contain only letters")]
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly dob { get; set; }

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? phoneNo { get; set; }

        [RegularExpression(@"^\S.*$", ErrorMessage = "Street is invalid.")]
        public string? street { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? city { get; set; }

        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "State is invalid.")]
        public string? state { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode must be 6 digits long.")]
        public string? zipCode { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Room number is invalid.")]
        public string? roomNo { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Country is invalid.")]
        public string? country { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
      ErrorMessage = "8 characters long (one uppercase, one lowercase letter, one digit, and one special character.)")]
        public string? password { get; set; }

        [Compare("password", ErrorMessage = "Password Missmatch")]
        public string? confirmPassword { get; set; }

        public List<IFormFile>? file { get; set; }


    }

    public class FamilyReqModel
    {
        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name should contain only letters")]
        public string? firstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name should contain only letters")]
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Please enter the email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? phoneNo { get; set; }

        [Required(ErrorMessage = "Please Enter Relation")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Relation is invalid.")]
        public string? relation { get; set; }

        [RegularExpression(@"^\S.*$", ErrorMessage = "Symptoms cannot be null or whitespace.")]
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Patient First name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Patient First name should contain only letters")]
        public string? patientFirstName { get; set; }

        [Required(ErrorMessage = "Patient Last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Patient Last name should contain only letters")]
        public string? patientLastName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly patientDob { get; set; }

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? patientEmail { get; set; }

        [Required(ErrorMessage = "Patient Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? patientPhoneNo { get; set; }

        [RegularExpression(@"^\S.*$", ErrorMessage = "Street is invalid.")]
        public string? street { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? city { get; set; }

        [Required(ErrorMessage = "State is required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "State is invalid.")]
        public string? state { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode must be 6 digits long.")]
        public string? zipCode { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Room number is invalid.")]
        public int? roomNo { get; set; }

        public List<IFormFile>? file { get; set; }
    }

    public class ConciergeReqModel
    {
        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name should contain only letters")]
        public string? firstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name should contain only letters")]
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Please enter the email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? phoneNo { get; set; }


        [Required(ErrorMessage = "Please Enter Hotel/Property Name")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Hotel/Property name is invalid.")]
        public string? hotelName { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Symptoms is invalid.")]
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Patient First name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Patient First name should contain only letters")]
        public string? patientFirstName { get; set; }

        [Required(ErrorMessage = "Patient Last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Patient Last name should contain only letters")]
        public string? patientLastName { get; set; }

        [Required(ErrorMessage = "Patient Date of Birth is required")]
        public DateOnly patientDob { get; set; }

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? patientEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? patientPhoneNo { get; set; }

        [RegularExpression(@"^\S.*$", ErrorMessage = "Street is invalid.")]
        public string? street { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [RegularExpression(@"^\S*$", ErrorMessage = "State is invalid.")]
        public string? state { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode must be 6 digits long.")]
        public string? zipCode { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Room number is invalid.")]
        public int? roomNo { get; set; }
    }


    public class BusinessReqModel
    {

        [Required(ErrorMessage = "Firstname is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name should contain only letters")]
        public string? firstName { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name should contain only letters")]
        public string? lastName { get; set; }

        [Required(ErrorMessage = "Please enter the email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? phoneNo { get; set; }


        [Required(ErrorMessage = "Please Enter Business/Property Name")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Business Name is invalid.")]
        public string? businessName { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Case No is invalid.")]
        public string? caseNo { get; set; }
        public string? symptoms { get; set; }

        [Required(ErrorMessage = "Patient First name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Patient First name should contain only letters")]
        public string? patientFirstName { get; set; }

        [Required(ErrorMessage = "Patient Last name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Patient Last name should contain only letters")]
        public string? patientLastName { get; set; }
        [Required(ErrorMessage = "Patient Date of Birth is required")]
        public DateOnly patientDob { get; set; }

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? patientEmail { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? patientPhoneNo { get; set; }

        [RegularExpression(@"^\S.*$", ErrorMessage = "Street is invalid.")]
        public string? street { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        [RegularExpression(@"^\S*$", ErrorMessage = "State is invalid.")]
        public string? state { get; set; }

        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode must be 6 digits long.")]
        public string? zipCode { get; set; }

        [RegularExpression(@"^\S*$", ErrorMessage = "Room number is invalid.")]
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



