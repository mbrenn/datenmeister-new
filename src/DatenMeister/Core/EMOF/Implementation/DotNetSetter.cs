using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DatenMeister.Provider.DotNet;
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

        private readonly IDotNetTypeLookup _typeLookup;

        /// <summary>
        /// Stores a list of already visited elements, so a recursion is avoided
        /// </summary>
        private readonly HashSet<object> _visitedElements = new HashSet<object>();

        private IUriResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the DotNetSetter class
        /// </summary>
        /// <param name="extent">Extent being used as reference to find typeLookup and Resolver</param>
        /// <param name="typeLookup">The lookup class being used to retrieve the meta class. May be null</param>
        public DotNetSetter(MofExtent extent)
        {
            _factory = new MofFactory(extent);
            _typeLookup = extent.TypeLookup;
            _resolver = extent.Resolver;
        }

        /// <summary>
        /// Sets the given object into the MofObject. 
        /// </summary>
        /// <param name="receiver">Object which shall receive the dotnet value</param>
        /// <param name="value">Value to be set</param>
        /// /// <param name="typeLookup">The lookup class being used to retrieve the meta class</param>
        public static object Convert(MofExtent receiver, object value)
        {
            var setter = new DotNetSetter(receiver);
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
            else if (value == null)
            {
                return null;
            }
            else
            {
                // Check, if the element already existed 
                if (_visitedElements.Contains(value))
                {
                    return null;
                }

                _visitedElements.Add(value);

                var metaClassUri = _typeLookup?.ToElement(value.GetType());
                var createdElement = _factory.create(
                    metaClassUri == null ?
                    null : 
                    _resolver.Resolve(metaClassUri));
                
                var type = value.GetType();
                foreach (var reflectedProperty in type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public))
                {
                    var innerValue = reflectedProperty.GetValue(value);
                    if (DotNetHelper.IsOfEnumeration(innerValue))
                    {
                        var list = new List<object>();
                        var enumeration = (IEnumerable) innerValue;
                        foreach (var innerElementValue in enumeration)
                        {
                            list.Add(Convert(innerElementValue));
                        }

                        createdElement.set(reflectedProperty.Name, list);
                    }
                    else
                    {
                        createdElement.set(reflectedProperty.Name, Convert(innerValue));
                    }
                }

                return createdElement;
            }
        }
    }
}
