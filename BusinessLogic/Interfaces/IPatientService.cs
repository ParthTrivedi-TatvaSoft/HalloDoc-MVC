using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.CustomModels;

namespace BusinessLogic.Interfaces
{
    public interface IPatientService
    {
         void AddPatientInfo(PatientInfoModel patientInfoModel);


         void AddFamilyReq(FamilyReqModel familyReqModel);


        void AddConciergeReq(ConciergeReqModel conciergeReqModel);

        void AddBusinessReq(BusinessReqModel businessReqModel);

      
        Task<bool> IsEmailExists(string email);


        List<PatientDashboard> GetPatientInfos();
        List<MedicalHistory> GetMedicalHistory(string email);








    }


}
