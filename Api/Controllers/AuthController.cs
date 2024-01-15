using System.Security.Claims;
using Filio.Api.Models.RestApi.Auth;
using Filio.Api.Services.Auth;
using Filio.Api.Settings.ApiKey;
using Microsoft.AspNetCore.Mvc;

namespace Filio.Api.Controllers;

//TODO:The authentication should be optional. Also it must implement via database to store api keys.
/// <summary>
/// Auth Controller
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApiKeySettings _apiKeySettings;
    private readonly IAuthService _authService;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="apiKeySettings"></param>
    /// <param name="authService"></param>

    public AuthController(ApiKeySettings apiKeySettings, IAuthService authService)
    {
        _apiKeySettings = apiKeySettings;
        _authService = authService;
    }

    /// <summary>
    /// Check your api key and returns the access_token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!(_apiKeySettings.ApiKey == request.ApiKey && BCrypt.Net.BCrypt.Verify(request.ApiSecret, _apiKeySettings.ApiSecretHash)))
        {
            //TODO:Log data as much as possible
            //TODO:Aggressive brute force protection
            return Unauthorized();
        }

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier,request.ApiKey),
            new Claim(ClaimTypes.Role,"User")
         };

        var (accessToken, _) = _authService.GenerateAccessToken(claims);


        return Ok(accessToken);
    }
}