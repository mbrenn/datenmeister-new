using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Excel.Helper;
using DatenMeister.Models;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;
using Path = System.IO.Path;

namespace DatenMeister.WPF.Windows
{
    public enum ExcelImportType
    {
        AsCopy,
        AsReference
    }

    /// <summary>
    /// Interaktionslogik für ExcelImportDefinitioOnClosedxaml
    /// </summary>
    public partial class ExcelImportDefinitionDialog : Window
    {
        private ExcelImporter? _importer;

        public ExcelImportType ImportType { get; set; } = ExcelImportType.AsCopy;

        /// <summary>
        /// Defines the excel settings
        /// </summary>
        public IElement? ExcelSettings => _importer?.LoaderConfig;

        public ExcelImportDefinitionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Prepares the excel file by reading it in and updating all the necessary views
        /// </summary>
        /// <param name="filePath">Excel file to be loaded</param>
        public async Task<ExcelImporter> LoadFile(string filePath)
        {
            txtFileName.Text = Path.GetFileName(filePath);
            var importConfig =
                InMemoryObject.CreateEmpty(_DatenMeister.TheOne.ExtentLoaderConfigs.__ExcelImportLoaderConfig) as IElement
                ?? throw new InvalidOperationException("Not an IElement");
            importConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.extentUri, "dm:///dm_temp");
            importConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.filePath, filePath);
            
            _importer = new ExcelImporter(importConfig);

            await Task.Run(() => _importer.LoadExcel());

            DataContext = _importer.LoaderConfig;

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
            if (_importer == null)
                throw new InvalidOperationException("_importer == null");

            if (IsExcelNotLoaded()) return;

            // Gets the columns names
            var columnNames = _importer.GetColumnNames().ToList();
            var countRows = _importer.LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.countRows);
            var countColumns = _importer.LoaderConfig.getOrDefault<int>(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.countColumns);

            // Now create the items
            dgrExcelDataGrid.Columns.Clear();
            var items = new List<object>();
            for (
                var r = 0; 
                r < countRows; 
                r++)
            {
                var item = (IDictionary<string, object>) new ExpandoObject();

                for (
                    var c = 0;
                    c < countColumns;
                    c++)
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

        private bool IsExcelNotLoaded() =>
            !IsInitialized || _importer?.LoaderConfig == null || !_importer.IsExcelLoaded;

        [Flags]
        private enum ContentRange
        {
            None = 0,
            Rows = 1,
            Columns = 1 << 1,
        }

        private void GuessContentRange(ContentRange rangeTypes)
        {
            if (_importer == null)
                throw new InvalidOperationException("_importer == null");
            
            if (rangeTypes.HasFlag(ContentRange.Rows))
            {
                txtCountRow.Text = _importer.GuessRowCount().ToString();
            }

            if (rangeTypes.HasFlag(ContentRange.Columns))
            {
                txtCountColumn.Text = _importer.GuessColumnCount().ToString();
            }
        }

        private void CboSheet_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsExcelNotLoaded()) return;

            txtOffsetRow.Text = "0";
            txtOffsetColumn.Text = "0";
            GuessContentRange(ContentRange.Columns | ContentRange.Rows);
            UpdateDataPreview();
        }

        private void TxtOffsetRow_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_importer == null)
                throw new InvalidOperationException("_importer == null");

            if (IsExcelNotLoaded()) return;

            _importer.LoaderConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.offsetRow, DotNetHelper.AsInteger(txtOffsetRow.Text));
            _importer.GuessRowCount();
            UpdateDataPreview();
        }

        private void TxtOffsetColumn_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_importer == null)
                throw new InvalidOperationException("_importer == null");

            if (IsExcelNotLoaded()) return;

            
            _importer.LoaderConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.offsetColumn, DotNetHelper.AsInteger(txtOffsetColumn.Text));
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
            if (_importer == null)
                throw new InvalidOperationException("_importer == null");

            if (IsExcelNotLoaded()) return;

            _importer.LoaderConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.hasHeader, chkHeaderRow.IsChecked == true);
            _importer.GuessRowCount();
            UpdateDataPreview();
        }

        private void chkAutoCount_OnClick(object sender, RoutedEventArgs e)
        {
            if (_importer == null)
                throw new InvalidOperationException("_importer == null");

            if (IsExcelNotLoaded()) return;

            _importer.LoaderConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.fixRowCount, chkAutoCount.IsChecked == true);
            _importer.LoaderConfig.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.fixColumnCount, chkAutoCount.IsChecked == true);

            _importer.GuessColumnCount();
            _importer.GuessRowCount();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if (_importer == null)
                throw new InvalidOperationException("_importer == null");

            if (IsExcelNotLoaded()) return;

            _importer.GuessColumnCount();
            _importer.GuessRowCount();
            UpdateDataPreview();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            ImportType = ExcelImportType.AsCopy;
            DialogResult = true;
            Close();
        }

        private void btnImportReference_Click(object sender, RoutedEventArgs e)
        {
            ImportType = ExcelImportType.AsReference;
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Gets the configuration as an item
        /// </summary>
        /// <returns>The configuration object describing the elements</returns>
        public IElement? GetConfigurationObject()
            => _importer?.LoaderConfig;

        private void OnClosed(object sender, EventArgs e)
        {
            Owner?.Focus();
        }
    }
}
