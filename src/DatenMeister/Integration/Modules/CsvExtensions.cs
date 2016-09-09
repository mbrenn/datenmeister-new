using System.IO;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.CSV;
using DatenMeister.CSV.EMOF;
using DatenMeister.Integration;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Models.Forms
{
    public static class CsvExtensions
    {
        /// <summary>
        /// Loads a csv extent by settings
        /// </summary>
        /// <param name="path">Path to be loaded</param>
        /// <param name="settings">Settings being loaded</param>
        /// <returns>Extent to be used</returns>
        public static IUriExtent LoadCsv(this IDatenMeisterContainer container, string path, string uri, CSV.CSVSettings settings = null)
        {
            settings = settings ?? new CSVSettings();

            var provider = container.Resolve<CSVDataProvider>();
            var extent = new CSVExtent(uri);
            var factory = new CSVFactory(extent);

            using (var stream = new FileStream(path, FileMode.Open))
            {
                provider.Load(extent, factory, stream, settings);
            }

            return extent;

            // var workspaceCollection = settings.Resolve<IWorkspaceCollection>();
            //workspaceCollection.
        }

    }
}