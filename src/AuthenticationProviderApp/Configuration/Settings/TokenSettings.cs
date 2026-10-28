using System.ComponentModel.DataAnnotations;

namespace AuthenticationProvider.Configuration.Settings;

public class TokenSettings
{
    [Required(ErrorMessage = nameof(Issuer) + " was not provided")]
    public string Issuer {get;set;} = null!;

    [Required(ErrorMessage = nameof(Audience) + " was not provided")]
    public string Audience {get;set;} = null!;

    [Required(ErrorMessage = nameof(Lifetime) + " was not provided")]
    public TimeSpan Lifetime {get;set;} = TimeSpan.Zero;
    
    [Required(ErrorMessage = nameof(CookieName) + " was not provided")]
    public string CookieName {get;set;} = null!;
}