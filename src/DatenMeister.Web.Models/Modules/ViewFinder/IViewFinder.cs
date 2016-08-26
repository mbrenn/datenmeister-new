using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Models.Modules.ViewFinder
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
        /// <param name="extent">Owning extent to be used to find perfect view</param>
        /// <param name="value">Value for whom the object shall be created</param>
        /// <param name="viewname">Name of the view</param>
        /// <returns>Found view or null</returns>
        IObject FindView(IUriExtent extent, IObject value, string viewname);

        /// <summary>
        /// Finds all views, which might be used for the given extent and item. 
        /// If the item is null, all views will be returned that might fit to the extent
        /// </summary>
        /// <param name="extent">Extent being shown</param>
        /// <param name="value">Object being shown or null, if complete
        /// extent is shown</param>
        /// <returns>Enumeration of allowable extents</returns>
        IEnumerable<IElement> FindViews(IUriExtent extent, IObject value);
    }
}