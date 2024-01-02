using System.Security.Claims;

namespace Filio.Api.Services.Auth;

public interface IAuthService
{
    /// <summary>
    /// Generates an access token via the provided claims
    /// </summary>
    /// <param name="AccessToken"></param>
    /// <param name="claims"></param>
    /// <returns>A Tuple which does contain the access token and its expiration</returns>
    (string AccessToken, DateTime AccessTokenExpiration) GenerateAccessToken(IEnumerable<Claim> claims);

    /// <summary>
    /// Generates a refresh token with the specified byte length
    /// </summary>
    /// <param name="byteLength"></param>
    /// <returns>A new refresh token</returns>
    string GenerateRefreshToken(int byteLength = 64);
}