using System.Text.Json.Nodes;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.Json;

public class JsonCompressedImporter(IFactory factory, JsonCompressedImporterSettings settings)
{
    /// <summary>
    /// Performs the import from the file
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="container">Container in which the result shall be added</param>
    public void ImportFromFile(string fileName, IReflectiveCollection container)
    {
        Console.WriteLine($"* Importing from file: {fileName}");
        var jsonContent = File.ReadAllText(fileName);
        Console.WriteLine($"** Json loaded");

        ImportFromText(jsonContent, container);
    }
    
    /// <summary>
    /// Parses the json script from the text and puts it into the container
    /// </summary>
    /// <param name="jsonContent">Content to be parsed</param>
    /// <param name="container">Container to be used</param>
    public void ImportFromText(string jsonContent, IReflectiveCollection container)
    {
        var jsonImporter = new JsonImporter(factory);
        var parsedJson = JsonNode.Parse(jsonContent) ??
                         throw new InvalidOperationException("Parsing failed");
        
        Console.WriteLine($"** Json parsed");
        
        var columns = parsedJson[settings.ColumnPropertyName]?.AsArray()
                      ?? throw new InvalidOperationException(
                          $"The column \"{settings.ColumnPropertyName}\" could not be found");

        var columnPositions = new List<string>();

        var currentPosition = 0;
        foreach (var column in columns.Where(x => x != null))
        {
            columnPositions.Add(column!.ToString());
            currentPosition++;
        }

        var data = parsedJson[settings.DataPropertyName]?.AsArray()
                   ?? throw new InvalidOperationException(
                       $"The column \"{settings.DataPropertyName}\" could not be found");

        var x = 0;
        foreach (var item in data.AsArray())
        {
            x++;
            if (x % 100 == 0)
            {
                Console.Write($"** {x}/{data.Count} converted\r");
            }
            
            // We found something, now perform the conversion
            // Confirm that array is an item
            var itemAsArray = item?.AsArray();
            if (itemAsArray == null)
            {
                throw new InvalidOperationException("The item is an array");
            }

            if (itemAsArray.Count != columnPositions.Count)
            {
                throw new InvalidOperationException(
                    $"We have a mismatch of array length. " +
                    $"{itemAsArray.Count} != {columnPositions.Count}");
            }

            // Now perform the conversion
            currentPosition = 0;
            var createdItem = factory.create(null);
            container.add(createdItem);
            foreach (var cell in itemAsArray)
            {
                if (settings.FilterProperty(columnPositions[currentPosition]))
                {
                    object? convertedCell = cell;
                    
                    // Perform the conversion
                    if (settings.ConvertProperty != null)
                    {
                        convertedCell = settings.ConvertProperty(columnPositions[currentPosition], cell);
                    }

                    // Only the items which shall be managed are really imported

                    createdItem.set(
                        columnPositions[currentPosition],
                        jsonImporter.Import(convertedCell));
                }

                currentPosition++;
            }
        }

        Console.WriteLine();
    }
}