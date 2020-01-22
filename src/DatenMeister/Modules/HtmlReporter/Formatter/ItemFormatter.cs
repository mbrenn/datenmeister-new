using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.HtmlReporter.Formatter
{
    public class ItemFormatter
    {
        private readonly HtmlReport _htmlEngine;

        /// <summary>
        /// Defines the logger being used for this class
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(ItemFormatter));

        /// <summary>
        /// Initializes a new instance of the ItemFormatter class
        /// </summary>
        /// <param name="htmlEngine">Html engine to be used</param>
        public ItemFormatter(HtmlReport htmlEngine)
        {
            _htmlEngine = htmlEngine;
        }

        /// <summary>
        /// Creates a set of tables for each tabulator within the given defined form
        /// </summary>
        /// <param name="collection">Collection of items to be parsed</param>
        /// <param name="extentForm">The extent form being used to create the tables</param>
        public void FormatCollectionOfItems(IEnumerable<object?> collection, IObject extentForm)
        {
            collection = collection.ToList();
            var tabs = extentForm.getOrDefault<IReflectiveCollection>(_FormAndFields._ExtentForm.tab);
            if (tabs == null)
            {
                FormatCollectionByTab(collection, extentForm);
                return;
            }

            foreach (var tab in tabs.OfType<IObject>())
            {
                FormatCollectionByTab(collection, tab);
            }
        }

        private void FormatCollectionByTab(IEnumerable<object?> collection, IObject tab)
        {
            var table = new HtmlTable();
            var fields = tab.getOrDefault<IReflectiveCollection>(_FormAndFields._DetailForm.field);

            if (fields == null)
            {
                Logger.Warn("The given tabulator does not contain fields");
                return;
            }

            // Adds the fields that will be added to the table as the headline
            var listFields = new List<HtmlElement>();
            foreach (var field in fields.OfType<IObject>())
            {
                var fieldName = field.getOrDefault<string>(_FormAndFields._FieldData.name);
                if (fieldName == null)
                {
                    continue;
                }

                listFields.Add(
                    new HtmlTableCell(field.getOrDefault<string>(_FormAndFields._FieldData.title ?? "unset"))
                        {IsHeading = true});
            }

            table.AddRow(listFields.ToArray());

            // Now go through the items and show them
            foreach (var item in collection.OfType<IObject>())
            {
                listFields.Clear();
                foreach (var field in fields.OfType<IElement>())
                {
                    var fieldName = field.getOrDefault<string>(_FormAndFields._FieldData.name);
                    if (fieldName == null)
                    {
                        continue;
                    }

                    var value = (HtmlElement) item.getOrDefault<string>(fieldName)
                                ?? new HtmlRawString("<i>unset</i>");

                    listFields.Add(new HtmlTableCell(value));
                }

                table.AddRow(listFields.ToArray());
            }

            _htmlEngine.Add(table);
        }

        /// <summary>
        /// Formats a single item
        /// </summary>
        /// <param name="item">Item to be formatted</param>
        /// <param name="detailForm">The detailform being used for formatting</param>
        public void FormatItem(IObject item, IObject detailForm)
        {
            var table = new HtmlTable();
            table.AddRow(
                new HtmlTableCell("Field") {IsHeading = true},
                new HtmlTableCell("Value") {IsHeading = true});

            CreateRowForFields(item, detailForm, table);

            var tabs = detailForm.getOrDefault<IReflectiveCollection>(_FormAndFields._DetailForm.tab);
            if (tabs != null)
            {
                foreach (var tab in tabs.OfType<IElement>())
                {
                    CreateRowForFields(item, tab, table);
                }
            }

            _htmlEngine.Add(table);
        }

        /// <summary>
        /// Creates the rows for the given fields, which are stored in the property 'field' of the
        /// 'form' parameter
        /// </summary>
        /// <param name="item">Item to be shown</param>
        /// <param name="form">Form containing the field</param>
        /// <param name="table">The Html Table in which the fields will be include</param>
        private static void CreateRowForFields(IObject item, IObject form, HtmlTable table)
        {
            var fields = form.getOrDefault<IReflectiveCollection>(_FormAndFields._ListForm.field);
            if (fields == null)
            {
                throw new InvalidOperationException("Fields are null...");
            }

            foreach (var field in fields.OfType<IElement>())
            {
                var fieldName = field.getOrDefault<string>(_FormAndFields._FieldData.name);
                var title = field.getOrDefault<string>(_FormAndFields._FieldData.title);

                // Skip titles with null value
                if (fieldName == null) continue;

                table.AddRow(
                    new HtmlTableCell(title),
                    new HtmlTableCell(
                        (HtmlElement) item.getOrDefault<string>(fieldName) ??
                        new HtmlRawString("<i>unset</i>")));
            }
        }
    }
}