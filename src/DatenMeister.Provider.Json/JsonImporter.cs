using System.Collections;
using System.Text.Json;
using System.Text.Json.Nodes;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Provider.Json;

public class JsonImporter(IFactory factory)
{
    /// <summary>
    /// Performs the import from the file
    /// </summary>
    /// <param name="fileName">Name of the file</param>
    /// <param name="container">Container in which the result shall be added</param>
    public void ImportFromFile(string fileName, IReflectiveCollection container)
    {
        var jsonContent = File.ReadAllText(fileName);

        ImportFromText(jsonContent, container);

    }

    /// <summary>
    /// Parses the json script from the text and puts it into the container
    /// </summary>
    /// <param name="jsonContent">Content to be parsed</param>
    /// <param name="container">Container to be used</param>
    public void ImportFromText(string jsonContent, IReflectiveCollection container)
    {
        var parsedJson = JsonNode.Parse(jsonContent);
        
        var result = Import(parsedJson);
        if (result is IEnumerable enumerable and not string)
        {
            foreach (var value in enumerable)
            {
                container.add(value);
            }
        }
        else if(result != null)
        {
            container.add(result);
        }
    }

    /// <summary>
    ///  Performs an import of the parsed Json into the container
    /// </summary>
    /// <param name="parsedJson">Json to be parsed</param>
    internal object? Import(object? parsedJson)
    {
        switch (parsedJson)
        {
            case null:
                return null;
            case JsonArray asArray:
            {
                return asArray
                    .Where(x => x is not null)
                    .Select(item => Import(item)!)
                    .ToList();
            }
            case JsonObject asObject:
            {
                var result = factory.create(null);

                foreach (var property in asObject)
                {
                    var propertyName = property.Key;
                    var value = property.Value;

                    var convertedValue = Import(value);
                    if (convertedValue is IEnumerable asEnumerable and not string)
                    {
                        foreach (var enumeratedValue in asEnumerable)
                        {
                            result.AddCollectionItem(propertyName, enumeratedValue);
                        }
                    }
                    else
                    {
                        result.set(propertyName, convertedValue);
                    }
                }

                return result;
            }
            case JsonValue jsonValue:
                switch (jsonValue.GetValueKind())
                {
                    case JsonValueKind.Undefined:
                        return null;
                    case JsonValueKind.Object:
                        return Import(jsonValue.GetValue<JsonObject>());
                    case JsonValueKind.Array:
                        return Import(jsonValue.GetValue<JsonArray>());
                    case JsonValueKind.String:
                        return jsonValue.GetValue<string>();
                    case JsonValueKind.Number:
                        return jsonValue.GetValue<double>();
                    case JsonValueKind.True:
                        return true;
                    case JsonValueKind.False:
                        return false;
                    case JsonValueKind.Null:
                        return null;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            default:
                return parsedJson;
        }
    }
}