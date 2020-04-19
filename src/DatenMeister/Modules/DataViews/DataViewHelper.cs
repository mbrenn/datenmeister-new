using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.DataViews;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Modules.DataViews
{
    public class DataViewHelper
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        public DataViewHelper(IWorkspaceLogic workspaceLogic)
        {
            _workspaceLogic = workspaceLogic;
        }

        /// <summary>
        /// Gets the model
        /// </summary>
        /// <returns></returns>
        public _DataViews GetModel() =>
            _workspaceLogic.GetTypesWorkspace().Create<FillTheDataViews, _DataViews>();

        public IElement CreateDataview(string name, string extentUri)
        {
            var viewExtent = _workspaceLogic.GetUserFormsExtent();
            var metaClass = GetModel().__DataView;
            var createdElement = new MofFactory(viewExtent).create(metaClass);

            createdElement.set(_DataViews._DataView.name, name);
            createdElement.set(_DataViews._DataView.uri, extentUri);

            viewExtent.elements().add(createdElement);

            return createdElement;
        }

        /// <summary>
        /// Gets the extent for the user views which is usually used to define the views
        /// </summary>
        /// <returns>Extent containing the user views</returns>
        public IUriExtent GetUserFormExtent() =>
            _workspaceLogic.GetUserFormsExtent();

        public Workspace GetViewWorkspace() =>
            _workspaceLogic.GetViewsWorkspace();
    }
}