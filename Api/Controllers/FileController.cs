using Amazon.S3;
using Filio.Api.Models.RestApi.Upload;
using Filio.FileLib;
using Filio.FileLib.Settings.Aws;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Filio.Api.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public partial class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    private readonly AwsSettings _settings;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileService"></param>
    /// <param name="settings"></param>
    public FileController(IFileService fileService, AwsSettings settings)
    {
        _fileService = fileService;
        _settings = settings;
    }
}