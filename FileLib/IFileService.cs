using Filio.Common.ErrorHandler;
using Filio.Common.ErrorHandler.RecoverableErrors;
using Filio.FileLib.Models.Delete;
using Filio.FileLib.Models.Get;
using Filio.FileLib.Models.Upload;

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
    /// Uploads a new file
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Either<HttpError, SingleUploadOutput>> UploadAsync(SingleUploadInput input, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a file
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task DeleteAsync(SingleDeleteInput input, CancellationToken cancellationToken = default);


    /// <summary>
    /// Returns the public url
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    string GetPublicUrl(SingleGetInput input);

    /// <summary>
    /// Signs and returns the url
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    string GetSignedUrl(SingleGetInput input);
}
