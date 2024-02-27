using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using DataAccess.Data;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;

        public AdminService(ApplicationDbContext db)
        {
            _db = db;
        }

       
        public List<AdminDashTableModel> GetRequestsByStatus()
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
                            reqClientId=rc.Requestclientid,

                        };

            var result = query.ToList();


            return result;
        }
        public ViewCaseViewModel ViewCase(int reqClientId)
        {
            var data = _db.Requestclients.FirstOrDefault(x => x.Requestclientid == reqClientId);
            var cNumber = _db.Requests.FirstOrDefault(x => x.Requestid == data.Requestid);
            var confirm = cNumber.Confirmationnumber;

            var viewdata = new ViewCaseViewModel
            {
                Requestclientid = reqClientId,
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Strmonth = data.Strmonth,
                ConfirmationNumber = confirm,
                Notes = data.Notes,
                Address = data.Address,
                Email = data.Email,
                Phonenumber = data.Phonenumber,
                City = data.City,
                State = data.State,
                Street = data.Street,
                Zipcode = data.Zipcode,
                Regionid = data.Regionid,
            };

            return viewdata;
        }



    }
}