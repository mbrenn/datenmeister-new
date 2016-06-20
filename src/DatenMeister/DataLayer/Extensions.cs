using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.DataLayer
{
    public static class Extensions
    {
        /// <summary>
        /// Gets the metalayer
        /// </summary>
        /// <param name="logic">Datalayer being queried</param>
        /// <param name="value">Datalayer for the value</param>
        /// <returns>The metalayer ot the object</returns>
        public static IDataLayer GetMetaLayerOfObject(this IDataLayerLogic logic, IObject value)
        {
            var dataLayer = logic.GetDataLayerOfObject(value);
            if (dataLayer == null)
            {
                return null;
            }

            return logic.GetMetaLayerFor(dataLayer);
        }

        /// <summary>
        /// Gets the given class from the metalayer to the datalayer
        /// </summary>
        /// <typeparam name="TFilledType">Type that is queried</typeparam>
        /// <param name="logic">The logic being used fby this method</param>
        /// <param name="dataLayer">The datalayer that is requested</param>
        /// <returns>The instance of the type</returns>
        public static TFilledType GetFromMetaLayer<TFilledType>(
            this IDataLayerLogic logic, 
            IDataLayer dataLayer)
            where TFilledType : class, new()
        {
            var metaLayer = logic.GetMetaLayerFor(dataLayer);
            return metaLayer != null ? logic.Get<TFilledType>(metaLayer) : null;
        }

        /// <summary>
        /// Gets the given class from the metalayer to the datalayer
        /// </summary>
        /// <typeparam name="TFilledType">Type that is queried</typeparam>
        /// <param name="logic">The logic being used fby this method</param>
        /// <param name="extent">The extent that is requested</param>
        /// <returns>The instance of the type</returns>
        public static TFilledType GetFromMetaLayer<TFilledType>(
            this IDataLayerLogic logic,
            IExtent extent)
            where TFilledType : class, new()
        {
            var dataLayer = logic.GetDataLayerOfExtent(extent);
            if (dataLayer == null)
            {
                return null;
            }

            return GetFromMetaLayer<TFilledType>(logic, dataLayer);
        }
    }
}