using Microsoft.EntityFrameworkCore;
using Moserware.Skills;

namespace SkillsWebApp.Data
{
    public class SkillContext : DbContext
    {
        public SkillContext(DbContextOptions<SkillContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>()
                .HasOne<Rating>(s => s.Rating)
                .WithOne(ad => ad.Player)
                .HasForeignKey<Rating>(ad => ad.RatingOfPlayer);
        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Rating> Rating { get; set; }

        public DbSet<Moserware.Skills.Player> Player { get; set; }
    }
}
