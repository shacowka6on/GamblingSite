using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Infrastructure.Models
{
    public class Card
    {
        public string Suit { get; }
        public string Rank { get; }
        public int Value 
        { 
            get 
            {
                if (Rank == "Ace") return 11;
                if (Rank == "King" || Rank == "Queen" || Rank == "Jack") return 10;
                return int.Parse(Rank);
            } 
        }
        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }
}
