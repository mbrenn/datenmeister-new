#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Models;
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
        private const bool ConfigurationActivateDebuggingForViewRetrieval = true;
#warning Internal Debugging Info activated
#else
        private const bool ConfigurationActivateDebuggingForViewRetrieval = false;
#endif

        private readonly FormsPlugin _formsPlugin;

        /// <summary>
        /// Initializes a new instance of the ViewFinder class
        /// </summary>
        /// <param name="formsPlugin">View logic</param>
        public FormFinder(
            FormsPlugin formsPlugin)
        {
            _formsPlugin = formsPlugin;
        }

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
            var formAssociations = _formsPlugin.GetAllFormAssociations().Select(x => x as IElement).ToList();
            InternalDebug("---");
            InternalDebug("# of FormAssociations: " + formAssociations.Count);

            var foundForms = new List<FoundForm>();

            foreach (var element in formAssociations)
            {
                InternalDebug("-");
                if (element is IHasId hasId)
                {
                    InternalDebug(
                        $"- Handling: ID = {hasId.Id ?? "none"}, Name = {NamedElementMethods.GetFullName(element)}");
                }

                var points = 0;
                if (element == null) throw new NullReferenceException("element");

                var associationExtentType = element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.extentType);
                var associationMetaClass = element.getOrDefault<IElement>(_DatenMeister._Forms._FormAssociation.metaClass);
                var associationViewType = 
                    element.getOrNull<FormType>(_DatenMeister._Forms._FormAssociation.formType) ??
                                    FormType.Detail;
                var associationParentMetaclass =
                    element.getOrDefault<IElement>(_DatenMeister._Forms._FormAssociation.parentMetaClass);
                var associationParentProperty =
                    element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.parentProperty);
                var associationForm = element.getOrDefault<IElement>(_DatenMeister._Forms._FormAssociation.form);
                var associationViewModeId = element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.viewModeId);
                if (associationExtentType == null && associationMetaClass == null
                                                  && associationParentMetaclass == null 
                                                  && associationParentProperty == null
                                                  && associationViewModeId == null)
                {
                    InternalDebug("- - This item is too unspecific");
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
                        && query.extentType.Contains(associationExtentType))
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
                
                // ViewMode Id
                if (!string.IsNullOrEmpty(associationViewModeId))
                {
                    if (!string.IsNullOrEmpty(query.viewModeId)
                        && query.viewModeId.Equals(associationViewModeId))
                    {
                        InternalDebug("-- MATCH: ViewMode: " + query.viewModeId + ", FormAssociation ViewModeId: " +
                                      associationViewModeId);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: ViewMode: " + query.viewModeId +
                                      ", FormAssociation ViewMode: " +
                                      associationViewModeId);
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
        /// Writes the information to the debugger, if the ConfigurationActivateDebuggingForViewRetrieval is configured as true
        /// </summary>
        /// <param name="s"></param>
        private void InternalDebug(string s)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (ConfigurationActivateDebuggingForViewRetrieval)
#pragma warning disable 162
            {
                Logger.Trace(s);
            }
#pragma warning restore 162
        }

        private static readonly ClassLogger Logger = new ClassLogger(typeof(FormFinder));
    }
}