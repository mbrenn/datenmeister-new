using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Runtime.Exceptions
{
    /// <summary>
    /// Defines the exception that is used within DatenMeister containing and object with
    /// an exception object being a Mof Object
    /// </summary>
    public class DatenMeisterException : DatenMeisterRawException
    {
        /// <summary>
        /// Gets the Mof Object that is characterizing the exception
        /// </summary>
        public IObject MofObject { get; }

        /// <summary>
        /// Initializes a new instance of the DatenMeisterException class.
        /// </summary>
        public DatenMeisterException(IObject mofObject)
        {
            MofObject = mofObject;
        }

        /// <summary>
        /// Initializes a new instance of the DatenMeisterException class.
        /// </summary>
        /// <param name="message">Message, why check failed</param>
        /// <param name="mofObject">The mofObject being used</param>
        public DatenMeisterException(string message, IObject mofObject) : base(message)
        {
            MofObject = mofObject;
        }

        /// <summary>
        /// Initializes a new instance of the DatenMeisterException class.
        /// </summary>
        /// <param name="message">Message of check</param>
        /// <param name="mofObject">The mofObject being used</param>
        /// <param name="inner">Inner exception</param>
        public DatenMeisterException(string message, IObject mofObject, Exception inner)
            : base(message, inner)
        {
            MofObject = mofObject;
        }

        /// <summary>
        /// Converts the element to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MofObject != null
                ? MofObject.isSet("message")
                    ? MofObject.getOrDefault<string>("message")
                    : $"{nameof(DatenMeisterException)} ({NamedElementMethods.GetName(MofObject)}"
                : base.ToString();
        }
    }
}