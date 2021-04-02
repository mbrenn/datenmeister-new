#nullable enable

using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.Provider.Xmi;
using DatenMeister.DependencyInjection;
using DatenMeister.Runtime.ExtentStorage;

namespace DatenMeister.Integration
{
    public static partial class Extensions
    {
        /// <summary>
        /// Creates a new xmi extent and adds it to the
        /// </summary>
        /// <param name="scope">Scope being used</param>
        /// <param name="uri">Uri being used</param>
        /// <returns>The created xmi extent</returns>
        public static IUriExtent CreateXmiExtent(
            this IDatenMeisterScope scope,
            string uri)
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
            this IDatenMeisterScope scope,
            string uri,
            string filename)
        {
            return scope.Resolve<ExtentManager>().CreateAndAddXmiExtent(uri, filename);
        }
    }
}