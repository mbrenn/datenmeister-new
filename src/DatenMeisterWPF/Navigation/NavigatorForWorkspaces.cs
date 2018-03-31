using DatenMeisterWPF.Forms.Lists;

namespace DatenMeisterWPF.Navigation
{
    public static class NavigatorForWorkspaces
    {
        /// <summary>
        /// Navigates to the workspaces
        /// </summary>
        /// <param name="window">Windows to be used</param>
        /// <returns>The navigation to control the view</returns>
        public static IControlNavigation NavigateToWorkspaces(INavigationHost window)
        {
            return window.NavigateTo(
                () =>
                {
                    var workspaceControl = new WorkspaceList {IsTreeVisible = true};
                    return workspaceControl;
                },
                NavigationMode.List);
        }
    }
}