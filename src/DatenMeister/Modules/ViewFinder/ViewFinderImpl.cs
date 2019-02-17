using System.Collections.Generic;
using System.Linq;
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
    public class ViewFinderImpl
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
        /// <param name="metaClass">Meta class of the elements that are shown in the current list</param>
        /// <returns>The found view</returns>
        public IElement FindListView(IUriExtent extent, IElement metaClass)
        {
            var extentType = extent.GetExtentType();
            if ((!string.IsNullOrEmpty(extentType)) || (metaClass != null))
            {
                var viewResult = _viewLogic.FindViewForExtentType(extentType, metaClass, ViewType.List);
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
        /// <param name="value">Value, whose view is currently is requested as a list</param>
        /// <param name="metaClass">Meta class of the elements that are shown in the current list</param>
        /// <returns>The found view or null, if not found</returns>
        public IElement FindListViewFor(IObject value, IElement metaClass)
        {
            if (metaClass != null)
            {
                var viewResult = _viewLogic.FindViewFor(metaClass, ViewType.List);
                if (viewResult != null)
                {
                    return viewResult;
                }
            }

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
        /// Finds the view matching to the most of the items and constraints
        /// </summary>
        /// <param name="viewType">The view type to be found</param>
        /// <param name="extentType">The extent type whose view is queried. May be null, if not relevant</param>
        /// <param name="metaClassName">The uri of the metaclass whose view is queried. May be null, if not relevant</param>
        /// <param name="metaClass">The element of the metaclass whose view is queried. May be null, if not relevant</param>
        /// <returns>The found view or null, if not found</returns>
        public IElement FindViewFor(
            ViewType viewType,
            string extentType,
            string metaClassName,
            IElement metaClass)
        {
            return _viewLogic.FindViewFor(viewType, extentType, metaClassName, metaClass);
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
            return DotNetHelper.ConvertToMofElement(form, _viewLogic.GetInternalViewExtent());
        }

        /// <summary>
        /// Creates an object for an element by parsing the properties of the element
        /// </summary>
        /// <param name="element">Element to be used</param>
        /// <returns>Created form object</returns>
        public IElement CreateView(IObject element)
        {
            var form = _formCreator.CreateForm(element, FormCreator.CreationMode.All);
            return DotNetHelper.ConvertToMofElement(form, _viewLogic.GetInternalViewExtent());
        }

        /// <inheritdoc />
        /// <summary>
        /// Finds a specific view by the given value and the given viewname. 
        /// If the given viewname is empty or null, the default view will be returned
        /// </summary>
        /// <param name="value">Value whose view need to be created</param>
        /// <returns>The view itself</returns>
        public IElement FindDetailView(IObject value)
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
            return _viewLogic.GetAllForms().Select(x => x as IElement);
        }
    }
}