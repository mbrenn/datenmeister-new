using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using DatenMeister.Core.Helper;

namespace DatenMeister.WPF.Windows;

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

        var environmentBuilder = new StringBuilder();
        environmentBuilder.AppendLine("OS Version: " + Environment.OSVersion);
        environmentBuilder.AppendLine(".Net Version: " + Environment.Version);
        environmentBuilder.AppendLine("64 Bit OS: " + (Environment.Is64BitOperatingSystem? "Yes" : "No"));
        environmentBuilder.AppendLine("64 Bit Process: " + (Environment.Is64BitProcess ? "Yes" : "No"));
        environmentBuilder.AppendLine(string.Empty);

        foreach (var loadedAssembly in AppDomain.CurrentDomain.GetAssemblies().OrderBy(x=>x.GetName().Name))
        {
            environmentBuilder.AppendLine($" - {loadedAssembly.GetName().Name}: {loadedAssembly.GetName().Version}");
        }
        TxtEnvironment.Text = environmentBuilder.ToString();
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