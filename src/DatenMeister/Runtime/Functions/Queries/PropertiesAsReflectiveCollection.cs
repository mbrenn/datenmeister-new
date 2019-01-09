using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class PropertiesAsReflectiveCollection : IReflectiveCollection, IHasExtent
    {
        private readonly IObject _detailElement;

        public PropertiesAsReflectiveCollection(IObject detailElement)
        {
            _detailElement = detailElement;
        }

        public IEnumerator<object> GetEnumerator()
        {
            if (_detailElement is IObjectAllProperties objectWithProperties)
            {
                foreach (var property in objectWithProperties.getPropertiesBeingSet())
                {
                    if (!_detailElement.isSet(property))
                    {
                        continue;
                    }

                    if (!(_detailElement.get(property) 
                        is IReflectiveCollection valueAsCollection))
                    {
                        continue;
                    }

                    foreach (var child in valueAsCollection)
                    {
                        yield return child;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool add(object value)
        {
            throw new System.NotImplementedException();
        }

        public bool addAll(IReflectiveSequence value)
        {
            throw new System.NotImplementedException();
        }

        public void clear()
        {
            throw new System.NotImplementedException();
        }

        public bool remove(object value)
        {
            throw new System.NotImplementedException();
        }

        public int size()
        {
            return this.Count();
        }

        /// <summary>
        /// Gets the extent associated to the parent extent
        /// </summary>
        public IExtent Extent => (_detailElement as IHasExtent)?.Extent;
    }
}