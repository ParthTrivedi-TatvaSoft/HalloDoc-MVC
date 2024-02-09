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
    }
}
