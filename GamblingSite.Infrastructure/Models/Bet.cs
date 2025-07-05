using System.ComponentModel.DataAnnotations;

namespace GamblingSite.Models
{
    public class Bet
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Amount { get; set; }
        public bool IsWin { get; set; }
        public DateTime PlacedAt { get; set; } = DateTime.UtcNow;
    }
}
