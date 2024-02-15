using BusinessLogic.Interfaces;
using DataAccess.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
        public class LoginService : ILoginService
        {

            private readonly DataAccess.Data.ApplicationDbContext _db;

            public LoginService(DataAccess.Data.ApplicationDbContext db)
            {
                _db = db;
            }

     

        public bool Login(LoginModel loginModel)
        {


            return _db.Aspnetusers.Any(x => x.Email == loginModel.Email && x.Passwordhash == loginModel.Password);

        }
       
     
        }
    


}
