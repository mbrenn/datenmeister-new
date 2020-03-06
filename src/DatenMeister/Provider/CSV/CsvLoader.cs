using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.CSV
{
    /// <summary>
    ///     Loads and stores the the extent from an CSV file
    /// </summary>
    public class CsvLoader
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        public CsvLoader(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="path">Path being used to load the extent</param>
        /// <param name="settings">Settings to load the extent</param>
        /// <returns>The loaded extent</returns>
        public void Load(IProvider extent, string path, CsvSettings settings)
        {
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                Load(extent, fileStream, settings);
            }
        }

        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="stream">Path being used to load the extent</param>
        /// <param name="settings">Settings to load the extent</param>
        /// <returns>The loaded extent</returns>
        public void Load(IProvider extent, Stream stream, CsvSettings settings)
        {
            ReadFromStream(extent, stream, settings);
        }

        /// <summary>
        ///     Reads the file from the stream
        /// </summary>
        /// <param name="extent">Extet being stored</param>
        /// <param name="stream">Stream being used to read in</param>
        /// <param name="settings">Settings being used to store it.</param>
        private void ReadFromStream(IProvider extent, Stream stream, CsvSettings settings)
        {
            if (settings == null)
            {
                settings = new CsvSettings();
            }

            var columns = settings.Columns;
            var createColumns = false;

            IElement? metaClass = null;
            if (!string.IsNullOrEmpty(settings.MetaclassUri))
            {
                metaClass = _workspaceLogic.FindItem(settings.MetaclassUri);
            }

            using (var streamReader = new StreamReader(stream, Encoding.GetEncoding(settings.Encoding)))
            {
                if (columns == null)
                {
                    createColumns = true;
                    columns = new List<string>();
                }

                // Reads header, if necessary
                if (settings.HasHeader)
                {
                    columns.Clear();

                    // Creates the column names for the headline
                    var ignoredLine = streamReader.ReadLine();
                    var columnNames = SplitLine(ignoredLine, settings);
                    foreach (var columnName in columnNames)
                    {
                        columns.Add(columnName);
                    }
                }

                // Reads the data itself
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var values = SplitLine(line, settings);

                    var csvObject = extent.CreateElement(metaClass?.GetUri());

                    // we now have the created object, let's fill it
                    var valueCount = values.Count;
                    for (var n = 0; n < valueCount; n++)
                    {
                        string foundColumn;

                        // Check, if we have enough columns, if we don't have enough columns, create one
                        if (columns.Count <= n && (createColumns || !settings.HasHeader))
                        {
                            // Create new column
                            foundColumn = $"Column {n + 1}";
                            columns.Add(foundColumn);
                        }
                        else
                        {
                            foundColumn = columns[n];
                        }

                        csvObject.SetProperty(foundColumn, values[n]);
                    }


                    extent.AddElement(csvObject);
                }
            }
        }

        /// <summary>
        ///     Splits a CSV line into columns
        /// </summary>
        /// <returns>List of column values</returns>
        private static IList<string> SplitLine(string line, CsvSettings settings)
        {
            return line.Split(new[] {settings.Separator}, StringSplitOptions.None);
        }

        /// <summary>
        /// Saves the extent into database.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where file shall be stored</param>
        /// <param name="settings">Settings being used</param>
        public void Save(IProvider extent, string path, CsvSettings settings)
        {
            // Open File
            using (var streamWriter = new StreamWriter(File.OpenWrite(path), Encoding.GetEncoding(settings.Encoding)))
            {
                SaveToStream(streamWriter, extent, settings);
            }
        }

        /// <summary>
        /// Saves the csv to a datastream
        /// </summary>
        /// <param name="streamWriter">Stream, where data will be stored</param>
        /// <param name="extent">Extent being stored</param>
        /// <param name="settings">Settings of the csv</param>
        public void SaveToStream(TextWriter streamWriter, IProvider extent, CsvSettings settings)
        {
            var columns = new List<string>();

            // Retrieve the column headers
            if (settings.HasHeader && settings.Columns.Any())
            {
                // Column headers given by old extent
                columns.AddRange(settings.Columns);
            }
            else
            {
                // Column headers given by number by asking each object about the number of properties and
                // then use the maximum value of the elements. This assumes that every element has the same type
                // If no value can be retrieved, the already set columns will be retrieved, which is an empty list
                columns = extent.GetRootObjects().ElementAtOrDefault(0)?.GetProperties()?.ToList() ?? columns;
            }

            // Writes the header
            if (settings.HasHeader)
            {
                WriteRow(streamWriter, settings, columns, x => x.ToString());
            }

            // Writes the elements
            foreach (var element in extent.GetRootObjects().Select(x => x))
            {
                WriteRow(
                    streamWriter,
                    settings,
                    columns,
                    x => element.IsPropertySet(x) 
                        ? element.GetProperty(x) ?? string.Empty
                        : string.Empty);
            }
        }

        /// <summary>
        ///     Writes a columete
        /// </summary>
        /// <param name="streamWriter"></param>
        /// <param name="settings">Settings to be used</param>
        /// <param name="values"></param>
        /// <param name="conversion">Converter to be used, to show the content</param>
        private void WriteRow(
            TextWriter streamWriter,
            CsvSettings settings,
            IEnumerable<string> values,
            Func<string, object> conversion)
        {
            var builder = new StringBuilder();
            var first = true;
            foreach (var value in values)
            {
                if (!first)
                {
                    builder.Append(settings.Separator);
                }

                var cellValue = DotNetHelper.AsString(conversion(value));
                if (cellValue != null && cellValue.Contains(settings.Separator))
                {
                    cellValue = $"\"{cellValue.Replace("\"", "\"\"")}\"";
                }

                builder.Append(cellValue);
                first = false;
            }

            streamWriter.WriteLine(builder.ToString());
        }
    }
}