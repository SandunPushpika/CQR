namespace CQR.Exceptions;

/// <summary>
/// Throws when there are no assemblies found.
/// </summary>
public class NoAssembliesFoundException : Exception
{
    public NoAssembliesFoundException() : base("No assemblies were found.")
    {
        
    }
    
    public NoAssembliesFoundException(string message) : base(message)
    {
        
    }
}