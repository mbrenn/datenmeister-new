using System.Windows;
using Autofac;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Windows;

namespace DatenMeisterWPF.Navigation
{
    public static class NavigatorForDialogs
    {
        /// <summary>
        /// Opens the window in which the user can search for an item by a specific url
        /// </summary>
        /// <param name="window">Navigation host being used to open up the new dialog</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigation SearchByUrl(INavigationHost window)
        {
            var dlg = new QueryElementDialog {Owner = window as Window};
            if (dlg.ShowDialog() == true)
            {
                var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
                var element = workspaceLogic.FindItem(dlg.QueryUrl.Text);

                if (element == null)
                {
                    MessageBox.Show("Item does not exist.");
                    return null;
                }

                return NavigatorForItems.NavigateToElementDetailView(
                    window,
                    element);
            }

            return null;
        }

        public static IControlNavigation Locate(MainWindow mainWindow)
        {
            throw new System.NotImplementedException();
        }
    }
}