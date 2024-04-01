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
        LoginResponseViewModel patient_login(LoginModel model);
        Task<bool> IsEmailExists(string email);
         void AddPatientInfo(PatientInfoModel patientInfoModel);


         void AddFamilyReq(FamilyReqModel familyReqModel);


        void AddConciergeReq(ConciergeReqModel conciergeReqModel);

        void AddBusinessReq(BusinessReqModel businessReqModel);

        MedicalHistoryList GetMedicalHistory(int userid);
        Profile GetProfile(int userid);
        bool EditProfile(Profile profile);

        bool UploadDocuments(List<IFormFile> files, int reqId);
    

        public DocumentModel GetAllDocById(int requestId);







    }


}
