﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.DataViews;
using DatenMeister.Modules.DefaultTypes;
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
                    null,
                    new DefaultClassifierHints(_workspaceLogic));
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
            
            table.AddRow( new HtmlTableRow(cells));
            
            foreach (var listElement in elements.OfType<IElement>())
            {
                cells.Clear();
                foreach (var field in fields.OfType<IElement>())
                {
                    var property = field.getOrDefault<string>(_FormAndFields._FieldData.name);
                    if (listElement.isSet(property))
                    {
                        cells.Add(new HtmlTableCell(listElement.getOrDefault<string>(property)));
                    }
                }

                table.AddRow(new HtmlTableRow(cells));   
            }
            
            _htmlReporter.Add(table);
        }
    }
}