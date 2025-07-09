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
        Task<BlackjackGame> Start(int userId, decimal betAmount);
        Task<BlackjackGame> Hit(int gameId);
        Task<BlackjackGame> Stand(int gameId);
    }
}
