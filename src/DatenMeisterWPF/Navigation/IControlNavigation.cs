using System;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeisterWPF.Navigation
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
    /// This interface is created for every navigated element
    /// </summary>
    public interface IControlNavigationNewItem : IControlNavigation
    {
        /// <summary>
        /// This event will be called, when a new item is created
        /// </summary>
        event EventHandler<NewItemEventArgs> NewItemCreated;
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
}