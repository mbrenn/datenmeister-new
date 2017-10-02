﻿using System;
using System.Collections.Generic;
using System.Reflection;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;

namespace DatenMeister.Provider.DotNet
{
    public static class Extensions
    {
        /// <summary>
        /// Creates a dot net element out of the given type lookup and the value
        /// </summary>
        /// <param name="typeLookup">Type lookup being used</param>
        /// <param name="value">Value to be converted</param>
        /// <param name="extent">Defines the extent being associated to the DotNetElement</param>
        /// <param name="id">If set, the id will be set to the given element</param>
        public static IElement CreateDotNetElement(
            this IDotNetTypeLookup typeLookup,
            MofUriExtent extent,
            object value,
            string id = null)
        {
            var providerAsDotNet = extent.Provider as DotNetProvider;
            if (providerAsDotNet == null)
            {
                throw new InvalidOperationException("Given extent is not from DotNetProvider");
            }

            var result = CreateDotNetProviderObject(typeLookup, providerAsDotNet, value, id);

            return new MofElement(result, extent);
        }

        /// <summary>
        /// Creates a DotNetProvider object out of the internal information
        /// </summary>
        /// <param name="typeLookup"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static DotNetProviderObject CreateDotNetProviderObject(
            this IDotNetTypeLookup typeLookup,
            DotNetProvider provider,
            object value,
            string id = null)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var metaclass = typeLookup.ToElement(value.GetType());
            if (metaclass == null)
            {
                throw new InvalidOperationException(
                    $"The type '{value.GetType().FullName}' is not known to the DotNetTypeLookup");
            }

            var result = new DotNetProviderObject(provider, typeLookup, value, metaclass);
            if (!string.IsNullOrEmpty(id))
            {
                result.Id = id;
            }

            return result;
        }

        /// <summary>
        /// Creates a .net reflective sequence
        /// </summary>
        /// <param name="lookup">The dotnet lookup to be used</param>
        /// <param name="list">The list to be parsed</param>
        /// <param name="container">Stores the container for the given element</param>
        /// <returns>The created reflective sequence working on the given list</returns>
        public static IReflectiveSequence CreateDotNetReflectiveSequence(
            this IDotNetTypeLookup lookup, 
            object list,
            DotNetProviderObject container)
        {
            throw new NotImplementedException();
            /*
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var type = list.GetType();

            // Finds the interface type
            var interfaceType =
                type
                    .GetInterfaces()
                    .FirstOrDefault(x => x.IsConstructedGenericType && x.GetGenericTypeDefinition() == typeof(IList<>));

            if (interfaceType == null)
            {
                throw new InvalidOperationException($"list is not of Type IList<T>. It is of type: {list.GetType()}");
            }

            var genericParameter = interfaceType.GenericTypeArguments[0];
            var dotNetReflectiveSequenceType = typeof(DotNetReflectiveSequence<>).MakeGenericType(genericParameter);

            var constructorInfo = dotNetReflectiveSequenceType.GetConstructor(new[] {type, typeof(IDotNetTypeLookup), typeof(DotNetElement)});
            return constructorInfo.Invoke(new[] {list, lookup, container}) as IReflectiveSequence;*/
        }

        /// <summary>
        /// Creates a .net reflective sequence
        /// </summary>
        /// <param name="lookup">The dotnet lookup to be used</param>
        /// <param name="list">The list to be parsed</param>
        /// <param name="extent">Stores the extent for the given element</param>
        /// <returns>The created reflective sequence working on the given list</returns>
        public static IReflectiveSequence CreateDotNetReflectiveSequence(
            this IDotNetTypeLookup lookup,
            object list)
        {
            throw new NotImplementedException();
            /*
            var result = CreateDotNetReflectiveSequence(lookup, list, (DotNetElement) null);
            ((IDotNetReflectiveSequence)result).SetExtent(extent);
            return result;*/
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

            var elementAsDotNetElement = element as DotNetProviderObject;
            if (elementAsDotNetElement != null)
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
        /// <returns>The converted or non-converted type</returns>
        public static object CreateDotNetElementIfNecessary(
            this IDotNetTypeLookup dotNetTypeLookup, 
            object result,
            DotNetProvider provider)
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
                var listResult = new List<object>();
                var asEnumeration = (IEnumerable<object>) result;
                foreach (var item in asEnumeration)
                {
                    listResult.Add(dotNetTypeLookup.CreateDotNetElementIfNecessary(item, provider));
                }

                return listResult;
            }

            if (DotNetHelper.IsOfMofObject(result))
            {
                // Returns the given element itself, if it is an MofElement or MofObject
                return result;
            }
            
            var dotNetResult = dotNetTypeLookup.CreateDotNetProviderObject(provider, result);

            return dotNetResult;
        }

        /// <summary>
        /// Generates the mof element out of the given type and adds it to the .Net Type Lookup
        /// </summary>
        /// <param name="uml">Uml instance being used to create all necessary instances</param>
        /// <param name="extent">Extent to which the generated element will be added</param>
        /// <param name="dotNetType">And finally the .Net type that is converted and adde</param>
        /// <returns></returns>
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