using DataAccess.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IProviderService
    {
        bool TransferRequest(TransferRequest model);
        MonthWiseScheduling PhysicianMonthlySchedule(string date, int status, string aspnetuserid);
        int GetPhysicianId(string userid);
    }
}