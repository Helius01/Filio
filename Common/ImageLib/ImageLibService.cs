using System.ComponentModel;
using Filio.Common.Blurhash;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Filio.Common.ImageLib;

internal class ImageLibService : IImageLibService
{
    private readonly IBlurhashService _blurhashService;

    public ImageLibService(IBlurhashService blurhashService)
    {
        _blurhashService = blurhashService;
    }

    ///<inheritdoc />
    public async Task<string> GenerateBlurhashAsync(Stream imageStream, int componentsX = 6, int componentsY = 6)
    {
        imageStream.Position = 0;
        using var image = await Image.LoadAsync<Rgb24>(imageStream);

        return _blurhashService.Encode(image, componentsX, componentsY);
    }

    ///<inheritdoc />
    public async Task<Stream> ResizeAsync(Stream imageStream, int? width = null, int? height = null, int quality = 100, bool removeMetaData = true)
    {
        if (height is null && width is null)
            throw new ArgumentException("Width and height cannot be null at the same time");

        imageStream.Position = 0;

        using var image = await Image.LoadAsync(imageStream);

        if (removeMetaData)
            image.Metadata.ExifProfile = null;

        var resizeOptions = new ResizeOptions
        {
            Compand = false,
            Mode = ResizeMode.Crop,
            Position = AnchorPositionMode.Center,
            Size = new Size(width ?? 0, height ?? 0)
        };

        image.Mutate(x => x.Resize(resizeOptions));

        using var newStream = new MemoryStream();

        image.Save(newStream, new JpegEncoder
        {
            Quality = quality,
            ColorType = JpegEncodingColor.YCbCrRatio444
        });

        return newStream;
    }

    ///<inheritdoc/>
    public bool TryGenerateBlurhash(Stream imageStream, out string? blurhash, int componentsX = 6, int componentsY = 6)
    {
        blurhash = null;
        imageStream.Position = 0;
        try
        {
            using var image = Image.LoadAsync<Rgb24>(imageStream).GetAwaiter().GetResult();
            blurhash = _blurhashService.Encode(image, componentsX, componentsY);
        }
        catch
        {
            //TODO:Capture
            return false;
        }

        return true;
    }
}