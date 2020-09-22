using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Reports;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.Reports
{
    /// <summary>
    /// Includes additional helper methods for the reports like evaluation of sources
    /// </summary>
    public class ReportLogic
    {
        private ClassLogger Logger = new ClassLogger(typeof(ReportLogic));
        
        private readonly IWorkspaceLogic _workspaceLogic;
        
        public const string PackagePathTypesReport = "DatenMeister::Reports";

        public ReportLogic(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
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
                var workspace = _workspaceLogic.GetWorkspace(workspaceId) ?? _workspaceLogic.GetDataWorkspace();
                
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

    /// <summary>
    /// Defines the report source, consisting of name and collection
    /// </summary>
    public class ReportSource
    {
        public ReportSource(string name, IReflectiveCollection collection)
        {
            Name = name;
            Collection = collection;
        }

        /// <summary>
        /// Gets or sets the name of the source
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets or sets the collection
        /// </summary>
        public IReflectiveCollection Collection { get; set; }
    }
}