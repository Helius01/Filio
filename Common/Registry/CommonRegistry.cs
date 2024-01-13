using Microsoft.Extensions.DependencyInjection;

namespace Filio.Common.Registry;

/// <summary>
/// Registers the common services
/// </summary>
public class CommonRegistry
{
    private readonly IServiceCollection _services;
    public CommonRegistry(IServiceCollection services)
    {
        _services = services;
    }

    public IServiceCollection Services => _services;
}