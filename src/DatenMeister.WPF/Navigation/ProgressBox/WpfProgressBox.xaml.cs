using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
