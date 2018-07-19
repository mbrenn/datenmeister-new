using System.Net;
using Autofac;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime.Workspaces;
using DatenMeisterWPF.Forms.Lists;

namespace DatenMeisterWPF.Navigation
{
    public static class NavigatorForExtents
    {
        /// <summary>
        /// Navigates to an extent list
        /// </summary>
        /// <param name="window">Root window being used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns>The navigation being used to control the view</returns>
        public static IControlNavigation NavigateToExtentList(INavigationHost window, string workspaceId)
        {
            return window.NavigateTo(
                () => new ExtentList {IsTreeVisible = true, WorkspaceId = workspaceId},
                NavigationMode.List);
        }

        /// <summary>
        /// Opens the extent as 
        /// </summary>
        /// <param name="navigationHost">Host for navigation being to be used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent to be shown</param>
        /// <returns>Navigation to be used</returns>
        public static IControlNavigation OpenExtent(INavigationHost navigationHost, string workspaceId, string extentUrl)
        {
            var workspaceLogic = App.Scope.Resolve<IWorkspaceLogic>();
            var uri = ExtentOfWorkspaces.WorkspaceUri + "#" + WebUtility.UrlEncode(extentUrl);
            return NavigatorForItems.NavigateToElementDetailView(
                navigationHost, 
                workspaceLogic.FindItem(uri));
        }
    }
}