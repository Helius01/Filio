namespace Filio.Common.ImageLib;

/// <summary>
/// Provides many functions to play with images
/// </summary>
public interface IImageLibService
{
    /// <summary>
    /// Generates a blurhash from an image
    /// </summary>
    /// <param name="imageStream">The image stream</param>
    /// <param name="componentsX">X component</param>
    /// <param name="componentsY">Y component</param>
    /// <returns></returns>
    Task<string> GenerateBlurhashAsync(Stream imageStream, int componentsX = 6, int componentsY = 6);

    /// <summary>
    /// The function try to generate blurhash else returns false
    /// </summary>
    /// <param name="imageStream"></param>
    /// <param name="componentsX"></param>
    /// <param name="componentsY"></param>
    /// <returns></returns>
    bool TryGenerateBlurhash(Stream imageStream, out string? blurhash, int componentsX = 6, int componentsY = 6);

    /// <summary>
    /// Resizes an image and returns the new stream
    /// </summary>
    /// <param name="imageStream"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="quality"></param>
    /// <param name="removeMetaData"></param>
    /// <returns></returns>
    Task<Stream> ResizeAsync(Stream imageStream, int? width = null, int? height = null, int quality = 100, bool removeMetaData = true);
}