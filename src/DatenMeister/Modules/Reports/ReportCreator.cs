using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Integration;
using DatenMeister.Models.Reports;
using DatenMeister.Modules.DataViews;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Reports
{
    public class ReportCreator
    {
        private readonly ClassLogger Logger = new ClassLogger(typeof(ReportCreator));

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        private readonly Dictionary<string, IReflectiveCollection> _sources 
            = new Dictionary<string, IReflectiveCollection>();
        
        public IWorkspaceLogic WorkspaceLogic { get; }

        public IScopeStorage ScopeStorage { get; }

        /// <summary>
        /// Stores the possible source of the report
        /// </summary>
        public Dictionary<string, IReflectiveCollection> Sources => _sources;

        public ReportCreator(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
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
            var sources = reportInstance.getOrDefault<IReflectiveCollection>(_Reports._HtmlReportInstance.sources);
            foreach (var source in sources.OfType<IObject>())
            {
                var name = source.getOrDefault<string>(_Reports._ReportInstanceSource.name);
                if (string.IsNullOrEmpty(name))
                {
                    throw new InvalidOperationException("name of ReportInstanceSource is not set");
                }
                
                var workspaceId = source.getOrDefault<string>(_Reports._ReportInstanceSource.workspaceId);
                if (string.IsNullOrEmpty(workspaceId))
                {
                    workspaceId = WorkspaceNames.WorkspaceData;
                }
                
                var sourceRef = source.getOrDefault<string>(_Reports._ReportInstanceSource.source);

                IReflectiveCollection? sourceItems = null;
                var workspace = WorkspaceLogic.GetWorkspace(workspaceId) ?? WorkspaceLogic.GetDataWorkspace();
                
                var foundSource = workspace.Resolve(sourceRef, ResolveType.Default);
                if (foundSource is IExtent extent)
                {
                    sourceItems = extent.elements();
                }
                else if (foundSource is IReflectiveCollection asReflectiveCollection)
                {
                    sourceItems = asReflectiveCollection;
                }
                else if (foundSource is IObject asObject)
                {
                    sourceItems = new TemporaryReflectiveCollection(new[] {asObject});
                }
                
                if (sourceItems == null)
                {
                    throw new InvalidOperationException($"Source with uri {sourceRef} was not found");
                }
                
                yield return new ReportSource(name, sourceItems);
            }
        }
    }
}