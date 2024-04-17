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
                _db.Requests.Update(req);

                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = (int)model.ReqId;
                rsl.Status = (int)StatusEnum.Unassigned;
                rsl.Notes = model.description;
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
    }
}