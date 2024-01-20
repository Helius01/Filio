namespace Filio.FileLib.Models.Upload;

public class SingleUploadInput
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <remarks>
    /// <para>Other constructors</para>
    /// <seealso cref="SingleUploadInput.SingleUploadInput(Stream, string, string, Dictionary{string, string})"/>
    /// </remarks>
    /// <param name="stream">file stream</param>
    /// <param name="path">file path</param>
    /// <param name="bucket">bucket name</param>

    public SingleUploadInput(Stream stream, string path, string bucket)
    {
        Stream = stream;
        Path = path;
        Bucket = bucket;
    }
    

    /// <summary>
    /// File stream
    /// </summary>
    /// <value></value>
    public Stream Stream { get; private set; } = null!;

    /// <summary>
    /// File path
    /// </summary>
    /// <value></value>
    public string Path { get; private set; } = null!;

    /// <summary>
    /// Bucket name
    /// </summary>
    /// <value></value>
    public string Bucket { get; private set; } = null!;
}