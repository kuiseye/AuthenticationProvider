using System.ComponentModel.DataAnnotations;

namespace AuthenticationProvider.Configuration.Settings;

public class CookieSettings
{
    [Required(ErrorMessage = nameof(HttpOnly) + " was not provided")]
    public bool HttpOnly {get;set;} = true;

    [Required(ErrorMessage = nameof(Secure) + " was not provided")]
    public bool Secure {get;set;} = true;

    [Required(ErrorMessage = nameof(SameSiteMode) + " was not provided")]
    public SameSiteMode SameSiteMode {get;set;} = SameSiteMode.Lax;
}