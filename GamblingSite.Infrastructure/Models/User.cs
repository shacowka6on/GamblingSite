using System.ComponentModel.DataAnnotations;

namespace GamblingSite.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public double Balance { get; set; }   
    }
}
