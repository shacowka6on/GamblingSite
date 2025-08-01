using GamblingSite.Core.Interfaces;
using GamblingSite.Infrastructure.Data;
using GamblingSite.Infrastructure.Models.Roulette;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Services
{
    public class RouletteService : IRouletteService
    {
        private readonly GamblingSiteDbContext _context;
        public RouletteService(GamblingSiteDbContext context)
        {
            _context = context;
        }
        public async Task<Roulette> Spin(decimal betAmount, int playerNumber)
        {
            //var user = _context.Users.Find();
            //Figure out how to implement colors, even/odd, etc.
            try
            {

                Random rand = new Random();
                int winningNumber = rand.Next(1, 37);
                if (winningNumber == playerNumber)
                {
                    //win
                    //payout
                }
                else
                {
                    //loss
                }
                return new Roulette()
                {
                    CurrentNumber = playerNumber,
                    WinningNumber = winningNumber,
                    WinAmount = betAmount
                };
            }
            catch (Exception ex) 
            {
                throw new Exception("Exception");
            }
        }
    }
}
