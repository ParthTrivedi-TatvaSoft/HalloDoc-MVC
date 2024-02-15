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
   
        public string? Email { get; set; } = null;

        [Required(ErrorMessage = "Password Is Required.")]
        public string? Password { get; set; } = null;

        }


   
}
