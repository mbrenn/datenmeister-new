using System.Windows;

namespace DatenMeister.WPF.Navigation.ProgressBox
{
    /// <summary>
    /// Interaktionslogik für WpfProgressBox.xaml
    /// </summary>
    public partial class WpfProgressBox : Window, IProgressBox
    {
        public WpfProgressBox()
        {
            InitializeComponent();
        }

        /// <inheritdoc />
        public void SetProgress(double percentage, string text)
        {
            Dispatcher?.InvokeAsync(() =>
            {
                TxtStatus.Text = text;
                if (percentage < 0.0)
                {
                    ProgressBar.IsIndeterminate = true;
                }
                else
                {
                    ProgressBar.Maximum = 1.0;
                    ProgressBar.IsIndeterminate = false;
                    ProgressBar.Value = percentage;
                }
            });
        }

        /// <inheritdoc />
        public void CloseProgress()
        {
            Dispatcher?.InvokeAsync(Close);
        }
    }
}
