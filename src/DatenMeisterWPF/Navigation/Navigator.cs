using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Base.ViewExtensions;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Navigation
{
    public enum NavigationMode
    {
        /// <summary>
        /// Shows a list of items 
        /// </summary>
        List, 

        /// <summary>
        /// Shows a detailled view of items
        /// </summary>
        Detail,

        /// <summary>
        /// Forces a popup of the window which will be centered before hand. 
        /// </summary>
        ForceNewWindow
    }

    /// <summary>
    /// Defines the possible categories for navigation
    /// </summary>
    public static class NavigationCategories
    {
        /// <summary>
        /// Category for all global and file issues
        /// </summary>
        public static string File = "File";

        public static string Type = "Types";
    }

    /// <summary>
    /// Defines the navigation method to allow a fluent navigation between instances
    /// </summary>
    public static class Navigator
    {
        /// <summary>
        /// Performs the default navigation by creating a new window
        /// </summary>
        /// <param name="parentWindow">Parent window to be used</param>
        /// <param name="factoryMethod">Factory method to be used to create the usercontrol</param>
        /// <returns>Creates a new window which can be used by the user. </returns>
        public static IControlNavigation NavigateByCreatingAWindow(Window parentWindow, Func<UserControl> factoryMethod, NavigationMode navigationMode)
        {
            if (parentWindow == null)
            {
                throw new InvalidOperationException("No parent window is not allowed");
            }

            var result = new ControlNavigation();
            var userControl = factoryMethod();
            switch (userControl)
            {
                case null:
                    return null;
                case ItemListViewControl asListViewControl:
                {
                    var listFormWindow = new ListFormWindow
                    {
                        Owner = parentWindow,
                        MainViewSet =
                        {
                            Content = asListViewControl
                        },
                    };

                    asListViewControl.NavigationHost = listFormWindow;
                    listFormWindow.Closed += (x, y) => result.OnClosed();

                    SetPosition(listFormWindow, parentWindow, navigationMode);
                    break;
                }
                case DetailFormControl asDetailFormControl:
                {
                    var detailFormWindow = new DetailFormWindow
                    {
                        Owner = parentWindow
                    };
                    detailFormWindow.SetMainContent(asDetailFormControl);
                    asDetailFormControl.NavigationHost = detailFormWindow;
                    detailFormWindow.Show();
                    detailFormWindow.Closed += (x, y) =>
                    {
                        result.OnClosed();
                        parentWindow.Focus();
                    };
                    detailFormWindow.Saved += (x, y) => { result.OnSaved(y); };
                    SetPosition(detailFormWindow, parentWindow, navigationMode);
                    break;
                }
            }
            
            if (userControl is INavigationGuest guest)
            {
                var viewExtensions = guest.GetViewExtensions();
            }

            return result;
        }

        /// <summary>
        /// Sets the position of the new window dependent on the position of the old window and the used navigation mode
        /// </summary>
        /// <param name="newWindow">New window whose position need to be defined</param>
        /// <param name="parentWindow">The parent window, which is the source of creation</param>
        /// <param name="navigationMode">The used navigation mode</param>
        private static void SetPosition(Window newWindow, Window parentWindow, NavigationMode navigationMode)
        {
            if (parentWindow == null)
            {
                // No window, no cry
                return;
            }

            // Defines the window position dependent on the parentwindow
            if (navigationMode == NavigationMode.ForceNewWindow)
            {
                var deltaX = parentWindow.Width - newWindow.Width;
                var deltaY = parentWindow.Height - newWindow.Height;

                newWindow.Left = parentWindow.Left + deltaX / 2;
                newWindow.Top = parentWindow.Top + deltaY / 2;
            }
            else
            {
                // Checks the position of the window
                newWindow.Left = parentWindow.Left + 20;
                newWindow.Top = parentWindow.Top + 20;
            }
        }
    }
}