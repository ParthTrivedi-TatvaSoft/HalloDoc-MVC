using DataAccess.CustomModels;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace BusinessLogic.Interfaces
{
    public interface ILoginService
    {
       
        public User Login(LoginModel loginModel);

        

    }


}


