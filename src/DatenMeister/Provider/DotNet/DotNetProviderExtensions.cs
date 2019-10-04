using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.DotNet
{
    public static class DotNetProviderExtensions
    {
        /// <summary>
        /// Creates a Mof Element reflecting the .Net Element out of the given extent.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="extent">Defines the extent being associated to the DotNetElement</param>
        /// <param name="id">If set, the id will be set to the given element</param>
        public static IElement CreateDotNetMofElement(
            this MofUriExtent extent,
            object value,
            string id = null)
        {
            if (!(extent.Provider is DotNetProvider providerAsDotNet))
            {
                throw new InvalidOperationException("Given extent is not from DotNetProvider");
            }

            var result = CreateDotNetProviderObject(providerAsDotNet, value, id);

            return new MofElement(result, extent);
        }

        /// <summary>
        /// Creates a DotNetProvider object out of the internal information. It just creates DotNetProviderObject which contains the given element
        /// </summary>
        /// <param name="typeLookup"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static DotNetProviderObject CreateDotNetProviderObject(
            this DotNetProvider provider,
            object value,
            string id = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var metaclass = provider.TypeLookup.ToElement(value.GetType());
            if (metaclass == null)
            {
                throw new InvalidOperationException(
                    $"The type '{value.GetType().FullName}' is not known to the DotNetTypeLookup");
            }

            var result = new DotNetProviderObject(provider, value, metaclass);
            if (!string.IsNullOrEmpty(id))
            {
                result.Id = id;
            }

            return result;
        }
        /// <summary>
        /// Converts the given element to a .Net native object, which means that it
        /// unwraps an DotNetElement element to its abstracted value
        /// </summary>
        /// <param name="element">Element to be converted</param>
        /// <returns>The converted object</returns>
        public static object ConvertToNative(object element)
        {
            if (!DotNetHelper.IsOfProviderObject(element))
            {
                return element;
            }

            if (element is DotNetProviderObject elementAsDotNetElement)
            {
                return elementAsDotNetElement.GetNativeValue();
            }

            throw new InvalidOperationException("Converting from another IProviderObject instance, except DotNetElement, is not supported (yet).");
        }

        /// <summary>
        /// Verifies the type of the given element and creates a DotNetElement if the given
        /// value is not null and is not a primitive type
        /// </summary>
        /// <param name="dotNetTypeLookup">The .NetType Lookup being used</param>
        /// <param name="result">Value to be converted</param>
        /// <param name="provider">The provider being used to convert the .Net Value as MofElement</param>
        /// <returns>The converted or non-converted type</returns>
        public static object CreateDotNetElementIfNecessary(
            this DotNetProvider provider,
            object result)
        {
            if (result == null)
            {
                return null;
            }

            var resultType = result.GetType();
            if (DotNetHelper.IsPrimitiveType(resultType)
                || DotNetHelper.IsEnum(resultType))
            {
                return result;
            }

            if (DotNetHelper.IsEnumeration(resultType))
            {
                var asEnumeration = (IEnumerable<object>) result;

                return asEnumeration.Select(provider.CreateDotNetElementIfNecessary).ToList();
            }

            if (DotNetHelper.IsOfMofObject(result))
            {
                // Returns the given element itself, if it is an MofElement or MofObject
                return result;
            }
            
            var dotNetResult = provider.CreateDotNetProviderObject(result);

            return dotNetResult;
        }

        /// <summary>
        /// Generates the mof element out of the given type and adds it to the .Net Type Lookup
        /// </summary>
        /// <param name="uml">Uml instance being used to create all necessary instances</param>
        /// <param name="extent">Extent to which the generated element will be added</param>
        /// <param name="dotNetType">And finally the .Net type that is converted and adde</param>
        /// <returns>The created type specification</returns>
        public static IElement CreateTypeSpecification(this MofUriExtent extent, _UML uml, Type dotNetType)
        {
            var factory = new MofFactory(extent);
            var dotNetTypeCreator = new DotNetTypeGenerator(factory, uml);
            var element = dotNetTypeCreator.CreateTypeFor(dotNetType);
            extent.elements().add(element);
            
            extent.TypeLookup.Add(element, dotNetType);
            return element;
        }

        /// <summary>
        /// Adds an association between type and element
        /// </summary>
        /// <param name="lookup">The DotNetType lookup to whom the value will be added</param>
        /// <param name="element">Element to be added</param>
        /// <param name="type">Type to be added</param>
        public static void Add(this IDotNetTypeLookup lookup, IElement element, Type type)
        {
            lookup.Add(element.GetUri(), type);
        }
    }
}