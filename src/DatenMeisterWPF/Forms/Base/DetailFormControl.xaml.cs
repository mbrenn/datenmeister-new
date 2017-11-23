﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
using DatenMeisterWPF.Forms.Detail.Fields;
using DatenMeisterWPF.Navigation;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für DetailFormControl.xaml
    /// </summary>
    public partial class DetailFormControl : UserControl
    {
        /// <summary>
        /// Defines the form definition being used in the detail for
        /// </summary>
        private IElement _actualFormDefinition;

        /// <summary>
        /// Gets or sets the view definition
        /// </summary>
        public ViewDefinition ViewDefinition { get; set; }

        /// <summary>
        /// Gets or sets the navigation host
        /// </summary>
        public INavigationHost NavigationHost { get; set; }

        /// <summary>
        /// Gets the detailled element, whose content is shown in the dialog
        /// </summary>
        public IElement DetailElement { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether new properities may be added by the user to the element
        /// </summary>
        public bool AllowNewProperties { get; set; }

        /// <summary>
        /// This event handler is thrown, when the user clicks on 'Save' and after the properties are
        /// transferred from form display to element
        /// </summary>
        public event EventHandler ElementSaved;

        /// <summary>
        /// Stores the list of actions that will be performed when the user clicks on set
        /// </summary>
        public List<Action> SetActions { get; }= new List<Action>();
        
        public DetailFormControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the content for a new object
        /// </summary>
        /// <param name="formDefinition">Form Definition being used</param>
        public void SetContentForNewObject(IElement formDefinition)
        {
            SetContent(
                InMemoryObject.CreateEmpty(),
                formDefinition);
        }

        public void SetContent(IElement element, IElement formDefinition)
        {
            DetailElement = element ?? InMemoryObject.CreateEmpty();

            ViewDefinition = new ViewDefinition(
                null,
                formDefinition,
                formDefinition == null ? ViewDefinitionMode.Default : ViewDefinitionMode.Specific
            );

            RefreshViewDefinition();
            UpdateContent();
        }

        private int _fieldCount;

        /// <summary>
        /// This method gets called, when a new item is added or an existing item was modified. 
        /// Derivation of the class which have automatic creation of columns may include additional columns
        /// </summary>
        private void RefreshViewDefinition()
        {
            var viewFinder = App.Scope.Resolve<IViewFinder>();
            if (ViewDefinition.Mode == ViewDefinitionMode.Default)
            {
                _actualFormDefinition = viewFinder.FindView(DetailElement);
            }

            if (ViewDefinition.Mode == ViewDefinitionMode.AllProperties
                || ViewDefinition.Mode == ViewDefinitionMode.Default && _actualFormDefinition == null)
            {
                _actualFormDefinition = viewFinder.CreateView(DetailElement);
            }

            if (ViewDefinition.Mode == ViewDefinitionMode.Specific)
            {
                _actualFormDefinition = ViewDefinition.Element;
            }
        }

        /// <summary>
        /// Updates the content
        /// </summary>
        private void UpdateContent()
        {
            SetActions.Clear();
            if (!(_actualFormDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }

            _fieldCount = 0;

            CreateRows(fields);

            if (DetailElement != null)
            {
                var mofElement = (MofElement)DetailElement;
                var uriExtent = mofElement.Extent as MofUriExtent;

                CreateSeparator();

                var uriExtentText = uriExtent?.contextURI() ?? string.Empty;
                var fullName = NamedElementMethods.GetFullName(DetailElement);
                CreateRowForField("Extent:", uriExtentText, true);
                CreateRowForField("Full Name:", fullName, true);
                CreateRowForField("Url w/ ID:", mofElement.GetUri(), true);
                CreateRowForField("Url w/Fullname:", $"{uriExtentText}?{fullName}", true);

                // Sets the metaclass
                if (DotNetHelper.IsFalseOrNotSet(_actualFormDefinition, _FormAndFields._Form.hideMetaClass))
                {
                    var metaClass = DetailElement.getMetaClass();
                    CreateRowForField(
                        "Meta Class:",
                        metaClass == null ? string.Empty : NamedElementMethods.GetFullName(metaClass));
                }
            }
        }

        /// <summary>
        /// Creates the rows
        /// </summary>
        /// <param name="fields">Fields to be created</param>
        private void CreateRows(IReflectiveCollection fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            foreach (var field in fields.Cast<IElement>())
            {
                var fieldType = field.get(_FormAndFields._FieldData.fieldType)?.ToString();
                if (fieldType == MetaClassElementFieldData.FieldType)
                {
                    continue;
                }
                
                var title = field.get(_FormAndFields._FieldData.title).ToString();
                var isReadOnly = field.get(_FormAndFields._FieldData.isReadOnly).ToString();

                // Sets the title block
                var titleBlock = new TextBlock
                {
                    Text = $"{title}: ",
                    IsEnabled = !DotNetHelper.AsBoolean(isReadOnly)
                };

                var contentBlock = FieldFactory.GetUIElementFor(DetailElement, field, this);
                Grid.SetColumn(contentBlock, 1);
                Grid.SetRow(contentBlock, _fieldCount);
                
                CreateRowForField(titleBlock, contentBlock);
            }
        }

        /// <summary>
        /// Creates a row by having a text
        /// </summary>
        /// <param name="keyText">Text to be added</param>
        /// <param name="valueText">Value to be added</param>
        /// <param name="selectable">True, if the user can copy the content to the clipboard.</param>
        private void CreateRowForField(string keyText, string valueText, bool selectable = false)
        {
            var valueTextBlock = new TextBlock {Text = valueText};

            if (selectable)
            {
                valueTextBlock.ContextMenu = 
                    new ContextMenu();

                var copyToClipboardAdd = new MenuItem {Header = "Copy to Clipboard"};
                valueTextBlock.ContextMenu.Items.Add(copyToClipboardAdd);
                copyToClipboardAdd.Click += (x, y) =>
                {
                    Clipboard.SetText(valueText);
                };
            }

            CreateRowForField(
                new TextBlock {Text = keyText},
                valueTextBlock);
        }

        /// <summary>
        /// Creates a new row in the detail view 
        /// </summary>
        /// <param name="propertyKey">UIElement for the left column</param>
        /// <param name="propertyValue">UIElement for the right column</param>
        private void CreateRowForField(UIElement propertyKey, UIElement propertyValue)
        {
            DataGrid.RowDefinitions.Add(new RowDefinition());

            Grid.SetColumn(propertyKey, 0);
            Grid.SetRow(propertyKey, _fieldCount);

            Grid.SetColumn(propertyValue, 1);
            Grid.SetRow(propertyValue, _fieldCount);

            DataGrid.Children.Add(propertyKey);
            DataGrid.Children.Add(propertyValue);

            _fieldCount++;
        }

        private void CreateSeparator()
        {
            DataGrid.RowDefinitions.Add(new RowDefinition());
            var line = new Line
            {
                Stretch = Stretch.Fill,
                Stroke = Brushes.Gray,
                StrokeThickness = 1.0,
                Margin = new Thickness(0,10,0,10)
            };

            Grid.SetRow(line, _fieldCount);
            Grid.SetColumnSpan(line, 2);

            DataGrid.Children.Add(line);

            _fieldCount++;
        }

        /// <summary>
        /// Adds the default cancel and save buttons
        /// </summary>
        public void AddDefaultButtons(string saveText = "Save")
        {
            if (AllowNewProperties)
            {
                AddGenericButton("New Property", () =>
                {
                    var fieldKey = new TextBox();
                    var fieldValue = new TextBox();

                    SetActions.Add(() =>
                    {
                        var propertyKey = fieldKey.Text;
                        var propertyValue = fieldValue.Text;

                        if (!string.IsNullOrEmpty(propertyKey))
                        {
                            DetailElement.set(propertyKey, propertyValue);
                        }
                    });

                    CreateRowForField(fieldKey, fieldValue);
                    fieldKey.Focus();
                });
            }

            AddGenericButton("Cancel", () =>
            {
                var window = Window.GetWindow(this);
                window?.Close();
            });

            AddGenericButton(saveText, () =>
            {
                try
                {
                    foreach (var action in SetActions)
                    {
                        action();
                    }

                    OnElementSaved();
                    Window.GetWindow(this)?.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            });
        }

        /// <summary>
        /// Adds a button to the view
        /// </summary>
        /// <param name="name">Name of the button</param>
        /// <param name="onPressed">Called if the user clicks on the button</param>
        /// <returns>The created button</returns>
        public ViewButton AddGenericButton(string name, Action onPressed)
        {
            var button = new ViewButton {Content = name};

            button.Pressed += (x, y) => { onPressed(); };
            ButtonBar.Children.Add(button);
            return button;
        }

        protected void OnElementSaved()
        {
            ElementSaved?.Invoke(this, EventArgs.Empty);
        }
    }
}
