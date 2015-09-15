using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZipCodeFinder.Logic;

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

        private void txtZipCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateCity();
        }

        private void UpdateCity()
        {
            var found = Filter.WhenPropertyIs(
                    DataProvider.TheOne.ZipCodes.elements(),
                    DataProvider.Columns.ZipCode,
                    txtZipCode.Text)
                .FirstOrDefault();

            if (found != null)
            {
                var cityName = (found as IObject).get(DataProvider.Columns.Name).ToString();
                var zipCode = (found as IObject).get(DataProvider.Columns.ZipCode).ToString();
                txtCity.Text = $"{zipCode} {cityName}";
            }
            else
            {
                txtCity.Text = "Unbekannt";
            }
        }
    }
}
