using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für DetailFormControl.xaml
    /// </summary>
    public partial class DetailFormControl : UserControl
    {
        private IElement _formDefinition;
        private IElement _element;
        private IDatenMeisterScope _scope;

        /// <summary>
        /// Stores the list of actions that will be performed when the user clicks on set
        /// </summary>
        private readonly List<Action> _setActions= new List<Action>();


        public DetailFormControl()
        {
            InitializeComponent();
        }

        public void UpdateContent(IDatenMeisterScope scope, IElement element, IElement formDefinition)
        {
            _element = element;
            _formDefinition = formDefinition;
            _scope = scope;

            UpdateContent();
        }

        /// <summary>
        /// Updates the content
        /// </summary>
        public void UpdateContent()
        {
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
                var isEnumeration = ObjectHelper.IsTrue(field.get(_FormAndFields._FieldData.isEnumeration));
                var isReadOnly = field.get(_FormAndFields._FieldData.isReadOnly).ToString();

                var row = new RowDefinition();
                DataGrid.RowDefinitions.Add(row);

                // Sets the title block
                var titleBlock = new TextBlock
                {
                    Text = title,
                    IsEnabled = !ObjectHelper.IsTrue(isReadOnly)
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

                    var valueText = _element.get(name).ToString();
                    contentBlock.Text = valueText;

                    _setActions.Add(
                        () =>
                        {
                            if (valueText != contentBlock.Text)
                            {
                                _element.set(name, contentBlock.Text);
                            }
                        });
                }

                n++;
            }
        }

        public void AddDefaultButtons()
        {
            AddGenericButton("Cancel", () =>
            {
                var window = Window.GetWindow(this);
                window?.Close();
            });

            AddGenericButton("Save", () =>
            {
                try
                {
                    foreach (var action in _setActions)
                    {
                        action();
                    }

                    var window = Window.GetWindow(this);
                    window?.Close();
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
    }
}
