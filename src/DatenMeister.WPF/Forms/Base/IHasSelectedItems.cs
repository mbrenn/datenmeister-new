using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Forms.Base
{
    /// <summary>
    /// Defines the interface that the user can selecte item
    /// </summary>
    public interface IHasSelectedItems
    {
        /// <summary>
        /// Gets the first or no selected item
        /// </summary>
        /// <returns>The selected item</returns>
        IObject? GetSelectedItem();

        /// <summary>
        /// Gets an enumeration of all selected items
        /// </summary>
        /// <returns>Enumeration of selected items</returns>
        IEnumerable<IObject> GetSelectedItems();
    }
}