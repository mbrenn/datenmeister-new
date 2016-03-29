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
    }
}