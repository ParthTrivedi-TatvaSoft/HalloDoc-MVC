//using BusinessLogic.Interfaces;
//using DataAccess.CustomModels;
//using DataAccess.Data;
//using DataAccess.Enums;
//using DataAccess.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace BusinessLogic.Services
//{
//    public class ProviderService : IProviderService
//    {
//        private readonly ApplicationDbContext _db;

//        public ProviderService(ApplicationDbContext db)
//        {
//            _db = db;
//        }

//        //public bool TransferRequest(TransferRequest model)
//        //{
//        //    var req = _db.Requests.Where(x => x.Requestid == model.ReqId).FirstOrDefault();
//        //    if (req != null)
//        //    {
//        //        req.Status = (int)StatusEnum.Unassigned;
//        //        req.Modifieddate = DateTime.Now;
//        //        _db.Requests.Update(req);

//        //        Requeststatuslog rsl = new Requeststatuslog();
//        //        rsl.Requestid = (int)model.ReqId;
//        //        rsl.Status = (int)StatusEnum.Unassigned;
//        //        rsl.Notes = model.description;
//        //        rsl.Createddate = DateTime.Now;
//        //        _db.Requeststatuslogs.Add(rsl);
//        //        _db.SaveChanges();

//        //        return true;
//        //    }
//        //    return false;
//        //}
//    }
//}