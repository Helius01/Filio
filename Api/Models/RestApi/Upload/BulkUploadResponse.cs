namespace Filio.Api.Models.RestApi.Upload;

/// <summary>
/// The response model on bulk upload
/// </summary>
public class BulkUploadResponse
{
    /// <summary>
    /// The list of uploaded files
    /// </summary>
    /// <value></value>
    public List<Guid> FileIds { get; set; } = null!;
}