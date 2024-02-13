using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CustomModels
{
   
        public class LoginModel
        {
        [Required(ErrorMessage = "Email Is Required.")]
        [RegularExpression("^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\\-+)|([A-Za-z0-9]+\\.+)|([A-Za-z0-9]+\\++))*[A-Za-z0-9]+@((\\w+\\-+)|(\\w+\\.))*" +
          "\\w{1,63}\\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password Is Required.")]
        [RegularExpression("(?=^.{8,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{\":;'?/>.<,])(?!.*\\s).*$", 
        ErrorMessage = "Password must be 8-10 characters long with at least one numeric,one upper case character and one special character.")]
        public string? Password { get; set; }

        }
   
}
