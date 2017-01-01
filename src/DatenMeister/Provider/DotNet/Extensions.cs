using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
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
        public static DotNetElement CreateDotNetElement(
            this IDotNetTypeLookup typeLookup,
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

            var result = new DotNetElement(typeLookup, value, metaclass);
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
            DotNetElement container)
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
            if (!DotNetHelper.IsOfMofElement(element))
            {
                return element;
            }

            var elementAsDotNetElement = element as DotNetElement;
            if (elementAsDotNetElement != null)
            {
                return elementAsDotNetElement.GetNativeValue();
            }

            throw new InvalidOperationException("Converting from another IElement instance, except DotNetElement, is not supported (yet).");
        }

        /// <summary>
        /// Verifies the type of the given element and creates a DotNetElement if the given 
        /// value is not null and is not a primitive type
        /// </summary>
        /// <param name="dotNetTypeLookup">The .NetType Lookup being used</param>
        /// <param name="result">Value to be converted</param>
        /// <param name="container">Defines the container for the .Net element</param>
        /// <param name="extent">The extent that is directly owning the elements. 
        /// See also IDotNetReflectiveSequence</param>
        /// <returns>The converted or non-converted type</returns>
        public static object CreateDotNetElementIfNecessary(
            this IDotNetTypeLookup dotNetTypeLookup, 
            object result,
            DotNetElement container)
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
                return dotNetTypeLookup.CreateDotNetReflectiveSequence(result, container);
            }

            if (DotNetHelper.IsOfMofObject(result))
            {
                // Returns the given element itself, if it is an MofElement or MofObject
                return result;
            }

            throw new NotImplementedException();
            /*
            var dotNetResult = dotNetTypeLookup.CreateDotNetElement(result, container);

            return dotNetResult;*/
        }

        /// <summary>
        /// Generates the mof element out of the given type and adds it to the .Net Type Lookup
        /// </summary>
        /// <param name="typeLookup">Type Lookup to be used</param>
        /// <param name="uml">Uml instance being used to create all necessary instances</param>
        /// <param name="factory">The factory to create the type</param>
        /// <param name="dotNetType">And finally the .Net type that is converted and adde</param>
        /// <returns></returns>
        public static IElement GenerateAndAdd(this IDotNetTypeLookup typeLookup, _UML uml, IFactory factory, Type dotNetType)
        {
            var dotNetTypeCreator = new DotNetTypeGenerator(factory, uml);
            var element = dotNetTypeCreator.CreateTypeFor(dotNetType);
            typeLookup.Add(element, dotNetType);
            return element;
        }
    }
}