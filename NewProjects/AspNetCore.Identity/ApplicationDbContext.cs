using AspNetCore.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity
{
    public class ApplicationDbContext :IdentityDbContext<User>
    {
        public DbSet<User> Users {  get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);
        }
    }
}