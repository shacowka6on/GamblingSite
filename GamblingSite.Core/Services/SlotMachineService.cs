using GamblingSite.Core.Interfaces;
using GamblingSite.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamblingSite.Core.Services
{
    public class SlotMachineService : ISlotMachineService
    {
        private readonly Dictionary<string, int> _symbolWeights = new()
        {
            ["💰"] = 5,  
            ["🍀"] = 10,
            ["7️⃣"] = 20,
            ["🍒"] = 30,
            ["🍋"] = 35
        };
        private readonly Dictionary<string, decimal> _payouts = new()
        {
            ["💰💰💰"] = 10m,  
            ["💰💰"] = 2m,  
            ["💰"] = 0.5m,
            ["🍀🍀🍀"] = 5m,
            ["🍀🍀"] = 1.3m,
            ["7️⃣7️⃣7️⃣"] = 3m,
            ["🍒🍒🍒"] = 2m,
            ["🍒🍒"] = 0.25m,
            ["🍋🍋🍋"] = 1.5m,
        };

        public SlotMachineResult Spin(decimal betAmount)
        {
            if(betAmount <= 0)
            {
                throw new ArgumentException("Bet amount must be positive", nameof(betAmount));
            }

            var rand = new Random();
            string[] slots = new string[3];

            for(int i = 0; i < 3; i++)
            {
                slots[i] = GetWeightedRandomSymbol(_symbolWeights, rand);
            }

            string spinResult = string.Join("", slots);
            decimal winAmount = 0;
            bool isJackpot = false;

            if (_payouts.TryGetValue(spinResult, out decimal value))
            {
                decimal multiplier = value;
                winAmount = multiplier * betAmount;
                if(spinResult == "💰💰💰")
                {
                    isJackpot = true;
                }
            }

            return new SlotMachineResult()
            {
                isJackpot = isJackpot,
                WinAmount = CalculateWin(slots, betAmount),
                Symbols = slots,
            };
        }

        private decimal CalculateWin(string[] slots, decimal betAmount)
        {
            string spinStr = string.Join("", slots);
            decimal win = 0;

            if(_payouts.TryGetValue(spinStr, out decimal multiplier))
            {
                win = betAmount * multiplier;
            }
            else
            {
                foreach(var combo in _payouts.Keys.Where(k => k.Length == 2))
                {
                    if (spinStr.Contains(combo))
                    {
                        win = betAmount * _payouts[combo];
                        break;
                    }
                }
                if(win == 0)
                {
                    foreach(var symbol in slots)
                    {
                        if(_payouts.ContainsKey(symbol))
                        {
                            win += betAmount * multiplier;
                        }
                    }
                }
            }
            return win;
        }

        private string GetWeightedRandomSymbol(Dictionary<string, int> weights, Random rand)
        {
            int totalWeight = weights.Values.Sum();
            int randomNumber = rand.Next(1, totalWeight + 1);
            int increasedWeight = 0;

            foreach(var (symbol, weight) in weights)
            {
                increasedWeight += weight;
                if (randomNumber <= increasedWeight) return symbol;
            }
            return weights.Keys.Last();
        }
    }
}
