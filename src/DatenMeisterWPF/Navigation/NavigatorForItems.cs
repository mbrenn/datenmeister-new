using System;
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
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Forms.Lists;

namespace DatenMeisterWPF.Navigation
{
    public static class NavigatorForItems
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
        /// <param name="extent">Extent, whose meta extents will be used to retrieve the fitting type</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToNewItem(INavigationHost window, IExtent extent)
        {
            var viewLogic = App.Scope.Resolve<ViewLogic>();
            var viewDefinitions = App.Scope.Resolve<ManagementViewDefinitions>();
            var extentFunctions = App.Scope.Resolve<ExtentFunctions>();

            var result = new ControlNavigationNewItem();
            var navigationControl = window.NavigateTo(
                () =>
                {
                    var element = InMemoryObject.CreateEmpty().SetReferencedExtent(viewLogic.GetViewExtent());
                    var items = extentFunctions.GetCreatableTypes(extent);
                    var formPathToType = viewDefinitions.GetFindTypeForm(items.CreatableTypes);

                    var control = new DetailFormControl();
                    control.SetContent(element, formPathToType);
                    control.AddDefaultButtons("Create");
                    control.ElementSaved += (x, y) =>
                    {
                        if (control.DetailElement.getOrDefault("selectedType") is IElement metaClass)
                        {
                            var detailResult = NavigateToCreateNewItem(window, extent, metaClass);
                            detailResult.NewItemCreated += (a, b) =>
                            {
                                result.OnNewItemCreated(b);
                            };
                            detailResult.Closed += (a, b) => result.OnClosed();
                        }
                    };

                    return control;
                },
                NavigationMode.Detail);

            navigationControl.Closed += (x, y) => result.OnClosed();
            return result;
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
            var controlFunction = NavigateToNewItem(window, extent);
            controlFunction.NewItemCreated += (x, y) => {
                extent.elements().add(y.NewItem);
            };

            return controlFunction;
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Object to whose property, the new element will be added
        /// </param>
        /// <param name="metaClass">Meta class whose instance will be created</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToNewItemForExtent(INavigationHost window, IExtent extent, IElement metaClass)
        {
            var controlFunction = NavigateToCreateNewItem(window, extent, metaClass);
            controlFunction.NewItemCreated += (x, y) => {
                extent.elements().add(y.NewItem);
            };

            return controlFunction;
        }

        /// <summary>
        /// Creates a new item for the given extent and reflective collection by metaclass 
        /// </summary>
        /// <param name="window">Navigation extent of window</param>
        /// <param name="extent">Extent being used to create the item in a factory</param>
        /// <param name="metaClass">Metaclass to be added</param>
        /// <returns>The navigation being used for control</returns>
        public static  IControlNavigationNewItem NavigateToCreateNewItem(
            INavigationHost window, 
            IExtent extent,
            IElement metaClass)
        {
            var result = new ControlNavigationNewItem();
            var factory = new MofFactory(extent);
            var newElement = factory.create(metaClass);

            var detailControlView = NavigateToElementDetailView(window, newElement);
            detailControlView.Closed += (a, b) =>
            {
                result.OnNewItemCreated(new NewItemEventArgs(newElement));
                result.OnClosed();
            };

            return result;
        }
    }
}