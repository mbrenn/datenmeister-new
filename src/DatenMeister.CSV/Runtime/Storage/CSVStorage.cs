using System;
using System.IO;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.CSV.Runtime.Storage
{
    /// <summary>
    /// The engine being used to load and store the extent into a csv file
    /// </summary>
    public class CSVStorage : IExtentStorage<CSVStorageConfiguration>
    {
        public IUriExtent LoadExtent(CSVStorageConfiguration configuration)
        {
            var provider = new CSVDataProvider();
            var mofExtent = new MofUriExtent(configuration.ExtentUri);
            var factory = new MofFactory();

            provider.Load(mofExtent,factory, configuration.Path, configuration.Settings);

            return mofExtent;
        }

        public void StoreExtent(IUriExtent extent, CSVStorageConfiguration configuration)
        {
            var provider = new CSVDataProvider();
            provider.Save(extent, configuration.Path, configuration.Settings);
        }
    }
}