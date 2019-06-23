using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Runtime.ExtentStorage
{
    public static class Extension
    {
        /// <summary>
        /// Creates an xmi extent and adds it to the Data workspace
        /// </summary>
        /// <param name="extentManager">Extent Manager to be used</param>
        /// <param name="uri"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IUriExtent CreateAndAddXmiExtent(this IExtentManager extentManager, string uri, string filename)
        {
            var xmiConfiguration = new XmiStorageConfiguration
            {
                extentUri = uri,
                workspaceId = WorkspaceNames.NameData,
                filePath = filename
            };

            return extentManager.LoadExtent(xmiConfiguration, ExtentCreationFlags.LoadOrCreate);
        }
    }
}