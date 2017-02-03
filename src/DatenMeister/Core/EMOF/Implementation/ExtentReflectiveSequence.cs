using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Provider;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Implements the reflective sequence for 
    /// </summary>
    public class ExtentReflectiveSequence : IReflectiveSequence
    {
        private readonly MofExtent _extent;

        /// <summary>
        /// Initializes a new instance of the ExtentReflectiveSequence class
        /// </summary>
        /// <param name="extent"></param>
        public ExtentReflectiveSequence(MofExtent extent)
        {
            _extent = extent;
        }

        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator()
        {
            foreach (var element in _extent.Provider.GetRootObjects())
            {
                var resultElement = new MofElement(element,_extent);
                yield return resultElement;
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc />
        public bool add(object value)
        {
           return AddInternal(value, -1);
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
            _extent.Provider.DeleteAllElements();
        }

        /// <inheritdoc />
        public bool remove(object value)
        {
            var valueAsObject = value as MofObject;
            if (valueAsObject != null)
            {
                return _extent.Provider.DeleteElement(valueAsObject.ProviderObject.Id);
                
            }

            throw new NotImplementedException("Only the deletion of values are supported");
        }

        /// <inheritdoc />
        public int size()
        {
            return _extent.Provider.GetRootObjects().Count();
        }

        /// <inheritdoc />
        public void add(int index, object value)
        {
            AddInternal(value, index);
        }

        /// <summary>
        /// Adds the object internally
        /// </summary>
        /// <param name="value">Value to be added</param>
        /// <param name="index">Index of the object to be added</param>
        /// <returns>true, if object could be added</returns>
        private bool AddInternal(object value, int index)
        {
            var valueAsObject = value as MofObject;
            if (valueAsObject != null)
            {
                if (valueAsObject.Extent == _extent || valueAsObject.Extent == null)
                {
                    _extent.Provider.AddElement(valueAsObject.ProviderObject, index);
                    valueAsObject.Extent = _extent;
                    return true;
                }

                throw new NotImplementedException("Only objects from the extent are currently supported");
            }

            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                throw new InvalidOperationException(
                    $"An instance of a primitive type may not be added to the extent root elements: {value}");
            }

            _extent.Provider.AddElement((IProviderObject)MofExtent.ConvertForSetting(_extent, value), index);
            return true;
        }

        /// <inheritdoc />
        public object get(int index)
        {
            return _extent.Provider.GetRootObjects().ElementAt(index);
        }

        /// <inheritdoc />
        public void remove(int index)
        {
            remove(_extent.Provider.GetRootObjects().ElementAt(index));
        }

        /// <inheritdoc />
        public object set(int index, object value)
        {
            var size = this.size();
            if (index < 0 || index >= size)
            {
                throw new ArgumentException("Object could not be added due to wrong index: " + index);
            }

            var result = get(index);
            remove(index);
            set(index, value);

            return result;
        }
    }
}