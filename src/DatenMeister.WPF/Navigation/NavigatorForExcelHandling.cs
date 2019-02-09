using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
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
                            var importSettings =
                                DotNetConverter.ConvertToDotNetObject<ExcelImportSettings>(dlg.GetConfigurationObject());
                            GiveMe.Scope.Resolve<IExtentManager>().LoadExtent(importSettings, true);
                            break;
                        case ExcelImportType.AsReference:
                            var referenceSettings =
                                DotNetConverter.ConvertToDotNetObject<ExcelReferenceSettings>(dlg.GetConfigurationObject());
                            GiveMe.Scope.Resolve<IExtentManager>().LoadExtent(referenceSettings, true);
                            break;
                    }

                    result.OnClosed();
                }
            }

            return result;
        }
    }
}