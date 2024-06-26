﻿
using DataAccess.CustomModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAdminService
    {
        Aspnetuser GetAspnetuser(string email);
        DashboardModel GetRequestsByStatus(int tabNo, int CurrentPage);

        LoginDetail GetLoginDetail(string email);

        StatusCountModel GetStatusCount();


        ViewCaseViewModel ViewCaseViewModel(int Requestclientid, int RequestTypeId);

        ViewNotesModel ViewNotes(int ReqId);

        bool UpdateAdminNotes(string additionalNotes, int reqId, int aspNetRole);



        CancelCaseModel CancelCase(int reqId);

        bool SubmitCancelCase(CancelCaseModel cancelCaseModel);

        AssignCaseModel AssignCase(int reqId);

        List<Physician> GetPhysicianByRegion(int Regionid);
        bool SubmitAssignCase(AssignCaseModel assignCaseModel);

        BlockCaseModel BlockCase(int reqId);
        bool SubmitBlockCase(BlockCaseModel blockCaseModel);

        ViewUploadModel GetAllDocById(int requestId);
        bool UploadFiles(List<IFormFile> files, int reqId);

        bool DeleteFileById(int reqFileId);

        bool DeleteAllFiles(List<string> filename, int reqId);

        Order FetchProfession();
        JsonArray FetchVendors(int selectedValue);
        Healthprofessional VendorDetails(int selectedValue);

        bool SendOrder(Order order);

        bool ClearCase(int reqId);
        

        SendAgreementModel SendAgreementCase(int reqId);

        CloseCaseModel ShowCloseCase(int reqId);

        bool SaveCloseCase(CloseCaseModel closeCaseModel);
        bool SubmitCloseCase(int ReqId);

        EncounterFormModel EncounterForm(int reqId);

        bool SubmitEncounterForm(EncounterFormModel model);

        bool AgreeAgreement(AgreementModel model);

        AgreementModel CancelAgreement(int reqId);

        bool SubmitCancelAgreement(AgreementModel model);
        int GetStatusForReviewAgreement(int reqId);

        List<AdminRegionTable> AdminRegionTable(string email);
        List<AdminRegionTable> AdminRegionTableById(int adminid);
        MyProfileModel MyProfile(string email);
        bool ResetPassword(string tokenEmail, string resetPassword);
        bool SubmitAdminInfo(MyProfileModel model, string tokenEmail);
        bool SubmitBillingInfo(MyProfileModel model, string tokenEmail);
        bool VerifyState(string state);
        void SendRegistrationEmailCreateRequest(string email, string registrationLink);
        bool CreateRequest(CreateRequestModel model, string sessionEmail, string createAccountLink);

        List<AccountAccess> AccountAccess();

        AccountAccess GetEditAccessData(int roleid);

        bool SetEditAccessAccount(AccountAccess accountAccess, List<int> AccountMenu, string sessionEmail);

        List<AccountMenu> GetAccountMenu(int accounttype, int roleid);

        List<Aspnetrole> GetAccountType();
        List<ProviderModel> GetProvider();
        List<ProviderModel> GetProviderByRegion(int regionId);

        public bool StopNotification(int phyId);

        bool ProviderContactEmail(int phyId, string msg, string tokenEmail);
        bool ProviderContactSms(int phyId, string msg, string tokenEmail);

        EditProviderModel EditProviderProfile(int phyId, string tokenEmail);

        List<Region> RegionTable();

        List<PhysicianRegionTable> PhyRegionTable(int phyId);
        bool CreateAdminAccount(CreateAdminAccount obj, List<int> AdminRegion, string email);

        CreateAdminAccount adminEditPage(int adminId);
        bool EditAdminDetailsDb(CreateAdminAccount model, string email, List<int> adminRegions);


        bool DeleteRole(int roleId);

        CreateAccess FetchRole(short selectedValue);
        bool CreateRole(List<int> menuIds, string roleName, short accountType);

        bool RoleExists(string roleName, short accountType);
        List<Physicianlocation> GetPhysicianlocations();
       

        CreateProviderAccount CreateProviderAccount(CreateProviderAccount obj, List<int> physicianRegions);
        bool providerResetPass(string email, string password);
        bool editProviderForm1(int phyId, int roleId, int statusId);
        bool editProviderForm2(string fname, string lname, string email, string phone, string medical, string npi, string sync, int phyId, int[] phyRegionArray);
        bool editProviderForm3(EditProviderModel2 dataMain);
        bool PhysicianBusinessInfoUpdate(EditProviderModel2 dataMain);
        bool EditOnBoardingData(EditProviderModel2 dataMain);
        void editProviderDeleteAccount(int phyId);

        List<UserAccess> FetchAccess(short selectedValue);

        List<BusinessTableModel> BusinessTable(string vendor, string profession);
        bool AddBusiness(AddBusinessModel obj);
        List<Healthprofessionaltype> GetProfession();
        bool RemoveBusiness(int VendorId);
        AddBusinessModel GetEditBusiness(int VendorId);
        List<RequestsRecordModel> SearchRecords(RecordsModel recordsModel);
        List<User> PatientRecords(PatientRecordsModel patientRecordsModel);
        void DeleteRecords(int reqId);
        byte[] GenerateExcelFile(List<RequestsRecordModel> recordsModel);
        List<GetRecordExplore> GetPatientRecordExplore(int userId);
        EmailSmsRecords2 EmailSmsLogs(int tempId, EmailSmsRecords2 recordsModel);
        List<BlockHistory> BlockHistory(BlockHistory2 blockHistory2);
        bool UnblockRequest(int blockId);
        bool IsBlockRequestActive(int blockId);
        DayWiseScheduling GetDayTable(string PartialName, string date, int regionid, int status);
        WeekWiseScheduling GetWeekTable(string date, int regionid, int status);
        MonthWiseScheduling GetMonthTable(string date, int regionid, int status);
        void CreateNewShiftSubmit(string selectedDays, CreateShiftModel obj, int adminId);
        bool CreateShift(SchedulingViewModel model, string Email, List<int> repeatdays);
        CreateNewShift ViewShift(int ShiftDetailId);
        bool EditShift(CreateNewShift model, string email);
        bool ReturnShift(int ShiftDetailId, string email);
        bool DeleteShift(int ShiftDetailId, string email);
        OnCallModal GetOnCallDetails(int regionId);
        List<ShiftReview> GetShiftReview(int regionId, int callId);
        bool DeleteShiftReview(int[] shiftDetailsId, string Aspid);
        bool ApproveSelectedShift(int[] shiftDetailsId, string Aspid);
        DashboardModel GetRequestByRegion(FilterModel filterModel);
        List<AdminDashTableModel> Export(int tabNo);
        List<Role> GetPhyRoles();
        List<Role> GetAdminRoles();

        List<PhysicianViewModel> GetPhysiciansForInvoicing();
        string CheckInvoicingApprove(string selectedValue, int PhysicianId);
        InvoicingViewModel GetApprovedViewData(string selectedValue, int PhysicianId);

    }
}