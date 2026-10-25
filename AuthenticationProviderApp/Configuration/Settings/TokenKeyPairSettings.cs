using System.ComponentModel.DataAnnotations;

namespace AuthenticationProvider.Configuration.Settings;

public class TokenKeyPairSettings
{
    [Required(ErrorMessage = nameof(SigningKey) + " was not provided")]
    public string SigningKey {get;set;} = null!;

    [Required(ErrorMessage = nameof(VerificationKey) + " was not provided")]
    public string VerificationKey {get;set;} = null!;
}