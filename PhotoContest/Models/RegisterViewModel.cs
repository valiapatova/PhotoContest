using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoContest.Models
{
    public class RegisterViewModel:LoginViewModel
    {
        [Required]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "{0} must be between {2} and {1}")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "{0} must be between {2} and {1}")]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }   

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
