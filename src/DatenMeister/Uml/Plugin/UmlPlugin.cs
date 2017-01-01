using System;
using DatenMeister.Core.EMOF.Interface.Reflection;
 using DatenMeister.Core.Plugins;
 using DatenMeister.Models.Forms;
 using DatenMeister.Modules.ViewFinder;
 using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.XMI;
using DatenMeister.Runtime;
 using DatenMeister.Runtime.Workspaces;
 using DatenMeister.Uml.Helper;

namespace DatenMeister.Uml.Plugin
{
    // ReSharper disable once UnusedMember.Global
    public class UmlPlugin : IDatenMeisterPlugin
    {
        private readonly IWorkspaceLogic _workspaceLogic;

        private readonly NamedElementMethods _namedElementMethods;
        private readonly IDotNetTypeLookup _typeLookup;

        /// <summary>
        /// Defines the view logic
        /// </summary>
        private readonly ViewLogic _viewLogic;

        public UmlPlugin(IWorkspaceLogic workspaceLogic, NamedElementMethods namedElementMethods, IDotNetTypeLookup typeLookup, ViewLogic viewLogic)
        {
            _workspaceLogic = workspaceLogic;
            _namedElementMethods = namedElementMethods;
            _typeLookup = typeLookup;
            _viewLogic = viewLogic;
        }

        public void Start()
        {
            InitViews();
        }

        /// <summary>
        /// Initializes the views for the given extent
        /// </summary>
        private void InitViews()
        {
            var umlExtent = _workspaceLogic.FindExtent(WorkspaceNames.UriUml);

            var classView = new DefaultViewForMetaclass(
                WorkspaceNames.UriUml + "#Class",
                ViewType.Detail,
                new Form(
                    "Class",
                    new TextFieldData("name", "Classname"),
                    new SubElementFieldData("ownedAttribute", "Properties"),
                    new SubElementFieldData("attribute", "Properties")));
            
            AddForView(classView);

            var packageView = new DefaultViewForMetaclass(
                WorkspaceNames.UriUml + "#Package",
                ViewType.Detail,
                new Form(
                    "Package",
                    new TextFieldData("name", "Classname")));

            AddForView(packageView);


            var classExtentView = new DefaultViewForExtentType(
                "Uml.Classes",
                new Form(
                    "Class",
                    new TextFieldData("name", "Classname"),
                    new SubElementFieldData("ownedAttribute", "Properties"),
                    new SubElementFieldData("attribute", "Properties")));

            AddForView(classExtentView);
        }

        /// <summary>
        /// Adds a default view for as a detail and list item
        /// </summary>
        /// <param name="classView">Defaultview to be added</param>
        private void AddForView(DefaultViewForMetaclass classView)
        {
            throw  new NotImplementedException();
            /*
            var element = _typeLookup.CreateDotNetElement(classView);
            _viewLogic.Add(element);
            */
        }

        /// <summary>
        /// Adds a default view for as a detail and list item
        /// </summary>
        /// <param name="classView">Defaultview to be added</param>
        private void AddForView(DefaultViewForExtentType classView)
        {
            throw new NotImplementedException();
            /*
            var element = _typeLookup.CreateDotNetElement(classView);
            _viewLogic.Add(element);
            */
        }
    }
}