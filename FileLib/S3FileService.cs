using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Filio.Common.ErrorHandler;
using Filio.Common.ErrorHandler.RecoverableErrors;
using Filio.FileLib.Models.Delete;
using Filio.FileLib.Models.Get;
using Filio.FileLib.Models.Upload;
using Filio.FileLib.Settings.Aws;
using Microsoft.AspNetCore.Hosting;

namespace Filio.FileLib;

/// <summary>
/// The implementation of IFileService which integrated with AWS S3 
/// </summary>
internal sealed class S3FileService(AmazonS3Client client, AwsSettings awsSettings, IHostingEnvironment environment) : IFileService
{
    private readonly AmazonS3Client _client = client;
    private readonly AwsSettings _awsSettings = awsSettings;
    private readonly IHostingEnvironment _environment = environment;

    ///<inheritdoc />
    public Task DeleteAsync(SingleDeleteInput input, CancellationToken cancellationToken = default)
    {
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = input.Bucket,
            Key = input.Path
        };

        return _client.DeleteObjectAsync(deleteRequest, cancellationToken);
    }

    ///<inheritdoc />
    public string GetSignedUrl(SingleGetInput input)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = input.Bucket,
            Key = input.Path,
            Expires = DateTime.Now.AddMinutes(_awsSettings.ExpirationTimeInMinutes),
            Protocol = _environment.IsDevelopment() ? Protocol.HTTP : Protocol.HTTPS
        };

        return _client.GetPreSignedURL(request);
    }

    ///<inheritdoc />
    public string GetPublicUrl(SingleGetInput input)
    {
        var url = _awsSettings.ServiceUrl;

        //TODO:Should validates bucket ACL ???
        return $"{url.Trim('/')}/{input.Bucket}/{input.Path.Trim('/')}";
    }

    ///<inheritdoc />
    public async Task UploadAsync(SingleUploadInput input, CancellationToken cancellationToken = default)
    {
        using var fileTransferUtility = new TransferUtility(_client);

        //Creating upload request
        var uploadRequest = new TransferUtilityUploadRequest
        {
            BucketName = input.Bucket,
            Key = input.Path,
            InputStream = input.Stream,
            AutoCloseStream = true,
            AutoResetStreamPosition = true,
        };

        await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken).ConfigureAwait(false);
    }
}