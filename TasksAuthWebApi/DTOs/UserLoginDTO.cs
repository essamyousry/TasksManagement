using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TasksAuthWebApi.DTOs
{
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is requried")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}