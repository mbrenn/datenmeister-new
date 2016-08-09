using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Workspaces;
using DatenMeister.Web.Models.Modules.ViewFinder.Helper;

namespace DatenMeister.Web.Models.Modules.ViewFinder
{
    /// <summary>
    /// Includes the implementation of the IViewFinder
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewFinderImpl : IViewFinder
    {
        private readonly FormCreator _formCreator;
        private readonly IDotNetTypeLookup _dotNetTypeLookup;
        private readonly IWorkspaceCollection _workspaceCollection;
        private readonly ViewLogic _viewLogic;

        public ViewFinderImpl(
            IDotNetTypeLookup dotNetTypeLookup, 
            IWorkspaceCollection workspaceCollection,
            ViewLogic viewLogic)
        {
            _formCreator = new FormCreator();
            _dotNetTypeLookup = dotNetTypeLookup;
            _workspaceCollection = workspaceCollection;
            _viewLogic = viewLogic;
        }

        /// <summary>
        /// Finds the specific view by name
        /// </summary>
        /// <param name="extent">Extent to be shown</param>
        /// <param name="viewname">Name of the view</param>
        /// <returns>The found view</returns>
        public IObject FindView(IUriExtent extent, string viewname)
        {
            if (string.IsNullOrEmpty(viewname))
            {
                var view = _formCreator.CreateFields(extent);
                return _dotNetTypeLookup.CreateDotNetElement(view);
            }

            return _viewLogic.GetView(viewname);
        }

        public IObject FindView(IObject value, string viewname)
        {
            if (string.IsNullOrEmpty(viewname))
            {
                var view = _formCreator.CreateFields(value);
                return _dotNetTypeLookup.CreateDotNetElement(view);
            }
            
            return _viewLogic.GetView(viewname);
        }
    }
}