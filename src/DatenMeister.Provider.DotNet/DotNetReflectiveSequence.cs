using System.Collections;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Provider.DotNet
{
    public class DotNetReflectiveSequence<T>
    {
        /// <summary>
        /// Stores the list being used in the Reflective sequence
        /// </summary>
        private IList<T> list;
    }
}