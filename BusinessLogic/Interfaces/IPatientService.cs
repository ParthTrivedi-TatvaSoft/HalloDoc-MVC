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
         void AddPatientInfo(PatientInfoModel patientInfoModel);


         void AddFamilyReq(FamilyReqModel familyReqModel);


        void AddConciergeReq(ConciergeReqModel conciergeReqModel);

        void AddBusinessReq(BusinessReqModel businessReqModel);

      
        Task<bool> IsEmailExists(string email);


        List<MedicalHistory> GetMedicalHistory(User user);

        void AddFile(IFormFile file, int reqId);



        IQueryable<Requestwisefile>? GetAllDocById(int requestId);






    }


}
