using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
using DatenMeister.Modules.Reports.Generic;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Reports.Html
{
    /// <summary>
    /// Creates the report
    /// </summary>
    public class HtmlReportCreator : GenericReportCreator
    {
        private readonly ClassLogger Logger = new ClassLogger(typeof(HtmlReportCreator));

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        private readonly Dictionary<string, IReflectiveCollection> _sources 
            = new Dictionary<string, IReflectiveCollection>();

        private HtmlReport? _htmlReporter;

        public TextWriter TextWriter { get; set; }

        public HtmlReportCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage, TextWriter textWriter)
        : base(workspaceLogic, scopeStorage)
        {
            TextWriter = textWriter;
        }

        public HtmlReport HtmlReporter => 
            _htmlReporter ?? throw new InvalidOperationException("_htmlReporter is null");


        /// <summary>
        /// Generates a report after the sources has been manually attached
        /// It is important that the reportDefinition value is not of type ReportInstance.
        /// If a report shall be generated upon a Report Instance, use GenerateByInstance
        /// </summary>
        /// <param name="reportDefinition">The report definition to be used</param>
        /// <param name="writer">The writer being used</param>
        public override void GenerateReportByDefinition(IObject reportDefinition)
        {
            _htmlReporter = new HtmlReport(TextWriter);
            
            var title = reportDefinition.getOrDefault<string>(_DatenMeister._Reports._ReportDefinition.title);
            _htmlReporter.SetDefaultCssStyle();
            _htmlReporter.StartReport(title);

            var evaluators = ScopeStorage.Get<HtmlReportEvaluators>();

            var elements = reportDefinition.getOrDefault<IReflectiveCollection>(_DatenMeister._Reports._ReportDefinition.elements);
            foreach (var element in elements.OfType<IElement>())
            {
                var foundItem =
                    (from x in evaluators.Evaluators
                        where x.IsRelevant(element)
                        select x)
                    .FirstOrDefault();
                if (foundItem != null)
                {
                    foundItem.Evaluate(this, element);
                }
                else
                {
                    Logger.Warn("No evaluator found" + element);
                }
            }
            
            _htmlReporter.EndReport();
        }
    }
}