using System;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms.FormFinder;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.ManagementProviders
{
    public class ManagementViewDefinitions
    {
        private FormLogic _formLogic;

        public ManagementViewDefinitions(FormLogic formLogic)
        {
            _formLogic = formLogic;
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

        public const string IdWorkspaceListView = "WorkspacesAndExtents.Workspaces.List";

        public const string IdNewXmiDetailForm = "#WorkspacesAndExtents.Xmi.New";

        /// <summary>
        /// Creates a form to create a new type.
        /// </summary>
        /// <param name="preSelectedPackage">Package that shall be preselected</param>
        /// <param name="workspace">The workspace to which the type shall be found</param>
        /// <param name="extent">The extent to which the type shall be found</param>
        /// <returns>The created type</returns>
        public IElement GetFindTypeForm(IObject preSelectedPackage, IWorkspace workspace = null, IExtent? extent = null)
        {
            var form = new DetailForm(FindTypeForm)
            {
                hideMetaInformation = true,
                defaultHeight = 600,
                defaultWidth = 700,
                allowNewProperties = false
            };

            var type2Field = new ReferenceFieldData("selectedType", "Type")
            {
                isSelectionInline = true,
                defaultValue = preSelectedPackage,
                defaultWorkspace = workspace?.id,
                defaultExtentUri = (extent as IUriExtent)?.contextURI()
            };

            form.AddFields(type2Field);

            var createdForm = DotNetConverter.ConvertToMofObject(_formLogic.GetInternalFormExtent(), form) as IElement;
            return createdForm ?? throw new InvalidOperationException("Form could not be created");
        }
    }
}