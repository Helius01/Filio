using Filio.Common.ErrorHandler;
using Filio.Common.ErrorHandler.RecoverableErrors;
namespace Filio.FileLib;

/// <summary>
/// Provides many functionalities to manage files and objects
/// </summary>
public interface IFileService
{
    /*****************
     * SINGLE OBJECT *
     *****************/

    /// <summary>
    /// Uploads a new object
    /// </summary>
    /// <param name="fileStream">The stream</param>
    /// <param name="bucket">bucket name to upload.Only set the bucket name without any slash or backs slash</param>
    /// <param name="path" example="ss">The file path with specified extensions and folder(folder is optional)</param>
    /// <returns>A Result type (UploadedUrl,HttpError)</returns>
    /// <example>
    /// <code>
    /// await UploadAsync(myFile,"secrets","very-secrets/card.jpg"); => object location = /secrets/very-secrets/card.jpg
    /// await UploadAsync(myFile,"secrets","card.jpg"); => object location = /secrets/card.jpg
    /// </code>
    /// </example>
    Task<Result<(string SignedUrl, string PublicUrl), HttpError>> UploadAsync(Stream fileStream, string bucket, string path);

    /// <summary>
    /// Deletes an object via bucket name and object path
    /// </summary>
    /// <param name="bucket">bucket name</param>
    /// <param name="path">object path</param>
    /// <returns>Awaitable task</returns>
    Task DeleteAsync(string bucket, string path);

    /// <summary>
    /// Returns the object url via given bucket name and object path
    /// </summary>
    /// <remarks>
    /// The function used to public buckets not private read access buckets.<br />
    /// The function doesn't effect access key or signature on the generated url
    /// </remarks>
    /// <seealso cref="IFileService.GetSignedUrl(string, string)"/>
    /// <param name="bucket">bucket name</param>
    /// <param name="path">object path</param>
    /// <returns>https://object-storgae-service-url/bucket/object-path</returns>
    string GetPublicUrl(string bucket, string path);


    /// <summary>
    /// Generates and returns a presigned url for private buckets.
    /// </summary>
    /// <seealso cref="GetPublicUrl(string, string)"/>
    /// <param name="bucket">Bucket name</param>
    /// <param name="path">Objet path</param>
    /// <param name="expirationInMinute">Expiration time in minute. The url expires after the given minutes</param>
    /// <returns>A presigned url which does contains access key and signature</returns>
    string GetSignedUrl(string bucket, string path, int expirationInMinute = 10);
}
