using System.Text;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Core.Formatter;

public class TableFormatter
{
    private readonly StringBuilder _result = new();

    /// <summary>
    /// Converts an enumeration of elements to a table like text
    /// </summary>
    /// <param name="collection">Collection to be enumerated</param>
    /// <returns>The string containing all the values</returns>
    public string ConvertToText(IReflectiveCollection collection)
    {
        _result.Clear();
        var columnWidth = new Dictionary<string, int>();
        var properties = ExtentHelper.GetProperties(collection).ToList();

        // Find the lengths
        foreach (var property in properties)
        {
            columnWidth[property] = property.Length + 1;
        }

        foreach (var element in collection)
        {
            var asObject = element as IObject;
            if (asObject == null)
            {
                continue;
            }

            foreach (var property in properties)
            {
                var value = asObject.getOrDefault<string>(property);
                if (value == null)
                {
                    continue;
                }

                columnWidth[property] = Math.Max(columnWidth[property], value.Length + 1);
            }
        }

        // Now, do the output
        foreach (var property in properties)
        {
            Output(property, columnWidth[property]);
        }

        _result.AppendLine();

        foreach (var property in properties)
        {
            Output(string.Empty, columnWidth[property], "-");
        }

        _result.AppendLine();


        foreach (var asObject in collection.OfType<IObject>())
        {
            foreach (var property in properties)
            {
                var value = asObject.getOrDefault<string>(property);
                Output(value ?? string.Empty, columnWidth[property]);
            }

            _result.AppendLine();
        }

        return _result.ToString();
    }

    private void Output(string text, int i, string sep = " ")
    {
        _result.Append(text);
        for (var n = text.Length; n < i; n++)
        {
            _result.Append(sep);
        }
    }

    public static string ToText(IReflectiveCollection collection)
    {
        var formatter = new TableFormatter();
        return formatter.ConvertToText(collection);
    }
}