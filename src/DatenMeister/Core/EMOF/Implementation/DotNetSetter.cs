using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Runtime;

namespace DatenMeister.Core.EMOF.Implementation
{
    /// <summary>
    /// Support class, that is capable to set properties by using DotNet Objects
    /// </summary>
    public class DotNetSetter
    {
        /// <summary>
        /// Stores the MOF Factory
        /// </summary>
        private readonly MofFactory _factory;

        /// <summary>
        /// Stores a list of already visited elements, so a recursion is avoided
        /// </summary>
        private readonly HashSet<object> _visitedElements = new HashSet<object>();

        /// <summary>
        /// Initializes a new instance of the DotNetSetter class
        /// </summary>
        /// <param name="factory">Factory to be set</param>
        public DotNetSetter(MofFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Sets the given object into the MofObject. 
        /// </summary>
        /// <param name="receiver">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        public static object Convert(MofExtent receiver, object value)
        {
            var factory = new MofFactory(receiver);
            return Convert(value, factory);
        }

        /// <summary>
        /// Sets the given object into the MofObject. 
        /// </summary>
        /// <param name="mofObject">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        public static object Convert(MofObject mofObject, object value)
        {
            var factory = new MofFactory(mofObject);
            return Convert(value, factory);
        }


        private static object Convert(object value, MofFactory factory)
        {
            var setter = new DotNetSetter(factory);

            // First, initialize all necessary methods
            return setter.Convert(value);
        }

        /// <summary>
        /// Converts the given object and stores it into the receiver's method
        /// </summary>
        /// <param name="value"></param>
        private object Convert(object value)
        {
            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                return value;
            }
            else
            {
                // Check, if the element already existed 
                if (_visitedElements.Contains(value))
                {
                    return null;
                }

                _visitedElements.Add(value);

                var createdElement = _factory.create(null);
                
                var type = value.GetType();
                foreach (var reflectedProperty in type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public))
                {
                    var innerValue = reflectedProperty.GetValue(value);
                    createdElement.set(reflectedProperty.Name, Convert(innerValue));
                }

                return createdElement;
            }
        }
    }
}
