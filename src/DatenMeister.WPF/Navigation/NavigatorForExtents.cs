using System;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Integration;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Runtime.ExtentStorage.Validators;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Lists;
using MessageBox = System.Windows.MessageBox;

namespace DatenMeister.WPF.Navigation
{
    public static class NavigatorForExtents
    {
        /// <summary>
        /// Stores the logger being used in navigator for extents
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(NavigatorForExtents));

        /// <summary>
        /// Navigates to an extent list
        /// </summary>
        /// <param name="window">Root window being used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns>The navigation being used to control the view</returns>
        public static Task<NavigateToElementDetailResult?>? NavigateToExtentList(
            INavigationHost window,
            string workspaceId)
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
        public static async Task<NavigateToElementDetailResult?> OpenDetailOfExtent(INavigationHost navigationHost, string extentUrl)
        {
            var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
            var uri = WorkspaceNames.ExtentManagementExtentUri + "#" + WebUtility.UrlEncode(extentUrl);
            var foundItem = workspaceLogic.FindItem(uri);
            if (foundItem == null)
            {
                MessageBox.Show($"No item found at {extentUrl}");
                return null;
            }

            return await NavigatorForItems.NavigateToElementDetailView(
                navigationHost,
                foundItem);
        }

        /// <summary>
        /// Opens the extent as
        /// </summary>
        /// <param name="navigationHost">Host for navigation being to be used</param>
        /// <param name="extent">Url of the extent to be shown</param>
        /// <returns>Navigation to be used</returns>
        public async static Task<NavigateToElementDetailResult?> OpenPropertiesOfExtent(INavigationHost navigationHost, IExtent extent)
        {
            if (extent is MofExtent mofExtent)
            {
                return await NavigatorForItems.NavigateToElementDetailView(
                    navigationHost,
                    mofExtent.GetMetaObject());
            }

            return await NavigatorForItems.NavigateToElementDetailView(
                navigationHost,
                new ExtentPropertyObject(extent));
        }
        
        /// <summary>
        /// Opens the dialog in which the user can create a new xmi extent
        /// </summary>
        /// <param name="window">Window being used as an owner</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns></returns>
        public static async Task<NavigateToElementDetailResult> NavigateToNewXmiExtentDetailView(
            INavigationHost window,
            string workspaceId)
        {
            var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
            var form =
                viewLogic.GetInternalFormExtent().element(ManagementViewDefinitions.IdNewXmiDetailForm)
                ?? throw new InvalidOperationException(ManagementViewDefinitions.IdNewXmiDetailForm + " was not found");
            
            var formDefinition = new FormDefinition(form);
            formDefinition.Validators.Add( new NewXmiExtentValidator(
                GiveMe.Scope.WorkspaceLogic,
                workspaceId));
            
            var navigateToItemConfig = new NavigateToItemConfig()
            {
                Form = formDefinition
            };

            if (navigateToItemConfig.Form == null)
            {
                var text = $"The Form Definition in " +
                           $"{ManagementViewDefinitions.IdNewXmiDetailForm} " +
                           $"was not found";
                Logger.Error(text);
                
                MessageBox.Show(text);
            }

            var result = await NavigatorForItems.NavigateToElementDetailViewAsync(window, navigateToItemConfig);
            var detailElement = result?.DetailElement ?? 
                                throw new InvalidOperationException("detailElement == null");
            if (result.Result == NavigationResult.Saved)
            {
                var configuration = new XmiStorageConfiguration
                {
                    extentUri = detailElement.isSet("uri")
                        ? detailElement.getOrDefault<string>("uri")
                        : string.Empty,
                    filePath = detailElement.isSet("filepath")
                        ? detailElement.getOrDefault<string>("filepath")
                        : string.Empty,
                    workspaceId = workspaceId
                };

                var extentManager = GiveMe.Scope.Resolve<IExtentManager>();
                try
                {
                    extentManager.LoadExtent(configuration, ExtentCreationFlags.LoadOrCreate);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                    Logger.Error($"Error during creation of XMI extent: {exc}");
                }
            }

            return result;
        }
    }
}