using System.Text;
using AuthenticationProvider.Configuration;
using AuthenticationProvider.Configuration.ConfigurationOptions;
using AuthenticationProvider.Configuration.Settings;
using AuthenticationProvider.Domain;
using AuthenticationProvider.Domain.Dtos.ClaimsDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationProvider;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(op =>
        {
            op.UseNpgsql(configuration.GetConnectionString("Default"), op =>
            {
                op.MigrationsHistoryTable("ef_migrations_history");
            }).UseSnakeCaseNamingConvention();
        });

        return services;
    }

    public static IServiceCollection AddJwtCookieAuthentication(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var tokenKeyPairOptions = sp.GetRequiredService<IOptions<TokenKeyPairSettings>>();

            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyPairOptions.Value.SigningKey)),SecurityAlgorithms.HmacSha256);
        });
        
        services.ConfigureOptions<CustomJwtBearerOptions>();
        services.AddAuthentication(ApplicationSchemes.JwtCookieSchema)
        .AddJwtBearer(ApplicationSchemes.JwtCookieSchema);

        return services;
    }

    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TokenSettings>()
        .Bind(configuration.GetSection(nameof(TokenSettings)))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        services.AddOptions<TokenKeyPairSettings>()
        .Bind(configuration.GetSection(nameof(TokenKeyPairSettings)))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        services.AddOptions<CookieSettings>()
        .Bind(configuration.GetSection(nameof(CookieSettings)))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        services.AddOptions<ServerSettings>()
        .Bind(configuration.GetSection(nameof(ServerSettings)))
        .ValidateDataAnnotations()
        .ValidateOnStart();

        return services;
    }

    public static IServiceCollection AddAppUserClaimsDto(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped(sp => 
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var claimsPrincipal = httpContextAccessor.HttpContext?.User
                ?? throw new InvalidOperationException("No user found in HttpContext (is this being resolved outside an HTTP request?)");
            
            return new AppUserClaimsDto(claimsPrincipal);
        });
        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    //Application
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            try{
                var DbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await DbContext.Database.MigrateAsync();
            }
            catch(Exception e)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                Log.MigrationsFailed(logger, e);

                throw;
            }
        }
    }

    public static void MapSwaggerEndpoints(this WebApplication app)
    {
        var serverOptions = app.Services.GetRequiredService<IOptions<ServerSettings>>();
        var serverSettings = serverOptions.Value;

        var prefix = serverSettings.UrlPrefix + serverSettings.AppVersion + '/';

        app.MapSwagger(prefix + "swagger/{documentName}/swagger.json");
        app.MapSwaggerUI(setupAction: options =>
        {
            options.RoutePrefix = prefix + "swagger";
            options.SwaggerEndpoint($"{serverSettings.AppVersion}/swagger.json", "AuthenticationProvider " + serverSettings.AppVersion);
        });
    }
}