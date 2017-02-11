using System;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.CSV;

namespace DatenMeister.Integration
{
    public static partial class Extensions
    {
        /// <summary>
        /// Loads a csv extent by settings
        /// </summary>
        /// <param name="scope">Container being initialized</param>
        /// <param name="path">Path to be loaded</param>
        /// <param name="uri">Uri for the loaded extent</param>
        /// <param name="settings">Settings being loaded</param>
        /// <returns>Extent to be used</returns>
        public static IUriExtent LoadCsv(this IDatenMeisterScope scope, string uri, string path, CSVSettings settings = null)
        {
            settings = settings ?? new CSVSettings();

            var provider = scope.Resolve<CSVDataProvider>();

            throw new NotImplementedException();
            /*var extent = new CSVExtent(uri);
            var factory = new CSVFactory(extent);

            using (var stream = new FileStream(path, FileMode.Open))
            {
                provider.Load(extent, factory, stream, settings);
            }

            return extent;*/
        }

    }
}