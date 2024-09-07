using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Provider.CSV
{
    /// <summary>
    ///     Loads and stores the the extent from an CSV file
    /// </summary>
    public class CsvLoader
    {
        private readonly IWorkspaceLogic? _workspaceLogic;

        public CsvLoader(IWorkspaceLogic? workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="path">Path being used to load the extent</param>
        /// <param name="settings">Settings to load the extent. Of Type CsvSettings</param>
        /// <returns>The loaded extent</returns>
        public void Load(IProvider extent, string path, IElement? settings)
        {
            using var fileStream = new FileStream(path, FileMode.Open);

            Load(extent, fileStream, settings);
        }

        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="stream">Path being used to load the extent</param>
        /// <param name="settings">Settings to load the extent. Of Type CsvSettings</param>
        /// <returns>The loaded extent</returns>
        public void Load(IProvider extent, Stream stream, IElement? settings)
        {
            ReadFromStream(extent, stream, settings);
        }

        /// <summary>
        ///     Reads the file from the stream
        /// </summary>
        /// <param name="extent">Extet being stored</param>
        /// <param name="stream">Stream being used to read in</param>
        /// <param name="settings">Settings being used to store it.</param>
        private void ReadFromStream(IProvider extent, Stream stream, IElement? settings)
        {
            var columns =
                settings?.getOrDefault<IReflectiveCollection>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.columns);
            var hasHeader =
                settings?.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.hasHeader) ?? true;

            var trimCells =
                settings?.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.trimCells) ?? false;
            var createColumns = false;

            IElement? metaClass = null;
            var metaClassUri =
                settings?.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.metaclassUri) ?? null;

            if (_workspaceLogic != null && !string.IsNullOrEmpty(metaClassUri))
            {
                metaClass = _workspaceLogic.FindElement(metaClassUri);
            }

            using var streamReader = new StreamReader(stream, Encoding.GetEncoding(
                settings.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.encoding) ?? "UTF-8"));
            var tempColumns = columns?.OfType<string>().ToList();
                
            if (tempColumns == null)
            {
                createColumns = true;
                tempColumns = new List<string>();
            }

            // Reads header, if necessary
            if (hasHeader)
            {
                tempColumns.Clear();

                // Creates the column names for the headline
                var headerLine = streamReader.ReadLine();
                if (headerLine != null)
                {
                    var columnNames = SplitLine(headerLine, settings);
                    foreach (var columnName in columnNames)
                    {
                        tempColumns.Add(EvaluateString(columnName));
                    }
                }
            }

            // Reads the data itself
            string? line;
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
                    if (tempColumns.Count <= n && (createColumns || !hasHeader))
                    {
                        // Create new column
                        foundColumn = $"Column {n + 1}";
                        tempColumns.Add(foundColumn);
                    }
                    else
                    {
                        foundColumn = tempColumns[n];
                    }

                    csvObject.SetProperty(foundColumn, EvaluateString(values[n]));
                }

                extent.AddElement(csvObject);
            }
                
            settings?.set(_DatenMeister._ExtentLoaderConfigs._CsvSettings.columns, tempColumns);

            string EvaluateString(string value)
            {
                return trimCells ? value.Trim() : value;
            }

        }

        /// <summary>
        ///     Splits a CSV line into columns
        /// </summary>
        /// <returns>List of column values</returns>
        private static IList<string> SplitLine(string line, IElement? settings)
        {
            char separator = ' ';
            if (settings != null)
            {
                separator = settings.getOrDefault<char>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.separator);
            }
            
            return line.Split(
                new[] {separator},
                StringSplitOptions.None);
        }

        /// <summary>
        /// Saves the extent into database.
        /// </summary>
        /// <param name="extent">Extent to be stored</param>
        /// <param name="path">Path, where file shall be stored</param>
        /// <param name="settings">Settings being used. Of type CSV Settings</param>
        public void Save(IProvider extent, string path, IElement settings)
        {
            var encoding = settings.getOrDefault<string>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.encoding) ?? "UTF-8";
            // Open File
            using (var streamWriter = new StreamWriter(File.OpenWrite(path), Encoding.GetEncoding(encoding)))
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
        public void SaveToStream(TextWriter streamWriter, IProvider extent, IElement settings)
        {
            var csvColumns =
                settings.getOrDefault<IReflectiveCollection>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.columns);
            var hasHeader =
                settings.getOrDefault<bool>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.hasHeader);

            var columns = new List<string>();

            // Retrieve the column headers
            if (hasHeader && csvColumns.Any())
            {
                // Column headers given by old extent
                columns.AddRange(csvColumns.OfType<string>().ToList());
            }
            else
            {
                // Column headers given by number by asking each object about the number of properties and
                // then use the maximum value of the elements. This assumes that every element has the same type
                // If no value can be retrieved, the already set columns will be retrieved, which is an empty list
                columns = extent.GetRootObjects().ElementAtOrDefault(0)?.GetProperties()?.ToList() ?? columns;
            }

            // Writes the header
            if (hasHeader)
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
            IElement settings,
            IEnumerable<string> values,
            Func<string, object> conversion)
        {
            var separator = settings.getOrDefault<char>(_DatenMeister._ExtentLoaderConfigs._CsvSettings.separator);
            var builder = new StringBuilder();
            var first = true;
            foreach (var value in values)
            {
                if (!first)
                {
                    builder.Append(separator);
                }

                var cellValue = DotNetHelper.AsString(conversion(value));
                if (cellValue != null && cellValue.Contains(separator))
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