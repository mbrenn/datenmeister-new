using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.CSV
{
    /// <summary>
    ///     Loads and stores the the extent from an CSV file
    /// </summary>
    public class CSVDataProvider
    {
        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="path">Path being used to load the extent</param>
        /// <param name="settings">Settings to load the extent</param>
        /// <returns>The loaded extent</returns>
        public void Load(IUriExtent extent, IFactory factory, string path, CSVSettings settings)
        {
            ReadFromFile(path, extent, factory, settings);
        }

        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="path">Path being used to load the extent</param>
        /// <param name="settings">Settings to load the extent</param>
        /// <returns>The loaded extent</returns>
        public void Load(IUriExtent extent, IFactory factory, Stream stream, CSVSettings settings)
        {
            ReadFromStream(extent, factory, settings, stream);
        }

        /// <summary>
        ///     Reads an extent from file
        /// </summary>
        /// <param name="path">Path being used to load the file</param>
        /// <param name="extent">Extet being stored</param>
        /// <param name="settings">
        ///     Settings being used to store it.
        ///     When the settings are null, a default setting will be loaded
        /// </param>
        private void ReadFromFile(string path, IUriExtent extent, IFactory factory, CSVSettings settings)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                ReadFromStream(extent, factory, settings, fileStream);
            }
        }

        /// <summary>
        ///     Reads the file from the stream
        /// </summary>
        /// <param name="path">Path being used to load the file</param>
        /// <param name="extent">Extet being stored</param>
        /// <param name="settings">
        ///     Settings being used to store it.
        ///     <param name="stream"></param>
        public void ReadFromStream(IUriExtent extent, IFactory factory, CSVSettings settings, Stream stream)
        {
            if (settings == null)
            {
                settings = new CSVSettings();
            }

            using (var streamReader = new StreamReader(stream, settings.Encoding))
            {
                var createColumns = false;
                // Reads header, if necessary
                if (settings.HasHeader)
                {
                    // TODO: Do not skip first line...
                    //extent.HeaderNames.AddRange(this.SplitLine(stream.ReadLine(), settings));
                    var ignoredLine = streamReader.ReadLine();
                }

                if (settings.Columns == null)
                {
                    settings.Columns = new List<object>();
                    createColumns = true;
                }

                // Reads the data itself
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var values = SplitLine(line, settings);

                    var csvObject = factory.create(null);

                    // we now have the created object, let's fill it
                    var valueCount = values.Count;
                    for (var n = 0; n < valueCount; n++)
                    {
                        object foundColumn;
                        if (settings.Columns.Count <= n && createColumns)
                        {
                            // Create new column
                            foundColumn = $"Column {n + 1}";
                            settings.Columns.Add(foundColumn);
                        }
                        else
                        {
                            foundColumn = settings.Columns[n];
                        }

                        csvObject.set(foundColumn, values[n]);
                    }


                    extent.elements().add(csvObject);
                }
            }
        }

        /*
        /// <summary>
        /// Saves the extent into database.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where file shall be stored</param>
        /// <param name="settings">Settings being used</param>
        public void Save(IURIExtent extent, string path, CSVSettings settings)
        {
            var columnHeaders = new List<string>();
            var extentAsCSV = extent as CSVExtent;

            // Retrieve the column headers
            if (settings.HasHeader && extentAsCSV != null && extentAsCSV.HeaderNames.Count > 0)
            {
                // Column headers given by old extent
                columnHeaders.AddRange(extentAsCSV.HeaderNames);
            }
            else
            {
                // Column headers given by number by asking each object about the number of properties and
                // then use the maximum value of the elements. This assumes that every element has the same type
                var maxColumnCount = extent.Elements().Select(x => x.AsIObject().getAll().Count()).Max();
                for (var n = 0; n < maxColumnCount; n++)
                {
                    columnHeaders.Add(string.Format("Column {0}", n));
                }
            }

            // Open File
            using (var streamWriter = new StreamWriter(path, false, settings.Encoding))
            {
                // Writes the header
                if (settings.HasHeader)
                {
                    this.WriteRow(streamWriter, settings, columnHeaders, x => x);
                }

                // Writes the elements
                foreach (var element in extent.Elements().Select(x => x.AsIObject()))
                {
                    this.WriteRow(
                        streamWriter,
                        settings,
                        columnHeaders,
                        x => element.get(x));
                }
            }
        }*/

        /// <summary>
        ///     Splits a CSV line into columns
        /// </summary>
        /// <returns>List of column values</returns>
        private IList<string> SplitLine(string line, CSVSettings settings)
        {
            return line.Split(new[] {settings.Separator}, StringSplitOptions.None);
        }

        /// <summary>
        ///     Writes a columete
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="values"></param>
        private void WriteRow<T>(StreamWriter streamWriter, CSVSettings settings, IEnumerable<T> values,
            Func<T, object> conversion)
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var value in values)
            {
                if (!first)
                {
                    builder.Append(settings.Separator);
                }

                builder.Append(conversion(value).ToString());

                first = false;
            }

            streamWriter.WriteLine(builder.ToString());
        }
    }
}