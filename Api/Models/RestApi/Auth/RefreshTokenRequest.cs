namespace Filio.Api.Models.RestApi.Auth;

/// <summary>
/// 
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Api Key
    /// </summary>
    /// <value></value>
    public string ApiKey { get; set; } = null!;

    /// <summary>
    /// Api secret
    /// </summary>
    /// <value></value>
    public string ApiSecret { get; set; } = null!;
}