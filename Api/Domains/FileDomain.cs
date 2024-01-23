using System.Net;
using Filio.Api.Abstractions;
using Filio.Api.Exceptions;
using Microsoft.VisualBasic;

namespace Filio.Api.Domains;

/// <summary>
/// File keyword has been reserved,so the suffix is fine 
/// </summary>
public class FileDomain : BaseEntity
{
    private FileDomain() { }

    /// <summary>
    /// Creates a new file
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="sizeInByte"></param>
    /// <param name="extension"></param>
    /// <param name="originalName"></param>
    /// <param name="type"></param>
    /// <param name="imageBlurhash"></param>
    public FileDomain(string bucketName, long sizeInByte, string extension, string originalName, FileDomainType type, string? imageBlurhash)
    {
        SetBucketName(bucketName);
        SetFileSize(sizeInByte);
        SetExtension(extension);
        SetOriginalName(originalName);
        SetPath(Id, extension);
        SetType(type);

        if (imageBlurhash is not null)
        {
            SetBlurhash(imageBlurhash);
        }
    }
    /// <summary>
    /// File full path 
    /// </summary>
    /// <value>dummy-bucket/2c5b7917-c6de-405d-8f50-b3a6cc780243.jpg</value>
    public string Path { get; private set; } = null!;

    /// <summary>
    /// FileType
    /// </summary>
    /// <value></value>
    public FileDomainType Type { get; private set; }

    /// <summary>
    /// File full path - no store in db
    /// </summary>
    /// <value></value>
    public string FullPath => $"{BucketName}/{Path}";

    /// <summary>
    /// The name of bucket that holds the file
    /// </summary>
    /// <value></value>
    public string BucketName { get; private set; } = null!;

    /// <summary>
    /// File size in byte
    /// </summary>
    /// <value></value>
    public long SizeInByte { get; private set; }

    /// <summary>
    /// The image blurhash
    /// </summary>
    /// <value></value>
    public string? ImageBlurhash { get; private set; }

    /// <summary>
    /// The file extension
    /// </summary>
    /// <value></value>
    public string Extension { get; private set; } = null!;

    /// <summary>
    /// Original file name
    /// </summary>
    /// <value></value>
    public string OriginalName { get; private set; } = null!;

    /// <summary>
    /// Deleted flag for soft delete
    /// </summary>
    /// <value></value>
    public bool IsDeleted { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected override T ToDto<T>()
    {
        throw new NotImplementedException();
    }

    private void SetBucketName(string bucketName)
    {
        if (string.IsNullOrWhiteSpace(bucketName))
            throw new DomainException("Bucket name cannot be empty");

        if (bucketName.Length < 3 || bucketName.Length > 63)
            throw new DomainException("Bucket name must be between 3 and 128 characters");

        if (bucketName.Any(x => char.IsUpper(x)))
            throw new DomainException("Bucket name can't have upper case charachters");

        if (IPAddress.TryParse(bucketName, out _))
            throw new DomainException("Bucket name can't be look likes an IP");

        if (bucketName.StartsWith("xn--"))
            throw new DomainException("Bucket name can't start with xn-- prefix");

        if (bucketName.StartsWith("sthree-"))
            throw new DomainException("Bucket name can't start with sthree- prefix");

        if (bucketName.StartsWith("sthree-configurator"))
            throw new DomainException("Bucket name can't start with sthree-configurator prefix");

        if (bucketName.EndsWith("-s3alias"))
            throw new DomainException("Bucket name can't start with -s3alias suffix");

        if (bucketName.EndsWith("--ol-s3"))
            throw new DomainException("Bucket name can't start with --ol-s3 suffix");

        if (bucketName.Contains('.'))
            throw new DomainException("Bucket name can't containts dot");

        BucketName = bucketName;
    }

    private void SetFileSize(long sizeInByte)
    {
        if (sizeInByte < 1)
            throw new DomainException("File size cannot be less than 1 byte");

        SizeInByte = sizeInByte;
    }

    private void SetExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            throw new DomainException("Extension cannot be empty");

        if (extension.Length < 1 || extension.Length > 12)
            throw new DomainException("Extension must be between 1 and 12 characters");

        Extension = extension;
    }

    private void SetOriginalName(string originalName)
    {
        if (string.IsNullOrWhiteSpace(originalName))
            throw new DomainException("Original name cannot be empty");

        OriginalName = originalName;
    }

    private void SetPath(Guid id, string extension)
    {
        Path = $"{id}{extension}";
    }

    private void SetType(FileDomainType type)
    {
        //TODO:Validate
        Type = type;
    }

    private void SetBlurhash(string imageBlurhash)
    {
        if (string.IsNullOrWhiteSpace(imageBlurhash))
            throw new DomainException("Blurhash cannot be empty or null");
        ImageBlurhash = imageBlurhash;
    }

    /// <summary>
    /// Deletes the file
    /// </summary>
    /// <remarks>
    /// Soft Delete in database
    /// </remarks>
    public void Delete() => IsDeleted = true;

    /// <summary>
    /// Updates the image blurhash
    /// </summary>
    /// <param name="imageBlurhash">The new blurhash to replace</param>
    /// <exception cref="DomainException" />
    public void UpdateBlurhash(string imageBlurhash)
    {
        if (string.IsNullOrWhiteSpace(imageBlurhash))
            throw new DomainException("Blurhash cannot be empty or null");

        ImageBlurhash = imageBlurhash;
    }
}