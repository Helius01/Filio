namespace Filio.Api.Models.RestApi.Upload;

/// <summary>
/// The response model for single upload
/// </summary>
public class SingleUploadResponse
{
    /// <summary>
    /// The uploaded file signed url
    /// </summary>
    /// <value></value>
    public string SignedUrl { get; set; } = null!;

    /// <summary>
    /// The uploaded file public url
    /// </summary>
    /// <value></value>
    public string PublicUrl { get; set; } = null!;

    /// <summary>
    /// The file id 
    /// </summary>
    /// <value></value>
    public Guid FileId { get; set; }
}