using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.Interfaces;
using DatenMeister.Extent.Manager.ExtentStorage;
using DatenMeister.Integration.DotNet;
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

                excelSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.workspaceId, workspaceId);
                excelSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.extentUri, "dm:///excelimport_" + newGuid);
                excelSettings.set(_DatenMeister._ExtentLoaderConfigs._ExcelImportLoaderConfig.extentPath,  newGuid + ".xmi");

                dlg.Owner = host as Window;
                if (dlg.ShowDialog() == true)
                {
                    var configurationObject = dlg.GetConfigurationObject();
                    if (configurationObject == null) throw new InvalidOperationException("No configuration object set");
                    switch (dlg.ImportType)
                    {
                        case ExcelImportType.AsCopy:
                            GiveMe.Scope.Resolve<ExtentManager>().LoadExtent(configurationObject, ExtentCreationFlags.LoadOrCreate);
                            break;
                        case ExcelImportType.AsReference:
                            GiveMe.Scope.Resolve<ExtentManager>().LoadExtent(configurationObject, ExtentCreationFlags.LoadOrCreate);
                            break;
                    }

                    result.OnClosed();
                }
            }

            return result;
        }
    }
}