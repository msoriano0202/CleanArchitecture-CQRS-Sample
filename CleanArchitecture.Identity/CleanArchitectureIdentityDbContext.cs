//using CleanArchitecture.Identity.Configurations;
using CleanArchitecture.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Identity
{
    public class CleanArchitectureIdentityDbContext : IdentityDbContext<ApplicationUser>
    //public class CleanArchitectureIdentityDbContext : IdentityDbContext // --- refresh token ---
    {
        public CleanArchitectureIdentityDbContext(DbContextOptions<CleanArchitectureIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- Refresh Token ---
            //builder.ApplyConfiguration(new RoleConfiguration());
            //builder.ApplyConfiguration(new UserConfiguration());
            //builder.ApplyConfiguration(new UserRoleConfiguration());
        }

        //public virtual DbSet<RefreshToken>? RefreshToken { get; set; } //--Refresh Token
        //public virtual DbSet<ApplicationUser>? ApplicationUsers { get; set; } //--Refresh Token

    }
}
