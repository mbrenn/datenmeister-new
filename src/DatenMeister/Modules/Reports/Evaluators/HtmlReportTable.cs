using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.DataViews;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Evaluators
{
    public class HtmlReportTable : IHtmlReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_Reports.TheOne.__ReportTable) == true;
        }

        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNode)
        {
            var viewNode = reportNode.getOrDefault<IElement>(_Reports._ReportTable.viewNode);
            var form = reportNode.getOrDefault<IElement>(_Reports._ReportTable.form);
            
            // Gets the elements for the table
            var dataviewEvaluation =
                new DataViewEvaluation(htmlReportCreator.WorkspaceLogic, htmlReportCreator.ScopeStorage);
            foreach (var source in htmlReportCreator.Sources)
            {
                dataviewEvaluation.AddDynamicSource(source.Key, source.Value);
            }

            var elements = dataviewEvaluation.GetElementsForViewNode(viewNode);

            // Find form 
            if (form == null)
            {
                // Create form
                var formCreator = new FormCreator(
                    htmlReportCreator.WorkspaceLogic, 
                    null);
                form = formCreator.CreateListFormForElements(elements, CreationMode.All);
            }
            
            // Creates the table
            var table = new HtmlTable();
            var cells = new List<HtmlTableCell>();
            var fields = form.getOrDefault<IReflectiveCollection>(_FormAndFields._ListForm.field);
            foreach (var field in fields.OfType<IElement>())
            {
                cells.Add(
                    new HtmlTableCell(field.getOrDefault<string>(_FormAndFields._FieldData.title))
                    {
                        IsHeading = true
                    });
            }

            table.AddRow(new HtmlTableRow(cells));
            
            foreach (var listElement in elements.OfType<IElement>())
            {
                cells.Clear();
                foreach (var field in fields.OfType<IElement>())
                {
                    var cell = CreateCellForField(listElement, field);
                    cells.Add(cell);
                }

                table.AddRow(new HtmlTableRow(cells));   
            }
            
            htmlReportCreator.HtmlReporter.Add(table);
        }

        /// <summary>
        /// Creates the cell for a specific element and field
        /// </summary>
        /// <param name="listElement">Element to be shown</param>
        /// <param name="field">Field definition for the element</param>
        /// <returns>The created Html Table Cell</returns>
        private HtmlTableCell CreateCellForField(IObject listElement, IElement field)
        {
            var property = field.getOrDefault<string>(_FormAndFields._FieldData.name);
            var metaClass = field.getMetaClass();
            var isPropertySet = listElement.isSet(property);
            if (metaClass?.@equals((_FormAndFields.TheOne.__DateTimeFieldData)) == true)
            {
                if (isPropertySet)
                {
                    var hasDate = field?.getOrDefault<bool>(_FormAndFields._DateTimeFieldData.hideDate) != true;
                    var hasTime = field?.getOrDefault<bool>(_FormAndFields._DateTimeFieldData.hideTime) != true;
                    var date = listElement.getOrDefault<DateTime>(property);

                    var result = string.Empty;
                    if (hasDate && hasTime)
                    {
                        result = date.ToString(CultureInfo.CurrentCulture);
                    }
                    else if (hasDate)
                    {
                        result = date.ToShortDateString();
                    }
                    else if (hasTime)
                    {
                        result = date.ToShortTimeString();
                    }

                    return new HtmlTableCell(result);
                }

                return new HtmlTableCell("-");
            }

            if (metaClass?.equals(_FormAndFields.TheOne.__CheckboxFieldData) == true)
            {
                if (isPropertySet)
                {
                    var value = listElement.getOrDefault<bool>(property);
                    return new HtmlTableCell(value ? "X" : "-");
                }

                return new HtmlTableCell("-");
            }

            if (metaClass?.@equals(_FormAndFields.TheOne.__NumberFieldData) == true)
            {
                var format = field.getOrDefault<string>(_FormAndFields._NumberFieldData.format) ?? "";
                var isInteger = field.getOrDefault<bool>(_FormAndFields._NumberFieldData.isInteger);
                
                if (isPropertySet)
                {
                    if (isInteger)
                    {
                        var value = listElement.getOrDefault<int>(property);
                        return new HtmlTableCell(value.ToString(format, CultureInfo.CurrentCulture));
                    }
                    else
                    {
                        var value = listElement.getOrDefault<double>(property);
                        return new HtmlTableCell(value.ToString(format, CultureInfo.CurrentCulture));
                        
                    }
                }

                return new HtmlTableCell("0");
            }

            if (isPropertySet)
            {
                return new HtmlTableCell(listElement.getOrDefault<string>(property));
            }

            return new HtmlTableCell(new HtmlRawString("<i>Null</i>"));

        }
    }
}