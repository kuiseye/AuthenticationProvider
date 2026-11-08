using Microsoft.EntityFrameworkCore.Design;
using AuthenticationProvider.Domain;
using Microsoft.EntityFrameworkCore;
using Bogus;
using AuthenticationProvider.Domain.Entities;
using Microsoft.AspNetCore.Identity;

class SeedingDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{


    const int DEFAULT_SIZE = 5000;
    const int BATCH_SIZE = 5000;
    const int RAND_SEED = 6969;

    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = args.ElementAtOrDefault(0)
            ?? Environment.GetEnvironmentVariable("DB_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=auth_provider_db;Username=user;Password=password";

        var sizeText = args.ElementAtOrDefault(1) ?? Environment.GetEnvironmentVariable("SEED_SIZE");
        var size = int.TryParse(sizeText, out var parsed) && parsed > 0 ? parsed : DEFAULT_SIZE;

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(
                connectionString,
                sqlServer => sqlServer.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .UseSnakeCaseNamingConvention()
            .UseSeeding((context, _) => SeedAsync(context, size, new CancellationToken()).GetAwaiter().GetResult())
            .UseAsyncSeeding((context, _, ct) => SeedAsync(context, size, ct))
            .Options;

        return new ApplicationDbContext(options);
    }

    private static async Task SeedAsync(DbContext context, int size, CancellationToken ct)
    {
        var users = context.Set<AppUser>();
        var existing = await users.CountAsync(u => u.Username.StartsWith("user_"), ct);
        var missing = size - existing;

        Console.WriteLine(existing);

        if (missing < 0)
        {
            await users.Where(u => u.Username.StartsWith("user_"))
                        .OrderBy(u => u.Username.Length)
                        .ThenBy(u => u.Username)
                        .Skip(size)
                        .ExecuteDeleteAsync(ct);
            return;
        }
        if (missing <= 0)
            return;
  
        var hash = new PasswordHasher<AppUser>().HashPassword(null!, "password");
        var anchor = new DateTimeOffset(2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var n = existing;
        
        var faker = new Faker<AppUser>()
            .UseSeed(RAND_SEED + existing)
            .RuleFor(f=>f.Id, f=>f.Random.Guid())
            .RuleFor(u => u.Username, f => $"user_{n++}")
            .RuleFor(u => u.Email, (f, u) => $"{u.Username}@example.com")
            .RuleFor(f=>f.PasswordHash, f=>hash)
            .RuleFor(f=>f.Role, f=>AppUserRole.User)
            .RuleFor(u => u.CreatedAt, f => f.Date.PastOffset(2, anchor).ToUniversalTime())
            .RuleFor(u => u.UpdatedAt, (f, u) => f.Date.BetweenOffset(u.CreatedAt, anchor).ToUniversalTime());

        for(var i = 0; i< missing; i += 5000)
        {
            await context.Set<AppUser>().AddRangeAsync(faker.Generate(Math.Min(BATCH_SIZE, missing - i)), ct);
            await context.SaveChangesAsync(ct);
            context.ChangeTracker.Clear();
        }
    }
}
