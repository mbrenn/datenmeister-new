using System;
using System.Collections.Generic;
using System.Linq;
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

        public ItemsOverviewModel(ILogger<ItemsOverviewModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        [Parameter] public string Workspace { get; set; } = string.Empty;

        [Parameter] public string Extent { get; set; } = string.Empty;

        [Parameter] public string Item { get; set; } = string.Empty;

        [Inject] public ExtentController? ExtentController { get; set; }

        public IReflectiveCollection? Items { get; set; }

        public IObject? Form { get; set; }

        public List<IElement> Fields = new();

        protected void OnInitialized()
        {
            if (ExtentController == null) throw new InvalidOperationException("ExtentController is not set");

            var result = ExtentController.GetItems(Workspace, Extent, Item);
            if (result == null)
            {
                throw new InvalidOperationException($"Items not found: {Workspace}/{Extent}");
            }

            Items = XmiHelper.ConvertCollectionFromXmi(result.items)
                    ?? throw new InvalidOperationException("Items are null. They may not be null");
            Form = XmiHelper.ConvertItemFromXmi(result.form)
                   ?? throw new InvalidOperationException("Form is null. It may not be null");

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