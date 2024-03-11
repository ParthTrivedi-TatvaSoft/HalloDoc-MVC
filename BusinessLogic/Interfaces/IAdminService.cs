
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
        List<AdminDashTableModel> GetRequestsByStatus(int status);


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

        public bool SendOrder(Order order);


    }
}