using System.Threading.Tasks;
using DatenMeisterWPF.Windows;
using Microsoft.Win32;

namespace DatenMeisterWPF.Navigation
{
    public static class ImportFromExcelNavigation
    {
        /// <summary>
        /// Performs the import from excel
        /// </summary>
        /// <param name="host">Host being used</param>
        public static void ImportFromExcel(INavigationHost host)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Excel-Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*"
            };

            if (fileDialog.ShowDialog() == true)
            {
                var dlg = new ExcelImportDefinitionDialog();
                dlg.ShowDialog();
            }
        }
    }
}