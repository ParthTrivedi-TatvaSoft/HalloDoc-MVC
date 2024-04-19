using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Enums;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLogic.Services
{
    public class ProviderService : IProviderService
    {
        private readonly ApplicationDbContext _db;

        public ProviderService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool TransferRequest(TransferRequest model)
        {
            var req = _db.Requests.Where(x => x.Requestid == model.ReqId).FirstOrDefault();
            if (req != null)
            {
                req.Status = (int)StatusEnum.Unassigned;
                req.Modifieddate = DateTime.Now;
                req.Physicianid = null;
                _db.Requests.Update(req);

                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = (int)model.ReqId;
                rsl.Status = (int)StatusEnum.Unassigned;
                rsl.Notes = model.description;
                rsl.Transtoadmin = new BitArray(1, true);
                rsl.Createddate = DateTime.Now;
                _db.Requeststatuslogs.Add(rsl);
                _db.SaveChanges();

                return true;
            }
            return false;
        }

        public MonthWiseScheduling PhysicianMonthlySchedule(string date, int status, string aspnetuserid)
        {
            var currentDate = DateTime.Parse(date);
            int? phy = _db.Physicians.Where(x => x.Aspnetuserid == aspnetuserid).Select(x => x.Physicianid).FirstOrDefault();



            BitArray deletedBit = new BitArray(new[] { false });
            MonthWiseScheduling month = new MonthWiseScheduling
            {
                date = currentDate,

            };

            if (status != 0)
            {
                var shiftdetailList = from t1 in _db.Shiftdetails
                                      join t2 in _db.Shifts
                                      on t1.Shiftid equals t2.Shiftid
                                      where t1.Status == status && t2.Physicianid == phy
                                      orderby t1.Starttime ascending
                                      select t1;
                month.shiftdetails = shiftdetailList.ToList();
            }
            else
            {
                var shiftdetailList = from t1 in _db.Shiftdetails
                                      join t2 in _db.Shifts
                                      on t1.Shiftid equals t2.Shiftid
                                      where t2.Physicianid == phy
                                      orderby t1.Starttime ascending
                                      select t1;
                month.shiftdetails = shiftdetailList.ToList();
            }
            return month;
        }

        public int GetPhysicianId(string userid)
        {
            int phyid = _db.Physicians.Where(x => x.Aspnetuserid == userid).Select(x => x.Physicianid).First();
            return phyid;
        }

        public void AcceptCase(int requestId, string loginUserId)
        {

            Request? req = _db.Requests.Where(i => i.Requestid == requestId).FirstOrDefault();
            var reqStatLog = _db.Requeststatuslogs.Where(i => i.Requestid == requestId).FirstOrDefault();

            int phyId = _db.Physicians.Where(x => x.Aspnetuserid == loginUserId).Select(x => x.Physicianid).FirstOrDefault();
            Requeststatuslog requestList = new Requeststatuslog()
            {
                Requestid = requestId,
                Status = (int)StatusEnum.Accepted,
                Physicianid = phyId,
                Createddate = DateTime.Now,
                Notes = "Req Accepted By Physicion ",
            };
            _db.Add(requestList);
            req.Status = (int)StatusEnum.Accepted;

            _db.SaveChanges();

        }
        public void CallType(int requestId, short callType)
        {
            Request? req = _db.Requests.FirstOrDefault(x => x.Requestid == requestId);
            req.Calltype = callType;

            if (callType == 1)
            {
                req.Status = (int)StatusEnum.MDOnSite;

            }
            else
            {
                req.Status = (int)StatusEnum.Conclude;
            }
            _db.Requests.Update(req);
            _db.SaveChanges();

        }

        public DashboardModel GetRequestsByStatus(int tabNo, int CurrentPage, int phyid)
        {
            var query = from r in _db.Requests
                        join rc in _db.Requestclients on r.Requestid equals rc.Requestid
                        where r.Physicianid == phyid
                        select new AdminDashTableModel
                        {
                            firstName = rc.Firstname,
                            lastName = rc.Lastname,
                            intDate = rc.Intdate,
                            intYear = rc.Intyear,
                            strMonth = rc.Strmonth,
                            requestorFname = r.Firstname,
                            requestorLname = r.Lastname,
                            createdDate = r.Createddate,
                            mobileNo = rc.Phonenumber,
                            city = rc.City,
                            state = rc.State,
                            street = rc.Street,
                            zipCode = rc.Zipcode,
                            requestTypeId = r.Requesttypeid,
                            status = r.Status,
                            requestClientId = rc.Requestclientid,
                            Reqid = r.Requestid,
                            regionId = rc.Regionid,
                            callType = r.Calltype,
                            isFinalized = _db.Encounterforms.Where(x => x.Requestid == r.Requestid).Select(x => x.Isfinalized).First() ?? null
                        };


            if (tabNo == 1)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Unassigned);
            }

            else if (tabNo == 2)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Accepted);
            }
            else if (tabNo == 3)
            {

                query = query.Where(x => x.status == (int)StatusEnum.MDEnRoute || x.status == (int)StatusEnum.MDOnSite);
            }
            else if (tabNo == 4)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Conclude);
            }



            var result = query.ToList();
            int count = result.Count();
            int TotalPage = (int)Math.Ceiling(count / (double)5);
            result = result.Skip((CurrentPage - 1) * 5).Take(5).ToList();

            DashboardModel dashboardModel = new DashboardModel();
            dashboardModel.adminDashTableList = result;
            dashboardModel.regionList = _db.Regions.ToList();
            dashboardModel.TotalPage = TotalPage;
            dashboardModel.CurrentPage = CurrentPage;
            return dashboardModel;
        }

        public void housecall(int requestId)
        {
            Request? req = _db.Requests.FirstOrDefault(x => x.Requestid == requestId);
            req.Status = (int)StatusEnum.Conclude;
            _db.Requests.Update(req);
            _db.SaveChanges();
        }

        public bool finalizesubmit(int reqid)
        {
            try
            {
                var enc = _db.Encounterforms.FirstOrDefault(x => x.Requestid == reqid);
                enc.Isfinalized = true;
                _db.Encounterforms.Update(enc);
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool concludecaresubmit(int ReqId, string ProviderNote)
        {
            try
            {
                var req1 = _db.Requests.FirstOrDefault(x => x.Requestid == ReqId);
                var ise = new BitArray(1, false);
                req1.Status = (int)StatusEnum.Closed;
                req1.Isurgentemailsent = ise;
                _db.Requests.Update(req1);

                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = ReqId;
                rsl.Status = (int)StatusEnum.Closed;
                rsl.Notes = ProviderNote;
                rsl.Createddate = DateTime.Now;
                _db.Requeststatuslogs.Add(rsl);

                _db.SaveChanges();
                return true;

            }

            catch { return false; }
        }

        public StatusCountModel GetStatusCount(int phyid)
        {
            var requestsWithClients = _db.Requests
     .Join(_db.Requestclients,
         r => r.Requestid,
         rc => rc.Requestid,
         (r, rc) => new { Request = r, RequestClient = rc })
     .Where(r => r.Request.Physicianid == phyid).ToList();

            StatusCountModel statusCount = new StatusCountModel
            {
                NewCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.Unassigned),
                PendingCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.Accepted),
                ActiveCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.MDEnRoute || x.Request.Status == (int)StatusEnum.MDOnSite),
                ConcludeCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.Conclude),

            };

            return statusCount;


        }
    }
}