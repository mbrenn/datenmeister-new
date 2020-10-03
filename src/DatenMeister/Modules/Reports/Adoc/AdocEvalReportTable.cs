using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.TextTemplates;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.Reports.Adoc
{
    public class AdocEvalReportTable : IAdocReportEvaluator
    {
        public bool IsRelevant(IElement element)
        {
            var metaClass = element.getMetaClass();
            return metaClass?.@equals(_Reports.TheOne.__ReportTable) == true;
        }

        public void Evaluate(AdocReportCreator adocReportCreator, IElement reportNode, TextWriter writer)
        {
            var viewNode = reportNode.getOrDefault<IElement>(_Reports._ReportTable.viewNode);
            if (viewNode == null)
            {
                throw new InvalidOperationException("The viewNode is null");
            }
            
            var form = reportNode.getOrDefault<IElement>(_Reports._ReportTable.form);

            var dataviewEvaluation = adocReportCreator.GetDataViewEvaluation();
            var elements = dataviewEvaluation.GetElementsForViewNode(viewNode);

            // Find form 
            if (form == null)
            {
                // Create form
                var formCreator = FormCreator.Create(
                    adocReportCreator.WorkspaceLogic, 
                    null);
                form = formCreator.CreateListFormForElements(elements, CreationMode.All);
            }
            
            // Creates the table
            var table = new StringBuilder();
            table.AppendLine("[%header]");
            table.AppendLine("|===");
            
            var fields = form.getOrDefault<IReflectiveCollection>(_FormAndFields._ListForm.field);
            foreach (var field in fields.OfType<IElement>())
            {
                table.Append("|" + field.getOrDefault<string>(_FormAndFields._FieldData.title));
            }

            table.AppendLine(string.Empty);

            foreach (var listElement in elements.OfType<IElement>())
            {
                foreach (var field in fields.OfType<IElement>())
                {
                    table.Append("|" + CreateCellForField(listElement, field));
                }

                table.AppendLine(string.Empty);

            }

            table.AppendLine("|===");
            writer.Write(table.ToString());
        }
        /// <summary>
        /// Creates the cell for a specific element and field
        /// </summary>
        /// <param name="listElement">Element to be shown</param>
        /// <param name="field">Field definition for the element</param>
        /// <returns>The created Html Table Cell</returns>
        private string CreateCellForField(IObject listElement, IElement field)
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

                    return result;
                }

                return "-";
            }

            if (metaClass?.equals(_FormAndFields.TheOne.__CheckboxFieldData) == true)
            {
                if (isPropertySet)
                {
                    var value = listElement.getOrDefault<bool>(property);
                    return value ? "X" : "-";
                }

                return "-";
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
                        return value.ToString(format, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        var value = listElement.getOrDefault<double>(property);
                        return value.ToString(format, CultureInfo.CurrentCulture);
                        
                    }
                }

                return "0";
            }

            if (metaClass?.@equals(_FormAndFields.TheOne.__EvalTextFieldData) == true)
            {
                var cellInformation = InMemoryObject.CreateEmpty();
                var defaultText = listElement.getOrDefault<string>(property);
                cellInformation.set("text", defaultText);

                var evalProperties = field.getOrDefault<string>(_FormAndFields._EvalTextFieldData.evalCellProperties);
                if (evalProperties != null)
                {
                    TextTemplateEngine.Parse(
                        "{{" + evalProperties + "}}",
                        new Dictionary<string, object>
                        {
                            ["i"] = listElement,
                            ["c"] = cellInformation
                        });
                }

                return cellInformation.isSet("text") 
                    ? cellInformation.getOrDefault<string>("text") 
                    : defaultText;
            }

            if (isPropertySet)
            {
                return listElement.getOrDefault<string>(property);
            }

            return "Null";
        }
    }
}