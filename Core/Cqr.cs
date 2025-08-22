using CQR.Abstractions;

namespace CQR.Core;

internal class Cqr : ICqr
{
    private RequestHandlerWrapper _requestHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="Cqr"/> class with the specified
    /// <see cref="RequestHandlerWrapper"/>, which is responsible for resolving and invoking handlers.
    /// </summary>
    /// <param name="requestHandler">The request handler wrapper used to locate and execute handlers.</param>
    public Cqr(RequestHandlerWrapper requestHandler)
    {
        _requestHandler = requestHandler;
    }
    
    public async Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request)
    {
        var response = await _requestHandler.HandleRequest(request);
        return response;
    }

    public async Task SendRequest(IRequest request)
    {
        await _requestHandler.HandleRequest(request);
    }
}