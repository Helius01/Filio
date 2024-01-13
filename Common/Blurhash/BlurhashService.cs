using Blurhash.ImageSharp;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Filio.Common.Blurhash;

/// <summary>
/// An abstraction on Blurhasher
/// </summary>
internal class BlurhashService : IBlurhashService
{
    /// <summary>
    /// Decodes a blurhash to image
    /// </summary>
    /// <param name="blurhash"></param>
    /// <param name="outputWith"></param>
    /// <param name="outputHeight"></param>
    /// <param name="punch"></param>
    /// <returns></returns>
    public Image<Rgb24> Decode(string blurhash, int outputWith, int outputHeight, double punch = 1)
    {
        return Blurhasher.Decode(blurhash, outputWith, outputHeight, punch);
    }

    /// <summary>
    /// Encode an image to blurhash
    /// </summary>
    /// <param name="image"></param>
    /// <param name="componentsX"></param>
    /// <param name="componentsY"></param>
    /// <returns></returns>
    public string Encode(Image<Rgb24> image, int componentsX, int componentsY)
    {
        return Blurhasher.Encode(image, componentsX, componentsY);
    }
}