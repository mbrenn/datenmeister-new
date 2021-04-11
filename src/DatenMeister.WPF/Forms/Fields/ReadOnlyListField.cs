#nullable enable

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Fields
{
    public class ReadOnlyListField : IDetailField
    {
        public UIElement CreateElement(
            IObject valueElement,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            var contentBlock = new Grid();
            contentBlock.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});
            contentBlock.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Auto});

            var name = fieldData.get<string>(_DatenMeister._Forms._FieldData.name);

            if (valueElement?.isSet(name) == true)
            {
                var value = valueElement.get(name);
                if (!DotNetHelper.IsOfEnumeration(value))
                {
                    value = new[] {value};
                }

                var inner = 0;
                if (!(value is IEnumerable<object> valueAsEnumeration))
                    throw new InvalidOperationException("Not an enumeration");
                
                foreach (var innerValue in valueAsEnumeration)
                {
                    contentBlock.RowDefinitions.Add(new RowDefinition());

                    // Creates the text
                    var innerTextBlock = innerValue != null
                        ? new TextBlock
                        {
                            Text = innerValue.ToString()
                        }
                        : new TextBlock
                        {
                            Text="null",
                            FontStyle = FontStyles.Italic
                        };

                    Grid.SetRow(innerTextBlock, inner);
                    Grid.SetColumn(innerTextBlock, 0);
                    contentBlock.Children.Add(innerTextBlock);

                    // Creates the button
                    if (innerValue is IElement asIElement)
                    {
                        var button = new Button {Content = "Edit", Padding = new Thickness(10,0,10,0)};
                        Grid.SetRow(button, inner);
                        Grid.SetColumn(button, 1);

                        button.Click += async (sender, args) =>
                            await NavigatorForItems.NavigateToElementDetailView(
                                detailForm.NavigationHost,
                                asIElement);

                        contentBlock.Children.Add(button);
                    }

                    inner++;
                }
            }

            return contentBlock;
        }

        public void CallSetAction(IObject element)
        {
            
        }
    }
}