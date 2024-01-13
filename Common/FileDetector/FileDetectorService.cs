using MimeDetective;
using MimeDetective.Definitions;

namespace Filio.Common.FileDetector;

/// <summary>
/// The implementation of IFileDetectorService 
/// </summary>
internal class FileDetectorService : IFileDetectorService
{
    private static readonly ContentInspector inspector = new ContentInspectorBuilder()
    {
        Definitions = new ExhaustiveBuilder()
        {
            UsageType = MimeDetective.Definitions.Licensing.UsageType.PersonalNonCommercial
        }.Build()
    }.Build();

    ///<inheritdoc />
    public FileType GetType(Stream stream)
    {
        var inspectResult = inspector.Inspect(stream, ResetPosition: true);

        if (!inspectResult.Any())
            return FileType.None;

        var mimeType = inspectResult.OrderByDescending(x => x.Points)?.First().Definition?.File?.MimeType;

        return mimeType switch
        {
            string type when type.Contains("image", StringComparison.InvariantCultureIgnoreCase) => FileType.Image,
            string type when type.Contains("video", StringComparison.InvariantCultureIgnoreCase) => FileType.Video,
            string type when type.Contains("audio", StringComparison.InvariantCultureIgnoreCase) => FileType.Audio,
            string type when type.Contains("text", StringComparison.InvariantCultureIgnoreCase) => FileType.Text,
            string type when type.Contains("pdf", StringComparison.InvariantCultureIgnoreCase) => FileType.PDF,
            string type when type.Contains("log", StringComparison.InvariantCultureIgnoreCase) => FileType.Log,
            _ => FileType.None,
        };
    }
}