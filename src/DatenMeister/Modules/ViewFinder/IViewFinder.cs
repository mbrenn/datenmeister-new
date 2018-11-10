using System.Collections.Generic;
using DatenMeister.Core.EMOF.Implementation;
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
        /// <param name="metaClass">Meta class of the elements that are shown in the current list</param>
        /// <returns>Found view or null</returns>
        IElement FindListView(IUriExtent extent, IElement metaClass = null);

        /// <summary>
        /// Finds the view for a specific object in a detail view
        /// </summary>
        /// <param name="value">Value for whom the object shall be created</param>
        /// <returns>Found view or null</returns>
        IElement FindDetailView(IObject value);

        /// <summary>
        /// Finds the list view of all sub elements of the given items
        /// </summary>
        /// <param name="value">Object, whose sub items shall be parsed</param>
        /// <param name="metaClass">Meta class of the elements that are shown in the current list</param>
        /// <returns>Found view or null, if none found</returns>
        IElement FindListViewFor(IObject value, IElement metaClass = null);

        /// <summary>
        /// Creates an object for a reflective sequence by parsing each object and returning the formview
        /// showing the properties and extents
        /// </summary>
        /// <param name="sequence">Sequence to be used</param>
        /// <returns>Created form object</returns>
        IElement CreateView(IReflectiveCollection sequence);

        /// <summary>
        /// Creates an object for an element by parsing the properties of the element
        /// </summary>
        /// <param name="element">Element to be used</param>
        /// <returns>Created form object</returns>
        IElement CreateView(IObject element);

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