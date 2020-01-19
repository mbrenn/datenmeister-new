using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Excel.Helper;
using DatenMeister.Integration;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.ExtentStorage.Interfaces;
using DatenMeister.WPF.Windows;
using Microsoft.Win32;

namespace DatenMeister.WPF.Navigation
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
                var excelSettings =
                    dlg.ExcelSettings ?? throw new InvalidOperationException("dlg.ExcelSettings == null");

                excelSettings.workspaceId = workspaceId;
                excelSettings.extentUri = "datenmeister:///excelimport_" + newGuid;
                excelSettings.extentPath = newGuid + ".xmi";

                dlg.Owner = host as Window;
                if (dlg.ShowDialog() == true)
                {
                    var configurationObject = dlg.GetConfigurationObject();
                    if (configurationObject == null) throw new InvalidOperationException("No configuration object set");
                    switch (dlg.ImportType)
                    {
                        case ExcelImportType.AsCopy:
                            var importSettings =
                                DotNetConverter.ConvertToDotNetObject<ExcelImportSettings>(configurationObject);
                            GiveMe.Scope.Resolve<IExtentManager>().LoadExtent(importSettings, ExtentCreationFlags.LoadOrCreate);
                            break;
                        case ExcelImportType.AsReference:
                            var referenceSettings =
                                DotNetConverter.ConvertToDotNetObject<ExcelReferenceSettings>(configurationObject);
                            GiveMe.Scope.Resolve<IExtentManager>().LoadExtent(referenceSettings, ExtentCreationFlags.LoadOrCreate);
                            break;
                    }

                    result.OnClosed();
                }
            }

            return result;
        }
    }
}