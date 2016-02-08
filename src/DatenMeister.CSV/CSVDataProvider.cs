using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatenMeister.EMOF.Helper;
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
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                Load(extent, factory, fileStream, settings);
            }
        }

        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="stream">Path being used to load the extent</param>
        /// <param name="settings">Settings to load the extent</param>
        /// <returns>The loaded extent</returns>
        public void Load(IUriExtent extent, IFactory factory, Stream stream, CSVSettings settings)
        {
            ReadFromStream(extent, factory, stream, settings);
        }

        /// <summary>
        ///     Reads the file from the stream
        /// </summary>
        /// <param name="path">Path being used to load the file</param>
        /// <param name="extent">Extet being stored</param>
        /// <param name="settings">
        ///     Settings being used to store it.
        ///     <param name="stream"></param>
        private void ReadFromStream(IUriExtent extent, IFactory factory, Stream stream, CSVSettings settings)
        {
            if (settings == null)
            {
                settings = new CSVSettings();
            }

            using (var streamReader = new StreamReader(stream, Encoding.GetEncoding(settings.Encoding)))
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

        /// <summary>
        ///     Splits a CSV line into columns
        /// </summary>
        /// <returns>List of column values</returns>
        private static IList<string> SplitLine(string line, CSVSettings settings)
        {
            return line.Split(new[] {settings.Separator}, StringSplitOptions.None);
        }

        /// <summary>
        /// Saves the extent into database.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where file shall be stored</param>
        /// <param name="settings">Settings being used</param>
        public void Save(IUriExtent extent, string path, CSVSettings settings)
        {
            var columns = new List<object>();

            // Retrieve the column headers
            if (settings.HasHeader && settings.Columns.Count > 0)
            {
                // Column headers given by old extent
                columns.AddRange(settings.Columns);
            }
            else
            {
                // Column headers given by number by asking each object about the number of properties and
                // then use the maximum value of the elements. This assumes that every element has the same type
                columns = extent.GetProperties().ToList();
            }

            // Open File
            using (var streamWriter = new StreamWriter(File.OpenWrite(path), Encoding.GetEncoding(settings.Encoding)))
            {
                // Writes the header
                if (settings.HasHeader)
                {
                    WriteRow(streamWriter, settings, columns, x => x.ToString());
                }

                // Writes the elements
                foreach (var element in extent.elements().Select(x => x as IObject))
                {
                    WriteRow(
                        streamWriter,
                        settings,
                        columns,
                        x => element.isSet(x) ? element.get(x) : string.Empty);
                }
            }
        }

        /// <summary>
        ///     Writes a columete
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="settings">Settings to be used</param>
        /// <param name="values"></param>
        /// <param name="conversion">Converter to be used, to show the content</param>
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