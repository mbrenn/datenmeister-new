using System;
using System.IO;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.CSV;
using DatenMeister.Provider.InMemory;

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

            var provider = scope.Resolve<CSVLoader>();

            var memoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(memoryProvider, uri);
            
            using (var stream = new FileStream(path, FileMode.Open))
            {
                provider.Load(memoryProvider, stream, settings);
            }

            return extent;
        }

    }
}