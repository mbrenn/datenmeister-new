using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Runtime;
using Path = System.IO.Path;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für ExcelImportDefinitionDialog.xaml
    /// </summary>
    public partial class ExcelImportDefinitionDialog : Window
    {
        private ExcelImporter _importer;

        private Func<IReflectiveCollection> _extentFactory;

        public ExcelImportDefinitionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Prepares the excel file by reading it in and updating all the necessary views
        /// </summary>
        /// <param name="filePath">Excel file to be loaded</param>
        /// <param name="extentFactory">Extent factory being used to create/find the extent, when user clicks on 'import'</param>
        public async Task<ExcelImporter> PrepareFile(string filePath, Func<IReflectiveCollection> extentFactory)
        {
            _extentFactory = extentFactory;
            txtFileName.Text = Path.GetFileName(filePath);

            _importer = new ExcelImporter(
                new ExcelImportSettings { filePath = filePath }
            );

            await Task.Run(() => { _importer.LoadExcel(); });

            DataContext = _importer.Settings;

            _importer.GuessColumnCount();
            _importer.GuessRowCount();
            UpdateDataPreview();

            cboSheet.ItemsSource = _importer.SheetNames;
            cboSheet.SelectedItem = _importer.SheetNames.FirstOrDefault();

            return _importer;
        }

        /// <summary>
        /// Whenever a value is changed, the view of the imported Excel will be updated
        /// </summary>
        private void UpdateDataPreview()
        {
            if (!IsInitialized || _importer?.Settings == null || !_importer.IsExcelLoaded) return;

            // Gets the columns names
            var columnNames = _importer.GetColumnNames();

            // Now create the items
            dgrExcelDataGrid.Columns.Clear();
            var items = new List<object>();
            for (var r = 0; r < _importer.Settings.countRows; r++)
            {
                var item = (IDictionary<string, object>) new ExpandoObject();

                for (var c = 0; c < _importer.Settings.countColumns; c++)
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

                    item[internalColumnName] = _importer.GetCellContent(r, c);
                }

                items.Add(item);
            }

            dgrExcelDataGrid.ItemsSource = items;
        }

        private void CboSheet_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtOffsetRow.Text = "0";
            txtOffsetColumn.Text = "0";
            _importer.GuessRowCount();
            _importer.GuessColumnCount();
            UpdateDataPreview();
        }

        private void TxtOffsetRow_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _importer.Settings.offsetRow = DotNetHelper.AsInteger(txtOffsetRow.Text);
            _importer.GuessRowCount();
            UpdateDataPreview();
        }

        private void TxtOffsetColumn_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _importer.Settings.offsetColumn = DotNetHelper.AsInteger(txtOffsetColumn.Text);
            _importer.GuessColumnCount();
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
            _importer.GuessRowCount();
            UpdateDataPreview();
        }

        private void chkAutoCount_OnClick(object sender, RoutedEventArgs e)
        {
            _importer.GuessColumnCount();
            _importer.GuessRowCount();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            _importer.GuessColumnCount();
            _importer.GuessRowCount();
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

        private void btnImportReference_Click(object sender, RoutedEventArgs e)
        {
            ImportIntoGivenExtent();
        }

        /// <summary>
        /// Gets the configuration as an item 
        /// </summary>
        /// <returns>The configuration object describing the elements</returns>
        public IObject GetConfigurationObject()
        {
            return DotNetConverter.ConvertFromDotNetObject(_importer.Settings.GetSettingsAsMofObject());
        }

        /// <summary>
        /// Performs the import into the extent
        /// </summary>
        private void ImportIntoGivenExtent()
        {
            ExcelImporter.ImportExcelAsCopy(GetConfigurationObject(), _extentFactory);
        }
    }
}
