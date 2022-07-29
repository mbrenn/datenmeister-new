using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.TextTemplates;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Controls.GridControl;
using DatenMeister.WPF.Modules.ViewExtensions.Definition;
using DatenMeister.WPF.Modules.ViewExtensions.Definition.Buttons;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Base
{
    public class ItemListViewGridDm : ItemListViewGrid
    {
        /// <summary>
        /// Gets or sets the navigation guest
        /// </summary>
        public INavigationGuest? NavigationGuest { get; set; }
        
        /// <summary>
        /// Stores the current form
        /// </summary>
        private IElement? _currentForm;

        private List<IElement>? _currentElements;
        
        /// <summary>
        /// Sets the form and creates the depending columns out of the form definition
        /// </summary>
        /// <param name="formDefinition">Definition of the form </param>
        /// <param name="viewExtensions">Definitions for row items</param>
        public void SetForm(IElement formDefinition, IEnumerable<ViewExtension> viewExtensions)
        {
            _currentForm = formDefinition;
            ColumnDefinitions.Clear();
            
            var fields = _currentForm.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._TableForm.field);
            foreach (var field in fields.Cast<IElement>())
            {
                var fieldMetaClass = field.getMetaClass();
                
                var title = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.title) ?? string.Empty;

                if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
                {
                    title = "Metaclass";
                }

                ColumnDefinitions.Add(new GridTextBlockColumnDefinition
                {
                    Field = field,
                    Title = title
                });
            }

            foreach (var rowItemDefinition in viewExtensions.OfType<RowItemButtonDefinition>())
            {
                var definition = new GridButtonColumnDefinition
                {
                    Title = rowItemDefinition.Name,
                    OnPressed = rowItemDefinition.OnPressed
                };

                if (rowItemDefinition.Position == ItemListViewControl.ButtonPosition.Before)
                    ColumnDefinitions.Insert(0, definition);
                else
                    ColumnDefinitions.Add(definition);
            }

            InvalidateMeasure();
            InvalidateVisual();
        }

        public void SetContent(IReflectiveCollection collection)
        {
            _currentElements = collection.OfType<IElement>().ToList();
            
            FindSelectedElement();
            
            InvalidateMeasure();
            InvalidateVisual();
        }

        public override int DataRowCount => _currentElements?.Count ?? 0;

        /// <inheritdoc />
        public override object? GetDataOfRow(int dataRow)
        {
            return _currentElements?.ElementAtOrDefault(dataRow);
        }

        public override RowInstantiation? GetRowOfContent(int dataRow)
        {
            if (_currentElements == null)
                return null;

            var rowInstantiation = new RowInstantiation {Height = 25};

            var currentElement = _currentElements.ElementAtOrDefault(dataRow);
            if (currentElement == null)
                return null;
            
            foreach (var columnDefinition in ColumnDefinitions)
            {
                var cell = new CellInstantiation();
                switch (columnDefinition)
                {
                    case GridTextBlockColumnDefinition gridTextBlockColumnDefinition 
                        when gridTextBlockColumnDefinition.Field != null:
                    {
                        var value = GetTextFieldContent(currentElement, gridTextBlockColumnDefinition.Field);
                        cell.CellElement = new TextBlock
                        {
                            Text = value,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        break;
                    }
                    case GridButtonColumnDefinition gridButtonColumnDefinition:
                    {
                        if (NavigationGuest == null)
                            throw new InvalidOperationException(nameof(NavigationGuest) + "is null");
                        
                        var button = new Button
                        {
                            Content = gridButtonColumnDefinition.Title,
                            Padding = new Thickness(5,2,5,2)
                        };
                        button.Click += (x, y) =>
                        {
                            gridButtonColumnDefinition.OnPressed?.Invoke(NavigationGuest, currentElement);
                        };
                    
                        cell.CellElement = button;
                        break;
                    }
                }

                rowInstantiation.Cells.Add(cell);
            }

            return rowInstantiation;
        }

        private string GetTextFieldContent(IElement element, IElement field)
        {
            var value = GetValueOfElement(element, field);
            var isEnumeration =
                field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration);

            if (isEnumeration || DotNetHelper.IsEnumeration(value?.GetType()))
            {
                var result = new StringBuilder();
                var valueAsList = DotNetHelper.AsEnumeration(value);
                if (valueAsList != null)
                {
                    var elementCount = 0;
                    var nr = string.Empty;
                    foreach (var valueElement in valueAsList)
                    {
                        result.Append(nr + NamedElementMethods.GetName(valueElement));
                        nr = "\r\n";

                        elementCount++;
                        if (elementCount > 10)
                        {
                            result.Append("\r\n... (more)");
                            break;
                        }
                    }
                }

                value = result.ToString();
            }

            return value?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Gets the value of the element by the field
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <param name="field">Field being used for query</param>
        /// <returns>Returned element for the </returns>
        private object? GetValueOfElement(IObject element, IElement field)
        {
            var fieldMetaClass = field.getMetaClass();
            if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
            {
                var elementAsElement = element as IElement;
                var metaClass = elementAsElement?.getMetaClass();

                return metaClass == null
                    ? string.Empty
                    : NamedElementMethods.GetFullName(metaClass);
            }

            var name = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__EvalTextFieldData) == true)
            {
                var cellInformation = InMemoryObject.CreateEmpty();
                var defaultText = name != null ? element.getOrDefault<string>(name) : string.Empty;
                cellInformation.set("text", defaultText);

                var evalProperties = field.getOrDefault<string>(_DatenMeister._Forms._EvalTextFieldData.evalCellProperties);
                if (evalProperties != null)
                {
                    defaultText = TextTemplateEngine.Parse(
                        evalProperties,
                        new Dictionary<string, object>
                        {
                            ["i"] = element,
                            ["c"] = cellInformation
                        });
                }

                return cellInformation.isSet("text")
                    ? cellInformation.getOrDefault<string>("text")
                    : defaultText;
            }

            return element.isSet(name) ? element.get(name) : null;
        }
    }
}