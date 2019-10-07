using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Functions.Queries
{
    /// <summary>
    /// Returns the properties of the given value as a reflective collection.
    /// Only the values are returned which are inside a reflective collection, so properties with
    /// multiplicity of one are not enumerated.
    /// </summary>
    public class PropertiesAsReflectiveCollection : IReflectiveCollection, IHasExtent
    {
        private readonly IObject _detailElement;

        /// <summary>
        /// Defines the names of the properties to be parsed
        /// </summary>
        private readonly ICollection<string> _propertyNames;

        public PropertiesAsReflectiveCollection(IObject detailElement)
        {
            _detailElement = detailElement;
        }

        public PropertiesAsReflectiveCollection(IObject detailElement, string propertyName) : this(detailElement)
        {
            _propertyNames = new[] {propertyName};
        }

        public PropertiesAsReflectiveCollection(IObject detailElement, ICollection<string> propertyNames) : this(detailElement)
        {
            _propertyNames = propertyNames;
        }

        public IEnumerator<object> GetEnumerator()
        {
            if (_detailElement is IObjectAllProperties objectWithProperties)
            {
                var propertyNames = _propertyNames ?? objectWithProperties.getPropertiesBeingSet();

                foreach (var property in propertyNames
                    .Where(property => _detailElement.isSet(property)))
                {
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

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool add(object value) => throw new System.NotImplementedException();

        public bool addAll(IReflectiveSequence value) => throw new System.NotImplementedException();

        public void clear()
        {
            throw new System.NotImplementedException();
        }

        public bool remove(object value) => throw new System.NotImplementedException();

        public int size() => this.Count();

        /// <summary>
        /// Gets the extent associated to the parent extent
        /// </summary>
        public IExtent Extent => (_detailElement as IHasExtent)?.Extent;
    }
}