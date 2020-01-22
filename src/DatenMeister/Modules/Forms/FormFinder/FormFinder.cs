#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models.Forms;
using DatenMeister.Runtime;
using DatenMeister.Uml.Helper;

// ReSharper disable HeuristicUnreachableCode

namespace DatenMeister.Modules.Forms.FormFinder
{
    /// <summary>
    /// Searches a form by looking through the form extents
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FormFinder
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

        private readonly FormLogic _formLogic;

        /// <summary>
        /// Initializes a new instance of the ViewFinder class
        /// </summary>
        /// <param name="formLogic">View logic</param>
        public FormFinder(
            FormLogic formLogic)
        {
            _formLogic = formLogic;
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
        public IEnumerable<IElement> FindFormsFor(FindFormQuery query)
        {
            var formAssociations = _formLogic.GetAllFormAssociations().Select(x => x as IElement).ToList();
            InternalDebug("---");
            InternalDebug("# of FormAssociations: " + formAssociations.Count);

            var foundForms = new List<FoundForm>();

            foreach (var element in formAssociations)
            {
                InternalDebug("-");
                var points = 0;
                if (element == null) throw new NullReferenceException("element");

                var associationExtentType = element.getOrDefault<string>(_FormAndFields._FormAssociation.extentType);
                var associationMetaClass = element.getOrDefault<IElement>(_FormAndFields._FormAssociation.metaClass);
                var associationViewType = element.getOrNull<FormType>(_FormAndFields._FormAssociation.formType) ??
                                    FormType.Detail;
                var associationParentMetaclass =
                    element.getOrDefault<IElement>(_FormAndFields._FormAssociation.parentMetaClass);
                var associationParentProperty =
                    element.getOrDefault<string>(_FormAndFields._FormAssociation.parentProperty);
                var associationForm = element.getOrDefault<IElement>(_FormAndFields._FormAssociation.form);
                if (associationExtentType == null && associationMetaClass == null
                                                  && associationParentMetaclass == null 
                                                  && associationParentProperty == null)
                {
                    // Skip item because it is too unspecific
                    continue;
                }

                if (associationForm == null)
                {
                    Logger.Warn("Given form has null value. This is not recommended and will lead of unintended behavior of default views.");
                    continue;
                }

                var isMatching = true;

                // Now go through each property and get the points

                // ExtentType
                if (!string.IsNullOrEmpty(associationExtentType))
                {
                    if (!string.IsNullOrEmpty(query.extentType)
                        && query.extentType.Equals(associationExtentType))
                    {
                        InternalDebug("-- MATCH: ExtentType: " + query.extentType + ", FormAssociation ExtentType: " +
                                      associationExtentType);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: ExtentType: " + query.extentType +
                                      ", FormAssociation ExtentType: " +
                                      associationExtentType);
                        isMatching = false;
                    }
                }

                // MetaClass
                if (associationMetaClass != null)
                {
                    if (query.metaClass != null && query.metaClass?.@equals(associationMetaClass) == true)
                    {
                        InternalDebug("-- MATCH: metaClass: " + NamedElementMethods.GetName(query.metaClass) +
                                      ", FormAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(associationMetaClass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: metaClass: " + NamedElementMethods.GetName(query.metaClass) +
                                      ", FormAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(associationMetaClass));
                        isMatching = false;
                    }
                }

                // ViewType
                if (!query.FormType.Equals(associationViewType))
                {
                    InternalDebug("-- NO MATCH: viewType: " + query.FormType + ", FormAssociation viewType: " +
                                  associationViewType);
                    isMatching = false;
                }
                else
                {
                    InternalDebug("-- MATCH: viewType: " + query.FormType + ", FormAssociation viewType: " + associationViewType);
                }

                // ´ParentMetaClass
                if (associationParentMetaclass != null)
                {
                    if (query.parentMetaClass != null && query.parentMetaClass?.@equals(associationParentMetaclass) == true)
                    {
                        InternalDebug("-- MATCH: parentMetaClass: " + NamedElementMethods.GetName(query.parentMetaClass) +
                                      ", FormAssociation parentMetaClass: " +
                                      NamedElementMethods.GetName(associationParentMetaclass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: parentMetaClass: " + NamedElementMethods.GetName(query.parentMetaClass) +
                                      ", FormAssociation parentMetaClass: " +
                                      NamedElementMethods.GetName(associationParentMetaclass));
                        isMatching = false;
                    }
                }

                // ParentProperty
                if (!string.IsNullOrEmpty(associationParentProperty))
                {
                    if (!string.IsNullOrEmpty(query.parentProperty) && query.parentProperty?.Equals(associationParentProperty) == true)
                    {
                        InternalDebug("-- MATCH: ParentProperty: " + query.parentProperty + ", FormAssociation ParentProperty: " +
                                      associationParentProperty);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: ParentProperty: " + query.parentProperty + ", FormAssociation ParentProperty: " +
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

        private static readonly ClassLogger Logger = new ClassLogger(typeof(FormFinder));
    }
}