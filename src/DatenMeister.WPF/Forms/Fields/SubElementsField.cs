using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Autofac;
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
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Forms.Base.ViewExtensions.Buttons;
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
        private DockPanel _panel;
        private INavigationHost _navigationHost;
        private IObject _element;
        private IElement _fieldData;
        private string _propertyName;
        private ItemListViewControl _listViewControl;

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
            _panel.Children.Clear();

            var valueOfElement = _element.getOrDefault<IReflectiveCollection>(_propertyName);
            var form = _fieldData.getOrDefault<IObject>(_FormAndFields._SubElementFieldData.form);
            var isReadOnly = _fieldData.getOrDefault<bool>(_FormAndFields._SubElementFieldData.isReadOnly);

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
                var viewLogic = GiveMe.Scope.Resolve<FormLogic>();
                form = viewLogic.GetListFormForElementsProperty(_element, _propertyName);
            }

            var viewExtensions = 
                isReadOnly ? 
                new List<ViewExtension>() :  
                new List<ViewExtension>
            {
                new RowItemButtonDefinition(
                    "Edit",
                    (guest, item) => NavigatorForItems.NavigateToElementDetailView(_navigationHost, item),
                    ItemListViewControl.ButtonPosition.Before)/*,
                    
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
        /// Removes the item from the reflective collection and asks the user beforehand
        /// </summary>
        /// <param name="reflectiveCollection"></param>
        /// <param name="item"></param>
        private static void RemoveItem(IReflectiveCollection reflectiveCollection, IObject item)
        {
            if (MessageBox.Show(
                    "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                MessageBoxResult.Yes)
            {
                reflectiveCollection.remove(item);
            }
        }

        /// <summary>
        /// Creates the buttons for the manipulations
        /// </summary>
        private void CreateManipulationButtons(IReflectiveCollection collection)
        {
            var stackPanel = new StackPanel {Orientation = Orientation.Vertical};
            var buttonUp = new Button {Content = "⭡"};
            SetStyle(buttonUp);
            
            var buttonDown = new Button {Content = "⭣"};
            SetStyle(buttonDown);
            
            var buttonNew = new Button {Content = "N"};
            SetStyle(buttonDown);
            SetNewButton(buttonNew);
            
            var buttonDelete = new Button {Content = "✗"};
            buttonDelete.Click += (x, y) =>
            {
                var selectedItem = _listViewControl.GetSelectedItem();
                if (selectedItem == null)
                {
                    MessageBox.Show("No item is currently selected");
                    return;
                }

                RemoveItem(collection, selectedItem);
            };
            
            SetStyle(buttonDelete);

            stackPanel.Children.Add(buttonUp);
            stackPanel.Children.Add(buttonDown);
            stackPanel.Children.Add(buttonDelete);
            stackPanel.Children.Add(buttonNew);
            
            DockPanel.SetDock(stackPanel, Dock.Right);
            stackPanel.VerticalAlignment = VerticalAlignment.Bottom;
            _panel.Children.Add(stackPanel);

            static void SetStyle(Button button)
            {
                button.Padding = new Thickness(10,3, 10, 3);
            }
        }
        
        /// <summary>
        /// Creates a new button 
        /// </summary>
        private void CreateNewItemButton()
        {
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
            // Create the button for the items
            var listItems = new List<Tuple<string, Action>>
            {
                new Tuple<string, Action>(
                    "Select Type",
                    () =>
                    {
                        var result = NavigatorForItems.NavigateToCreateNewItem(
                            _navigationHost,
                            (_element as MofObject)?.ReferencedExtent,
                            null);

                        result.NewItemCreated += (a, b) =>
                        {
                            if (_element.GetOrDefault(_propertyName) is IReflectiveCollection propertyCollection)
                            {
                                propertyCollection.add(b.NewItem);
                            }
                            else
                            {
                                _element.set(_propertyName, new List<object> {b.NewItem});
                            }

                            _panel.Children.Clear();
                            CreatePanelElement();
                        };
                    })
            };

            // Gets the buttons for specific types
            if (_fieldData?.getOrDefault<IReflectiveCollection>(_FormAndFields._SubElementFieldData
                .defaultTypesForNewElements) is { } defaultTypesForNewItems)
            {
                var specializedTypes =
                    (from type in defaultTypesForNewItems.OfType<IElement>()
                        from newSpecializationType in ClassifierMethods.GetSpecializations(type)
                        select newSpecializationType).Distinct();

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
            var typeName = type.get(_UML._CommonStructure._NamedElement.name);

            var result = new Tuple<string, Action>(
                $"New {typeName}",
                () =>
                {
                    var elements =
                        NavigatorForItems.NavigateToCreateNewItem(
                            _navigationHost,
                            (_element as MofObject)?.ReferencedExtent, type);
                    elements.NewItemCreated += (x, y) =>
                    {
                        if (_element.GetOrDefault(_propertyName) is IReflectiveCollection propertyCollection)
                        {
                            propertyCollection.add(y.NewItem);
                        }
                        else
                        {
                            _element.set(_propertyName, new List<object> {y.NewItem});
                        }

                        _panel.Children.Clear();
                        CreatePanelElement();
                    };
                });

            return result;
        }
    }
}