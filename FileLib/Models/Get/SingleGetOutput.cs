namespace Filio.FileLib.Models.Get;

public class SingleGetOutput
{
    public SingleGetOutput(string publicUrl, string signedUrl)
    {
        PublicUrl = publicUrl;
        SignedUrl = signedUrl;
    }

    public string PublicUrl { get; private set; }
    public string SignedUrl { get; private set; }
}