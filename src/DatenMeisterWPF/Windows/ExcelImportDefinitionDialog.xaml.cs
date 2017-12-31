using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
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
        private Func<IReflectiveCollection> _extentFactory;

        public ExcelImportDefinitionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Prepares the excel file by reading it in. 
        /// </summary>
        /// <param name="filePath">Excel file to be loaded</param>
        /// <param name="extentFactory">Extent factory being used to create/find the extent, when user clicks on 'import'</param>
        public async void PrepareFile(string filePath, Func<IReflectiveCollection> extentFactory)
        {
            _extentFactory = extentFactory;
            txtFileName.Text = Path.GetFileName(filePath);

            var sheetNames = new List<string>();
            await Task.Run(() =>
            {
                _excelDocument = SSDocument.LoadFromFile(filePath);
                sheetNames.AddRange(_excelDocument.Tables.Select(sheets => sheets.Name));
            });

            GuessColumnCount();
            GuessRowCount();
            UpdateDataPreview();

            cboSheet.ItemsSource = sheetNames;
            cboSheet.SelectedItem = sheetNames.FirstOrDefault();
        }

        private void UpdateDataPreview()
        {
            if (!IsInitialized) return;

            // Reads the data from the form
            if (_excelDocument == null)
            {
                return;
            }

            if (!int.TryParse(txtOffsetColumn.Text, out var offsetColumn)
                || !int.TryParse(txtOffsetRow.Text, out var offsetRow)
                || !int.TryParse(txtCountColumn.Text, out var countColumns)
                || !int.TryParse(txtCountRow.Text, out var countRows))
            {
                MessageBox.Show("Invalid offset.");
                return;
            }

            var hasHeaderRows = chkHeaderRow.IsChecked == true;

            var foundSheet = GetSelectedSheet();
            if (foundSheet == null)
            {
                return;
            }

            // Gets the columns names
            var columnNames = 
                GetColumnNames(foundSheet, countColumns, offsetColumn, ref offsetRow, hasHeaderRows);

            // Now create the items
            dgrExcelDataGrid.Columns.Clear();
            var items = new List<object>();
            for (var r = 0; r < countRows; r++)
            {
                var item = (IDictionary<string, object>) new ExpandoObject();
                for (var c = 0; c < countColumns; c++)
                {
                    var internalColumnName = "_ " + c;
                    var columnName = columnNames[c];
                    if (columnName == null)
                    {
                        // Skip not set columns
                        continue;
                    }

                    if (r == 0)
                    {
                        var column = new DataGridTextColumn
                        {
                            Binding = new Binding(internalColumnName),
                            Header = columnName
                        };

                        dgrExcelDataGrid.Columns.Add(column);
                    }

                    item[internalColumnName] = foundSheet.GetCellContent(r + offsetRow, c + offsetColumn);
                }

                items.Add(item);
            }

            dgrExcelDataGrid.ItemsSource = items;
        }

        private SsTable GetSelectedSheet()
        {
            var sheet = cboSheet?.SelectedItem?.ToString();
            if (sheet == null)
            {
                return null;
            }

            return _excelDocument.Tables.FirstOrDefault(x => x.Name == sheet);
        }

        /// <summary>
        /// Gets the column names
        /// </summary>
        /// <param name="foundSheet">The sheet being used to extract the data</param>
        /// <param name="countColumns">Number of columns</param>
        /// <param name="offsetColumn">Offset of column</param>
        /// <param name="offsetRow">Offset of row</param>
        /// <param name="hasHeaderRows">true, if it has header rows</param>
        /// <returns>List of Columns</returns>
        private static List<string> GetColumnNames(SsTable foundSheet, int countColumns, int offsetColumn, ref int offsetRow,
            bool hasHeaderRows)
        {
            // Get Header Rows
            var columnNames = new List<string>();
            if (!hasHeaderRows)
            {
                for (var c = 0; c < countColumns; c++)
                {
                    columnNames.Add("_" + c);
                }
            }
            else
            {
                for (var c = 0; c < countColumns; c++)
                {
                    var columnName = foundSheet.GetCellContent(offsetRow, c + offsetColumn);
                    if (string.IsNullOrEmpty(columnName) || columnNames.Contains(columnName))
                    {
                        columnNames.Add(null);
                    }
                    else
                    {
                        columnNames.Add(columnName);
                    }
                }

                offsetRow++;
            }
            return columnNames;
        }

        private void GuessRowCount()
        {
            if (!IsInitialized) return;

            var hasHeaderRows = chkHeaderRow?.IsChecked == true;
            var foundSheet = GetSelectedSheet();

            if (foundSheet == null
                || !int.TryParse(txtOffsetColumn.Text, out var offsetColumn)
                || !int.TryParse(txtOffsetRow.Text, out var offsetRow))
            {
                return;
            }

            offsetRow = hasHeaderRows ? offsetRow + 1 : offsetRow;
            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(offsetRow + n, offsetColumn);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            txtCountRow.Text = n.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Guesses the number of columns
        /// </summary>
        private void GuessColumnCount()
        {
            if (!IsInitialized) return;

            var foundSheet = GetSelectedSheet();

            if (foundSheet == null
                || !int.TryParse(txtOffsetColumn.Text, out var offsetColumn)
                || !int.TryParse(txtOffsetRow.Text, out var offsetRow))
            {
                return;
            }

            var n = 0;
            while (true)
            {
                var content = foundSheet.GetCellContent(offsetRow, offsetColumn + n);
                if (string.IsNullOrEmpty(content))
                {
                    break;
                }

                n++;
            }

            txtCountColumn.Text = n.ToString(CultureInfo.InvariantCulture);
        }

        private void CboSheet_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtOffsetRow.Text = "0";
            txtOffsetColumn.Text = "0";
            GuessRowCount();
            GuessColumnCount();
            UpdateDataPreview();
        }

        private void TxtOffsetRow_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            GuessRowCount();
            UpdateDataPreview();
        }

        private void TxtOffsetColumn_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            GuessColumnCount();
            UpdateDataPreview();
        }

        private void TxtCountRow_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataPreview();
        }

        private void TxtCountColumn_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDataPreview();
        }

        private void ChkHeaderRow_OnClick(object sender, RoutedEventArgs e)
        {
            GuessRowCount();
            UpdateDataPreview();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            GuessColumnCount();
            GuessRowCount();
            UpdateDataPreview();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            ImportIntoGivenExtent();
        }

        private void ImportIntoGivenExtent()
        {
            // Reads the data from the form
            if (_excelDocument == null)
            {
                return;
            }

            if (!int.TryParse(txtOffsetColumn.Text, out var offsetColumn)
                || !int.TryParse(txtOffsetRow.Text, out var offsetRow)
                || !int.TryParse(txtCountColumn.Text, out var countColumns)
                || !int.TryParse(txtCountRow.Text, out var countRows))
            {
                MessageBox.Show("Invalid offset.");
                return;
            }

            var hasHeaderRows = chkHeaderRow.IsChecked == true;

            var foundSheet = GetSelectedSheet();
            if (foundSheet == null)
            {
                return;
            }

            // Gets the columns names
            var extent = _extentFactory();
            var factory = new MofFactory(extent);

            var columnNames =
                GetColumnNames(foundSheet, countColumns, offsetColumn, ref offsetRow, hasHeaderRows);
            for (var r = 0; r < countRows; r++)
            {
                var item = factory.create(null);
                for (var c = 0; c < countColumns; c++)
                {
                    var columnName = columnNames[c];
                    if (columnName == null)
                    {
                        // Skip not set columns
                        continue;
                    }


                    item.set(columnName, foundSheet.GetCellContent(r + offsetRow, c + offsetColumn));
                }

                extent.add(item);
            }
        }
    }
}
