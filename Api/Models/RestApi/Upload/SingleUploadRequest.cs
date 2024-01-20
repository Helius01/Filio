using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Filio.Api.Models.RestApi.Upload;

/// <summary>
/// The request model to upload a single object
/// </summary>
public sealed class SingleUploadRequest
{
    /// <summary>
    /// The name of bucket to store object
    /// </summary>
    /// <value></value>
    [Required]
    [MinLength(3)]
    public string BucketName { get; set; } = null!;

    /// <summary>
    /// The file to upload
    /// </summary>
    /// <value></value>
    [Required]
    public IFormFile File { get; set; } = null!;
}