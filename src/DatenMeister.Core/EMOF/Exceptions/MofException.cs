namespace DatenMeister.Core.EMOF.Exceptions;

/// <summary>
///     This exception will be thrown, when a violation of Mof is found out
/// </summary>
public class MofException : Exception
{
    public MofException()
    {
    }

    public MofException(string message) : base(message)
    {
    }

    public MofException(string message, Exception inner) : base(message, inner)
    {
    }
}