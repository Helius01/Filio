namespace Filio.FileLib.Models.Upload;

public class SingleUploadOutput
{
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="publicUrl">public url without ACL for public buckets</param>
    /// <param name="signedUrl">signed url with ACL for private buckets</param>
    public SingleUploadOutput(string publicUrl, string signedUrl)
    {
        PublicUrl = publicUrl;
        SignedUrl = signedUrl;
    }

    /// <summary>
    /// The file public url
    /// </summary>
    /// <value></value>
    public string PublicUrl { get; private set; } = null!;

    /// <summary>
    /// The file signed url
    /// </summary>
    /// <value></value>
    public string SignedUrl { get; private set; } = null!;
}
