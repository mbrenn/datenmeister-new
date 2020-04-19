using System.Collections;
using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewExtentPlugin : IEnumerable<IExtent>
    {
        /// <summary>
        /// Stores the workspaces
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly DataViewLogic _dataViewLogic;

        /// <summary>
        /// Initializes a new instance of the workspace logic
        /// </summary>
        /// <param name="workspaceLogic">Workspace Logic to be added</param>
        /// <param name="dataViewLogic">The logic for the dataviews</param>
        public DataViewExtentPlugin(IWorkspaceLogic workspaceLogic, DataViewLogic dataViewLogic)
        {
            _workspaceLogic = workspaceLogic;
            _dataViewLogic = dataViewLogic;
        }

        public IEnumerator<IExtent> GetEnumerator()
        {
            foreach (var dataView in _dataViewLogic.GetDataViewElements())
            {
                yield return new DataViewExtent(dataView, _workspaceLogic, _dataViewLogic);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}