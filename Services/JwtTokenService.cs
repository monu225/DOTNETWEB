using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WEBAPI_CRUD.DTOs;
using WEBAPI_CRUD.Models;

namespace WEBAPI_CRUD.Services;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;
    private readonly IJwtSigningKeyProvider _signingKeyProvider;

    public JwtTokenService(IConfiguration configuration, IJwtSigningKeyProvider signingKeyProvider)
    {
        _configuration = configuration;
        _signingKeyProvider = signingKeyProvider;
    }

    public AuthResponse GenerateToken(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is missing.");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("Jwt:Audience is missing.");
        var expiresInMinutes = int.TryParse(jwtSettings["ExpiresInMinutes"], out var configuredMinutes)
            ? configuredMinutes
            : 60;
        var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, user.Role)
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_signingKeyProvider.GetSigningKey())),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expiresAt,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role
        };
    }
}
