using System;
using System.Collections.Generic;
using System.Dynamic;
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
using System.Windows.Shapes;
using DatenMeister.Excel.Spreadsheet;
using Path = System.IO.Path;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für ExcelImportDefinitionDialog.xaml
    /// </summary>
    public partial class ExcelImportDefinitionDialog : Window
    {
        private SSDocument _excelDocument;
        public ExcelImportDefinitionDialog()
        {
            InitializeComponent();
        }

        public async void PrepareFile(string filePath)
        {
            txtFileName.Text = Path.GetFileName(filePath);

            var sheetNames = new List<string>();
            await Task.Run(() =>
            {
                _excelDocument = SSDocument.LoadFromFile(filePath);
                sheetNames.AddRange(_excelDocument.Tables.Select(sheets => sheets.Name));
            });

            UpdateDataPreview();
            cboSheet.ItemsSource = sheetNames;
        }

        public void UpdateDataPreview()
        {
            if (_excelDocument == null)
            {
                return;
            }

            var sheet = cboSheet.SelectedItem?.ToString();
            if (sheet == null)
            {
                return;
            }

            if (!int.TryParse(txtOffsetColumn.Text, out var offsetColumn)
                || !int.TryParse(txtOffsetRow.Text, out var offsetRow))
            {
                MessageBox.Show("Invalid offset.");
                return;
            }

            var foundSheet = _excelDocument.Tables.FirstOrDefault(x => x.Name == sheet);
            if (foundSheet == null)
            {
                return;
            }

            // Get Header Rows

            
            var items = new List<object>();
            for (var r = 0; r < 10; r++)
            {
                var item = (IDictionary<string, object>) new ExpandoObject();
                for (var c = 0; c < 10; c++)
                {
                    if (r == 0)
                    {
                        var column = new DataGridTextColumn {Binding = new Binding("_" + c)};
                        dgrExcelDataGrid.Columns.Add(column);
                    }

                    item["_" + c] = foundSheet.GetCellContent(r + offsetRow, c + offsetColumn);
                }

                items.Add(item);
            }

            dgrExcelDataGrid.ItemsSource = items;
        }

        private void CboSheet_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataPreview();
        }

        private void TxtOffsetRow_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataPreview();
        }

        private void ChkHeaderRow_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateDataPreview();
        }
    }
}
