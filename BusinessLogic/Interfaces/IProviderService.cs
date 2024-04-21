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

        LoginDetail GetLoginDetail(string email);
        MonthWiseScheduling PhysicianMonthlySchedule(string date, int status, string aspnetuserid);
        int GetPhysicianId(string userid);
        void AcceptCase(int requestId, string loginUserId);
        void CallType(int requestId, short callType);
        public DashboardModel GetRequestsByStatus(int tabNo, int CurrentPage, int phyid);
        public StatusCountModel GetStatusCount(int phyid);
        void housecall(int requestId);
        bool finalizesubmit(int reqid);
        bool concludecaresubmit(int ReqId, string ProviderNote);

        void RequestAdmin(RequestAdmin model, string sessionEmail);
        void SendRegistrationEmailCreateRequest(string email, string note, string sessionEmail);
    }
}