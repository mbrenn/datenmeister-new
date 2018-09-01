using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.ManagementProviders
{
    public class ManagementViewDefinitions
    {
        private ViewLogic _viewLogic;

        public ManagementViewDefinitions(ViewLogic viewLogic)
        {
            _viewLogic = viewLogic;
        }

        /// <summary>
        /// Stores the name of the package
        /// </summary>
        public const string PackageName = "WorkspacesAndExtents";

        public const string NewWorkspaceForm = "Detail - New Workspace";

        public const string NewXmiDetailForm = "Detail - New Xmi Extent";

        public const string WorkspaceListView = "List - Workspaces";

        public const string ExtentListView = "List - Extents";

        public const string FindTypeForm = "Detail - Find Type";

        public const string PathNewXmiDetailForm = PackageName + "::" + NewXmiDetailForm;

        public const string PathFindTypeForm = PackageName + "::" + FindTypeForm;

        public const string PathWorkspaceListView = PackageName + "::" + WorkspaceListView;

        public const string PathExtentListView = PackageName + "::" + ExtentListView;

        public const string PathNewWorkspaceForm = PackageName + "::" + NewWorkspaceForm;

        /// <summary>
        /// Creates a form to create a new type. 
        /// </summary>
        /// <param name="preSelectedPackage">Package that shall be preselected</param>
        /// <param name="types">Types to be added to the form</param>
        /// <returns>The created type</returns>
        public IElement GetFindTypeForm(IObject preSelectedPackage, IWorkspace workspace = null, IExtent extent = null)
        {
            var form = new Form(FindTypeForm)
            {
                inhibitNewItems = true,
                fixView = true,
                hideMetaClass = true,
                minimizeDesign = true,
                defaultHeight = 600,
                defaultWidth = 700
            };

            var type2Field = new ReferenceFieldData("selectedType", "Type")
            {
                isSelectionInline = true,
                defaultValue = preSelectedPackage,
                defaultWorkspace = workspace?.id,
                defaultExtentUri = (extent as IUriExtent)?.contextURI()
            };

            form.AddFields(type2Field);

            var createdForm = DotNetSetter.Convert(_viewLogic.GetInternalViewExtent(), form) as IElement;
            return createdForm;
        }
    }
}