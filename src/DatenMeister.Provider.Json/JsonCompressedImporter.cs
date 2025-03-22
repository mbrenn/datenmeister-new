using System.Text.Json.Nodes;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Provider.Json;

public class JsonCompressedImporter
{
    
    /// <summary>
    /// Defines the name in which the compressed data is given
    /// </summary>
    public string ColumnPropertyName { get; set; } = "columns";
    
    /// <summary>
    /// Defines the name in which the compressed data is given
    /// </summary>
    public string DataPropertyName { get; set; } = "data";

    /// <summary>
    /// Performs the import from the file
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="container">Container in which the result shall be added</param>
    public void ImportFromFile(string fileName, IReflectiveCollection container)
    {
        var jsonContent = File.ReadAllText(fileName);
        var parsedJson = JsonNode.Parse(jsonContent)
                         ?? throw new InvalidOperationException($"Json could not be loaded: {fileName}");


        var columns = parsedJson[ColumnPropertyName]?.AsArray()
                      ?? throw new InvalidOperationException($"The column {ColumnPropertyName} could not be found");

        var columnPositions = new List<string>();
        var backwardsPosition = new Dictionary<string, int>();

        var currentPosition = 0;
        foreach (var column in columns.Where(x => x != null))
        {
            columnPositions.Add(column.ToString());
            backwardsPosition[column.ToString()] = currentPosition;

            currentPosition++;
        }

        var data = parsedJson[DataPropertyName]?.AsArray()
                   ?? throw new InvalidOperationException($"The column {DataPropertyName} could not be found");

        foreach (var item in data.AsArray())
        {
            // We found something, now perform the conversion
        }
    }
}