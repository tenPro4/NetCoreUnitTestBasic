using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using XTesting.Services.Models;

namespace Blog.Turnmeup.API.Models.Users
{
    public class UpdateRequestModel : UserModel
    {
        [Required]
        public string Password { get; set; }
    }
}
