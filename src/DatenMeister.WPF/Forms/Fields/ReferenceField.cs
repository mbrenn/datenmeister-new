#nullable enable

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Fields
{
    /// <summary>
    /// Implements a reference field which is shown the currently selected instance and allows the user to select
    /// another instance to set the appropriate property
    /// </summary>
    public class ReferenceField : IDetailField
    {
        /// <summary>
        /// Stores the name of the property of the field
        /// </summary>
        private string? _name;
        
        /// <summary>
        /// Defines the form control in which the element is hosted
        /// </summary>
        private DetailFormControl? _detailFormControl;
        
        /// <summary>
        /// The control element 
        /// </summary>
        private LocateElementControl? _control;
        
        /// <summary>
        /// The value that is modified
        /// </summary>
        private IObject? _value;

        public UIElement CreateElement(
            IObject value,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));
            
            var isInline = fieldData.getOrDefault<bool>(_FormAndFields._ReferenceFieldData.isSelectionInline);
            var isReadOnly = fieldData.getOrDefault<bool>(_FormAndFields._ReferenceFieldData.isReadOnly);
            _name = fieldData.get<string>(_FormAndFields._FieldData.name);
            _detailFormControl = detailForm;
            _value = value;

            if (_name == null)
            {
                return new TextBlock
                {
                    Text = "Bad configuration: _name == null"
                };
            }

            // Checks, whether the reference shall be included as an inline selection
            if (isInline)
            {
                // Defines the locate element control in which the user can select
                // workspace, extent and element
                _control = new LocateElementControl
                {
                    MinHeight = 400,
                    MaxHeight = 400,
                    MinWidth = 600
                };

                var filterMetaClasses =
                    fieldData.getOrDefault<IReflectiveCollection>(_FormAndFields._ReferenceFieldData.metaClassFilter);
                if (filterMetaClasses != null)
                {
                    _control.FilterMetaClasses = filterMetaClasses.OfType<IElement>();
                }

                var showAllChildren = fieldData.getOrDefault<bool>(_FormAndFields._ReferenceFieldData.showAllChildren);
                if (showAllChildren) _control.ShowAllChildren = true;

                var element = fieldData.getOrDefault<IElement>(_FormAndFields._ReferenceFieldData.defaultValue);
                if (element != null)
                {
                    _control.Select(element);
                }
                else
                {
                    var workspace = fieldData.getOrDefault<string>(_FormAndFields._ReferenceFieldData.defaultWorkspace);
                    var extent = fieldData.getOrDefault<string>(_FormAndFields._ReferenceFieldData.defaultExtentUri);

                    if (!string.IsNullOrEmpty(workspace) && !string.IsNullOrEmpty(extent))
                    {
                        var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                        workspaceLogic.RetrieveWorkspaceAndExtent(
                            workspace,
                            extent,
                            out var foundWorkspace,
                            out var foundExtent);
                        if (foundWorkspace != null && foundExtent != null)
                        {
                            _control.Select(foundWorkspace, foundExtent);
                        }
                    }
                    else if (!string.IsNullOrEmpty(workspace))
                    {
                        var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();
                        var foundWorkspace = workspaceLogic.GetWorkspace(workspace);
                        if (foundWorkspace != null)
                        {
                            _control.Select((IWorkspace) foundWorkspace);
                        }
                    }
                }

                fieldFlags.CanBeFocused = true;
                return _control;
            }

            var panel = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1.0, GridUnitType.Star)}, // Text field
                    new ColumnDefinition {Width = new GridLength(90.0, GridUnitType.Pixel)}, // Select button
                    new ColumnDefinition {Width = new GridLength(90.0, GridUnitType.Pixel)}, // Remove button
                }
            };

            var foundItem = value.getOrDefault<IElement>(_name);

            var itemText = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            UpdateTextOfTextBlock(foundItem, itemText);
            
            var selectButton = new Button
            {
                Content = "Select",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            
            selectButton.Click += (sender, args) =>
            {
                // TODO: Select the one, of the currently referenced field
                var selectedItem = NavigatorForDialogs.Locate(
                    detailForm.NavigationHost,
                    null /* workspace */,
                    (value as IHasExtent)?.Extent);
                
                if (selectedItem != null)
                {
                    value.set(_name, selectedItem);
                    UpdateTextOfTextBlock(selectedItem, itemText);
                }
            };

            var removeButton = new Button
            {
                Content = "Remove",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            
            removeButton.Click += (sender, args) => { value.unset(_name); };

            // Adds the ui elements
            Grid.SetColumn(selectButton, 1);
            Grid.SetColumn(removeButton, 2);
            panel.Children.Add(itemText);

            if (!isReadOnly)
            {
                panel.Children.Add(selectButton);
                panel.Children.Add(removeButton);
            }

            fieldFlags.CanBeFocused = true;
            return panel;
        }

        public void CallSetAction(IObject element)
        {
            if(_name == null) throw new InvalidOperationException("_name == null");
            
            var selectedElement = _control?.SelectedElement;
            if (selectedElement != null)
            {
                element.set(_name, selectedElement);
            }
        }

        /// <summary>
        /// Updates the text of the text block. 
        /// </summary>
        /// <param name="value">The item which is used to set the textfield</param>
        /// <param name="textBlock">The textblock which shall be followed</param>
        /// <returns>true, if an item was given</returns>
        private void UpdateTextOfTextBlock(IObject? value, TextBlock textBlock)
        {
            if (value == null)
            {
                textBlock.Text = "No item";
                textBlock.FontStyle = FontStyles.Italic;

                return;
            }

            textBlock.Text = value.ToString();
            textBlock.TextDecorations = TextDecorations.Underline;
            textBlock.Cursor = Cursors.Hand;
            textBlock.MouseDown += TextBlockOnMouseDown;
        }

        private void TextBlockOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_detailFormControl == null) throw new InvalidOperationException("_detailFormControl == null");

            var itemToOpen = _value.getOrDefault<IElement>(_name);
            if (itemToOpen == null)
            {
                MessageBox.Show("No item selected");
            }
            else
            {
                NavigatorForItems.NavigateToElementDetailView(
                    _detailFormControl.NavigationHost,
                    itemToOpen);
            }
        }
    }
}