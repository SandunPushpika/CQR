using CQR.Abstractions;
using CQR.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CQR.Extensions;

/// <summary>
/// Registers all the required services
/// </summary>
public static class CqrExtension
{
    /// <summary>
    /// Used to configure and Register CQR services
    /// </summary>
    /// <param name="serviceCollection">Default DI registry</param>
    /// <param name="configuration">CqrConfiguration with all assemblies</param>
    /// <returns>IServiceColleciton</returns>
    public static IServiceCollection AddCQR(this IServiceCollection serviceCollection, Action<CqrConfiguration> configuration)
    {
        var cqrConfig = new CqrConfiguration();
        configuration.Invoke(cqrConfig);
        
        cqrConfig.LoadHandlers(serviceCollection);
        
        serviceCollection.AddSingleton(cqrConfig);
        serviceCollection.AddSingleton<RequestHandlerWrapper>();
        serviceCollection.AddScoped<ICqr, Cqr>();
        
        return serviceCollection;
    }
}