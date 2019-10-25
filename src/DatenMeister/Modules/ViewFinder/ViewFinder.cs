using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

// ReSharper disable HeuristicUnreachableCode

namespace DatenMeister.Modules.ViewFinder
{
    /// <summary>
    /// Includes the implementation of the IViewFinder
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ViewFinder
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

        private readonly ViewLogic _viewLogic;

        /// <summary>
        /// Initializes a new instance of the ViewFinder class
        /// </summary>
        /// <param name="viewLogic">View logic</param>
        public ViewFinder(
            ViewLogic viewLogic)
        {
            _viewLogic = viewLogic;
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
            => throw new InvalidOperationException();

        private class FoundForm
        {
            public int Points { get; set; }

            public IElement Form { get; set; }

            public FoundForm(IElement form, int points)
            {
                Form = form;
                Points = points;
            }
        }

        /// <summary>
        /// Finds the view matching to the most of the items
        /// </summary>
        /// <param name="query">Query being used to find the correct form</param>
        /// <returns>The found view or null, if not found</returns>
        public IEnumerable<IElement> FindFormsFor(FindViewQuery query)
        {
            var viewAssociations = _viewLogic.GetAllViewAssociations().Select(x => x as IElement).ToList();
            InternalDebug("---");
            InternalDebug("# of ViewAssociations: " + viewAssociations.Count);

            var foundForms = new List<FoundForm>();

            foreach (var element in viewAssociations)
            {
                InternalDebug("-");
                var points = 0;
                if (element == null) throw new NullReferenceException("element");

                var associationExtentType = element.getOrDefault<string>(_FormAndFields._ViewAssociation.extentType);
                var associationMetaClass = element.getOrDefault<IElement>(_FormAndFields._ViewAssociation.metaClass);
                var associationViewType = element.getOrNull<ViewType>(_FormAndFields._ViewAssociation.viewType) ??
                                    ViewType.Detail;
                var associationParentMetaclass =
                    element.getOrDefault<IElement>(_FormAndFields._ViewAssociation.parentMetaClass);
                var associationParentProperty =
                    element.getOrDefault<string>(_FormAndFields._ViewAssociation.parentProperty);
                var associationForm = element.getOrDefault<IElement>(_FormAndFields._ViewAssociation.form);

                if (associationForm == null)
                {
                    Logger.Warn("Given form has null value. This is not recommended and will lead of unintended behavior of default views.");
                }

                var isMatching = true;

                // Now go through each property and get the points

                // ExtentType
                if (!string.IsNullOrEmpty(query.extentType) || !string.IsNullOrEmpty(associationExtentType))
                {
                    if (query.extentType?.Equals(associationExtentType) == true)
                    {
                        InternalDebug("-- MATCH: ExtentType: " + query.extentType + ", ViewAssociation ExtentType: " +
                                      associationExtentType);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: ExtentType: " + query.extentType + ", ViewAssociation ExtentType: " +
                                      associationExtentType);
                        isMatching = false;
                    }
                }

                // MetaClass
                if (query.metaClass != null || associationMetaClass != null)
                {
                    if (query.metaClass?.@equals(associationMetaClass) == true)
                    {
                        InternalDebug("-- MATCH: metaClass: " + NamedElementMethods.GetName(query.metaClass) +
                                      ", ViewAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(associationMetaClass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: metaClass: " + NamedElementMethods.GetName(query.metaClass) +
                                      ", ViewAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(associationMetaClass));
                        isMatching = false;
                    }
                }

                // ViewType
                if (!query.viewType.Equals(associationViewType))
                {
                    InternalDebug("-- NO MATCH: viewType: " + query.viewType + ", ViewAssociation viewType: " +
                                  associationViewType);
                    isMatching = false;
                }
                else
                {
                    InternalDebug("-- MATCH: viewType: " + query.viewType + ", ViewAssociation viewType: " + associationViewType);
                }

                // ´ParentMetaClass
                if (query.parentMetaClass != null || associationParentMetaclass != null)
                {
                    if (query.parentMetaClass?.@equals(associationParentMetaclass) == true)
                    {
                        InternalDebug("-- MATCH: parentMetaClass: " + NamedElementMethods.GetName(query.parentMetaClass) +
                                      ", ViewAssociation parentMetaClass: " +
                                      NamedElementMethods.GetName(associationParentMetaclass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: parentMetaClass: " + NamedElementMethods.GetName(query.parentMetaClass) +
                                      ", ViewAssociation parentMetaClass: " +
                                      NamedElementMethods.GetName(associationParentMetaclass));
                        isMatching = false;
                    }
                }

                // ParentProperty
                if (!string.IsNullOrEmpty(query.parentProperty) || !string.IsNullOrEmpty(associationParentProperty))
                {
                    if (query.parentProperty?.Equals(associationParentProperty) == true)
                    {
                        InternalDebug("-- MATCH: ParentProperty: " + query.parentProperty + ", ViewAssociation ParentProperty: " +
                                      associationParentProperty);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: ParentProperty: " + query.parentProperty + ", ViewAssociation ParentProperty: " +
                                      associationParentProperty);
                        isMatching = false;
                    }
                }

                InternalDebug("-- Points: " + points + ", Matched" + isMatching);

                // The matching view with the maximum points win
                if (isMatching)
                {
                    var foundForm = new FoundForm(
                        associationForm,
                        points);
                    foundForms.Add(foundForm);
                }
            }

            return foundForms.OrderByDescending(x => x.Points).Select(x => x.Form);
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

        private static readonly ClassLogger Logger = new ClassLogger(typeof(ViewFinder));
    }
}