using Filio.Api.Domains;
using Filio.Common.FileDetector;

namespace Filio.Api.Extensions;

/// <summary>
/// Provides functions on FileType
/// </summary>
public static class FileTypeExtensions
{
    /// <summary>
    /// Converts FileType to FileDomainType
    /// </summary>
    /// <param name="fileType"></param>
    /// <returns></returns>
    public static FileDomainType ToFileDomainType(this FileType fileType)
    {
        return fileType switch
        {
            FileType.Image => FileDomainType.Image,
            FileType.Archive => FileDomainType.Archive,
            FileType.Audio => FileDomainType.Audio,
            FileType.Log => FileDomainType.Log,
            FileType.PDF => FileDomainType.PDF,
            FileType.Text => FileDomainType.Text,
            _ => throw new NotImplementedException($"Can't convert FileType {fileType} To FileDomainType")
        };
    }
}