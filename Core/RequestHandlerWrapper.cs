using System.Reflection;
using CQR.Abstractions;
using CQR.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CQR.Core;

/// <summary>
/// Provides a uniform abstraction over request handlers, allowing requests to be
/// executed without knowing the concrete handler type at compile time.
/// 
/// This wrapper is responsible for locating the appropriate <c>IRequestHandler</c>
/// implementation, invoking its <c>HandleRequestAsync</c> method, and returning
/// the result in a type-safe manner.
/// 
/// Used internally by the mediator pipeline to decouple request dispatching
/// from concrete handler implementations.
/// </summary>
public class RequestHandlerWrapper
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CqrConfiguration _configuration;

    public RequestHandlerWrapper(IServiceProvider serviceProvider, CqrConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }
    
    internal async Task<TResponse> HandleRequest<TResponse>(IRequest<TResponse> request)
    {
        var requestType = request.GetType();

        if (!_configuration._handlerCache.TryGetValue(requestType, out var handlerType))
            throw new NoHandlerFoundException(requestType);

        var scope = _serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        var handler = scopedProvider.GetRequiredService(handlerType.Handler);
        
        try
        {
            return await (Task<TResponse>)handlerType.Method?.Invoke(handler, new[] { request });
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException ?? e;
        }
    }
    
    internal async Task HandleRequest(IRequest request)
    {
        var requestType = request.GetType();
        
        if (!_configuration._handlerCache.TryGetValue(requestType, out var handlerType))
            throw new NoHandlerFoundException(requestType);
        
        var scope = _serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;
        var handler = scopedProvider.GetRequiredService(handlerType.Handler);
        
        if(handler == null)
            throw new InvalidOperationException($"Handler {handlerType.Handler.Name} not registered in DI");

        try
        {
            await (Task)handlerType.Method?.Invoke(handler, new[] { request });
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException ?? e;
        }
    }
}