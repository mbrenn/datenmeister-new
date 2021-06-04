using System;

namespace DatenMeister.Core.Runtime.Proxies.ReadOnly
{
    /// <summary>
    /// This exception is thrown, when write access to a read-only element was triggered
    /// </summary>
    public class ReadOnlyAccessException : Exception
    {
        public ReadOnlyAccessException()
        {
        }

        public ReadOnlyAccessException(string message) : base(message)
        {
        }

        public ReadOnlyAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}