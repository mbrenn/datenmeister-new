using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Reflection;
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
        public static DotNetElement CreateDotNetElement(this IDotNetTypeLookup typeLookup, object value)
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

            return new DotNetElement(typeLookup, value, metaclass);
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
            return constructorInfo.Invoke(new[] {list, lookup, container}) as IReflectiveSequence;
        }

        /// <summary>
        /// Creates a new dot net element ot of the given type lookup and the value. 
        /// In addition, the method also creates the extents from the owner to the given value
        /// </summary>
        /// <param name="typeLookup"></param>
        /// <param name="value"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static DotNetElement CreateDotNetElement(this IDotNetTypeLookup typeLookup, object value, DotNetElement owner)
        {
            var result = CreateDotNetElement(typeLookup, value);
            if (owner != null)
            {
                result.SetContainer(owner);
                result.TransferExtents(owner);
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
            if (element is IElement)
            {
                if (element is DotNetElement)
                {
                    var elementAsDotNet = element as DotNetElement;
                    return elementAsDotNet.GetNativeValue();
                }

                throw new InvalidOperationException("Converting from another IElement instance, except DotNetElement, is not supported (yet).");
            }

            return element;
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
            DotNetElement container, 
            DotNetExtent extent)
        {
            if (result == null)
            {
                return null;
            }

            var resultType = result.GetType();
            if (DotNetHelper.IsPrimitiveType(resultType))
            {
                return result;
            }

            if (DotNetHelper.IsEnumeration(resultType))
            {
                return dotNetTypeLookup.CreateDotNetReflectiveSequence(result, container);
            }

            var dotNetResult = dotNetTypeLookup.CreateDotNetElement(result, container);
            if (extent != null)
            {
                dotNetResult.SetExtent(extent);
            }
            return dotNetResult;
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