using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms;
using DatenMeister.Integration.DotNet;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Fields
{
    public class SubElementsField : IDetailField
    {
        private DockPanel? _panel;
        private INavigationHost? _navigationHost;
        private IObject? _element;
        private IElement? _fieldData;
        private string _propertyName = string.Empty;
        private ItemListViewControl? _listViewControl;
        private bool _includeSpecializationsForDefaultTypes;
        private FieldParameter? _fieldFlags;

        /// <summary>
        /// Creates the element
        /// </summary>
        /// <param name="value">Value to be shown</param>
        /// <param name="fieldData">Field of type</param>
        /// <param name="detailForm">Detail form control</param>
        /// <param name="fieldFlags"></param>
        /// <returns>The created UI Element</returns>
        public UIElement CreateElement(
            IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            _fieldFlags = fieldFlags;
            _element = value;
            _fieldData = fieldData;
            _navigationHost = detailForm.NavigationHost;
            if (_navigationHost == null) throw new InvalidOperationException("detailform.NavigationHost is null");

            _propertyName = _fieldData.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            _panel = new DockPanel();

            RefreshPanelElement();

            fieldFlags.CanBeFocused = true;

            return _panel;
        }

        public void CallSetAction(IObject element)
        {
        }

        /// <summary>
        /// Creates the content for the panel element dependent on the item and its values
        /// </summary>
        private void RefreshPanelElement()
        {
            if (_panel == null)
                throw new InvalidOperationException("_panel == null");
            if (_element == null)
                throw new InvalidOperationException("_element == null");
            if (_navigationHost == null)
                throw new InvalidOperationException("_navigationHost == null");
            if (_fieldData == null)
                throw new InvalidOperationException("_fieldData == null");
            
            // First of all, clear the panel
            _panel.Children.Clear();

            // Get the required information
            var valueOfElement = _element.getOrDefault<IReflectiveCollection>(_propertyName);
            var form = _fieldData.getOrDefault<IObject>(_DatenMeister._Forms._SubElementFieldData.form);
            var isReadOnly = _fieldData.getOrDefault<bool>(_DatenMeister._Forms._SubElementFieldData.isReadOnly)
                || _fieldFlags?.IsReadOnly == true;

            // Check whether specialized classes shall be included
            _includeSpecializationsForDefaultTypes =
                !_fieldData.isSet(_DatenMeister._Forms._SubElementFieldData.includeSpecializationsForDefaultTypes)
                || _fieldData.getOrDefault<bool>(
                    _DatenMeister._Forms._SubElementFieldData.includeSpecializationsForDefaultTypes);

            valueOfElement ??= _element.get<IReflectiveCollection>(_propertyName);
            var valueCount = valueOfElement.Count();

            _listViewControl = new ItemListViewControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = Math.Min(400, 200 + 20 * valueCount),
                MinWidth = 650,
                NavigationHost = _navigationHost
            };

            // Checks, whether a form is given
            if (form == null)
            {
                // otherwise, we have to automatically create a form
                var formsLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                IElement? propertyType = null;
                var property = ClassifierMethods.GetPropertyOfClassifier(_element, _propertyName);
                if (property != null)
                {
                    propertyType = PropertyMethods.GetPropertyType(property);
                }

                // The form will be created by evaluating the name of the property
                // and the type of the property
                form = formsLogic.GetListFormForElementsProperty(
                           _element, 
                           _propertyName, 
                           propertyType) ??
                       throw new InvalidOperationException("Form could not be created");
            }

            IncludePropertiesTypeIntoDefaultTypes(form);

            var viewExtensions =
                isReadOnly
                    ? new List<ViewExtension>()
                    : new List<ViewExtension>
                    {
                        new RowItemButtonDefinition(
                            "Edit",
                            async (guest, item) =>
                                await NavigatorForItems.NavigateToElementDetailView(_navigationHost, item),
                            ItemListViewControl.ButtonPosition.Before)
                    };

            form.set(
                _DatenMeister._Forms._ListForm.inhibitNewItems,
                _fieldData.getOrDefault<bool>(_DatenMeister._Forms._SubElementFieldData.allowOnlyExistingElements));

            _listViewControl.SetContent(valueOfElement, form, viewExtensions);

            if (!isReadOnly)
            {
                CreateManipulationButtons(valueOfElement);
            }
            
            _panel.Children.Add(_listViewControl);
        }

        /// <summary>
        /// Tries to figure out the default type of the property within the element
        /// </summary>
        /// <param name="form">Form to be evaluated</param>
        private void IncludePropertiesTypeIntoDefaultTypes(IObject form)
        {
            var metaClass = (_element as IElement)?.getMetaClass();
            if (metaClass == null)
            {
                // The metaclass is not found
                return;
            }

            var property = ClassifierMethods.GetPropertyOfClassifier(metaClass, _propertyName);
            if (property == null)
            {
                // The property is not found
                return;
            }

            var propertyType = PropertyMethods.GetPropertyType(property);
            if (propertyType == null)
            {
                // The property type is not found
                return;
            }

            var defaultTypes =
                form.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.defaultTypesForNewElements);
            if (defaultTypes == null || defaultTypes.Any(x => x != null && x.Equals(propertyType)))
            {
                // Already included
                return;
            }

            var defaultType = MofFactory.Create(form, _DatenMeister.TheOne.Forms.__DefaultTypeForNewElement);
            defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass, propertyType);
            defaultType.set(_DatenMeister._Forms._DefaultTypeForNewElement.name, NamedElementMethods.GetName(propertyType));
            defaultTypes.add(defaultType);
        }

        /// <summary>
        /// Removes the item from the reflective collection and asks the user beforehand
        /// </summary>
        /// <param name="reflectiveCollection">Defines the reflective collection from which the item will be removed</param>
        /// <param name="items">The items to be removed</param>
        private static void RemoveItem(IReflectiveCollection reflectiveCollection, IList<IObject> items)
        {
            var names = items.Select(x=>NamedElementMethods.GetName(x)).Join(", ");
            if (MessageBox.Show(
                    "Are you sure to delete the item: " +
                    $"{names}?",
                    "Confirmation",
                    MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                foreach (var item in items)
                {
                    reflectiveCollection.remove(item);
                }
            }
        }

        /// <summary>
        /// Creates the buttons for the manipulations which include the
        /// moving of elements, the deletion of elements and the creation of new
        /// elements
        /// </summary>
        private void CreateManipulationButtons(IReflectiveCollection collection)
        {
            if (_listViewControl == null)
                throw new InvalidOperationException("_listViewControl == null");
            if (_panel == null)
                throw new InvalidOperationException("_panel == null");
            
            var stackPanel = new StackPanel {Orientation = Orientation.Vertical};

            if (collection is IReflectiveSequence reflectiveSequence)
            {
                var buttonUp = new Button {Content = "⭡"};
                buttonUp.Click += (x, y) =>
                {
                    var selectedItem = _listViewControl.GetSelectedItem();
                    if (selectedItem == null)
                    {
                        MessageBox.Show("No item is currently selected");
                        return;
                    }

                    reflectiveSequence.MoveElementUp(selectedItem);
                    _listViewControl.ForceRefresh();
                };

                SetStyle(buttonUp);

                var buttonDown = new Button {Content = "⭣"};
                buttonDown.Click += (x, y) =>
                {
                    var selectedItem = _listViewControl.GetSelectedItem();
                    if (selectedItem == null)
                    {
                        MessageBox.Show("No item is currently selected");
                        return;
                    }

                    reflectiveSequence.MoveElementDown(selectedItem);
                    _listViewControl.ForceRefresh();
                };
                
                SetStyle(buttonDown);
                
                stackPanel.Children.Add(buttonUp);
                stackPanel.Children.Add(buttonDown);
            }

            var buttonDelete = new Button {Content = "✗"};
            buttonDelete.Click += (x, y) =>
            {
                var selectedItems = _listViewControl.GetSelectedItems().ToList();
                if (selectedItems.Count == 0)
                {
                    MessageBox.Show("No item is currently selected");
                    return;
                }

                RemoveItem(collection, selectedItems);
                _listViewControl.ForceRefresh();
            };
            
            SetStyle(buttonDelete);
            stackPanel.Children.Add(buttonDelete);
            
            var allowOnlyExistingElements =
                _fieldData.getOrDefault<bool>(_DatenMeister._Forms._SubElementFieldData.allowOnlyExistingElements);
            if (!allowOnlyExistingElements)
            {
                var buttonNew = new CreateNewInstanceButton {Content = "N"};
                SetStyle(buttonNew);
                SetNewButton(buttonNew);
                stackPanel.Children.Add(buttonNew);
            }

            var buttonAttach = new Button {Content = "A"};
            var metaClass = _fieldData.getOrDefault<IElement>(_DatenMeister._Forms._SubElementFieldData.metaClass);
            
            buttonAttach.Click += async (x, y) =>
            {
                var typesWorkspace = GiveMe.Scope.WorkspaceLogic.GetTypesWorkspace();
                var internalTypes = typesWorkspace.FindExtent(WorkspaceNames.UriExtentInternalTypes);

                var result = await NavigatorForDialogs.Locate(
                    _navigationHost ?? throw new InvalidOperationException("Navigation Host"),
                    new NavigatorForDialogs.NavigatorForDialogConfiguration
                    {
                        Title = "Attach Element",
                        Description = "Select the element to be attached",
                        DefaultWorkspace = typesWorkspace,
                        DefaultExtent = internalTypes,
                        FilteredMetaClasses = metaClass == null ? null : new[] {metaClass}
                    });

                if (result != null)
                {
                    collection.add(result);
                    if (collection.Count() == 1)
                    {
                        // When the first item is added, then the complete form must be regenerated. 
                        RefreshPanelElement();
                    }
                    else
                    {
                        _listViewControl.ForceRefresh();    
                    }
                }
            };
            
            SetStyle(buttonAttach);
            stackPanel.Children.Add(buttonAttach);
                
            // Adds it to the stack panel
            DockPanel.SetDock(stackPanel, Dock.Right);
            stackPanel.VerticalAlignment = VerticalAlignment.Top;
            _panel.Children.Add(stackPanel);

            static void SetStyle(Control button)
            {
                button.Padding = new Thickness(10,3, 10, 3);
            }
        }

        /// <summary>
        /// Sets the style and the interaction for a button to become a button which
        /// can create new elements
        /// </summary>
        /// <param name="createItemButton"></param>
        private void SetNewButton(CreateNewInstanceButton createItemButton)
        {
            var navigationHost = _navigationHost ??
                                 throw new InvalidOperationException("_navigationHost == null");
            var panel = _panel ??
                        throw new InvalidOperationException("_panel == null");
            var element = _element as IElement ??
                          throw new InvalidOperationException("_element == null || as IElement");


            // Gets the default types
            var defaultTypesForNewItems = 
                _fieldData?.getOrDefault<IReflectiveCollection>(
                    _DatenMeister._Forms._SubElementFieldData.defaultTypesForNewElements);
            
            var typeList =
                defaultTypesForNewItems?.OfType<IElement>().Select(
                    innerType =>
                        innerType.isSet(_DatenMeister._Forms._DefaultTypeForNewElement.metaClass)
                            ? innerType.getOrDefault<IElement>(_DatenMeister._Forms._DefaultTypeForNewElement
                                .metaClass)
                            : innerType);

            createItemButton.SetDefaultTypesForCreation(typeList);

            // Sets the information whether the element will be generated as composite or not
            createItemButton.ToolTip =
                ObjectOperations.IsCompositeProperty(element, _propertyName)
                    ? "Composite"
                    : "Non-Composite";

            // Reacts upon the click
            // If user clicks on the button, an empty reflective collection is created
            createItemButton.TypeSelected += async (x, y) =>
            {
                var referencedExtent = (element as MofObject)?.ReferencedExtent;
                if (referencedExtent == null)
                    throw new InvalidOperationException("referencedExtent == null");

                var result =
                    await NavigatorForItems.NavigateToCreateNewItem(
                        navigationHost,
                        referencedExtent,
                        y.SelectedType);

                if (result?.IsNewObjectCreated == true 
                    && result.NewObject is IElement newElementAsIElement)
                {
                    ObjectOperations.AddItemReferenceToInstanceProperty(
                        element,
                        _propertyName,
                        newElementAsIElement,
                        true);

                    panel.Children.Clear();
                    RefreshPanelElement();
                }
            };
        }
    }
}