using System.Text;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces.MOF.Common;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Provider;
using DatenMeister.Core.Provider;
using DatenMeister.Core.Uml.Helper;

namespace DatenMeister.Core.Formatter;

/// <summary>
/// Offers a support function, which converts the data of an extent into a human-readable
/// </summary>
public class ExtentFormatter
{
    /// <summary>
    /// Defines the builder being used to create the text
    /// </summary>
    private readonly StringBuilder _builder = new();

    /// <summary>
    /// Stores a string to create the current indentation
    /// </summary>
    private string _currentIndent = string.Empty;

    /// <inheritdoc />
    public override string ToString() =>
        _builder.ToString();

    /// <summary>
    /// Converts the given extent to a text string
    /// </summary>
    /// <param name="extent">Extent to be exported</param>
    /// <returns>The created text</returns>
    private string ConvertToText(IExtent extent)
    {
        _builder.Clear();

        if (extent is IUriExtent uriExtent)
        {
            _builder.AppendLine($"URIExtent: {uriExtent.contextURI()}");
        }
        else
        {
            _builder.AppendLine("Extent");
        }

        IncreaseIndentation();
        Parse(extent.elements());
        DecreaseIndentation();

        return _builder.ToString();
    }

    private void Parse(IReflectiveCollection elements)
    {
        foreach (var element in elements)
        {
            if (element == null)
            {
                _builder.AppendLine($"{_currentIndent}null");
            }
            else if (DotNetHelper.IsOfPrimitiveType(element))
            {
                _builder.AppendLine($"{_currentIndent}{element}");
            }
            else if (DotNetHelper.IsOfMofObject(element))
            {
                Parse((MofObject) element);
            }
        }
    }

    /// <summary>
    /// Parses a mof object
    /// </summary>
    /// <param name="parsedValue">Mof object</param>
    private void Parse(IObject parsedValue)
    {
        var mofObject = (MofObject) parsedValue;
        var asProperties = (IObjectAllProperties) mofObject;
        if (mofObject is IElement)
        {
            var mofElement = (MofElement) parsedValue;
            _builder.AppendLine(
                $"{_currentIndent}Element '{NamedElementMethods.GetName(mofObject)}' [#{mofElement.Id}] of type: {NamedElementMethods.GetName(mofElement.getMetaClass())}");
        }
        else
        {
            _builder.AppendLine($"{_currentIndent}Object '{NamedElementMethods.GetName(mofObject)}'");
        }

        IncreaseIndentation();
        foreach (var property in asProperties.getPropertiesBeingSet())
        {
            var value = mofObject.get(property, true, ObjectType.None);

            if (DotNetHelper.IsOfPrimitiveType(value))
            {
                _builder.AppendLine($"{_currentIndent}{property}: {value}");
            }
            else if (DotNetHelper.IsOfMofObject(value))
            {
                _builder.AppendLine($"{_currentIndent}{property}:");
                IncreaseIndentation();
                Parse((MofObject) value!);
                DecreaseIndentation();
            }
            else if (DotNetHelper.IsOfReflectiveCollection(value))
            {
                _builder.AppendLine($"{_currentIndent}{property}[]:");
                IncreaseIndentation();
                Parse((IReflectiveCollection) value!);
                DecreaseIndentation();
            }
            else if (value is UriReference reference)
            {
                _builder.AppendLine($"{_currentIndent}{property}->: {reference.Uri}");
            }
        }

        DecreaseIndentation();
    }

    /// <summary>
    /// Increases the indentation
    /// </summary>
    private void IncreaseIndentation()
    {
        _currentIndent += "  ";
    }

    /// <summary>
    /// Decreases indentation
    /// </summary>
    private void DecreaseIndentation()
    {
        _currentIndent = _currentIndent.Substring(2);
    }

    /// <summary>
    /// Converts the given extent to a text string
    /// </summary>
    /// <param name="extent">Extent to be exported</param>
    /// <returns>The created text</returns>
    public static string ToText(IExtent extent)
    {
        var formatter = new ExtentFormatter();
        return formatter.ConvertToText(extent);
    }

    /// <summary>
    /// Converts the given extent to a text string
    /// </summary>
    /// <param name="collection">Collection to be exported</param>
    /// <returns>The created text</returns>
    public static string ToText(IReflectiveCollection collection)
    {
        var formatter = new ExtentFormatter();
        formatter.Parse(collection);
        return formatter.ToString();
    }

    /// <summary>
    /// Converts the given extent to a text string
    /// </summary>
    /// <param name="mofObject">MofObject to be exported</param>
    /// <returns>The created text</returns>
    public static string ToText(IObject mofObject)
    {
        var formatter = new ExtentFormatter();
        formatter.Parse(mofObject);
        return formatter.ToString();
    }
}