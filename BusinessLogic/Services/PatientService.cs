using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLogic.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _env;

        public PatientService(ApplicationDbContext db, IHostingEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public Task<bool> IsEmailExists(string email)
        {
            bool isExist = _db.Aspnetusers.Any(x => x.Email == email);
            if (isExist)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public void AddPatientInfo(PatientInfoModel patientInfoModel)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var aspnetuser = _db.Aspnetusers.Where(m => m.Email ==  patientInfoModel.email).FirstOrDefault();
                    User u = new User();
                    if (aspnetuser == null)
                    {
                        Aspnetuser aspnetuser1 = new Aspnetuser();
                        aspnetuser1.Id = Guid.NewGuid().ToString();
                        aspnetuser1.Passwordhash =patientInfoModel.password;
                        aspnetuser1.Email = patientInfoModel.email;
                        String username = patientInfoModel.firstName + patientInfoModel.lastName;
                        aspnetuser1.Username = username;
                        aspnetuser1.Phonenumber = patientInfoModel.phNo;
                        aspnetuser1.Createddate = DateTime.Now;
                        aspnetuser1.Modifieddate = DateTime.Now;
                        _db.Aspnetusers.Add(aspnetuser1);
                        aspnetuser1 = aspnetuser1;

                        u.Aspnetuserid = aspnetuser1.Id;
                        u.Firstname = patientInfoModel.firstName;
                        u.Lastname = patientInfoModel.lastName;
                        u.Email = patientInfoModel.email;
                        u.Mobile = patientInfoModel.phNo;
                        u.Street = patientInfoModel.street;
                        u.City = patientInfoModel.city;
                        u.State = patientInfoModel.state;
                        u.Zipcode = patientInfoModel.zipCode;
                        u.Createdby = patientInfoModel.firstName + patientInfoModel.lastName;
                        u.Intyear = int.Parse(patientInfoModel.dob.ToString("yyyy"));
                        u.Intdate = int.Parse(patientInfoModel.dob.ToString("dd"));
                        u.Strmonth = patientInfoModel.dob.ToString("MMM");
                        u.Createddate = DateTime.Now;
                        u.Modifieddate = DateTime.Now;
                        u.Status = 1;
                        u.Regionid = 1;
                    }
                    else
                    {
                        u = _db.Users.Where(m => m.Email == patientInfoModel.email).FirstOrDefault();
                    }


                    Request request = new Request
                    {
                        Requesttypeid = 2,
                        Status = 1,
                        Createddate = DateTime.Now,
                        Modifieddate = DateTime.Now,
                        Isurgentemailsent = new BitArray(1),
                        Firstname = patientInfoModel.firstName,
                        Lastname = patientInfoModel.lastName,
                        Phonenumber = patientInfoModel.phNo,
                        Email = patientInfoModel.email,
                        Requestid = patientInfoModel.requestId,
                        User = u,
                    };

                    _db.Requests.Add(request);
                    _db.SaveChanges();

                    Requestclient info = new Requestclient();
                    info.Request = request;
                    info.Requestid = request.Requestid;
                    info.Notes = patientInfoModel.symptoms;
                    info.Firstname = patientInfoModel.firstName;
                    info.Lastname = patientInfoModel.lastName;
                    info.Phonenumber = patientInfoModel.phNo;
                    info.Email = patientInfoModel.email;
                    info.Street = patientInfoModel.street;
                    info.City = patientInfoModel.city;
                    info.State = patientInfoModel.state;
                    info.Zipcode = patientInfoModel.zipCode;
                    info.Intyear = int.Parse(patientInfoModel.dob.ToString("yyyy"));
                    info.Intdate = int.Parse(patientInfoModel.dob.ToString("dd"));
                    info.Strmonth = patientInfoModel.dob.ToString("MMM");
                    info.Regionid = 1;

                    _db.Requestclients.Add(info);
                    _db.SaveChanges();

                    if (patientInfoModel.File != null)
                    {
                        uploadFile(patientInfoModel.File, request.Requestid);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                }
            }
        }

        public void uploadFile(List<IFormFile> upload, int id)
        {
            foreach (var item in upload)
            {
                string path = _env.WebRootPath + "/UploadedFiles/" + item.FileName;
                FileStream stream = new FileStream(path, FileMode.Create);

                item.CopyTo(stream)
        ;
                Requestwisefile requestwisefile = new Requestwisefile
                {
                    Requestid = id,
                    Filename = item.FileName,
                    Createddate = DateTime.Now,

                };
                _db.Add(requestwisefile);
                _db.SaveChanges();
            }
        }
        //        Guid id = Guid.NewGuid();

        //            Request request = new Request();
        //            request.Requesttypeid = 2;
        //            request.Status = 1;
        //            request.Createddate = DateTime.Now;
        //            request.Isurgentemailsent = new BitArray(1);
        //            request.Firstname = patientInfoModel.firstName;
        //            request.Lastname = patientInfoModel.lastName;
        //            request.Phonenumber = patientInfoModel.phNo;
        //            request.Email = patientInfoModel.email;

        //            _db.Requests.Add(request);
        //            _db.SaveChanges();

        //            Requestclient info = new Requestclient();
        //            info.Requestid = request.Requestid;
        //            info.Notes = patientInfoModel.symptoms;
        //            info.Firstname = patientInfoModel.firstName;
        //            info.Lastname = patientInfoModel.lastName;
        //            info.Phonenumber = patientInfoModel.phNo;
        //            info.Email = patientInfoModel.email;
        //            info.Street = patientInfoModel.street;
        //            info.City = patientInfoModel.city;
        //            info.State = patientInfoModel.state;
        //            info.Zipcode = patientInfoModel.zipCode;

        //            _db.Requestclients.Add(info)
        //;
        //            _db.SaveChanges();

        //            var user = _db.Aspnetusers.Where(x => x.Email == patientInfoModel.email).FirstOrDefault();

        //            Aspnetuser obj = _db.Aspnetusers.FirstOrDefault(rq => rq.Email == patientInfoModel.email);
        //            if (obj == null)
        //            {
        //                if (patientInfoModel.password== patientInfoModel.confirmPassword)
        //                {
        //                    Aspnetuser aspnetuser = new()
        //                    {
        //                        Id = id.ToString(),
        //                        Username = patientInfoModel.firstName,
        //                        Passwordhash = patientInfoModel.password,
        //                        Createddate = DateTime.Now,
        //                        Email=patientInfoModel.email,
        //                        Phonenumber=patientInfoModel.phNo,

        //                    };
        //                    _db.Aspnetusers.Add(aspnetuser);
        //                    _db.SaveChanges();
        //                    user = aspnetuser;
        //                }

        //            }

        //            User u = new User();
        //            u.Aspnetuserid = user.Id;
        //            u.Firstname = patientInfoModel.firstName;
        //            u.Lastname = patientInfoModel.lastName;
        //            u.Email = patientInfoModel.email;
        //            u.Mobile = patientInfoModel.phNo;
        //            u.Street = patientInfoModel.street;
        //            u.City = patientInfoModel.city;
        //            u.State = patientInfoModel.state;
        //            u.Zipcode = patientInfoModel.zipCode;
        //            u.Createdby = patientInfoModel.firstName;
        //            u.Createddate = DateTime.Now;

        //            _db.Users.Add(u);
        //            _db.SaveChanges();



        //            if (patientInfoModel.File != null)
        //            {


        //                foreach (IFormFile file in patientInfoModel.File)
        //                {
        //                    if (patientInfoModel.File != null && file.Length > 0)
        //                    {
        //                        //get file name
        //                        var fileName = Path.GetFileName(file.FileName);

        //                        //define path
        //                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

        //                        // Copy the file to the desired location
        //                        using (var stream = new FileStream(filePath, FileMode.Create))
        //                        {
        //                            file.CopyTo(stream);
        //                        }

        //                        Requestwisefile requestwisefile = new()
        //                        {

        //                            Filename = fileName,
        //                            Requestid = request.Requestid,
        //                            Createddate = DateTime.Now
        //                        };

        //                        _db.Requestwisefiles.Add(requestwisefile);
        //                        _db.SaveChanges();
        //                    };

        //                }
        //            }


        //        }

        public void AddFamilyReq(FamilyReqModel familyReqModel)
        {
            Request request = new Request();
            request.Requesttypeid = 3;
            request.Status = 1;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(1);
            request.Firstname = familyReqModel.firstName;
            request.Lastname = familyReqModel.lastName;
            request.Phonenumber = familyReqModel.phoneNo;
            request.Email = familyReqModel.email;
            request.Relationname = familyReqModel.relation;

            _db.Requests.Add(request);
            _db.SaveChanges();

            Requestclient info = new Requestclient();
            info.Requestid = request.Requestid;
            info.Notes = familyReqModel.symptoms;
            info.Firstname = familyReqModel.patientFirstName;
            info.Lastname = familyReqModel.patientLastName;
            info.Phonenumber = familyReqModel.patientPhoneNo;
            info.Email = familyReqModel.patientEmail;
            info.Street = familyReqModel.street;
            info.City = familyReqModel.city;
            info.State = familyReqModel.state;
            info.Zipcode = familyReqModel.zipCode;




            if (familyReqModel.File != null && familyReqModel.File.Length > 0)
            {
                //get file name
                var fileName = Path.GetFileName(familyReqModel.File.FileName);

                //define path
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                // Copy the file to the desired location
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    familyReqModel.File.CopyTo(stream);
                }

                Requestwisefile requestwisefile = new()
                {
                    Filename = fileName,
                    Requestid = request.Requestid,
                    Createddate = DateTime.Now
                };





                _db.Requestwisefiles.Add(requestwisefile);
                _db.SaveChanges();
            };


            _db.Requestclients.Add(info);
            _db.SaveChanges();


        }


        public void AddConciergeReq(ConciergeReqModel conciergeReqModel)
        {
            Request request = new Request();
            request.Requesttypeid = 4;
            request.Status = 1;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(1);
            request.Firstname = conciergeReqModel.firstName;
            request.Lastname = conciergeReqModel.lastName;
            request.Phonenumber = conciergeReqModel.phoneNo;
            request.Email = conciergeReqModel.email;
            request.Relationname = "Concierge";

            _db.Requests.Add(request);
            _db.SaveChanges();

            Requestclient info = new Requestclient();
            info.Requestid = request.Requestid;
            info.Notes = conciergeReqModel.symptoms;
            info.Firstname = conciergeReqModel.patientFirstName;
            info.Lastname = conciergeReqModel.patientLastName;
            info.Phonenumber = conciergeReqModel.patientPhoneNo;
            info.Email = conciergeReqModel.patientEmail;



            _db.Requestclients.Add(info);
            _db.SaveChanges();

            Concierge concierge = new Concierge();
            concierge.Conciergename = conciergeReqModel.firstName + " " + conciergeReqModel.lastName;
            concierge.Createddate = DateTime.Now;
            concierge.Regionid = 1;
            concierge.Street = conciergeReqModel.street;
            concierge.City = conciergeReqModel.city;
            concierge.State = conciergeReqModel.state;
            concierge.Zipcode = conciergeReqModel.zipCode;

            _db.Concierges.Add(concierge);
            _db.SaveChanges();

            Requestconcierge reqCon = new Requestconcierge();
            reqCon.Requestid = request.Requestid;
            reqCon.Conciergeid = concierge.Conciergeid;

            _db.Requestconcierges.Add(reqCon);
            _db.SaveChanges();

        }

        public void AddBusinessReq(BusinessReqModel businessReqModel)
        {
            Request request = new Request();
            request.Requesttypeid = 1;
            request.Status = 1;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(1);
            request.Firstname = businessReqModel.firstName;
            request.Lastname = businessReqModel.lastName;
            request.Phonenumber = businessReqModel.phoneNo;
            request.Email = businessReqModel.email;
            request.Relationname = "Business";

            _db.Requests.Add(request);
            _db.SaveChanges();

            Requestclient info = new Requestclient();
            info.Requestid = request.Requestid;
            info.Notes = businessReqModel.symptoms;
            info.Firstname = businessReqModel.patientFirstName;
            info.Lastname = businessReqModel.patientLastName;
            info.Phonenumber = businessReqModel.patientPhoneNo;
            info.Email = businessReqModel.patientEmail;

            _db.Requestclients.Add(info);
            _db.SaveChanges();

            Business business = new Business();
            business.Createddate = DateTime.Now;
            business.Name = businessReqModel.businessName;
            business.Phonenumber = businessReqModel.phoneNo;
            business.City = businessReqModel.city;
            business.Zipcode = businessReqModel.zipCode;

            _db.Businesses.Add(business);
            _db.SaveChanges();

            Requestbusiness requestbusiness = new Requestbusiness();
            requestbusiness.Businessid = business.Businessid;
            requestbusiness.Requestid = request.Requestid;

            _db.Requestbusinesses.Add(requestbusiness);
            _db.SaveChanges();


        }


        public List<MedicalHistory> GetMedicalHistory(User user)
        {
            var medicalhistory = (from request in _db.Requests
                                  join requestfile in _db.Requestwisefiles
                                  on request.Requestid equals requestfile.Requestid
                                  where request.Email == user.Email && request.Email != null
                                  group requestfile by request.Requestid into groupedFiles
                                  select new MedicalHistory
                                  {
                                      FirstName = user.Firstname,
                                      LastName = user.Lastname,
                                      PhoneNo = user.Mobile,
                                      Email = user.Email,
                                      Street = user.Street,
                                      City = user.City,
                                      State = user.State,
                                      ZipCode = user.Zipcode,
                                      reqId = groupedFiles.Select(x => x.Request.Requestid).FirstOrDefault(),
                                      createdDate = groupedFiles.Select(x => x.Request.Createddate).FirstOrDefault(),
                                      currentStatus = groupedFiles.Select(x => x.Request.Status).FirstOrDefault().ToString(),
                                      document = groupedFiles.Select(x => x.Filename.ToString()).ToList()
                                  }).ToList();


            return medicalhistory;
        }

        public IQueryable<Requestwisefile>? GetAllDocById(int requestId)
        {
            var data = from request in _db.Requestwisefiles
                       where request.Requestid == requestId
                       select request;
            return data;
        }


        public void AddFile(IFormFile file, int reqId)
        {
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
            _db.SaveChanges();
        }


    }

}




