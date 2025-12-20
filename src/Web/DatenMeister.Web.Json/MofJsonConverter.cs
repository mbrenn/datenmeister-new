using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Web;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Identifiers;
using DatenMeister.Core.Interfaces.MOF.Reflection;

namespace DatenMeister.Web.Json;

/// <summary>
/// Converts an object or a mof element to a json element in which
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
    ///     Converts the given element to a json object
    /// </summary>
    /// <param name="value">Value to be converted</param>
    /// <returns>The converted element</returns>
    public string ConvertToJson(object? value)
    {
        var builder = new StringBuilder();

        rootExtent =
            GetConnectedExtent(value);

        AppendValue(builder, value, -1 /* starts with -1 since AppendValue will directly increase the value */);
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
    public static string ConvertToJsonWithDefaultParameter(IObject value)
    {
        return new MofJsonConverter().ConvertToJson(value);
    }

    /// <summary>
    /// Converts the given value to a Json object by using the string builder
    /// </summary>
    /// <param name="builder">The string builder to be used</param>
    /// <param name="value">Value to be converted</param>
    /// <param name="recursionDepth">Defines the recursion depth to be handled</param>
    private void ConvertToJson(StringBuilder builder, IObject value, int recursionDepth = 0)
    {
        if (value is MofObjectShadow asShadow)
        {
            // Element is of type MofObjectShadow and can just be referenced
            builder.Append($"{{\"r\": \"{HttpUtility.JavaScriptStringEncode(asShadow.Uri)}\"}}");
            return;
        }

        if (value is not IObjectAllProperties allProperties)
        {
            throw new ArgumentException("value is not of type IObjectAllProperties.");
        }

        builder.Append('{');

        // Creates the values
        var komma = string.Empty;

        builder.Append("\"v\": {");
        var classModel = (value as MofObject)?.GetClassModel();
        foreach (var property in allProperties.getPropertiesBeingSet())
        {
            builder.AppendLine(komma);
            builder.Append($"\"{HttpUtility.JavaScriptStringEncode(property)}\": ");
            var propertyValue = value.get(property);

            var attributeModel = classModel?.FindAttribute(property);
            var isComposite = attributeModel?.IsComposite ?? false; // Default to true if no model is found

            // TODO: We are prepared just to return composites, but do not do it.
            AppendValue(builder, propertyValue, recursionDepth/*, !isComposite*/);

            komma = ",";
        }

        builder.Append("}");

        // Creates the metaclass
        if (value is IElement asElement)
        {
            var item = ItemWithNameAndId.Create(asElement.getMetaClass());
            if (item != null)
            {
                builder.Append(", \"m\": ");
                item.AppendJson(builder);
            }
        }
            
        // Creates the id
        var id = value.GetId();
        if (!string.IsNullOrEmpty(id))
        {
            builder.Append(", \"id\": ");
            AppendValue(builder, id);
        }

        // Creates the uri
        var uri = value.GetUri();
        if (uri != null)
        {
            builder.Append(", \"u\": ");
            AppendValue(builder, uri);
        }

        var extent = value.GetUriExtentOf();
        if (extent != null)
        {
            var contextUri = extent.contextURI();
            builder.Append(", \"e\": ");
            AppendValue(builder, contextUri);
        }

        var workspace = extent?.GetWorkspace();
        if (workspace != null)
        {
            var workspaceName = workspace.id;
            builder.Append(", \"w\": ");
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
    private void AppendValue(StringBuilder builder, object? propertyValue, int recursionDepth = 0, bool forceReference = false)
    {
        var connectedExtent = GetConnectedExtent(propertyValue);
        var forceReferenceByExtent = !ResolveReferenceToOtherExtents
                             && rootExtent != null
                             && rootExtent != connectedExtent;

        // If the item is of another extent, the recursion depth will be set, so there will be no deeper 
        // parsing. If a deeper parsing is required, the client shall explicitly query
        var isOtherExtent = rootExtent != null && connectedExtent != null && rootExtent != connectedExtent;
        if (isOtherExtent)
        {
            recursionDepth = Math.Max(recursionDepth, MaxRecursionDepth - 1);
        }

        if (propertyValue is IObject asObject && (recursionDepth >= MaxRecursionDepth || forceReference || forceReferenceByExtent))
        {
            // Try to resolve the item to find the workspace, unfortunately, MofObjectShadow does not include the workspace
            if (asObject is MofObjectShadow mofObjectShadow)
            {
                asObject = rootExtent?.Resolve(mofObjectShadow) ?? mofObjectShadow;
            }

            // Create the element
            builder.Append("{");

            builder.Append($"\"r\": \"{HttpUtility.JavaScriptStringEncode(asObject.GetUri() ?? "None")}\"");
            var workspace = asObject.GetExtentOf()?.GetWorkspace();
            if (workspace != null)
            {
                var workspaceName = workspace.id;
                builder.Append(", \"w\": ");
                AppendValue(builder, workspaceName);
            }

            builder.Append("}");
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
            ConvertToJson(builder, (propertyValue as IObject)!, recursionDepth + 1);
        }
        else if (DotNetHelper.IsOfEnumeration(propertyValue)
                 && propertyValue is IEnumerable enumeration)
        {
            Debug.Assert(enumeration != null, "enumeration != null");

            builder.Append("[");
            var komma = string.Empty;
            foreach (var innerValue in enumeration)
            {
                builder.AppendLine(komma);
                AppendValue(builder, innerValue, recursionDepth + 1, forceReference);
                komma = ",";
            }

            builder.Append("]");
        }
        else
        {
            builder.Append($"\"{propertyValue}\"");
        }
    }
}