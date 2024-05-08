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
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using BusinessLogic.Services;
using LinqKit;

namespace BusinessLogic.Services
{
    public class ProviderService : IProviderService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGenericService<Weeklytimesheet> _weeklyTimeSheetRepo;
        private readonly IGenericService<Shiftdetail> _shiftDetailrepo;
        private readonly IGenericService<Payrate> _payRateRepo;
        private readonly IGenericService<Weeklytimesheetdetail> _weeklyTimeSheetDetailRepo;

        public ProviderService(ApplicationDbContext db, IGenericService<Weeklytimesheet> weeklyTimeSheetRepo, IGenericService<Shiftdetail> shiftDetailrepo, IGenericService<Weeklytimesheetdetail> weeklyTimeSheetDetailRepo, IGenericService<Payrate> payRateRepo)
        {
            _db = db;
            _weeklyTimeSheetRepo = weeklyTimeSheetRepo;
            _shiftDetailrepo = shiftDetailrepo;
            _weeklyTimeSheetDetailRepo = weeklyTimeSheetDetailRepo;
            _payRateRepo = payRateRepo;
        }

        public LoginDetail GetLoginDetail(string email)
        {
            var phy = _db.Physicians.Where(x => x.Email == email).FirstOrDefault();
            LoginDetail model = new();
            model.firstName = phy.Firstname;
            model.lastName = phy.Lastname;
            return model;
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
                            phyId = r.Physicianid ?? null,
                            isFinalized = _db.Encounterforms.Where(x => x.Requestid == r.Requestid).Select(x => x.Isfinalized).First() ?? null,
                            reqDate = r.Createddate.ToString("yyyy-MMM-dd"),
                            notes = _db.Requeststatuslogs
                                     .Where(x => x.Requestid == r.Requestid)
                                     .OrderBy(x => x.Requeststatuslogid)
                                     .Select(x => x.Notes)
                                     .LastOrDefault() ?? null,
                            email = rc.Email ?? null,
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
                if (enc == null)
                {
                    return false;
                }
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

        public void SendRegistrationEmailCreateRequest(string email, string note, string sessionEmail)
        {
            string senderEmail = "tatva.dotnet.parthtrivedi@outlook.com";
            string senderPassword = "Parth@70160";
            SmtpClient client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Request Note To Admin",
                IsBodyHtml = true,
                Body = $"Note: '{note}'"
            };

            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + email + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = email,
                Roleid = 3,
                Physicianid = _db.Physicians.Where(r => r.Email == sessionEmail).Select(r => r.Physicianid).First(),
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = sessionEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };


            _db.Emaillogs.Add(emailLog);
            _db.SaveChanges();


            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }

        public void RequestAdmin(RequestAdmin model, string sessionEmail)
        {
            var email = _db.Admins.ToList();

            foreach (var item in email)
            {
                try
                {
                    SendRegistrationEmailCreateRequest(item.Email, model.Note, sessionEmail);
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }


        public List<DateViewModel> GetDates()
        {
            List<DateViewModel> dates = new List<DateViewModel>();
            int startMonth = 0;
            int startYear = 0;
            int startDate = 1;
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int nextDate = 1;
            if (today.Day > 15)
            {
                nextDate = 2;
            }
            if (today.Month - 6 < 0)
            {
                startMonth = 12 - (6 - today.Month) + 1;
                startYear = today.Year - 1;
            }
            else if (today.Month - 6 == 0)
            {
                startMonth = 1;
                startYear = today.Year;
            }
            else
            {
                startMonth = today.Month - 6;
                startYear = today.Year;
            }
            int count = 12;
            if (nextDate == 1)
            {
                count = 11;
            }
            for (int i = 1; i <= count; i++)
            {

                if (i % 2 == 0)
                {
                    startDate = 16;
                }
                else
                {
                    startDate = 1;

                }
                if (startMonth > 12)
                {
                    startMonth = 1;
                    startYear = today.Year;
                }
                DateViewModel date = new DateViewModel();
                date.StartDate = new DateOnly(startYear, startMonth, startDate);
                if (startDate != 1)
                    date.EndDate = date.StartDate.AddMonths(1).AddDays(-16);
                else
                    date.EndDate = new DateOnly(startYear, startMonth, 15);
                dates.Add(date);
                if (startDate == 16)
                    startMonth += 1;
            }
            dates.Reverse();
            return dates;
        }


        public InvoicingViewModel GetInvoicingDataonChangeOfDate(DateOnly startDate, DateOnly endDate, int? PhysicianId, int? AdminID)
        {
            InvoicingViewModel model = new InvoicingViewModel();
            var weeklyTimeSheet = _db.Weeklytimesheets.FirstOrDefault(u => u.ProviderId == PhysicianId && (u.StartDate == startDate && u.EndDate == endDate));
            if (weeklyTimeSheet != null)
            {
                var TimehseetsData = _weeklyTimeSheetDetailRepo.SelectWhereOrderBy(x => new Timesheet
                {
                    Date = x.Date,
                    NumberofShift = x.NumberOfShifts ?? 0,
                    NightShiftWeekend = x.IsWeekendHoliday == true ? 1 : 0,
                    NumberOfHouseCall = (x.IsWeekendHoliday == false ? x.HouseCall : 0) ?? 0,
                    HousecallNightsWeekend = (x.IsWeekendHoliday == true ? x.HouseCall : 0) ?? 0,
                    NumberOfPhoneConsults = (x.IsWeekendHoliday == false ? x.PhoneConsult : 0) ?? 0,
                    phoneConsultNightsWeekend = (x.IsWeekendHoliday == true ? x.PhoneConsult : 0) ?? 0,
                    BatchTesting = x.BatchTesting ?? 0
                }, x => x.TimeSheetId == weeklyTimeSheet.TimeSheetId, x => x.Date);
                List<Timesheet> list = new List<Timesheet>();
                foreach (Timesheet item in TimehseetsData)
                {
                    list.Add(item);
                }
                model.timesheets = list;
                model.PhysicianId = PhysicianId ?? 0;
                model.IsFinalized = weeklyTimeSheet.IsFinalized == true ? true : false;
                model.startDate = startDate;
                model.endDate = endDate;
                model.Status = weeklyTimeSheet.Status == 1 ? "Pending" : "Aprooved";
            }
            else
            {
                model.timesheets = new List<Timesheet>();
            }
            model.isAdminSide = AdminID == null ? false : true;
            return model;
        }


        public InvoicingViewModel GetUploadedDataonChangeOfDate(DateOnly startDate, DateOnly endDate, int? PhysicianId, int pageNumber, int pagesize)
        {
            Weeklytimesheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && (u.StartDate == startDate && u.EndDate == endDate));
            InvoicingViewModel model = new InvoicingViewModel();
            Expression<Func<Weeklytimesheetdetail, bool>> whereClauseSyntax = PredicateBuilder.New<Weeklytimesheetdetail>();
            whereClauseSyntax = x => x.Bill != null && x.TimeSheetId == weeklyTimeSheet.TimeSheetId;
            if (weeklyTimeSheet != null)
            {
                var UploadedItems = _weeklyTimeSheetDetailRepo.GetAllWithPagination(x => new Timesheet
                {
                    Date = x.Date,
                    Items = x.Item ?? "-",
                    Amount = x.Amount ?? 0,
                    BillName = x.Bill ?? "-",
                }, whereClauseSyntax, pageNumber, pagesize, x => x.Date, true);
                List<Timesheet> list = new List<Timesheet>();
                foreach (Timesheet item in UploadedItems)
                {
                    list.Add(item);
                }
                model.timesheets = list;

                model.pager = new Pager
                {
                    TotalItems = _weeklyTimeSheetDetailRepo.GetTotalcount(whereClauseSyntax),
                    CurrentPage = pageNumber,
                    ItemsPerPage = pagesize
                };
                model.SkipCount = (pageNumber - 1) * pagesize;
                model.CurrentPage = pageNumber;
                model.TotalPages = (int)Math.Ceiling((decimal)model.pager.TotalItems / pagesize);
                model.IsFinalized = weeklyTimeSheet.IsFinalized == true ? true : false;
            }
            model.PhysicianId = PhysicianId ?? 0;
            return model;
        }

        public InvoicingViewModel getDataOfTimesheet(DateOnly startDate, DateOnly endDate, int? PhysicianId, int? AdminID)
        {
            InvoicingViewModel model = new InvoicingViewModel();
            model.startDate = startDate;
            model.endDate = endDate;
            model.differentDays = endDate.Day - startDate.Day;
            Weeklytimesheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && u.StartDate == startDate && u.EndDate == endDate);
            Expression<Func<Weeklytimesheetdetail, bool>> whereClauseSyntax1 = PredicateBuilder.New<Weeklytimesheetdetail>();


            if (weeklyTimeSheet != null)
            {
                Payrate payRate = _payRateRepo.GetFirstOrDefault(u => u.PhysicianId == weeklyTimeSheet.ProviderId);
                whereClauseSyntax1 = x => x.TimeSheet!.ProviderId == PhysicianId && x.TimeSheet.StartDate == startDate && x.TimeSheet.EndDate == endDate;
                model.TimeSheetId = weeklyTimeSheet.TimeSheetId;
                var ExistingTimeshet = _weeklyTimeSheetDetailRepo.SelectWhereOrderBy(x => new Timesheet
                {
                    Date = x.Date,
                    NumberOfHouseCall = x.HouseCall ?? 0,
                    NumberOfPhoneConsults = x.PhoneConsult ?? 0,
                    Weekend = x.IsWeekendHoliday ?? false,
                    TotalHours = x.TotalHours ?? 0,
                    OnCallhours = x.NumberOfShifts ?? 0,
                    Amount = x.Amount ?? 0,
                    Items = x.Item,
                    BillName = x.Bill,
                    WeeklyTimesheetDeatilsId = x.TimeSheetDetailId,
                }, whereClauseSyntax1, x => x.Date);
                List<Timesheet> list = new List<Timesheet>();
                foreach (Timesheet item in ExistingTimeshet)
                {
                    model.shiftTotal += (item.TotalHours * payRate.Shift) ?? 0;
                    model.weekendTotal += item.Weekend == true ? (1 * payRate.NightShiftWeekend) ?? 0 : 0;
                    model.HouseCallTotal += (item.NumberOfHouseCall * payRate.HouseCall) ?? 0;
                    model.phoneconsultTotal += (item.NumberOfPhoneConsults * payRate.PhoneConsult) ?? 0;
                    list.Add(item);
                }
                model.timesheets = list;
                model.shiftPayrate = payRate.Shift ?? 0;
                model.weekendPayrate = payRate.NightShiftWeekend ?? 0;
                model.HouseCallPayrate = payRate.HouseCall ?? 0;
                model.phoneConsultPayrate = payRate.PhoneConsult ?? 0;
                model.GrandTotal = model.shiftTotal + model.weekendTotal + model.HouseCallTotal + model.phoneconsultTotal;

            }
            else
            {
                DateOnly currentDate = startDate;
                while (currentDate <= endDate)
                {
                    model.timesheets.Add(new Timesheet
                    {
                        Date = currentDate,

                    });

                    currentDate = currentDate.AddDays(1);
                }
            }
            model.startDate = startDate;
            model.endDate = endDate;
            model.PhysicianId = PhysicianId ?? 0;
            model.IsFinalized = weeklyTimeSheet == null ? false : true;
            model.isAdminSide = AdminID == null ? false : true;
            return model;

        }

        public void AprooveTimeSheet(InvoicingViewModel model, int? AdminID)
        {
            Weeklytimesheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == model.PhysicianId && u.StartDate == model.startDate && u.EndDate == model.endDate);
            if (weeklyTimeSheet != null)
            {
                weeklyTimeSheet.AdminId = AdminID;
                weeklyTimeSheet.Status = 2;
                //weeklyTimeSheet.BonusAmount = model.BonusAmount;
                weeklyTimeSheet.AdminNote = model.AdminNotes;
                _weeklyTimeSheetRepo.Update(weeklyTimeSheet);
            }
        }

        public void SubmitTimeSheet(InvoicingViewModel model, int? PhysicianId)
        {
            Weeklytimesheet existingWeekltTimesheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && u.StartDate == model.startDate && u.EndDate == model.endDate);
            if (existingWeekltTimesheet == null)
            {
                Weeklytimesheet weeklyTimeSheet = new Weeklytimesheet();
                weeklyTimeSheet.StartDate = model.startDate;
                weeklyTimeSheet.EndDate = model.endDate;
                weeklyTimeSheet.Status = 1;
                weeklyTimeSheet.CreatedDate = DateTime.Now;
                weeklyTimeSheet.ProviderId = PhysicianId ?? 0;
                _weeklyTimeSheetRepo.Add(weeklyTimeSheet);

                foreach (var item in model.timesheets)
                {
                    BitArray deletedBit = new BitArray(new[] { false });

                    Weeklytimesheetdetail detail = new Weeklytimesheetdetail();
                    detail.Date = item.Date;
                    detail.NumberOfShifts = _shiftDetailrepo.Count(u => u.Shift.Physicianid == PhysicianId && u.Shiftdate == item.Date && u.Isdeleted == deletedBit);
                    detail.TotalHours = item.TotalHours;
                    detail.IsWeekendHoliday = item.Weekend;
                    detail.HouseCall = item.NumberOfHouseCall;
                    detail.PhoneConsult = item.NumberOfPhoneConsults;
                    detail.TimeSheetId = weeklyTimeSheet.TimeSheetId;
                    if (item.Bill != null)
                    {
                        IFormFile newFile = item.Bill;
                        detail.Bill = newFile.FileName;
                        var filePath = Path.Combine("wwwroot", "UploadedFiles", "ProviderBills", PhysicianId + "-" + item.Date + "-" + Path.GetFileName(newFile.FileName));
                        using (FileStream stream = System.IO.File.Create(filePath))
                        {
                            newFile.CopyTo(stream);
                        }
                    }
                    detail.Item = item.Items;
                    detail.Amount = item.Amount;
                    _weeklyTimeSheetDetailRepo.Add(detail);
                }
            }
            else
            {
                var exsitingTimeSheetDetail = _weeklyTimeSheetDetailRepo.GetList(u => u.TimeSheetId == existingWeekltTimesheet.TimeSheetId && u.Date >= model.startDate && u.Date <= model.endDate);
                List<Weeklytimesheetdetail> list = new List<Weeklytimesheetdetail>();

                for (int i = 0; i < model.timesheets.Count; i++)
                {
                    var currentDate = model.timesheets[i].Date;
                    Weeklytimesheetdetail weeklyTimeSheetDetail = exsitingTimeSheetDetail.FirstOrDefault(detail => detail.Date == currentDate)!;
                    if (weeklyTimeSheetDetail != null)
                    {
                        weeklyTimeSheetDetail.Date = model.timesheets[i].Date;
                        weeklyTimeSheetDetail.HouseCall = model.timesheets[i].NumberOfHouseCall;
                        weeklyTimeSheetDetail.PhoneConsult = model.timesheets[i].NumberOfPhoneConsults;
                        weeklyTimeSheetDetail.Item = model.timesheets[i].Items ?? null;
                        weeklyTimeSheetDetail.Amount = model.timesheets[i].Amount;
                        weeklyTimeSheetDetail.TotalHours = model.timesheets[i].TotalHours;
                        weeklyTimeSheetDetail.IsWeekendHoliday = model.timesheets[i].Weekend;
                        if (model.timesheets[i].Bill != null && model.timesheets[i].Bill!.Length > 0)
                        {
                            IFormFile newFile = model.timesheets[i].Bill!;
                            weeklyTimeSheetDetail.Bill = newFile.FileName;
                            var filePath = Path.Combine("wwwroot", "Uploaded_files", "ProviderBills", PhysicianId + "-" + model.timesheets[i].Date + "-" + Path.GetFileName(newFile.FileName));
                            FileStream stream = null;
                            try
                            {
                                stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                                newFile.CopyToAsync(stream)
;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                        list.Add(weeklyTimeSheetDetail);
                    }
                }
                foreach (Weeklytimesheetdetail item in list)
                {
                    _weeklyTimeSheetDetailRepo.Update(item);
                }
            }

        }

        public void DeleteBill(int id)
        {
            Weeklytimesheetdetail weeklyTimeSheetDetail = _weeklyTimeSheetDetailRepo.GetFirstOrDefault(u => u.TimeSheetDetailId == id);
            weeklyTimeSheetDetail.Bill = null;
            _weeklyTimeSheetDetailRepo.Update(weeklyTimeSheetDetail);

        }
        public void FinalizeTimeSheet(int id)
        {
            Weeklytimesheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.TimeSheetId == id);
            if (weeklyTimeSheet != null)
            {
                weeklyTimeSheet.IsFinalized = true;
                _weeklyTimeSheetRepo.Update(weeklyTimeSheet);
            }
        }
    }
}