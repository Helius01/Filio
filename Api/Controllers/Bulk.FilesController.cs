using System.Data;
using Filio.Api.Abstractions;
using Filio.Api.Data;
using Filio.Api.Domains;
using Filio.Api.Extensions;
using Filio.Api.Models.RestApi.Upload;
using Filio.Common.ErrorHandler;
using Filio.Common.ErrorHandler.RecoverableErrors;
using Filio.Common.FileDetector;
using Filio.FileLib.Models.Upload;
using Microsoft.AspNetCore.Mvc;

namespace Filio.Api.Controllers;

public partial class FilesController
{
    /// <summary>
    /// Bulk upload files
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="400">Error : File type not supported</response>
    /// <response code="200">Success : BulkUploadResponse</response>
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType((typeof(BulkUploadResponse)), StatusCodes.Status200OK)]
    [HttpPost("bulk")]
    public async Task<IActionResult> BulkUpload([FromForm] BulkUploadRequest request)
    {
        var uploadable = new Dictionary<FileDomain, ManualMemoryStream>();

        foreach (var file in request.Files)
        {
            var fileStream = file.OpenReadStream();

            var fileType = _fileDetectorService.GetType(fileStream);

            if (fileType == FileType.None)
            {
                _logger.LogWarning("Detected an unsupported file type = {Type}", fileType);
                return BadRequest(new { Error = "File type not supported" });
            }

            var newFile = new FileDomain(bucketName: request.BucketName,
                                        sizeInByte: file.Length,
                                        extension: Path.GetExtension(file.FileName),
                                        originalName: file.FileName,
                                        type: file.ContentType);

            if (fileType == FileType.Image)
            {
                var imageBlurhash = await _imageLibService.GenerateBlurhashAsync(fileStream);
                newFile.UpdateBlurhash(imageBlurhash);
            }

            uploadable.Add(newFile, fileStream.Clone());
            _context.FileDomains.Add(newFile);
        }

        List<Either<HttpError, SingleUploadOutput>> serviceResponse = new();

        await ResilientTransaction.Create(_context).ExecuteAsync(async () =>
        {
            await _context.SaveChangesAsync();

            var uploadTasks = uploadable.Select(async x =>
            {
                return await _fileService.UploadAsync(new SingleUploadInput(x.Value, x.Key.Path, bucket: request.BucketName));
            });

            await Task.WhenAll(uploadTasks);

            serviceResponse = uploadTasks.Select(x => x.GetAwaiter().GetResult()).ToList();

            if (serviceResponse.Any(x => x.IsLeft))
            {
                _logger.LogError("Couldn't upload some files with message = {Message}",
                     serviceResponse.First(x => x.IsLeft).LeftOrDefault()!.Message);

                throw new Exception("Something went wrong");
            }
        });

        uploadable.Values.ToList().ForEach(x => x.ManualDispose());

        _logger.LogInformation("Uploaded {Count} file successfully", uploadable.Count);

        return Ok(new BulkUploadResponse
        {
            FileIds = uploadable.Keys.Select(x => x.Id).ToList()
        });
    }
}
