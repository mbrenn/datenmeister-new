using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Web.Models.Modules.ViewFinder
{
    /// <summary>
    /// The viewfinder is responsible to find the correct view for a certain
    /// extent or a detail that can be shown to the interface
    /// </summary>
    public interface IViewFinder
    {
        /// <summary>
        /// Finds the view for a specific extent in the list view
        /// </summary>
        /// <param name="extent">Value for whom the extent shall be created</param>
        /// <param name="viewname">Name of the view</param>
        /// <returns>Found view or null</returns>
        IObject FindView(IUriExtent extent, string viewname);

        /// <summary>
        /// Finds the view for a specific object in a detail view
        /// </summary>
        /// <param name="value">Value for whom the object shall be created</param>
        /// <param name="viewname">Name of the view</param>
        /// <returns>Found view or null</returns>
        IObject FindView(IObject value, string viewname);
    }
}