using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Infrastructure.Models.Blackjack
{
    public class BlackjackGame
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User Player { get; set; }

        public string DeckJson { get; set; }

        public string PlayerCardsJson { get; set; }
        public string DealerCardsJson { get; set; }

        [NotMapped]
        public List<Card> PlayerCards { get; set; } = new();
        [NotMapped]
        public List<Card> DealerCards { get; set; } = new();

        public decimal BetAmount { get; set; }

        public bool IsFinished { get; set; }
        public string Result { get; set; } // win or lose
    }
}
