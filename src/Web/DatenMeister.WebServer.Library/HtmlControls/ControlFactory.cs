using System.Collections.Generic;
using System.Text;
using System.Web;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Forms;
using DatenMeister.HtmlEngine;
using DatenMeister.Modules.ZipCodeExample;
using DatenMeister.TextTemplates;

namespace DatenMeister.WebServer.Library.HtmlControls
{
    /// <summary>
    /// The control factory creates the Html Elements dependent on the
    /// content and value of the items and the field information
    /// </summary>
    public static class ControlFactory
    {
        /// <summary>
        /// Gets the html element for a certain value and the field information itself is also evaluated
        /// </summary>
        /// <param name="item">Item to be evaluated</param>
        /// <param name="field">Field definition for the value</param>
        /// <param name="scriptLines">The JavaScript lines being usable</param>
        /// <param name="contextWorkspace">The workspace in which the element is residing</param>
        /// <param name="contextExtent">The extent in which the element is residing</param>
        /// <returns>The created Html Element</returns>
        public static HtmlElement GetHtmlElementForItemsField(
            IObject item,
            IElement field,
            StringBuilder scriptLines,
            string contextWorkspace,
            string contextExtent)
        {
            void WriteScriptLineFunction(string htmlId, string functionName, params string?[] parameters)
            {
                var notFirst = false;
                scriptLines.Append(
                    $"$('#{htmlId}').click(function() " +
                    $"{{DatenMeister.FormActions.{functionName}(");

                foreach (var parameter in parameters)
                {
                    if (notFirst)
                    {
                        scriptLines.Append(", ");
                    }

                    scriptLines.Append($"'{HttpUtility.JavaScriptStringEncode(parameter ?? string.Empty)}'");
                    notFirst = true;
                }
                
                scriptLines.AppendLine(");});");
            }
            
            void WriteScriptLines(string htmlId, string functionName, IWorkspace? workspace, IUriExtent? extent, string? itemId)
            {
                WriteScriptLineFunction(
                    htmlId, functionName, workspace?.id, extent?.contextURI(), itemId);
            }
            
            void WriteScriptLinesWithContext(string htmlId, string functionName, string? itemId)
            {
                WriteScriptLineFunction(
                    htmlId, functionName, contextWorkspace, contextExtent, itemId);
            }

            (IWorkspace? workspace, IUriExtent? extent, string? itemId) GetWorkspaceExtentAndItemId(IObject item)
            {
                var extent = item?.GetUriExtentOf();
                var workspace = extent?.GetWorkspace();
                var itemId = (item as IHasId)?.Id ?? string.Empty;
                return (workspace, extent, itemId);
            }

            var isEnumeration =
                field.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration);

            // Checks, if the value is an action field
            if (field.getMetaClassWithoutTracing()?.equals(_DatenMeister.TheOne.Forms.__ActionFieldData) == true)
            {
                var id = HtmlElement.GetRandomId();
                var button = new HtmlButtonElement(
                    field.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.title))
                {
                    Id = id,
                    CssClass = "btn btn-secondary"
                };

                var actionType = field.getOrDefault<string>(_DatenMeister._Forms._ActionFieldData.actionName);
                var itemAsElement = item as IElement;
                if (actionType == ExtentFormPlugin.NavigationExtentNavigateTo && itemAsElement is not null)
                {
                    WriteScriptLineFunction(id, "extentNavigateTo",
                        itemAsElement.getOrDefault<string>(_DatenMeister._Management._Extent.workspaceId),
                        itemAsElement.getOrDefault<string>(_DatenMeister._Management._Extent.uri));
                }

                if (actionType == ItemsFormsPlugin.NavigationItemDelete && itemAsElement is not null)
                {
                    var (workspace, extent, itemId) = GetWorkspaceExtentAndItemId(itemAsElement);
                    WriteScriptLines(id, "itemDelete", workspace, extent, itemId);
                }

                if (actionType == ExtentFormPlugin.NavigationItemNew && itemAsElement is not null)
                {
                    WriteScriptLineFunction(id, "itemNew",
                        itemAsElement.getOrDefault<string>(_DatenMeister._Management._Extent.workspaceId),
                        itemAsElement.getOrDefault<string>(_DatenMeister._Management._Extent.uri));
                }

                if (actionType == ItemsFormsPlugin.NavigationExtentsListViewItem && itemAsElement is not null)
                {
                    var (workspace, extent, itemId) = GetWorkspaceExtentAndItemId(itemAsElement);
                    WriteScriptLinesWithContext(id, "extentsListViewItem", itemId);
                }

                if (actionType == ItemsFormsPlugin.NavigationExtentsListDeleteItem &&
                    itemAsElement is not null)
                {
                    var (workspace, extent, itemId) = GetWorkspaceExtentAndItemId(itemAsElement);
                    WriteScriptLinesWithContext(id, "extentsListDeleteItem", itemId);
                }

                if (actionType == ZipCodePlugin.CreateZipExample && itemAsElement is not null)
                {
                    WriteScriptLineFunction(id, "createZipExample",
                        itemAsElement.getOrDefault<string>(_DatenMeister._Management._Workspace.id));
                }

                return button;
            }

            var value = GetValueOfElement(item, field);

            // Checks, if the element is null
            if (value == null)
            {
                return new HtmlRawString("<em>null</em>");
            }

            // Checks, if the value is an enumeration, so a list will be shown
            if (isEnumeration || DotNetHelper.IsEnumeration(value.GetType()))
            {
                var valueAsList = DotNetHelper.AsEnumeration(value);
                if (valueAsList != null)
                {
                    var elementCount = 0;

                    var htmlList = new HtmlListElement();
                    foreach (var valueElement in valueAsList)
                    {
                        htmlList.Items.Add(
                            GetHtmlElementForValue(
                                valueElement,
                                scriptLines,
                                contextWorkspace,
                                contextExtent));

                        elementCount++;
                        if (elementCount > 10)
                        {
                            htmlList.Items.Add(new HtmlDivElement(new HtmlRawString("<em>(more)</em>")));
                            break;
                        }
                    }

                    return htmlList;
                }

                return new HtmlDivElement(new HtmlRawString("<em>Shall not occur.</em>"));
            }

            // Checks, if the value is a reference
            if (value is IElement and IHasId asHasId)
            {
                return new HtmlDivElement(
                    new HtmlLinkElement
                    {
                        Href = "/Item/" + HttpUtility.HtmlAttributeEncode(HttpUtility.UrlEncode(contextWorkspace)!) +
                               "/" + HttpUtility.HtmlAttributeEncode(HttpUtility.UrlEncode(contextExtent)!) +
                               "/" + HttpUtility.HtmlAttributeEncode(HttpUtility.UrlEncode(asHasId.Id)!),
                        Content = NamedElementMethods.GetName(value)
                    });
            }

            // Default fallback
            return GetHtmlElementForValue(value, scriptLines, contextWorkspace, contextExtent);
        }

        /// <summary>
        /// Gets the html for a certain element, just describing the value itself.
        /// The field's property is already parsed.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="scriptLines"></param>
        /// <param name="contextWorkspace"></param>
        /// <param name="contextExtent"></param>
        /// <returns></returns>
        public static HtmlElement GetHtmlElementForValue(
            object value,
            StringBuilder scriptLines,
            string contextWorkspace,
            string contextExtent)
        {

            // Checks, if the element is an object shadow whose value is retrieved automatically
            if (value is MofObjectShadow mofObjectShadow)
            {
                return CreateHtmlForObjectShadow(mofObjectShadow, scriptLines);
            }

            // Checks, if the element is a reference
            if (value is IElement and IHasId asHasId)
            {
                return new HtmlDivElement(
                    new HtmlLinkElement
                    {
                        Href = "/Item/" + HttpUtility.HtmlAttributeEncode(HttpUtility.UrlEncode(contextWorkspace)!) +
                               "/" + HttpUtility.HtmlAttributeEncode(HttpUtility.UrlEncode(contextExtent)!) +
                               "/" + HttpUtility.HtmlAttributeEncode(HttpUtility.UrlEncode(asHasId.Id)!),
                        Content = NamedElementMethods.GetName(value)
                    });
            }

            return new HtmlDivElement(NamedElementMethods.GetName(value));

        }

        private static HtmlElement CreateHtmlForObjectShadow(MofObjectShadow mofObjectShadow, StringBuilder scriptLines)
        {
            var id = HtmlElement.GetRandomId();
            var result = new HtmlDivElement("Loading")
            {
                Id = id
            };

            scriptLines.AppendLine(
                "DatenMeister.DomHelper.injectNameByUri($('#" + id + "')," +
                "encodeURIComponent('" + HttpUtility.JavaScriptStringEncode(mofObjectShadow.Uri) + "')" +
                ");");

            return result;
        }

        /// <summary>
        /// Gets the value of the element by the field
        /// </summary>
        /// <param name="element">Element being queried</param>
        /// <param name="field">Field being used for query</param>
        /// <returns>Returned element for the </returns>
        public static object? GetValueOfElement(IObject element, IElement field)
        {
            var fieldMetaClass = field?.getMetaClass();
            if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
            {
                return (element as MofElement)?.getMetaClass(false);
            }

            var name = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__EvalTextFieldData) == true)
            {
                var cellInformation = InMemoryObject.CreateEmpty();
                var defaultText = name != null ? element.getOrDefault<string>(name) : string.Empty;
                cellInformation.set("text", defaultText);

                var evalProperties = field.getOrDefault<string>(_DatenMeister._Forms._EvalTextFieldData.evalCellProperties);
                if (evalProperties != null)
                {
                    defaultText = TextTemplateEngine.Parse(
                        evalProperties,
                        new Dictionary<string, object>
                        {
                            ["i"] = element,
                            ["c"] = cellInformation
                        });
                }

                return cellInformation.isSet("text")
                    ? cellInformation.getOrDefault<string>("text")
                    : defaultText;
            }

            return element.isSet(name) ? element.get(name) : null;
        }
    }
}