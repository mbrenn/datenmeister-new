#nullable enable

using System;
using System.Text;
using System.Windows;
using BurnSystems.Logging;
using BurnSystems.Logging.Provider;
using DatenMeister.WPF.Helper;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für LogWindow.xaml
    /// </summary>
    public partial class LogWindow : Window
    {
        private readonly DelayedRefreshDispatcher _dispatcher;

        public LogWindow()
        {
            InitializeComponent();

            SelectedLogLevels.ItemsSource = Enum.GetNames(typeof(LogLevel));
            SelectedLogLevels.SelectedValue = TheLog.FilterThreshold.ToString();

            _dispatcher = new DelayedRefreshDispatcher(Dispatcher, UpdateMessageContentDelayed)
            {
                MinDispatchTime = TimeSpan.FromSeconds(1),
                MaxDispatchTime = TimeSpan.FromSeconds(2)
            };
        }

        private void LogWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateMessageContent();

            void Action(object x, LogEventArgs y) => UpdateMessageContent();

            TheLog.MessageLogged += Action;
            Closed += (x, y) => TheLog.MessageLogged -= Action;
        }

        private void UpdateMessageContent()
        {
            _dispatcher.RequestRefresh();
        }

        private void UpdateMessageContentDelayed()
        {
            if (Dispatcher?.CheckAccess() == false)
            {
                // The method is called by the senders of the events. These may be within the 
                // thread context of the application or not. 
                Dispatcher.InvokeAsync(UpdateMessageContent);
            }
            else
            {
                var builder = new StringBuilder();
                foreach (var message in InMemoryDatabaseProvider.TheOne.Messages)
                {
                    builder.AppendLine(message.ToString());
                }

                LogText.Text = builder.ToString();
                LogText.ScrollToEnd();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Copy_To_Clipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(LogText.Text);
            MessageBox.Show("Text copied to clipboard");
        }

        private void SelectedLogLevels_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedValue = SelectedLogLevels.SelectedValue?.ToString();
            if (!string.IsNullOrEmpty(selectedValue))
            {
                TheLog.FilterThreshold =
                    (LogLevel) Enum.Parse(typeof(LogLevel), selectedValue);
            }
        }
    }
}
