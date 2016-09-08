using System.Collections.Generic;

namespace DatenMeister.Core.Filler
{
    /// <summary>
    /// This is just a dummy filler, which can be used to create the filledtype on demand
    /// by defining the type via the Create method of the datalayer
    /// </summary>
    /// <typeparam name="T">Type to be filled</typeparam>
    public class DummyFiller<T> : IFiller<T>
    {
        public void Fill(IEnumerable<object> collection, T filled)
        {
            // Is doing nothing
        }
    }
}