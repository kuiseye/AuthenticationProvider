using System.Text;
using AuthenticationProvider.Configuration.Settings;
using AuthenticationProvider.AppInterface.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationProvider.Configuration.ConfigurationOptions;

public class CustomJwtBearerOptions(IOptions<TokenSettings> tokenOptions, IOptions<TokenKeyPairSettings> tokenKeyPairOptions)
     : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            NameClaimType = ApplicationClaimTypes.Id,
            RoleClaimType = ApplicationClaimTypes.Role,

            ValidateIssuer = true,
            ValidIssuer = tokenOptions.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = tokenOptions.Value.Audience,

            ValidateLifetime = true,

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyPairOptions.Value.VerificationKey))
        };
        options.Events.OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies.FirstOrDefault(cookie=>cookie.Key == tokenOptions.Value.CookieName).Value;
            return Task.CompletedTask;
        };
        options.Challenge = ApplicationSchemes.JwtCookieSchema;
    }
}