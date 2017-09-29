using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                
                var row = new RowDefinition();
                DataGrid.RowDefinitions.Add(row);

                // Sets the title block
                var titleBlock = new TextBlock
                {
                    Text = title
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
                var window = Window.GetWindow(this);
                window?.Close();
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
