using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
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

    public class AgreementModal
    {
        public int? Reqid { get; set; }
        public int ReqClientId { get; set; }
        public string? fname { get; set; }
        public string? lname { get; set; }
        public string? Reason { get; set; }
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

}