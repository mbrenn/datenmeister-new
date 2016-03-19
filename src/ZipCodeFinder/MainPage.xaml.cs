using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using DatenMeister.Apps.ZipCode;

// Die Vorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 dokumentiert.

namespace ZipCodeFinder
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            UpdateCity();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtZipCode.Focus(FocusState.Programmatic);
        }

        private void txtZipCode_TextChanged(object sender, TextChangedEventArgs e)
        {            
            UpdateCity();
        }

        private void UpdateCity()
        {
            var amount = 200;
            var foundArray = DataProvider.TheOne.FindBySearchString(txtZipCode.Text)
                .Take(amount);

            var builder = new StringBuilder();

            var any = 0;
            foreach (var found in foundArray)
            {
                any++;
                var cityName = found.get(DataProvider.Columns.Name).ToString();
                var zipCode = found.get(DataProvider.Columns.ZipCode).ToString();

                builder.Append($"{zipCode} {cityName}\r\n");
            }

            if (any == 0)
            {
                txtCity.Text = "Nicht vergeben";
            }
            else

            {
                if (any == amount)
                {
                    builder.Append("...");
                }

                txtCity.Text = builder.ToString();
            }
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var newSize = e.NewSize;
            OnResize(newSize);
        }

        private void OnResize(Size newSize)
        {
            // Performs the scaling
            var fullSize = 1200.0;
            var fontSize = Math.Min(fullSize, newSize.Height) / fullSize * 48;
            txtCity.FontSize = fontSize;
            txtCityHeader.FontSize = fontSize;
            txtZipCode.FontSize = fontSize;
            txtZipCodeHeader.FontSize = fontSize;

            if (newSize.Height > newSize.Width || true)
            {
                // Vertical layout
                columnFirst.Width = new GridLength(1, GridUnitType.Star);
                columnSecond.Width = new GridLength(0, GridUnitType.Pixel);
                rowFirst.Height = GridLength.Auto;
                rowSecond.Height = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(viewBoxLocation, 1);
                Grid.SetColumn(viewBoxLocation, 0);
            }
            else
            {
                // Horizontal layout
                columnFirst.Width = GridLength.Auto;
                columnSecond.Width = new GridLength(70, GridUnitType.Star);
                rowFirst.Height = new GridLength(1, GridUnitType.Star);
                rowSecond.Height = new GridLength(0, GridUnitType.Pixel);
                Grid.SetColumn(viewBoxLocation, 1);
                Grid.SetRow(viewBoxLocation, 0);
            }
        }

        private async void PasteToClipboard_Click(object sender, RoutedEventArgs e)
        {
            var package = new DataPackage();
            package.SetText(txtCity.Text);
            Clipboard.SetContent(package);

            btnClipboard.Content = "...kopiert...";
            await Task.Delay(1000);
            btnClipboard.Content = "In Zwischenablage";

        }
    }
}
