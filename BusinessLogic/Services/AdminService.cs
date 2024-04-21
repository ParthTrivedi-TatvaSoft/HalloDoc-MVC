using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Enums;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Globalization;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BusinessLogic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public AdminService(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }


        public Aspnetuser GetAspnetuser(string email)
        {
            var aspNetUser = _db.Aspnetusers.Include(x => x.Aspnetuserroles).FirstOrDefault(x => x.Email == email);
            return aspNetUser;
        }

        public LoginDetail GetLoginDetail(string email)
        {
            var admin = _db.Admins.Where(x => x.Email == email).FirstOrDefault();
            LoginDetail model = new();
            model.firstName = admin.Firstname;
            model.lastName = admin.Lastname;
            return model;
        }

        public DashboardModel GetRequestsByStatus(int tabNo, int CurrentPage)
        {
            var query = from r in _db.Requests
                        join rc in _db.Requestclients on r.Requestid equals rc.Requestid
                        //where r.Status == status
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
                            phyId = r.Physicianid ?? null

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
            else if (tabNo == 5)
            {

                query = query.Where(x => (x.status == (int)StatusEnum.Cancelled || x.status == (int)StatusEnum.CancelledByPatient) || x.status == (int)StatusEnum.Closed);
            }
            else if (tabNo == 6)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Unpaid);
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



        public StatusCountModel GetStatusCount()
        {
            var requestsWithClients = _db.Requests
     .Join(_db.Requestclients,
         r => r.Requestid,
         rc => rc.Requestid,
         (r, rc) => new { Request = r, RequestClient = rc })
     .ToList();

            StatusCountModel statusCount = new StatusCountModel
            {
                NewCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.Unassigned),
                PendingCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.Accepted),
                ActiveCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.MDEnRoute || x.Request.Status == (int)StatusEnum.MDOnSite),
                ConcludeCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.Conclude),
                ToCloseCount = requestsWithClients.Count(x => (x.Request.Status == (int)StatusEnum.Cancelled || x.Request.Status == (int)StatusEnum.CancelledByPatient) || x.Request.Status == (int)StatusEnum.Closed),
                UnpaidCount = requestsWithClients.Count(x => x.Request.Status == (int)StatusEnum.Unpaid)
            };
            return statusCount;
        }

        public List<AdminDashTableModel> Export(int tabNo)
        {
            var query = from r in _db.Requests
                        join rc in _db.Requestclients on r.Requestid equals rc.Requestid
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
                            phyId = r.Physicianid ?? null
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
            else if (tabNo == 5)
            {

                query = query.Where(x => (x.status == (int)StatusEnum.Cancelled || x.status == (int)StatusEnum.CancelledByPatient) || x.status == (int)StatusEnum.Closed);
            }
            else if (tabNo == 6)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Unpaid);
            }
            var result = query.ToList();
            return result;

        }

        public DashboardModel GetRequestByRegion(FilterModel filterModel)
        {

            var query = from r in _db.Requests
                        join rc in _db.Requestclients on r.Requestid equals rc.Requestid
                        //where r.Status == status
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
                            phyId = r.Physicianid ?? null
                        };


            if (filterModel.tabNo == 1)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Unassigned);
            }

            else if (filterModel.tabNo == 2)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Accepted);
            }
            else if (filterModel.tabNo == 3)
            {

                query = query.Where(x => x.status == (int)StatusEnum.MDEnRoute || x.status == (int)StatusEnum.MDOnSite);
            }
            else if (filterModel.tabNo == 4)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Conclude);
            }
            else if (filterModel.tabNo == 5)
            {

                query = query.Where(x => (x.status == (int)StatusEnum.Cancelled || x.status == (int)StatusEnum.CancelledByPatient) || x.status == (int)StatusEnum.Closed);
            }
            else if (filterModel.tabNo == 6)
            {

                query = query.Where(x => x.status == (int)StatusEnum.Unpaid);
            }

            if (filterModel.regionId != null)
            {
                query = query.Where(x => x.regionId == filterModel.regionId);
            }
            if (filterModel.searchWord != null)
            {
                query = query.Where(x => x.firstName.Trim().ToLower().Contains(filterModel.searchWord.Trim().ToLower()));
            }
            if (filterModel.requestTypeId != null)
            {
                query = query.Where(x => x.requestTypeId == filterModel.requestTypeId);
            }

            var result = query.ToList();
            int count = result.Count();
            int TotalPage = (int)Math.Ceiling(count / (double)5);
            result = result.Skip((filterModel.CurrentPage - 1) * 5).Take(5).ToList();

            DashboardModel dashboardModel = new DashboardModel();
            dashboardModel.adminDashTableList = result;
            dashboardModel.regionList = _db.Regions.ToList();
            dashboardModel.TotalPage = TotalPage;
            dashboardModel.CurrentPage = filterModel.CurrentPage;
            return dashboardModel;
        }


        public ViewNotesModel ViewNotes(int ReqId)
        {

            var requestNotes = _db.Requestnotes.Where(x => x.Requestid == ReqId).FirstOrDefault();
            var statuslogs = _db.Requeststatuslogs.Where(x => x.Requestid == ReqId).ToList();
            ViewNotesModel model = new ViewNotesModel();
            if (model == null)
            {
                model.TransferNotesList = null;
                model.PhysicianNotes = null;
                model.AdminNotes = null;
            }


            if (requestNotes != null)
            {
                model.PhysicianNotes = requestNotes.Physiciannotes;
                model.AdminNotes = requestNotes.Adminnotes;
            }
            if (statuslogs != null)
            {
                model.TransferNotesList = statuslogs;
            }

            return model;
        }

        public bool UpdateAdminNotes(string additionalNotes, int reqId, int aspNetRole)
        {
            var reqNotes = _db.Requestnotes.FirstOrDefault(x => x.Requestid == reqId);
            try
            {

                if (reqNotes == null)
                {
                    Requestnote rn = new Requestnote();
                    rn.Requestid = reqId;
                    if (aspNetRole == 1)
                    {
                        rn.Adminnotes = additionalNotes;
                        rn.Createdby = "Admin";
                    }
                    else
                    {
                        rn.Physiciannotes = additionalNotes;
                        rn.Createdby = "Physician";
                    }

                    //here instead of admin , add id of the admin through which admin is loggedIn 
                    rn.Createddate = DateTime.Now;
                    _db.Requestnotes.Add(rn);
                    _db.SaveChanges();
                }
                else
                {
                    if (aspNetRole == 1)
                    {
                        reqNotes.Adminnotes = additionalNotes;
                        reqNotes.Modifiedby = "Admin";
                    }
                    else
                    {
                        reqNotes.Physiciannotes = additionalNotes;
                        reqNotes.Modifiedby = "Physician";
                    }
                    reqNotes.Modifieddate = DateTime.Now;
                    //here instead of admin , add id of the admin through which admin is loggedIn 
                    _db.Requestnotes.Update(reqNotes);
                    _db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public ViewCaseViewModel ViewCaseViewModel(int Requestclientid, int RequestTypeId)
        {
            Requestclient obj = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == Requestclientid);
            ViewCaseViewModel viewCaseViewModel = new()
            {
                RequestId = obj.Requestid,
                Requestclientid = obj.Requestclientid,
                Firstname = obj.Firstname,
                Lastname = obj.Lastname,
                Email = obj.Email,
                Phonenumber = obj.Phonenumber,
                City = obj.City,
                Street = obj.Street,
                State = obj.State,
                Zipcode = obj.Zipcode,
                Room = obj.Address,
                Notes = obj.Notes,
                RequestTypeId = RequestTypeId
            };
            return viewCaseViewModel;
        }


        public CancelCaseModel CancelCase(int reqId)
        {
            var casetags = _db.Casetags.ToList();
            var request = _db.Requests.Where(x => x.Requestid == reqId).FirstOrDefault();
            CancelCaseModel model = new()
            {
                PatientFName = request.Firstname,
                PatientLName = request.Lastname,
                casetaglist = casetags

            };
            return model;
        }

        public bool SubmitCancelCase(CancelCaseModel cancelCaseModel)
        {
            try
            {
                var req = _db.Requests.Where(x => x.Requestid == cancelCaseModel.reqId).FirstOrDefault();
                req.Status = (int)StatusEnum.Cancelled;
                req.Casetag = cancelCaseModel.casetag.ToString();
                req.Modifieddate = DateTime.Now;
                var reqStatusLog = _db.Requeststatuslogs.Where(x => x.Requestid == cancelCaseModel.reqId).FirstOrDefault();
                if (reqStatusLog == null)
                {
                    Requeststatuslog rsl = new Requeststatuslog();
                    rsl.Requestid = (int)cancelCaseModel.reqId;
                    rsl.Status = (int)StatusEnum.Cancelled;
                    rsl.Notes = cancelCaseModel.notes;
                    rsl.Createddate = DateTime.Now;
                    _db.Requeststatuslogs.Add(rsl);
                    _db.Requests.Update(req);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    reqStatusLog.Status = (int)StatusEnum.Cancelled;
                    reqStatusLog.Notes = cancelCaseModel.notes;

                    _db.Requeststatuslogs.Update(reqStatusLog);
                    _db.Requests.Update(req);
                    _db.SaveChanges();
                    return true;
                }




            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public AssignCaseModel AssignCase(int reqId)
        {

            var regionlist = _db.Regions.ToList();
            AssignCaseModel assignCaseModel = new()
            {
                regionList = regionlist,

            };
            return assignCaseModel;
        }



        public List<Physician> GetPhysicianByRegion(int Regionid)
        {

            var physicianList = _db.Physicianregions.Where(x => x.Regionid == Regionid).Select(x => x.Physician).ToList();

            return physicianList;


        }

        public bool SubmitAssignCase(AssignCaseModel assignCaseModel)
        {
            try
            {

                var req = _db.Requests.Where(x => x.Requestid == assignCaseModel.ReqId).FirstOrDefault();
                req.Physicianid = assignCaseModel.selectPhysicianId;
                req.Modifieddate = DateTime.Now;


                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = (int)assignCaseModel.ReqId;
                rsl.Status = (int)StatusEnum.Unassigned;
                rsl.Notes = assignCaseModel.description;
                rsl.Physicianid = assignCaseModel.selectPhysicianId;
                rsl.Createddate = DateTime.Now;
                _db.Requeststatuslogs.Add(rsl);
                _db.Requests.Update(req);
                _db.SaveChanges();
                return true;



            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public BlockCaseModel BlockCase(int reqId)
        {
            var reqClient = _db.Requestclients.Where(x => x.Requestid == reqId).FirstOrDefault();
            BlockCaseModel model = new()
            {
                ReqId = reqId,
                firstName = reqClient.Firstname,
                lastName = reqClient.Lastname,
                reason = null
            };

            return model;
        }

        public bool SubmitBlockCase(BlockCaseModel blockCaseModel)
        {
            try
            {
                var request = _db.Requests.FirstOrDefault(r => r.Requestid == blockCaseModel.ReqId);
                if (request != null)
                {
                    if (request.Isdeleted == null)
                    {
                        request.Isdeleted = new BitArray(1);
                        request.Isdeleted[0] = true;
                        request.Status = (int)StatusEnum.Clear;
                        request.Modifieddate = DateTime.Now;

                        _db.Requests.Update(request);

                    }
                    Blockrequest blockrequest = new Blockrequest();

                    blockrequest.Phonenumber = request.Phonenumber == null ? "+91" : request.Phonenumber;
                    blockrequest.Email = request.Email;
                    blockrequest.Reason = blockCaseModel.reason;
                    blockrequest.Requestid = (int)blockCaseModel.ReqId;
                    blockrequest.Createddate = DateTime.Now;

                    _db.Blockrequests.Add(blockrequest);
                    _db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public Order FetchProfession()
        {


            var Healthprofessionaltype = _db.Healthprofessionaltypes.ToList();

            Order order = new()
            {
                Profession = Healthprofessionaltype

            };
            return order;
        }
        public JsonArray FetchVendors(int proffesionId)
        {
            var result = new JsonArray();
            IEnumerable<Healthprofessional> businesses = _db.Healthprofessionals.Where(prof => prof.Profession == proffesionId);

            foreach (Healthprofessional business in businesses)
            {
                result.Add(new { businessId = business.Vendorid, businessName = business.Vendorname });
            }
            return result;
        }

        public Healthprofessional VendorDetails(int selectedValue)
        {
            Healthprofessional business = _db.Healthprofessionals.First(prof => prof.Vendorid == selectedValue);

            return business;
        }

        public bool SendOrder(Order order)
        {
            try
            {
                Orderdetail od = new Orderdetail();
                od.Vendorid = order.BusinessId;
                od.Requestid = order.ReqId;
                od.Faxnumber = order.faxnumber;
                od.Email = order.email;
                od.Businesscontact = order.BusineesContact;
                od.Prescription = order.orderDetail;
                od.Noofrefill = order.RefilNo;
                od.Createddate = DateTime.Now;
                od.Createdby = "Admin";

                _db.Orderdetails.Add(od);
                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ClearCase(int reqId)
        {
            try
            {
                var request = _db.Requests.FirstOrDefault(x => x.Requestid == reqId);
                if (request != null)
                {
                    request.Status = (int)StatusEnum.Clear;
                    _db.Requests.Update(request);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }





        public ViewUploadModel GetAllDocById(int requestId)
        {

            var list = _db.Requestwisefiles.Where(x => x.Requestid == requestId).ToList();
            var reqClient = _db.Requestclients.Where(x => x.Requestid == requestId).FirstOrDefault();

            ViewUploadModel result = new()
            {
                files = list,
                firstName = reqClient.Firstname,
                lastName = reqClient.Lastname,

            };

            return result;

        }


        public SendAgreementModel SendAgreementCase(int reqId)
        {
            var requestClient = _db.Requestclients.Where(x => x.Requestid == reqId).FirstOrDefault();
            SendAgreementModel obj = new();
            obj.Reqid = reqId;
            obj.PhoneNumber = requestClient.Phonenumber;
            obj.Email = requestClient.Email;

            return obj;
        }



        public CloseCaseModel ShowCloseCase(int reqId)
        {
            var rc = _db.Requestclients.FirstOrDefault(x => x.Requestid == reqId);

            var list = _db.Requestwisefiles.Where(x => x.Requestid == reqId).ToList();

            CloseCaseModel model = new CloseCaseModel();

            model.reqid = reqId;
            model.fname = rc.Firstname;
            model.lname = rc.Lastname;
            model.email = rc.Email;
            model.phoneNo = rc.Phonenumber;
            model.files = list;
            if (rc.Intdate == null || rc.Intyear == null || rc.Strmonth == null)
            {
                model.DateOfBirth = new DateTime(Convert.ToInt32(2000), DateTime.ParseExact("Jan", "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(1));
            }
            else
            {
                model.DateOfBirth = new DateTime(Convert.ToInt32(rc.Intyear), DateTime.ParseExact(rc.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(rc.Intdate));
            }



            return model;
        }

        public bool SaveCloseCase(CloseCaseModel model)
        {
            try
            {
                var reqClient = _db.Requestclients.FirstOrDefault(x => x.Requestid == model.reqid);
                reqClient.Phonenumber = model.phoneNo;
                reqClient.Email = model.email;
                _db.Requestclients.Update(reqClient);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool SubmitCloseCase(int ReqId)
        {
            try
            {
                var request = _db.Requests.FirstOrDefault(x => x.Requestid == ReqId);
                request.Status = (int)StatusEnum.Unpaid;
                _db.Requests.Update(request);
                _db.Requeststatuslogs.Add(new Requeststatuslog()
                {
                    Requestid = ReqId,
                    Status = (int)StatusEnum.Unpaid,
                    Notes = "Case closed and unpaid",
                    Createddate = DateTime.Now,
                });
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }



        public bool UploadFiles(List<IFormFile> files, int reqId)
        {

            try
            {
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {

                        //get file name
                        var fileName = Path.GetFileName(file.FileName);

                        //define path
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                        // Copy the file to the desired location
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream)
                   ;
                        }

                        Requestwisefile requestwisefile = new()
                        {
                            Filename = fileName,
                            Requestid = reqId,
                            Createddate = DateTime.Now
                        };

                        _db.Requestwisefiles.Add(requestwisefile);


                    }
                    _db.SaveChanges();
                    return true;
                }
                else { return false; }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DeleteFileById(int reqFileId)
        {
            try
            {
                var reqWiseFile = _db.Requestwisefiles.Where(x => x.Requestwisefileid == reqFileId).FirstOrDefault();
                if (reqWiseFile != null)
                {
                    _db.Requestwisefiles.Remove(reqWiseFile);
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }







        public bool DeleteAllFiles(List<string> filenames, int reqId)
        {
            try
            {
                var list = _db.Requestwisefiles.Where(x => x.Requestid == reqId).ToList();

                foreach (var filename in filenames)
                {
                    var existFile = list.Where(x => x.Filename == filename && x.Requestid == reqId).FirstOrDefault();
                    if (existFile != null)
                    {
                        list.Remove(existFile);
                        _db.Requestwisefiles.Remove(existFile);
                    }
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public int GetStatusForReviewAgreement(int reqId)
        {
            var status = (int)_db.Requests.Where(x => x.Requestid == reqId).Select(x => x.Status).FirstOrDefault();
            return status;
        }

        public bool AgreeAgreement(AgreementModel model)
        {
            try
            {
                var req = _db.Requests.FirstOrDefault(x => x.Requestid == model.reqId);


                req.Status = (int)StatusEnum.MDEnRoute;
                req.Modifieddate = DateTime.Now;
                Requeststatuslog rsl = new Requeststatuslog();
                rsl.Requestid = req.Requestid;
                rsl.Status = (int)StatusEnum.MDEnRoute;
                rsl.Createddate = DateTime.Now;
                rsl.Notes = "Agreement Accepted By Patient";
                _db.Requests.Update(req);
                _db.Requeststatuslogs.Add(rsl);
                _db.SaveChanges();
                return true;
            }

            catch (Exception e)
            {
                return false;
            }

        }


        public AgreementModel CancelAgreement(int reqId)
        {
            var requestclient = _db.Requestclients.FirstOrDefault(x => x.Requestid == reqId);
            AgreementModel model = new()
            {
                reqId = reqId,
                fName = requestclient.Firstname,
                lName = requestclient.Lastname,
            };
            return model;
        }

        public bool SubmitCancelAgreement(AgreementModel model)
        {
            try
            {
                var request = _db.Requests.FirstOrDefault(x => x.Requestid == model.reqId);


                if (request != null)
                {
                    request.Status = (int)StatusEnum.CancelledByPatient;
                    request.Modifieddate = DateTime.Now;
                    Requeststatuslog rsl = new Requeststatuslog();
                    rsl.Requestid = request.Requestid;
                    rsl.Status = (int)StatusEnum.CancelledByPatient;
                    rsl.Notes = model.reason;
                    rsl.Createddate = DateTime.Now;

                    _db.Requests.Update(request);
                    _db.Requeststatuslogs.Add(rsl);
                    _db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public EncounterFormModel EncounterForm(int reqId)
        {
            var reqClient = _db.Requestclients.FirstOrDefault(x => x.Requestid == reqId);
            var encForm = _db.Encounterforms.FirstOrDefault(x => x.Requestid == reqId);
            EncounterFormModel ef = new EncounterFormModel();
            ef.reqid = reqId;
            ef.FirstName = reqClient.Firstname;
            ef.LastName = reqClient.Lastname;
            ef.Location = reqClient.Street + " " + reqClient.City + " " + reqClient.State + " " + reqClient.Zipcode;
            //ef.BirthDate = new DateTime((int)(reqClient.Intyear), Convert.ToInt16(reqClient.Strmonth), (int)(reqClient.Intdate)).ToString("yyyy-MM-dd");
            ef.PhoneNumber = reqClient.Phonenumber;
            ef.Email = reqClient.Email;
            if (encForm != null)
            {
                ef.HistoryIllness = encForm.Illnesshistory;
                ef.MedicalHistory = encForm.Medicalhistory;
                //ef.Date = new DateTime((int)(encForm.Intyear), Convert.ToInt16(encForm.Strmonth), (int)(encForm.Intdate)).ToString("yyyy-MM-dd");
                ef.Medications = encForm.Medications;
                ef.Allergies = encForm.Allergies;
                ef.Temp = encForm.Temperature;
                ef.Hr = encForm.Heartrate;
                ef.Rr = encForm.Respirationrate;
                ef.BpS = encForm.Bloodpressuresystolic;
                ef.BpD = encForm.Bloodpressurediastolic;
                ef.O2 = encForm.Oxygenlevel;
                ef.Pain = encForm.Pain;
                ef.Heent = encForm.Heent;
                ef.Cv = encForm.Cardiovascular;
                ef.Chest = encForm.Chest;
                ef.Abd = encForm.Abdomen;
                ef.Extr = encForm.Extremities;
                ef.Skin = encForm.Skin;
                ef.Neuro = encForm.Neuro;
                ef.Other = encForm.Other;
                ef.Diagnosis = encForm.Diagnosis;
                ef.TreatmentPlan = encForm.Treatmentplan;
                ef.MedicationDispensed = encForm.Medicationsdispensed;
                ef.Procedures = encForm.Procedures;
                ef.FollowUp = encForm.Followup;
            }
            return ef;
        }

        public bool SubmitEncounterForm(EncounterFormModel model)
        {
            try
            {
                //concludeEncounter _obj = new concludeEncounter();

                var ef = _db.Encounterforms.FirstOrDefault(r => r.Requestid == model.reqid);

                if (ef == null)
                {
                    Encounterform _encounter = new Encounterform()
                    {
                        Requestid = model.reqid,
                        Firstname = model.FirstName,
                        Lastname = model.LastName,
                        Location = model.Location,
                        Phonenumber = model.PhoneNumber,
                        Email = model.Email,
                        Illnesshistory = model.HistoryIllness,
                        Medicalhistory = model.MedicalHistory,
                        //date = model.Date,
                        Medications = model.Medications,
                        Allergies = model.Allergies,
                        Temperature = model.Temp,
                        Heartrate = model.Hr,
                        Respirationrate = model.Rr,
                        Bloodpressuresystolic = model.BpS,
                        Bloodpressurediastolic = model.BpD,
                        Oxygenlevel = model.O2,
                        Pain = model.Pain,
                        Heent = model.Heent,
                        Cardiovascular = model.Cv,
                        Chest = model.Chest,
                        Abdomen = model.Abd,
                        Extremities = model.Extr,
                        Skin = model.Skin,
                        Neuro = model.Neuro,
                        Other = model.Other,
                        Diagnosis = model.Diagnosis,
                        Treatmentplan = model.TreatmentPlan,
                        Medicationsdispensed = model.MedicationDispensed,
                        Procedures = model.Procedures,
                        Followup = model.FollowUp,
                        Isfinalized = false
                    };

                    _db.Encounterforms.Add(_encounter);

                    //_obj.indicate = true;
                }
                else
                {
                    var efdetail = _db.Encounterforms.FirstOrDefault(x => x.Requestid == model.reqid);

                    efdetail.Requestid = model.reqid;
                    efdetail.Illnesshistory = model.HistoryIllness;
                    efdetail.Medicalhistory = model.MedicalHistory;
                    //efdetail.Date = model.Date;
                    efdetail.Medications = model.Medications;
                    efdetail.Allergies = model.Allergies;
                    efdetail.Temperature = model.Temp;
                    efdetail.Heartrate = model.Hr;
                    efdetail.Respirationrate = model.Rr;
                    efdetail.Bloodpressuresystolic = model.BpS;
                    efdetail.Bloodpressurediastolic = model.BpD;
                    efdetail.Oxygenlevel = model.O2;
                    efdetail.Pain = model.Pain;
                    efdetail.Heent = model.Heent;
                    efdetail.Cardiovascular = model.Cv;
                    efdetail.Chest = model.Chest;
                    efdetail.Abdomen = model.Abd;
                    efdetail.Extremities = model.Extr;
                    efdetail.Skin = model.Skin;
                    efdetail.Neuro = model.Neuro;
                    efdetail.Other = model.Other;
                    efdetail.Diagnosis = model.Diagnosis;
                    efdetail.Treatmentplan = model.TreatmentPlan;
                    efdetail.Medicationsdispensed = model.MedicationDispensed;
                    efdetail.Procedures = model.Procedures;
                    efdetail.Followup = model.FollowUp;
                    efdetail.Modifieddate = DateTime.Now;
                    ef.Isfinalized = false;
                    _db.Encounterforms.Update(efdetail);
                    // _obj.indicate = true;
                };


                _db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public MyProfileModel MyProfile(string sessionEmail)
        {
            var myProfileMain = _db.Admins.Where(x => x.Email == sessionEmail).Select(x => new MyProfileModel()
            {
                fname = x.Firstname,
                lname = x.Lastname,
                email = x.Email,
                confirm_email = x.Email,
                mobile_no = x.Mobile,
                addr1 = x.Address1,
                addr2 = x.Address2,
                city = x.City,
                zip = x.Zip,
                state = _db.Regions.Where(r => r.Regionid == x.Regionid).Select(r => r.Name).First(),
                roles = _db.Aspnetroles.ToList(),
            }).ToList().FirstOrDefault();

            var aspnetuser = _db.Aspnetusers.Where(r => r.Email == sessionEmail).First();



            myProfileMain.username = aspnetuser.Username;
            //myProfileMain.password = aspnetuser.Passwordhash;

            return myProfileMain;
        }


        public bool ResetPassword(string tokenEmail, string resetPassword)
        {
            try
            {
                var aspUser = _db.Aspnetusers.Where(r => r.Email == tokenEmail).Select(r => r).First();

                if (aspUser.Passwordhash != resetPassword)
                {
                    aspUser.Passwordhash = resetPassword;
                    _db.Aspnetusers.Update(aspUser);

                    _db.SaveChanges();

                    return true;
                }
                return false;



            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool SubmitAdminInfo(MyProfileModel model, string tokenEmail)
        {
            try
            {

                var aspUser = _db.Aspnetusers.Where(r => r.Email == tokenEmail).Select(r => r).First();

                var adminInfo = _db.Admins.Where(r => r.Email == tokenEmail).Select(r => r).First();

                if (adminInfo.Firstname != model.fname || adminInfo.Lastname != model.lname || adminInfo.Email != model.email || adminInfo.Mobile != model.mobile_no || model.email == model.confirm_email)
                {
                    if (adminInfo.Firstname != model.fname)
                    {

                        adminInfo.Firstname = model.fname;

                    }

                    if (adminInfo.Lastname != model.lname)
                    {
                        adminInfo.Lastname = model.lname;
                    }

                    if (adminInfo.Email != model.email)
                    {
                        adminInfo.Email = model.email;
                        aspUser.Email = model.email;

                        int index = model.email.IndexOf('@');
                        var username = model.email.Substring(0, index);
                        aspUser.Username = username;
                    }

                    if (adminInfo.Mobile != model.mobile_no)
                    {
                        adminInfo.Mobile = model.mobile_no;
                        aspUser.Phonenumber = model.mobile_no;
                    }


                    aspUser.Modifieddate = DateTime.Now;

                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SubmitBillingInfo(MyProfileModel model, string tokenEmail)
        {
            try
            {
                var adminInfo = _db.Admins.Where(r => r.Email == tokenEmail).Select(r => r).First();

                var regionid = _db.Regions.Where(x => x.Name.ToLower() == model.state.ToLower()).Select(x => x.Regionid).First();

                if (adminInfo.Address1 != model.addr1 || adminInfo.Address2 != model.addr2 || adminInfo.City != model.city || adminInfo.Zip != model.zip || adminInfo.Regionid != regionid)
                {

                    if (adminInfo.Address1 != model.addr1)
                    {
                        adminInfo.Address1 = model.addr1;
                    }

                    if (adminInfo.Address2 != model.addr2)
                    {
                        adminInfo.Address2 = model.addr2;
                    }

                    if (adminInfo.City != model.city)
                    {
                        adminInfo.City = model.city;
                    }

                    if (adminInfo.Zip != model.zip)
                    {
                        adminInfo.Zip = model.zip;
                    }

                    if (adminInfo.Regionid != regionid)
                    {
                        adminInfo.Regionid = regionid;
                    }

                    _db.SaveChanges();

                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool VerifyState(string state)
        {

            var stateMain = _db.Regions.Where(r => r.Name.ToLower() == state.ToLower().Trim()).FirstOrDefault();

            if (stateMain == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SendRegistrationEmailCreateRequest(string email, string registrationLink)
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
                Subject = "Create Account",
                IsBodyHtml = true,
                Body = $"Click The Following Link To Create Account: <a href='{registrationLink}'>{registrationLink}</a>"
            };



            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }
        public bool CreateRequest(CreateRequestModel model, string sessionEmail, string createAccountLink)
        {
            CreateRequestModel _create = new CreateRequestModel();

            var stateMain = _db.Regions.Where(r => r.Name.ToLower() == model.state.ToLower().Trim()).FirstOrDefault();

            if (stateMain == null)
            {
                return false;
            }
            else
            {
                Request req = new();
                Requestclient reqClient = new();
                User user = new User();
                Aspnetuser asp = new Aspnetuser();
                Requestnote note = new Requestnote();

                var admin = _db.Admins.Where(r => r.Email == sessionEmail).Select(r => r).First();

                var existUser = _db.Aspnetusers.FirstOrDefault(r => r.Email == model.email);

                if (existUser == null)
                {
                    asp.Id = Guid.NewGuid().ToString();
                    asp.Username = model.firstname + "_" + model.lastname;
                    asp.Email = model.email;
                    asp.Phonenumber = model.phone;
                    asp.Createddate = DateTime.Now;
                    _db.Aspnetusers.Add(asp);
                    _db.SaveChanges();

                    user.Aspnetuserid = asp.Id;
                    user.Firstname = model.firstname;
                    user.Lastname = model.lastname;
                    user.Email = model.email;
                    user.Mobile = model.phone;
                    user.City = model.city;
                    user.State = model.state;
                    user.Street = model.street;
                    user.Zipcode = model.zipcode;
                    user.Strmonth = model.dateofbirth.Substring(5, 2);
                    user.Intdate = Convert.ToInt16(model.dateofbirth.Substring(8, 2));
                    user.Intyear = Convert.ToInt16(model.dateofbirth.Substring(0, 4));
                    user.Createdby = admin.Adminid.ToString();
                    user.Createddate = DateTime.Now;
                    user.Regionid = stateMain.Regionid;
                    _db.Users.Add(user);
                    _db.SaveChanges();




                    try
                    {
                        SendRegistrationEmailCreateRequest(model.email, createAccountLink);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                req.Requesttypeid = (int)RequestTypeEnum.Patient;
                req.Userid = _db.Users.Where(x => x.Email == sessionEmail).Select(x => x.Userid).First();
                req.Firstname = admin.Firstname;
                req.Lastname = admin.Lastname;
                req.Phonenumber = admin.Mobile;
                req.Email = admin.Email;
                req.Status = (int)StatusEnum.Unassigned;
                req.Confirmationnumber = admin.Firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
                req.Createddate = DateTime.Now;
                req.Isurgentemailsent = new BitArray(1);
                req.Isurgentemailsent[0] = false;
                _db.Requests.Add(req);

                _db.SaveChanges();



                reqClient.Requestid = req.Requestid;
                reqClient.Firstname = model.firstname;
                reqClient.Lastname = model.lastname;
                reqClient.Phonenumber = model.phone;
                reqClient.Strmonth = model.dateofbirth.Substring(5, 2);
                reqClient.Intdate = Convert.ToInt16(model.dateofbirth.Substring(8, 2));
                reqClient.Intyear = Convert.ToInt16(model.dateofbirth.Substring(0, 4));
                reqClient.Street = model.street;
                reqClient.City = model.city;
                reqClient.State = model.state;
                reqClient.Zipcode = model.zipcode;
                reqClient.Regionid = stateMain.Regionid;
                reqClient.Email = model.email;

                _db.Requestclients.Add(reqClient);
                _db.SaveChanges();

                note.Requestid = req.Requestid;
                note.Adminnotes = model.admin_notes;
                note.Createdby = _db.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r.Id).First();
                note.Createddate = DateTime.Now;
                _db.Requestnotes.Add(note);
                _db.SaveChanges();

                return true;
            }




        }

        public List<ProviderModel> GetProvider()
        {
            BitArray deletebit = new BitArray(new[] { true });

            var provider = from phy in _db.Physicians
                           join role in _db.Roles on phy.Roleid equals role.Roleid
                           join phynoti in _db.Physiciannotifications on phy.Physicianid equals phynoti.Pysicianid
                           orderby phy.Physicianid
                            where phy.Isdeleted != deletebit
                           select new ProviderModel
                           {
                               phyId = phy.Physicianid,
                               firstName = phy.Firstname,
                               lastName = phy.Lastname,
                               status = phy.Status.ToString(),
                               role = role.Name,
                               onCallStatus = "Un Available",
                               notification = phynoti.Isnotificationstopped[0],
                           };
            var result = provider.ToList();

            return result;
        }

        public List<ProviderModel> GetProviderByRegion(int regionId)
        {


            var provider = from phy in _db.Physicians
                           join role in _db.Roles on phy.Roleid equals role.Roleid
                           join phynoti in _db.Physiciannotifications on phy.Physicianid equals phynoti.Pysicianid
                           join phyregion in _db.Physicianregions on phy.Physicianid equals phyregion.Physicianid
                           where phyregion.Regionid == regionId
                           orderby phy.Physicianid
                           select new ProviderModel
                           {
                               phyId = phy.Physicianid,
                               firstName = phy.Firstname,
                               lastName = phy.Lastname,
                               status = phy.Status.ToString(),
                               role = role.Name,
                               onCallStatus = "un available",
                               notification = phynoti.Isnotificationstopped[0],
                           };
            var result = provider.ToList();

            return result;
        }


        public bool StopNotification(int phyId)
        {

            var phyNotification = _db.Physiciannotifications.Where(r => r.Pysicianid == phyId).Select(r => r).First();

            var notification = new BitArray(1);
            notification[0] = false;

            if (phyNotification.Isnotificationstopped[0] == notification[0])
            {
                phyNotification.Isnotificationstopped = new BitArray(1);
                phyNotification.Isnotificationstopped[0] = true;
                _db.Physiciannotifications.Update(phyNotification);
                _db.SaveChanges();

                return true;
            }
            else
            {
                phyNotification.Isnotificationstopped = new BitArray(1);
                phyNotification.Isnotificationstopped[0] = false;
                _db.Physiciannotifications.Update(phyNotification);
                _db.SaveChanges();

                return false;
            }
        }


        public bool ProviderContactEmail(int phyId, string msg)
        {

            var providerEmail = _db.Physicians.Where(x => x.Physicianid == phyId).Select(x => x.Email).First();

            try
            {
                SendAndSaveProviderEmail(providerEmail, msg, phyId);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }
        public void SendAndSaveProviderEmail(string provider, string msg, int phyId)
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
                Subject = "Mail For Provider",
                IsBodyHtml = true,
                Body = $"{msg}",
            };


            mailMessage.To.Add(provider);

            client.Send(mailMessage);

            Emaillog emaillog = new Emaillog();


            emaillog.Subjectname = mailMessage.Subject;
            emaillog.Emailtemplate = "Sender : " + senderEmail + "Reciver :" + provider + "Subject : " + mailMessage.Subject + "Message : " + msg;
            emaillog.Emailid = provider;
            emaillog.Roleid = 1;
            emaillog.Adminid = _db.Admins.Where(r => r.Email == "user@gmail.com").Select(r => r.Adminid).First();
            emaillog.Physicianid = phyId;
            emaillog.Createdate = DateTime.Now;
            emaillog.Sentdate = DateTime.Now;
            emaillog.Isemailsent = new BitArray(1, true);



            _db.Emaillogs.Add(emaillog);
            _db.SaveChanges();
        }

        public CreateProviderAccount CreateProviderAccount(CreateProviderAccount obj, List<int> physicianRegions)
        {
            CreateProviderAccount model = new CreateProviderAccount();

            var aspUser = _db.Aspnetusers.FirstOrDefault(r => r.Email == obj.Email);


            if (aspUser == null && obj.latitude != null)
            {


                Aspnetuser _user = new Aspnetuser();
                Physician phy = new Physician();
                Guid id = Guid.NewGuid();
                _user.Id = id.ToString();
                _user.Username = obj.username;
                _user.Passwordhash = obj.password;
                _user.Email = obj.Email;
                _user.Phonenumber = obj.PhoneNumber;
                _user.Createddate = DateTime.Now;

                _db.Aspnetusers.Add(_user);
                _db.SaveChanges();



                phy.Aspnetuserid = _user.Id;
                phy.Firstname = obj.Firstname;
                phy.Lastname = obj.Lastname;
                phy.Email = obj.Email;
                phy.Mobile = obj.PhoneNumber;
                phy.Medicallicense = obj.MedicalLicesnse;
                phy.Adminnotes = obj.Adminnotes;
                phy.Address1 = obj.Address1;
                phy.Address2 = obj.Address2;
                phy.City = obj.city;
                phy.Regionid = obj.Regionid;
                phy.Zip = obj.zipcode;
                phy.Altphone = obj.altPhone;
                phy.Createdby = _user.Id;
                phy.Createddate = _user.Createddate;
                phy.Status = 1;
                phy.Businessname = obj.Businessname;
                phy.Businesswebsite = obj.BusinessWebsite;
                phy.Roleid = obj.Roleid;
                phy.Syncemailaddress = obj.SycnEmail;

                _db.Physicians.Add(phy);
                _db.SaveChanges();



                foreach (var item in physicianRegions)
                {
                    var region = _db.Regions.FirstOrDefault(x => x.Regionid == item);

                    _db.Physicianregions.Add(new Physicianregion
                    {
                        Physicianid = phy.Physicianid,
                        Regionid = region.Regionid,
                    });
                }
                _db.SaveChanges();

                Physiciannotification notification = new Physiciannotification();
                notification.Pysicianid = phy.Physicianid;
                notification.Isnotificationstopped = new BitArray(1, false);
                _db.Physiciannotifications.Add(notification);
                _db.SaveChanges();

                Aspnetuserrole userRole = new Aspnetuserrole();
                userRole.Userid = _user.Id;
                userRole.Roleid = 3;
                userRole.Roleid = (int)AspNetRole.physician;

                _db.Aspnetuserroles.Add(userRole);
                _db.SaveChanges();


                Physicianlocation _phyLoc = new Physicianlocation();
                _phyLoc.Physicianid = phy.Physicianid;
                _phyLoc.Latitude = obj.latitude;
                _phyLoc.Longitude = obj.longitude;
                _phyLoc.Createddate = DateTime.Now;
                _phyLoc.Physicianname = phy.Firstname;
                _phyLoc.Address = phy.Address1;

                _db.Physicianlocations.Add(_phyLoc);
                _db.SaveChanges();


                AddProviderDocuments(phy.Physicianid, obj.Photo, obj.ContractorAgreement, obj.BackgroundCheck, obj.HIPAA, obj.NonDisclosure);

                model.flag = "done";
                return model;
            }
            else if (aspUser != null)
            {
                model.flag = "email";
            }
            else
            {
                model.flag = "zip";
            }

            return model;
        }

        public void AddProviderDocuments(int Physicianid, IFormFile Photo, IFormFile ContractorAgreement, IFormFile BackgroundCheck, IFormFile HIPAA, IFormFile NonDisclosure)
        {
            var physicianData = _db.Physicians.FirstOrDefault(x => x.Physicianid == Physicianid);

            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianFiles", Physicianid.ToString());
            string photoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianImages", Physicianid.ToString());

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!Directory.Exists(photoDirectory))
            {
                Directory.CreateDirectory(photoDirectory);
            }

            if (Photo != null)
            {
                string path = Path.Combine(photoDirectory, Photo.FileName);
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", Photo.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }

                physicianData.Photo = Photo.FileName;
            }


            if (ContractorAgreement != null)
            {
                string path = Path.Combine(directory, "Independent_Contractor" + Path.GetExtension(ContractorAgreement.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    ContractorAgreement.CopyTo(fileStream);
                }

                physicianData.Isagreementdoc = new BitArray(1, true);
            }

            if (BackgroundCheck != null)
            {
                string path = Path.Combine(directory, "Background" + Path.GetExtension(BackgroundCheck.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    BackgroundCheck.CopyTo(fileStream);
                }

                physicianData.Isbackgrounddoc = new BitArray(1, true);
            }

            if (HIPAA != null)
            {
                string path = Path.Combine(directory, "HIPAA" + Path.GetExtension(HIPAA.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    HIPAA.CopyTo(fileStream);
                }

                physicianData.Istrainingdoc = new BitArray(1, true);
            }

            if (NonDisclosure != null)
            {
                string path = Path.Combine(directory, "Non_Disclosure" + Path.GetExtension(NonDisclosure.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    NonDisclosure.CopyTo(fileStream);
                }

                physicianData.Isnondisclosuredoc = new BitArray(1, true);
            }

            _db.SaveChanges();
        }

        //var phy = _db.Physicians.Where(r => r.Aspnetuserid == aspnetid).Select(r => r).First();

    


        public EditProviderModel EditProviderProfile(int phyId, string tokenEmail)
        {
            var phy = _db.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();

            var user = _db.Aspnetusers.Where(r => r.Email == phy.Email).First();

            EditProviderModel _profile = new EditProviderModel()
            {
                //username = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r.Username).First(),
                Firstname = phy.Firstname,
                Lastname = phy.Lastname,
                Email = phy.Email,
                PhoneNumber = phy.Mobile,
                MedicalLicesnse = phy.Medicallicense,
                NPInumber = phy.Npinumber,
                SycnEmail = phy.Syncemailaddress,
                Address1 = phy.Address1,
                Address2 = phy.Address2,
                city = phy.City,
                zipcode = phy.Zip,
                altPhone = phy.Altphone,
                Businessname = phy.Businessname,
                BusinessWebsite = phy.Businesswebsite,
                Adminnotes = phy.Adminnotes,
                statusId = (int)phy.Status,
                PhyID = phyId,
                Roleid = phy.Roleid,
                Regionid = phy.Regionid,
                PhotoValue = phy.Photo,
                SignatureValue = phy.Signature,
                IsContractorAgreement = phy.Isagreementdoc == null ? false : true,
                IsBackgroundCheck = phy.Isbackgrounddoc == null ? false : true,
                IsHIPAA = phy.Istrainingdoc == null ? false : true,
                IsNonDisclosure = phy.Isnondisclosuredoc == null ? false : true,
                IsLicenseDocument = phy.Islicensedoc == null ? false : true,


                username = user.Username,
                password = user.Passwordhash,
            };

            return _profile;
        }

        public List<Role> GetRoles(int AccType)
        {
            BitArray deletedBit = new BitArray(new[] { false });
            var roles = _db.Roles.Where(x => (x.Accounttype == 2) && (x.Isdeleted.Equals(deletedBit))).ToList();

            return roles;
        }

        public List<Role> GetRoles()
        {
            BitArray deletedBit = new BitArray(new[] { false });
            var roles = _db.Roles.Where(x => x.Isdeleted.Equals(deletedBit)).ToList();
            return roles;
        }
        public List<Region> RegionTable()
        {
            var region = _db.Regions.ToList();
            return region;
        }

        public List<PhysicianRegionTable> PhyRegionTable(int phyId)
        {
            var region = _db.Regions.ToList();
            var phyRegion = _db.Physicianregions.ToList();

            var checkedRegion = region.Select(r1 => new PhysicianRegionTable
            {
                Regionid = r1.Regionid,
                Name = r1.Name,
                ExistsInTable = phyRegion.Any(r2 => r2.Physicianid == phyId && r2.Regionid == r1.Regionid),
            }).ToList();

            return checkedRegion;
        }
        public bool providerResetPass(string email, string password)
        {
            var resetPass = _db.Aspnetusers.Where(r => r.Email == email).Select(r => r).First();

            if (resetPass.Passwordhash != password)
            {
                resetPass.Passwordhash = password;
                _db.SaveChanges();

                return true;
            }
            return false;

        }

        public bool editProviderForm1(int phyId, int roleId, int statusId)
        {
            var user = _db.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();

            if (user.Status != (short)statusId || user.Roleid != roleId)
            {
                user.Status = (short)statusId;
                user.Roleid = roleId;

                _db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool editProviderForm2(string fname, string lname, string email, string phone, string medical, string npi, string sync, int phyId, int[] phyRegionArray)
        {
            var user = _db.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();
            var aspUser = _db.Aspnetusers.Where(r => r.Id == user.Aspnetuserid).Select(r => r).First();

            var abc = _db.Physicianregions.Where(x => x.Physicianid == phyId).Select(r => r.Regionid).ToList();

            var changes = abc.Except(phyRegionArray);

            if (user.Firstname != fname || user.Lastname != lname || user.Email != email || user.Mobile != phone || user.Medicallicense != medical || user.Npinumber != npi || user.Syncemailaddress != sync || changes.Any() == true)
            {
                user.Firstname = fname;
                user.Lastname = lname;
                if (user.Email != email)
                {
                    user.Email = email;
                    aspUser.Email = email;
                }

                user.Mobile = phone;
                user.Medicallicense = medical;
                user.Npinumber = npi;
                user.Syncemailaddress = sync;

                _db.SaveChanges();




                if (changes.Any())
                {
                    if (_db.Physicianregions.Any(x => x.Physicianid == phyId))
                    {
                        var physicianRegion = _db.Physicianregions.Where(x => x.Physicianid == phyId).ToList();

                        _db.Physicianregions.RemoveRange(physicianRegion);
                        _db.SaveChanges();
                    }

                    var phyRegion = _db.Physicianregions.ToList();

                    foreach (var item in phyRegionArray)
                    {
                        var region = _db.Regions.FirstOrDefault(x => x.Regionid == item);

                        _db.Physicianregions.Add(new Physicianregion
                        {
                            Physicianid = phyId,
                            Regionid = region.Regionid,
                        });
                    }
                    _db.SaveChanges();
                }
                return true;
            }



            return false;
        }

        public bool editProviderForm3(EditProviderModel2 dataMain)
        {
            var data = _db.Physicians.Where(r => r.Physicianid == dataMain.editPro.PhyID).Select(r => r).First();
            if (data.Address1 != dataMain.editPro.Address1 || data.Address2 != dataMain.editPro.Address2 || data.City != dataMain.editPro.city || data.Regionid != dataMain.editPro.Regionid || data.Zip != dataMain.editPro.zipcode || data.Altphone != dataMain.editPro.altPhone)
            {
                data.Address1 = dataMain.editPro.Address1;
                data.Address2 = dataMain.editPro.Address2;
                data.City = dataMain.editPro.city;
                data.Regionid = dataMain.editPro.Regionid;
                data.Zip = dataMain.editPro.zipcode;
                data.Altphone = dataMain.editPro.altPhone;

                _db.SaveChanges();

                return true;
            }
            return false;
        }
        public bool PhysicianBusinessInfoUpdate(EditProviderModel2 dataMain)
        {


            var physician = _db.Physicians.FirstOrDefault(x => x.Physicianid == dataMain.editPro.PhyID);

            if (physician != null)
            {
                physician.Businessname = dataMain.editPro.Businessname;
                physician.Businesswebsite = dataMain.editPro.BusinessWebsite;
                physician.Adminnotes = dataMain.editPro.Adminnotes;
                physician.Modifieddate = DateTime.Now;

                _db.SaveChanges();

                if (dataMain.editPro.Photo != null || dataMain.editPro.Signature != null)
                {
                    AddProviderBusinessPhotos(dataMain.editPro.Photo, dataMain.editPro.Signature, dataMain.editPro.PhyID);
                }

            }

            return true;
        }
        public void AddProviderBusinessPhotos(IFormFile photo, IFormFile signature, int phyId)
        {
            var physician = _db.Physicians.FirstOrDefault(x => x.Physicianid == phyId);

            if (photo != null)
            {
                string photoDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianImages", phyId.ToString());
                if (!Directory.Exists(photoDirectory))
                {
                    Directory.CreateDirectory(photoDirectory);
                }

                string path = Path.Combine(photoDirectory, photo.FileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }

                physician.Photo = photo.FileName;
            }

            if (signature != null)
            {
                string signDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianSign", phyId.ToString());
                if (!Directory.Exists(signDirectory))
                {
                    Directory.CreateDirectory(signDirectory);
                }

                string path = Path.Combine(signDirectory, signature.FileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    signature.CopyTo(fileStream);
                }

                physician.Signature = signature.FileName;
            }

            _db.SaveChanges();

        }

        public bool EditOnBoardingData(EditProviderModel2 dataMain)
        {

            var physicianData = _db.Physicians.FirstOrDefault(x => x.Physicianid == dataMain.editPro.PhyID);

            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PhysicianFiles", physicianData.Physicianid.ToString());

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (dataMain.editPro.ContractorAgreement != null)
            {
                string path = Path.Combine(directory, "Independent_Contractor" + Path.GetExtension(dataMain.editPro.ContractorAgreement.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain.editPro.ContractorAgreement.CopyTo(fileStream);
                }

                physicianData.Isagreementdoc = new BitArray(1, true);
            }

            if (dataMain.editPro.BackgroundCheck != null)
            {
                string path = Path.Combine(directory, "Background" + Path.GetExtension(dataMain.editPro.BackgroundCheck.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain.editPro.BackgroundCheck.CopyTo(fileStream);
                }

                physicianData.Isbackgrounddoc = new BitArray(1, true);
            }

            if (dataMain.editPro.HIPAA != null)
            {
                string path = Path.Combine(directory, "HIPAA" + Path.GetExtension(dataMain.editPro.HIPAA.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain.editPro.HIPAA.CopyTo(fileStream);
                }

                physicianData.Istrainingdoc = new BitArray(1, true);
            }

            if (dataMain.editPro.NonDisclosure != null)
            {
                string path = Path.Combine(directory, "Non_Disclosure" + Path.GetExtension(dataMain.editPro.NonDisclosure.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain.editPro.NonDisclosure.CopyTo(fileStream);
                }

                physicianData.Isnondisclosuredoc = new BitArray(1, true);
            }

            if (dataMain.editPro.LicenseDocument != null)
            {
                string path = Path.Combine(directory, "Licence" + Path.GetExtension(dataMain.editPro.LicenseDocument.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain.editPro.LicenseDocument.CopyTo(fileStream);
                }

                physicianData.Islicensedoc = new BitArray(1, true);
            }

            _db.SaveChanges();



            return true;

        }
        public void editProviderDeleteAccount(int phyId)
        {
            var phy = _db.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();
            BitArray deletedBit = new BitArray(1, true);

            phy.Isdeleted = deletedBit;
            _db.Physicians.Update(phy);
            _db.SaveChanges();

        }


        public List<AccountAccess> AccountAccess()
        {
            var obj = (from role in _db.Roles
                       where role.Isdeleted != new BitArray(1, true)
                       select new AccountAccess
                       {
                           Name = role.Name,
                           RoleId = role.Roleid,
                           AccountType = role.Accounttype,
                       }).ToList();
            return obj;
        }

        public bool DeleteRole(int roleId)
        {
            try
            {
                var role = _db.Roles.FirstOrDefault(x => x.Roleid == roleId);
                role.Isdeleted = new BitArray(1, true);
                _db.Roles.Update(role);
                _db.SaveChanges();

                var rolemenu = _db.Rolemenus.Where(x => x.Roleid == roleId).ToList();

                foreach (var item in rolemenu)
                {
                    _db.Rolemenus.Remove(item);
                }
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public List<AccountMenu> GetAccountMenu(int accounttype, int roleid)
        {

            var menu = _db.Menus.Where(r => r.Accounttype == accounttype).ToList();


            var rolemenu = _db.Rolemenus.ToList();

            var checkedMenu = menu.Select(r1 => new AccountMenu
            {
                menuid = r1.Menuid,
                name = r1.Name,
                ExistsInTable = rolemenu.Any(r2 => r2.Roleid == roleid && r2.Menuid == r1.Menuid),

            }).ToList();

            return checkedMenu;

        }

        public AccountAccess GetEditAccessData(int roleid)
        {
            var role = _db.Roles.FirstOrDefault(i => i.Roleid == roleid);
            if (role != null)
            {
                var roledata = new AccountAccess()
                {
                    Name = role.Name,
                    RoleId = roleid,
                    AccountType = role.Accounttype,
                };
                return roledata;
            }
            return null;
        }
        public bool SetEditAccessAccount(AccountAccess accountAccess, List<int> AccountMenu, string sessionEmail)
        {
            try
            {
                var user = _db.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();

                var role = _db.Roles.FirstOrDefault(x => x.Roleid == accountAccess.RoleId);
                if (role != null)
                {
                    role.Name = accountAccess.Name;
                    role.Accounttype = (short)accountAccess.AccountType;
                    role.Modifiedby = user.Id;
                    role.Modifieddate = DateTime.Now;

                    _db.SaveChanges();

                    var rolemenu = _db.Rolemenus.Where(i => i.Roleid == accountAccess.RoleId).ToList();
                    if (rolemenu != null)
                    {
                        _db.Rolemenus.RemoveRange(rolemenu);
                    }

                    if (AccountMenu != null)
                    {
                        foreach (int menuid in AccountMenu)
                        {
                            _db.Rolemenus.Add(new Rolemenu
                            {
                                Roleid = role.Roleid,
                                Menuid = menuid,
                            });
                        }
                        _db.SaveChanges();
                    }
                }
                return true;

            }
            catch
            {
                return false;

            }
        }
        public List<Aspnetrole> GetAccountType()
        {
            var role = _db.Aspnetroles.ToList();
            return role;
        }
        public CreateAccess FetchRole(short selectedValue)
        {
            if (selectedValue == 0)
            {
                CreateAccess obj = new()
                {
                    Menu = _db.Menus.ToList(),
                };
                return obj;
            }
            else if (selectedValue == 1 || selectedValue == 2)
            {

                CreateAccess obj = new()
                {
                    Menu = _db.Menus.Where(x => x.Accounttype == selectedValue).ToList(),
                };
                return obj;
            }
            else
            {
                CreateAccess obj = new();
                return obj;
            }
        }
        public bool RoleExists(string roleName, short accType)
        {
            BitArray deletedBit = new BitArray(new[] { false });

            var isRoleExists = (accType == 0) ? _db.Roles.Where(x => x.Name.ToLower() == roleName.Trim().ToLower() && x.Isdeleted.Equals(deletedBit)).Any() : _db.Roles.Where(x => (x.Name.ToLower() == roleName.Trim().ToLower() && x.Accounttype == accType) && (x.Isdeleted.Equals(deletedBit))).Any();

            return isRoleExists ? true : false;
        }
        public bool CreateRole(List<int> menuIds, string roleName, short accountType)
        {
            try
            {
                if (accountType == 1)
                {
                    Role role = new()
                    {
                        Name = roleName,
                        Accounttype = accountType,
                        Createdby = "Admin",
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(1, false),
                    };
                    _db.Roles.Add(role);
                    _db.SaveChanges();

                    foreach (int menuId in menuIds)
                    {
                        Rolemenu rolemenu = new()
                        {
                            Roleid = role.Roleid,
                            Menuid = menuId,
                        };
                        _db.Rolemenus.Add(rolemenu);
                    };
                    _db.SaveChanges();
                    return true;
                }

                else if (accountType == 2)
                {
                    Role role = new()
                    {
                        Name = roleName,
                        Accounttype = accountType,
                        Createdby = "Physician",
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(1, false),
                    };
                    _db.Roles.Add(role);
                    _db.SaveChanges();

                    foreach (int menuId in menuIds)
                    {
                        Rolemenu rolemenu = new()
                        {
                            Roleid = role.Roleid,
                            Menuid = menuId,
                        };
                        _db.Rolemenus.Add(rolemenu);
                    };
                    _db.SaveChanges();
                    return true;
                }

                else
                {
                    Role role = new()
                    {
                        Name = roleName,
                        Accounttype = 1,
                        Createdby = "Admin",
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(1, false),
                    };
                    _db.Roles.Add(role);
                    _db.SaveChanges();

                    foreach (int menuId in menuIds)
                    {
                        Rolemenu rolemenu = new()
                        {
                            Roleid = role.Roleid,
                            Menuid = menuId,
                        };
                        _db.Rolemenus.Add(rolemenu);
                        _db.SaveChanges();
                    };

                    Role role2 = new()
                    {
                        Name = roleName,
                        Accounttype = 2,
                        Createdby = "Physician",
                        Createddate = DateTime.Now,
                        Isdeleted = new BitArray(1, false),
                    };
                    _db.Roles.Add(role2);
                    _db.SaveChanges();

                    foreach (int menuId in menuIds)
                    {
                        Rolemenu rolemenu2 = new()
                        {
                            Roleid = role2.Roleid,
                            Menuid = menuId,
                        };
                        _db.Rolemenus.Add(rolemenu2);
                        _db.SaveChanges();
                    };
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool CreateAdminAccount(CreateAdminAccount obj, List<int> AdminRegion, string email)
        {
            var emailExists = _db.Aspnetusers.Where(x => x.Email == obj.Email).Any();
            if (emailExists)
            {
                return false;
            }
            else
            {
                Guid id = Guid.NewGuid();
                Aspnetuser aspnetuser = new()
                {
                    Id = id.ToString(),
                    Username = obj.UserName,
                    Passwordhash = obj.AdminPassword,
                    Email = obj.Email,
                    Phonenumber = obj.AdminPhone,
                    Createddate = DateTime.Now,


                };
                _db.Aspnetusers.Add(aspnetuser);
                _db.SaveChanges();


                Aspnetuserrole aspnetuserrole = new Aspnetuserrole();
                aspnetuserrole.Userid = aspnetuser.Id;
                aspnetuserrole.Roleid = (int)AspNetRole.admin;
                _db.Aspnetuserroles.Add(aspnetuserrole);
                _db.SaveChanges();

                var aspnetId = _db.Aspnetusers.Where(x => x.Email == email).Select(x => x.Id).First();
                Admin admin = new Admin();


                admin.Aspnetuserid = aspnetuser.Id;
                admin.Firstname = obj.FirstName;
                admin.Lastname = obj.LastName;
                admin.Email = obj.Email;
                admin.Status = 1;
                admin.Mobile = obj.AdminPhone;
                admin.Address1 = obj.Address1;
                admin.Regionid = obj.regionId;
                admin.Roleid = obj.roleId;
                admin.Address2 = obj.Address2;
                admin.Zip = obj.Zip;
                admin.Altphone = obj.BillingPhone;
                admin.Createdby = aspnetId;
                admin.Createddate = DateTime.Now;
                admin.Isdeleted = new BitArray(1, false);


                _db.Admins.Add(admin);
                _db.SaveChanges();




                var AdminRegions = AdminRegion.ToList();
                for (int i = 0; i < AdminRegions.Count; i++)
                {
                    Adminregion adminregion = new()
                    {
                        Adminid = admin.Adminid,
                        Regionid = _db.Regions.First(x => x.Regionid == AdminRegions[i]).Regionid,
                    };

                    _db.Adminregions.Add(adminregion);
                    _db.SaveChanges();
                }

                return true;

            }


        }








        public List<Physicianlocation> GetPhysicianlocations()
        {
            var phyLocation = _db.Physicianlocations.ToList();
            return phyLocation;
        }

        public List<UserAccess> FetchAccess(short selectedValue)
        {
            if (selectedValue == 1)
            {
                var admin = from admins in _db.Admins
                            join role in _db.Roles on admins.Roleid equals role.Roleid
                            orderby admins.Createddate
                            select new UserAccess
                            {
                                fname = admins.Firstname,
                                lname = admins.Lastname,
                                accType = role.Accounttype,
                                phone = admins.Mobile,
                                status = admins.Status,
                                openReq = _db.Requests.Where(i => (i.Status != 10)).Count(),
                            };
                var result1 = admin.ToList();
                return result1;
            }
            else if (selectedValue == 2)
            {
                var physician = from phy in _db.Physicians
                                join role in _db.Roles on phy.Roleid equals role.Roleid
                                orderby phy.Createddate
                                select new UserAccess
                                {
                                    fname = phy.Firstname,
                                    lname = phy.Lastname,
                                    accType = role.Accounttype,
                                    phone = phy.Mobile,
                                    status = phy.Status,
                                    openReq = _db.Requests.Where(i => i.Physicianid == phy.Physicianid && new int[] { 1, 2, 4, 5, 6 }.Contains(i.Status)).Count()
                                };
                var result2 = physician.ToList();
                return result2;
            }
            else
            {
                var r1 = FetchAccess(1);
                var r2 = FetchAccess(2);
                var r3 = r1.Union(r2).ToList();
                return r3;
            }
        }






        public List<Healthprofessionaltype> GetProfession()
        {
            var obj = _db.Healthprofessionaltypes.ToList();
            return obj;
        }

        public List<BusinessTableModel> BusinessTable(string vendor, string profession)
        {
            BitArray deletedBit = new BitArray(1, false);

            var obj = (from t1 in _db.Healthprofessionals
                       join t2 in _db.Healthprofessionaltypes on t1.Profession equals t2.Healthprofessionalid
                       where t1.Isdeleted == deletedBit
                       select new BusinessTableModel
                       {
                           BusinessId = t1.Vendorid,
                           BusinessName = t1.Vendorname,
                           ProfessionId = t2.Healthprofessionalid,
                           ProfessionName = t2.Professionname,
                           Email = t1.Email,
                           PhoneNumber = t1.Phonenumber,
                           FaxNumber = t1.Faxnumber,
                           BusinessContact = t1.Businesscontact
                       });
            var objList = obj.ToList();
            if (vendor != null)
            {
                objList = objList.Where(x => x.BusinessName.Trim().ToLower().Contains(vendor.Trim().ToLower())).Select(x => x).ToList();
            }
            if (profession != null)
            {
                objList = objList.Where(x => x.ProfessionName.Trim().ToLower().Contains(profession.Trim().ToLower())).Select(x => x).ToList();
            }
            return objList;
        }
      
        public bool AddBusiness(AddBusinessModel obj)
        {
            try
            {
                if (obj.VendorId == null)
                {
                    Healthprofessional healthprofessional = new()
                    {
                        Vendorname = obj.BusinessName,
                        Profession = obj.ProfessionId,
                        Faxnumber = obj.FaxNumber,
                        Address = obj.Street,
                        City = obj.City,
                        State = _db.Regions.Where(x => x.Regionid == obj.RegionId).Select(x => x.Name).First(),
                        Zip = obj.Zip,
                        Regionid = obj.RegionId,
                        Createddate = DateTime.Now,
                        Businesscontact = obj.BusinessContact,
                        Phonenumber = obj.PhoneNumber,
                        Email = obj.Email,
                        Isdeleted = new BitArray(1, false),
                    };
                    _db.Healthprofessionals.Add(healthprofessional);
                    _db.SaveChanges();
                }

                else
                {
                    var vendor = _db.Healthprofessionals.Where(x => x.Vendorid == obj.VendorId).First();
                    vendor.Vendorname = obj.BusinessName;
                    vendor.Profession = obj.ProfessionId;
                    vendor.Email = obj.Email;
                    vendor.Faxnumber = obj.FaxNumber;
                    vendor.Phonenumber = obj.PhoneNumber;
                    vendor.Businesscontact = obj.BusinessContact;
                    vendor.Address = obj.Street;
                    vendor.City = obj.City;
                    vendor.Zip = obj.Zip;
                    vendor.Regionid = obj.RegionId;

                    _db.Healthprofessionals.Update(vendor);
                    _db.SaveChanges();
                }


                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool RemoveBusiness(int VendorId)
        {
            try
            {
                var vendor = _db.Healthprofessionals.FirstOrDefault(x => x.Vendorid == VendorId);
                if (vendor != null && vendor.Isdeleted != null)
                {
                    vendor.Isdeleted[0] = true;
                    _db.Healthprofessionals.Update(vendor);
                    _db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }



        }

        public AddBusinessModel GetEditBusiness(int VendorId)
        {
            var vendor = _db.Healthprofessionals.FirstOrDefault(x => x.Vendorid == VendorId);

            var vendorType = _db.Healthprofessionaltypes.FirstOrDefault(x => x.Healthprofessionalid == vendor.Profession);
            AddBusinessModel obj = new()
            {
                VendorId = VendorId,
                BusinessName = vendor.Vendorname,
                ProfessionId = (int)vendor.Profession,
                Email = vendor.Email,
                PhoneNumber = vendor.Phonenumber,
                FaxNumber = vendor.Faxnumber,
                BusinessContact = vendor.Businesscontact,
                Street = vendor.Address,
                City = vendor.City,
                Zip = vendor.Zip,
                RegionList = RegionTable(),
                ProfessionList = GetProfession(),
                RegionId = (int)vendor.Regionid
            };
            return obj;

        }



        public List<RequestsRecordModel> SearchRecords(RecordsModel recordsModel)
        {


            var requestList = _db.Requests.Where(r => r.Isdeleted == null).Select(x => new RequestsRecordModel()
            {
                requestid = x.Requestid,
                requesttypeid = x.Requesttypeid,
                patientname = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Firstname).First(),
                requestor = x.Firstname,
                email = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Email).First(),
                contact = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Phonenumber).First(),
                address = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Street).First() + " " + x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.City).First() + " " + x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.State).First(),
                zip = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Zipcode).First(),
                statusId = x.Status,
                physician = _db.Physicians.Where(r => r.Physicianid == x.Physicianid).Select(r => r.Firstname).First(),
                physicianNote = x.Requestnotes.Where(r => r.Requestid == x.Requestid).Select(r => r.Physiciannotes).First(),
                AdminNote = x.Requestnotes.Where(r => r.Requestid == x.Requestid).Select(r => r.Adminnotes).First(),
                pateintNote = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Notes).First(),
            }).ToList();

            if (recordsModel != null)
            {
                if (recordsModel.searchRecordOne != null)
                {
                    requestList = requestList.Where(r => r.statusId == recordsModel.searchRecordOne).Select(r => r).ToList();
                }

                if (recordsModel.searchRecordTwo != null)
                {
                    requestList = requestList.Where(r => r.patientname.Trim().ToLower().Contains(recordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                }

                if (recordsModel.searchRecordThree != null)
                {
                    requestList = requestList.Where(r => r.requesttypeid == recordsModel.searchRecordThree).Select(r => r).ToList();
                }

                if (recordsModel.searchRecordSix != null)
                {
                    requestList = requestList.Where(r => r.requestor.Trim().ToLower().Contains(recordsModel.searchRecordSix.Trim().ToLower())).Select(r => r).ToList();
                }

                if (recordsModel.searchRecordSeven != null)
                {
                    requestList = requestList.Where(r => r.email.Trim().ToLower().Contains(recordsModel.searchRecordSeven.Trim().ToLower())).Select(r => r).ToList();
                }

                if (recordsModel.searchRecordEight != null)
                {
                    requestList = requestList.Where(r => r.contact.Trim().ToLower().Contains(recordsModel.searchRecordEight.Trim().ToLower())).Select(r => r).ToList();
                }
            }

            return requestList;
        }




        public List<User> PatientRecords(PatientRecordsModel patientRecordsModel)
        {

            var users = _db.Users.ToList();

            if (patientRecordsModel != null)
            {
                if (patientRecordsModel.searchRecordOne != null)
                {
                    users = users.Where(r => r.Firstname.Trim().ToLower().Contains(patientRecordsModel.searchRecordOne.Trim().ToLower())).Select(r => r).ToList();
                }
                if (patientRecordsModel.searchRecordTwo != null)
                {
                    users = users.Where(r => r.Lastname.Trim().ToLower().Contains(patientRecordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                }
                if (patientRecordsModel.searchRecordThree != null)
                {
                    users = users.Where(r => r.Email.Trim().ToLower().Contains(patientRecordsModel.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                }
                if (patientRecordsModel.searchRecordFour != null)
                {
                    users = users.Where(r => r.Mobile.Trim().ToLower().Contains(patientRecordsModel.searchRecordFour.Trim().ToLower())).Select(r => r).ToList();
                }
            }

            return users;
        }

        public List<GetRecordExplore> GetPatientRecordExplore(int userId)
        {

            var dataMain = _db.Requests.Where(r => r.Userid == userId).Select(r => new GetRecordExplore()
            {
                requestid = r.Requestid,
                requesttypeid = r.Requesttypeid,
                requestclientid = _db.Requestclients.Where(x => x.Requestid == r.Requestid).Select(x => x.Requestclientid).First(),

                createddate = r.Createddate.ToString("yyyy-MM-dd"),
                confirmationnumber = r.Confirmationnumber,
                providername = _db.Physicians.Where(x => x.Physicianid == r.Physicianid).Select(x => x.Firstname).First(),
                status = r.Status,
                fullname = r.Requestclients.Where(x => x.Requestid == r.Requestid).Select(r => r.Firstname).First() + " " + r.Requestclients.Where(x => x.Requestid == r.Requestid).Select(r => r.Lastname).First(),
                concludedate = r.Status == 6 ? Convert.ToDateTime(r.Modifieddate).ToString("yyyy-MM-dd") : null, // Add this condition to check if the status is equal to 6,
            }).ToList();

            //var list = from t0 in _db.Users
            //           join t1 in _db.Requests on t0.Userid equals t1.Userid
            //           join t2 in _db.Requestclients on t1.Requestid equals t2.Requestid
            //           join t3 in _db.Requestwisefiles on t1.Requestid equals t3.Requestid
            //           where t0.Userid == userId
            //           select new GetRecordExplore()
            //           {
            //               fullname = t0.Firstname + " " + t0.Lastname,
            //               createddate= t1.Createddate,
            //               confirmationnumber= t1.Confirmationnumber,
            //               providername= _db.Physicians.FirstOrDefault(x => x.Physicianid == t1.Physicianid).Firstname ?? "-",
            //               requestclientid= t2.Requestclientid,
            //               requestid= t1.Requestid,
            //               //DocumentCount = _context.Requestwisefiles.Where(x => x.Requestid == t2.Requestid).ToList().Count,
            //           };


            return dataMain;
        }

        public void DeleteRecords(int reqId)
        {
            var reqClient = _db.Requests.Where(r => r.Requestid == reqId).Select(r => r).First();

            if (reqClient.Isdeleted == null)
            {
                reqClient.Isdeleted = new BitArray(1, true);
                _db.SaveChanges();
            }
        }

        public byte[] GenerateExcelFile(List<RequestsRecordModel> recordsModel)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Requests");

                // Add headers
                worksheet.Cells[1, 1].Value = "Patient Name";
                worksheet.Cells[1, 2].Value = "Requestor";
                worksheet.Cells[1, 3].Value = "Date Of Service";
                worksheet.Cells[1, 4].Value = "Close Case Date";
                worksheet.Cells[1, 5].Value = "Email";
                worksheet.Cells[1, 6].Value = "Phone Number";
                worksheet.Cells[1, 7].Value = "Address";
                worksheet.Cells[1, 8].Value = "Zip";
                worksheet.Cells[1, 9].Value = "Physician";
                worksheet.Cells[1, 10].Value = "Physician Notes";
                worksheet.Cells[1, 11].Value = "Admin Note";
                worksheet.Cells[1, 12].Value = "Patient Notes";

                // Populate data
                for (int i = 0; i < recordsModel.Count; i++)
                {
                    var rowData = recordsModel[i];
                    worksheet.Cells[i + 2, 1].Value = rowData.patientname;
                    worksheet.Cells[i + 2, 2].Value = rowData.requestor;
                    worksheet.Cells[i + 2, 3].Value = rowData.dateOfService;
                    worksheet.Cells[i + 2, 4].Value = rowData.closeCaseDate;
                    worksheet.Cells[i + 2, 5].Value = rowData.email;
                    worksheet.Cells[i + 2, 6].Value = rowData.contact;
                    worksheet.Cells[i + 2, 7].Value = rowData.address;
                    worksheet.Cells[i + 2, 8].Value = rowData.zip;
                    worksheet.Cells[i + 2, 9].Value = rowData.physician;
                    worksheet.Cells[i + 2, 10].Value = rowData.physicianNote;
                    worksheet.Cells[i + 2, 11].Value = rowData.AdminNote;
                    worksheet.Cells[i + 2, 12].Value = rowData.pateintNote;
                }

                // Convert package to bytes for download
                return excelPackage.GetAsByteArray();
            }
        }




        public EmailSmsRecords2 EmailSmsLogs(int tempId, EmailSmsRecords2 recordsModel)
        {
            EmailSmsRecords2 model = new EmailSmsRecords2();
            model.tempid = tempId;
            model.emailRecords = new List<EmailSmsRecords>();
            if (tempId == 0)
            {
                var records = _db.Emaillogs.ToList();
                foreach (var item in records)
                {
                    if (item.Requestid != null)
                    {

                        var newRecord = new EmailSmsRecords
                        {
                            email = item.Emailid,
                            createddate = item.Createdate,
                            sentdate = item.Sentdate,
                            sent = item.Isemailsent[0] ? "Yes" : "No",
                            recipient = _db.Requestclients.Where(i => i.Requestid == item.Requestid).Select(i => i.Firstname).First(),
                            rolename = _db.Aspnetroles.Where(i => i.Id == item.Roleid).Select(i => i.Name).First(),
                            senttries = item.Senttries,
                            confirmationNumber = item.Confirmationnumber,
                        };

                        model.emailRecords.Add(newRecord);
                    }
                    else
                    {
                        var newRecord = new EmailSmsRecords
                        {
                            email = item.Emailid,
                            createddate = item.Createdate,
                            sentdate = item.Sentdate,
                            sent = item.Isemailsent[0] ? "Yes" : "No",
                            recipient = _db.Physicians.Where(i => i.Physicianid == item.Physicianid).Select(i => i.Firstname).FirstOrDefault(),
                            rolename = _db.Aspnetroles.Where(i => i.Id == item.Roleid).Select(i => i.Name).First(),
                            senttries = item.Senttries,
                            confirmationNumber = item.Confirmationnumber,
                        };

                        model.emailRecords.Add(newRecord);
                    }
                }

                if (recordsModel != null)
                {
                    if (recordsModel.searchRecordOne != null && recordsModel.searchRecordOne != "All")
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.rolename.Contains(recordsModel.searchRecordOne)).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordTwo != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.recipient.Trim().ToLower().Contains(recordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordThree != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.email.Trim().ToLower().Contains(recordsModel.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFour != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate >= recordsModel.searchRecordFour).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFive != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate <= recordsModel.searchRecordFive).Select(r => r).ToList();
                    }
                }
            }

            else
            {
                var records = _db.Smslogs.ToList();
                foreach (var item in records)
                {

                    var newRecord = new EmailSmsRecords
                    {
                        contact = item.Mobilenumber,
                        createddate = item.Createdate,
                        sentdate = item.Sentdate,
                        sent = item.Issmssent[0] ? "Yes" : "No",
                        recipient = _db.Requestclients.Where(i => i.Requestid == item.Requestid).Select(i => i.Firstname).FirstOrDefault(),
                        rolename = _db.Aspnetroles.Where(i => i.Id == item.Roleid).Select(i => i.Name).First(),
                        senttries = item.Senttries,
                        confirmationNumber = item.Confirmationnumber,
                    };

                    model.emailRecords.Add(newRecord);
                }
                if (recordsModel != null)
                {
                    if (recordsModel.searchRecordOne != null && recordsModel.searchRecordOne != "All")
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.rolename.Contains(recordsModel.searchRecordOne)).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordTwo != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.recipient.Trim().ToLower().Contains(recordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordThree != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.contact.Trim().ToLower().Contains(recordsModel.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFour != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate >= recordsModel.searchRecordFour).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFive != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate <= recordsModel.searchRecordFive).Select(r => r).ToList();
                    }
                }

            }
            return model;
        }

        public List<BlockHistory> BlockHistory(BlockHistory2 blockHistory2)
        {
            var requestData = _db.Blockrequests.Where(x => x.Isactive != null).Select(x => new BlockHistory()
            {
                patientname = _db.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Firstname).First(),
                phonenumber = x.Phonenumber,
                email = x.Email,
                createddate = Convert.ToDateTime(x.Createddate).ToString("yyyy-MM-dd"),
                notes = x.Reason,
                blockId = x.Blockrequestid,
                isActive = x.Isactive,
            }).ToList();

            if (blockHistory2 != null)
            {
                if (blockHistory2.searchRecordOne != null)
                {
                    requestData = requestData.Where(r => r.patientname.Trim().ToLower().Contains(blockHistory2.searchRecordOne.Trim().ToLower())).Select(r => r).ToList();
                }
                if (blockHistory2.searchRecordTwo != null && blockHistory2.searchRecordTwo != DateTime.MinValue)
                {
                    requestData = requestData.Where(r => r.createddate == Convert.ToDateTime(blockHistory2.searchRecordTwo).ToString("yyyy-MM-dd")).Select(r => r).ToList();
                }
                if (blockHistory2.searchRecordThree != null)
                {
                    requestData = requestData.Where(r => r.email.Trim().ToLower().Contains(blockHistory2.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                }
                if (blockHistory2.searchRecordFour != null)
                {
                    requestData = requestData.Where(r => r.phonenumber.Trim().ToLower().Contains(blockHistory2.searchRecordFour.Trim().ToLower())).Select(r => r).ToList();
                }
            }


            return requestData;
        }

        public bool UnblockRequest(int blockId)
        {
            try
            {
                var block = _db.Blockrequests.Where(r => r.Blockrequestid == blockId).Select(r => r).First();

                block.Isactive = null;
                _db.Blockrequests.Update(block);
                _db.SaveChanges();

                Requeststatuslog requeststatuslog = new()
                {
                    Requestid = block.Requestid,
                    Status = (int)StatusEnum.Unassigned,
                    Notes = "unblocked",
                    Createddate = DateTime.Now
                };
                _db.Requeststatuslogs.Add(requeststatuslog);

                var request = _db.Requests.Where(r => r.Requestid == block.Requestid).Select(r => r).First();

                request.Status = (int)StatusEnum.Unassigned;
                request.Isdeleted = null;
                request.Modifieddate = DateTime.Now;
                _db.Requests.Update(request);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool IsBlockRequestActive(int blockId)
        {
            var blockrequest = _db.Blockrequests.Where(x => x.Blockrequestid == blockId).First();
            var isActive = new BitArray(1);
            isActive[0] = false;

            if (blockrequest.Isactive != null && blockrequest.Isactive[0] == isActive[0])
            {
                blockrequest.Isactive[0] = true;
                _db.Blockrequests.Update(blockrequest);
                _db.SaveChanges();

                return true;
            }
            else
            {
                if (blockrequest.Isactive != null)
                    blockrequest.Isactive[0] = false;
                _db.Blockrequests.Update(blockrequest);
                _db.SaveChanges();

                return false;
            }
        }


        public DayWiseScheduling GetDayTable(string PartialName, string date, int regionid, int status)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _db.Physicianregions.Include(u => u.Physician).Where(u => u.Regionid == regionid).Select(u => u.Physician).ToList();
            if (regionid == 0)
            {
                physician = _db.Physicians.ToList();
            }

            BitArray deletedBit = new BitArray(new[] { false });

            DayWiseScheduling day = new DayWiseScheduling
            {
                date = currentDate,
                physicians = physician,
            };
            if (regionid != 0 && status != 0)
            {
                day.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Regionid == regionid && m.Status == status).ToList();
            }
            else if (regionid != 0)
            {
                day.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Regionid == regionid).ToList();

            }
            else if (status != 0)
            {
                day.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Status == status).ToList();

            }
            else
            {
                day.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(x => x.Isdeleted.Equals(deletedBit)).ToList();
            }

            return day;
        }


        public WeekWiseScheduling GetWeekTable(string date, int regionid, int status)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _db.Physicianregions.Include(u => u.Physician).Where(u => u.Regionid == regionid).Select(u => u.Physician).ToList();
            if (regionid == 0)
            {
                physician = _db.Physicians.ToList();
            }
            WeekWiseScheduling week = new WeekWiseScheduling
            {
                date = currentDate,
                physicians = physician,

            };
            if (regionid != 0 && status != 0)
            {
                week.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Regionid == regionid && m.Status == status).ToList();
            }
            else if (regionid != 0)
            {
                week.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Regionid == regionid).ToList();

            }
            else if (status != 0)
            {
                week.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Status == status).ToList();

            }
            else
            {
                week.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).ToList();
            }

            return week;
        }

        public MonthWiseScheduling GetMonthTable(string date, int regionid, int status)
        {
            var currentDate = DateTime.Parse(date);
            List<Physician> physician = _db.Physicianregions.Include(u => u.Physician).Where(u => u.Regionid == regionid).Select(u => u.Physician).ToList();
            if (regionid == 0)
            {
                physician = _db.Physicians.ToList();
            }
            MonthWiseScheduling month = new MonthWiseScheduling
            {
                date = currentDate,
                physicians = physician,
            };
            if (regionid != 0 && status != 0)
            {
                month.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Regionid == regionid && m.Status == status).ToList();
            }
            else if (regionid != 0)
            {
                month.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Regionid == regionid).ToList();

            }
            else if (status != 0)
            {
                month.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).Where(m => m.Status == status).ToList();

            }
            else
            {
                month.shiftdetails = _db.Shiftdetails.Include(u => u.Shift).ToList();
            }
            return month;
        }

       

        public void CreateNewShiftSubmit(string selectedDays, CreateShiftModel obj, int adminId)
        {
            var admin = _db.Admins.FirstOrDefault(x => x.Adminid == adminId);

            var day = JsonSerializer.Deserialize<List<CheckBoxData>>(selectedDays);

            var curDate = obj.StartDate;
            var curDay = (int)obj.StartDate.DayOfWeek;

            if (!obj.IsRepeat)
            {
                var shift = new Shift()
                {
                    Physicianid = obj.PhysicianId,
                    Startdate = obj.StartDate,
                    Isrepeat = new BitArray(0, false),
                    Repeatupto = obj.RepeatUpto,
                    Createdby = admin.Aspnetuserid,
                    Createddate = DateTime.Now,
                };
                _db.Shifts.Add(shift);
                _db.SaveChanges();
            }
            else
            {
                var shift = new Shift()
                {
                    Physicianid = obj.PhysicianId,
                    Startdate = obj.StartDate,
                    Isrepeat = new BitArray(1, true),
                    Repeatupto = obj.RepeatUpto,
                    Createdby = admin.Aspnetuserid,
                    Createddate = DateTime.Now,
                };
                _db.Shifts.Add(shift);
                _db.SaveChanges();


                for (int i = 1; i <= obj.RepeatUpto; i++)
                {
                    foreach (var item in day)
                    {
                        if (item.Checked)
                        {
                            var shiftDay = 7 * i - curDay + item.Id;
                            var shiftDate = curDate.AddDays(shiftDay);

                            var shiftdetail = new Shiftdetail()
                            {
                                Shiftid = shift.Shiftid,
                                Shiftdate = shiftDate,
                                Starttime = obj.StartTime,
                                Endtime = obj.EndTime,
                                Status = (short)_db.Physicians.FirstOrDefault(x => x.Physicianid == obj.PhysicianId).Status,

                            };
                            _db.Shiftdetails.Add(shiftdetail);
                            _db.SaveChanges();

                            var shiftRegion = new Shiftdetailregion()
                            {
                                Regionid = obj.RegionId,
                                Shiftdetailid = shiftdetail.Shiftdetailid,
                            };
                            _db.Shiftdetailregions.Add(shiftRegion);
                            _db.SaveChanges();
                        }
                    }
                }
            }

        }




        public async Task CreateShift(SchedulingViewModel model, string Email, List<int> repeatdays)
        {
            Aspnetuser? aspNetUser = _db.Aspnetusers.FirstOrDefault(a => a.Email == Email);


            var chk = repeatdays.ToList();

            var shiftid = _db.Shifts.Where(u => u.Physicianid == model.providerid).Select(u => u.Shiftid).ToList();
            if (shiftid.Count() > 0)
            {
                foreach (var obj in shiftid)
                {
                    var shiftdetailchk = _db.Shiftdetails.Where(u => u.Shiftid == obj && u.Shiftdate == model.shiftdate).ToList();
                    if (shiftdetailchk.Count() > 0)
                    {
                        foreach (var item in shiftdetailchk)
                        {
                            if ((model.starttime >= item.Starttime && model.starttime <= item.Endtime) || (model.endtime >= item.Starttime && model.endtime <= item.Endtime))
                            {
                                //TempData["error"] = "Shift is already assigned in this time";
                                //return RedirectToAction("Scheduling");
                            }
                        }
                    }
                }
            }
            Shift shift = new Shift
            {
                Physicianid = model.providerid,
                Startdate = model.shiftdate,
                Repeatupto = model.repeatcount,
                Createddate = DateTime.Now,
                Createdby = aspNetUser!.Id
            };
            if (chk.Count != 0)
            {
                foreach (var obj in chk)
                {
                    shift.Weekdays += obj;
                }
            }
            if (model.repeatcount > 0)
            {
                shift.Isrepeat = new BitArray(new[] { true });
            }
            else
            {
                shift.Isrepeat = new BitArray(new[] { false });
            }
            _db.Shifts.Add(shift);
            await _db.SaveChangesAsync();
            DateOnly curdate = model.shiftdate;

            Shiftdetail shiftdetail = new Shiftdetail();
            shiftdetail.Shiftid = shift.Shiftid;
            shiftdetail.Shiftdate = curdate;
            shiftdetail.Regionid = model.regionid;
            shiftdetail.Starttime = model.starttime;
            shiftdetail.Endtime = model.endtime;
            shiftdetail.Isdeleted = new BitArray(new[] { false });
            _db.Shiftdetails.Add(shiftdetail);
            await _db.SaveChangesAsync();

            var dayofweek = model.shiftdate.DayOfWeek.ToString();
            int valueforweek;
            if (dayofweek == "Sunday")
            {
                valueforweek = 0;
            }
            else if (dayofweek == "Monday")
            {
                valueforweek = 1;
            }
            else if (dayofweek == "Tuesday")
            {
                valueforweek = 2;
            }
            else if (dayofweek == "Wednesday")
            {
                valueforweek = 3;
            }
            else if (dayofweek == "Thursday")
            {
                valueforweek = 4;
            }
            else if (dayofweek == "Friday")
            {
                valueforweek = 5;
            }
            else
            {
                valueforweek = 6;
            }
            if (shift.Isrepeat[0] == true)
            {
                for (int j = 0; j < shift.Weekdays.Count(); j++)
                {
                    var z = shift.Weekdays;
                    var p = shift.Weekdays.ElementAt(j).ToString();
                    int ele = Int32.Parse(p);
                    int x;
                    if (valueforweek > ele)
                    {
                        x = 6 - valueforweek + 1 + ele;
                    }
                    else
                    {
                        x = ele - valueforweek;
                    }
                    if (x == 0)
                    {
                        x = 7;
                    }
                    DateOnly newcurdate = model.shiftdate.AddDays(x);
                    for (int i = 0; i < model.repeatcount; i++)
                    {
                        Shiftdetail shiftdetailnew = new Shiftdetail
                        {
                            Shiftid = shift.Shiftid,
                            Shiftdate = newcurdate,
                            Regionid = model.regionid,
                            Starttime = model.starttime,
                            Endtime = model.endtime,
                            Isdeleted = new BitArray(new[] { false })
                        };
                        _db.Shiftdetails.Add(shiftdetailnew);
                        await _db.SaveChangesAsync();
                        newcurdate = newcurdate.AddDays(7);
                    }
                }
            }
        }

        public CreateNewShift ViewShift(int ShiftDetailId)
        {
            Shiftdetail? shiftDetails = _db.Shiftdetails.Include(a => a.Shift).Where(a => a.Shiftdetailid == ShiftDetailId).FirstOrDefault();
            Physician? physicians = _db.Physicians.Where(a => a.Physicianid == shiftDetails.Shift.Physicianid).FirstOrDefault();
            Region? region = _db.Regions.Where(a => a.Regionid == physicians!.Regionid).FirstOrDefault();
            CreateNewShift model = new CreateNewShift()
            {
                PhysicianId = physicians.Physicianid,
                PhysicianName = physicians.Firstname + " " + physicians.Lastname,
                RegionId = region.Regionid,
                RegionName = region.Name,
                ShiftDate = shiftDetails.Shiftdate,
                Start = shiftDetails.Starttime,
                End = shiftDetails.Endtime,
            };
            return model;
        }

        public bool EditShift(CreateNewShift model, string email)
        {
            Aspnetuser? aspNetUser = _db.Aspnetusers.FirstOrDefault(a => a.Email == email);
            Shiftdetail? shiftDetails = _db.Shiftdetails.Include(a => a.Shift).Where(a => a.Shiftdetailid == model.shiftdetailid).FirstOrDefault();

            if (shiftDetails != null)
            {
                shiftDetails.Shiftdate = model.ShiftDate;
                shiftDetails.Starttime = model.Start;
                shiftDetails.Endtime = model.End;
                shiftDetails.Modifieddate = DateTime.Now;
                shiftDetails.Modifiedby = aspNetUser.Id;
                _db.Shiftdetails.Update(shiftDetails);
            }
            _db.SaveChanges();
            return true;
        }

        public bool ReturnShift(int ShiftDetailId, string email)
        {
            Aspnetuser? aspNetUser = _db.Aspnetusers.FirstOrDefault(a => a.Email == email);
            Shiftdetail? shiftDetails = _db.Shiftdetails.Include(a => a.Shift).Where(a => a.Shiftdetailid == ShiftDetailId).FirstOrDefault();

            if (shiftDetails != null)
            {
                shiftDetails!.Status = (int)StatusEnum.Unassigned;
                shiftDetails.Modifieddate = DateTime.Now;
                shiftDetails.Modifiedby = aspNetUser.Id;
                _db.Shiftdetails.Update(shiftDetails);
            }
            _db.SaveChanges();
            return true;
        }

        public bool DeleteShift(int ShiftDetailId, string email)
        {
            Aspnetuser? aspNetUser = _db.Aspnetusers.FirstOrDefault(a => a.Email == email);
            Shiftdetail? shiftDetails = _db.Shiftdetails.Include(a => a.Shift).Where(a => a.Shiftdetailid == ShiftDetailId).FirstOrDefault();
            if (shiftDetails != null)

            {
                shiftDetails.Isdeleted = new BitArray(new[] { true });
                shiftDetails.Modifieddate = DateTime.Now;
                shiftDetails.Modifiedby = aspNetUser.Id;
                _db.Shiftdetails.Update(shiftDetails);
            }
            _db.SaveChanges();
            return true;
        }


        public OnCallModal GetOnCallDetails(int regionId)
        {
            var currentTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);
            BitArray deletedBit = new BitArray(new[] { false });

            var onDutyQuery = _db.Shiftdetails
                .Include(sd => sd.Shift.Physician)
                .Where(sd => (regionId == 0 || sd.Shift.Physician.Physicianregions.Any(pr => pr.Regionid == regionId)) &&
                             //sd.Shiftdate.Date == DateTime.Today &&
                             currentTime >= sd.Starttime &&
                             currentTime <= sd.Endtime &&
                             sd.Isdeleted.Equals(deletedBit))
                .Select(sd => sd.Shift.Physician)
                .Distinct()
                .ToList();


            var offDutyQuery = _db.Physicians
                .Include(p => p.Physicianregions)
                .Where(p => (regionId == 0 || p.Physicianregions.Any(pr => pr.Regionid == regionId))
                    && !_db.Shiftdetails.Any(sd =>
                    sd.Shift.Physicianid == p.Physicianid &&
                    //sd.Shiftdate.Date == DateTime.Today &&
                    currentTime >= sd.Starttime &&
                    currentTime <= sd.Endtime &&
                    sd.Isdeleted.Equals(deletedBit)) && p.Isdeleted == null).ToList();
            var onCallModal = new OnCallModal
            {
                OnCall = onDutyQuery,
                OffDuty = offDutyQuery,
                regions = RegionTable(),
            };

            return onCallModal;
        }
        public List<ShiftReview> GetShiftReview(int regionId, int callId)
        {
            BitArray deletedBit = new BitArray(new[] { false });

            var shiftDetail = _db.Shiftdetails.Where(i => i.Isdeleted.Equals(deletedBit) && i.Status != 2);

            DateTime currentDate = DateTime.Now;

            if (regionId != 0)
            {
                shiftDetail = shiftDetail.Where(i => i.Regionid == regionId);
            }

            if (callId == 1)
            {
                shiftDetail = shiftDetail.Where(i => i.Shiftdate.Month == currentDate.Month);
            }

            var reviewList = shiftDetail.Select(x => new ShiftReview
            {
                shiftDetailId = x.Shiftdetailid,
                PhysicianName = _db.Physicians.FirstOrDefault(y => y.Physicianid == _db.Shifts.FirstOrDefault(z => z.Shiftid == x.Shiftid).Physicianid).Firstname + ", " + _db.Physicians.FirstOrDefault(y => y.Physicianid == _db.Shifts.FirstOrDefault(z => z.Shiftid == x.Shiftid).Physicianid).Lastname,
                ShiftDate = x.Shiftdate.ToString("MMM dd, yyyy"),
                ShiftTime = x.Starttime.ToString("hh:mm tt") + " - " + x.Endtime.ToString("hh:mm tt"),
                ShiftRegion = _db.Regions.Where(y => y.Regionid == x.Regionid).Select(x => x.Name).First(),
            }).ToList();
            return reviewList;
        }
        public bool ApproveSelectedShift(int[] shiftDetailsId, string Aspid)
        {
            try
            {

                foreach (var shiftId in shiftDetailsId)
                {
                    var shift = _db.Shiftdetails.FirstOrDefault(i => i.Shiftdetailid == shiftId);
                    shift.Status = 2;
                    shift.Modifieddate = DateTime.Now;
                    shift.Modifiedby = Aspid;
                }
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteShiftReview(int[] shiftDetailsId, string Aspid)
        {
            foreach (var shiftId in shiftDetailsId)
            {
                var shift = _db.Shiftdetails.FirstOrDefault(i => i.Shiftdetailid == shiftId);

                shift.Isdeleted = new BitArray(1, true);
                shift.Modifieddate = DateTime.Now;
                shift.Modifiedby = Aspid;
                _db.Shiftdetails.Update(shift);
            }
            _db.SaveChanges();
            return true;
        }
    }




}

