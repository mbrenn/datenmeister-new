using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.WebServer.Controller;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace DatenMeister.WebServer.Pages
{
    public class ItemsOverviewModel : PageModel
    {
        private readonly ILogger<ItemsOverviewModel> _logger;

        public class RowItem
        {
            public string Id { get; set; } = string.Empty;

            public Dictionary<string, CellItem> Cell { get; } = new();
        }

        public class CellItem
        {
            public string HtmlRaw { get; set; } = string.Empty;
        }

        public ItemsOverviewModel(ILogger<ItemsOverviewModel> logger, ExtentItemsController extentItemsController)
        {
            _logger = logger;
            ExtentItemsController = extentItemsController;
        }

        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string Extent { get; set; } = string.Empty;

        [Parameter] public string? Item { get; set; } = string.Empty;

        private ExtentItemsController? ExtentItemsController { get; set; }

        public IReflectiveCollection? Items { get; set; }

        private IObject? Form { get; set; }

        public readonly List<IElement> Fields = new();

        public readonly List<RowItem> HtmlCells = new();

        public void OnGet(string workspace, string extent, string? item)
        {
            HtmlCells.Clear();
            Fields.Clear();
            Workspace = WebUtility.UrlDecode(workspace);
            Extent = WebUtility.UrlDecode(extent);
            Item = WebUtility.UrlDecode(item);

            if (ExtentItemsController == null) throw new InvalidOperationException("ExtentController is not set");

            var result = ExtentItemsController.GetItemsAndForm(Workspace, Extent, Item);
            if (result == null)
            {
                throw new InvalidOperationException($"Items not found: {Workspace}/{Extent}");
            }

            Items = XmiHelper.ConvertCollectionFromXmi(result.items)
                    ?? throw new InvalidOperationException("Items are null. They may not be null");
            Form = XmiHelper.ConvertItemFromXmi(result.form)
                   ?? throw new InvalidOperationException("Form is null. It may not be null");

            // Gets the field items
            ConsolidateFields();

            // Converts the items to a useful HtmlRaw Collection
            foreach (var rowItem in Items.OfType<IObject>())
            {
                var row = new RowItem
                {
                    Id = (rowItem as IHasId)?.Id ?? string.Empty
                };
                
                foreach (var field in Fields)
                {
                    var content = new CellItem();
                    var name = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);

                    if (field.getMetaClass()?.@equals(_DatenMeister.TheOne.Forms.__MetaClassElementFieldData) == true)
                    {
                        content.HtmlRaw =
                            WebUtility.HtmlEncode((rowItem as IElement)?.metaclass?.ToString() ?? "Unknown");
                    }
                    else
                    {
                        content.HtmlRaw = WebUtility.HtmlEncode(rowItem.getOrDefault<string>(name));
                    }

                    row.Cell[name] = content;
                }

                HtmlCells.Add(row);
            }
        }

        /// <summary>
        /// Consolidates all fields from tabs and and the extentform itself
        /// </summary>
        private void ConsolidateFields()
        {
            // Consolidate fields from tab and fields into the list of fields
            foreach (var field in
                Form.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field).OfType<IElement>())
            {
                Fields.Add(field);
            }

            foreach (var tab in
                Form.get<IReflectiveCollection>(_DatenMeister._Forms._ExtentForm.tab).OfType<IElement>())
            {
                foreach (var field in
                    tab.get<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field).OfType<IElement>())
                {
                    Fields.Add(field);
                }
            }
        }
    }
}