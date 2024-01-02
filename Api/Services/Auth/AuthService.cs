using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Filio.Api.Settings.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Filio.Api.Services.Auth;

/// <summary>
/// The implementation of IAuthService
/// </summary>
public class AuthService : IAuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _signingSecret;
    private readonly byte[] _encryptionSecret;

    public AuthService(JwtSettings jwtSettings)
    {
        _jwtSettings = jwtSettings;
        _signingSecret = Encoding.UTF8.GetBytes(jwtSettings.SigningSecret);
        _encryptionSecret = Encoding.UTF8.GetBytes(jwtSettings.EncryptionSecret);
    }

    public (string AccessToken, DateTime AccessTokenExpiration) GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

        var signingKey = new SymmetricSecurityKey(_signingSecret);
        var encryptionKey = new SymmetricSecurityKey(_encryptionSecret);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
            EncryptingCredentials = new EncryptingCredentials(encryptionKey, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512)
        };

        var accessToken = new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);

        return (accessToken, expiresAt);
    }

    public string GenerateRefreshToken(int byteLength = 64)
    {
        var bytes = new byte[byteLength];
        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        return Convert.ToBase64String(bytes).Replace("=", "").Replace('+', '-').Replace('/', '_');
    }
}