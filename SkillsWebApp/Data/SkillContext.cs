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
            modelBuilder.Entity<Player>().HasIndex(p => p.PlayfabId).IsUnique();
            modelBuilder.Entity<Player>().HasOne(s => s.Rating);
        }

        public DbSet<Rating> Rating { get; set; }

        public DbSet<Player> Player { get; set; }
    }
}
