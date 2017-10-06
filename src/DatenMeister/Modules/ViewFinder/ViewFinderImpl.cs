﻿using System.Collections.Generic;
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
        /// <param name="viewUrl">Name of the view</param>
        /// <returns>The found view</returns>
        public IElement FindView(IUriExtent extent, string viewUrl)
        {
            if (viewUrl == "{All}")
            {
                var viewExtent = _viewLogic.GetViewExtent();
                var view = _formCreator.CreateForm(extent, FormCreator.CreationMode.All);
                return DotNetHelper.ConvertToMofElement(view, _viewLogic.GetViewExtent());
                //return _dotNetTypeLookup.CreateDotNetElement(InMemoryProvider.TemporaryExtent, view);
            }

            if (!string.IsNullOrEmpty(viewUrl))
            {
                _viewLogic.GetViewByUrl(viewUrl);
            }

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
            return FindView(extent, "{All}");
        }

        /// <summary>
        /// Creates an object for a reflective sequence by parsing each object and returning the formview
        /// showing the properties and extents
        /// </summary>
        /// <param name="sequence">Sequence to be used</param>
        /// <returns>Created form object</returns>
        public IElement CreateView(IReflectiveSequence sequence)
        {
            var form = _formCreator.CreateForm(sequence, FormCreator.CreationMode.All);
            return DotNetHelper.ConvertToMofElement(form, _viewLogic.GetViewExtent());
        }

        /// <inheritdoc />
        /// <summary>
        /// Finds a specific view by the given value and the given viewname. 
        /// If the given viewname is empty or null, the default view will be returned
        /// </summary>
        /// <param name="value">Value whose view need to be created</param>
        /// <param name="viewUrl">The view, that shall be done</param>
        /// <returns>The view itself</returns>
        public IElement FindView(IObject value, string viewUrl)
        {
            if (viewUrl == "{All}")
            {
                var form = _formCreator.CreateForm(
                    value,
                    FormCreator.CreationMode.All);

                return DotNetHelper.ConvertToMofElement(form, _viewLogic.GetViewExtent());
                // return _dotNetTypeLookup.CreateDotNetElement(InMemoryProvider.TemporaryExtent, form);
            }

            if (!string.IsNullOrEmpty(viewUrl))
            {
                var result =  _viewLogic.GetViewByUrl(viewUrl);
                if (result != null)
                {
                    return (IElement) result;
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