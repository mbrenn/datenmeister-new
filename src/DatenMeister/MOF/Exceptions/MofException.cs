using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.MOF.Exceptions
{
    /// <summary>
    /// This exception will be thrown, when a violation of Mof is found out
    /// </summary>
    public class MofException : Exception
    {
        public MofException() { }
        public MofException(string message) : base(message) { }
        public MofException(string message, Exception inner) : base(message, inner) { }
    }
}
