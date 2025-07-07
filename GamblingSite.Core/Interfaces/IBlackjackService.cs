using GamblingSite.Infrastructure.Models;
using GamblingSite.Infrastructure.Models.Blackjack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Interfaces
{
    public interface IBlackjackService
    {
        BlackjackGame Start(int userId, decimal betAmount);
        BlackjackGame Hit(int gameId);
        BlackjackGame Stand(int gameId);
    }
}
