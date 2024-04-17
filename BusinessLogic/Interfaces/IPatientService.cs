using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CustomModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Interfaces
{
    public interface IPatientService
    {
        Aspnetuser GetAspnetuser(string email);
        LoginResponseViewModel patient_login(LoginModel model);
        bool IsEmailExists(string email);
        bool IsPasswordExists(string email);
        bool CreateAccount(CreateAccountModel model);
        bool AddPatientInfo(PatientInfoModel patientInfoModel);

        MedicalHistoryList GetMedicalHistory(string email);
        Profile GetProfile(int userid);
        bool EditProfile(Profile profile);

        bool UploadDocuments(List<IFormFile> files, int reqId);
    

        public DocumentModel GetAllDocById(int requestId);
        void SendRegistrationEmailCreateRequest(string email, string registrationLink);
        bool AddFamilyReq(FamilyReqModel familyReqModel, string createAccountLink);
        bool AddConciergeReq(ConciergeReqModel conciergeReqModel, string createAccountLink);
        bool AddBusinessReq(BusinessReqModel businessReqModel, string createAccountLink);

        public PatientInfoModel FetchData(string email);
    }


}
