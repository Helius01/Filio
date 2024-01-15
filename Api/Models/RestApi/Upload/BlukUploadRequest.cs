using System.ComponentModel.DataAnnotations;

namespace Filio.Api.Models.RestApi.Upload;

/// <summary>
/// The request model to bulk upload
/// </summary>
public class BulkUploadRequest
{
    /// <summary>
    /// Files to upload
    /// </summary>
    /// <value></value>
    [Required]
    public IFormFileCollection Files { get; set; } = null!;

    /// <summary>
    /// Bucket name
    /// </summary>
    /// <value></value>
    [Required]
    public string BucketName { get; set; } = null!;
}