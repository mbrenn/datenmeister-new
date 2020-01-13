using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements a reflective sequence as given by the MOF specification.
    /// The sequence needs to be correlated to a Mof Object
    /// </summary>
    public class MofReflectiveSequence : IReflectiveSequence, IHasExtent
    {
        /// <summary>
        /// Gets the name of the property
        /// </summary>
        internal string PropertyName { get; }

        /// <summary>
        /// Gets the mof object being assigned to the
        /// </summary>
        internal MofObject MofObject { get; }
        
        /// <summary>
        /// Gets or sets a flag indicating whether references shall be followed or not
        /// </summary>
        public bool NoReferences { get; set; }

        public MofReflectiveSequence(MofObject mofObject, string property)
        {
            MofObject = mofObject;
            PropertyName = property;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator()
            => Enumerate().GetEnumerator();

        /// <summary>
        /// Performs an enumeration of all members of the collection
        /// </summary>
        /// <param name="noReferences">true, if UriReferences shall be resolved</param>
        /// <returns>Enumeration of collection</returns>
        internal IEnumerable<object> Enumerate(bool noReferences = false)
        {
            var result = GetPropertyAsEnumerable();
            foreach (var item in result)
            {
                yield return MofObject.ConvertToMofObject(MofObject, PropertyName, item, noReferences || NoReferences);
            }
        }

        /// <summary>
        /// Gets the given property as an enumerable
        /// </summary>
        /// <returns>Enumerable which was retrieved</returns>
        private IEnumerable<object> GetPropertyAsEnumerable()
        {
            if (MofObject.ProviderObject.IsPropertySet(PropertyName))
            {
                var value = MofObject.ProviderObject.GetProperty(PropertyName);
                
                if (value is IEnumerable<object> asEnumerable)
                {
                    return asEnumerable;
                }
                else
                {
                    return new[] {value};
                }
            }

            return Array.Empty<object>();
        }

        /// <inheritdoc />
        public bool add(object value)
        {
            var valueToBeAdded = MofExtent.ConvertForSetting(MofObject, value);
            var result = MofObject.ProviderObject.AddToProperty(PropertyName, valueToBeAdded);

            MofObject.SetContainer(MofObject.ProviderObject, value, valueToBeAdded);

            UpdateContent();

            return result;
        }

        /// <inheritdoc />
        public bool addAll(IReflectiveSequence value)
        {
            bool? result = null;

            foreach (var element in value)
            {
                if (result == null)
                {
                    result = add(element);
                }
                else
                {
                    result |= add(element);
                }
            }

            return result == true;
        }

        /// <inheritdoc />
        public void clear()
        {
            // Performs now the final deletion
            MofObject.ProviderObject.EmptyListForProperty(PropertyName);
            UpdateContent();
        }

        /// <inheritdoc />
        public bool remove(object value)
        {
            bool result;
            if (value is MofObject valueAsMofObject)
            {
                var asProviderObject = valueAsMofObject.ProviderObject;
                result = MofObject.ProviderObject.RemoveFromProperty(PropertyName, asProviderObject);
            }
            else
            {
                result = MofObject.ProviderObject.RemoveFromProperty(PropertyName, value);
            }

            UpdateContent();
            return result;
        }

        /// <inheritdoc />
        public int size()
            => GetPropertyAsEnumerable().Count();

        /// <inheritdoc />
        public void add(int index, object value)
        {
            var valueToBeAdded = MofExtent.ConvertForSetting(MofObject, value);
            MofObject.ProviderObject.AddToProperty(PropertyName, valueToBeAdded, index);

            UpdateContent();
        }

        /// <inheritdoc />
        public object get(int index)
        {
            var providerObject = GetPropertyAsEnumerable().ElementAt(index);
            return MofObject.ConvertToMofObject(MofObject, PropertyName, providerObject);
        }

        /// <inheritdoc />
        public void remove(int index)
        {
            var foundvalue = ((IEnumerable<object>) MofObject.ProviderObject.GetProperty(PropertyName)).ElementAt(index);
            if (foundvalue != null)
            {
                MofObject.ProviderObject.RemoveFromProperty(
                    PropertyName,
                    foundvalue);

                UpdateContent();
            }
        }

        /// <inheritdoc />
        public object set(int index, object value)
        {
            var valueToBeRemoved = GetPropertyAsEnumerable().ElementAt(index);
            MofObject.ProviderObject.RemoveFromProperty(PropertyName, valueToBeRemoved);
            add(index, value);

            var result = MofObject.ConvertToMofObject(MofObject, PropertyName, valueToBeRemoved);

            MofObject.Extent?.ChangeEventManager?.SendChangeEvent(MofObject);
            return result;
        }

        /// <inheritdoc />
        public IExtent Extent => MofObject.Extent ?? MofObject.ReferencedExtent;

        /// <summary>
        /// Updates the content
        /// </summary>
        public void UpdateContent()
        {
            MofObject.Extent?.ChangeEventManager?.SendChangeEvent(MofObject);
            MofObject.Extent?.SignalUpdateOfContent();
        }
    }
}