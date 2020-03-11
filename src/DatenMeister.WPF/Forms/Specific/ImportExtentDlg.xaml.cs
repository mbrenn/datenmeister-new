using System;
using System.Windows;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.Extents;
using DatenMeister.Runtime.ExtentStorage;
using Microsoft.Win32;

namespace DatenMeister.WPF.Forms.Specific
{
    /// <summary>
    /// Interaktionslogik für ImportExtentDlg.xaml
    /// </summary>
    public partial class ImportExtentDlg : Window
    {
        public IObject? ImportCommand { get; set; }

        /// <summary>
        /// Gets or sets the workspace id of the workspace that shall import the new extent
        /// </summary>
        public string? Workspace { get; set; }

        public ImportExtentDlg()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            ImportCommand = InMemoryObject.CreateEmpty();
            ImportCommand.set(nameof(ImportSettings.fileToBeImported), sourceFilename.Text);
            ImportCommand.set(nameof(ImportSettings.newExtentUri), newExtentUri.Text);
            ImportCommand.set(nameof(ImportSettings.fileToBeExported), targetFilename.Text);
            ImportCommand.set(nameof(ImportSettings.Workspace), Workspace);
            Close();
        }

        private void SourceImportPathClick(object sender, RoutedEventArgs e)
        {
            sourceFilename.Text = SelectFileNameByUser();

            // Checks, if the newExtentUri is empty... If yes, try to get the name of the extent and prefills it
            if (string.IsNullOrEmpty(newExtentUri.Text))
            {
                try
                {
                    var loader = new XmiStorage();
                    var provider = loader.LoadProvider(new XmiStorageConfiguration("dm:///dm_temp")
                        {
                            filePath = sourceFilename.Text
                        },
                        ExtentCreationFlags.LoadOnly);

                    var extent = new MofUriExtent(provider.Provider);
                    newExtentUri.Text = extent.contextURI();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private void TargetImportPathClick(object sender, RoutedEventArgs e)
        {
            targetFilename.Text = SelectFileNameByUser();
        }

        private static string SelectFileNameByUser()
        {
            string resultingFilename;
            var dlg = new OpenFileDialog
            {
                Filter = "Xmi-Files (*.xmi)|*.xmi|All Files (*.*)|*.*", RestoreDirectory = true
            };
            if (dlg.ShowDialog() == true)
            {
                resultingFilename = dlg.FileName;
            }
            else
            {
                resultingFilename = string.Empty;
            }

            return resultingFilename;
        }
    }
}
