using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Filler;

namespace DatenMeister.DataLayer
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
                _data.Default = DataLayers.Data;
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

        public IDataLayer GetMetaLayerOfObject(IObject value)
        {
            lock (_data)
            {
                var asHasExtent = value as IObjectHasExtent;
                if (asHasExtent == null)
                {
                    throw new InvalidOperationException("Not known to which extent this value belongs :-(");
                }

                return GetDataLayerOfExtent(asHasExtent.GetContainingExtent());
            }
        }

        public IDataLayer GetMetaLayerFor(IDataLayer data)
        {
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

        public TFilledType Get<TFiller, TFilledType>(IDataLayer layer)
            where TFiller : IFiller<TFilledType>, new()
            where TFilledType : class, new()
        {
            lock (_data)
            {
                var layerAsObject = layer as DataLayer;
                if (layerAsObject == null)
                {
                    throw new ArgumentException($"{nameof(layer)} is not of type DataLayer", nameof(layer));
                }

                // Looks into the cache for the filledtypes
                foreach (var value in layerAsObject.FilledTypeCache)
                {
                    if (value is TFilledType)
                    {
                        return value as TFilledType;
                    }
                }

                // Not found, we need to fill it on our own... Congratulation
                var filler = new TFiller();
                var filledType = new TFilledType();

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

        public void ClearCache(IDataLayer layer)
        {
            lock (_data)
            {
                var layerAsObject = layer as DataLayer;
                layerAsObject.FilledTypeCache.Clear();
            }
        }
    }
}