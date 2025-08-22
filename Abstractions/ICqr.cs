namespace CQR.Abstractions;

/// <summary>
/// Provides a high-level API for sending commands and queries through the CQRS.
/// This interface acts as the main entry point for dispatching requests to their corresponding handlers.
/// </summary>
public interface ICqr
{
    /// <summary>
    /// Sends a request that expects a response and returns the result of the handler execution.
    /// </summary>
    /// <typeparam name="TResponse">The type of response expected from the handler.</typeparam>
    /// <param name="request">The request to be processed.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TResponse"/>.</returns>
    Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request);
    
    /// <summary>
    /// Sends a request that does not expect a response.
    /// </summary>
    /// <param name="request">The request to be processed.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendRequest(IRequest request);
}