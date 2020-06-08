using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using BurnSystems;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Navigation;
using Button = System.Windows.Controls.Button;
using ContextMenu = System.Windows.Controls.ContextMenu;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using Orientation = System.Windows.Controls.Orientation;

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

            _propertyName = _fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            _panel = new DockPanel();

            CreatePanelElement();

            fieldFlags.CanBeFocused = true;

            return _panel;
        }

        public void CallSetAction(IObject element)
        {
        }

        /// <summary>
        /// Creates the content for the panel element dependent on the item and its values
        /// </summary>
        private void CreatePanelElement()
        {
            if (_panel == null)
                throw new InvalidOperationException("_panel == null");
            if (_element == null)
                throw new InvalidOperationException("_element == null");
            if (_navigationHost == null)
                throw new InvalidOperationException("_navigationHost == null");
            if (_fieldData == null)
                throw new InvalidOperationException("_fieldData == null");
            
            _panel.Children.Clear();

            var valueOfElement = _element.getOrDefault<IReflectiveCollection>(_propertyName);
            var form = _fieldData.getOrDefault<IObject>(_FormAndFields._SubElementFieldData.form);
            var isReadOnly = _fieldData.getOrDefault<bool>(_FormAndFields._SubElementFieldData.isReadOnly)
                || _fieldFlags?.IsReadOnly == true;
            _includeSpecializationsForDefaultTypes =
                _fieldData.getOrDefault<bool>(_FormAndFields._SubElementFieldData
                    .includeSpecializationsForDefaultTypes);

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
                var viewLogic = GiveMe.Scope.Resolve<FormsPlugin>();
                form = viewLogic.GetListFormForElementsProperty(_element, _propertyName) ??
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
                            ItemListViewControl.ButtonPosition.Before) /*,
                    
                    The DELETE button is not required anymore. It is in the stack

                new RowItemButtonDefinition(
                    "Delete",
                    (guest, item) => { RemoveItem(valueOfElement, item); })*/
                    };

            _listViewControl.SetContent(valueOfElement, form, viewExtensions);

            if (!isReadOnly)
            {
                // CreateNewItemButton();
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
                form.get<IReflectiveCollection>(_FormAndFields._ListForm.defaultTypesForNewElements);
            if (defaultTypes == null || defaultTypes.Any(x => x != null && x.Equals(propertyType)))
            {
                // Already included
                return;
            }

            defaultTypes.add(propertyType);
        }

        /// <summary>
        /// Removes the item from the reflective collection and asks the user beforehand
        /// </summary>
        /// <param name="reflectiveCollection">Defines the reflective collection from which the item will be removed</param>
        /// <param name="item">The item to be removed</param>
        private static void RemoveItem(IReflectiveCollection reflectiveCollection, IList<IObject> items)
        {
            var names = items.Select(NamedElementMethods.GetName).Join(", ");
            if (MessageBox.Show(
                    $"Are you sure to delete the item: " +
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
        /// Creates the buttons for the manipulations
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

            var buttonNew = new Button {Content = "N"};
            SetStyle(buttonNew);
            SetNewButton(buttonNew);
            
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
            stackPanel.Children.Add(buttonNew);
            
            DockPanel.SetDock(stackPanel, Dock.Right);
            stackPanel.VerticalAlignment = VerticalAlignment.Top;
            _panel.Children.Add(stackPanel);

            static void SetStyle(Control button)
            {
                button.Padding = new Thickness(10,3, 10, 3);
            }
        }
        
        /// <summary>
        /// Creates a new button 
        /// </summary>
        private void CreateNewItemButton()
        {
           _ = _panel ?? throw new InvalidOperationException("_panel == null");
            
            var createItemButton = new Button
                {Content = "Create new item", HorizontalAlignment = HorizontalAlignment.Right};
            
            SetNewButton(createItemButton);            

            DockPanel.SetDock(createItemButton, Dock.Bottom);
            _panel.Children.Add(createItemButton);
        }
        
        /// <summary>
        /// Sets the style and the interaction for a button to become a button which
        /// can create new elements
        /// </summary>
        /// <param name="createItemButton"></param>
        private void SetNewButton(Button createItemButton)
        {
            var navigationHost = _navigationHost ??
                throw new InvalidOperationException("_navigationHost == null");
            var panel = _panel ??
                        throw new InvalidOperationException("_panel == null");
            var element = _element ??
                        throw new InvalidOperationException("_element == null");
            
            // Create the button for the items
            var listItems = new List<Tuple<string, Action>>
            {
                new Tuple<string, Action>(
                    "Select Type...",
                    async () =>
                    {
                        var referencedExtent = (element as MofObject)?.ReferencedExtent;
                        if (referencedExtent == null)
                            throw new InvalidOperationException("referencedExtent == null");

                        var result = await NavigatorForItems.NavigateToCreateNewItem(
                            navigationHost,
                            referencedExtent,
                            null);
                        
                        if (result?.IsNewObjectCreated == true && result.NewObject != null)
                        {
                            var propertyCollection = element.getOrDefault<IReflectiveCollection>(_propertyName); 
                            if (propertyCollection != null)
                            {
                                propertyCollection.add(result.NewObject);
                            }
                            else
                            {
                                element.set(_propertyName, new List<object> {result.NewObject});
                            }

                            panel.Children.Clear();
                            CreatePanelElement();
                        };
                    })
            };

            var getAllSpecializations = _includeSpecializationsForDefaultTypes;
            // Gets the buttons for specific types
            if (_fieldData?.getOrDefault<IReflectiveCollection>(_FormAndFields._SubElementFieldData
                .defaultTypesForNewElements) is { } defaultTypesForNewItems)
            {
                IEnumerable<IElement> specializedTypes;

                if (getAllSpecializations)
                {
                    specializedTypes =
                        (from type in defaultTypesForNewItems.OfType<IElement>()
                            from newSpecializationType in ClassifierMethods.GetSpecializations(type)
                            select newSpecializationType).Distinct();
                }
                else
                {
                    specializedTypes = defaultTypesForNewItems.OfType<IElement>();
                }

                listItems.AddRange(
                    from x in specializedTypes
                    select CreateButtonForType(x));
            }

            // If user clicks on the button, an empty reflective collection is created
            createItemButton.Click += (x, y) =>
            {
                var menu = new ContextMenu();
                var menuItems = new List<MenuItem>();

                foreach (var item in listItems)
                {
                    var menuItem = new MenuItem
                    {
                        Header = item.Item1
                    };
                    menuItem.Click += (a, b) => item.Item2();
                    menuItems.Add(menuItem);
                }

                menu.ItemsSource = menuItems;
                menu.PlacementTarget = createItemButton;
                menu.IsOpen = true;
            };
        }
        

        /// <summary>
        /// Creates a button for a certain type and add it to the panel
        /// </summary>
        /// <param name="type">Type which shall be added</param>
        private Tuple<string, Action> CreateButtonForType(IElement type)
        {
            if (_element == null) throw new InvalidOperationException("_element == null");
            if (_panel == null) throw new InvalidOperationException("_panel == null");
            if (_navigationHost == null) throw new InvalidOperationException("_navigationHost == null");
            
            var typeName = type.get(_UML._CommonStructure._NamedElement.name);

            var result = new Tuple<string, Action>(
                $"New {typeName}",
                async () =>
                {
                    var referencedExtent = (_element as MofObject)?.ReferencedExtent
                                           ?? throw new InvalidOperationException("referencedExtent == null");

                    var elements =
                        await NavigatorForItems.NavigateToCreateNewItem(
                            _navigationHost, referencedExtent, type);
                    if (elements?.IsNewObjectCreated == true && elements.NewObject != null)
                    {
                        var propertyCollection = _element.getOrDefault<IReflectiveCollection>(_propertyName); 
                        if (propertyCollection != null)
                        {
                            propertyCollection.add(elements.NewObject);
                        }
                        else
                        {
                            _element.set(_propertyName, new List<object> {elements.NewObject});
                        }

                        _panel.Children.Clear();
                        CreatePanelElement();
                    };
                });

            return result;
        }
    }
}