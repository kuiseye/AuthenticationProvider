using System.ComponentModel.DataAnnotations;

namespace AuthenticationProvider.Configuration.Settings;

public class ServerSettings
{
    [Required(ErrorMessage = nameof(UrlPrefix) + " was not provided")]
    public string UrlPrefix {get;set;} = null!;

    [Required(ErrorMessage = nameof(AppVersion) + " was not provided")]
    public string AppVersion {get;set;} = null!;

    [Required(ErrorMessage = nameof(DomainName) + " was not provided")]
    public string DomainName {get;set;} = null!;
}