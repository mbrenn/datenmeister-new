using System.Windows;

namespace DatenMeister.WPF.Windows
{
    /// <summary>
    /// Interaktionslogik für QueryElementDialog.xaml
    /// </summary>
    public partial class QueryElementDialog : Window
    {
        public QueryElementDialog()
        {
            InitializeComponent();
            QueryUrl.Focus();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void OpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
