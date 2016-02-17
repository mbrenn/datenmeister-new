using System;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

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
    }
}