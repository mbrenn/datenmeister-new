#nullable enable

using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using DatenMeister.Runtime;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
            
            var entryAssembly = Assembly.GetEntryAssembly();
            TxtVersionNumber.Text = 
                entryAssembly != null
                ? "v" +
                  entryAssembly
                      .GetCustomAttributes(typeof(AssemblyFileVersionAttribute))
                      .Cast<AssemblyFileVersionAttribute>()
                      .Select(x => x.Version)
                      .FirstOrDefault()
                : "[Unknown Version]";
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            DotNetHelper.CreateProcess(e.Uri.ToString());
        }

        private void AboutDialog_OnClosed(object sender, EventArgs e)
        {
            Owner?.Focus();
        }
    }
}
