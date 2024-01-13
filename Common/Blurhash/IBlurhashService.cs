using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace Filio.Common.Blurhash;

internal interface IBlurhashService
{
    string Encode(Image<Rgb24> image, int componentsX, int componentsY);
    Image<Rgb24> Decode(string blurhash, int outputWith, int outputHeight, double punch = 1.0);
}