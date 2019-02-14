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
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Forms.Base.ViewExtensions;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Detail.Fields
{
    public class SubElementsField : IDetailField
    {
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
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            
            CreatePanelElement(value, fieldData, detailForm, panel);

            fieldFlags.CanBeFocused = true;

            return panel;
        }

        public void CallSetAction(IObject element)
        {
        }

        /// <summary>
        /// Creates the content for the panel element dependent on the item and its values
        /// </summary>
        /// <param name="value">Item to be shown</param>
        /// <param name="fieldData">Field Data being used to define the layout and used property</param>
        /// <param name="detailForm"></param>
        /// <param name="panel"></param>
        private void CreatePanelElement(IObject value, IElement fieldData, DetailFormControl detailForm, StackPanel panel)
        {
            var name = fieldData.getOrDefault<string>(_FormAndFields._FieldData.name);
            var valueOfElement = value.getOrDefault<IReflectiveCollection>(name);
            var form = fieldData.getOrDefault<IElement>(_FormAndFields._SubElementFieldData.form);

            if (valueOfElement == null)
            {
                var emptyText = new TextBlock
                {
                    Text = "No list"
                };
                panel.Children.Add(emptyText);

                // If user clicks on the button, an empty reflective collection is created
                var button = new Button {Content = "Create empty list"};
                button.Click += (x, y) =>
                {
                    var emptyList = new List<object>();
                    value.set(name, emptyList);
                    panel.Children.Clear();
                    CreatePanelElement(value, fieldData, detailForm, panel);
                };

                panel.Children.Add(button);
            }
            else
            {
                var listViewControl = new ItemListViewControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    MaxHeight = 500,
                    Width = 650,
                    NavigationHost = detailForm.NavigationHost
                };

                // Checks, whether a form is given
                if (form == null)
                {
                    // otherwise, we have to automatically create a form
                    var formFinder = GiveMe.Scope.Resolve<ViewFinderImpl>();
                    form = formFinder.CreateView(valueOfElement);
                }

                var viewExtensions = new List<ViewExtension>();
                viewExtensions.Add(
                    new RowItemButtonDefinition(
                        "Edit",
                        (guest, item) => NavigatorForItems.NavigateToElementDetailView(detailForm.NavigationHost, item),
                        ItemListViewControl.ButtonPosition.Before));
                listViewControl.SetContent(valueOfElement, form, viewExtensions);

                panel.Children.Add(listViewControl);
            }

            // Gets the buttons for specific types
            if (fieldData?.getOrDefault<IReflectiveCollection>(_FormAndFields._SubElementFieldData.defaultTypesForNewElements) is IReflectiveCollection defaultTypesForNewItems)
            {
                foreach (var type in defaultTypesForNewItems.OfType<IElement>())
                {
                    var typeName = type.get(_UML._CommonStructure._NamedElement.name);
                    var button = new Button {Content =  $"New {typeName}"};
                    button.Click += (a, b) =>
                    {
                        var elements = NavigatorForItems.NavigateToCreateNewItem(detailForm.NavigationHost, (value as MofObject)?.CreatedByExtent, type);
                        elements.NewItemCreated += (x, y) =>
                        {
                            if (value.GetOrDefault(name) is IReflectiveCollection propertyCollection)
                            {
                                propertyCollection.add(y.NewItem);
                            }
                            else
                            {
                                value.set(name, new List<object> { y.NewItem });
                            }

                            panel.Children.Clear();
                            CreatePanelElement(value, fieldData, detailForm, panel);
                        };
                    };

                    panel.Children.Add(button);
                }
            }

            // If user clicks on the button, an empty reflective collection is created
            var createItemButton = new Button { Content = "Add item" };
            createItemButton.Click += (x, y) =>
            {
                var result = NavigatorForItems.NavigateToCreateNewItem(
                    detailForm.NavigationHost,
                    (value as MofObject)?.CreatedByExtent,
                    null);

                result.NewItemCreated += (a, b) =>
                {
                    if (value.GetOrDefault(name) is IReflectiveCollection propertyCollection)
                    {
                        propertyCollection.add(b.NewItem);
                    }
                    else
                    {
                        value.set(name, new List<object> { b.NewItem });
                    }

                    panel.Children.Clear();
                    CreatePanelElement(value, fieldData, detailForm, panel);
                };
            };

            panel.Children.Add(createItemButton);
        }
    }
}