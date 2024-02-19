using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace BusinessLogic.Repository
{
    public class LoginService : ILoginService
    {

        private readonly DataAccess.Data.ApplicationDbContext _db;

        public LoginService(DataAccess.Data.ApplicationDbContext db)
        {
            _db = db;
        }



        public User Login(LoginModel loginModel)
        {

            //bool isExist = _db.Aspnetusers.Any(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password);
            //if (isExist)
            //{
            //    var user = _db.Users.FirstOrDefault(x => x.Aspnetuserid ==  )
            //}
            //return user;
            var obj = _db.Aspnetusers.ToList();
            User user = new User();

            //string hashPassword = GenerateSHA256(patient.Password);
            //match the email and pw with database entry
            foreach (var item in obj)
            {
                if (item.Email == loginModel.Email && item.Passwordhash == loginModel.Password)
                {
                    user = _db.Users.FirstOrDefault(u => u.Aspnetuserid == item.Id);
                    return user;
                }
            }
            return user;
        }


    }



}
