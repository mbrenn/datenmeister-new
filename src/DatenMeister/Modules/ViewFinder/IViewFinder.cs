using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Modules.ViewFinder
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
        /// <param name="viewUrl">Name of the view</param>
        /// <returns>Found view or null</returns>
        IElement FindView(IUriExtent extent, string viewUrl);

        /// <summary>
        /// Finds the view for a specific object in a detail view
        /// </summary>
        /// <param name="value">Value for whom the object shall be created</param>
        /// <param name="viewname">Name of the view</param>
        /// <returns>Found view or null</returns>
        IElement FindView(IObject value, string viewname);

        /// <summary>
        /// Creates an object for a reflective sequence by parsing each object and returning the formview
        /// showing the properties and extents
        /// </summary>
        /// <param name="sequence">Sequence to be used</param>
        /// <returns>Created form object</returns>
        IElement CreateView(IReflectiveSequence sequence);

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