using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Detail.Fields
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
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm,
            ref FieldFlags fieldFlags)
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical
            };
            
            CreatePanelElement(value, fieldData, detailForm, panel);

            return panel;
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
            var name = fieldData.getOrDefault(_FormAndFields._FieldData.name).ToString();
            var valueOfElement = value.getOrDefault(name) as IReflectiveSequence;
            var form = fieldData.getOrDefault(_FormAndFields._SubElementFieldData.form) as IElement;

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
                var listViewControl = new ListViewControl
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    MaxHeight = 500
                };
                
                listViewControl.AddRowItemButton(
                    "Open",
                    item => Navigator.NavigateToElementDetailView(detailForm.NavigationHost, item),
                    ListViewControl.ButtonPosition.Before);
                listViewControl.SetContent(valueOfElement, form);
                panel.Children.Add(listViewControl);
            }

            // If user clicks on the button, an empty reflective collection is created
            var createItemButton = new Button { Content = "Add item" };
            createItemButton.Click += (x, y) =>
            {
                var result = Navigator.NavigateToNewItem(detailForm.NavigationHost, ((IHasExtent) value).Extent);
                result.NewItemCreated += (a, b) =>
                {
                    if (value.getOrDefault(name) is IReflectiveCollection propertyCollection)
                    {
                        propertyCollection.add(b.NewItem);
                    }
                    else
                    {
                        value.set(name, new List<object> { b.NewItem });
                    }
                };
            };
            panel.Children.Add(createItemButton);
        }
    }
}