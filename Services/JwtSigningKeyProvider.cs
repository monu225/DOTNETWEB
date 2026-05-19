using System.Security.Cryptography;

namespace WEBAPI_CRUD.Services;

public sealed class JwtSigningKeyProvider : IJwtSigningKeyProvider
{
    private readonly string _signingKey;

    public JwtSigningKeyProvider(IConfiguration configuration)
    {
        var configuredKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
        var runtimeKey = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        _signingKey = $"{configuredKey}.{runtimeKey}";
    }

    public string GetSigningKey() => _signingKey;
}
