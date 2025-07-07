using GamblingSite.Infrastructure.Models;
using GamblingSite.Infrastructure.Models.Blackjack;
using Microsoft.EntityFrameworkCore;

namespace GamblingSite.Infrastructure.Data
{
    public class GamblingSiteDbContext : DbContext
    {
        public GamblingSiteDbContext(DbContextOptions<GamblingSiteDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<BlackjackGame> BlackjackGames { get; set; }
    }
}
