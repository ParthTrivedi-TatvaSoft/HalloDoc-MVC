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
using System.Net.Mail;
using System.Net;

namespace BusinessLogic.Services
{
    public class PatientService : IPatientService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _env;
        private readonly IJwtService _jwtService;

        public PatientService(ApplicationDbContext db, IHostingEnvironment env, IJwtService jwtService)
        {
            _db = db;
            _env = env;
            _jwtService = jwtService;
        }

        public LoginResponseViewModel patient_login(LoginModel model)
        {
            var user = _db.Aspnetusers.Where(u => u.Email == model.Email).FirstOrDefault();

            if (user == null)
                return new LoginResponseViewModel() { Status = ResponseStatus.Failed, Message = "User Not Found" };
            if (user.Passwordhash == null)
                return new LoginResponseViewModel() { Status = ResponseStatus.Failed, Message = "There Is No Password With This Account" };
            if (user.Passwordhash == model.Password)
            {
                var jwtToken = _jwtService.GetJwtToken(user);

                return new LoginResponseViewModel() { Status = ResponseStatus.Success, Token = jwtToken };
            }

            return new LoginResponseViewModel() { Status = ResponseStatus.Failed, Message = "Password Does Not Match" };
        }


        public bool IsEmailExists(string email)
        {
            bool isExist = _db.Aspnetusers.Any(x => x.Email == email);
            return isExist;
        }

        public bool IsPasswordExists(string email)
        {
            bool isExist = _db.Aspnetusers.Any(x => x.Email == email && x.Passwordhash != null);
            return isExist;
        }

        public bool CreateAccount(CreateAccountModel model)
        {
            try
            {
                Aspnetuser asp = new();
                User user = new();
                var existUser = _db.Aspnetusers.FirstOrDefault(r => r.Email == model.email);

                if (existUser == null)
                {
                    asp.Id = Guid.NewGuid().ToString();
                    asp.Email = model.email;
                    asp.Username = model.email.Split('@')[0];
                    asp.Passwordhash = model.password;
                    asp.Createddate = DateTime.Now;
                    _db.Aspnetusers.Add(asp);

                    user.Aspnetuserid = asp.Id;
                    user.Email = model.email;
                    user.Firstname = asp.Username;
                    user.Createdby = asp.Id;
                    user.Createddate = DateTime.Now;
                    _db.Users.Add(user);
                    _db.SaveChanges();
                }
                else
                {
                    existUser.Passwordhash = model.password;
                    _db.Aspnetusers.Update(existUser);
                    _db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }



        public bool AddPatientInfo(PatientInfoModel patientInfoModel)
        {
            var stateMain = _db.Regions.Where(r => r.Name.ToLower() == patientInfoModel.state.ToLower().Trim()).FirstOrDefault();
            if (stateMain == null)
            {
                return false;
            }

            var aspnetuser = _db.Aspnetusers.Where(m => m.Email == patientInfoModel.email).FirstOrDefault();
            User u = new User();
            if (aspnetuser == null)
            {
                Aspnetuser aspnetuser1 = new Aspnetuser();
                aspnetuser1.Id = Guid.NewGuid().ToString();
                aspnetuser1.Passwordhash = patientInfoModel.password;
                aspnetuser1.Email = patientInfoModel.email;
                aspnetuser1.Username = patientInfoModel.firstName + " " + patientInfoModel.lastName;
                aspnetuser1.Phonenumber = patientInfoModel.phoneNo;
                aspnetuser1.Createddate = DateTime.Now;
                aspnetuser1.Modifieddate = DateTime.Now;
                _db.Aspnetusers.Add(aspnetuser1);

                Aspnetuserrole role = new Aspnetuserrole();
                role.Userid = aspnetuser1.Id;
                role.Roleid = 2;
                _db.Aspnetuserroles.Add(role);

                u.Aspnetuserid = aspnetuser1.Id;
                u.Firstname = patientInfoModel.firstName;
                u.Lastname = patientInfoModel.lastName;
                u.Email = patientInfoModel.email;
                u.Mobile = patientInfoModel.phoneNo;
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
                u.Status = (int)StatusEnum.Unassigned;
                u.Regionid = stateMain.Regionid;

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
            request.Isurgentemailsent = new BitArray(1, false);
            request.Firstname = patientInfoModel.firstName;
            request.Lastname = patientInfoModel.lastName;
            request.Phonenumber = patientInfoModel.phoneNo;
            request.Email = patientInfoModel.email;
            request.Userid = u.Userid;


            _db.Requests.Add(request);
            _db.SaveChanges();

            Requestclient info = new Requestclient();
            info.Request = request;
            info.Requestid = request.Requestid;
            info.Notes = patientInfoModel.symptoms;
            info.Firstname = patientInfoModel.firstName;
            info.Lastname = patientInfoModel.lastName;
            info.Phonenumber = patientInfoModel.phoneNo;
            info.Email = patientInfoModel.email;
            info.Street = patientInfoModel.street;
            info.City = patientInfoModel.city;
            info.State = patientInfoModel.state;
            info.Zipcode = patientInfoModel.zipCode;
            info.Intyear = int.Parse(patientInfoModel.dob.ToString("yyyy"));
            info.Intdate = int.Parse(patientInfoModel.dob.ToString("dd"));
            info.Strmonth = patientInfoModel.dob.ToString("MMM");
            info.Regionid = stateMain.Regionid;

            _db.Requestclients.Add(info)
;
            _db.SaveChanges();



            if (patientInfoModel.file != null)
            {
                foreach (IFormFile file in patientInfoModel.file)
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

            return true;
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

        public bool AddFamilyReq(FamilyReqModel familyReqModel, string createAccountLink)
        {
            var stateMain = _db.Regions.Where(x => x.Name.ToLower() == familyReqModel.state.ToLower().Trim()).FirstOrDefault();

            if (stateMain == null)
            {
                return false;
            }
            User user = new User();
            Aspnetuser asp = new Aspnetuser();
            var existUser = _db.Aspnetusers.FirstOrDefault(r => r.Email == familyReqModel.patientEmail);

            if (existUser == null)
            {
                asp.Id = Guid.NewGuid().ToString();
                asp.Username = familyReqModel.patientFirstName + "_" + familyReqModel.patientLastName;
                asp.Email = familyReqModel.patientEmail;
                asp.Phonenumber = familyReqModel.patientPhoneNo;
                asp.Createddate = DateTime.Now;
                _db.Aspnetusers.Add(asp);
                _db.SaveChanges();

                user.Aspnetuserid = asp.Id;
                user.Firstname = familyReqModel.patientFirstName;
                user.Lastname = familyReqModel.patientFirstName;
                user.Email = familyReqModel.patientEmail;
                user.Mobile = familyReqModel.patientPhoneNo;
                user.City = familyReqModel.city;
                user.State = familyReqModel.state;
                user.Street = familyReqModel.street;
                user.Zipcode = familyReqModel.zipCode;
                user.Intyear = int.Parse(familyReqModel.patientDob.ToString("yyyy"));
                user.Intdate = int.Parse(familyReqModel.patientDob.ToString("dd"));
                user.Strmonth = familyReqModel.patientDob.ToString("MMM");
                user.Createdby = asp.Id;
                user.Createddate = DateTime.Now;
                user.Regionid = stateMain.Regionid;
                _db.Users.Add(user);
                _db.SaveChanges();




                try
                {
                    SendRegistrationEmailCreateRequest(familyReqModel.patientEmail, createAccountLink);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                user = _db.Users.Where(m => m.Email == familyReqModel.patientEmail).FirstOrDefault();
            }
            Request request = new Request();
            request.Requesttypeid = (int)RequestTypeEnum.Family;
            request.Status = (int)StatusEnum.Unassigned;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(1, false);
            request.Firstname = familyReqModel.firstName;
            request.Lastname = familyReqModel.lastName;
            request.Phonenumber = familyReqModel.phoneNo;
            request.Email = familyReqModel.email;
            request.Relationname = familyReqModel.relation;
            request.Userid = user.Userid;

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
            info.Regionid = stateMain.Regionid;

            _db.Requestclients.Add(info);
            _db.SaveChanges();
            return true;
        }

        // aspnet user    user            req reqclient ....
        public bool AddConciergeReq(ConciergeReqModel conciergeReqModel, string createAccountLink)
        {
            var stateMain = _db.Regions.Where(x => x.Name.ToLower() == conciergeReqModel.state.ToLower().Trim()).FirstOrDefault();

            if (stateMain == null)
            {
                return false;
            }
            User user = new User();
            Aspnetuser asp = new Aspnetuser();
            var existUser = _db.Aspnetusers.FirstOrDefault(r => r.Email == conciergeReqModel.patientEmail);

            if (existUser == null)
            {
                asp.Id = Guid.NewGuid().ToString();
                asp.Username = conciergeReqModel.patientFirstName + "_" + conciergeReqModel.patientLastName;
                asp.Email = conciergeReqModel.patientEmail;
                asp.Phonenumber = conciergeReqModel.patientPhoneNo;
                asp.Createddate = DateTime.Now;
                _db.Aspnetusers.Add(asp);
                _db.SaveChanges();

                user.Aspnetuserid = asp.Id;
                user.Firstname = conciergeReqModel.patientFirstName;
                user.Lastname = conciergeReqModel.patientLastName;
                user.Email = conciergeReqModel.patientEmail;
                user.Mobile = conciergeReqModel.patientPhoneNo;
                //user.City = conciergeReqModel.city;
                //user.State = conciergeReqModel.state;
                //user.Street = conciergeReqModel.street;
                //user.Zipcode = conciergeReqModel.zipCode;
                user.Intyear = int.Parse(conciergeReqModel.patientDob.ToString("yyyy"));
                user.Intdate = int.Parse(conciergeReqModel.patientDob.ToString("dd"));
                user.Strmonth = conciergeReqModel.patientDob.ToString("MMM");
                user.Createdby = asp.Id;
                user.Createddate = DateTime.Now;
                user.Regionid = stateMain.Regionid;
                _db.Users.Add(user);
                _db.SaveChanges();




                try
                {
                    SendRegistrationEmailCreateRequest(conciergeReqModel.patientEmail, createAccountLink);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                user = _db.Users.Where(m => m.Email == conciergeReqModel.patientEmail).FirstOrDefault();
            }

            Request request = new Request();
            request.Requesttypeid = (int)RequestTypeEnum.Concierge;
            request.Status = (int)StatusEnum.Unassigned;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(1, false);
            request.Firstname = conciergeReqModel.firstName;
            request.Lastname = conciergeReqModel.lastName;
            request.Phonenumber = conciergeReqModel.phoneNo;
            request.Email = conciergeReqModel.email;
            request.Relationname = "Concierge";
            request.Userid = user.Userid;

            _db.Requests.Add(request);
            _db.SaveChanges();

            Requestclient info = new Requestclient();
            info.Requestid = request.Requestid;
            info.Notes = conciergeReqModel.symptoms;
            info.Firstname = conciergeReqModel.patientFirstName;
            info.Lastname = conciergeReqModel.patientLastName;
            info.Phonenumber = conciergeReqModel.patientPhoneNo;
            info.Email = conciergeReqModel.patientEmail;
            info.Regionid = stateMain.Regionid;


            _db.Requestclients.Add(info);
            _db.SaveChanges();

            Concierge concierge = new Concierge();
            concierge.Conciergename = conciergeReqModel.firstName + " " + conciergeReqModel.lastName;
            concierge.Createddate = DateTime.Now;
            concierge.Regionid = stateMain.Regionid;
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

            return true;
        }
        public bool AddBusinessReq(BusinessReqModel businessReqModel, string createAccountLink)
        {
            var stateMain = _db.Regions.Where(x => x.Name.ToLower() == businessReqModel.state.ToLower().Trim()).FirstOrDefault();

            if (stateMain == null)
            {
                return false;
            }
            User user = new User();
            Aspnetuser asp = new Aspnetuser();
            var existUser = _db.Aspnetusers.FirstOrDefault(r => r.Email == businessReqModel.patientEmail);

            if (existUser == null)
            {
                asp.Id = Guid.NewGuid().ToString();
                asp.Username = businessReqModel.patientFirstName + "_" + businessReqModel.patientLastName;
                asp.Email = businessReqModel.patientEmail;
                asp.Phonenumber = businessReqModel.patientPhoneNo;
                asp.Createddate = DateTime.Now;
                _db.Aspnetusers.Add(asp);
                _db.SaveChanges();

                user.Aspnetuserid = asp.Id;
                user.Firstname = businessReqModel.patientFirstName;
                user.Lastname = businessReqModel.patientLastName;
                user.Email = businessReqModel.patientEmail;
                user.Mobile = businessReqModel.patientPhoneNo;
                user.City = businessReqModel.city;
                user.State = businessReqModel.state;
                user.Street = businessReqModel.street;
                user.Zipcode = businessReqModel.zipCode;
                user.Intyear = int.Parse(businessReqModel.patientDob.ToString("yyyy"));
                user.Intdate = int.Parse(businessReqModel.patientDob.ToString("dd"));
                user.Strmonth = businessReqModel.patientDob.ToString("MMM");
                user.Createdby = asp.Id;
                user.Createddate = DateTime.Now;
                user.Regionid = stateMain.Regionid;
                _db.Users.Add(user);
                _db.SaveChanges();




                try
                {
                    SendRegistrationEmailCreateRequest(businessReqModel.patientEmail, createAccountLink);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                user = _db.Users.Where(m => m.Email == businessReqModel.patientEmail).FirstOrDefault();
            }
            Request request = new Request();
            request.Requesttypeid = (int)RequestTypeEnum.Business;
            request.Status = (int)StatusEnum.Unassigned;
            request.Createddate = DateTime.Now;
            request.Isurgentemailsent = new BitArray(1, false);
            request.Firstname = businessReqModel.firstName;
            request.Lastname = businessReqModel.lastName;
            request.Phonenumber = businessReqModel.phoneNo;
            request.Email = businessReqModel.email;
            request.Relationname = "Business";
            request.Userid = user.Userid;

            _db.Requests.Add(request);
            _db.SaveChanges();

            Requestclient info = new Requestclient();
            info.Requestid = request.Requestid;
            info.Notes = businessReqModel.symptoms;
            info.Firstname = businessReqModel.patientFirstName;
            info.Lastname = businessReqModel.patientLastName;
            info.Phonenumber = businessReqModel.patientPhoneNo;
            info.Email = businessReqModel.patientEmail;
            info.Regionid = stateMain.Regionid;
            info.Street = businessReqModel.street;
            info.City = businessReqModel.city;
            info.State = businessReqModel.state;
            info.Zipcode = businessReqModel.zipCode;
            info.Regionid = stateMain.Regionid;
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
            return true;
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




