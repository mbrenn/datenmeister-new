#nullable enable

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für ListFormWindow.xaml
    /// </summary>
    public partial class ListFormWindow : Window, INavigationHost
    {
        /// <summary>
        /// Initializes a new instance of ListFormWindow class.
        /// </summary>
        public ListFormWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigates to a new window
        /// </summary>
        /// <param name="factoryMethod">Factory method being used</param>
        /// <param name="navigationMode">Navigation mode</param>
        /// <returns></returns>
        public async Task<NavigateToElementDetailResult?> NavigateTo(
            Func<UserControl> factoryMethod,
            NavigationMode navigationMode)
        {
            if (navigationMode == NavigationMode.Detail)
            {
                return await Navigator.NavigateByCreatingAWindow(this, factoryMethod, navigationMode);
            }
            else
            {
                MainViewSet.Content = factoryMethod();
                if (MainViewSet.Content is INavigationGuest navigationGuest)
                {
                    navigationGuest.NavigationHost = this;
                }

                var task = new TaskCompletionSource<NavigateToElementDetailResult?>();
                task.SetResult(new NavigateToElementDetailResult()
                {
                    NavigationGuest = MainViewSet as INavigationGuest,
                    NavigationHost = this,
                    Result = NavigationResult.None
                });
                return await task.Task;
            }
        }

        public void AddNavigationButton(string name, Action clickMethod, string imageName, string categoryName)
        {
        }

        public void SetFocus()
        {
            MainViewSet?.Focus();
        }

        public void RebuildNavigation()
        {
        }

        /// <inheritdoc />
        public Window GetWindow()
            => this;
    }
}
