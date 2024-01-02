using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
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
    public async Task DeleteAsync(string bucket, string path)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = bucket,
            Key = path
        };

        var _ = await _client.DeleteObjectAsync(deleteRequest);

        //TODO:Handle delete response 
        //Possible not found exception or forbidden.
        //The function should used to Result approach for the function Result
    }

    ///<inheritdoc />
    public string GetSignedUrl(string bucket, string path, int expirationInMinute = 10)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = path,
            Expires = DateTime.Now.AddMinutes(expirationInMinute),
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
    public async Task UploadAsync(Stream fileStream, string bucket, string path)
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

        //TODO:Should i raise exception?
        await fileTransferUtility.UploadAsync(uploadRequest).ConfigureAwait(false);
    }
}