using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Filio.Common.ErrorHandler;
using Filio.Common.ErrorHandler.RecoverableErrors;
using Filio.FileLib.Models.Delete;
using Filio.FileLib.Models.Get;
using Filio.FileLib.Models.Upload;
using Filio.FileLib.Settings.Aws;

namespace Filio.FileLib;

/// <summary>
/// The implementation of IFileService which integrated with AWS S3 
/// </summary>
internal sealed class S3FileService(AmazonS3Client client, AwsSettings awsSettings) : IFileService
{
    private readonly AmazonS3Client _client = client;
    private readonly AwsSettings _awsSettings = awsSettings;

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
        //TODO:Detect protocol from env
        var request = new GetPreSignedUrlRequest
        {
            BucketName = input.Bucket,
            Key = input.Path,
            Expires = DateTime.Now.AddMinutes(_awsSettings.ExpirationTimeInMinutes),
            Protocol = Protocol.HTTP
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
    public async Task<Either<HttpError, SingleUploadOutput>> UploadAsync(SingleUploadInput input, CancellationToken cancellationToken = default)
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

        try
        {
            await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken).ConfigureAwait(false);
        }

        //If the exception is not type of AmazonS3Exception, Let it throw
        catch (AmazonS3Exception exception)
        {
            //TODO:Log and capture 
            return Either<HttpError, SingleUploadOutput>.Left(new HttpError(exception.Message, exception.StatusCode));
        }

        var signedUrl = GetSignedUrl(new SingleGetInput(bucket: input.Bucket, path: input.Path));
        var publicUrl = GetPublicUrl(new SingleGetInput(bucket: input.Bucket, path: input.Path));

        return Either<HttpError, SingleUploadOutput>.Right(new SingleUploadOutput(publicUrl: publicUrl, signedUrl: signedUrl));
    }
}