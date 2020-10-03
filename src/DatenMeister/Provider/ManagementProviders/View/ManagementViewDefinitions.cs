using System;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.Forms;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Provider.ManagementProviders.View
{
    public class ManagementViewDefinitions
    {
        private FormsPlugin _formsPlugin;

        public ManagementViewDefinitions(FormsPlugin formsPlugin)
        {
            _formsPlugin = formsPlugin;
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
        /// <param name="parameter">Parameter to be evaluated</param>
        /// <param name="buttonName">Name of the button</param>
        /// <returns>The created type</returns>
        public IElement GetFindTypeForm(FindTypeFormParameter parameter, string? buttonName = null)
        {
            var form = new DetailForm(FindTypeForm)
            {
                hideMetaInformation = true,
                defaultHeight = 600,
                defaultWidth = 700,
                allowNewProperties = false
            };

            if (buttonName != null) form.buttonApplyText = buttonName;

            var type2Field = new ReferenceFieldData("selectedType", "Type")
            {
                isSelectionInline = true,
                defaultValue = parameter.PreSelectedPackage,
                defaultWorkspace = parameter.WorkspaceName,
                defaultExtentUri = parameter.ExtentUri
            };

            form.AddFields(type2Field);

            var createdForm = DotNetConverter.ConvertToMofObject(_formsPlugin.GetInternalFormExtent(), form) as IElement;
            return createdForm ?? throw new InvalidOperationException("Form could not be created");
        }
    }

    /// <summary>
    /// Defines the parameter for the find type form
    /// </summary>
    public class FindTypeFormParameter
    {
        /// <summary>
        /// Gets or sets the package that is pre-seleced
        /// </summary>
        public IObject? PreSelectedPackage { get; set; }
        
        /// <summary>
        /// Gets or sets the workspace
        /// </summary>
        public string? WorkspaceName { get; set; }
        
        /// <summary>
        /// Gets or sets the Uri of the extent that is preselected
        /// </summary>
        public string? ExtentUri { get; set; }

        /// <summary>
        /// Sets the workspace that is pre-selected
        /// </summary>
        public IWorkspace Workspace
        {
            set => WorkspaceName = value.id;
        }

        /// <summary>
        /// Gets the extent that is pre-selected
        /// </summary>
        public IUriExtent Extent
        {
            set => ExtentUri = value.contextURI();
        }
    }
}