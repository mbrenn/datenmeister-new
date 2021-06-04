using System.Collections.Generic;
using System.Linq;

namespace DatenMeister.Core.Modules.DataViews
{
    /// <summary>
    /// Stores the factories for the view nodes
    /// </summary>
    public class DataViewNodeFactories
    {
        private readonly List<IDataViewNodeEvaluation> _factories = new List<IDataViewNodeEvaluation>();

        /// <summary>
        /// Stores the view node factory
        /// </summary>
        public List<IDataViewNodeEvaluation> Evaluations
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
        /// <param name="evaluation"></param>
        public void Add(IDataViewNodeEvaluation evaluation)
        {
            lock (_factories)
            {
                _factories.Add(evaluation);
            }
        }
    }
}