namespace Filio.Api.Abstractions;

/// <summary>
/// The class has same behavior with MemoryStream but it needs to dispose manually 
/// </summary>
/// <remarks>
/// Please make sure to call <see cref="ManualMemoryStream.ManualDispose" /> when you done.
/// </remarks>
public sealed class ManualMemoryStream : MemoryStream
{
    /// <summary>
    /// Default dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        Flush();
        Seek(0, SeekOrigin.Begin);
    }

    /// <summary>
    /// Manual dispose
    /// </summary>
    public void ManualDispose()
    {
        base.Dispose(true);
    }
}