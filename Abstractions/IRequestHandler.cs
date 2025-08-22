namespace CQR.Abstractions;

/// <summary>
/// Defines a handler for requests that return a response.
/// Implementations should contain the business logic to process the <typeparamref name="TRequest"/>
/// and produce a <typeparamref name="TResponse"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled, implementing <see cref="IRequest{TResponse}"/>.</typeparam>
/// <typeparam name="TResponse">The type of response returned by the handler.</typeparam>
public interface IRequestHandler<in TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    /// <summary>
    /// Handles the specified request asynchronously and returns a response.
    /// </summary>
    /// <param name="request">The request instance to handle.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a result of type <typeparamref name="TResponse"/>.</returns>
    Task<TResponse> HandleRequestAsync(TRequest request);
}

/// <summary>
/// Defines a handler for requests that do not return a response (fire-and-forget).
/// Implementations should contain the business logic to process the <typeparamref name="TRequest"/>.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled, implementing <see cref="IRequest"/>.</typeparam>
public interface IRequestHandler<in TRequest>
    where TRequest : IRequest
{
    /// <summary>
    /// Handles the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request instance to handle.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task HandleRequestAsync(TRequest request);
}