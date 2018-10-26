using System;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Provider.XMI.ExtentStorage;
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
                var newGUID = Guid.NewGuid();

                var factory = new Func<IReflectiveCollection>(() =>
                {
                    var configuration = new XmiStorageConfiguration
                    {
                        ExtentUri = "datenmeister:///excelimport_" + newGUID,
                        Path = newGUID + ".xmi",
                        Workspace = workspaceId
                    };

                    var extentManager = App.Scope.Resolve<IExtentManager>();
                    return extentManager.LoadExtent(configuration, true).elements();
                });

                var dlg = new ExcelImportDefinitionDialog();
                var importer = await dlg.PrepareFile(fileDialog.FileName, factory);
                importer.Settings.workspaceId = workspaceId;
                importer.Settings.extentUri = "datenmeister:///excelimport_" + newGUID;
                importer.Settings.extentPath = newGUID + ".xmi";
                
                dlg.Owner = host as Window;
                if (dlg.ShowDialog() == true)
                {
                    result.OnClosed();
                }
            }

            return result;
        }
    }
}