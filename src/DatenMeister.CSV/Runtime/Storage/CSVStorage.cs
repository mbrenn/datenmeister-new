using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;

namespace DatenMeister.CSV.Runtime.Storage
{
    /// <summary>
    /// The engine being used to load and store the extent into a csv file
    /// </summary>
    [ConfiguredBy(typeof(CSVStorageConfiguration))]
    // ReSharper disable once InconsistentNaming
    public class CSVStorage : IExtentStorage
    { 
        public IUriExtent LoadExtent(ExtentStorageConfiguration configuration)
        {
            var csvConfiguration = (CSVStorageConfiguration) configuration;
            var provider = new CSVDataProvider();
            var mofExtent = new MofUriExtent(csvConfiguration.ExtentUri);
            var factory = new MofFactory();

            provider.Load(mofExtent,factory, csvConfiguration.Path, csvConfiguration.Settings);

            return mofExtent;
        }

        public void StoreExtent(IUriExtent extent, ExtentStorageConfiguration configuration)
        {
            var csvConfiguration = (CSVStorageConfiguration) configuration;

            var provider = new CSVDataProvider();
            provider.Save(extent, csvConfiguration.Path, csvConfiguration.Settings);
        }
    }
}