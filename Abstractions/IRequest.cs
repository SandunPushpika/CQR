namespace CQR.Abstractions;

/// <summary>
/// Represents a request that does not return a response.
/// Marker interface used to identify commands or queries handled by the CQRS pipeline.
/// </summary>
public interface IRequest
{
    
}

/// <summary>
/// Represents a request that expects a response of type <typeparamref name="TResponse"/>.
/// Marker interface used to identify queries or commands handled by the CQRS pipeline
/// that return a result.
/// </summary>
/// <typeparam name="TResponse">The type of response expected from the request.</typeparam>
public interface IRequest<TResponse>
{
    
}