﻿using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{

    public class LoginDetail
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
    }
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        public string email { get; set; } 

        [Required(ErrorMessage = "Password Is Required")]
        public string password { get; set; } 
    }

    public class StatusCountModel
    {
        public int NewCount { get; set; }
        public int PendingCount { get; set; }
        public int ActiveCount { get; set; }
        public int ConcludeCount { get; set; }
        public int ToCloseCount { get; set; }
        public int UnpaidCount { get; set; }

    }


    public class AdminDashTableModel
    {
        public int Reqid { get; set; }
        public int reqClientId { get; set; }
        public string? firstName { get; set; }

        public string? lastName { get; set; }

        public string strMonth { get; set; }
        public int? intYear { get; set; }
        public int? intDate { get; set; }

        public string reqstrMonth { get; set; }
        public int? reqintYear { get; set; }
        public int? reqintDate { get; set; }

        public string? requestorFname { get; set; }

        public string? requestorLname { get; set; }

        public DateTime createdDate { get; set; }

        public string? mobileNo { get; set; }

        public string? city { get; set; }

        public string? street { get; set; }

        public string? zipCode { get; set; }

        public string? state { get; set; }

        public string? notes { get; set; }

        public int? requestTypeId { get; set; }

        public int? status { get; set; }

        public int? requestClientId { get; set; }

        public int? regionId { get; set; }

        public int? callType { get; set; }
        public int? phyId { get; set; }

        public bool? isFinalized { get; set; }

        public string? reqDate { get; set; }

        public string? email { get; set; }
    }


    public class DashboardModel
    {
        public List<AdminDashTableModel>? adminDashTableList { get; set; }
        public List<Region>? regionList { get; set; }
        public int? TotalPage { get; set; }
        public int? CurrentPage { get; set; }
    }
    public class ViewCaseViewModel
    {
        public int RequestId { get; set; }
        public int Requestclientid { get; set; }
        public int RequestTypeId { get; set; }
        public string? Requesttype { get; set; }
        public string Firstname { get; set; } = null!;
        public string? Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Phonenumber { get; set; }
        public string? Address { get; set; }
        public string? RegionName { get; set; }
        public string? Notes { get; set; }
        public string? Email { get; set; }
        public string? StrMonth { get; set; }
        public int? IntYear { get; set; }
        public int? IntDate { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string? Room { get; set; }
        public string? ConfirmationNumber { get; set; }
        public int status { get; set; }
    }


    public class ViewNotesModel
    {
        public List<Requeststatuslog>? TransferNotesList { get; set; }
        public string? PhysicianNotes { get; set; }

        public string? AdminNotes { get; set; }

        [Required(ErrorMessage = "Note Is Required")]
        public string? AdditionalNotes { get; set; }

        public int ReqId { get; set; }
    }


    public class CancelCaseModel
    {
        public string? PatientFName { get; set; }
        public string? PatientLName { get; set; }
        public List<Casetag>? casetaglist { get; set; }

        public int? casetag { get; set; }
        public int? reqId { get; set; }
        public string? notes { get; set; }
    }



    public class AssignCaseModel
    {
        public int? ReqId { get; set; }
        public List<Region>? regionList { get; set; }
        public List<Physician> physicianList { get; set; }
        public int? selectRegionId { get; set; }
        public int? selectPhysicianId { get; set; }
        public string? description { get; set; }

    }

    public class BlockCaseModel
    {
        public int? ReqId { get; set; }
        public string? reason { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class ViewUploadModel
    {
        public List<Requestwisefile>? files { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public int? ReqId { get; set; }

        public List<IFormFile>? uploadedFiles { get; set; }

        public string? ProviderNote { get; set; }

    }

    public class Order
    {
        public int? ReqId { get; set; }
        public List<Healthprofessionaltype> Profession { get; set; }
        public List<Healthprofessional> Business { get; set; }
        public string BusineesContact { get; set; }
        public string email { get; set; }
        public string faxnumber { get; set; }
        public string orderDetail { get; set; }
        public int? ProfessionId { get; set; }
        public int? BusinessId { get; set; }
        public int? RefilNo { get; set; }

    }

    public class SendAgreementModel
    {
        public int? Reqid { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int? reqType { get; set; }
    }

    public class AgreementModel
    {
        public string? fName { get; set; }
        public string? lName { get; set; }
        public string? reason { get; set; }
        public int? reqId { get; set; }
    }


    public class CloseCaseModel
    {
        public int? reqid { get; set; }
        public string? email { get; set; }
        public string? phoneNo { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? IntDate { get; set; }
        public string StrMonth { get; set; }
        public int? IntYear { get; set; }
        public string? confirmation_no { get; set; }
        public int? status { get; set; }
        public List<Requestwisefile>? files { get; set; }

        [Required(ErrorMessage = "Please Enter Atleast One File")]
        public IFormFile Upload { get; set; }
    }

    public class EncounterFormModel
    {
        public int reqid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Location { get; set; }
        public string? BirthDate { get; set; }
        public DateTime? Date { get; set; }
        public string? fullDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HistoryIllness { get; set; }
        public string? MedicalHistory { get; set; }
        public string? Medications { get; set; }
        public string? Allergies { get; set; }
        public decimal? Temp { get; set; }
        public decimal? Hr { get; set; }
        public decimal? Rr { get; set; }
        public int? BpS { get; set; }
        public int? BpD { get; set; }
        public decimal? O2 { get; set; }
        public string? Pain { get; set; }
        public string? Heent { get; set; }
        public string? Cv { get; set; }
        public string? Chest { get; set; }
        public string? Abd { get; set; }
        public string? Extr { get; set; }
        public string? Skin { get; set; }
        public string? Neuro { get; set; }
        public string? Other { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? MedicationDispensed { get; set; }
        public string? Procedures { get; set; }
        public string? FollowUp { get; set; }
        public bool? indicate { get; set; }
    }

    public class MyProfileModel
    {
        public int? admin_id { get; set; }
        public int? aspnetuserid { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First Name must contain only letters")]
        public string? fname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last Name must contain only letters")]
        public string? lname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        public string? email { get; set; }

        [Compare("email", ErrorMessage = "Email Missmatch")]
        public string? confirm_email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Mobile number")]
        public string? mobile_no { get; set; }

        [Required(ErrorMessage = "Please Enter Address-1")]
        public string? addr1 { get; set; }

        [Required(ErrorMessage = "Please Enter Address-2")]
        public string? addr2 { get; set; }

        [Required(ErrorMessage = "Please Enter Your City")]
        public string? city { get; set; }
        public int regionId { get; set; }

        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        public string zip { get; set; }

        [Required(ErrorMessage = "Please Enter Your Alternate phone")]
        public string? altphone { get; set; }
        public int? createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public int status { get; set; }
        public int? roleid { get; set; }
        public string username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string password { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        public string state { get; set; }
        public List<Aspnetrole> roles { get; set; }
        public int? flag { get; set; }

        public bool indicate { get; set; }

        public List<AdminRegionTable> adminregions { get; set; }

    }


    public class SendLinkModel
    {
        [Required(ErrorMessage = "FirstName Is Required")]
        public string? fName { get; set; }
        [Required(ErrorMessage = "LastName Is Required")]
        public string? lName { get; set; }
        [Required(ErrorMessage = "Phone No. Is Required")]
        public string? phoneNo { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        public string? email { get; set; }

    }


    public class CreateRequestModel
    {

        public int? requesttypeid { get; set; }

        public int? userid { get; set; }

        [Required(ErrorMessage = "Please Enter Your Street Name")]
        public string? street { get; set; }

        [Required(ErrorMessage = "Please Enter Your City Name")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter Your State Name")]
        public string? state { get; set; }

        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        public string? zipcode { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string? lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Date Of Birth")]
        public string? dateofbirth { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        public string? phone { get; set; }

        [Required(ErrorMessage = "Please Enter Your Room")]
        public string? room { get; set; }

        public string? admin_notes { get; set; }

        public bool? indicate { get; set; }
    }



    public class ProviderModel
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }


        public bool? notification { get; set; }

        public string? role { get; set; }

        public string? onCallStatus { get; set; }

        public string? status { get; set; }

        public int? phyId { get; set; }

        [Required(ErrorMessage = "Please Enter A Message")]
        public string? message { get; set; }
    }

    public class ProviderModel2
    {
        public List<ProviderModel>? providerModels { get; set; }
        public List<Region>? regions { get; set; }
    }

    public class EditProviderModel
    {
        public string? username { get; set; }

        public string? password { get; set; }

        public string? Email { get; set; }
        public string? Con_Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Status { get; set; }


        public string? city { get; set; }

        public string? country { get; set; }

        public string? zipcode { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public int? Regionid { get; set; }

        public int? Roleid { get; set; }

        public string? MedicalLicesnse { get; set; }

        public string? NPInumber { get; set; }

        public string? SycnEmail { get; set; }

        public string? Businessname { get; set; }

        public string? BusinessWebsite { get; set; }

        public string? Adminnotes { get; set; }

        public string? Address1 { get; set; }
        public string? Address2 { get; set; }

        public int PhyID { get; set; }
        public int statusId { get; set; }

        public List<Region> regions { get; set; }

        //public List<Physicianregion> physicianregions { get; set; }

        public string? altPhone { get; set; }
        public string? State { get; set; }
        public int? StateId { get; set; }
        public string? flag { get; set; }

        public IFormFile? Photo { get; set; }

        public string? PhotoValue { get; set; }

        public IFormFile? Signature { get; set; }

        public string? SignatureValue { get; set; }

        public IFormFile? ContractorAgreement { get; set; }

        public bool IsContractorAgreement { get; set; }

        public IFormFile? BackgroundCheck { get; set; }

        public bool IsBackgroundCheck { get; set; }

        public IFormFile? HIPAA { get; set; }

        public bool IsHIPAA { get; set; }

        public IFormFile? NonDisclosure { get; set; }

        public bool IsNonDisclosure { get; set; }

        public IFormFile? LicenseDocument { get; set; }

        public bool IsLicenseDocument { get; set; }

        public List<Role> roles { get; set; }

        public bool? indicate { get; set; }
    }
    public class PhysicianRegionTable
    {
        public int? PhysicianId { get; set; }

        public int? Regionid { get; set; }

        public string? Name { get; set; }

        public bool ExistsInTable { get; set; }
    }
    public class EditProviderModel2
    {
        
        public EditProviderModel? editPro { get; set; }
        public List<Region>? regions { get; set; }

        public List<PhysicianRegionTable>? physicianregiontable { get; set; }
        public List<Role>? roles { get; set; }
    }
    
   
    public class AccountAccess
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public short AccountType { get; set; }

    }

    public class AccountMenu
    {
        public int menuid { get; set; }

        public int roleid { get; set; }

        public string name { get; set; }

        public int accounttype { get; set; }

        public bool ExistsInTable { get; set; }

    }
    public class accessModel
    {
        public List<AccountAccess> AccountAccess { get; set; }

        public AccountAccess CreateAccountAccess { get; set; }

        public List<Aspnetrole> Aspnetroles { get; set; }

        public List<Menu> Menus { get; set; }

        public List<AccountMenu> AccountMenu { get; set; }

        public List<UserAccess> UserAccess { get; set; }

        public List<Aspnetrole> AspnetUserroles { get; set; }

        public int Aspid { get; set; }

        public int? flag { get; set; }
    }


    public class CreateAccess
    {
        public List<Menu> Menu { get; set; }
    }

    public class CreateProviderAccount
    {
        [Required(ErrorMessage = "Usernaame Is Required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "User is invalid.")]
        public string? username { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        
        public string? password { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please Enter A Valid Email Address.")]
        public string? Email { get; set; }
        [Compare("Email", ErrorMessage = "Email Missmatch")]
        public string? Con_Email { get; set; }

        [Required(ErrorMessage = "PhoneNumber Is Required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number Must Be 10 Digits Long.")]
        public string? PhoneNumber { get; set; }

        public string? Status { get; set; }

        [Required(ErrorMessage = "City Is Required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? city { get; set; }

        public string? country { get; set; }
        [Required(ErrorMessage = "Zipcode Is Required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode Must Be 6 Digits Long.")]
        public string? zipcode { get; set; }
        [Required(ErrorMessage = "First Name Is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name Should Contain Only Letters")]
        public string? Firstname { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name Should Contain Only Letters")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Region Is Required")]
        public int? Regionid { get; set; }

        [Required(ErrorMessage = "Role Is Required")]
        public int? Roleid { get; set; }
        [Required(ErrorMessage = "MedicalLicense No Is Required")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Cannot Be Null Or Whitespace.")]
        public string? MedicalLicesnse { get; set; }
        [Required(ErrorMessage = "NPInumber Is Required")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Cannot Be Null Or Whitespace.")]
        public string? NPInumber { get; set; }

        public string? SycnEmail { get; set; }

        [Required(ErrorMessage = "Business Name Is Required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Businessname Is Invalid.")]
        public string? Businessname { get; set; }

        [Required(ErrorMessage = "Business Website Is Required")]
        [RegularExpression(@"^(https?://)?(www\.)?([a-zA-Z0-9\-]+)\.([a-zA-Z]{2,63})(:\d{1,5})?(/.*)?$",
       ErrorMessage = "Invalid Website URL")]
        public string? BusinessWebsite { get; set; }

        [Required(ErrorMessage = "Admin notes Is Required")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Cannot Be Null Or Whitespace.")]
        public string? Adminnotes { get; set; }
        [Required(ErrorMessage = "Address1 Is Required")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Address Is Invalid.")]
        public string? Address1 { get; set; }
        [Required(ErrorMessage = "Address2 Is Required")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Address Is Invalid.")]
        public string? Address2 { get; set; }

        public int PhyID { get; set; }
        public int statusId { get; set; }
        public int adminId { get; set; }
        public int aspnetUserId { get; set; }

        public List<Region> regions { get; set; }

        //public List<Physicianregion> physicianregions { get; set; }

        [Required(ErrorMessage = "PhoneNumber Is Required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone Number Must Be 10 Digits Long.")]
        public string? altPhone { get; set; }
        public string? State { get; set; }
        public int? StateId { get; set; }
        public string? flag { get; set; }

        public IFormFile? Photo { get; set; }

        public string? PhotoValue { get; set; }

        public IFormFile? Signature { get; set; }

        public string? SignatureValue { get; set; }

        public IFormFile? ContractorAgreement { get; set; }

        public bool IsContractorAgreement { get; set; }

        public IFormFile? BackgroundCheck { get; set; }

        public bool IsBackgroundCheck { get; set; }

        public IFormFile? HIPAA { get; set; }

        public bool IsHIPAA { get; set; }

        public IFormFile? NonDisclosure { get; set; }

        public bool IsNonDisclosure { get; set; }

        public IFormFile? LicenseDocument { get; set; }

        public bool IsLicenseDocument { get; set; }

        public List<Role> roles { get; set; }

        public bool? indicate { get; set; }
        public string? indicateTwo { get; set; }

        public decimal longitude { get; set; }

        public decimal latitude { get; set; }

        public DateTime? created_date { get; set; }

        public int? createdBy { get; set; }

        public DateTime? modified_date { get; set; }

        public int? modifiedBy { get; set; }
    }



    public class AdminRegionTable
    {
        public int? AdminId { get; set; }

        public int? Regionid { get; set; }

        public string? Name { get; set; }

        public bool ExistsInTable { get; set; }
    }

    public class CreateAdminAccount
    {
        [Required(ErrorMessage = "Username is required")]
        [RegularExpression(@"^\S*$", ErrorMessage = "User is invalid.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
   
        public string? AdminPassword { get; set; }
        public string? aspnetUserId { get; set; }
        public int? adminId { get; set; }
        public short? Status { get; set; }
        public string? Role { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name should contain only letters")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name  is required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name should contain only letters")]
        public string? LastName { get; set; }


        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? AdminPhone { get; set; }


        [Required(ErrorMessage = "Please enter the email address.")]
        [RegularExpression(@"^\S+@\S+\.\S{2,}$", ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }

        [Compare("Email", ErrorMessage = "Email Missmatch")]
        public string? ConfirmEmail { get; set; }
        public List<Region>? RegionList { get; set; }
        public IEnumerable<int> AdminRegion { get; set; }

        [RegularExpression(@"^\S.*$", ErrorMessage = "Address is invalid.")]
        public string? Address1 { get; set; }
        [RegularExpression(@"^\S.*$", ErrorMessage = "Address is invalid.")]
        public string? Address2 { get; set; }
        [RegularExpression(@"^\S*$", ErrorMessage = "City is invalid.")]
        public string? City { get; set; }
        [RegularExpression(@"^\S*$", ErrorMessage = "State is invalid.")]
        public string? State { get; set; }
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Zipcode must be 6 digits long.")]
        public string? Zip { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits long.")]
        public string? BillingPhone { get; set; }
        public int[] RegionArray { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int roleId { get; set; }
        [Required(ErrorMessage = "Region is required")]
        public int regionId { get; set; }
        public List<Role> roles { get; set; }
        public List<AdminRegionTable> adminRegions { get; set; }
        public List<Region> regions { get; set; }
    }

    public class UserAccess
    {
        public short? accType { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? phone { get; set; }
        public short? status { get; set; }
        public int? openReq { get; set; }
        public int? phyId { get; set; }
        public int? adminId { get; set; }
    }


    public class CheckBoxData
    {
        public int Id { get; set; }
        public string value { get; set; }

        public bool Checked { get; set; }
    }

    public class BusinessTableModel
    {
        public int BusinessId { get; set; }
        public int ProfessionId { get; set; }
        public string ProfessionName { get; set; }

        [Required(ErrorMessage = "Business Name Is Required")]
        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Fax Number Is Required")]
        public string FaxNumber { get; set; }
        public string BusinessContact { get; set; }

 

    }
    public class AddBusinessModel
    {
        public int? VendorId { get; set; }

        [Required(ErrorMessage = "Business Name Is Required")]
        public string BusinessName { get; set; }
        public int ProfessionId { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number Is Required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Fax Number Is Required")]
        public string FaxNumber { get; set; }
        [Required(ErrorMessage = "Business Contact Is Required")]
        public string BusinessContact { get; set; }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
        public List<Region> RegionList { get; set; }
        public int RegionId { get; set; }
        public List<Healthprofessionaltype> ProfessionList { get; set; }

    }

    public class RecordsModel
    {
        public List<RequestsRecordModel>? requestListMain { get; set; }
        public int? searchRecordOne { get; set; }
        public string? searchRecordTwo { get; set; }
        public int? searchRecordThree { get; set; }
        public DateOnly? searchRecordFour { get; set; }
        public DateOnly? searchRecordFive { get; set; }
        public string? searchRecordSix { get; set; }
        public string? searchRecordSeven { get; set; }
        public string? searchRecordEight { get; set; }
    }
    public class RequestsRecordModel
    {
        public string? patientname { get; set; }
        public string? requestor { get; set; }
        public DateTime? dateOfService { get; set; }
        public DateTime? closeCaseDate { get; set; }
        public string? email { get; set; }
        public string? contact { get; set; }
        public string? address { get; set; }
        public string? zip { get; set; }
        public string? status { get; set; }
        public int? statusId { get; set; }
        public string? physician { get; set; }
        public string? physicianNote { get; set; }
        public string? providerNote { get; set; }
        public string? AdminNote { get; set; }
        public string? pateintNote { get; set; }
        public int? requestid { get; set; }
        public int? requesttypeid { get; set; }
        public int? userid { get; set; }
        public int? flag { get; set; }

    }

    public class PatientRecordsModel
    {
        public List<User>? users { get; set; }
        public List<Request>? requestList { get; set; }
        public List<Requestclient>? requestClient { get; set; }

        public string? searchRecordOne { get; set; }
        public string? searchRecordTwo { get; set; }
        public string? searchRecordThree { get; set; }
        public string? searchRecordFour { get; set; }
        public int? flag { get; set; }
    }

    public class GetRecordExplore
    {
        public string? fullname { get; set; }
        public string? confirmationnumber { get; set; }
        public string? providername { get; set; }
        public string? createddate { get; set; }
        public string? concludedate { get; set; }
        public int? status { get; set; }
        public int? requestid { get; set; }
        public int? requestclientid { get; set; }
        public int? requesttypeid { get; set; }


        
    }

    public class blockHistory
    {
        public string? patientname { get; set; }
        public string? phonenumber { get; set; }
        public string? email { get; set; }
        public string? createddate { get; set; }
        public BitArray? isActive { get; set; }
        public string? notes { get; set; }
        public int? blockId { get; set; }
        public int? flag { get; set; }
        public string? searchRecordOne { get; set; }
        public DateTime searchRecordTwo { get; set; }
        public string? searchRecordThree { get; set; }
        public string? searchRecordFour { get; set; }
        public bool indicate { get; set; }

    }


    public class EmailSmsRecords
    {

        public int? requestid { get; set; }
        public string? recipient { get; set; }
        public string? action { get; set; }
        public string? rolename { get; set; }
        public string? email { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? sentdate { get; set; }
        public string? sent { get; set; }
        public int? senttries { get; set; }
        public string? confirmationNumber { get; set; }
        public string? contact { get; set; }
    }

    public class EmailSmsRecords2
    {
        public List<EmailSmsRecords>? emailRecords { get; set; }
        public int? tempid { get; set; }
        public string? searchRecordOne { get; set; }
        public string? searchRecordTwo { get; set; }
        public string? searchRecordThree { get; set; }
        public DateTime? searchRecordFour { get; set; }
        public DateTime? searchRecordFive { get; set; }
    }

    public class BlockHistory
    {
        public string? patientname { get; set; }
        public string? phonenumber { get; set; }
        public string? email { get; set; }
        public string? createddate { get; set; }
        public BitArray? isActive { get; set; }
        public string? notes { get; set; }

        public int? blockId { get; set; }


        public bool indicate { get; set; }

    }

    public class BlockHistory2
    {
        public List<BlockHistory>? blockHistories { get; set; }
        public string? searchRecordOne { get; set; }
        public DateTime searchRecordTwo { get; set; }
        public string? searchRecordThree { get; set; }
        public string? searchRecordFour { get; set; }
        public int? flag { get; set; }

    }


    public class CreateShiftModel
    {
        public int RegionId { get; set; }
        public int PhysicianId { get; set; }
        public DateOnly StartDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsRepeat { get; set; }
        public string? WeekDays { get; set; }
        public int? RepeatUpto { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<Physician> Physicians { get; set; }
        public IEnumerable<CheckBoxData> Days { get; set; }

    }
    public class SchedulingViewModel
    {

        public List<Region> regions { get; set; }
        public List<Physicianregion> physicianregionlist { get; set; }
        [Required]
        public int regionid { get; set; }
        [Required]
        public int providerid { get; set; }
        public DateOnly shiftdateviewshift { get; set; }
        [Required]
        public DateOnly shiftdate { get; set; }
        public TimeOnly starttime { get; set; }
        public TimeOnly endtime { get; set; }
        public int repeatcount { get; set; }
        public int shiftid { get; set; }
        public int shiftdetailid { get; set; }
        public string physicianname { get; set; }
        public string regionname { get; set; }

    }
    public class DayWiseScheduling
    {
        public int shiftid { get; set; }
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }
        public List<Shiftdetail> shiftdetails { get; set; }
    }
    public class MonthWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Shiftdetail> shiftdetails { get; set; }
        public List<Physician> physicians { get; set; }

    }
    public class WeekWiseScheduling
    {
        public DateTime date { get; set; }
        public List<Physician> physicians { get; set; }

        public List<Shiftdetail> shiftdetails { get; set; }

    }

    public class ProviderOnCall
    {
        public IEnumerable<Shiftdetail> shiftdetaillist { get; set; }
        public IEnumerable<Shift> shiftlist { get; set; }
        public IEnumerable<Physician> ondutyphysicianlist { get; set; }
        public IEnumerable<Physician> offdutyphysicianlist { get; set; }
        public List<Region> regions { get; set; }

    }
    public class CreateNewShift
    {
        public List<Region>? RegionList { get; set; }
        [Required(ErrorMessage = "Please Select Region")] public int RegionId { get; set; }
        public string? RegionName { get; set; }
        [Required(ErrorMessage = "Please Select Physician")] public int PhysicianId { get; set; }
        public string PhysicianName { get; set; }
        [Required(ErrorMessage = "ShiftDate is required")] public DateOnly ShiftDate { get; set; }
        [Required(ErrorMessage = "StartTime is required")] public TimeOnly Start { get; set; }
        [Required(ErrorMessage = "EndTime is required")] public TimeOnly End { get; set; }
        public List<int>? RepeatDays { get; set; }
        public int RepeatEnd { get; set; }
        public int shiftdetailid { get; set; }
    }

    public class OnCallModal
    {

        public List<Physician> OnCall { get; set; }

        public List<Physician> OffDuty { get; set; }

        public List<Region> regions { get; set; }
    }
    public class ShiftReview
    {
        public int shiftDetailId { get; set; }

        public string PhysicianName { get; set; }

        public string ShiftDate { get; set; }

        public string ShiftTime { get; set; }

        public string ShiftRegion { get; set; }

    }

    public class ShiftReview2
    {


        public List<Region> regions { get; set; }


        public List<ShiftReview> ShiftReview { get; set; }

        public int regionId { get; set; }

        public int callId { get; set; }
    }

    public class FilterModel
    {
        public string? searchWord { get; set; }
        public int? requestTypeId { get; set; }
        public int? regionId { get; set; }
        public int tabNo { get; set; }
        public int? TotalPage { get; set; }

        public int CurrentPage { get; set; }

    }

    public class TransferRequest
    {
        public int? ReqId { get; set; }
        public string? description { get; set; }
    }

    public class RequestAdmin
    {
        public int? reqId { get; set; }

        [Required(ErrorMessage = "Please Enter Description")]
        [RegularExpression(@"^\S.*$", ErrorMessage = "Note Can Not Be Null")]
        public string? Note { get; set; }

    }



}