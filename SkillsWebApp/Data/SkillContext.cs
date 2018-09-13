using Microsoft.EntityFrameworkCore;
using Moserware.Skills;

namespace SkillsWebApp.Data
{
    public class SkillContext : DbContext
    {
        public SkillContext(DbContextOptions<SkillContext> options) : base(options)
        {

        }
        
        public DbSet<PlayerInt> Players { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<RatingDB> Rating { get; set; }
    }
}
