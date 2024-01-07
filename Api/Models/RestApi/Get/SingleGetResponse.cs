namespace Filio.Api.Models.RestApi.Get;

/// <summary>
/// Single Get
/// </summary>
public class SingleGetResponse
{
    /// <summary>
    /// File signed url
    /// </summary>
    /// <value></value>
    public string SignedUrl { get; set; } = null!;
}