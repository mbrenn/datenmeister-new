using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.Core.Modules.DataViews
{
    public class DataViewLogic
    {
        /// <summary>
        /// Defines the path to the packages of the dataviews
        /// </summary>
        public const string PackagePathTypesDataView = "DatenMeister::DataViews";

        /// <summary>
        ///     Stores the logger
        /// </summary>
        private static readonly ILogger Logger = new ClassLogger(typeof(DataViewLogic));

        private readonly IScopeStorage _scopeStorage;

        private readonly IWorkspaceLogic _workspaceLogic;

        public DataViewLogic(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
        {
            _workspaceLogic = workspaceLogic;
            _scopeStorage = scopeStorage;
        }

        public IEnumerable<IElement> GetDataViewElements()
        {
            var metaClass = _DatenMeister.TheOne.DataViews.__DataView;

            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();
            foreach (var dataView in managementWorkspace.extent.OfType<IUriExtent>()
                         .Where(extent => extent.contextURI() != WorkspaceNames.UriExtentWorkspaces)
                         .SelectMany(extent =>
                             extent.elements().GetAllDescendants().WhenMetaClassIs(metaClass).Cast<IElement>()))
            {
                yield return dataView;
            }
        }

        /// <summary>
        /// Parses the given view node and return the values of the viewnode as a reflective sequence
        /// </summary>
        /// <param name="viewNode">View Node to be parsed</param>
        /// <returns>The reflective Sequence</returns>
        public IReflectiveCollection GetElementsForViewNode(IElement viewNode)
        {
            var evaluation = new DataViewEvaluation(_workspaceLogic, _scopeStorage);
            return evaluation.GetElementsForViewNode(viewNode);
        }
    }
}