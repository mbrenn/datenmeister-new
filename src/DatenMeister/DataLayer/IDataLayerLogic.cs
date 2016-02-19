using System;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Filler;

namespace DatenMeister.DataLayer
{
    public interface IDataLayerLogic
    {
        void SetDefaultDatalayer(IDataLayer layer);
        /// <summary>
        /// Sets the relationship between two layers
        /// </summary>
        /// <param name="dataLayer">Layer to be allocated to another layer</param>
        /// <param name="metaDataLayer">Layer being allocated</param>
        void SetRelationShip(IDataLayer dataLayer, IDataLayer metaDataLayer);

        /// <summary>
        /// Assigns a layer
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="dataLayer"></param>
        void AssignToDataLayer(IExtent extent, IDataLayer dataLayer);

        IDataLayer GetDataLayerOfExtent(IExtent extent);

        IDataLayer GetMetaLayerOfObject(IObject value);
        /// <summary>
        /// Gets the meta layer for the given data layer
        /// </summary>
        /// <param name="data">Datalayer to be queried</param>
        /// <returns>The corresponding datalayer</returns>
        IDataLayer GetMetaLayerFor(IDataLayer data);

        /// <summary>
        /// Gets an instance of the filled type by using the filler. 
        /// The instance will be cached on first call of the method
        /// </summary>
        /// <typeparam name="TFiller">Filler to be used to create the filled type</typeparam>
        /// <typeparam name="TFilledType">The filled type which is returned</typeparam>
        /// <returns>The filled type, could also be cached</returns>
        TFilledType Get<TFiller, TFilledType>(IDataLayer layer)
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new();

        /// <summary>
        /// Clears the cache, so a new instance can be created
        /// </summary>
        /// <param name="layer">Layer, whose cache needs to be deleted</param>
        void ClearCache(IDataLayer layer);
    }
}