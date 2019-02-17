using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;

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
            TxtVersionNumber.Text = "v" +
                Assembly.GetAssembly(typeof(AboutDialog))
                    .GetCustomAttributes(typeof(AssemblyFileVersionAttribute))
                    .Cast<AssemblyFileVersionAttribute>()
                    .Select(x => x.Version)
                    .FirstOrDefault() ?? "[Unknown version]";
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
        }
    }
}
