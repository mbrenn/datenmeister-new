using DatenMeister.CSV;
using DatenMeister.EMOF.InMemory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace ZipCodeFinder.Logic
{
    class DataProvider
    {
        private static DataProvider Singleton;

        public static DataProvider TheOne
        {
            get { return Singleton; }
        }

        public MofUriExtent ZipCodes
        {
            get;
            private set;
        }

        static DataProvider()
        {
            Singleton = new DataProvider();
        }

        public async void LoadZipCodes()
        {
            var csvSettings = new CSVSettings();
            csvSettings.Encoding = Encoding.UTF8;// Encoding.GetEncoding("ISO-8859-1");
            csvSettings.Separator = '\t';
            csvSettings.HasHeader = false;

            ZipCodes = new MofUriExtent("datenmeister:///zipcodes");
            var factory = new MofFactory();

            var csvProvider = new CSVDataProvider();

            var file = await Package.Current.InstalledLocation.GetFileAsync(@"Assets\data\plz.csv");
            var stream = await file.OpenStreamForReadAsync();

            csvProvider.Load(
                ZipCodes,
                factory,
                stream,
                csvSettings);

            Columns.ZipCode = csvSettings.Columns[1];
            Columns.Name = csvSettings.Columns[4];

            Debug.WriteLine($"Loaded: {ZipCodes.elements().Count().ToString()} Zipcodes");
        }


        public static class Columns
        {
            public static object ZipCode
            {
                get;
                set;
            }

            public static object Name
            {
                get;
                set;
            }
        }
    }
}
