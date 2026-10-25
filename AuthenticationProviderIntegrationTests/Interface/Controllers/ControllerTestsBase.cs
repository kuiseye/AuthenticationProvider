using AuthenticationProvider.Application.Services;
using AuthenticationProvider.Configuration.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthenticationProvider.Tests.Integration.Interface.Controllers;

public class ControllerTestsBase : IClassFixture<ControllerWebFactory>
{

    protected IServiceScope _scope;
    protected HttpClient _client;

    public ControllerTestsBase(ControllerWebFactory webFactory)
    {
        _scope = webFactory.Services.CreateScope();

        _client = webFactory.CreateClient(new WebApplicationFactoryClientOptions
        {
           BaseAddress = new Uri(webFactory.Server.BaseAddress, GetPrefix())
        });
    }

    private string GetPrefix()
    {
        var serverOptions = _scope.ServiceProvider.GetRequiredService<IOptions<ServerSettings>>();
        string prefix = serverOptions.Value.UrlPrefix + serverOptions.Value.AppVersion + '/';
        return prefix;
    }
}