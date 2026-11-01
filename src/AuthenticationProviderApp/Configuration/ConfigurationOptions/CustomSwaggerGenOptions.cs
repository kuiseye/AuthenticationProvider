using Swashbuckle.AspNetCore.SwaggerGen;
using AuthenticationProvider.Configuration.Settings;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authorization;

namespace AuthenticationProvider.Configuration.ConfigurationOptions;

public class CustomSwaggerGenOptions(IOptions<TokenSettings> tokenSettings, IOptions<ServerSettings> serverOptions) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.AddServer(new OpenApiServer(){Url = '/' + serverOptions.Value.UrlPrefix + serverOptions.Value.AppVersion + '/'});

        options.SwaggerDoc(serverOptions.Value.AppVersion, new OpenApiInfo
        {
            Title = "AuthenticationProvider API",
            Version = serverOptions.Value.AppVersion,
            Description = "API for Authentication Services"
        });

        options.AddSecurityDefinition(ApplicationSchemes.JwtCookieSchema, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.ApiKey,
            In = ParameterLocation.Cookie,
            Name = tokenSettings.Value.CookieName,  
            Description = "Authentication cookie containing jwt token"
        });
        options.OperationFilter<AuthorizeCheckOperationFilter>();
    }
}

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool hasAuthorize = context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
            || (context.MethodInfo.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ?? false);

        if(!hasAuthorize) return;

        operation.Security ??= [];
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference(ApplicationSchemes.JwtCookieSchema, context.Document)] = []
        });
    }
}