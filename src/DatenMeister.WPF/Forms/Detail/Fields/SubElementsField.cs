using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Detail.Fields
{
    public class SubElementsField : IDetailField
    {
        private StackPanel _panel;
        private INavigationHost _navigationHost;
        private IObject _element;
        private IElement _fieldData;
        private string _propertyName;

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
            _propertyName = _fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            _panel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            
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
            IObject form = _fieldData.getOrDefault<IElement>(_FormAndFields._SubElementFieldData.form);

            valueOfElement = valueOfElement ?? _element.GetAsReflectiveCollection(_propertyName);
            var listViewControl = new ItemListViewControl
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Height = 400,
                MinWidth = 650,
                NavigationHost = _navigationHost
            };

            // Checks, whether a form is given
            if (form == null)
            {
                // otherwise, we have to automatically create a form
                var formFinder = GiveMe.Scope.Resolve<ViewFinderImpl>();
                form = formFinder.FindListFormForTreeItemDetailView(_element, _propertyName, null);
            }

            var viewExtensions = new List<ViewExtension>
            {
                new RowItemButtonDefinition(
                    "Edit",
                    (guest, item) => NavigatorForItems.NavigateToElementDetailView(_navigationHost, item),
                    ItemListViewControl.ButtonPosition.Before),

                new RowItemButtonDefinition(
                    "Delete",
                    (guest, item) =>
                    {
                        if (MessageBox.Show(
                                "Are you sure to delete the item?", "Confirmation", MessageBoxButton.YesNo) ==
                            MessageBoxResult.Yes)
                        {
                            valueOfElement.remove(item);
                        }
                    })
            };

            listViewControl.SetContent(valueOfElement, form, viewExtensions);

            _panel.Children.Add(listViewControl);

            var listItems = new List<Tuple<string, Action>>
            {
                new Tuple<string, Action>(
                    "Select object",
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
                .defaultTypesForNewElements) is IReflectiveCollection defaultTypesForNewItems)
            {
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    listItems.Add(CreateButtonForType(type));
                }
            }

            // If user clicks on the button, an empty reflective collection is created
            var createItemButton = new Button {Content = "Add item", HorizontalAlignment = HorizontalAlignment.Right};
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

            _panel.Children.Add(createItemButton);
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
                        NavigatorForItems.NavigateToCreateNewItem(_navigationHost,
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