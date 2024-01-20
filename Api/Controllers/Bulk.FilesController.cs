using System.Data;
using Filio.Api.Abstractions;
using Filio.Api.Data;
using Filio.Api.Domains;
using Filio.Api.Extensions;
using Filio.Api.Models.RestApi.Get;
using Filio.Api.Models.RestApi.Upload;
using Filio.Common.ErrorHandler;
using Filio.Common.ErrorHandler.RecoverableErrors;
using Filio.Common.FileDetector;
using Filio.FileLib.Models.Get;
using Filio.FileLib.Models.Upload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                                        type: fileType.ToFileDomainType(),
                                        imageBlurhash: null);

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

    /// <summary>
    /// Returns the files urls and its data via given ids
    /// </summary>
    /// <param name="ids">A string of file guid which separated with coma</param>
    /// <returns></returns>
    /// <response code="400">Error : Invalid id | Not match files</response>
    /// <response code="200">Success : List of SingleGetResponse</response>
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(List<SingleGetResponse>), StatusCodes.Status200OK)]
    [HttpGet("bulk/{ids}")]
    public async Task<IActionResult> GetMany([FromRoute] string ids)
    {
        var separatedIds = ids.Split(",");

        var fileIds = new List<Guid>();

        foreach (var id in separatedIds)
        {
            if (Guid.TryParse(id, out var parsedGuid))
            {
                fileIds.Add(parsedGuid);
            }
            else
            {
                _logger.LogWarning("Couldn't parse the string {Id} to GUID", id);
                return BadRequest(new { Error = "Invalid id" });
            }
        }

        var files = await _context.FileDomains
                    .Where(x => fileIds.Contains(x.Id))
                    .AsNoTracking()
                    .ToListAsync();

        if (files.Count != fileIds.Count)
        {
            //TODO:Log
            return BadRequest(new { Error = "The count of ids doesn't match with the count of files" });
        }

        var response = files.ConvertAll(x => new SingleGetResponse(
            signedUrl: _fileService.GetSignedUrl(new SingleGetInput(x.BucketName, x.Path)),
            publicUrl: _fileService.GetPublicUrl(new SingleGetInput(x.BucketName, x.Path)),
            bucket: x.BucketName,
            imageBlurhash: x.ImageBlurhash,
            type: nameof(x.Type)
        ));

        return Ok(response);
    }
}
