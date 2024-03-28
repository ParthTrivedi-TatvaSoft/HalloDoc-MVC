using DataAccess.Models;
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
        public int Requestclientid { get; set; }
        public int? RequestTypeId { get; set; }
        public int? Requestid { get; set; }
        public string Firstname { get; set; } = null!;
        public string? Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phonenumber { get; set; }
        public string? Address { get; set; }
        public int? Regionid { get; set; }
        public string? Notes { get; set; }
        public string? Email { get; set; }
        public string? Strmonth { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zipcode { get; set; }
        public string? Room { get; set; }
        public string? ConfirmationNumber { get; set; }
    }



    public class ViewNotesModel
    {
        public List<Requeststatuslog>? TransferNotesList { get; set; }
        public string? PhysicianNotes { get; set; }

        public string? AdminNotes { get; set; }
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

        [Required(ErrorMessage = "Please Enter address-1")]
        public string? addr1 { get; set; }

        [Required(ErrorMessage = "Please Enter address-2")]
        public string? addr2 { get; set; }

        [Required(ErrorMessage = "Please Enter Your City")]
        public string? city { get; set; }
        public int regionId { get; set; }

        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        public string zip { get; set; }
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
    }


    public class SendLinkModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string? fName { get; set; }
        [Required(ErrorMessage = "LastName is required")]
        public string? lName { get; set; }
        [Required(ErrorMessage = "Phone No. is required")]
        public string? phoneNo { get; set; }
        [Required(ErrorMessage = "Email is required")]
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


    public class EditProviderModel
    {
        public string? username { get; set; }

        public string? password { get; set; }

        public string? Email { get; set; }

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

        public int? PhyID { get; set; }



        public string? altPhone { get; set; }
        public string? State { get; set; }

        public IFormFile? Photo { get; set; }

        public IFormFile? Signature { get; set; }

        public BitArray? isAgreementdoc { get; set; }

        public BitArray? isBackgrounddoc { get; set; }

        public BitArray? isTrainingdoc { get; set; }

        public BitArray? isNondiclosuserdoc { get; set; }

        public BitArray? isLicesensdoc { get; set; }

        public IFormFile? isAgreementdocument { get; set; }

        public IFormFile? isBackgrounddocument { get; set; }

        public IFormFile? isTrainingdocument { get; set; }

        public IFormFile? isNondiclosuserdocument { get; set; }

        public IFormFile? isLicesensdocument { get; set; }

        public List<Role>? roles { get; set; }
    }
    public class PhysicianRegionTable
    {
        public int PhysicianId { get; set; }

        public int Regionid { get; set; }

        public string Name { get; set; }

        public bool ExistsInTable { get; set; }
    }
    public class EditProviderModel2
    {
        public EditProviderModel? editPro { get; set; }
        public List<Region>? regions { get; set; }

        public List<PhysicianRegionTable>? physicianregiontable { get; set; }
    }
    public class AccountAccess
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public short AccountType { get; set; }

    }

    public class CreateAccess
    {
        public List<Menu> Menu { get; set; }
    }

    public class CreateAdminAccount
    {
        public string? UserName { get; set; }
        public string? AdminPassword { get; set; }
        public short? Status { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AdminPhone { get; set; }
        public string? Email { get; set; }
        public string? ConfirmEmail { get; set; }
        public List<Region>? RegionList { get; set; }
        public IEnumerable<int> AdminRegion { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? BillingPhone { get; set; }
    }
}