using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Helper;
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
            lock (_data)
            {
                var layerAsObject = layer as DataLayer;
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

                return null;
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