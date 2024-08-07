using _60Names.Models;
using Microsoft.EntityFrameworkCore;

namespace _60Names
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Individual> Individuals { get; set; }
        public DbSet<LegalEntity> LegalEntities { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}