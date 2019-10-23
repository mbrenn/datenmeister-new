using System.Net;
using System.Threading.Tasks;
using System.Windows.Automation;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Lists;

namespace DatenMeister.WPF.Navigation
{
    public static class NavigatorForExtents
    {
        /// <summary>
        /// Navigates to an extent list
        /// </summary>
        /// <param name="window">Root window being used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns>The navigation being used to control the view</returns>
        public static Task<NavigateToElementDetailResult> NavigateToExtentList(INavigationHost window, string workspaceId)
        {
            return window.NavigateTo(
                () => new ExtentList {WorkspaceId = workspaceId},
                NavigationMode.List);
        }

        /// <summary>
        /// Opens the extent as
        /// </summary>
        /// <param name="navigationHost">Host for navigation being to be used</param>
        /// <param name="extentUrl">Url of the extent to be shown</param>
        /// <returns>Navigation to be used</returns>
        public static Task<NavigateToElementDetailResult> OpenDetailOfExtent(INavigationHost navigationHost, string extentUrl)
        {
            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var uri = WorkspaceNames.ExtentManagementExtentUri + "#" + WebUtility.UrlEncode(extentUrl);
            return NavigatorForItems.NavigateToElementDetailView(
                navigationHost,
                workspaceLogic.FindItem(uri));
        }

        /// <summary>
        /// Opens the extent as
        /// </summary>
        /// <param name="navigationHost">Host for navigation being to be used</param>
        /// <param name="extent">Url of the extent to be shown</param>
        /// <returns>Navigation to be used</returns>
        public static Task<NavigateToElementDetailResult> OpenPropertiesOfExtent(INavigationHost navigationHost, IExtent extent)
        {
            if (extent is MofExtent mofExtent)
            {
                return NavigatorForItems.NavigateToElementDetailView(
                    navigationHost,
                    mofExtent.GetMetaObject());
            }

            return NavigatorForItems.NavigateToElementDetailView(
                navigationHost,
                new ExtentPropertyObject(extent));
        }
    }
}