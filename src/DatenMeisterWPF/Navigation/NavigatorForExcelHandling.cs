using System;
using System.Threading.Tasks;
using System.Windows;
using DatenMeister.Excel.Helper;
using DatenMeisterWPF.Windows;
using Microsoft.Win32;

namespace DatenMeisterWPF.Navigation
{
    public static class NavigatorForExcelHandling
    {
        /// <summary>
        /// Performs the import from excel
        /// </summary>
        /// <param name="host">Host being used</param>
        /// <param name="workspaceId">Id of the workspace into which the excel shall be imported</param>
        public static async Task<IControlNavigation> ImportFromExcel(INavigationHost host, string workspaceId)
        {
            var result = new ControlNavigation();
            var fileDialog = new OpenFileDialog
            {
                Filter = "Excel-Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*"
            };

            if (fileDialog.ShowDialog() == true)
            {
                var newGuid = Guid.NewGuid();
                var dlg = new ExcelImportDefinitionDialog();

                var importer = await dlg.LoadFile(fileDialog.FileName);
                importer.Settings.workspaceId = workspaceId;
                importer.Settings.extentUri = "datenmeister:///excelimport_" + newGuid;
                importer.Settings.extentPath = newGuid + ".xmi";

                dlg.Owner = host as Window;
                if (dlg.ShowDialog() == true)
                {
                    switch (dlg.ImportType)
                    {
                        case ExcelImportType.AsCopy:
                            ExcelImporter.ImportExcelAsCopy(App.Scope, dlg.GetConfigurationObject());
                            break;
                        case ExcelImportType.AsReference:
                            ExcelImporter.ImportExcelAsReference(App.Scope, dlg.GetConfigurationObject());
                            break;
                    }

                    result.OnClosed();
                }
            }

            return result;
        }
    }
}