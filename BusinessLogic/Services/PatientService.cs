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
using DataAccess.Enums;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Globalization;

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


            var aspnetuser = _db.Aspnetusers.Where(m => m.Email == patientInfoModel.email).FirstOrDefault();
            User u = new User();
            if (aspnetuser == null)
            {
                Aspnetuser aspnetuser1 = new Aspnetuser();
                aspnetuser1.Id = Guid.NewGuid().ToString();
                aspnetuser1.Passwordhash = patientInfoModel.password;
                aspnetuser1.Email = patientInfoModel.email;
                string username = patientInfoModel.firstName + patientInfoModel.lastName;
                aspnetuser1.Username = username;
                aspnetuser1.Phonenumber = patientInfoModel.phNo;
                aspnetuser1.Createddate = DateTime.Now;
                aspnetuser1.Modifieddate = DateTime.Now;
                _db.Aspnetusers.Add(aspnetuser1);



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
                u.Intyear = patientInfoModel.dob.Year;
                u.Intdate = patientInfoModel.dob.Day;
                u.Strmonth = patientInfoModel.dob.ToString("MMM");
                u.Createddate = DateTime.Now;
                u.Modifieddate = DateTime.Now;
                u.Status = (int)StatusEnum.Unassigned;
                u.Regionid = 1;

                _db.Users.Add(u);
                _db.SaveChanges();
            }
            else
            {
                u = _db.Users.Where(m => m.Email == patientInfoModel.email).FirstOrDefault();
            }


            Request request = new Request();
            request.Requesttypeid = (int)RequestTypeEnum.Patient;
            request.Status = (int)StatusEnum.Unassigned;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(1);
            request.Firstname = patientInfoModel.firstName;
            request.Lastname = patientInfoModel.lastName;
            request.Phonenumber = patientInfoModel.phNo;
            request.Email = patientInfoModel.email;



            _db.Requests.Add(request);
            _db.SaveChanges();

            var regionData = _db.Regions.Where(x => x.Regionid == u.Regionid).FirstOrDefault();


            var count = (from req in _db.Requests where req.Userid == u.Userid && req.Createddate.Date == DateTime.Now.Date select req).Count();
            request.Confirmationnumber = $"{regionData.Abbreviation.Substring(0, 2)}{request.Createddate.Day.ToString().PadLeft(2, '0')}{request.Createddate.Month.ToString().PadLeft(2, '0')}{(request.Createddate.Year % 100).ToString().PadLeft(2, '0')}{u.Lastname.ToUpper().Substring(0, 2)}{u.Firstname.ToUpper().Substring(0, 2)}{count.ToString().PadLeft(4, '0')}";

            _db.Requests.Update(request);
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
            info.Intyear = patientInfoModel.dob.Year;
            info.Intdate = patientInfoModel.dob.Day;
            info.Strmonth = patientInfoModel.dob.ToString("MMM");
     
            info.Regionid = 1;

            _db.Requestclients.Add(info)
;
            _db.SaveChanges();



            if (patientInfoModel.File != null)
            {
                foreach (IFormFile file in patientInfoModel.File)
                {
                    if (file != null && file.Length > 0)
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
                            Requestid = request.Requestid,
                            Createddate = DateTime.Now
                        };

                        _db.Requestwisefiles.Add(requestwisefile);
                        _db.SaveChanges();
                    };
                }
            }
        }



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


        public MedicalHistoryList GetMedicalHistory(int userid)
        {
            var user = _db.Users.FirstOrDefault(x => x.Userid == userid);


            var medicalhistory = (from request in _db.Requests
                                  join requestfile in _db.Requestwisefiles
                                  on request.Requestid equals requestfile.Requestid
                                  where request.Email == user.Email && request.Email != null
                                  group requestfile by request.Requestid into groupedFiles
                                  select new MedicalHistory
                                  {

                                      reqId = groupedFiles.Select(x => x.Request.Requestid).FirstOrDefault(),
                                      createdDate = groupedFiles.Select(x => x.Request.Createddate).FirstOrDefault(),
                                      currentStatus = groupedFiles.Select(x => x.Request.Status).FirstOrDefault(),
                                      document = groupedFiles.Select(x => x.Filename.ToString()).ToList(),
                                      ConfirmationNumber = groupedFiles.Select(x => x.Request.Confirmationnumber).FirstOrDefault(),
                                  }).ToList();

            MedicalHistoryList medicalHistoryList = new()
            {
                medicalHistoriesList = medicalhistory,
                id = userid,
                firstName = user.Firstname,
                lastName = user.Lastname
            };

            return medicalHistoryList;
        }

        public DocumentModel GetAllDocById(int requestId)
        {
            var list = _db.Requestwisefiles.Where(x => x.Requestid == requestId).ToList();
            var reqClient = _db.Requestclients.Where(x => x.Requestid == requestId).FirstOrDefault();
            var req = _db.Requests.Where(x => x.Requestid == requestId).FirstOrDefault();
            DocumentModel result = new()
            {
                files = list,
                firstName = reqClient.Firstname,
                lastName = reqClient.Lastname,
                ConfirmationNumber = req.Confirmationnumber,
            };
            return result;
        }


     //   public void AddFile(IFormFile file, int reqId)
     //   {
     //       var fileName = Path.GetFileName(file.FileName);

     //       //define path
     //       var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

     //       // Copy the file to the desired location
     //       using (var stream = new FileStream(filePath, FileMode.Create))
     //       {
     //           file.CopyTo(stream)
     //;
     //       }
     //       Requestwisefile requestwisefile = new()
     //       {
     //           Filename = fileName,
     //           Requestid = reqId,
     //           Createddate = DateTime.Now
     //       };
     //       _db.Requestwisefiles.Add(requestwisefile);
     //       _db.SaveChanges();
     //   }

        public bool UploadDocuments(List<IFormFile> files, int reqId)
        {
            try
            {
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            //get file name
                            var fileName = Path.GetFileName(file.FileName);

                            //define path
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", fileName);

                            // Copy the file to the desired location
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            Requestwisefile requestwisefile = new()
                            {
                                Filename = fileName,
                                Requestid = reqId,
                                Createddate = DateTime.Now
                            };
                            _db.Requestwisefiles.Add(requestwisefile);
                        }
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



        public Profile GetProfile(int userid)
        {

            var user = _db.Users.FirstOrDefault(x => x.Userid == userid);
            Profile profile = new()
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Email = user.Email,
                PhoneNo = user.Mobile,
                State = user.State,
                City = user.City,
                //isMobileCheck = user.Ismobile[0] ? 1 : 0,
                Street = user.Street,
                ZipCode = user.Zipcode,
                //DateOfBirth = new DateTime(Convert.ToInt32(user.Intyear), DateTime.ParseExact(user.Strmonth, "MMM", CultureInfo.InvariantCulture).Month, Convert.ToInt32(user.Intdate)),


            };
            return profile;
        }

        public bool EditProfile(Profile profile)
        {

            try
            {
                var existingUser = _db.Users.Where(x => x.Userid == profile.userId).FirstOrDefault();
                if (existingUser != null)
                {

                    existingUser.Firstname = profile.FirstName;
                    existingUser.Lastname = profile.LastName;
                    existingUser.Mobile = profile.PhoneNo;
                    existingUser.Street = profile.Street;
                    existingUser.City = profile.City;
                    existingUser.State = profile.State;
                    existingUser.Zipcode = profile.ZipCode;
                    existingUser.Intdate = profile.DateOfBirth.Day;
                    existingUser.Strmonth = profile.DateOfBirth.ToString("MMM");
                    existingUser.Intyear = profile.DateOfBirth.Year;
                    //existingUser.Ismobile[0] = profile.isMobileCheck == 1 ? true : false;
                    _db.Users.Update(existingUser);
                    _db.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex) { return false; }
        }


    }

}




