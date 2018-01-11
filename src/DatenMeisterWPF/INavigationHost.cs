using System;
using System.Windows.Controls;
using Autofac;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF
{
    /// <summary>
    /// Defines the navigation host
    /// </summary>
    public interface INavigationHost
    {
        /// <summary>
        /// Called, if the host shall navigate to a certain element. 
        /// All user elements are removed after the call itself. 
        /// </summary>
        /// <param name="factoryMethod">Factory method creating the navigation element. </param>
        /// <param name="navigationMode">
        /// Type of the navigation mode. Can create subwindows or 
        /// other certain special modes. </param>
        /// <returns>The navigation information being used to receive certain events</returns>
        IControlNavigation NavigateTo(
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode);

        /// <summary>
        /// Adds a navigation element being used by the control. 
        /// </summary>
        /// <param name="name">Name of the element being used and shown to the user</param>
        /// <param name="clickMethod">Method that is called</param>
        /// <param name="imageName">Name of the image being shown</param>
        /// <param name="categoryName">Name of the category</param>
        void AddNavigationButton(
            string name,
            Action clickMethod,
            string imageName,
            string categoryName);

        /// <summary>
        /// Sets the focus of the navigation host, so user can click on it
        /// </summary>
        void SetFocus();
    }
}