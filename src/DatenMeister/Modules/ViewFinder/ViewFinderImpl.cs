using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder.Helper;
using DatenMeister.Runtime;

namespace DatenMeister.Modules.ViewFinder
{
    /// <summary>
    /// Includes the implementation of the IViewFinder
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewFinderImpl : IViewFinder
    {
        private readonly FormCreator _formCreator;
        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Initializes a new instance of the ViewFinderImpl class
        /// </summary>
        /// <param name="viewLogic">View logic</param>
        /// <param name="formCreator">Form Creator being used</param>
        public ViewFinderImpl(
            ViewLogic viewLogic, 
            FormCreator formCreator)
        {
            _viewLogic = viewLogic;
            _formCreator = formCreator;
        }

        /// <inheritdoc />
        /// <summary>
        /// Finds the specific view by name
        /// </summary>
        /// <param name="extent">Extent to be shown</param>
        /// <returns>The found view</returns>
        public IElement FindView(IUriExtent extent)
        {
            var extentType = extent.GetExtentType();
            if (!string.IsNullOrEmpty(extentType))
            {
                var viewResult = _viewLogic.FindViewForExtentType(extentType, ViewType.List);
                if (viewResult != null)
                {
                    return viewResult;
                }
            }

            // Ok, find it by creating the properties
            return null;
        }

        /// <summary>
        /// Finds the view for the given item.
        /// First, it looks for the specific instance
        /// Second, it looks by the given type
        /// </summary>
        /// <param name="value">Value, whose view is currently is requested</param>
        /// <returns>The found view or null, if not found</returns>
        public IElement FindListViewFor(IObject value)
        {
            if (value != null)
            {
                var viewResult = _viewLogic.FindViewForValue(value, ViewType.List);
                if (viewResult != null)
                {
                    return viewResult;
                }
            }

            // Ok, find it by creating the properties
            return null;
        }

        /// <summary>
        /// Creates an object for a reflective sequence by parsing each object and returning the formview
        /// showing the properties and extents
        /// </summary>
        /// <param name="sequence">Sequence to be used</param>
        /// <returns>Created form object</returns>
        public IElement CreateView(IReflectiveCollection sequence)
        {
            var form = _formCreator.CreateForm(sequence, FormCreator.CreationMode.All);
            return DotNetHelper.ConvertToMofElement(form, _viewLogic.GetViewExtent());
        }

        /// <summary>
        /// Creates an object for an element by parsing the properties of the element
        /// </summary>
        /// <param name="element">Element to be used</param>
        /// <returns>Created form object</returns>
        public IElement CreateView(IObject element)
        {
            var form = _formCreator.CreateForm(element, FormCreator.CreationMode.All);
            return DotNetHelper.ConvertToMofElement(form, _viewLogic.GetViewExtent());
        }

        /// <inheritdoc />
        /// <summary>
        /// Finds a specific view by the given value and the given viewname. 
        /// If the given viewname is empty or null, the default view will be returned
        /// </summary>
        /// <param name="value">Value whose view need to be created</param>
        /// <returns>The view itself</returns>
        public IElement FindView(IObject value)
        {
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

            return null;
        }

        /// <inheritdoc />
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