namespace FileLib;

public interface IFileService
{
    Task UploadAsync(Stream fileStream, string bucket, string path);

    Task DeleteAsync(string bucket, string path);

    string GetPublicUrl(string bucket, string path);
}
