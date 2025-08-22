namespace CQR.Exceptions;

/// <summary>
/// Throw when there is no such handler.
/// </summary>
public class NoHandlerFoundException : Exception
{
    public object Request { get; }

    public NoHandlerFoundException(object request)
        : base($"No handler found for this request({request.GetType().Name})")
    {
        Request = request;
    }
}