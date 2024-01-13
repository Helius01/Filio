using Filio.Common.Blurhash;
using Filio.Common.Registry;
using Microsoft.Extensions.DependencyInjection;

namespace Filio.Common.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Gets the services and pass it to the CommonRegistry.
    /// The function doesn't register all services. u have to use extensions on 
    /// <see cref="CommonRegistry"/>
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static CommonRegistry AddCommon(this IServiceCollection services)
    {
        //Internal registration
        services = services.AddScoped<IBlurhashService, BlurhashService>();
        return new CommonRegistry(services);
    }
}