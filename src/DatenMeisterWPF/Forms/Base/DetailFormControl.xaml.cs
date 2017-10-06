using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für DetailFormControl.xaml
    /// </summary>
    public partial class DetailFormControl : UserControl
    {
        private IElement _formDefinition;

        /// <summary>
        /// Gets the detailled element, whose content is shown in the dialog
        /// </summary>
        public IElement DetailElement { get; private set; }

        private IDatenMeisterScope _scope;

        /// <summary>
        /// This event handler is thrown, when the user clicks on 'Save' and after the properties are
        /// transferred from form display to element
        /// </summary>
        public event EventHandler ElementSaved;
        

        /// <summary>
        /// Stores the list of actions that will be performed when the user clicks on set
        /// </summary>
        private readonly List<Action> _setActions= new List<Action>();


        public DetailFormControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Sets the content for a new object
        /// </summary>
        /// <param name="scope">Scope of DatenMeister</param>
        /// <param name="formDefinition">Form Definition being used</param>
        public void SetContentForNewObject(IDatenMeisterScope scope, IElement formDefinition)
        {
            SetContent(
                scope,
                InMemoryObject.CreateEmpty(),
                formDefinition);
        }

        public void SetContent(IDatenMeisterScope scope, IElement element, IElement formDefinition)
        {
            DetailElement = element ?? InMemoryObject.CreateEmpty();
            _formDefinition = formDefinition;
            _scope = scope;

            SetContent();
        }

        /// <summary>
        /// Updates the content
        /// </summary>
        public void SetContent()
        {
            // Checks, if automatic view is required
            if (_formDefinition == null)
            {
                var viewFinder = _scope.Resolve<IViewFinder>();
                _formDefinition = viewFinder.FindView(DetailElement, null);
            }

            _setActions.Clear();
            if (!(_formDefinition?.get(_FormAndFields._Form.fields) is IReflectiveCollection fields))
            {
                return;
            }

            var n = 0;
            foreach (var field in fields.Cast<IElement>())
            {
                var name = field.get(_FormAndFields._FieldData.name).ToString();
                var title = field.get(_FormAndFields._FieldData.title).ToString();
                var isEnumeration = DotNetHelper.AsBoolean(field.get(_FormAndFields._FieldData.isEnumeration));
                var isReadOnly = field.get(_FormAndFields._FieldData.isReadOnly).ToString();

                var row = new RowDefinition();
                DataGrid.RowDefinitions.Add(row);

                // Sets the title block
                var titleBlock = new TextBlock
                {
                    Text = title,
                    IsEnabled = !DotNetHelper.AsBoolean(isReadOnly)
                };

                Grid.SetColumn(titleBlock, 0);
                Grid.SetRow(titleBlock, n);
                DataGrid.Children.Add(titleBlock);
                
                // Content Block
                if (!isEnumeration)
                {
                    var contentBlock = new TextBox();
                    Grid.SetColumn(contentBlock, 1);
                    Grid.SetRow(contentBlock, n);
                    DataGrid.Children.Add(contentBlock);

                    string valueText = string.Empty;
                    if (DetailElement.isSet(name))
                    {
                        valueText = DetailElement?.get(name)?.ToString() ?? string.Empty;
                        contentBlock.Text = valueText;
                    }

                    _setActions.Add(
                        () =>
                        {
                            if (valueText != contentBlock.Text)
                            {
                                DetailElement.set(name, contentBlock.Text);
                            }
                        });
                }

                n++;
            }
        }

        /// <summary>
        /// Adds the default cancel and save buttons
        /// </summary>
        public void AddDefaultButtons(string saveText = "Save")
        {
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
            var button = new ViewButton
            {
                Content = name

            };

            button.Pressed += (x, y) => { onPressed(); };
            ButtonBar.Children.Add(button);
            return button;
        }

        protected virtual void OnElementSaved()
        {
            ElementSaved?.Invoke(this, EventArgs.Empty);
        }
    }
}
