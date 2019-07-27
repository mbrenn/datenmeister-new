using System.Linq;
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

        public ItemFormatter(HtmlReport htmlEngine)
        {
            _htmlEngine = htmlEngine;
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
            var fields = form.getOrDefault<IReflectiveCollection>(_FormAndFields._Form.field);
            foreach (var field in fields.OfType<IElement>())
            {
                var fieldName = field.getOrDefault<string>(_FormAndFields._FieldData.name);
                var title = field.getOrDefault<string>(_FormAndFields._FieldData.title);

                // Skip titles with null value
                if (fieldName == null) continue;
                
                table.AddRow(
                    new HtmlTableCell(title),
                    new HtmlTableCell(
                        (object) item.getOrDefault<string>(fieldName) ??
                        new HtmlRawString("<i>unset</i>")));
            }
        }
    }
}