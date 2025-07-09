using GamblingSite.Core.Interfaces;
using GamblingSite.Infrastructure.Data;
using GamblingSite.Infrastructure.Models.SlotMachine;

namespace GamblingSite.Core.Services
{
    public class SlotMachineService : ISlotMachineService
    {
        private readonly GamblingSiteDbContext _context;
        public SlotMachineService(GamblingSiteDbContext context)
        {
            _context = context;
        }
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
        public async Task<SlotMachine> Spin(decimal betAmount, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {


                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException("User not found");
                }

                if (user.Balance < betAmount)
                {
                    throw new ArgumentException("User doesn't have enough money");
                }

                if (betAmount <= 0)
                {
                    throw new ArgumentException("Bet amount must be positive", nameof(betAmount));
                }

                user.Balance -= betAmount;
                await _context.SaveChangesAsync();

                var rand = new Random();
                string[] slots = new string[9];

                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i] = GetWeightedRandomSymbol(_symbolWeights, rand);
                }

                decimal result = CalculateWin(slots, betAmount);
                if (result > 0)
                {
                    user.Balance += result;
                    _context.SaveChanges();
                }

                return new SlotMachine()
                {
                    WinAmount = result,
                    Symbols = slots,
                };
            }
            catch 
            {
                await transaction.RollbackAsync(); 
                throw;
            }
        }

        private decimal CalculateWin(string[] slots, decimal betAmount)
        {
            var matrix = new string[3, 3];
            decimal win = 0;
            int row = 0;
            for (int i = 0; i < slots.Length; i += 3)
            {
                for (int col = 0; col < 3; col++)
                {
                    matrix[row, col] = slots[i + col];
                }
                row += 1;
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                //Previous code joined 3 string elements and checked for rewards
                //New version has 3x3 matrix of string elements and instead of checking
                //and returning only a single row it checks all 3 rows for rewards.

                string[] joinRow = new string[3];
                for (int col = 0; col < matrix.GetLength(0); col++)
                {
                    joinRow[col] = matrix[i, col];
                }
                string spinStr = string.Join("", joinRow);

                if (_payouts.TryGetValue(spinStr, out decimal multiplier))
                {
                    win += betAmount * multiplier;
                }
                else
                {
                    foreach (var combo in _payouts.Keys.Where(k => k.Length == 2))
                    {
                        if (spinStr.Contains(combo))
                        {
                            win += betAmount * _payouts[combo];
                            break;
                        }
                    }
                    if (win == 0)
                    {
                        foreach (var symbol in slots)
                        {
                            if (_payouts.ContainsKey(symbol))
                            {
                                win += betAmount * multiplier;
                            }
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

            foreach (var (symbol, weight) in weights)
            {
                increasedWeight += weight;
                if (randomNumber <= increasedWeight) return symbol;
            }
            return weights.Keys.Last();
        }
    }
}
