using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;
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
        /// Defines the path to the packages of the dataviews
        /// </summary>
        public const string PackagePathTypesDataView = "DatenMeister::DataViews";

        private readonly IWorkspaceLogic _workspaceLogic;

        public DataViewLogic(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        public IEnumerable<IElement> GetDataViewElements()
        {
            var metaClass = (IElement?)
                (_workspaceLogic.GetTypesWorkspace()
                    .FindElementByUri("datenmeister:///_internal/types/internal?DatenMeister::DataViews::DataView"));
            if (metaClass == null)
            {
                Logger.Warn("DataView MetaClass was not found");
                yield break;
            }
                 
            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();
            foreach (var dataView in managementWorkspace.extent.OfType<IUriExtent>()
                .Where(extent => extent.contextURI() != WorkspaceNames.ExtentManagementExtentUri)
                .SelectMany(extent => extent.elements().GetAllDescendants().WhenMetaClassIs(metaClass).Cast<IElement>()))
            {
                yield return dataView;
            }
        }

        /// <summary>
        /// Parses the given view node and return the values of the viewnode as a reflective sequence
        /// </summary>
        /// <param name="viewNode">View Node to be parsed</param>
        /// <returns>The reflective Sequence</returns>
        public IReflectiveSequence GetElementsForViewNode(IElement viewNode)
        {
            var evaluation = new DataViewEvaluation(_workspaceLogic);
            return evaluation.GetElementsForViewNode(viewNode);
        }
    }
}