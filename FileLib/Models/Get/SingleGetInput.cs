namespace Filio.FileLib.Models.Get;

public class SingleGetInput
{
    public SingleGetInput(string bucket, string path)
    {
        Bucket = bucket;
        Path = path;
    }

    public string Bucket { get; private set; } = null!;
    public string Path { get; private set; } = null!;
}