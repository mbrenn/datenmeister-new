using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Runtime.Exceptions
{
    /// <summary>
    /// Defines the exception that is used within DatenMeister containing and object with
    /// an exception object being a Mof Object
    /// </summary>
    public class DatenMeisterRawException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DatenMeisterRawException class.
        /// </summary>
        public DatenMeisterRawException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DatenMeisterRawException class.
        /// </summary>
        /// <param name="message">Message, why check failed</param>
        public DatenMeisterRawException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DatenMeisterRawException class.
        /// </summary>
        /// <param name="message">Message of check</param>
        /// <param name="inner">Inner exception</param>
        public DatenMeisterRawException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}