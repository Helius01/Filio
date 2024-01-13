namespace Filio.FileLib.Settings.Aws;

/// <summary>
/// The class holds aws setting values
/// </summary>
public sealed class AwsSettings
{
    /// <summary>
    /// Service url 
    /// </summary>
    /// <value></value>
    public string ServiceUrl { get; init; } = null!;

    /// <summary>
    /// AccessKey
    /// </summary>
    /// <value></value>
    public string AccessKey { get; init; } = null!;

    /// <summary>
    /// SecretKey
    /// </summary>
    /// <value></value>
    public string SecretKey { get; init; } = null!;

    /// <summary>
    /// Max retry on error
    /// </summary>
    /// <value></value>
    public int MaxErrorRetry { get; init; }

    /// <summary>
    /// Expiration time for signed urls in minutes
    /// </summary>
    /// <value></value>
    public int ExpirationTimeInMinutes { get; init; }
}