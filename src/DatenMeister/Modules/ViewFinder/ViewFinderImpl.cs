using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Modules.ViewFinder.Helper;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

namespace DatenMeister.Modules.ViewFinder
{
    /// <summary>
    /// Includes the implementation of the IViewFinder
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewFinderImpl
    {
        /// <summary>
        /// Stores a debug variable that can be used to extent the debugging of view retrieval process.
        /// </summary>
#if VIEWLOGICINFO
        private const bool ActivateDebuggingForViewRetrieval = true;
#warning Internal Debugging Info activated

#else
        private const bool ActivateDebuggingForViewRetrieval = false;
#endif

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


        /// <summary>
        /// Finds the extent form for the given extent
        /// </summary>
        /// <param name="extent"></param>
        /// <returns></returns>
        public IElement FindFormForExtentView(IUriExtent extent)
        {
            var extentType = extent.GetExtentType();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Finds the specific list form for the given extent and 
        /// </summary>
        /// <param name="extent">Extent to be shown</param>
        /// <param name="metaClass">Meta class of the elements that are shown in the current list</param>
        /// <returns>The found view</returns>
        public IElement FindListFormForExtentView(IUriExtent extent, IElement metaClass)
        {
            var extentType = extent.GetExtentType();
            if ((!string.IsNullOrEmpty(extentType)) || (metaClass != null))
            {
                var viewResult = FindBestViewFor(
                    new FindViewQuery
                    {
                        extentType = extentType,
                        metaClass = metaClass,
                        viewType = ViewType.TreeItemList
                    });

                if (viewResult != null)
                {
                    return viewResult;
                }
            }

            // Ok, find it by creating the properties
            return null;
        }

        /// <summary>
        /// Finds the form matching to the detail form
        /// </summary>
        /// <param name="element">Element to be parsed</param>
        /// <returns>The form to be found</returns>
        public IElement FindFormForDetailView(IObject element)
        {
            var valueAsElement = element as IElement;
            var metaClass = valueAsElement?.metaclass;

            if (valueAsElement != null)
            {
                // Check, if we have a specific form
                var viewResult = FindBestViewFor(
                    new FindViewQuery
                    {
                        metaClass = metaClass,
                        viewType = ViewType.Detail

                    });

                if (viewResult != null)
                {
                    return viewResult;
                }
            }

            return null;
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Finds the form which is used when the user clicks in the ItemExplorer View on one item
        /// </summary>
        /// <param name="element">Element which is clicked</param>
        /// <param name="uriExtent">The uri extent in which the element is hosted</param>
        /// <returns>Found detail form which is shown in the item explorer window</returns>
        public IElement FindFormForTreeItemDetailView(IObject element, IUriExtent uriExtent)
        {

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Finds the list form for the properties of the clicked item.
        /// This method is called by the automatic generator for each metaclass in the property of the item
        /// </summary>
        /// <param name="element">Element whose property is queried</param>
        /// <param name="property">Property which contains the collection</param>
        /// <param name="metaClass">Metaclass which is currently in interest</param>
        /// <returns>Found list form for the properties of the item</returns>
        public IElement FindListFormForTreeItemDetailView(IObject element, string property, IElement metaClass)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Finds the view matching to the most of the items
        /// </summary>
        /// <param name="viewType">The view type to be found</param>
        /// <param name="extentType">The extent type whose view is queried. May be null, if not relevant</param>
        /// <param name="metaClassName">The uri of the metaclass whose view is queried. May be null, if not relevant</param>
        /// <param name="metaClass">The element of the metaclass whose view is queried. May be null, if not relevant</param>
        /// <returns>The found view or null, if not found</returns>
        public IElement FindBestViewFor(
            FindViewQuery query)
        {
            var foundPoints = 0;
            IElement foundView = null;
            var viewAssociations = _viewLogic.GetAllViewAssociations().Select(x => x as IElement).ToList();
            InternalDebug("---");
            InternalDebug("# of ViewAssociations: " + viewAssociations.Count);

            foreach (
                var element in viewAssociations)
            {
                InternalDebug("-");
                var points = 0;
                if (element == null) throw new NullReferenceException("element");

                var innerExtentType = element.getOrDefault<string>(_FormAndFields._ViewAssociation.extentType);
                var innerMetaClass = element.getOrDefault<IElement>(_FormAndFields._ViewAssociation.metaclass);
                var innerViewType = element.getOrNull<ViewType>(_FormAndFields._ViewAssociation.viewType) ??
                                    ViewType.Detail;
                var innerForm = element.getOrDefault<IElement>(_FormAndFields._ViewAssociation.form);

                if (innerForm == null)
                {
                    Logger.Warn("Given form has null value. This is not recommended and will lead of unintended behavior of default views.");
                }

                var isMatching = true;

                /*
                // Now go through each property and get the points
                if (extentType != null && innerExtentType != null)
                {
                    if (extentType.Equals(innerExtentType))
                    {
                        InternalDebug("-- MATCH: ExtentType: " + extentType + ", ViewAssociation ExtentType: " +
                                      innerExtentType);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: ExtentType: " + extentType + ", ViewAssociation ExtentType: " +
                                      innerExtentType);
                        isMatching = false;
                    }
                }

                if (metaClassName != null && innerMetaClassName != null)
                {
                    if (metaClassName.Equals(innerMetaClassName))
                    {
                        InternalDebug("-- MATCH: metaClassName: " + metaClassName +
                                      ", ViewAssociation innerMetaClassName: " + innerMetaClassName);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: metaClassName: " + metaClassName +
                                      ", ViewAssociation innerMetaClassName: " + innerMetaClassName);
                        isMatching = false;
                    }
                }

                if (metaClass != null && innerMetaClass != null)
                {
                    if (metaClass.@equals(innerMetaClass))
                    {
                        InternalDebug("-- MATCH: metaClass: " + NamedElementMethods.GetName(metaClass) +
                                      ", ViewAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(innerMetaClass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: metaClass: " + NamedElementMethods.GetName(metaClass) +
                                      ", ViewAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(innerMetaClass));
                        isMatching = false;
                    }
                }

                if (!viewType.Equals(innerViewType))
                {
                    InternalDebug("-- NO MATCH: viewType: " + viewType + ", ViewAssociation viewType: " +
                                  innerViewType);
                    isMatching = false;
                }
                else
                {
                    InternalDebug("-- MATCH: viewType: " + viewType + ", ViewAssociation viewType: " + innerViewType);
                }

                InternalDebug("-- Points: " + points + ", Matched" + isMatching);

                // The matching view with the maximum points win
                if (isMatching)
                {
                    if (points > foundPoints)
                    {
                        foundPoints = points;
                        foundView = innerView;

                        InternalDebug("-- Selected!");
                    }
                }*/
            }

            return foundView;
        }
        /// <summary>
        /// Writes the information to the debugger, if the ActivateDebuggingForViewRetrieval is configured as true
        /// </summary>
        /// <param name="s"></param>
        private void InternalDebug(string s)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ActivateDebuggingForViewRetrieval)
#pragma warning disable 162
            {
                Logger.Trace(s);
            }
#pragma warning restore 162
        }

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ViewFinderImpl));

    }
}