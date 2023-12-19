using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Models;

namespace UserManagementAPI.Models
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {
                
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { ConcurrencyStamp = "1", Name = "Admin", NormalizedName = "Admin" },
                new IdentityRole { ConcurrencyStamp = "2", Name = "User", NormalizedName = "User" },
                new IdentityRole { ConcurrencyStamp = "3", Name = "HR", NormalizedName = "HR" }
                );
        }
    }
}
