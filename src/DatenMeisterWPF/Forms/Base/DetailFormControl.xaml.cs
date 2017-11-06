using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;
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
        private IElement _formDefinition;

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
        private readonly List<Action> _setActions = new List<Action>();
        
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
            _formDefinition = formDefinition;

            SetContent();
        }

        private int _fieldCount;

        /// <summary>
        /// Updates the content
        /// </summary>
        public void SetContent()
        {
            // Checks, if automatic view is required
            if (_formDefinition == null)
            {
                var viewFinder = App.Scope.Resolve<IViewFinder>();
                _formDefinition = viewFinder.FindView(DetailElement, null);
            }

            _setActions.Clear();
            if (!(_formDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }

            _fieldCount = 0;

            CreateRows(fields);

            if (DetailElement != null)
            {
                var mofElement = (MofElement)DetailElement;
                var uriExtent = mofElement.Extent as MofUriExtent;

                var uriExtentText = uriExtent?.contextURI() ?? string.Empty;
                var fullName = NamedElementMethods.GetFullName(DetailElement);
                CreateRowForField("Extent:", uriExtentText, true);
                CreateRowForField("Full Name:", fullName, true);
                CreateRowForField("Url:", $"{uriExtentText}?{fullName}", true);

                // Sets the metaclass
                if (_formDefinition.isSet(_FormAndFields._Form.hideMetaClass) &&
                    DotNetHelper.AsBoolean(_formDefinition.get(_FormAndFields._Form.hideMetaClass)))
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

                var name = field.get(_FormAndFields._FieldData.name).ToString();
                var title = field.get(_FormAndFields._FieldData.title).ToString();
                var isEnumeration = DotNetHelper.AsBoolean(field.get(_FormAndFields._FieldData.isEnumeration));
                var isReadOnly = field.get(_FormAndFields._FieldData.isReadOnly).ToString();

                // Sets the title block
                var titleBlock = new TextBlock
                {
                    Text = $"{title}: ",
                    IsEnabled = !DotNetHelper.AsBoolean(isReadOnly)
                };

                /* Local functions for text and enumerations */
                UIElement CreateForText()
                {
                    var contentBlock = new TextBox();

                    var valueText = string.Empty;
                    if (DetailElement?.isSet(name) == true)
                    {
                        valueText = DetailElement.get(name)?.ToString() ?? string.Empty;
                        contentBlock.Text = valueText;
                    }
                    else
                    {
                        if (field.isSet(_FormAndFields._FieldData.defaultValue))
                        {
                            contentBlock.Text = field.get(_FormAndFields._FieldData.defaultValue)?.ToString() ?? string.Empty;
                        }
                    }

                    _setActions.Add(
                        () =>
                        {
                            if (valueText != contentBlock.Text)
                            {
                                DetailElement.set(name, contentBlock.Text);
                            }
                        });

                    return contentBlock;
                }

                UIElement CreateForEnumeration()
                {
                    var contentBlock = new Grid();
                    Grid.SetColumn(contentBlock, 1);
                    Grid.SetRow(contentBlock, _fieldCount);
                    contentBlock.ColumnDefinitions.Add(new ColumnDefinition());
                    contentBlock.ColumnDefinitions.Add(new ColumnDefinition());

                    if (DetailElement?.isSet(name) == true)
                    {
                        var value = DetailElement.get(name);
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
                            var innerTextBlock = new TextBlock
                            {
                                Text = innerValue.ToString()
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
                                        NavigationHost,
                                        asIElement);

                                contentBlock.Children.Add(button);
                            }

                            inner++;
                        }
                    }

                    return contentBlock;
                }

                /* Now do your job */
                var item = isEnumeration ? CreateForEnumeration() : CreateForText();
                CreateRowForField(titleBlock, item);
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

                    _setActions.Add(() =>
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
                    foreach (var action in _setActions)
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
