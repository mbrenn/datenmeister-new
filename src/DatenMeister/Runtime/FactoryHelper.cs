using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

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
        public static TFilledType GetMetaInformation<TFilledType>(
            this IFactory factory) 
            where TFilledType : class, new()
        {
            var mofFactory = factory as MofFactory ??
                             throw new ArgumentException(
                                 "Not of type MofExtent", 
                                 nameof(factory));
           return mofFactory.Extent.Workspace.GetFromMetaWorkspace<TFilledType>();
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
    }
}