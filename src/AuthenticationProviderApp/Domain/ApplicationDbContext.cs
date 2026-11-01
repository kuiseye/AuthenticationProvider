using AuthenticationProvider.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationProvider.Domain;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(
            appUserBuilder=>
            appUserBuilder.Property(appUser=>appUser.Role)
                          .HasConversion<string>());
    }
}