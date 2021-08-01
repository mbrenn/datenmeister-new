using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.HtmlEngine;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Library.HtmlControls;
using DatenMeister.WebServer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatenMeister.WebServer.Pages
{
    public class ItemModel : PageModel
    {
        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string Extent { get; set; } = string.Empty;

        [Parameter] public string? Item { get; set; } = string.Empty;

        /// <summary>
        /// Gets the item url of the currently selected item
        /// </summary>
        public string ItemUrl => $"{Extent}#{Item}";
        
        private ExtentItemsController ExtentItemsController { get; set; }

        public IObject? FoundItem { get; set; }

        public IObject? Form { get; set; }

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
                CssClass = "table table-striped table-bordered dm-table-nofullwidth align-top"
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
                
                var value = ControlFactory.GetHtmlElementForItemsField(FoundItem, field, ScriptLines, Workspace, Extent);

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
                if (tab.getMetaClassWithoutTracing()?.equals(_DatenMeister.TheOne.Forms.__ListForm) == true)
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