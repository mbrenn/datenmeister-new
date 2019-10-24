using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
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
            if (_navigationHost == null) throw new InvalidOperationException("detailform.NavigationHost is null");

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

            valueOfElement ??= _element.GetAsReflectiveCollection(_propertyName);
            var valueCount = valueOfElement.Count();

            var listViewControl = new ItemListViewControl
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
                var viewLogic = GiveMe.Scope.Resolve<ViewLogic>();
                form = viewLogic.GetListFormForElementsProperty(_element, _propertyName);
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
            var createItemButton = new Button
                {Content = "Create new item", HorizontalAlignment = HorizontalAlignment.Right};
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