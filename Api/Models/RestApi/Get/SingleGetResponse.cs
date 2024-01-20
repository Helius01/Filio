namespace Filio.Api.Models.RestApi.Get;

/// <summary>
/// Single Get
/// </summary>
public class SingleGetResponse
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="signedUrl"></param>
    /// <param name="publicUrl"></param>
    /// <param name="bucket"></param>
    /// <param name="imageBlurhash"></param>
    /// <param name="type"></param>
    public SingleGetResponse(string signedUrl, string publicUrl, string bucket, string? imageBlurhash, string type)
    {
        SignedUrl = signedUrl;
        PublicUrl = publicUrl;
        Bucket = bucket;
        ImageBlurhash = imageBlurhash;
        Type = type;
    }

    /// <summary>
    /// The file signed url
    /// </summary>
    /// <value></value>
    public string SignedUrl { get; private set; } = null!;

    /// <summary>
    /// The file public url
    /// </summary>
    /// <value></value>
    public string PublicUrl { get; private set; } = null!;

    /// <summary>
    /// Bucket name
    /// </summary>
    /// <value></value>
    public string Bucket { get; private set; } = null!;

    /// <summary>
    /// image blurhash
    /// </summary>
    /// <value></value>
    public string? ImageBlurhash { get; private set; }

    /// <summary>
    /// File type
    /// </summary>
    /// <value></value>
    public string Type { get; private set; } = null!;
}