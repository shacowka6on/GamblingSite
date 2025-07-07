using GamblingSite.Core.Interfaces;
using GamblingSite.Infrastructure.Data;
using GamblingSite.Infrastructure.Models;
using GamblingSite.Infrastructure.Models.Blackjack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GamblingSite.Core.Services
{
    public class BlackjackService : IBlackjackService
    {
        private readonly GamblingSiteDbContext _context;
        public BlackjackService(GamblingSiteDbContext context)
        {
            _context = context;
        }
        private static int GetBlackjackHandValue(List<Card> playerHand)
        {
            int totalValue = playerHand.Sum(c => c.Value);
            int aceCount = playerHand.Count(c => c.Rank == "Ace");
            while (totalValue > 21 && aceCount > 0)
            {
                totalValue -= 10;
                aceCount--;
            }
            return totalValue;
        }
        public BlackjackGame Hit(int gameId)
        {
            var game = _context.BlackjackGames.FirstOrDefault(x => x.Id == gameId);
            if (game == null || game.IsFinished)
            {
                throw new ArgumentException("Game is finished or not found");
            }

            var deck = JsonSerializer.Deserialize<List<Card>>(game.DeckJson);
            game.PlayerCards.Add(DrawCard(deck));

            game.DeckJson = JsonSerializer.Serialize(deck);


            var user = _context.Users.Find(game.UserId);

            if (GetBlackjackHandValue(game.PlayerCards) > 21)
            {
                game.IsFinished = true;
                game.Result = "Player busted";
                user.Balance -= game.BetAmount;
            }

            _context.SaveChanges();
            return game;
        }

        public BlackjackGame Stand(int gameId)
        {
            var game = _context.BlackjackGames.FirstOrDefault(x => x.Id == gameId);
            if (game == null || game.IsFinished)
            {
                throw new ArgumentException("Game is finished or not found");
            }

            var deck = JsonSerializer.Deserialize<List<Card>>(game.DeckJson);
            while (GetBlackjackHandValue(game.DealerCards) < 17)
            {
                game.DealerCards.Add(DrawCard(deck));
            }
            
            int playerTotal = GetBlackjackHandValue(game.PlayerCards);
            int dealerTotal = GetBlackjackHandValue(game.DealerCards);
            
            game.IsFinished = true;
            game.Result = dealerTotal > 21 || playerTotal > dealerTotal
                ? "Player wins"
                : "Dealer wins";

            var user = _context.Users.Find(game.UserId);
            if(game.Result == "Player wins")
            {
                user.Balance += game.BetAmount * 2;
            }

            game.DeckJson = JsonSerializer.Serialize(deck);

            _context.SaveChanges();
            return game;
        }

        public BlackjackGame Start(int userId, decimal betAmount)
        {
            var user = _context.Users.Find(userId) ?? throw new ArgumentException("Player doesn't exist");
            if ((user.Balance - betAmount) < 0)
            {
                throw new ArgumentException("Insufficient funds");
            }

            user.Balance -= betAmount;

            var deck = GenerateDeck();
            deck = ShuffleDeck(deck);

            var game = new BlackjackGame
            {
                UserId = userId,
                BetAmount = betAmount,
                IsFinished = false,
            };
            //player draws 2 cards
            game.PlayerCards.Add(DrawCard(deck));
            game.PlayerCards.Add(DrawCard(deck));

            //dealer draws 2 cards
            game.DealerCards.Add(DrawCard(deck));
            game.DealerCards.Add(DrawCard(deck));

            game.PlayerCardsJson = JsonSerializer.Serialize(game.PlayerCards);
            game.DealerCardsJson = JsonSerializer.Serialize(game.DealerCards);

            _context.BlackjackGames.Add(game);
            _context.SaveChanges();

            return game;
        }

        private List<Card> GenerateDeck()
        {
            var suits = new[] { "Hearts", "Diamonds", "Clubs", "Spades" };
            var ranks = new[] { "Ace", "King", "Queen", "Jack", "10", "9", "8", "7", "6", "5", "4", "3", "2" };
            var deck = new List<Card>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card(suit, rank));
                }
            }
            return deck;
        }
        private List<Card> ShuffleDeck(List<Card> deck)
        {
            var random = new Random();
            return deck.OrderBy(x => random.Next()).ToList();
        }
        private static Card DrawCard(List<Card> deck)
        {
            if (deck.Count == 0)
            {
                throw new InvalidOperationException("Deck is empty");
            }

            var card = deck[0];
            deck.RemoveAt(0);
            return card;
        }
    }
}
