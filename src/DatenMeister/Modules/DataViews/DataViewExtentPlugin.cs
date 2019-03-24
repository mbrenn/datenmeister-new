using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Functions.Queries;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewExtentPlugin : IEnumerable<IExtent>
    {
        /// <summary>
        /// Stores the workspaces
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        /// <summary>
        /// Initializes a new instance of the workspace logic
        /// </summary>
        /// <param name="workspaceLogic">Workspace Logic to be added</param>
        public DataViewExtentPlugin(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        public IEnumerator<IExtent> GetEnumerator()
        {
            var metaClass = (IElement) _workspaceLogic.GetTypesWorkspace().FindElementByUri("datenmeister:///_internal/types/internal?DatenMeister::DataViews::DataView");
            var managementWorkspace = _workspaceLogic.GetManagementWorkspace();
            foreach (var extent in managementWorkspace.extent)
            {
                extent.elements().GetAllDescendants().WhenMetaClassIs(metaClass);
                yield return null;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}