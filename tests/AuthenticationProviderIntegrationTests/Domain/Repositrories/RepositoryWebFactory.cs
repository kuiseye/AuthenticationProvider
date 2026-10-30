using AuthenticationProvider.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace AuthenticationProvider.Tests.Integration.Domain.Repositories;

public class RepositoryWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private PostgreSqlContainer postgresContainer;

    public RepositoryWebFactory()
    {
        postgresContainer = new PostgreSqlBuilder("postgres:17-alpine")
        .WithUsername("authentication_admin")
        .WithPassword("authentication_admin")
        .WithDatabase("authentication_provider")
        .Build();
    }


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("TokenKeyPairSettings:SigningKey", "test-signing-key");
        builder.UseSetting("TokenKeyPairSettings:VerificationKey", "test-verification-key");
        builder.UseContentRoot(AppContext.BaseDirectory);
        builder.ConfigureTestServices(services =>
        {
           var descriptor = services.SingleOrDefault(s => s.ServiceType==typeof(DbContextOptions<ApplicationDbContext>));
           if(descriptor is not null) services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                
                options.UseNpgsql(postgresContainer.GetConnectionString(), op =>
                {
                    op.MigrationsHistoryTable("ef_migrations_history");
                    op.MigrationsAssembly(typeof(Program).Assembly);
                }).UseSnakeCaseNamingConvention();
            });
        });
    }

    public async Task InitializeAsync()
    {
        await postgresContainer.StartAsync();
        
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await postgresContainer.DisposeAsync();
    }
}