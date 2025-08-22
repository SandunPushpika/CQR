using System.Collections.Concurrent;
using System.Reflection;
using CQR.Abstractions;
using CQR.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CQR.Core;

/// <summary>
/// Provides centralized configuration for request handlers, including the registration of
/// command and query handlers into the dependency injection container and the caching of
/// handler metadata for fast runtime resolution.
/// </summary>
public class CqrConfiguration
{
    internal List<Assembly> Assemblies { get; } = new();
    
    internal ConcurrentDictionary<Type, (Type Handler, MethodInfo? Method)> _handlerCache { get; } = new();

    public void AddAssembly(Assembly assembly)
    {
        Assemblies.Add(assembly);
    }

    internal void LoadHandlers(IServiceCollection collection)
    {
        if (Assemblies.Count == 0)
            throw new NoAssembliesFoundException();

        var handlerInfo = Assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                             i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .Select(i =>
                {
                    collection.AddScoped(t);
                    return new
                    {
                        RequestType = i.GetGenericArguments()[0],
                        Handler = t,
                        Method = t.GetMethod("HandleRequestAsync")
                    };
                })
            );

        foreach (var handler in handlerInfo)
        {
            _handlerCache.TryAdd(handler.RequestType, (handler.Handler, handler.Method));
        }
        
    }
}