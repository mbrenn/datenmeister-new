﻿using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// Gets the datalayer of a certain extent
        /// </summary>
        /// <param name="extent"></param>
        /// <returns></returns>
        IDataLayer GetDataLayerOfExtent(IExtent extent);

        /// <summary>
        /// Gets the meta layer of a certain object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IDataLayer GetDataLayerOfObject(IObject value);

        /// <summary>
        /// Gets the meta layer for the given data layer
        /// </summary>
        /// <param name="data">Datalayer to be queried</param>
        /// <returns>The corresponding datalayer</returns>
        IDataLayer GetMetaLayerFor(IDataLayer data);

        /// <summary>
        /// Gets all extents for a specific datalayer
        /// </summary>
        /// <param name="dataLayer">Datalayer to be retrieved</param>
        /// <returns>enumeration of extents within the datalayer</returns>
        IEnumerable<IUriExtent> GetExtentsForDatalayer(IDataLayer dataLayer);

        /// <summary>
        /// Gets an instance of the filled type by using the filler. 
        /// The instance will be cached on first call of the method
        /// </summary>
        /// <typeparam name="TFiller">Filler to be used to create the filled type</typeparam>
        /// <typeparam name="TFilledType">The filled type which is returned</typeparam>
        /// <returns>The filled type, could also be cached</returns>
        TFilledType Create<TFiller, TFilledType>(IDataLayer layer)
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new();

        /// <summary>
        /// Gets a cached instance of the filled type. 
        /// This cached instance has to be created by the Create method before. If not found, 
        /// null will be returned
        /// </summary>
        /// <typeparam name="TFilledType">Type of the filled type</typeparam>
        /// <param name="layer">Layer whose filled type shall be retrieved</param>
        /// <returns>The found instance</returns>
        TFilledType Get<TFilledType>(IDataLayer layer)
            where TFilledType : class, new();

        /// <summary>
        /// Clears the cache, so a new instance can be created
        /// </summary>
        /// <param name="layer">Layer, whose cache needs to be deleted</param>
        void ClearCache(IDataLayer layer);

        
    }
}