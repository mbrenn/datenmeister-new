﻿using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    /// <summary>
    /// Implements a reference field which is shown the currently selected instance and allows the user to select 
    /// another instance to set the appropriate property
    /// </summary>
    public class ReferenceField : IDetailField
    {
        public UIElement CreateElement(IObject value, IElement fieldData, DetailFormControl detailForm, ref FieldFlags fieldFlags)
        {
            var panel = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1.0, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1.0, GridUnitType.Auto)},
                    new ColumnDefinition {Width = new GridLength(1.0, GridUnitType.Auto)}
                }
            };

            var fieldName = fieldData.get(_FormAndFields._FieldData.name).ToString();
            var foundItem = value.getOrDefault(fieldName) as IElement;

            var itemText = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            UpdateTextOfTextBlock(foundItem, itemText);

            var openButten = new Button {Content = "Open"};
            openButten.Click += (sender, args) =>
            {
                if (!(value.getOrDefault(fieldName) is IElement itemToOpen))
                {
                    MessageBox.Show("No item selected");
                }
                else
                {
                    NavigatorForItems.NavigateToElementDetailView(
                        detailForm.NavigationHost,
                        itemToOpen);
                }
            };

            var selectButton = new Button { Content = "Select" };
            selectButton.Click += (sender, args) =>
            {
                var selectedItem =  NavigatorForDialogs.Locate(
                    detailForm.NavigationHost,
                    (value as IHasExtent)?.Extent);
                if (selectedItem != null)
                {
                    value.set(fieldName, selectedItem);
                    UpdateTextOfTextBlock(selectedItem, itemText);
                }
            };

            // Adds the ui elements
            Grid.SetColumn(openButten, 1);
            Grid.SetColumn(selectButton, 2);
            panel.Children.Add(itemText);
            panel.Children.Add(openButten);
            panel.Children.Add(selectButton);
            return panel;
        }

        private static void UpdateTextOfTextBlock(IObject foundItem, TextBlock itemText)
        {
            if (foundItem == null)
            {
                itemText.Text = "No item";
                itemText.FontStyle = FontStyles.Italic;
            }
            else
            {
                itemText.Text = foundItem.ToString();
                itemText.FontStyle = new FontStyle();
            }
        }
    }
}