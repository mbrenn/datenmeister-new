using System.Text;
using System.Windows;
using BurnSystems.Logging.Provider;

namespace DatenMeisterWPF.Windows
{
    /// <summary>
    /// Interaktionslogik für LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();
        }

        private void LogWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var builder = new StringBuilder();
            foreach (var message in InMemoryDatabaseProvider.TheOne.Messages)
            {
                builder.AppendLine(message.ToString());
            }

            LogText.Text = builder.ToString();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
