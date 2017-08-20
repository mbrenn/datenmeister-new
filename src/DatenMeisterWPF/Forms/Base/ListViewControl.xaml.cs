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
using DatenMeister.Models.Forms;

namespace DatenMeisterWPF.Forms.Base
{
    /// <summary>
    /// Interaktionslogik für ListViewControl.xaml
    /// </summary>
    public partial class ListViewControl : UserControl
    {
        public ListViewControl()
        {
            InitializeComponent();
        }

        public IReflectiveCollection Items { get; set; }


        public IElement FormDefinition { get; set; }

        public void UpdateContent()
        {
            var fields = FormDefinition?.get(_FormAndFields._Form.fields) as IReflectiveCollection;

            if (fields == null)
            {
                return;
            }

            foreach (var field in fields.Cast<IElement>())
            {
                var dataColumn = new DataGridTextColumn
                {
                    Header = field.get(_FormAndFields._FieldData.name)
                };

                DataGrid.Columns.Add(dataColumn);
            }
        }
    }
}
