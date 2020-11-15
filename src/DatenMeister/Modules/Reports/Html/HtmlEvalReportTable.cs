using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Html
{
    public class HtmlReportTable : IHtmlReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_DatenMeister.TheOne.Reports.__ReportTable) == true;
        }

        /// <summary>
        /// Evaluates the table
        /// </summary>
        /// <param name="htmlReportCreator">The report creator</param>
        /// <param name="reportNode">The element describing the report.
        /// The form should be of type ListForm</param>
        public void Evaluate(HtmlReportCreator htmlReportCreator, IElement reportNode)
        {
            var viewNode = reportNode.getOrDefault<IElement>(_DatenMeister._Reports._ReportTable.viewNode);
            if (viewNode == null)
            {
                throw new InvalidOperationException("The viewNode is null");
            }
            
            var form = reportNode.getOrDefault<IElement>(_DatenMeister._Reports._ReportTable.form);

            var dataviewEvaluation = htmlReportCreator.GetDataViewEvaluation();
            var elements = dataviewEvaluation.GetElementsForViewNode(viewNode);

            // Find form 
            if (form == null)
            {
                // Create form
                var formCreator = FormCreator.Create(
                    htmlReportCreator.WorkspaceLogic, 
                    null);
                form = formCreator.CreateListFormForElements(elements, CreationMode.All);
            }
            
            // Creates the table
            var table = new HtmlTable();
            var cssClass = reportNode.getOrDefault<string>(_DatenMeister._Reports._ReportTable.cssClass);
            if (!string.IsNullOrEmpty(cssClass) && cssClass != null)
            {
                table.CssClass = cssClass;
            }
            
            var cells = new List<HtmlTableCell>();
            var fields = form.getOrDefault<IReflectiveCollection>(_DatenMeister._Forms._ListForm.field);
            foreach (var field in fields.OfType<IElement>())
            {
                cells.Add(
                    new HtmlTableCell(field.getOrDefault<string>(_DatenMeister._Forms._FieldData.title))
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
            var property = field.getOrDefault<string>(_DatenMeister._Forms._FieldData.name);
            var metaClass = field.getMetaClass();
            var isPropertySet = listElement.isSet(property);
            if (metaClass?.@equals(_DatenMeister.TheOne.Forms.__DateTimeFieldData) == true)
            {
                if (isPropertySet)
                {
                    var hasDate = field?.getOrDefault<bool>(_DatenMeister._Forms._DateTimeFieldData.hideDate) != true;
                    var hasTime = field?.getOrDefault<bool>(_DatenMeister._Forms._DateTimeFieldData.hideTime) != true;
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

            if (metaClass?.equals(_DatenMeister.TheOne.Forms.__CheckboxFieldData) == true)
            {
                if (isPropertySet)
                {
                    var value = listElement.getOrDefault<bool>(property);
                    return new HtmlTableCell(value ? "X" : "-");
                }

                return new HtmlTableCell("-");
            }

            if (metaClass?.@equals(_DatenMeister.TheOne.Forms.__NumberFieldData) == true)
            {
                var format = field.getOrDefault<string>(_DatenMeister._Forms._NumberFieldData.format) ?? "";
                var isInteger = field.getOrDefault<bool>(_DatenMeister._Forms._NumberFieldData.isInteger);
                
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

            if (metaClass?.@equals(_DatenMeister.TheOne.Forms.__EvalTextFieldData) == true)
            {
                var cellInformation = InMemoryObject.CreateEmpty();
                var defaultText = listElement.getOrDefault<string>(property);
                cellInformation.set("text", defaultText);

                var evalProperties = field.getOrDefault<string>(_DatenMeister._Forms._EvalTextFieldData.evalCellProperties);
                if (evalProperties != null)
                {
                    defaultText = TextTemplateEngine.Parse(
                        evalProperties,
                        new Dictionary<string, object>
                        {
                            ["i"] = listElement,
                            ["c"] = cellInformation
                        });
                }

                var cssClassName = cellInformation.getOrDefault<string>("cssClass") ?? string.Empty;

                var text= cellInformation.isSet("text") 
                    ? cellInformation.getOrDefault<string>("text") 
                    : defaultText;
                
                return new HtmlTableCell(text, cssClassName);
            }

            if (isPropertySet)
            {
                return new HtmlTableCell(listElement.getOrDefault<string>(property));
            }

            return new HtmlTableCell(new HtmlRawString("<i>Null</i>"));
        }
    }
}