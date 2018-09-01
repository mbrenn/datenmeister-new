﻿using System;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder;
using DatenMeister.Provider.ManagementProviders.Model;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Provider.ManagementProviders
{
    /// <summary>
    /// Gets the workspace view
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ManagementViewDefinitions
    {
        /// <summary>
        /// Stores the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Stores the view logic
        /// </summary>
        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly PackageMethods _packageMethods;

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
        /// Initializes a new instance of the ManagementViewDefinitions class
        /// </summary>
        /// <param name="viewLogic">View logic being used to find View Extent</param>
        /// <param name="workspaceLogic">Logic of the workspace</param>
        public ManagementViewDefinitions(ViewLogic viewLogic, IWorkspaceLogic workspaceLogic, PackageMethods packageMethods)
        {
            _viewLogic = viewLogic;
            _workspaceLogic = workspaceLogic;
            _packageMethods = packageMethods;
        }

        /// <summary>
        /// Gets the detail form to create a new workspace
        /// </summary>
        /// <returns></returns>
        public IElement GetNewWorkspaceDetail()
        {
            var form = new Form(NewWorkspaceForm);
            form.AddFields(
                new TextFieldData("id", "Name"),
                new TextFieldData("annotation", "Annotation"));

            return DotNetSetter.Convert(_viewLogic.GetInternalViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Creates a form object
        /// </summary>
        /// <returns>The created form</returns>
        private IElement GetWorkspaceListForm()
        {
            // Finds the forms
            var form = new Form(WorkspaceListView)
            {
                inhibitNewItems = true
            };

            form.AddFields(
                new TextFieldData("id", "Name")
                {
                    isReadOnly = true
                },
                new TextFieldData("annotation", "Annotation"),
                new TextFieldData("extents", "Extents")
                {
                    isEnumeration = true,
                    isReadOnly = true
                });

            return DotNetSetter.Convert(_viewLogic.GetInternalViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Gets the view for the extent
        /// </summary>
        /// <returns>The created form</returns>
        private IElement GetExtentListForm()
        {
            // Finds the forms
            var form = new Form(ExtentListView)
            {
                inhibitNewItems = true
            };

            form.AddFields(
                new TextFieldData("uri", "URI"),
                new TextFieldData("count", "# of items")
                {
                    isReadOnly = true
                },
                new TextFieldData("type", "Provider-Type")
                {
                    isReadOnly = true
                },
                new TextFieldData("extentType", "Extent Type"),
                new SubElementFieldData(_ManagementProvider._Extent.alternativeUris, "Alternative URI"));

            return DotNetSetter.Convert(_viewLogic.GetInternalViewExtent(), form) as IElement;
        }

        /// <summary>
        /// Gets the form data for the user where he can create a new Xmi extent
        /// </summary>
        /// <returns>The created form element</returns>
        private IElement GetNewXmiExtentDetailForm()
        {
            // Finds the forms
            var form = new Form(NewXmiDetailForm)
            {
                inhibitNewItems = true
            };

            form.AddFields(
                new TextFieldData("uri", "URI")
                {
                    defaultValue = "datenmeister:///"
                },
                new TextFieldData("filepath", "Path to Xmi File")
                {
                    defaultValue =
                        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/"
                });

            return DotNetSetter.Convert(_viewLogic.GetInternalViewExtent(), form) as IElement;
        }

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

        /// <summary>
        /// Adds the views to the view logic
        /// </summary>
        public void AddToViewDefinition()
        {
            var viewExtent = _viewLogic.GetInternalViewExtent();
            var factory = viewExtent.GetFactory();
            var formAndFields = _workspaceLogic.GetTypesWorkspace().Get<_FormAndFields>();
            var package = _workspaceLogic.GetTypesWorkspace().extent.ElementAt(0).elements().ElementAt(0) as IObject;

            // Creates the package for "ManagementProvider" containing the views
            var umlPackage = _packageMethods.GetOrCreatePackageStructure(viewExtent.elements(), PackageName);

            var workspaceForm = GetWorkspaceListForm();
            var extentForm = GetExtentListForm();

            var workspaceFormDefaultView = factory.create(formAndFields.__ViewAssociation);
            workspaceFormDefaultView.SetProperties(
                new Dictionary<string, object>
                {
                    [_FormAndFields._ViewAssociation.metaclassName] = WorkspaceObject.MetaclassUriPath,
                    [_FormAndFields._ViewAssociation.view] = workspaceForm,
                    [_FormAndFields._ViewAssociation.viewType] = ViewType.Detail
                }
            );

            var extentFormDefaultView = factory.create(formAndFields.__ViewAssociation);
            extentFormDefaultView.SetProperties(
                new Dictionary<string, object>
                {
                    [_FormAndFields._ViewAssociation.metaclassName] = ExtentObject.MetaclassUriPath,
                    [_FormAndFields._ViewAssociation.view] = extentForm,
                    [_FormAndFields._ViewAssociation.viewType] = ViewType.Detail
                }
            );

            var extentListView = factory.create(formAndFields.__ViewAssociation);
            extentListView.SetProperties(
                new Dictionary<string, object>
                {
                    [_FormAndFields._ViewAssociation.metaclass] = WorkspaceObject.MetaclassUriPath,
                    [_FormAndFields._ViewAssociation.view] = extentForm,
                    [_FormAndFields._ViewAssociation.viewType] = ViewType.List
                }
            );

            var items = new List<IElement>
            {
                workspaceForm,
                extentForm,
                GetNewXmiExtentDetailForm(),
                GetNewWorkspaceDetail(),
                GetFindTypeForm(package),
                workspaceFormDefaultView,
                extentFormDefaultView,
                extentListView
            };
            _packageMethods.AddObjectsToPackage(umlPackage, items);
        }
    }
}