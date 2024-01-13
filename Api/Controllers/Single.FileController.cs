using System.Net;
using Filio.Api.Data;
using Filio.Api.Domains;
using Filio.Api.Models.RestApi.Get;
using Filio.Api.Models.RestApi.Upload;
using Filio.Common.ErrorHandler;
using Filio.Common.ErrorHandler.RecoverableErrors;
using Filio.Common.FileDetector;
using Filio.FileLib.Models.Delete;
using Filio.FileLib.Models.Upload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Filio.Api.Controllers;

public partial class FileController
{
    /// <summary>
    /// Uploads a single file 
    /// </summary>
    /// <param name="request"></param>
    /// <response code="500">Error: Panic</response>
    /// <response code="404">Error:Usually on wrong bucket name</response>
    /// <response code="400">Error:Wrong request or invalid data</response>
    /// <response code="200">Success: SingleUploadResponse</response>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(SingleUploadResponse), StatusCodes.Status200OK)]
    [HttpPost("single")]
    public async Task<IActionResult> Upload([FromForm] SingleUploadRequest request)
    {
        using var fileStream = request.File.OpenReadStream();

        var fileType = _fileDetectorService.GetType(fileStream);

        if (fileType == FileType.None)
        {
            //TODO:Log error
            //TODO:Use strong type for error response
            return BadRequest(new { Error = "File type not supported" });
        }

        var blurhash = string.Empty;

        if (fileType == FileType.Image)
        {
            //Generating blurhash
            blurhash = await _imageLibService.GenerateBlurhashAsync(fileStream);
        }

        var newFile = new FileDomain(bucketName: request.BucketName,
                                    sizeInByte: request.File.Length,
                                    extension: Path.GetExtension(request.File.FileName),
                                    originalName: request.File.FileName,
                                    type: request.File.ContentType);

        if (!string.IsNullOrEmpty(blurhash))
        {
            newFile.UpdateBlurhash(blurhash);
        }

        //TODO:It doesn't works we needs creates a serializable dictionary in form mode
        if (request.MetaData != null && request.MetaData.Count > 0)
        {
            newFile.UpdateMetadata(request.MetaData);
        }


        _context.FileDomains.Add(newFile);


        Either<HttpError, SingleUploadOutput> result = new();

        await ResilientTransaction.Create(_context).ExecuteAsync(async () =>
        {
            await _context.SaveChangesAsync();

            result = await _fileService.UploadAsync(new SingleUploadInput(stream: fileStream,
                                                                            path: newFile.Path,
                                                                            bucket: newFile.BucketName,
                                                                        metadata: request.MetaData));
        });


        return result.Match<IActionResult>(
            left => left.StatusCode switch
            {
                HttpStatusCode.BadRequest => BadRequest(new { Error = left.Message }),
                HttpStatusCode.NotFound => NotFound(new { Error = left.Message }),
                _ => Problem(detail: left.Message, statusCode: (int)left.StatusCode)
            }
        , right => Ok(new SingleUploadResponse
        {
            FileId = newFile.Id,
            PublicUrl = right.PublicUrl,
            SignedUrl = right.SignedUrl
        }));
    }

    /// <summary>
    /// Deletes a single file
    /// </summary>
    /// <param name="id">The file id in guid</param>
    /// <param name="cancellationToken"></param>
    /// <response code="404">Error : File not found</response>
    /// <response code="204">Success : File deleted</response>
    /// <returns></returns>
    [HttpDelete("single/{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var file = await _context.FileDomains.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (file is null)
        {
            return NotFound(new { Error = "Couldn't find the file" });
        }

        file.Delete();
        _context.FileDomains.Update(file);

        await ResilientTransaction.Create(_context).ExecuteAsync(async () =>
        {
            await _context.SaveChangesAsync(cancellationToken);
            await _fileService.DeleteAsync(new SingleDeleteInput(path: file.Path, bucket: file.BucketName), cancellationToken);
        });

        return NoContent();
    }
}