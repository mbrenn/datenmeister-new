using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Lists;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Navigation
{
    public enum NavigationMode
    {
        List, 
        Detail,
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
        public static IControlNavigation NavigateByCreatingAWindow(Window parentWindow, Func<UserControl> factoryMethod)
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
                        Owner = parentWindow,
                        MainViewSet =
                        {
                            Content = asListViewControl
                        }
                    };

                    window.Closed += (x, y) => result.OnClosed();
                    break;
                }
                case DetailFormControl asDetailFormControl:
                {
                    var window = new DetailFormWindow
                    {
                        Owner = parentWindow
                    };
                    window.SetMainContent(asDetailFormControl);

                    asDetailFormControl.NavigationHost = window;
                    window.Show();
                    window.Closed += (x, y) =>
                    {
                        result.OnClosed();
                        parentWindow.Focus();
                    };
                    break;
                }
            }

            if (userControl is INavigationGuest guest)
            {
                guest.PrepareNavigation();
            }
            (userControl as INavigationGuest)?.PrepareNavigation();

            return result;
        }
    }
}