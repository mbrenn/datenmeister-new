using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Integration
{
    public static partial class Extensions
    {
		/// <summary>
        /// Creates a new xmi extent and adds it to the 
        /// </summary>
        /// <param name="scope">Scopee being used</param>
        /// <param name="uri">Uri being used</param>
        /// <returns>The created xmi extent</returns>
        public static IUriExtent CreateXmiExtent(
			this IDatenMeisterScope scope,
            string uri)
		{
            var xmlProvider = new XmlUriExtent();
		    return new MofUriExtent(xmlProvider, uri);
		}
    }
}