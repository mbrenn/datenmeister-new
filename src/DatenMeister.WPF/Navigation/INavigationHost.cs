using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.WPF.Navigation
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
        Task<NavigateToElementDetailResult> NavigateTo(
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode);

        /// <summary>
        /// Called, when the navigation shall be rebuilt.
        /// This method can be called by guests, when the ribbons or other
        /// nav
        /// </summary>
        void RebuildNavigation();

        /// <summary>
        /// Sets the focus of the navigation host, so user can click on it
        /// </summary>
        void SetFocus();

        /// <summary>
        /// Gets the window containing the host
        /// </summary>
        /// <returns></returns>
        Window GetWindow();
    }

    public interface IDetailNavigationHost : INavigationHost
    {
        IObject DetailElement { get; }

        IElement AttachedElement { get; }

        IElement EffectiveForm { get; }

        /// <summary>
        /// Gets the detail element container
        /// </summary>
        IReflectiveCollection DetailElementContainer { get; }
    }
}