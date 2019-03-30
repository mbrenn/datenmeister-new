using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.DataViews;
using DatenMeister.Provider.ManagementProviders;
using DatenMeister.Runtime;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Proxies;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewLogic
    {
        /// <summary>
        /// Stores the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(DataViewLogic));

        /// <summary>
        /// Defines the path to the packages of the fast view filters
        /// </summary>
        public const string PackagePathTypesDataView = "DatenMeister::DataViews";

        private readonly IWorkspaceLogic _workspaceLogic;

        public DataViewLogic(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        public IEnumerable<IElement> GetDataViewElements()
        {
            var metaClass = (IElement)_workspaceLogic.GetTypesWorkspace().FindElementByUri("datenmeister:///_internal/types/internal?DatenMeister::DataViews::DataView");
            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();
            foreach (var extent in managementWorkspace.extent.OfType<IUriExtent>())
            {
                if (extent.contextURI() == ExtentOfWorkspaces.WorkspaceUri)
                {
                    continue;
                }

                foreach (var dataView in extent.elements().GetAllDescendants().WhenMetaClassIs(metaClass).Cast<IElement>())
                {
                    yield return dataView;
                }
            }
        }

        /// <summary>
        /// Parses the given view node and return the values of the viewnode as a reflective sequence
        /// </summary>
        /// <param name="viewNode">View Node to be parsed</param>
        /// <returns>The reflective Sequence</returns>
        public IReflectiveSequence GetElementsForViewNode(IElement viewNode)
        {
            var dataview = _workspaceLogic.GetTypesWorkspace().Create<FillTheDataViews,_DataViews>();
            if (viewNode.getMetaClass()?.@equals(dataview.SourceExtentNode) == true)
            {
                var workspaceName = viewNode.getOrDefault<string>(_DataViews._SourceExtentNode.workspace);
                if (string.IsNullOrEmpty(workspaceName))
                {
                    workspaceName = WorkspaceNames.NameData;
                }

                var extentUri = viewNode.getOrDefault<string>(_DataViews._SourceExtentNode.extentUri);
                var workspace = _workspaceLogic.GetWorkspace(workspaceName);
                if (workspace == null)
                {
                    Logger.Warn($"Workspace is not found: {workspaceName}");
                    return new PureReflectiveSequence();
                }

                var extent = workspace.FindExtent(extentUri);
                if (extent == null)
                {
                    Logger.Warn($"Extent is not found: {extentUri}");
                    return new PureReflectiveSequence();
                }

                return extent.elements();
            }

            Logger.Warn($"Unknown type of viewnode: {viewNode.getMetaClass()}");

            throw new System.NotImplementedException();
        }
    }
}