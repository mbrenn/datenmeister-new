using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.DataViews;
using DatenMeister.Modules.HtmlExporter.HtmlEngine;
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

        private HtmlReport? _htmlReporter;

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

        /// <summary>
        /// Generates a full html report by using the instance
        /// </summary>
        /// <param name="reportInstance">Report instance to be used</param>
        /// <param name="writer">The writer being used</param>
        public void GenerateReportByInstance(IElement reportInstance, TextWriter writer)
        {
            var reportLogic = new ReportLogic(_workspaceLogic);
            foreach (var scope in reportLogic.EvaluateSources(reportInstance))
            {
                AddSource(scope.Name, scope.Collection);
            }

            var definition = reportInstance.getOrDefault<IObject>(_Reports._HtmlReportInstance.reportDefinition);
            if (definition == null)
            {
                throw new InvalidOperationException("There is no report definition set.");
            }
            
            GenerateReportByDefinition(definition, writer);
        }

        /// <summary>
        /// Generates a report after the sources has been manually attached
        /// It is important that the reportDefinition value is not of type ReportInstance.
        /// If a report shall be generated upon a Report Instance, use GenerateByInstance
        /// </summary>
        /// <param name="reportDefinition">The report definition to be used</param>
        /// <param name="writer">The writer being used</param>
        public void GenerateReportByDefinition(IObject reportDefinition, TextWriter writer)
        {
            _htmlReporter = new HtmlReport(writer);
            
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

        /// <summary>
        /// Gets the dataview evaluation for the given context with associated sources
        /// </summary>
        /// <returns>The dataview evaluation</returns>
        public DataViewEvaluation GetDataViewEvaluation()
        {
            // Gets the elements for the table
            var dataviewEvaluation =
                new DataViewEvaluation(WorkspaceLogic, ScopeStorage);
            foreach (var source in Sources)
            {
                dataviewEvaluation.AddDynamicSource(source.Key, source.Value);
            }

            return dataviewEvaluation;
        }
    }
}