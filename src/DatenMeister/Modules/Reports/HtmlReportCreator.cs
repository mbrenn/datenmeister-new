using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Reports;
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
        private readonly ClassLogger Logger = new ClassLogger(typeof(HtmlReportCreator));
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly IScopeStorage _scopeStorage;

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        private readonly Dictionary<string, IReflectiveCollection> _sources 
            = new Dictionary<string, IReflectiveCollection>();
        
        private  HtmlReport? _htmlReporter;
        
        public HtmlReportCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public HtmlReport HtmlReporter => 
            _htmlReporter ?? throw new InvalidOperationException("_htmlReporter is null");

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        public Dictionary<string, IReflectiveCollection> Sources => _sources;

        public IWorkspaceLogic WorkspaceLogic => _workspaceLogic;

        public IScopeStorage ScopeStorage => _scopeStorage;

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

            var evaluators = _scopeStorage.Get<HtmlReportEvaluators>();

            var elements = reportDefinition.getOrDefault<IReflectiveCollection>(_Reports._ReportDefinition.elements);
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