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
public partial class FilesController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly DataContext _context;
    private readonly IFileDetectorService _fileDetectorService;
    private readonly IImageLibService _imageLibService;
    private readonly ILogger<FilesController> _logger;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="fileService"></param>
    /// <param name="imageLibService"></param>
    /// <param name="context"></param>
    /// <param name="fileDetectorService"></param>
    /// <param name="logger"></param>
    public FilesController(IFileService fileService, IImageLibService imageLibService, DataContext context, IFileDetectorService fileDetectorService, ILogger<FilesController> logger)
    {
        _fileService = fileService;
        _imageLibService = imageLibService;
        _context = context;
        _fileDetectorService = fileDetectorService;
        _logger = logger;
    }
}