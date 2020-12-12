using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Modules.DefaultTypes;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;
using DatenMeister.WPF.Controls;
using DatenMeister.WPF.Forms.Base;
using DatenMeister.WPF.Navigation;

namespace DatenMeister.WPF.Forms.Fields
{
    /// <summary>
    /// Implements a reference field which is shown the currently selected instance and allows the user to select
    /// another instance to set the appropriate property
    /// </summary>
    public partial class ReferenceField : IDetailField
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
        private IObject? _element;

        /// <summary>
        /// Gets or sets a selected value
        /// </summary>
        public IObject? SelectedValue { get; set; }
        
        /// <summary>
        /// Gets or sets a flag whether the value shall be unset
        /// </summary>
        public bool IsValueUnsetted { get; set; }

        /// <summary>
        /// Gets or sets the flag whether the form is inline
        /// </summary>
        private bool _isInline;

        private TextBlock? _inputTextBox;

        private bool _isEnabled = true;
        private CreateNewInstanceButton? _newButton;
        private Button? _removeButton;
        
        private Button? _selectButton;
        private string? _workspace;
        private string? _extent;

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                if (!_isInline && _selectButton != null && _removeButton != null && _newButton != null)
                {
                    _newButton.IsEnabled = value;
                    _selectButton.IsEnabled = value;
                    _removeButton.IsEnabled = value;
                }
            }
        }

        public UIElement CreateElement(
            IObject element,
            IElement fieldData,
            DetailFormControl detailForm,
            FieldParameter fieldFlags)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (fieldData == null) throw new ArgumentNullException(nameof(fieldData));
            if (detailForm == null) throw new ArgumentNullException(nameof(detailForm));
            
            var navigationHost = detailForm.NavigationHost;
            
            _isInline = fieldData.getOrDefault<bool>(_DatenMeister._Forms._ReferenceFieldData.isSelectionInline);
            var isReadOnly = fieldData.getOrDefault<bool>(_DatenMeister._Forms._ReferenceFieldData.isReadOnly)
                || fieldFlags.IsReadOnly;
            _name = fieldData.getOrDefault<string>(_DatenMeister._Forms._FieldData.name) ?? string.Empty;
            _detailFormControl = detailForm;
            _element = element;
            
            _workspace = fieldData.getOrDefault<string>(_DatenMeister._Forms._ReferenceFieldData.defaultWorkspace);
            _extent = fieldData.getOrDefault<string>(_DatenMeister._Forms._ReferenceFieldData.defaultExtentUri);

            if (_extent == null)
            {
                var foundExtent = element.GetUriExtentOf();
                _extent = foundExtent?.contextURI() ?? null;

                if (foundExtent != null)
                {
                    _workspace = GiveMe.Scope.WorkspaceLogic.GetWorkspaceOfExtent(foundExtent)?.id;
                }
            }

            if (_name == null)
            {
                return new TextBlock
                {
                    Text = "Bad configuration: _name == null"
                };
            }

            // Checks, whether the reference shall be included as an inline selection
            return _isInline
                ? CreateInlineField(fieldData, fieldFlags)
                : CreateSelectionField(element, fieldData, navigationHost, fieldFlags, isReadOnly);
        }

        private UIElement CreateSelectionField(
            IObject value,
            IElement fieldData,
            INavigationHost navigationHost,
            FieldParameter fieldFlags,
            bool isReadOnly)
        {
            if (_name == null) throw new InvalidOperationException("_name == null");
            
            var panel = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1.0, GridUnitType.Star)}, // Text field
                    new ColumnDefinition {Width = new GridLength(90.0, GridUnitType.Pixel)}, // New button
                    new ColumnDefinition {Width = new GridLength(90.0, GridUnitType.Pixel)}, // Select button
                    new ColumnDefinition {Width = new GridLength(90.0, GridUnitType.Pixel)}, // Remove button
                },
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            SelectedValue = value.getOrDefault<IElement>(_name);

            CreateInputTextBox();

            CreateNewButton(value, navigationHost);

            CreateSelectButton(fieldData, navigationHost);

            CreateRemoveButton();

            if ( _newButton == null || _selectButton == null  || _removeButton == null || _inputTextBox == null)
                throw new InvalidOperationException("One of the fields are null... Should not be. ");

            // Adds the ui elements
            Grid.SetColumn(_newButton, 1);
            Grid.SetColumn(_selectButton, 2);
            Grid.SetColumn(_removeButton, 3);
            panel.Children.Add(_inputTextBox);

            if (!isReadOnly)
            {
                panel.Children.Add(_newButton);
                panel.Children.Add(_selectButton);
                panel.Children.Add(_removeButton);
            }

            fieldFlags.CanBeFocused = true;
            return panel;
        }

        private void CreateInputTextBox()
        {
            _inputTextBox = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Center
            };

            _inputTextBox.MouseDown += TextBlockOnMouseDown;
            UpdateTextOfTextBlock(_inputTextBox, SelectedValue);
        }

        /// <summary>
        /// Updates the text of the text block. 
        /// </summary>
        /// <param name="inputTextBox"></param>
        /// <param name="value">The item which is used to set the textfield</param>
        /// <returns>true, if an item was given</returns>
        internal static void UpdateTextOfTextBlock(TextBlock inputTextBox, IObject? value)
        {
            if (inputTextBox == null) 
                throw new InvalidOperationException("_inputTextBox == null");

            if (value == null)
            {
                inputTextBox.Text = "No item";
                inputTextBox.FontStyle = FontStyles.Italic;

                return;
            }

            inputTextBox.Text = value.ToString();
            inputTextBox.TextDecorations = TextDecorations.Underline;
            inputTextBox.Cursor = Cursors.Hand;
        }

        private void CreateRemoveButton()
        {
            if (_inputTextBox == null)
                throw new InvalidOperationException(nameof(_inputTextBox) + " is null");

            _removeButton = new Button
            {
                Content = "Remove",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            _removeButton.Click += (sender, args) =>
            {
                SelectedValue = null;
                IsValueUnsetted = false;
                UpdateTextOfTextBlock(_inputTextBox, null);
            };
        }

        private void CreateSelectButton(IElement fieldData, INavigationHost navigationHost)
        {
            if (_inputTextBox == null)
                throw new InvalidOperationException(nameof(_inputTextBox) + " is null");

            _selectButton = new Button
            {
                Content = "Select",
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            _selectButton.Click += async (sender, args) =>
            {
                var workspaceLogic = GiveMe.Scope.Resolve<IWorkspaceLogic>();

                var (foundWorkspace, foundExtent) = workspaceLogic.TryGetWorkspaceAndExtent(
                    _workspace,
                    _extent);

                var configuration =
                    new NavigatorForDialogs.NavigatorForDialogConfiguration
                    {
                        DefaultWorkspace = foundWorkspace,
                        DefaultExtent = foundExtent
                    };

                var filterMetaClasses =
                    fieldData.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ReferenceFieldData.metaClassFilter);
                if (filterMetaClasses != null)
                {
                    configuration.FilteredMetaClasses = filterMetaClasses.OfType<IElement>().ToList();
                }

                var selectedItem = await NavigatorForDialogs.Locate(
                    navigationHost,
                    configuration);

                if (selectedItem != null)
                {
                    IsValueUnsetted = false;
                    SelectedValue = selectedItem;
                    UpdateTextOfTextBlock(_inputTextBox, selectedItem);
                }
            };
        }

        private void CreateNewButton(IObject value, INavigationHost navigationHost)
        {
            if (_inputTextBox == null)
                throw new InvalidOperationException(nameof(_name) + " is null");
            if ( _name == null)
                throw new InvalidOperationException(nameof(_name) + " is null");

            var foundProperty = ClassifierMethods.GetPropertyTypeOfValuesProperty(value as IElement, _name);

            _newButton = new CreateNewInstanceButton
            {
                Content = "New...",
                ToolTip = foundProperty == null ? "Undefined Type" : foundProperty.ToString(),
                VerticalAlignment = VerticalAlignment.Center
            };

            _newButton.SetDefaultTypeForCreation(foundProperty);

            _newButton.TypeSelected += async (sender, args) =>
            {
                var factory = new MofFactory(value);
                var newElement = factory.create(args.SelectedType);

                var result = await NavigatorForItems.NavigateToElementDetailView(
                    navigationHost,
                    newElement);

                if (result?.Result == NavigationResult.Saved)
                {
                    ObjectOperations.AddItemReferenceToInstanceProperty(
                        value as IElement ?? throw new InvalidOperationException("value is not an element"),
                        _name,
                        newElement
                    );

                    UpdateTextOfTextBlock(_inputTextBox, newElement);
                }
            };
        }

        public void CallSetAction(IObject element)
        {
            if(_name == null) throw new InvalidOperationException("_name == null");

            if (_isInline)
            {
                var selectedElement = _control?.SelectedElement;
                if (selectedElement != null)
                {
                    element.set(_name, selectedElement);
                }
            }
            else
            {
                if (IsValueUnsetted)
                {
                    element.unset(_name);
                }
                else
                {
                    element.set(_name, SelectedValue);
                }
            }
        }

        private void TextBlockOnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_name == null) throw new InvalidOperationException("_name == null");
            if (_element == null) throw new InvalidOperationException("_value == null");
            if (_detailFormControl == null) throw new InvalidOperationException("_detailFormControl == null");

            var itemToOpen = _element.getOrDefault<IElement>(_name);
            if (itemToOpen == null)
            {
                MessageBox.Show("No item selected");
            }
            else
            {
                _ = NavigatorForItems.NavigateToElementDetailView(
                    _detailFormControl.NavigationHost,
                    itemToOpen);
            }
        }

        public void SetSelectedValue(object? elementValue)
        {
            if (_inputTextBox == null) throw new InvalidOperationException("_inputTextBox == null");
                
            if (!(elementValue is IObject elementAsObject))
            {
                SelectedValue = null;
                UpdateTextOfTextBlock(_inputTextBox, null);
            }
            else
            {
                SelectedValue = elementAsObject;
                UpdateTextOfTextBlock(_inputTextBox, SelectedValue);
            }
        }
    }
}