using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Windows;

namespace DatenMeister.WPF.Navigation
{
    public enum NavigationMode
    {
        /// <summary>
        /// Shows a list of items
        /// </summary>
        List,

        /// <summary>
        /// Shows a detailed view of items
        /// </summary>
        Detail
    }

    /// <summary>
    /// Defines the possible categories for navigation
    /// </summary>
    public static class NavigationCategories
    {
        /// <summary>
        /// Category for all global and file issues
        /// </summary>
        public const string DatenMeister = "DatenMeister";

        public const string Type = "TypeManager";

        public const string Views = "Views";

        public const string Extents = "Extent";

        /// <summary>
        /// Defines the order of the ribbon for standard items.
        /// All other ribbons will be stored after that
        /// </summary>
        public static readonly string[] RibbonOrder =
        {
            DatenMeister,
            "Extent",
            "Item"
        };

        public const string DatenMeisterNavigation = "DatenMeister" + ".Navigation";

        public const string Form = "Form";
    }

    /// <summary>
    /// Enumeration of the user dialog result
    /// </summary>
    public enum NavigationResult
    {
        /// <summary>
        /// User has done nothing, since the navigation type does not offer such kind of userdialog feedback
        /// </summary>
        None,

        /// <summary>
        /// User has clicked on closed without saving
        /// </summary>
        Closed,

        /// <summary>
        /// User has clicked explicitly on the save button
        /// </summary>
        Saved,

        /// <summary>
        /// User has cancelled the activity
        /// </summary>
        Cancelled
    }

    public class NavigateToElementDetailResult
    {
        /// <summary>
        /// Stores the result of the navigation
        /// </summary>
        public NavigationResult? Result { get; set; }

        /// <summary>
        /// Stores the detail element that was shown
        /// </summary>
        public IObject? DetailElement { get; set; }

        /// <summary>
        /// Gets or sets the host for navigation
        /// </summary>
        public INavigationHost? NavigationHost { get; set; }

        /// <summary>
        /// Gets or sets the guest for navigation
        /// </summary>
        public INavigationGuest? NavigationGuest { get; set; }

        public IObject? AttachedElement { get; set; }
    }

    /// <summary>
    /// Defines the navigation method to allow a fluent navigation between instances
    /// </summary>
    public static class Navigator
    {
        /// <summary>
        /// Creates a detail window showing the element and the event
        /// </summary>
        /// <param name="navigationHost"></param>
        /// <param name="navigateToItemConfig"></param>
        /// <returns></returns>
        public static async Task<NavigateToElementDetailResult> CreateDetailWindow(
            INavigationHost navigationHost,
            NavigateToItemConfig navigateToItemConfig)
        {
            if (navigateToItemConfig == null) 
                throw new ArgumentNullException(nameof(navigateToItemConfig));
            
            var task = new TaskCompletionSource<NavigateToElementDetailResult>();
            var result = new NavigateToElementDetailResult();

            var detailFormWindow = new DetailFormWindow
            {
                Owner = navigationHost.GetWindow(),
                Title = navigateToItemConfig.Title ?? string.Empty
            };

            var resultingUserControl = detailFormWindow.SetContent(
                navigateToItemConfig.DetailElement,
                navigateToItemConfig.Form,
                navigateToItemConfig.ContainerCollection,
                navigateToItemConfig.AttachedElement);
            if (resultingUserControl is DetailFormControl detailFormControl)
            {
                if (navigateToItemConfig.PropertyValueChanged != null)
                {
                    detailFormControl.PropertyValueChanged +=
                        (x, y) =>
                        {
                            navigateToItemConfig.PropertyValueChanged(y);
                        };
                }
            }

            detailFormWindow.Cancelled += (x, y) =>
            {
                result.Result = NavigationResult.Cancelled;
                task.SetResult(result);
            };

            detailFormWindow.Saved += (x, y) =>
            {
                result.Result = NavigationResult.Saved;
                result.DetailElement = y.Item;
                task.SetResult(result);
            };

            detailFormWindow.SwitchToMinimumSize();

            if (detailFormWindow.MainControl is DetailFormControl mainControl)
            {
                navigateToItemConfig.AfterCreatedFunction?.Invoke(mainControl);
                detailFormWindow.Show();
            }

            return await task.Task;
        }

        /// <summary>
        /// Performs the default navigation by creating a new window
        /// </summary>
        /// <param name="parentWindow">Parent window to be used</param>
        /// <param name="factoryMethod">Factory method to be used to create the usercontrol</param>
        /// <param name="navigationMode">Mode of the navigation</param>
        /// <returns>Creates a new window which can be used by the user. </returns>
        public static async Task<NavigateToElementDetailResult?> NavigateByCreatingAWindow(
            Window parentWindow,
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode)
        {
            var task = new TaskCompletionSource<NavigateToElementDetailResult?>();
            var result = new NavigateToElementDetailResult();

            if (parentWindow == null)
                throw new InvalidOperationException("No parent window is not allowed");

            var userControl = factoryMethod();
            result.NavigationHost = parentWindow as INavigationHost;
            result.NavigationGuest = userControl as INavigationGuest;

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
                    listFormWindow.Closed += (x, y) =>
                    {
                        result.Result = NavigationResult.Closed;
                        task.SetResult(result);
                    };

                    SetPosition(listFormWindow, parentWindow);
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
                    detailFormWindow.Cancelled += (x, y) =>
                    {
                        result.Result = NavigationResult.Cancelled;
                        task.SetResult(result);
                        parentWindow.Focus();
                    };
                    detailFormWindow.Saved += (x, y) =>
                    {
                        result.Result = NavigationResult.Saved;
                        result.DetailElement = y.Item;
                        task.SetResult(result);
                    };

                    asDetailFormControl.UpdateForm();
                    SetPosition(detailFormWindow, parentWindow);

                    detailFormWindow.Show();
                    break;
                }
            }

            return await task.Task;
        }

        /// <summary>
        /// Sets the position of the new window dependent on the position of the old window and the used navigation mode
        /// </summary>
        /// <param name="newWindow">New window whose position need to be defined</param>
        /// <param name="parentWindow">The parent window, which is the source of creation</param>
        private static void SetPosition(Window newWindow, Window parentWindow)
        {
            if (parentWindow == null)
            {
                // No window, no cry
                return;
            }

            // Defines the window position dependent on the parentwindow
            /*if (navigationMode == NavigationMode.ForceNewWindow)
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
            }*/

            if (newWindow is DetailFormWindow detailFormWindow)
            {
                detailFormWindow.SwitchToMinimumSize();
            }
        }
    }
}