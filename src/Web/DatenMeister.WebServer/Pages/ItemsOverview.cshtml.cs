using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.HtmlEngine;
using DatenMeister.WebServer.InterfaceController;
using DatenMeister.WebServer.Library.HtmlControls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DatenMeister.WebServer.Pages
{
    public class ExtentOverviewModel : PageModel
    {
        private readonly ILogger<ExtentOverviewModel> _logger;

        public ExtentOverviewModel(ILogger<ExtentOverviewModel> logger, ExtentItemsController extentItemsController)
        {
            _logger = logger;
            ExtentItemsController = extentItemsController;
        }
        
        /// <summary>
        /// Stores the enumeration of tables
        /// </summary>
        public readonly List<HtmlTable> Tables = new();


        public readonly StringBuilder ScriptLines = new();
        
        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string Extent { get; set; } = string.Empty;

        [Parameter] public string? Item { get; set; } = string.Empty;

        private ExtentItemsController ExtentItemsController { get; set; }

        public IReflectiveCollection? Items { get; set; }

        public IObject? Form { get; set; }

        public readonly List<MofElement> Fields = new();

        public void OnGet(string workspace, string extent, string? item)
        {
            Tables.Clear();
            
            Fields.Clear();
            Workspace = WebUtility.UrlDecode(workspace);
            Extent = WebUtility.UrlDecode(extent);
            Item = WebUtility.UrlDecode(item);

            if (ExtentItemsController == null) throw new InvalidOperationException("ExtentController is not set");

            var result = ExtentItemsController.GetItemsAndFormOfExtent(Workspace, Extent);
            if (result == null)
            {
                throw new InvalidOperationException($"Items not found: {Workspace}/{Extent}");
            }

            Items = XmiHelper.ConvertCollectionFromXmi(result.items)
                    ?? throw new InvalidOperationException("Items are null. They may not be null");
            Form = XmiHelper.ConvertItemFromXmi(result.form)
                   ?? throw new InvalidOperationException("Form is null. It may not be null");

            // Gets the field items
            ConsolidateFields(Fields, Form);
            var table = new HtmlTable
            {
                CssClass = "table table-striped table-bordered table-sm align-top dm-table-nofullwidth"
            };
            
            Tables.Add(table);
            
            // Creates the header row
            var headerRow = new HtmlTableRow();
            headerRow.Add(new HtmlTableCell("View") {IsHeading = true});
            foreach (var field in Fields)
            {
                headerRow.Add(
                    new HtmlTableCell(field.getOrDefault<string>(_DatenMeister._Forms._FieldData.title))
                        {IsHeading = true});
            }

            table.AddRow(headerRow);

            // Converts the items to a useful HtmlRaw Collection
            foreach (var rowItem in Items.OfType<IObject>())
            {
                var tableRow = new HtmlTableRow();
                var cell = new HtmlTableCell(
                    new HtmlRawString(
                        "<a href=\"/Item" +
                        $"/{WebUtility.UrlEncode(Workspace)}" +
                        $"/{WebUtility.UrlEncode(Extent)}" +
                        $"/{WebUtility.UrlEncode((rowItem as IHasId)?.Id) ?? string.Empty}\">View</a>"));
                tableRow.Add(cell);

                foreach (var field in Fields)
                {
                    var cellContent = ControlFactory.GetHtmlElementForItemsField(
                        rowItem, field, ScriptLines, Workspace, Extent);
                    tableRow.Add(new HtmlTableCell(cellContent));
                }

                table.AddRow(tableRow);
            }
        }

        /// <summary>
        /// Consolidates all fields from tabs and and the extentform itself
        /// </summary>
        /// <param name="fields">Field list that shall be filled with the consolidated fields</param>
        /// <param name="form">Form to be parsed through and to consolidate the fields</param>
        public static void ConsolidateFields(List<MofElement> fields, IObject form)
        {
            // Consolidate fields from tab and fields into the list of fields
            foreach (var field in
                form.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field).OfType<MofElement>())
            {
                fields.Add(field);
            }

            foreach (var tab in
                form.get<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).OfType<IObject>())
            {
                foreach (var field in
                    tab.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field).OfType<MofElement>())
                {
                    fields.Add(field);
                }
            }
        }
    }
}