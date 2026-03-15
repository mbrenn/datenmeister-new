﻿using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Web;
using System.Text.Json.Nodes;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Web.Json;

/// <summary>
/// Converts an object or a mof element to a json string in which
/// the metaclass and other meta information is available for the json interface.
///
/// The corresponding JavaScript function Mof.ts::createObjectFromJson
/// </summary>
public class MofJsonConverter
{
    /// <summary>
    ///     Stores the extent from which the resolving is started. This information is used to
    ///     evaluate the ResolveReferenceToOtherExtents flag properly
    /// </summary>
    private IExtent? rootExtent;

    /// <summary>
    /// Defines the maximum recursion depth being allowed to be converted the elements
    /// </summary>
    public int MaxRecursionDepth { get; set; } = 10;

    /// <summary>
    ///     Gets or sets the flag whether references to other extents shall also be resolved or whether
    ///     they shall just be given as a reference and the client side is responsible to resolve it.
    /// </summary>
    public bool ResolveReferenceToOtherExtents { get; set; } = false;

    /// <summary>
    ///     Gets or sets the flag whether composite properties shall be resolved recursively,
    ///     ignoring the MaxRecursionDepth limit for composite properties.
    ///     When enabled, the converter will continue resolving composite properties beyond the normal depth limit.
    /// </summary>
    public bool ResolveCompositesRecursively { get; set; } = false;
    
    /// <summary>
    /// Stores the current indentation
    /// </summary>
    public string IndentString { get; set; } = "";
    
    /// <summary>
    /// Stores the indentation string which added for each 'level-in' 
    /// </summary>
    public const string IndentStringTemplate = "  ";

    /// <summary>
    /// Increases the current indentation level by appending the predefined
    /// indentation string to the internal indent representation. This is used
    /// to structure the debug text or JSON output for better readability.
    /// </summary>
    public void IncreaseIndent()
    {
        IndentString += IndentStringTemplate;
    }

    /// <summary>
    /// Decreases the current indentation level by removing the predefined
    /// indentation string from the internal indent representation. This is used
    /// to maintain structure in the debug text or JSON output when exiting a nested level.
    /// </summary>
    public void DecreaseIndent()
    {
        IndentString = IndentString.Substring(0, IndentString.Length - IndentStringTemplate.Length);
    }

    /// <summary>
    /// Converts the object to a json object by just converting it to a string and then back to an object
    /// </summary>
    /// <param name="value">Value to be converted</param>
    /// <returns>The resulting object</returns>
    public JsonObject? ConvertToJsonObject(object? value)
    {
        var jsonAsString = ConvertToJsonString(value);
        return JsonNode.Parse(jsonAsString)?.AsObject();
    }

    /// <summary>
    ///     Converts the given element to a json object
    /// </summary>
    /// <param name="value">Value to be converted</param>
    /// <returns>The converted element</returns>
    public string ConvertToJsonString(object? value)
    {
        var builder = new StringBuilder();

        rootExtent =
            GetConnectedExtent(value);

        AppendValue(
            builder,
            value,
            -1, /* starts with -1 since AppendValue will directly increase the value */
            isComposite: true);
        return builder.ToString();
    }

    private static IExtent? GetConnectedExtent(object? value)
    {
        return (value is IHasExtent hasExtent
                   ? hasExtent.Extent
                   : null) ??
               (value is MofObject asMofObject
                   ? asMofObject.ReferencedExtent
                   : null);
    }

    /// <summary>
    /// Converts the given element to a json object
    /// </summary>
    /// <param name="value">Value to be converted</param>
    /// <returns>The converted value</returns>
    public static string ConvertToJsonStringWithDefaultParameter(IObject value)
    {
        return new MofJsonConverter().ConvertToJsonString(value);
    }

    /// <summary>
    /// Converts the given value to a Json object by using the string builder
    /// </summary>
    /// <param name="builder">The string builder to be used</param>
    /// <param name="value">Value to be converted</param>
    /// <param name="isReference">true, if the value is a reference</param>
    /// <param name="recursionDepth">Defines the recursion depth to be handled</param>
    private void ConvertToJsonString(StringBuilder builder, IObject value, bool isReference, int recursionDepth = 0)
    {
        if (value is MofObjectShadow asShadow)
        {
            // Element is of type MofObjectShadow and can just be referenced
            builder.Append($"\r\n{IndentString}{{\"r\": \"{HttpUtility.JavaScriptStringEncode(asShadow.Uri)}\"}}");
            
            return;
        }

        if (value is not IObjectAllProperties allProperties)
        {
            throw new ArgumentException("value is not of type IObjectAllProperties.");
        }

        builder.Append('{');
        IncreaseIndent();

        // Creates the values
        var komma = string.Empty;

        builder.Append($"\r\n{IndentString}\"v\": {{");
        var classModel = (value as MofObject)?.GetClassModel();
        var propertyList = classModel != null
            ? classModel.Attributes.Select(x => x.Name)
            : allProperties.getPropertiesBeingSet();
        
        foreach (var property in propertyList)
        {
            var isPropertySet = value.isSet(property);
            builder.AppendLine(komma);
            builder.Append($"{IndentString}\"{HttpUtility.JavaScriptStringEncode(property)}\": ");
            var propertyValue = value.get(property);

            var attributeModel = classModel?.FindAttribute(property);
            var isComposite = attributeModel?.IsComposite != false;

            AppendValue(builder, new[] {isPropertySet, propertyValue}, recursionDepth, isComposite: isComposite);

            komma = ",";
        }
        
        DecreaseIndent();
        builder.Append("}");

        // Creates the metaclass
        if (value is IElement asElement)
        {
            var item = ItemWithNameAndId.Create(asElement.getMetaClass());
            if (item != null)
            {
                builder.Append($",\r\n{IndentString}\"m\": ");
                item.AppendJson(builder, true);
            }
        }
            
        // Creates the id
        var id = value.GetId();
        if (!string.IsNullOrEmpty(id))
        {
            builder.Append($",\r\n{IndentString}\"id\": ");
            AppendValue(builder, id);
        }

        // Creates the uri
        var uri = value.GetUri();
        if (uri != null)
        {
            builder.Append($",\r\n{IndentString}\"u\": ");
            AppendValue(builder, uri);

            if (isReference)
            {
                builder.Append($",\r\n{IndentString}\"r\": ");
                AppendValue(builder, uri);

                if (value.GetUriExtentOf()?.GetWorkspace() == null)
                {
                    Debugger.Break();
                }
            }
        }

        var extent = value.GetUriExtentOf();
        if (extent != null)
        {
            var contextUri = extent.contextURI();
            builder.Append($",\r\n{IndentString}\"e\": ");
            AppendValue(builder, contextUri);
        }

        var workspace = extent?.GetWorkspace();
        if (workspace != null)
        {
            var workspaceName = workspace.id;
            builder.Append($",\r\n{IndentString}\"w\": ");
            AppendValue(builder, workspaceName);
        }

        builder.Append("}");


        builder.AppendLine();
    }

    /// <summary>
    /// Converts the value
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="propertyValue"></param>
    /// <param name="recursionDepth">Defines the recursion Depth</param>
    /// <param name="forceReference">true, if the reference shall be forced</param>
    /// <param name="isComposite">true, if the property is a composite</param>
    private void AppendValue(StringBuilder builder, object? propertyValue, int recursionDepth = 0, bool forceReference = false, bool isComposite = false)
    {
        var connectedExtent = GetConnectedExtent(propertyValue);
        var forceReferenceByExtent =
            !ResolveReferenceToOtherExtents
            && rootExtent != null
            && rootExtent != connectedExtent
            && !isComposite;
        if (isComposite)
        {
            forceReference = false;
        }

        // If the item is of another extent, the recursion depth will be set, so there will be no deeper 
        // parsing. If a deeper parsing is required, the client shall explicitly query
        var isOtherExtent = rootExtent != null && connectedExtent != null && rootExtent != connectedExtent;
        if (isOtherExtent)
        {
            recursionDepth = Math.Max(recursionDepth, MaxRecursionDepth - 1);
        }

        // Check if we should stop recursion based on depth
        // If ResolveCompositesRecursively is enabled and this is a composite property, ignore depth limit
        // If it is not a composite, just reference it.
        var shouldStopRecursion =
            recursionDepth >= MaxRecursionDepth && !(ResolveCompositesRecursively && isComposite);

        if (propertyValue is IObject asObject && (shouldStopRecursion || forceReference || forceReferenceByExtent))
        {
            // Try to resolve the item to find the workspace, unfortunately, MofObjectShadow does not include the workspace
            if (asObject is MofObjectShadow mofObjectShadow)
            {
                asObject = rootExtent?.Resolve(mofObjectShadow) ?? mofObjectShadow;
            }

            // Create the element
            builder.Append($"{{");
            IncreaseIndent();

            builder.Append($"\r\n{IndentString}\"r\": \"{HttpUtility.JavaScriptStringEncode(asObject.GetUri() ?? "None")}\"");
            var workspace = asObject.GetExtentOf()?.GetWorkspace();
            if (workspace != null)
            {
                var workspaceName = workspace.id;
                builder.Append($",\r\n{IndentString}\"w\": ");
                AppendValue(builder, workspaceName);
            }

            DecreaseIndent();
            builder.Append($"\r\n{IndentString}}}");
            return;
        }

        if (DotNetHelper.IsNull(propertyValue) || propertyValue == null)
        {
            builder.Append("null");
        }
        else if (DotNetHelper.IsOfBoolean(propertyValue))
        {
            builder.Append((bool)propertyValue ? "true" : "false");
        }
        else if (propertyValue is double propertyValueAsDouble)
        {
            builder.Append(propertyValueAsDouble.ToString(CultureInfo.InvariantCulture));
        }
        else if (DotNetHelper.IsOfDateTime(propertyValue))
        {
            builder.Append($"\"{DotNetHelper.AsString(propertyValue)}\"");
        }
        else if (DotNetHelper.IsOfPrimitiveType(propertyValue))
        {
            builder.Append($"\"{HttpUtility.JavaScriptStringEncode(propertyValue.ToString())}\"");
        }
        else if (DotNetHelper.IsOfMofObject(propertyValue))
        {
            ConvertToJsonString(builder, (propertyValue as IObject)!, !isComposite, recursionDepth + 1);
        }
        else if (DotNetHelper.IsOfEnumeration(propertyValue)
                 && propertyValue is IEnumerable enumeration)
        {
            Debug.Assert(enumeration != null, "enumeration != null");

            builder.Append("[");
            IncreaseIndent();
            var komma = string.Empty;
            foreach (var innerValue in enumeration)
            {
                builder.AppendLine(komma);
                builder.Append(IndentString);
                AppendValue(builder, innerValue, recursionDepth + 1, forceReference, isComposite);
                komma = ",";
            }

            DecreaseIndent();
            builder.Append($"\r\n{IndentString}]");
        }
        else
        {
            builder.Append($"\"{propertyValue}\"");
        }
    }
}