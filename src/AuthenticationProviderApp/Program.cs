using AuthenticationProvider;
using AuthenticationProvider.Application.Services;
using AuthenticationProvider.Configuration;
using AuthenticationProvider.Configuration.ConfigurationOptions;
using AuthenticationProvider.Configuration.Settings;
using AuthenticationProvider.Domain.Entities;
using AuthenticationProvider.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OpenTelemetry.Metrics;


var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureOptions<CustomSwaggerGenOptions>();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalException>();
//Options
builder.Services.AddApplicationOptions(builder.Configuration);
//Http
builder.Services.AddControllers();
//DbContext
builder.Services.AddApplicationDbContext(builder.Configuration);
//Authentication
builder.Services.AddJwtCookieAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddAppUserClaimsDto();
builder.Services.AddSingleton<IPasswordHasher<AppUser>, PasswordHasher<AppUser>>();
//Other
builder.Services.AddHealthChecks();
//Services

builder.Services.AddSingleton(TimeProvider.System);  
builder.Services.AddScoped<IRequestClock, RequestClock>();
builder.Services.AddScoped<ICookieManager, CookieManager>();

builder.Services.AddScoped<IAppUserRepository,AppUserRepository>();
builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
builder.Services.AddScoped<ITokenService,TokenService>();

builder.Services.AddOpenTelemetry()
.WithMetrics(m=>m.AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddRuntimeInstrumentation()
        .AddProcessInstrumentation().AddPrometheusExporter());

var app = builder.Build();

app.MapHealthChecks("health");

app.MapPrometheusScrapingEndpoint();
app.UseExceptionHandler();
//Dev
if (app.Environment.IsDevelopment())
{
    app.MapSwaggerEndpoints();
}

//Auth
app.UseAuthentication();
app.UseAuthorization();
//Endpoints
var serverOptions = app.Services.GetRequiredService<IOptions<ServerSettings>>();
var prefix = serverOptions.Value.UrlPrefix + serverOptions.Value.AppVersion;

app.MapGroup(prefix).MapControllers();

app.Run();
