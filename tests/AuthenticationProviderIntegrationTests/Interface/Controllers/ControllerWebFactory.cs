using AuthenticationProvider.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;

namespace AuthenticationProvider.Tests.Integration.Interface.Controllers;

public class ControllerWebFactory : WebApplicationFactory<Program>
{
    public IAuthenticationService AuthenticationService {get;set;} = Substitute.For<IAuthenticationService>();
    public FakeTimeProvider FakeTimeProvider {get;} = new FakeTimeProvider(DateTimeOffset.Now);
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseContentRoot(AppContext.BaseDirectory);
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(_ => FakeTimeProvider);

            services.RemoveAll<IAuthenticationService>();
            services.AddSingleton(_ => AuthenticationService);
        });
    }
}