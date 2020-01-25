﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Lists;
using DatenMeister.WPF.Windows;
// ReSharper disable IdentifierTypo

namespace DatenMeister.WPF.Navigation
{
    public class NavigateToItemConfig
    {
        /// <summary>
        /// Initializes a new instance
        /// </summary>
        public NavigateToItemConfig()
        {
            DetailElement = InMemoryObject.CreateEmpty();
        }

        /// <summary>
        /// Initializes a new instance of the NavigateToItemConfig
        /// </summary>
        /// <param name="detailElement">The element to which it will be navigated</param>
        public NavigateToItemConfig(IObject detailElement)
        {
            DetailElement = detailElement;
        }
        
        /// <summary>
        /// Gets or sets the detail element that shall be shown
        /// </summary>
        public IObject DetailElement { get; set; }

        /// <summary>
        /// Gets or sets the container for the detail element which can be used to
        /// delete it
        /// </summary>
        public IReflectiveCollection? DetailElementContainer { get; set; }

        /// <summary>
        /// Gets or sets the title for the dialog or window
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the action that shall be performed after the view has been created
        /// </summary>
        public Action<DetailFormControl>? AfterCreatedFunction { get; set; }

        /// <summary>
        /// If the detail element is empty, then the metaclass can be set, so a new element will be created which is derived
        /// by the metaclass.
        /// </summary>
        public IElement? MetaClass { get; set; }

        /// <summary>
        /// If the detail element is empty, then the element will be added upon the given container element by the <c>ContainerProperty</c>.
        /// If ContainerProperty is empty, then the user will be asked which property of the parent shall be used for adding
        /// </summary>
        public IObject? ContainerElement { get; set; }

        /// <summary>
        /// Gets or sets the property of the containerelement to which the element will be added
        /// </summary>
        public string ContainerProperty { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the form definition to be used to create the item
        /// </summary>
        public FormDefinition? Form { get; set; }
    }

    public class NavigatorForItems
    {
        /// <summary>
        /// Navigates to the detail window
        /// </summary>
        /// <param name="window">Window which is the owner for the detail window</param>
        /// <param name="element">Element to be shown</param>
        /// <param name="afterCreated">This method will be called after the instance of DetailFormControl was created. It can be used to hook upon event in DetailFormControl</param>
        /// <param name="title">Title of the element to be shown</param>
        /// <returns>The navigation being used to control the view</returns>
        public static async Task<NavigateToElementDetailResult?> NavigateToElementDetailView(
            INavigationHost window,
            IObject element,
            Action<DetailFormControl>? afterCreated = null,
            string? title = null)
            =>
                await NavigateToElementDetailViewAsync(
                    window,
                    new NavigateToItemConfig(element)
                    {
                        Title = title ?? string.Empty,
                        AfterCreatedFunction = afterCreated
                    });


        /// <summary>
        /// Performs asynchronous navigation to detail element
        /// </summary>
        /// <param name="window">Window to be used as navigation host</param>
        /// <param name="navigateToItemConfig">Configuration for navigation</param>
        /// <returns>The task providing the result</returns>
        public static async Task<NavigateToElementDetailResult?> NavigateToElementDetailViewAsync(
            INavigationHost window,
            NavigateToItemConfig navigateToItemConfig)
            =>
                await Navigator.CreateDetailWindow(window, navigateToItemConfig);


        /// <summary>
        /// Navigates to show the items in an extent
        /// </summary>
        /// <param name="window">Window to be used</param>
        /// <param name="workspaceId">Id of the workspace</param>
        /// <param name="extentUrl">Url of the extent to be shown</param>
        /// <returns>The navigation support</returns>
        public static async Task<NavigateToElementDetailResult?> NavigateToItemsInExtent(
            INavigationHost window,
            string workspaceId,
            string extentUrl)
        {
            return await window.NavigateTo(() =>
                    new ItemsInExtentList {WorkspaceId = workspaceId, ExtentUrl = extentUrl},
                NavigationMode.List);
        }

        /// <summary>
        /// Navigates to a plain item list with all the given items
        /// in the collection. A meta class is also given to create the appropriate form for these
        /// items
        /// </summary>
        /// <param name="window">Window to be used</param>
        /// <param name="collection">Collection of items</param>
        /// <param name="metaClassForForm">Metaclass being used for the form</param>
        /// <returns>The task for navigation support</returns>
        public static async Task<NavigateToElementDetailResult?> NavigateToItems(
            INavigationHost window,
            IReflectiveCollection collection,
            IElement metaClassForForm)
        {
            return await window.NavigateTo(() =>
                {
                    var formCreator = GiveMe.Scope.Resolve<FormCreator>();
                    var usedForm = formCreator.CreateListFormForMetaClass(metaClassForForm, CreationMode.ByMetaClass);

                    var viewExtensions = new List<ViewExtension>();
                    viewExtensions.Add(ViewExtensionHelper.GetCreateButtonForMetaClass(
                        window,
                        metaClassForForm,
                        collection));

                    var control = new ItemListViewControl();
                    control.SetContent(collection, usedForm,
                        viewExtensions);
                    return control;
                },
                NavigationMode.List);
        }

        /// <summary>
        /// Navigates to a plain item list with all the given items
        /// in the collection. A meta class is also given to create the appropriate form for these
        /// items.
        /// A simple ListFormWindow will be created
        /// </summary>
        /// <param name="collection">Collection of items</param>
        /// <param name="metaClassForForm">Metaclass being used for the form</param>
        /// <returns>The task for navigation support</returns>
        public static async Task<NavigateToElementDetailResult?> NavigateToItems(
            IReflectiveCollection collection,
            IElement metaClassForForm)
        {
            var window = new ListFormWindow();
            window.Show();
            return await NavigateToItems(
                window,
                collection,
                metaClassForForm);
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
            IElement? metaclass)
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
        /// <param name="parentProperty">The property on which the new element will be attached to the parent property</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToNewItemForItem(
            INavigationHost window,
            IObject element,
            IElement? metaclass,
            string parentProperty = "")
            =>
                NavigateToNewItemForItem(
                    window,
                    new NavigateToItemConfig(element)
                    {
                        MetaClass = metaclass,
                        ContainerProperty = parentProperty
                    });

        /// <summary>
        /// Creates a new item and adds the item to the given container element.
        /// The property to which the container will be determined is set by the Container Property
        /// If the property is not set, then the user will be asked about which property to be set
        /// </summary>
        /// <param name="window">Navigation Host being used</param>
        /// <param name="config">Configuration for the user</param>
        /// <returns>The control element</returns>
        public static IControlNavigationNewItem NavigateToNewItemForItem(
            INavigationHost window,
            NavigateToItemConfig config)
        {
            var containerElement2 = config.ContainerElement
                                    ?? throw new InvalidOperationException("containerElement == null");
            
            var result = new ControlNavigation();
            if (config.MetaClass == null)
            {
                var createableTypes = new CreatableTypeNavigator();
                createableTypes.Closed += (x, y)
                    => CreateElementItself(createableTypes.SelectedType);

                var extent = containerElement2.GetExtentOf();
                if (extent != null)
                {
                    _ = createableTypes.NavigateToSelectCreateableType(window, extent);
                }
            }
            else
            {
                CreateElementItself(config.MetaClass);
            }

            // Creates a element by the given metaclass. If parent property is not defined, the parent property
            // may be chosen by the user
            async void CreateElementItself(IElement? selectedMetaclass)
            {
                
                var factory = new MofFactory(containerElement2);
                var newElement = factory.create(selectedMetaclass);
                var typeWorkspace = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace();

                // Creates the dialog
                var detailControlView = await NavigateToElementDetailView(
                    window,
                    newElement,
                    form =>
                        form.ViewDefined += (x, y) =>
                        {
                            // Gets the view definition
                            var fields = y.View
                                .get<IReflectiveSequence>(_FormAndFields._DetailForm.field);
                            var formFactory = new MofFactory(fields);
                            var containerProperty = config.ContainerProperty;
                            var containerElement = config.ContainerElement;

                            if (containerProperty == null && containerElement != null) // ParentProperty is not given, so user gives property
                            {
                                // Parent property is already given by function call
                                var dropField = formFactory.Create<_FormAndFields>(typeWorkspace, f => f.__DropDownFieldData);

                                //dropField.set(_FormAndFields._DropDownFieldData.fieldType, DropDownFieldData.FieldType);
                                dropField.set(_FormAndFields._DropDownFieldData.name, "ParentProperty");
                                dropField.set(_FormAndFields._DropDownFieldData.title, "Parent Property");
                                dropField.set(_FormAndFields._DropDownFieldData.isAttached, true);

                                var list = new List<object>();
                                var properties = ObjectHelper.GetPropertyNames(containerElement)
                                    .OrderBy(z => z).Distinct();
                                foreach (var property in properties)
                                {
                                    var valuePair = formFactory.Create<_FormAndFields>(typeWorkspace, f => f.__ValuePair);
                                    valuePair.set(_FormAndFields._ValuePair.name, property);
                                    valuePair.set(_FormAndFields._ValuePair.value, property);
                                    list.Add(valuePair);
                                }

                                dropField.set(_FormAndFields._DropDownFieldData.values, list);
                                fields.add(0, dropField);

                                // Adds the line to separate it from the other side
                                var lineField = formFactory.Create<_FormAndFields>(typeWorkspace, f => f.__SeparatorLineFieldData);
                                fields.add(1, lineField);
                            }
                        },
                    "New Item");

                if (detailControlView != null && detailControlView.Result == NavigationResult.Saved)
                {
                    var attachedElement = detailControlView.AttachedElement ??
                        throw new InvalidOperationException("attachedElement == null");
                    var selectedProperty = config.ContainerProperty
                                           ?? attachedElement.getOrDefault<string>("ParentProperty");

                    if (string.IsNullOrEmpty(selectedProperty))
                    {
                        MessageBox.Show("Property to which the new item will be added is not set.");
                        return;
                    }

                    var detailElement = detailControlView.DetailElement ??
                                        throw new InvalidOperationException("DetailElement == null");

                    var containerElement = config.ContainerElement;
                    containerElement?.AddCollectionItem(selectedProperty, detailElement);

                    // Adds the element to the dialog
                    // collection.add(newElement);
                    result.OnNewItemCreated(new NewItemEventArgs(newElement));
                    result.OnClosed();
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Extent which will be used for the factory to create the new item.
        /// The item itself will NOT be added to the extent
        /// </param>
        /// <param name="metaclass">Metaclass, whose instance will be created</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToCreateNewItem(
            INavigationHost window,
            IExtent extent,
            IElement? metaclass)
        {
            var result = new ControlNavigation();
            var factory = new MofFactory(extent);

            if (metaclass == null)
            {
                var createableTypes = new CreatableTypeNavigator();
                createableTypes.Closed += (x, y)
                    => CreateElementItself(createableTypes.SelectedType);

                _ = createableTypes.NavigateToSelectCreateableType(window, extent);
            }
            else
            {
                CreateElementItself(metaclass);
            }

            // Defines the class itself that is used to create the elements
            async void CreateElementItself(IElement? selectedMetaClass)
            {
                var newElement = factory.create(selectedMetaClass);
                var detailControlView = await NavigateToElementDetailView(window, newElement, title: "New Item");

                if (detailControlView != null && detailControlView.Result == NavigationResult.Saved)
                {
                    result.OnNewItemCreated(new NewItemEventArgs(newElement));
                    result.OnClosed();
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a new item for the given extent being located in the workspace and adds it to the given item
        /// </summary>
        /// <param name="window">Navigation extent being used to open up the new dialog</param>
        /// <param name="extent">Extent which will be used for the factory to create the new item.
        /// </param>
        /// <param name="metaclass">Metaclass, whose instance will be created</param>
        /// <returns>The control element that can be used to receive events from the dialog</returns>
        public static IControlNavigationNewItem NavigateToCreateNewItemInExtent(
            INavigationHost window,
            IExtent extent,
            IElement metaclass)
        {
            var result = NavigateToCreateNewItem(window, extent, metaclass);
            result.NewItemCreated += (x, y) => extent.elements().add(y.NewItem);
            return result;
        }

        /// <summary>
        /// Creates a new item for the given object and adds it to the properties of the item
        /// If the property is not a reflective collection, a reflective collection will be created
        /// </summary>
        /// <param name="window">Navigation extent of window</param>
        /// <param name="host">Element which will receive the object</param>
        /// <param name="property">Property to which the result will be added</param>
        /// <param name="metaClass">Metaclass that is created </param>
        /// <returns>The navigation being used for the control</returns>
        public static IControlNavigationNewItem NavigateToNewItemForPropertyCollection(
            INavigationHost window,
            IObject host,
            string property,
            IElement metaClass)
        {
            var reflectiveCollection = host.get<IReflectiveCollection>(property);
            return NavigateToNewItemForCollection(
                window,
                reflectiveCollection,
                metaClass);
        }

        /// <summary>
        /// Creates a new item for the given extent and reflective collection by metaclass
        /// </summary>
        /// <param name="window">Navigation extent of window</param>
        /// <param name="collection">Extent to which the item will be added</param>
        /// <param name="metaclass">Metaclass to be added</param>
        /// <returns>The navigation being used for control</returns>
        public static IControlNavigationNewItem NavigateToNewItemForCollection(
            INavigationHost window,
            IReflectiveCollection collection,
            IElement? metaclass)
        {
            var result = new ControlNavigation();
            var factory = new MofFactory(collection);

            if (metaclass == null)
            {
                var createableTypes = new CreatableTypeNavigator();
                createableTypes.Closed += (x, y) => CreateElementItself(createableTypes.SelectedType);

                _ = createableTypes.NavigateToSelectCreateableType(window, collection.GetAssociatedExtent());
            }
            else
            {
                CreateElementItself(metaclass);
            }

            async void CreateElementItself(IElement? selectedMetaClass)
            {
                var newElement = factory.create(selectedMetaClass);

                var detailControlView = await NavigateToElementDetailView(window, newElement, title: "New Item");

                if (detailControlView != null && detailControlView.Result == NavigationResult.Saved)
                {
                    collection.add(newElement);
                    result.OnNewItemCreated(new NewItemEventArgs(newElement));
                    result.OnClosed();
                }
            }

            return result;
        }
    }
}
