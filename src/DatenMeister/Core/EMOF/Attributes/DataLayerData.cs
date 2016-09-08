using System.Collections.Generic;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Core.EMOF.Attributes
{
    /// <summary>
    /// Stores the data which is persistent within the process
    /// </summary>
    public class DataLayerData
    {
        public DataLayerData()
        {
            Default = new DataLayer.DataLayer("Default");
        }

        public DataLayerData(DataLayers dataLayers)
        {
            Default = dataLayers.Data;
        }

        /// <summary>
        /// Stores the relations between the layer and the metalayer. 
        /// Key is the layer, value is the metalayer
        /// </summary>
        public Dictionary<IDataLayer, IDataLayer> Relations { get; } = new Dictionary<IDataLayer, IDataLayer>();

        public Dictionary<IExtent, IDataLayer> Extents { get; } = new Dictionary<IExtent, IDataLayer>();

        /// <summary>
        /// Gets or sets the default layer that shall be assumed, if no information is considered as available.
        /// </summary>
        public IDataLayer Default { get; set; }
    }
}