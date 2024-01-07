using Filio.Api.Models.RestApi.Delete;
using Filio.Api.Models.RestApi.Get;
using Filio.Api.Models.RestApi.Upload;
using Microsoft.AspNetCore.Mvc;

namespace Filio.Api.Controllers;

public partial class FileController
{
    /// <summary>
    /// Uploads a single file 
    /// </summary>
    /// <param name="request"></param>
    /// <response code="400">Error:Wrong request or invalid data</response>
    /// <response code="404">Error:Usually on wrong bucket name</response>
    /// <response code="200">Success/response>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpPost("single")]
    public async Task<IActionResult> Upload([FromForm] SingleUploadRequest request)
    {
        using var fileStream = request.File.OpenReadStream();

        var result = await _fileService.UploadAsync(fileStream, request.BucketName, request.Path);

        return result.IsSuccess ? Ok(new SingleUploadResponse { PublicUrl = result.Value.PublicUrl, SignedUrl = result.Value.SignedUrl })
                                : result.Error!.StatusCode switch
                                {
                                    System.Net.HttpStatusCode.NotFound => NotFound(new { result.Error.Message }),
                                    System.Net.HttpStatusCode.BadRequest => BadRequest(new { result.Error.Message }),
                                    _ => Problem(detail: result.Error.Message, statusCode: (int)result.Error.StatusCode)
                                };
    }

    /// <summary>
    /// Deletes a single file
    /// </summary>
    /// <param name="request"></param>
    /// <response code="204">Success : File deleted</response>
    /// <returns></returns>
    [HttpDelete("single")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromBody] SingleDeleteRequest request)
    {
        await _fileService.DeleteAsync(request.BucketName, request.Path);

        return NoContent();
    }

    /// <summary>
    /// Returns the file url 
    /// </summary>
    /// <param name="bucket"></param>
    /// <param name="filePath"></param>
    /// <response code="200">Success : File urls</response>
    /// <returns></returns>
    [HttpGet("single/{bucket}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get([FromRoute] string bucket, [FromQuery] string filePath)
    {
        return Ok(new SingleGetResponse { SignedUrl = _fileService.GetSignedUrl(bucket, filePath) });
    }
}