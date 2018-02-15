using System;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Lists;

namespace DatenMeisterWPF.Navigation
{
    public class NavigatorForItems
    {
        /// <summary>
        /// Navigates to the detail window
        /// </summary>
        /// <param name="window">Window which is the owner for the detail window</param>
        /// <param name="element">Element to be shown</param>
        /// <returns>The navigation being used to control the view</returns>
        public static IControlNavigation NavigateToElementDetailView(INavigationHost window, IObject element)
        {
            return window.NavigateTo(
                () =>
                {
                    var control = new DetailFormControl();
                    control.SetContent(element, null);
                    control.AllowNewProperties = true;
                    control.AddDefaultButtons();
                    return control;
                },
                NavigationMode.Detail);
        }

        /// <summary>
        /// Opens the dialog in which the user can create a new xmi extent
        /// </summary>
        /// <param name="window">Window being used as an owner</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <returns></returns>
        public static IControlNavigation NavigateToNewXmiExtentDetailView(
            INavigationHost window,
            string workspaceId)
        {
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            return window.NavigateTo(
                () =>
                {
                    var newXmiDetailForm = NamedElementMethods.GetByFullName(
                        viewLogic.GetViewExtent(),
                        ManagementViewDefinitions.PathNewXmiDetailForm);

                    var control = new DetailFormControl();
                    control.SetContent(null, newXmiDetailForm);
                    control.AddDefaultButtons("Create");
                    control.ElementSaved += (x, y) =>
                    {
                        var configuration = new XmiStorageConfiguration
                        {
                            ExtentUri = control.DetailElement.isSet("uri")
                                ? control.DetailElement.get("uri").ToString()
                                : String.Empty,
                            Path = control.DetailElement.isSet("filepath")
                                ? control.DetailElement.get("filepath").ToString()
                                : String.Empty,
                            Workspace = workspaceId
                        };

                        var extentManager = App.Scope.Resolve<IExtentManager>();
                        extentManager.LoadExtent(configuration, true);
                    };

                    return control;
                },
                NavigationMode.Detail);
        }

        /// <summary>
        /// Navigates to show the items in an extent
        /// </summary>
        /// <param name="window">Window to be used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent to be shown</param>
        /// <returns>The navigation support</returns>
        public static IControlNavigation NavigateToItemsInExtent(
            INavigationHost window,
            string workspaceId,
            string extentUrl)
        {
            return window.NavigateTo(() =>
                {
                    var control = new ItemsInExtentList();
                    control.SetContent(workspaceId, extentUrl);

                    return control;
                },
                NavigationMode.List);
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Object to whose property, the new element will be added
        /// </param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToNewItemForExtent(INavigationHost window, IExtent extent)
        {
            var result = new ControlNavigationNewItem();

            var createableTypes = new CreatableTypeNavigator();
            createableTypes.Closed += (x, y) =>
            {
                var detailResult =
                    NavigateToNewItemForCollection(window, extent.elements(), createableTypes.SelectedType);
                detailResult.Closed += (a, b) => result.OnClosed();
            };

            createableTypes.NavigateToSelectCreateableType(window, extent);

            return result;
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Object to whose property, the new element will be added
        /// </param>
        /// <param name="metaclass">Metaclass, whose instance will be created</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToNewItemForExtent(INavigationHost window, IExtent extent, IElement metaclass)
        {
            var detailResult =
                NavigateToNewItemForCollection(window, extent.elements(), metaclass);
            return detailResult;
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace. 
        /// On first step, the user will be asked to select the metaclass to be created. 
        /// On a second step, he can modify the content
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Object to whose property, the new element will be added
        /// </param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToCreateNewItem(INavigationHost window, IExtent extent)
        {
            var result = new ControlNavigationNewItem();
            var createableTypes = new CreatableTypeNavigator();
            createableTypes.Closed += (x, y) =>
            {
                var detailResult =
                    NavigateToCreateNewItem(window, extent, createableTypes.SelectedType);
                detailResult.Closed += (a, b) => result.OnClosed();
            };

            createableTypes.NavigateToSelectCreateableType(window, extent);

            return result;
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Object to whose property, the new element will be added
        /// </param>
        /// <param name="metaclass">Metaclass, whose instance will be created</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToCreateNewItem(INavigationHost window, IExtent extent, IElement metaclass)
        {
            var result = new ControlNavigationNewItem();
            var factory = new MofFactory(extent);
            var newElement = factory.create(metaclass);

            var detailControlView = NavigateToElementDetailView(window, newElement);
            detailControlView.Closed += (a, b) =>
            {
                result.OnNewItemCreated(new NewItemEventArgs(newElement));
                result.OnClosed();
            };

            return result;
        }

        /// <summary>
        /// Creates a new item for the given extent and reflective collection by metaclass 
        /// </summary>
        /// <param name="window">Navigation extent of window</param>
        /// <param name="collection">Extent to which the item will be added</param>
        /// <param name="metaClass">Metaclass to be added</param>
        /// <returns>The navigation being used for control</returns>
        public static  IControlNavigationNewItem NavigateToNewItemForCollection(
            INavigationHost window, 
            IReflectiveCollection collection,
            IElement metaClass)
        {
            var result = new ControlNavigationNewItem();
            var factory = new MofFactory(collection);
            var newElement = factory.create(metaClass);

            var detailControlView = NavigateToElementDetailView(window, newElement);
            detailControlView.Closed += (a, b) =>
            {
                collection.add(newElement);
                result.OnNewItemCreated(new NewItemEventArgs(newElement));
                result.OnClosed();
            };

            return result;
        }
    }
}
