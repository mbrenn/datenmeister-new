#nullable enable

using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.Extent.Manager.ExtentStorage;

namespace DatenMeister.Extent.Manager
{
    public static class XmiExtensions
    {
        /// <summary>
        /// Creates a new xmi extent and adds it to the
        /// </summary>
        /// <param name="uri">Uri being used</param>
        /// <returns>The created xmi extent</returns>
        public static IUriExtent CreateXmiExtent(string uri)
        {
            var xmlProvider = new XmiProvider();
            return new MofUriExtent(xmlProvider, uri);
        }

        /// <summary>
        /// CReates the
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="uri"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ExtentStorageData.LoadedExtentInformation CreateAndAddXmiExtent(
            ExtentManager scope,
            string uri,
            string filename)
        {
            return scope.CreateAndAddXmiExtent(uri, filename);
        }
    }
}