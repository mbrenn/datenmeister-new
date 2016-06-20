using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Helper;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Reflection;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.CSV
{
    /// <summary>
    ///     Loads and stores the the extent from an CSV file
    /// </summary>
    public class CSVDataProvider
    {
        private readonly IWorkspaceCollection _workspaceCollection;
        private readonly IDataLayerLogic _dataLayerLogic;

        public CSVDataProvider(IWorkspaceCollection workspaceCollection, IDataLayerLogic dataLayerLogic)
        {
            _workspaceCollection = workspaceCollection;
            _dataLayerLogic = dataLayerLogic;
        }

        /// <summary>
        ///     Loads the CSV Extent out of the settings and stores the extent Uri
        /// </summary>
        /// <param name="extent">The uri being used for an extent</param>
        /// <param name="factory">Factory being used to create a new instance</param>
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
        /// <param name="factory">Factory being used to create a new instance</param>
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
        /// <param name="settings">Settings being used to store it.</param>
        private void ReadFromStream(IExtent extent, IFactory factory, Stream stream, CSVSettings settings)
        {
            if (settings == null)
            {
                settings = new CSVSettings();
            }

            var metaClass = GetMetaClassOfItems(extent, settings);
            var columns = ConvertColumnsToPropertyValues(extent, metaClass, settings);
            var createColumns = false;

            using (var streamReader = new StreamReader(stream, Encoding.GetEncoding(settings.Encoding)))
            {
                if (columns == null)
                {
                    createColumns = true;
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

                    var csvObject = factory.create(metaClass);

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

                        csvObject.set(foundColumn, values[n]);
                    }


                    extent.elements().add(csvObject);
                }
            }
        }

        private IList<string> ConvertColumnsToPropertyValues(IExtent extent, IElement metaClass, CSVSettings settings)
        {
            //////////////////////
            // Loads the workspace
            if (_dataLayerLogic == null)
            {
                throw new InvalidOperationException("No DataLayerLogic was given, even though we need to do Uml navigation");
            }

            var dmml = _dataLayerLogic.GetFromMetaLayer<DmML>(extent);

            var result = (IList<string>) metaClass.get(dmml.Class.Attribute);
            if (result == null)
            {
                result = new List<string>();
                metaClass.set(dmml.Class.Attribute, result);
            }

            return result;
        }

        /// <summary>
        /// Caches the metaclass being stored 
        /// </summary>
        private IElement _metaClassCache;

        /// <summary>
        /// Gets the metaclass as given in the settings
        /// Returns null, if the metaclass is not defined
        /// </summary>
        /// <param name="extent">Extent to be used to find the instance</param>
        /// <param name="settings">Settings being used</param>
        /// <returns>The metaclass of the object</returns>
        private IElement GetMetaClassOfItems(IExtent extent, CSVSettings settings)
        {
            if (_metaClassCache == null)
            {
                IElement metaClass = null;
                if (!string.IsNullOrEmpty(settings.MetaclassUri))
                {
                    if (_workspaceCollection == null)
                    {
                        throw new InvalidOperationException(
                            "Uri by metaclass is given, but we do not have a workspace collection");
                    }

                    // Finds the given item all the workspaces
                    metaClass = _workspaceCollection.FindItem(settings.MetaclassUri);
                    if (metaClass == null)
                    {
                        throw new InvalidOperationException($"Type with ID: {settings.MetaclassUri} was not found");
                    }
                }

                _metaClassCache = metaClass;
            }

            if (_metaClassCache == null)
            {
                // Ok, wie did not find the metaclass, we have to create a new class
                var dmMl = _dataLayerLogic.GetFromMetaLayer<DmML>(extent);
                var mofElement = new MofElement(dmMl.__Class, null);
                mofElement.set(dmMl.Class.Attribute, new MofReflectiveSequence());
            }

            return _metaClassCache;
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
            var columns = new List<string>();
            var dmml = _dataLayerLogic.GetFromMetaLayer<DmML>(extent);
            var metaClass = new ClassWrapper(GetMetaClassOfItems(extent, settings), dmml);
            

            // Retrieve the column headers
            if (settings.HasHeader && metaClass.Attributes.Any())
            {
                // Column headers given by old extent
                columns.AddRange(metaClass.Attributes.Select(x => x.Name));
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
        private void WriteRow(StreamWriter streamWriter, CSVSettings settings, IEnumerable<string> values,
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

                builder.Append(conversion(value).ToString());

                first = false;
            }

            streamWriter.WriteLine(builder.ToString());
        }
    }
}