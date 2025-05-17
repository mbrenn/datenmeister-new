#nullable enable

using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.CSV
{
    public static partial class Extensions
    {
        /// <summary>
        /// Loads a csv extent by settings
        /// </summary>
        /// <param name="workspaceLogic">Workspace to be used</param>
        /// <param name="path">Path to be loaded</param>
        /// <param name="uri">Uri for the loaded extent</param>
        /// <param name="settings">Settings being loaded</param>
        /// <returns>Extent to be used</returns>
        public static IUriExtent LoadCsv(this IWorkspaceLogic workspaceLogic, string uri, string path,
            IElement? settings = null)
        {
            var provider = new CsvLoader(workspaceLogic);

            var memoryProvider = new InMemoryProvider();
            var extent = new MofUriExtent(memoryProvider, uri, workspaceLogic.ScopeStorage);

            using var stream = new FileStream(path, FileMode.Open);
            provider.Load(memoryProvider, stream, settings);

            return extent;
        }
    }
}