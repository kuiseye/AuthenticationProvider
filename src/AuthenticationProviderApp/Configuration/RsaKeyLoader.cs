using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationProvider.Configuration;

public static class RsaKeyLoader
{
    public static RsaSecurityKey LoadRsaSigningKeyFromEnvVariable(string envVariable)
    {
        return LoadRsaKeyFromEnvVariable(envVariable, true);
    }
    public static RsaSecurityKey LoadRsaVerificationKeyFromEnvVariable(string envVariable)
    {
        return LoadRsaKeyFromEnvVariable(envVariable, false);
    }
    private static RsaSecurityKey LoadRsaKeyFromEnvVariable(string envVariable, bool includePrivateParameters = false)
    {
        var pem = Encoding.UTF8.GetString(Convert.FromBase64String(envVariable));
        using var rsa = RSA.Create();
        rsa.ImportFromPem(pem);
        return new RsaSecurityKey(rsa.ExportParameters(includePrivateParameters));
    }
}