﻿using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Helper;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Filler;

namespace DatenMeister.Core.DataLayer
{
    /// <summary>
    /// The logic defines the relationships between the layers and the metalayers. 
    /// </summary>
    public class DataLayerLogic : IDataLayerLogic
    {
        private readonly DataLayerData _data;

        public DataLayerLogic(DataLayerData data)
        {
            _data = data;
            if (_data.Default == null)
            {
                throw new InvalidOperationException("DataLayer.Default was not set");
            }
        }

        public void SetDefaultDatalayer(IDataLayer layer)
        {
            lock (_data)
            {
                _data.Default = layer;
            }
        }

        public void SetRelationShip(IDataLayer dataLayer, IDataLayer metaDataLayer)
        {
            lock (_data)
            {
                _data.Relations[dataLayer] = metaDataLayer;
            }
        }

        public void AssignToDataLayer(IExtent extent, IDataLayer dataLayer)
        {
            lock (_data)
            {
                _data.Extents[extent] = dataLayer;
            }
        }

        public IDataLayer GetDataLayerOfExtent(IExtent extent)
        {
            lock (_data)
            {
                IDataLayer dataLayer;
                if (_data.Extents.TryGetValue(extent, out dataLayer))
                {
                    return dataLayer;
                }

                return _data.Default;
            }
        }

        public IEnumerable<IExtent> GetExtentsOfDatalayer(IDataLayer layer)
        {
            lock (_data)
            {
                return _data.Extents.Where(x => x.Value == layer).Select(x => x.Key).ToList();
            }
        }

        public IDataLayer GetDataLayerOfObject(IObject value)
        {
            // If the object is contained by another object, query the contained objects 
            // because the extents will only be stored in the root elements
            var asElement = value as IElement;
            var parent = asElement?.container();
            if (parent != null)
            {
                return GetDataLayerOfObject(parent);
            }

            // If the object knows the extent to which it belongs to, it will return it
            var objectKnowsExtent = value as IObjectKnowsExtent;
            if (objectKnowsExtent != null)
            {
                var found = objectKnowsExtent.Extents.FirstOrDefault();
                return found == null
                    ? _data.Default
                    : GetDataLayerOfExtent(found);
            }

            // Otherwise check it by the dataextent
            lock (_data)
            {
                var extentContainingObject = _data.Extents.Select(x => x.Key).Cast<IUriExtent>().WithElement(value);
                if (extentContainingObject == null)
                {
                    return _data.Default;
                }

                IDataLayer result;
                _data.Extents.TryGetValue(extentContainingObject, out result);

                return result ?? _data.Default;
            }
        }

        public IDataLayer GetMetaLayerFor(IDataLayer data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            lock (_data)
            {
                IDataLayer result;
                if (_data.Relations.TryGetValue(data, out result))
                {
                    return result;
                }

                throw new InvalidOperationException($"No meta layer was given for datalayer: {data.Name}");
            }
        }

        public IEnumerable<IUriExtent> GetExtentsForDatalayer(IDataLayer dataLayer)
        {
            if (dataLayer == null) throw new ArgumentNullException(nameof(dataLayer));

            lock (_data)
            {
                return
                    _data.Extents.Where(x => x.Value == dataLayer)
                        .Select(x => x.Key as IUriExtent)
                        .Where(x => x != null)
                        .ToList();
            }
        }

        public TFilledType Create<TFiller, TFilledType>(IDataLayer layer)
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new()
        {
            if (layer == null) throw new ArgumentNullException(nameof(layer));

            lock (_data)
            {
                var layerAsObject = layer as DataLayer;
                VerifyThatNotNull(layerAsObject);

                var filledType = Get<TFilledType>(layerAsObject);
                if (filledType != null)
                {
                    return filledType;
                }

                // Not found, we need to fill it on our own... Congratulation
                var filler = new TFiller();
                filledType = new TFilledType();

                // Go through all extents of this datalayer
                foreach (var extent in GetExtentsOfDatalayer(layer))
                {
                    filler.Fill(extent.elements(), filledType);
                }

                // Adds it to the database
                layerAsObject.FilledTypeCache.Add(filledType);
                return filledType;
            }
        }

        public TFilledType Get<TFilledType>(IDataLayer layer)
            where TFilledType : class, new()
        {
            if (layer == null) throw new ArgumentNullException(nameof(layer));

            lock (_data)
            {
                var layerAsObject = layer as DataLayer;
                VerifyThatNotNull(layerAsObject);
                    
                // Looks into the cache for the filledtypes
                foreach (var value in layerAsObject.FilledTypeCache)
                {
                    if (value is TFilledType)
                    {
                        return value as TFilledType;
                    }
                }

                return null;
            }
        }

        public void Set<TFilledType>(IDataLayer layer, TFilledType value) where TFilledType : class, new()
        {
            lock (_data)
            {
                var layerAsObject = layer as DataLayer;
                VerifyThatNotNull(layerAsObject);

                layerAsObject.FilledTypeCache.Add(value);
            }
        }

        /// <summary>
        /// Gets the datalayer by name.
        /// The datalayer will only be returned, if there is a relationship
        /// </summary>
        /// <param name="name">Name of the datalayer</param>
        /// <returns>Found datalayer or null</returns>
        public IDataLayer GetByName(string name)
        {
            lock (_data)
            {
                var result = _data.Relations.Keys.FirstOrDefault(x => x.Name == name);
                result = result ?? _data.Relations.Values.FirstOrDefault(x => x.Name == name);
                result = result ?? (_data.Default?.Name == name ? _data.Default : null);
                return result;
            }
        }

        private static void VerifyThatNotNull(DataLayer layerAsObject)
        {
            if (layerAsObject == null)
            {
                throw new ArgumentException($"{nameof(layerAsObject)} is not of type DataLayer", nameof(layerAsObject));
            }
        }

        public void ClearCache(IDataLayer layer)
        {
            lock (_data)
            {
                var layerAsObject = layer as DataLayer;
                layerAsObject.FilledTypeCache.Clear();
            }
        }

        public static IDataLayerLogic InitDefault(out DataLayers dataLayers)
        {
            dataLayers = new DataLayers();
            var data = new DataLayerData(dataLayers);
            var logic = new DataLayerLogic(data);
            dataLayers.SetRelationsForDefaultDataLayers(logic);
            return logic;
        }
    }
}