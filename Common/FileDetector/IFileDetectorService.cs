
namespace Filio.Common.FileDetector;

/// <summary>
/// Provides functions to detects file type
/// </summary>
public interface IFileDetectorService
{
    /// <summary>
    /// Returns the file type via given stream
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    FileType GetType(Stream stream);
}