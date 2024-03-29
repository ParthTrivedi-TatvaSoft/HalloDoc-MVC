
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
        DashboardModel GetRequestByRegion(int regionId, int tabNo);


        StatusCountModel GetStatusCount();


        ViewCaseViewModel ViewCaseViewModel(int Requestclientid, int RequestTypeId);

        ViewNotesModel ViewNotes(int ReqId);

        bool UpdateAdminNotes(string additionalNotes, int reqId);



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

        MyProfileModel MyProfile(string email);
        bool ResetPassword(string tokenEmail, string resetPassword);
        bool SubmitAdminInfo(MyProfileModel model, string tokenEmail);
        bool SubmitBillingInfo(MyProfileModel model, string tokenEmail);
        bool VerifyState(string state);

        bool CreateRequest(CreateRequestModel model, string sessionEmail);


        List<AccountAccess> AccountAccess();
        bool DeleteRole(int roleId);

        CreateAccess FetchRole(short selectedValue);
        void CreateRole(List<int> menuIds, string roleName, short accountType);
        List<ProviderModel> GetProvider();

        //ProviderModel ProviderContact(int phyId);
        public bool StopNotification(int phyId);

        bool ProviderContactEmail(int phyIdMain, string msg);

        bool CreateAdminAccount(CreateAdminAccount createNewAccount,string email);

        CreateAdminAccount RegionList();

        List<Physicianlocation> GetPhysicianlocations();
    }
}