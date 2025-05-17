using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.InMemory;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.WPF.Helper
{
    public class ManagementViewDefinitions
    {
        /// <summary>
        ///     Stores the name of the package
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
        ///     Creates a form to create a new type.
        /// </summary>
        /// <param name="parameter">Parameter to be evaluated</param>
        /// <param name="buttonName">Name of the button</param>
        /// <returns>The created type</returns>
        public IElement GetFindTypeForm(FindTypeFormParameter parameter, string? buttonName = null)
        {
            var detailForm = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Forms.__RowForm);
            detailForm.set(_DatenMeister._Forms._RowForm.hideMetaInformation, true);
            detailForm.set(_DatenMeister._Forms._RowForm.defaultHeight, 600);
            detailForm.set(_DatenMeister._Forms._RowForm.defaultWidth, 700);
            detailForm.set(_DatenMeister._Forms._RowForm.hideMetaInformation, false);

            if (buttonName != null) detailForm.set(_DatenMeister._Forms._RowForm.buttonApplyText, buttonName);

            var type2Field = InMemoryObject.CreateEmpty(_DatenMeister.TheOne.Forms.__ReferenceFieldData);
            type2Field.set(_DatenMeister._Forms._ReferenceFieldData.name, "selectedType");
            type2Field.set(_DatenMeister._Forms._ReferenceFieldData.isSelectionInline, true);
            type2Field.set(_DatenMeister._Forms._ReferenceFieldData.defaultValue, parameter.PreSelectedPackage);
            type2Field.set(_DatenMeister._Forms._ReferenceFieldData.defaultWorkspace, parameter.WorkspaceName);
            type2Field.set(_DatenMeister._Forms._ReferenceFieldData.defaultItemUri, parameter.ExtentUri);

            detailForm.set(_DatenMeister._Forms._RowForm.field, new[] { type2Field });

            return detailForm ?? throw new InvalidOperationException("Form could not be created");
        }
    }

    /// <summary>
    ///     Defines the parameter for the find type form
    /// </summary>
    public class FindTypeFormParameter
    {
        /// <summary>
        ///     Gets or sets the package that is pre-seleced
        /// </summary>
        public IObject? PreSelectedPackage { get; set; }

        /// <summary>
        ///     Gets or sets the workspace
        /// </summary>
        public string? WorkspaceName { get; set; }

        /// <summary>
        ///     Gets or sets the Uri of the extent that is preselected
        /// </summary>
        public string? ExtentUri { get; set; }

        /// <summary>
        ///     Sets the workspace that is pre-selected
        /// </summary>
        public IWorkspace Workspace
        {
            set => WorkspaceName = value.id;
        }

        /// <summary>
        ///     Gets the extent that is pre-selected
        /// </summary>
        public IUriExtent Extent
        {
            set => ExtentUri = value.contextURI();
        }
    }
}