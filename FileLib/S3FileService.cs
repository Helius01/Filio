using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Filio.ErrorHandler;
using Filio.FileLib.Settings.Aws;

namespace Filio.FileLib;

/// <summary>
/// The implementation of IFileService which integrated with AWS S3 
/// </summary>
internal sealed class S3FileService : IFileService
{
    private readonly AmazonS3Client _client;
    private readonly AwsSettings _awsSettings;

    public S3FileService(AmazonS3Client client, AwsSettings awsSettings)
    {
        _client = client;
        _awsSettings = awsSettings;
    }

    ///<inheritdoc />
    public Task DeleteAsync(string bucket, string path)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucket,
            Key = path
        };

        return _client.DeleteObjectAsync(deleteRequest);
    }

    ///<inheritdoc />
    public string GetSignedUrl(string bucket, string path, int expirationInMinute = 10)
    {
        //TODO:Detect protocol from env
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = path,
            Expires = DateTime.Now.AddMinutes(expirationInMinute),
            Protocol = Protocol.HTTP
        };

        return _client.GetPreSignedURL(request);
    }

    ///<inheritdoc />
    public string GetPublicUrl(string bucket, string path)
    {
        var url = _awsSettings.ServiceUrl;

        //TODO:Should validates bucket ACL ???
        return $"{url.Trim('/')}/{bucket}/{path.Trim('/')}";
    }

    ///<inheritdoc />
    public async Task<Result<(string SignedUrl, string PublicUrl), HttpError>> UploadAsync(Stream fileStream, string bucket, string path)
    {
        using var fileTransferUtility = new TransferUtility(_client);

        //TODO:After the implementation of COMPRESS and BLURHASH feature. i can't close the stream.
        var uploadRequest = new TransferUtilityUploadRequest
        {
            BucketName = bucket,
            Key = path,
            InputStream = fileStream,
            AutoCloseStream = true,
            AutoResetStreamPosition = true,
        };


        try
        {
            await fileTransferUtility.UploadAsync(uploadRequest).ConfigureAwait(false);
        }
        //If the exception is not type of AmazonS3Exception, Let it throw
        catch (AmazonS3Exception exception)
        {
            return Result<(string, string), HttpError>.Failure(new HttpError(exception.Message, exception.StatusCode));
        }

        var signedUrl = GetSignedUrl(bucket, path);
        var publicUrl = GetPublicUrl(bucket, path);

        return Result<(string, string), HttpError>.Success((signedUrl, publicUrl));
    }
}