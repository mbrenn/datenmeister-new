using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Modules.DataViews
{
    /// <summary>
    /// Stores the factories for the view nodes
    /// </summary>
    public class DataViewNodeFactories
    {
        private readonly List<IDataViewNodeFactory> _factories = new List<IDataViewNodeFactory>();

        /// <summary>
        /// Stores the view node factory
        /// </summary>
        public List<IDataViewNodeFactory> Factories
        {
            get
            {
                lock (_factories)
                {
                    return _factories.ToList();
                }
            }
        }

        /// <summary>
        /// Adds a new factory to the instance
        /// </summary>
        /// <param name="factory"></param>
        public void Add(IDataViewNodeFactory factory)
        {
            lock (_factories)
            {
                _factories.Add(factory);
            }
        }
    }
}