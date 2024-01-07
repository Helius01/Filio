namespace Filio.Api.Settings.ApiKey;

/// <summary>
/// Holds Settings
/// </summary>
public class ApiKeySettings
{
    /// <summary>
    /// Api key
    /// </summary>
    /// <value></value>
    public string ApiKey { get; init; } = null!;

    /// <summary>
    /// The hash of api secret 
    /// </summary>
    /// <value></value>
    public string ApiSecretHash { get; init; } = null!;
}