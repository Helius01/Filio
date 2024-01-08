using Filio.Api.Abstractions;

namespace Filio.Api.Domains;

/// <summary>
/// File keyword has been reserved,so the suffix fine 
/// </summary>
public class FileDomain : BaseEntity
{
    /// <summary>
    /// File full path 
    /// </summary>
    /// <value>dummy-bucket/2c5b7917-c6de-405d-8f50-b3a6cc780243.jpg</value>
    public string Path { get; set; } = null!;

    /// <summary>
    /// The name of bucket that holds the file
    /// </summary>
    /// <value></value>
    public string BucketName { get; set; } = null!;

    /// <summary>
    /// File size in byte
    /// </summary>
    /// <value></value>
    public int SizeInByte { get; set; }

    /// <summary>
    /// Metadata
    /// </summary>
    /// <value></value>
    public Dictionary<string, string> MetaData { get; set; } = null!;

    /// <summary>
    /// The image blurhash
    /// </summary>
    /// <value></value>
    public string? ImageBlurhash { get; set; }

    /// <summary>
    /// The file extension
    /// </summary>
    /// <value></value>
    public string Extension { get; set; } = null!;

    /// <summary>
    /// Original file name
    /// </summary>
    /// <value></value>
    public string OriginalName { get; set; } = null!;

    /// <summary>
    /// Deleted flag for soft delete
    /// </summary>
    /// <value></value>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected override T ToDto<T>()
    {
        throw new NotImplementedException();
    }
}