using System;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

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
    }
}