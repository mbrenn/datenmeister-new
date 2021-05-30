﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using BurnSystems;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.HtmlEngine;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Scriban.Syntax;

namespace DatenMeister.WebServer.Pages
{
    public class ItemModel : PageModel
    {
        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string Extent { get; set; } = string.Empty;

        [Parameter] public string? Item { get; set; } = string.Empty;
        
        private ExtentItemsController ExtentItemsController { get; set; }

        public IObject? FoundItem { get; set; }

        private IObject? Form { get; set; }

        public readonly List<IElement> Fields = new();

        /// <summary>
        /// Stores the enumeration of tables
        /// </summary>
        public readonly List<HtmlTable> Tables = new();

        public readonly StringBuilder ScriptLines = new StringBuilder();
        public ItemAndFormModel? ItemAndFormModel;

        public ItemModel(ExtentItemsController extentItemsController)
        {
            ExtentItemsController = extentItemsController;
        }

        public void OnGet(string workspace, string extent, string? item)
        {
            Tables.Clear();
            
            Workspace = WebUtility.UrlDecode(workspace);
            Extent = WebUtility.UrlDecode(extent);
            Item = WebUtility.UrlDecode(item);
            
            ItemAndFormModel = ExtentItemsController.GetItemAndForm(Workspace, Extent, Item);
            if (ItemAndFormModel == null)
            {
                throw new InvalidOperationException($"Items not found: {Workspace}/{Extent}/{Item}");
            }

            if (ItemAndFormModel == null)
            {
                throw new InvalidOperationException($"Items not found: {Workspace}/{Extent}/{Item}");
            }

            FoundItem = XmiHelper.ConvertItemFromXmi(ItemAndFormModel.item)
                        ?? throw new InvalidOperationException("Items are null. They may not be null");
            Form = XmiHelper.ConvertItemFromXmi(ItemAndFormModel.form)
                   ?? throw new InvalidOperationException("Form is null. It may not be null");

            ConsolidateFields(Fields, Form);

            var htmlTable = new HtmlTable
            {
                CssClass = "table table-striped table-bordered table-sm align-top"
            };

            htmlTable.AddRow(
                new HtmlTableRow(
                    new[]
                    {
                        new HtmlTableCell("Key") {IsHeading = true},
                        new HtmlTableCell("Value") {IsHeading = true}
                    })
            );
            
            foreach (var field in Fields)
            {
                var name = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
                
                // Gets the cell content of the first column
                var titleField = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.title);
                
                var fieldMetaClass = field.getMetaClassWithoutTracing();
                if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
                {
                    titleField = "Metaclass";
                }
                if (string.IsNullOrEmpty(titleField))
                {
                    titleField = name;
                }
                
                var value = GetHtmlElementOfItemsField(FoundItem, field, ScriptLines,Workspace, Extent);

                var rowItem = new HtmlTableRow(
                    new[]
                    {
                        new HtmlTableCell(new HtmlRawString(HttpUtility.HtmlEncode(titleField))
                            {ConvertSpaceToNbsp = true}),
                        new HtmlTableCell(value)
                    });

                htmlTable.AddRow(rowItem);
            }
            
            Tables.Add(htmlTable);
        }

        public static HtmlElement GetHtmlElementOfItemsField(
            IObject foundItem,
            IElement field, 
            StringBuilder scriptLines,
            string contextWorkspace,
            string contextExtent
            )
        {
            // Gets the cell content of the second column
            var value = GetValueOfElement(foundItem, field);
            return GetHtmlElementForValue(
                value, field, scriptLines, contextWorkspace, contextExtent);
        }

        /// <summary>
        /// Gets the html element for a certain element within the the field
        /// </summary>
        /// <param name="value">Value to be evaluated</param>
        /// <param name="field">Field definition for the value</param>
        /// <param name="scriptLines">The JavaScript lines being usable</param>
        /// <returns>The created Html Element</returns>
        public static HtmlElement GetHtmlElementForValue(
            object? value, 
            IElement? field, 
            StringBuilder scriptLines,
            string contextWorkspace,
            string contextExtent)
        {
            var isEnumeration =
                field?.getOrDefault<bool>(_DatenMeister._Forms._FieldData.isEnumeration) ?? false;

            if (value == null)
            {
                return new HtmlRawString("<em>null</em>");
            }

            if (value is MofObjectShadow mofObjectShadow)
            {
                return CreateHtmlForObjectShadow(mofObjectShadow, scriptLines);
            }

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
                                null,
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

                value = new HtmlDivElement("Shall not occur.");
            }

            if (value is IElement and IHasId asHasId)
            {

                    return new HtmlDivElement(
                        new HtmlLinkElement
                        {
                            Href = "/Item/" + HttpUtility.HtmlAttributeEncode(WebUtility.UrlEncode(contextWorkspace)) +
                                   "/" + HttpUtility.HtmlAttributeEncode(WebUtility.UrlEncode(contextExtent)) +
                                   "/" + HttpUtility.HtmlAttributeEncode(WebUtility.UrlEncode(asHasId.Id)),
                            Content = NamedElementMethods.GetName(value)
                        });
            }
            
            return new HtmlDivElement(NamedElementMethods.GetName(value));
        }

        private static HtmlElement CreateHtmlForObjectShadow(MofObjectShadow mofObjectShadow, StringBuilder scriptLines)
        {
            var id = StringManipulation.RandomString(16);
            var result = new HtmlDivElement("Loading")
            {
                Id = id
            };

            scriptLines.Append(
                "DatenMeister.DomHelper.injectNameByUri($('#" + id + "')," +
                "encodeURIComponent('" + HttpUtility.JavaScriptStringEncode(mofObjectShadow.Uri) + "')" +
                ");\r\n");

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
            var fieldMetaClass = field.getMetaClass();
            if (fieldMetaClass?.equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
            {
                return (element as MofElement)?.getMetaClass(false);
            }

            var name = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            if (fieldMetaClass?.@equals(_DatenMeister.TheOne.Forms.__EvalTextFieldData) == true)
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

        /// <summary>
        /// Consolidates all fields from tabs and and the extentform itself
        /// </summary>
        /// <param name="fields">Field list that shall be filled with the consoldiated fields</param>
        /// <param name="form">Form to be parsed through and to consolidate the fields</param>
        public static void ConsolidateFields(List<IElement> fields, IObject form)
        {
            // Consolidate fields from tab and fields into the list of fields
            foreach (var field in
                form.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field).OfType<IElement>())
            {
                fields.Add(field);
            }

            foreach (var tab in
                form.get<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).OfType<MofElement>())
            {
                if (tab.getMetaClassWithoutTracing()?.@equals(_DatenMeister.TheOne.Forms.__ListForm) == true)
                {
                    fields.Add(
                        InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Forms.__TextFieldData)
                            .SetProperty(_DatenMeister._Forms._TextFieldData.name, tab.getOrDefault<string>(_DatenMeister._Forms._ListForm.property))
                            .SetProperty(_DatenMeister._Forms._TextFieldData.title, tab.getOrDefault<string>(_DatenMeister._Forms._ListForm.title))
                            .SetProperty(_DatenMeister._Forms._TextFieldData.isEnumeration, true)
                    );
                }
                else
                {
                    foreach (var field in
                        tab.get<IReflectiveCollection>(_DatenMeister._Forms._DetailForm.field).OfType<IElement>())
                    {
                        fields.Add(field);
                    }
                }
            }
        }
    }
}