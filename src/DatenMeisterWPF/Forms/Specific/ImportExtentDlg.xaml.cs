using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using Microsoft.Win32;

namespace DatenMeisterWPF.Forms.Specific
{
    /// <summary>
    /// Interaktionslogik für ImportExtentDlg.xaml
    /// </summary>
    public partial class ImportExtentDlg : Window
    {
        public IObject ExportCommand { get; set; }

        public ImportExtentDlg()
        {
            InitializeComponent();
            ExportCommand = InMemoryObject.CreateEmpty();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            ExportCommand.set("fileToBeImported", fileToBeImported.Text);
            ExportCommand.set("newExtentName", fileToBeImported.Text);
            ExportCommand.set("fileToBeImported", fileToBeImported.Text);

        }

        private void SourceImportPathClick(object sender, RoutedEventArgs e)
        {
            fileToBeImported.Text = SelectFileNameByUser();
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
