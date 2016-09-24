using Autofac;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.XMI.EMOF;

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
		    var workspaceCollection = scope.Resolve<IWorkspaceLogic>();
            var result = new XmlUriExtent(workspaceCollection,uri);
		    return result;
		}
    }
}