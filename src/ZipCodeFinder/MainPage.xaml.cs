using DatenMeister.App.ZipCode;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            this.txtZipCode.Focus(FocusState.Programmatic);
        }

        private void txtZipCode_TextChanged(object sender, TextChangedEventArgs e)
        {            
            UpdateCity();
        }

        private void UpdateCity()
        {
            var typedZipCode = txtZipCode.Text.Trim();
            var found = Filter.WhenPropertyStartsWith(
                    DataProvider.TheOne.ZipCodes.elements(),
                    DataProvider.Columns.ZipCode,
                    typedZipCode)
                .FirstOrDefault();

            if (found != null)
            {
                var cityName = (found as IObject).get(DataProvider.Columns.Name).ToString();
                var zipCode = (found as IObject).get(DataProvider.Columns.ZipCode).ToString();
                txtCity.Text = $"{zipCode} {cityName}";
            }
            else
            {
                txtCity.Text = "Nicht vergeben";
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
            var fontSize = Math.Min(fullSize, newSize.Height) / fullSize * 64;
            txtCity.FontSize = fontSize;
            txtCityHeader.FontSize = fontSize;
            txtZipCode.FontSize = fontSize;
            txtZipCodeHeader.FontSize = fontSize;

            if (newSize.Height > newSize.Width)
            {
                // Vertical layout
                columnFirst.Width = new GridLength(1, GridUnitType.Star);
                columnSecond.Width = new GridLength(0, GridUnitType.Pixel);
                rowFirst.Height = GridLength.Auto;
                rowSecond.Height = new GridLength(1, GridUnitType.Star);
                Grid.SetRow(viewBoxLocation, 1);
                Grid.SetColumn(viewBoxLocation, 0);
                viewBoxZip.VerticalAlignment = VerticalAlignment.Top;
                viewBoxLocation.VerticalAlignment = VerticalAlignment.Top;
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
                viewBoxZip.VerticalAlignment = VerticalAlignment.Center;
                viewBoxLocation.VerticalAlignment = VerticalAlignment.Center;
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
