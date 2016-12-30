using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder.Helper;
using DatenMeister.Provider.DotNet;

namespace DatenMeister.Modules.ViewFinder
{
    /// <summary>
    /// Includes the implementation of the IViewFinder
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewFinderImpl : IViewFinder
    {
        private readonly FormCreator _formCreator;
        private readonly IDotNetTypeLookup _dotNetTypeLookup;
        private readonly ViewLogic _viewLogic;

        public ViewFinderImpl(
            IDotNetTypeLookup dotNetTypeLookup,
            ViewLogic viewLogic, 
            FormCreator formCreator)
        {
            _dotNetTypeLookup = dotNetTypeLookup;
            _viewLogic = viewLogic;
            _formCreator = formCreator;
        }

        /// <summary>
        /// Finds the specific view by name
        /// </summary>
        /// <param name="extent">Extent to be shown</param>
        /// <param name="viewUrl">Name of the view</param>
        /// <returns>The found view</returns>
        public IObject FindView(IUriExtent extent, string viewUrl)
        {
            if (viewUrl == "{All}")
            {
                var view = _formCreator.CreateForm(extent, FormCreator.CreationMode.All);
                return _dotNetTypeLookup.CreateDotNetElement(view);
            }

            if (!string.IsNullOrEmpty(viewUrl))
            {
                _viewLogic.GetViewByUrl(viewUrl);
            }

            if (extent.isSet(_FormAndFields._DefaultViewForExtentType.extentType))
            {
                var extentType = extent.get(_FormAndFields._DefaultViewForExtentType.extentType).ToString();
                var viewResult = _viewLogic.FindViewForExtentType(extentType, ViewType.List);
                if (viewResult != null)
                {
                    return viewResult;
                }
            }

            // Ok, find it by creating the properties
            return FindView(extent, "{All}");
        }

        /// <summary>
        /// Finds a specific view by the given value and the given viewname. 
        /// If the given viewname is empty or null, the default view will be returned
        /// </summary>
        /// <param name="value">Value whose view need to be created</param>
        /// <param name="viewUrl">The view, that shall be done</param>
        /// <returns>The view itself</returns>
        public IObject FindView(IObject value, string viewUrl)
        {
            if (viewUrl == "{All}")
            {
                var form = _formCreator.CreateForm(
                    value,
                    FormCreator.CreationMode.All);
                return _dotNetTypeLookup.CreateDotNetElement(form);
            }

            if (!string.IsNullOrEmpty(viewUrl))
            {
                var result =  _viewLogic.GetViewByUrl(viewUrl);
                if (result != null)
                {
                    return result;
                }
            }

            var valueAsElement = value as IElement;
            var metaClass = valueAsElement?.metaclass;

            if (valueAsElement != null)
            {
                // Check, if we have a specific form
                var viewResult = _viewLogic.FindViewFor(metaClass, ViewType.Detail);
                if (viewResult != null)
                {
                    return viewResult;
                }
            }

            return FindView(value, "{All}");
        }

        /// <summary>
        /// Finds all views that are allowed for the given extent and value
        /// </summary>
        /// <param name="extent">Extent being queried</param>
        /// <param name="value">Value being queried</param>
        /// <returns>Enumeration of objects</returns>
        public IEnumerable<IElement> FindViews(IUriExtent extent, IObject value)
        {
            return _viewLogic.GetAllViews().Select(x => x as IElement);
        }
    }
}