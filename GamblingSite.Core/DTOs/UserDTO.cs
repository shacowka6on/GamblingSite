using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.DTOs
{
    public class RegisterUserDto
    {
        [Required] 
        public string Name { get; set; }
        [EmailAddress] 
        public string Email { get; set; }
        [MinLength(8)] 
        public string Password { get; set; }
    }
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }  
    }
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
    }
}
