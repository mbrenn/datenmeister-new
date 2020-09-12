#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.DotNet
{
    public static class DotNetProviderExtensions
    {
        /// <summary>
        /// Defines the class logger being used for messages
        /// </summary>
        private static readonly ILogger ClassLogger = new ClassLogger(typeof(DotNetProviderExtensions));

        /// <summary>
        /// Creates a Mof Element reflecting the .Net Element out of the given extent.
        /// </summary>
        /// <param name="value">Value to be converted</param>
        /// <param name="extent">Defines the extent being associated to the DotNetElement</param>
        /// <param name="id">If set, the id will be set to the given element</param>
        public static IElement CreateDotNetMofElement(
            this MofUriExtent extent,
            object value,
            string? id = null)
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
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static DotNetProviderObject CreateDotNetProviderObject(
            this DotNetProvider provider,
            object value,
            string? id = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var metaclass = provider.TypeLookup.ToElement(value.GetType());
            if (metaclass == null)
            {
                ClassLogger.Warn("MetaClass for type: " + value.GetType().FullName + " not found");
            }

            var result = new DotNetProviderObject(provider, value, metaclass ?? string.Empty);
            if (!string.IsNullOrEmpty(id))
            {
                result.Id = id!;
            }

            return result;
        }

        /// <summary>
        /// Converts the given element to a .Net native object, which means that it
        /// unwraps an DotNetElement element to its abstracted value
        /// </summary>
        /// <param name="element">Element to be converted</param>
        /// <returns>The converted object</returns>
        public static object? ConvertToNative(object? element)
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
        /// <param name="result">Value to be converted</param>
        /// <param name="provider">The provider being used to convert the .Net Value as MofElement</param>
        /// <returns>The converted or non-converted type</returns>
        public static object? CreateDotNetElementIfNecessary(
            this DotNetProvider provider,
            object? result)
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

                return asEnumeration.Select(provider.CreateDotNetElementIfNecessary!).ToList();
            }

            if (DotNetHelper.IsOfMofObject(result))
            {
                var asMofObject = result as MofObject ?? throw new InvalidOperationException("Not a MofObject");
                var otherProvider = asMofObject.ProviderObject.Provider;
                if (otherProvider == provider)
                {
                    // Returns the given element itself, if it is an MofElement or MofObject
                    return asMofObject.ProviderObject;
                }
                else
                {
                    var uriExtent = asMofObject.GetUriExtentOf()
                                    ?? throw new InvalidOperationException("No UriExtent connected");
                    var asElement = asMofObject as IElement
                                    ?? throw new InvalidOperationException("Element is not an IElement");
                    var uri = uriExtent.uri(asElement) ?? throw new InvalidOperationException("Uri not found");

                    // It is from another provider, so we have to create a urireference
                    return new UriReference(uri);
                }
            }

            var dotNetResult = provider.CreateDotNetProviderObject(result);

            return dotNetResult;
        }

        /// <summary>
        /// Generates the mof element out of the given type and adds it to the .Net Type Lookup
        /// </summary>
        /// <param name="extent">Extent to which the generated element will be added</param>
        /// <param name="dotNetType">And finally the .Net type that is converted and adde</param>
        /// <returns>The created type specification</returns>
        public static IElement CreateTypeSpecification(this MofUriExtent extent, Type dotNetType)
        {
            var factory = new MofFactory(extent);
            var dotNetTypeCreator = new DotNetTypeGenerator(factory);
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
            var uri = element.GetUri();
            if (uri != null)
            {
                lookup.Add(uri, type);
            }
        }
    }
}