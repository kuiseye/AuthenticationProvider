using AuthenticationProvider.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace AuthenticationProvider.Tests.Integration.Domain.Repositories;

[CollectionDefinition("Database")]
public class DatabaseCollectionBase :ICollectionFixture<RepositoryWebFactory>{}

[Collection("Database")]
public abstract class RepositoryTestsBase(RepositoryWebFactory _webFactory) : IAsyncLifetime
{
    protected IServiceScope _scope = _webFactory.Services.CreateScope();
    protected ApplicationDbContext _dbContext => _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    private static Respawner? _respawner = null;


    public async Task InitializeAsync()
    {
        var connection = _dbContext.Database.GetDbConnection();
  
        if(connection.State != System.Data.ConnectionState.Open)
            await connection.OpenAsync();

        _respawner ??= await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            TablesToIgnore = [new Respawn.Graph.Table("ef_migrations_history")],
            DbAdapter = DbAdapter.Postgres
        });
        
        await _respawner.ResetAsync(connection);
    }
    public Task DisposeAsync()
    {
        _scope.Dispose();
        return Task.CompletedTask;
    }
}