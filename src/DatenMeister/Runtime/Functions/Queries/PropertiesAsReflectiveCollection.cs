using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    public class PropertiesAsReflectiveCollection : IReflectiveCollection
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

                    var valueAsCollection = _detailElement.get(property) as IReflectiveCollection;
                    if (valueAsCollection == null)
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
    }
}