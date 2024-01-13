namespace Filio.FileLib.Models.Delete;

public class SingleDeleteInput
{
    /// <summary>
    /// Default constructor 
    /// </summary>
    /// <param name="path">The file path </param>
    /// <param name="bucket">The bucket name </param>
    public SingleDeleteInput(string path, string bucket)
    {
        Path = path;
        Bucket = bucket;
    }

    /// <summary>
    /// File id
    /// </summary>
    /// <value></value>
    public string Path { get; private set; }
    public string Bucket { get; private set; }
}
