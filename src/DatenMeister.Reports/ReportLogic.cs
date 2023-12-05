using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Copier;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.DataView;
using DatenMeister.Reports.Generic;
using DatenMeister.TextTemplates;

namespace DatenMeister.Reports
{
    public class ReportLogic
    {
        private static readonly ClassLogger Logger = new(typeof(ReportLogic));

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        private Dictionary<string, IReflectiveCollection> _sources
            = new();

        public IWorkspaceLogic WorkspaceLogic { get; set; }

        public IScopeStorage ScopeStorage { get; set; }

        public GenericReportCreator ReportCreator { get; }

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        public Dictionary<string, IReflectiveCollection> Sources
        {
            get => _sources;
            set => _sources = value;
        }

        public ReportLogic(
            IWorkspaceLogic workspaceLogic, 
            IScopeStorage scopeStorage,
            GenericReportCreator reportCreator)
        {
            ReportCreator = reportCreator;
            WorkspaceLogic = workspaceLogic;
            ScopeStorage = scopeStorage;
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

        public Dictionary<string, IReflectiveCollection> PushSources()
        {
            var old = _sources;
            _sources = new Dictionary<string, IReflectiveCollection>(_sources);

            return old;

        }

        public void PopSources(Dictionary<string, IReflectiveCollection> sources)
        {
            _sources = sources;
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

        /// <summary>
        /// Reads the report instance and returns all report data sources with
        /// name and collection of elements. 
        /// </summary>
        /// <param name="reportInstance">The Report Instance describing the sources</param>
        /// <returns>Enumeration of report sources</returns>
        public IEnumerable<ReportSource> EvaluateSources(IObject reportInstance)
        {
            var sources =
                reportInstance.get<IReflectiveCollection>(_DatenMeister._Reports._HtmlReportInstance.sources);
            foreach (var source in sources.OfType<IObject>())
            {
                var name = source.getOrDefault<string>(_DatenMeister._Reports._ReportInstanceSource.name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new InvalidOperationException("name of ReportInstanceSource is not set");
                }

                var workspaceId = source.getOrDefault<string>(_DatenMeister._Reports._ReportInstanceSource.workspaceId);
                if (string.IsNullOrEmpty(workspaceId))
                {
                    workspaceId = WorkspaceNames.WorkspaceData;
                }

                var sourceRef = source.getOrDefault<string>(_DatenMeister._Reports._ReportInstanceSource.path);

                IReflectiveCollection? sourceItems = null;
                var workspace = WorkspaceLogic.GetWorkspace(workspaceId) ?? WorkspaceLogic.GetDataWorkspace();

                var foundSource = workspace.Resolve(sourceRef, ResolveType.Default);
                sourceItems = foundSource switch
                {
                    IExtent extent => extent.elements(),
                    IReflectiveCollection asReflectiveCollection => asReflectiveCollection,
                    IObject asObject => new TemporaryReflectiveCollection(new[] {asObject}),
                    _ => sourceItems
                };

                if (sourceItems == null)
                {
                    throw new InvalidOperationException($"Source with uri {sourceRef} was not found");
                }

                yield return new ReportSource(name, sourceItems);
            }
        }

        /// <summary>
        /// Gets the object view the dataevaluation
        /// </summary>
        /// <param name="reportNodeOrigin"></param>
        /// <param name="element"></param>
        /// <param name="viewNodePropertyName"></param>
        /// <returns></returns>
        public bool GetObjectViaDataEvaluation(
            IElement reportNodeOrigin,
            out IElement? element,
            string viewNodePropertyName)
        {
            var viewNode = GetViewNode(reportNodeOrigin, viewNodePropertyName);

            var dataViewEvaluation = GetDataViewEvaluation();
            element = dataViewEvaluation.GetElementsForViewNode(viewNode).OfType<IElement>().FirstOrDefault();
            if (element == null)
            {
                Logger.Info("No Element found");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the view node
        /// </summary>
        /// <param name="reportNodeOrigin">The element which contains view node</param>
        /// <param name="viewNodePropertyName">The property name upon which the view node will be used</param>
        /// <returns>Found viewnode</returns>
        public static IElement GetViewNode(IElement reportNodeOrigin, string viewNodePropertyName)
        {
            var viewNode = reportNodeOrigin.getOrDefault<IElement>(viewNodePropertyName);
            viewNode ??= InMemoryObject.CreateEmpty(
                    _DatenMeister.TheOne.DataViews.__DynamicSourceNode)
                .SetProperty(_DatenMeister._DataViews._DynamicSourceNode.nodeName, "item");
            return viewNode;
        }

        public IObject GetNodeWithEvaluatedProperties(IElement reportNodeOrigin, string propertyName)
        {
            var reportNode = ObjectCopier.CopyForTemporary(reportNodeOrigin);
            if (reportNode.isSet(_DatenMeister._Reports._Elements._ReportParagraph.evalProperties))
            {
                GetObjectViaDataEvaluation(reportNodeOrigin, out var element, propertyName);
                var evalProperties = reportNode.getOrDefault<string>(_DatenMeister._Reports._Elements._ReportParagraph.evalProperties);

                var dict = new Dictionary<string, object> { ["v"] = reportNode };
                if (element != null)
                {
                    dict["i"] = element;
                }

                TextTemplateEngine.Parse(
                    "{{" + evalProperties + "}}",
                    dict);
            }

            return reportNode;
        }

        /// <summary>
        /// Generates a full html report by using the instance
        /// </summary>
        /// <param name="reportInstance">Report instance to be used</param>
        public void GenerateReportByInstance(IElement reportInstance)
        {
            foreach (var scope in EvaluateSources(reportInstance))
            {
                AddSource(scope.Name, scope.Collection);
            }

            var definition = reportInstance.getOrDefault<IObject>(_DatenMeister._Reports._HtmlReportInstance.reportDefinition);
            if (definition == null)
            {
                throw new InvalidOperationException("There is no report definition set.");
            }

            GenerateReportByDefinition(definition);
        }

        public void GenerateReportByDefinition(IObject reportDefinition)
        {
            ReportCreator.StartReport(this, reportDefinition);

            var elements = reportDefinition.getOrDefault<IReflectiveCollection>(
                _DatenMeister._Reports._ReportDefinition.elements);
            ReportCreator.EvaluateElements(this, elements);

            ReportCreator.EndReport(this, reportDefinition);
        }
    }
}