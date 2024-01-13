using Filio.Common.FileDetector;
using Filio.Common.ImageLib;
using Filio.Common.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Filio.Common.Extensions;

/// <summary>
/// Provides functions to register Common services
/// </summary>
public static class CommonRegistryExtensions
{
    /// <summary>
    /// Registers FileDetector
    /// </summary>
    /// <param name="registry"></param>
    /// <returns></returns>
    public static CommonRegistry AddFileDetector(this CommonRegistry registry)
    {
        registry.Services.AddSingleton<IFileDetectorService, FileDetectorService>();

        return registry;
    }

    /// <summary>
    /// Registers ImageLib
    /// </summary>
    /// <param name="registry"></param>
    /// <returns></returns>
    public static CommonRegistry AddImageLib(this CommonRegistry registry)
    {
        registry.Services.AddScoped<IImageLibService, ImageLibService>();

        return registry;
    }

    /// <summary>
    /// Final function on Registering which returns the services
    /// </summary>
    /// <param name="registry"></param>
    /// <returns></returns>
    public static IServiceCollection Register(this CommonRegistry registry)
    {
        return registry.Services;
    }
}