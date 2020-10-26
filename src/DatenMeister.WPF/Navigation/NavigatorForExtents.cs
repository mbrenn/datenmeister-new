using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Models.ManagementProviders;
using DatenMeister.Models.Runtime;
using DatenMeister.Modules.Forms;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.ManagementProviders.View;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage;
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
            var uri = WorkspaceNames.UriExtentWorkspaces + "#" + WebUtility.UrlEncode(extentUrl);
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
        public static async Task<NavigateToElementDetailResult?> OpenPropertiesOfExtent(INavigationHost navigationHost, IExtent extent)
        {
            if (extent is MofExtent mofExtent)
            {
                var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                var managementWorkspace = workspaceLogic.GetManagementWorkspace();
                var resolvedForm = managementWorkspace.ResolveElement(
                    $"{WorkspaceNames.UriExtentInternalForm}#Workspace.ExtentDetail", ResolveType.NoMetaWorkspaces, false);
                var extentSettings = GiveMe.Scope.ScopeStorage.Get<ExtentSettings>();

                // Look for the checkbox item list for the possible extent types
                if (resolvedForm != null)
                {
                    // Add the options for the extent types
                    var foundExtentType =
                        resolvedForm.GetByPropertyFromCollection(
                            _DatenMeister._Forms._ListForm.field, 
                            _DatenMeister._Forms._Form.name,
                            _ManagementProvider._Extent.extentType).FirstOrDefault();
                    if (foundExtentType == null)
                    {
                        Logger.Error($"Found Form #ExtentPropertyDetailForm did not find the field ${_ManagementProvider._Extent.extentType}");
                    }
                    else
                    {
                        var list = new List<IElement>();
                        var factory = new MofFactory(foundExtentType);
                        foreach (var setting in extentSettings.extentTypeSettings)
                        {
                            var pair = factory.create(_DatenMeister.TheOne.Forms.__ValuePair);
                            pair.set(_DatenMeister._Forms._ValuePair.name, setting.name);
                            pair.set(_DatenMeister._Forms._ValuePair.value, setting.name);

                            list.Add(pair);
                        }

                        foundExtentType.set(_DatenMeister._Forms._CheckboxListTaggingFieldData.values, list);
                    }
                }

                // Gets the properties of the extent themselves
                var uri = 
                    WorkspaceNames.UriExtentWorkspaces + "#" +
                    WebUtility.UrlEncode(((IUriExtent) mofExtent).contextURI());
                var foundItem = workspaceLogic.FindItem(uri);
                if (foundItem == null)
                {
                    var message = $"The element '{uri}' was not found";
                    Logger.Error(message);
                    MessageBox.Show("The extent which was selected, was not found.");
                }
                else
                {
                    var config = new NavigateToItemConfig(foundItem)
                    {
                        Form = new FormDefinition(resolvedForm),
                        AttachedElement = foundItem,
                        Title = "Edit Extent Properties"
                    };
                    
                    return await NavigatorForItems.NavigateToElementDetailView(
                        navigationHost,
                        config);
                }
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
            var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
            var form =
                viewLogic.GetInternalFormExtent().element(ManagementViewDefinitions.IdNewXmiDetailForm)
                ?? throw new InvalidOperationException(ManagementViewDefinitions.IdNewXmiDetailForm + " was not found");
            
            var formDefinition = new FormDefinition(form);
            formDefinition.Validators.Add( new NewXmiExtentValidator(
                GiveMe.Scope.WorkspaceLogic,
                workspaceId));
            
            var navigateToItemConfig = new NavigateToItemConfig
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

            var result = await NavigatorForItems.NavigateToElementDetailView(window, navigateToItemConfig);
            var detailElement = result?.DetailElement ?? 
                                throw new InvalidOperationException("detailElement == null");
            if (result.Result == NavigationResult.Saved)
            {
                var uri = detailElement.isSet("uri")
                    ? detailElement.getOrDefault<string>("uri")
                    : string.Empty;

                var configuration =
                    InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__XmiStorageLoaderConfig);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.extentUri,
                    uri);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.filePath,
                    detailElement.isSet("filepath")
                        ? detailElement.getOrDefault<string>("filepath")
                        : string.Empty);
                configuration.set(_DatenMeister._ExtentLoaderConfigs._XmiStorageLoaderConfig.workspaceId,
                    workspaceId);

                var extentManager = GiveMe.Scope.Resolve<ExtentManager>();
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