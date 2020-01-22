using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime
{
    public static class FactoryHelper
    {
        /// <summary>
        /// Gets the filled type information of the meta extents being connected
        /// to the factory.
        /// </summary>
        /// <typeparam name="TFilledType">Requested filled type information</typeparam>
        /// <param name="factory">Factory, which is queried</param>
        /// <returns>The resulting filled type</returns>
        public static TFilledType? GetMetaInformation<TFilledType>(
            this IFactory factory)
            where TFilledType : class, new()
        {
            var mofFactory = factory as MofFactory ??
                             throw new ArgumentException(
                                 @"Not of type MofExtent",
                                 nameof(factory));
            return mofFactory.Extent?.Workspace?.GetFromMetaWorkspace<TFilledType>();
        }

        /// <summary>
        /// Creates a specific type by using the meta information as retrieved
        /// by the meta workspace
        /// </summary>
        /// <typeparam name="TFilledType">Filled Type to retrieve the meta class</typeparam>
        /// <param name="factory">Factory to be used</param>
        /// <param name="type">The function to retrieve the evaluated type</param>
        /// <returns>The created element</returns>
        public static IElement Create<TFilledType>(
            this IFactory factory,
            Func<TFilledType, IElement> type)
            where TFilledType : class, new()
        {
            var metaInfo = GetMetaInformation<TFilledType>(factory);
            return factory.create(type(metaInfo));
        }

        /// <summary>
        /// Creates the element as being retrieved by the TFilledType
        /// by using the given workspace.
        /// </summary>
        /// <typeparam name="TFilledType">Type of the FilledType</typeparam>
        /// <param name="factory">Factory being used to create the instance</param>
        /// <param name="workspace">Workspace from which the TFilledType will be retrieved</param>
        /// <param name="funcType">Function used to create the specific type</param>
        /// <returns>Instance of the type being created</returns>
        public static IElement Create<TFilledType>(
            this IFactory factory,
            Workspace workspace,
            Func<TFilledType, IElement> funcType)
            where TFilledType : class, new()
        {
            var filledType = workspace.Get<TFilledType>();
            var typeToCreated = funcType(filledType);
            return factory.create(typeToCreated);
        }
    }
}