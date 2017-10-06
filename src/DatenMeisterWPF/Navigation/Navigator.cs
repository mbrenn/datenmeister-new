using System;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Navigation
{
    /// <summary>
    /// Defines the navigation method to allow a fluent navigation between instances
    /// </summary>
    public class Navigator
    {
        /// <summary>
        /// Stores the navigation for the given application
        /// </summary>
        public static Navigator TheNavigator { get; } = new Navigator();

        /// <summary>
        /// Performs a navigation from the current window to the given User Control.
        /// The user control is stored as an action method to allow
        /// </summary>
        /// <param name="root">Root window to be used</param>
        /// <param name="factoryMethod">The factory method</param>
        public IControlNavigation NavigateTo(
            Window root,
            Func<UserControl> factoryMethod)
        {
            var result = new ControlNavigation();
            var userControl = factoryMethod();
            switch (userControl)
            {
                case null:
                    return null;
                case ListViewControl asListViewControl:
                {
                    var window = new ListFormWindow
                    {
                        Owner = root,
                        MainViewSet =
                        {
                            Content = asListViewControl
                        }
                    };

                    window.Show();
                    window.Closed += (x, y) => result.OnClosed();
                    break;
                }
                case DetailFormControl asDetailFormControl:
                {
                    var window = new DetailFormWindow
                    {
                        Owner = root,
                        MainContent =
                        {
                            Content = asDetailFormControl
                        }
                    };

                    window.Show();
                    window.Closed += (x, y) => result.OnClosed();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Navigates to the detail window
        /// </summary>
        /// <param name="window">Window which is the owner for the detail window</param>
        /// <param name="scope">Scope to be used</param>
        /// <param name="element">Element to be shown</param>
        public void NavigateToDetailView(Window window, IDatenMeisterScope scope, IElement element)
        {
            NavigateTo(
                window, () =>
                {
                    var control = new DetailFormControl();
                    control.SetContent(scope, element, null);
                    control.AddDefaultButtons();
                    return control;
                });
        }
    }
}