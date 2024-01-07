using System.Security.Claims;
using Filio.Api.Models.RestApi.Auth;
using Filio.Api.Services.Auth;
using Filio.Api.Settings.ApiKey;
using Microsoft.AspNetCore.Mvc;

namespace Filio.Api.Controllers;

/// <summary>
/// Auth Controller
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApiKeySettings _apiKeySettings;
    private readonly IAuthService _authService;

    public AuthController(ApiKeySettings apiKeySettings, IAuthService authService)
    {
        _apiKeySettings = apiKeySettings;
        _authService = authService;
    }
    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!(_apiKeySettings.ApiKey == request.ApiKey && BCrypt.Net.BCrypt.Verify(request.ApiSecret, _apiKeySettings.ApiSecretHash)))
        {
            //TODO:Aggressive brute force protection
            return Unauthorized();
        }

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier,request.ApiKey),
            new Claim(ClaimTypes.Role,"User")
         };

        var jwtResult = _authService.GenerateAccessToken(claims);


        return Ok(jwtResult.AccessToken);
    }
}