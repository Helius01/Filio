using Filio.Api.Data;
using Filio.Api.Models.RestApi.Get;
using Filio.Common.FileDetector;
using Filio.Common.ImageLib;
using Filio.FileLib;
using Filio.FileLib.Models.Get;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Filio.Api.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public partial class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly DataContext _context;
    private readonly IFileDetectorService _fileDetectorService;
    private readonly IImageLibService _imageLibService;
    private readonly ILogger<FileController> _logger;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="fileService"></param>
    /// <param name="imageLibService"></param>
    /// <param name="context"></param>
    /// <param name="fileDetectorService"></param>
    /// <param name="logger"></param>
    public FileController(IFileService fileService, IImageLibService imageLibService, DataContext context, IFileDetectorService fileDetectorService, ILogger<FileController> logger)
    {
        _fileService = fileService;
        _imageLibService = imageLibService;
        _context = context;
        _fileDetectorService = fileDetectorService;
        _logger = logger;
    }

    /// <summary>
    /// Returns the file url 
    /// </summary>
    /// <param name="id">File id in GUID</param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Success : SingleGetResponse</response>
    /// <response code="404">Error : File not found</response>
    /// <returns></returns>
    [HttpGet("single/{id}")]
    [ProducesResponseType(typeof(SingleGetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var file = await _context.FileDomains.Where(x => x.Id == id && !x.IsDeleted)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(cancellationToken);

        if (file is null)
        {
            _logger.LogWarning("Someone requested for a non exists file with id ={Id}", id);
            return NotFound("Couldn't find the file");
        }

        var publicUrl = _fileService.GetPublicUrl(new SingleGetInput(bucket: file.BucketName, path: file.Path));
        var signedUrl = _fileService.GetSignedUrl(new SingleGetInput(bucket: file.BucketName, path: file.Path));

        var response = new SingleGetResponse(signedUrl: signedUrl,
                                               publicUrl: publicUrl,
                                               bucket: file.BucketName,
                                               metadata: file.MetaData,
                                               imageBlurhash: file.ImageBlurhash,
                                               type: file.Type);

        return Ok(response);
    }
}