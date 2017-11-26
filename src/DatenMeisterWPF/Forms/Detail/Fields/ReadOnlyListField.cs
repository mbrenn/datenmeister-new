using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeisterWPF.Forms.Base;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Detail.Fields
{
    public class ReadOnlyListField : IDetailField
    {
        public UIElement CreateElement(IElement valueElement, IElement fieldData, DetailFormControl detailForm)
        {
            var contentBlock = new Grid();
            contentBlock.ColumnDefinitions.Add(new ColumnDefinition());
            contentBlock.ColumnDefinitions.Add(new ColumnDefinition());

            var name = fieldData.get(_FormAndFields._FieldData.name).ToString();

            if (valueElement?.isSet(name) == true)
            {
                var value = valueElement.get(name);
                if (!DotNetHelper.IsOfEnumeration(value))
                {
                    value = new[] {value};
                }

                var inner = 0;
                var valueAsEnumeration = (IEnumerable<object>) value;
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
                        var button = new Button {Content = "Edit"};
                        Grid.SetRow(button, inner);
                        Grid.SetColumn(button, 1);

                        button.Click += (sender, args) =>
                            Navigator.TheNavigator.NavigateToElementDetailView(
                                detailForm.NavigationHost,
                                asIElement);

                        contentBlock.Children.Add(button);
                    }

                    inner++;
                }
            }

            return contentBlock;
        }
    }
}