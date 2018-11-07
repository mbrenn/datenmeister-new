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

                await dlg.LoadFile(fileDialog.FileName);

                dlg.ExcelSettings.workspaceId = workspaceId;
                dlg.ExcelSettings.extentUri = "datenmeister:///excelimport_" + newGuid;
                dlg.ExcelSettings.extentPath = newGuid + ".xmi";

                dlg.Owner = host as Window;
                if (dlg.ShowDialog() == true)
                {
                    switch (dlg.ImportType)
                    {
                        case ExcelImportType.AsCopy:
                            ExcelImporter.ImportExcelExtentAsCopy(App.Scope, dlg.GetConfigurationObject());
                            break;
                        case ExcelImportType.AsReference:
                            ExcelImporter.ImportExcelExtentAsReference(App.Scope, dlg.GetConfigurationObject());
                            break;
                    }

                    result.OnClosed();
                }
            }

            return result;
        }
    }
}