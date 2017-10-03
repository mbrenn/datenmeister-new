using System;

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
}