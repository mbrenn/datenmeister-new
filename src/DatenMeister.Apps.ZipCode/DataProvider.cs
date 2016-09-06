﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.CSV;
using DatenMeister.Runtime.Functions.Queries;

namespace DatenMeister.Apps.ZipCode
{
    public class DataProvider
    {
        public static DataProvider TheOne { get; }

        public MofUriExtent ZipCodes
        {
            get;
            private set;
        }

        static DataProvider()
        {
            TheOne = new DataProvider();
        }

        public void LoadZipCodes(Stream stream)
        {
            var csvSettings = new CSVSettings
            {
                Encoding = "UTF-8",
                Separator = '\t',
                HasHeader = false
            };

            ZipCodes = new MofUriExtent("datenmeister:///zipcodes");
            var factory = new MofFactory();

            var csvProvider = new CSVDataProvider(null, null);

            csvProvider.Load(
                ZipCodes,
                factory,
                stream,
                csvSettings);

            Columns.ZipCode = csvSettings.Columns[1];
            Columns.Name = csvSettings.Columns[4];

            Debug.WriteLine($"Loaded: {ZipCodes.elements().Count()} Zipcodes");
        }

        /// <summary>
        /// Finds a specific zip code by search string
        /// </summary>
        /// <param name="searchString">String to be used for searching</param>
        /// <returns>Enumeration of objects</returns>
        public IEnumerable<IObject> FindBySearchString(string searchString)
        {
            var columns = new[] { Columns.Name, Columns.ZipCode };
            var typedZipCode = searchString.Trim();
            var found = ZipCodes.elements()
                .WhenOneOfThePropertyContains(columns,
                    typedZipCode,
                    StringComparison.CurrentCultureIgnoreCase);
            
            return found.Cast<IObject>();
        }


        public static class Columns
        {
            public static string ZipCode
            {
                get;
                set;
            }

            public static string Name
            {
                get;
                set;
            }
        }
    }
}
