using System;
using System.Collections.Generic;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
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
        /// <param name="afterCreated">This method will be called after the instance of DetailFormControl was created. It can be used to hook upon event in DetailFormControl</param>
        /// <returns>The navigation being used to control the view</returns>
        public static IControlNavigationSaveItem NavigateToElementDetailView(
            INavigationHost window, 
            IObject element,
            Action<DetailFormControl> afterCreated = null)
        {
            return (IControlNavigationSaveItem) window.NavigateTo(
                () =>
                {
                    var control = new DetailFormControl
                    {
                        DetailElement = element,
                        AllowNewProperties = true
                    };

                    // calls the hook, so event handling can be introduced
                    afterCreated?.Invoke(control);

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
                        viewLogic.GetInternalViewExtent(),
                        ManagementViewDefinitions.PathNewXmiDetailForm);

                    var control = new DetailFormControl();
                    control.SetContent(null, newXmiDetailForm);
                    control.AddDefaultButtons("Create");
                    control.ElementSaved += (x, y) => 
                    {
                        var configuration = new XmiStorageConfiguration
                        {
                            extentUri = control.DetailElement.isSet("uri")
                                ? control.DetailElement.get("uri").ToString()
                                : string.Empty,
                            filePath = control.DetailElement.isSet("filepath")
                                ? control.DetailElement.get("filepath").ToString()
                                : string.Empty,
                            workspaceId = workspaceId
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
                    new ItemsInExtentList {WorkspaceId = workspaceId, ExtentUrl = extentUrl},
                NavigationMode.List);
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Object to whose property, the new element will be added
        /// </param>
        /// <param name="metaclass">Metaclass, whose instance will be created</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToNewItemForExtent(
            INavigationHost window,
            IExtent extent, 
            IElement metaclass)
        {
            var detailResult =
                NavigateToNewItemForCollection(window, extent.elements(), metaclass);

            return detailResult;
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="element">The item to which one of the properties will be aded
        /// </param>
        /// <param name="metaclass">Metaclass, whose instance will be created</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToNewItemForItem(
            INavigationHost window, 
            IObject element, 
            IElement metaclass)
        {
            var result = new ControlNavigation();
            if (metaclass == null)
            {
                var createableTypes = new CreatableTypeNavigator();
                createableTypes.Closed += (x, y) =>
                {
                    CreateElementItself(createableTypes.SelectedType);
                };

                createableTypes.NavigateToSelectCreateableType(window, element.GetExtentOf());
            }
            else
            {
                CreateElementItself(metaclass);
            }

            void CreateElementItself(IElement selectedMetaclass)
            {
                var factory = new MofFactory(element);
                var newElement = factory.create(selectedMetaclass);

                // Creates the dialog
                var detailControlView = NavigateToElementDetailView(
                    window,
                    newElement,
                    (form) =>
                    {
                        form.ViewDefined += (x, y) =>
                        {
                            // Gets the view definition
                            var fields = y.View
                                    .get<IReflectiveSequence>(_FormAndFields._Form.fields);
                            var formFactory = new MofFactory(fields);

                            var dropField = formFactory.Create<_FormAndFields>(f => f.__DropDownFieldData);
                            dropField.set(_FormAndFields._DropDownFieldData.fieldType, DropDownFieldData.FieldType);
                            dropField.set(_FormAndFields._DropDownFieldData.name, "ParentProperty");
                            dropField.set(_FormAndFields._DropDownFieldData.title, "Parent Property");
                            dropField.set(_FormAndFields._DropDownFieldData.isAttached, true);

                            var list = new List<object>();
                            var properties = ObjectHelper.GetPropertyNames(element);
                            foreach (var property in properties)
                            {
                                var valuePair = formFactory.Create<_FormAndFields>(f => f.__ValuePair);
                                valuePair.set(_FormAndFields._ValuePair.name, property);
                                valuePair.set(_FormAndFields._ValuePair.value, property);
                                list.Add(valuePair);
                            }

                            dropField.set(_FormAndFields._DropDownFieldData.values, list);
                            fields.add(0, dropField);
                        };
                    });

                detailControlView.Saved += (a, b) =>
                {
                    var selectedProperty = b.AttachedItem.getOrDefault<string>("ParentProperty");
                    element.AddCollectionItem(selectedProperty, b.Item);

                    // Adds the element to the dialog
                    // collection.add(newElement);
                    result.OnNewItemCreated(new NewItemEventArgs(newElement));
                    result.OnClosed();
                };
            }

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
        public static IControlNavigationNewItem NavigateToCreateNewItem(
            INavigationHost window, 
            IExtent extent, 
            IElement metaclass)
        {
            var result = new ControlNavigation();
            var factory = new MofFactory(extent);

            if (metaclass == null)
            {
                var createableTypes = new CreatableTypeNavigator();
                createableTypes.Closed += (x, y) =>
                {
                   CreateElementItself(createableTypes.SelectedType);
                };

                createableTypes.NavigateToSelectCreateableType(window, extent);
            }
            else
            {
                CreateElementItself(metaclass);
            }

            // Defines the class itself that is used to create the elements
            void CreateElementItself(IElement selectedMetaClass)
            {
                var newElement = factory.create(selectedMetaClass);
                var detailControlView = NavigateToElementDetailView(window, newElement);
                detailControlView.Closed += (a, b) =>
                {
                    result.OnNewItemCreated(new NewItemEventArgs(newElement));
                    result.OnClosed();
                };
            }

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
            IElement metaclass)
        {
            var result = new ControlNavigation();
            var factory = new MofFactory(collection);

            if (metaclass == null)
            {
                var createableTypes = new CreatableTypeNavigator();
                createableTypes.Closed += (x, y) =>
                {
                    CreateElementItself(createableTypes.SelectedType);
                };

                createableTypes.NavigateToSelectCreateableType(window, collection.GetAssociatedExtent());
            }
            else
            {
                CreateElementItself(metaclass);
            }

            void CreateElementItself(IElement selectedMetaClass)
            {
                var newElement = factory.create(selectedMetaClass);

                var detailControlView = NavigateToElementDetailView(window, newElement);
                detailControlView.Saved += (a, b) =>
                {
                    collection.add(newElement);
                    result.OnNewItemCreated(new NewItemEventArgs(newElement));
                    result.OnClosed();
                };
            }

            return result;
        }
    }
}
