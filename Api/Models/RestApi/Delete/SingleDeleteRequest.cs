using System.ComponentModel.DataAnnotations;

namespace Filio.Api.Models.RestApi.Delete;

/// <summary>
/// The request model to delete object
/// </summary>
public class SingleDeleteRequest
{
    /// <summary>
    /// Bucket name
    /// </summary>
    /// <value></value>
    [Required]
    public string BucketName { get; set; }=null!;

    /// <summary>
    /// File path
    /// </summary>
    /// <value></value>
    [Required]
    public string Path { get; set; } = null!;
}