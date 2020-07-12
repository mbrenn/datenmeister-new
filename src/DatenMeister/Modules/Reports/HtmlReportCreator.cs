using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.DataViews;
using DatenMeister.Modules.Forms.FormCreator;
using DatenMeister.Modules.HtmlReporter.HtmlEngine;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Reports
{
    /// <summary>
    /// Creates the report
    /// </summary>
    public class HtmlReportCreator
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        private readonly Dictionary<string, IReflectiveCollection> _sources 
            = new Dictionary<string, IReflectiveCollection>();
        
        private  HtmlReport? _htmlReporter;
        
        public HtmlReportCreator(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }
        
        /// <summary>
        /// Adds the source to the report
        /// </summary>
        /// <param name="id">Id of the source</param>
        /// <param name="collection">Collection to be evaluated</param>
        public void AddSource(string id, IReflectiveCollection collection)
        {
            _sources[id] = collection;
        }

        public void GenerateReport(IElement reportDefinition, TextWriter report)
        {
            _htmlReporter = new HtmlReport(report);
            
            var title = reportDefinition.getOrDefault<string>(_Reports._ReportDefinition.title);
            _htmlReporter.SetDefaultCssStyle();
            _htmlReporter.StartReport(title);

            var elements = reportDefinition.getOrDefault<IReflectiveCollection>(_Reports._ReportDefinition.elements);
            foreach (var element in elements.OfType<IElement>())
            {
                var metaClass = element.getMetaClass();
                if (metaClass?.@equals(_Reports.TheOne.__ReportParagraph) == true)
                {
                    AddParagraph(element);
                }

                if (metaClass?.@equals(_Reports.TheOne.__ReportHeadline) == true)
                {
                    AddHeadline(element);
                }

                if (metaClass?.@equals(_Reports.TheOne.__ReportTable) == true)
                {
                    AddTable(element);
                }
            }
            
            _htmlReporter.EndReport();
        }

        private void AddHeadline(IObject element)
        {
            if (_htmlReporter == null) throw new InvalidOperationException(
                "First call GenerateReport to prepare HtmlReport");
            var title = element.getOrDefault<string>(_Reports._ReportHeadline.title);
            _htmlReporter.Add(new HtmlHeadline(title, 1));
        }

        private void AddParagraph(IObject element)
        {
            if (_htmlReporter == null) throw new InvalidOperationException(
                "First call GenerateReport to prepare HtmlReport");
            var paragraph = element.getOrDefault<string>(_Reports._ReportParagraph.paragraph);
            _htmlReporter.Add(new HtmlParagraph(paragraph));
        }

        private void AddTable(IObject element)
        {
            if (_htmlReporter == null) throw new InvalidOperationException(
                "First call GenerateReport to prepare HtmlReport");

            var viewNode = element.getOrDefault<IElement>(_Reports._ReportTable.viewNode);
            var form = element.getOrDefault<IElement>(_Reports._ReportTable.form);
            
            // Gets the elements for the table
            var dataviewEvaluation = new DataViewEvaluation(_workspaceLogic);
            foreach (var source in _sources)
            {
                dataviewEvaluation.AddDynamicSource(source.Key, source.Value);
            }

            var elements = dataviewEvaluation.GetElementsForViewNode(viewNode);

            // Find form 
            if (form == null)
            {
                // Create form
                var formCreator = new FormCreator(
                    _workspaceLogic, 
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
            
            _htmlReporter.Add(table);
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