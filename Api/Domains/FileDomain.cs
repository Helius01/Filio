using Amazon.S3.Encryption.Internal;
using Filio.Api.Abstractions;
using Filio.Api.Exceptions;

namespace Filio.Api.Domains;

/// <summary>
/// File keyword has been reserved,so the suffix fine 
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
    public FileDomain(string bucketName, long sizeInByte, string extension, string originalName, string type)
    {
        SetBucketName(bucketName);
        SetFileSize(sizeInByte);
        SetExtension(extension);
        SetOriginalName(originalName);
        SetPath(Id, extension);
        SetType(type);
    }

    /// <summary>
    /// Creates a new file with metadata and blurhash
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="sizeInByte"></param>
    /// <param name="extension"></param>
    /// <param name="originalName"></param>
    /// <param name="type"></param>
    /// <param name="metaData"></param>
    /// <param name="imageBlurhash"></param>
    /// <see  cref="FileDomain(string, long, string, string, string)"/>
    /// <see  cref="FileDomain(string, long, string, string, string, string, Dictionary{string, string}?)"/>
    public FileDomain(string bucketName, long sizeInByte, string extension, string originalName, string type, Dictionary<string, string> metaData, string? imageBlurhash = null)
    {
        SetBucketName(bucketName);
        SetFileSize(sizeInByte);
        SetExtension(extension);
        SetOriginalName(originalName);
        SetPath(Id, extension);
        SetType(type);
        SetMetaData(metaData);
        SetMetaData(metaData);

        if (imageBlurhash is not null)
        {
            SetBlurhash(imageBlurhash);
        }
    }

    /// <summary>
    /// Creates a new file with metadata and blurhash
    /// </summary>
    /// <param name="bucketName"></param>
    /// <param name="sizeInByte"></param>
    /// <param name="extension"></param>
    /// <param name="originalName"></param>
    /// <param name="type"></param>
    /// <param name="imageBlurhash"></param>
    /// <param name="metaData"></param>
    /// <see  cref="FileDomain(string, long, string, string, string)"/>
    /// <see  cref="FileDomain(string, long, string, string, string, Dictionary{string, string}, string)"/>
    public FileDomain(string bucketName, long sizeInByte, string extension, string originalName, string type, string imageBlurhash, Dictionary<string, string>? metaData = null)
    {

        SetBucketName(bucketName);
        SetFileSize(sizeInByte);
        SetExtension(extension);
        SetOriginalName(originalName);
        SetPath(Id, extension);
        SetType(type);

        if (metaData is not null)
        {
            SetMetaData(metaData);
        }

        SetBlurhash(imageBlurhash);
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
    public string Type { get; set; } = null!;

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
    /// Metadata
    /// </summary>
    /// <value></value>
    public Dictionary<string, string>? MetaData { get; private set; }

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


        if (bucketName.Length < 3 || bucketName.Length > 128)
            throw new DomainException("Bucket name must be between 3 and 128 characters");

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

    private void SetType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new DomainException("Type cannot be empty");

        Type = type;
    }

    private void SetMetaData(Dictionary<string, string> metadata)
    {
        if (metadata.Count < 1)
            throw new DomainException("Metadata cannot be empty. Also you can use another constructor");

        MetaData = metadata;
    }

    private void SetBlurhash(string imageBlurhash)
    {
        ImageBlurhash = imageBlurhash;
    }
}