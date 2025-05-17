#nullable enable

using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Navigation
{
    /// <summary>
    /// This interface is created for every navigated element
    /// </summary>
    public interface IControlNavigation
    {
        /// <summary>
        /// Called, when the user closes the form or has finished the application
        /// </summary>
        event EventHandler Closed;
    }

    /// <summary>
    /// This interface is created for elements containing a save button.
    /// </summary>
    public interface IControlNavigationSaveItem : IControlNavigation
    {
        /// <summary>
        /// This event will be called, when the user has clicked upon the 'save' button and has not discarded the element
        /// </summary>
        event EventHandler<ItemEventArgs> Saved;
    }

    /// <summary>
    /// This interface is created for every navigated element
    /// </summary>
    public interface IControlNavigationNewObject : IControlNavigation
    {
        /// <summary>
        /// This event will be called, when a new item is created
        /// </summary>
        event EventHandler<NewItemEventArgs> NewObjectCreated;
        
        /// <summary>
        /// Gets of sets indicating whether a new item has been created. 
        /// </summary>
        bool IsNewObjectCreated { get; set; }
        
        /// <summary>
        /// Gets or sets the item that has been created
        /// </summary>
        IObject? NewObject { get; set; }
    }

    public class NewItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the NewItemEventArgs
        /// </summary>
        /// <param name="newItem"></param>
        public NewItemEventArgs(IObject newItem)
        {
            NewItem = newItem;
        }

        /// <summary>
        /// Gets or sets the newly created item
        /// </summary>
        public IObject NewItem { get; set; }
    }

    public class ItemEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the NewItemEventArgs
        /// </summary>
        /// <param name="item"></param>
        /// <param name="attachedItem">The attached item for additional properties</param>
        public ItemEventArgs(IObject item, IObject? attachedItem = null)
        {
            Item = item;
            AttachedItem = attachedItem;
        }

        /// <summary>
        /// Gets or sets the newly created item
        /// </summary>
        public IObject Item { get; set; }

        /// <summary>
        /// Gets or sets the attached item to the dialog
        /// </summary>
        public IObject? AttachedItem { get; set; }
    }
}