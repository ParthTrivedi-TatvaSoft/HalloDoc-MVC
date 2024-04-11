using DataAccess.Enums;
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

        public string Email { get; set; } = null;

        [Required(ErrorMessage = "Password Is Required.")]
        public string Password { get; set; } = null;

    }

    public class CreateAccountModel
    {
        [Required(ErrorMessage = "Email Is Required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Please Enter A Valid Email Address.")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
      //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
      //ErrorMessage = "8 characters long (one uppercase, one lowercase letter, one digit, and one special character.")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [Compare("password", ErrorMessage = "Password Missmatch")]
        public string? confirmPassword { get; set; }
    }

    public class LoginResponseViewModel
    {
        public ResponseStatus Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }



}
