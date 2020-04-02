using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Twitter.Models
{
    public class LoginViewModel
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string password { get; set; }
    }
}
