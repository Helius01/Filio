using Filio.Api.Abstractions;

namespace Filio.Api.Extensions;

/// <summary>
/// Provides some functions on ManualMemoryStream object
/// </summary>
public static class ManualMemoryStreamExtensions
{
    /// <summary>
    /// Clones the stream
    /// </summary>
    /// <param name="ms"></param>
    /// <param name="resetPosition"></param>
    /// <returns></returns>
    public static ManualMemoryStream Clone(this Stream ms, bool resetPosition = false)
    {
        var pos = ms.Position;
        if (resetPosition)
            pos = 0;
        var ms2 = new ManualMemoryStream();
        ms.CopyTo(ms2);
        ms2.Position = pos;
        return ms2;
    }
}