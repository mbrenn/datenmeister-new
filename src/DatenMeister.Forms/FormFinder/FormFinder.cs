#nullable enable

using System.Diagnostics;
using BurnSystems.Logging;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Uml.Helper;

// ReSharper disable HeuristicUnreachableCode

namespace DatenMeister.Forms.FormFinder
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

        private readonly FormMethods _formsMethods;

        /// <summary>
        /// Initializes a new instance of the ViewFinder class
        /// </summary>
        /// <param name="formsMethods">View logic</param>
        public FormFinder(
            FormMethods formsMethods)
        {
            _formsMethods = formsMethods;
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
            // Check, if the view mode is AutoGenerate. If that is the case, then no query will be performed
            if (query.viewModeId == ViewModes.AutoGenerate)
            {
                return Array.Empty<IElement>();
            }
            
            // Ok, View Mode is not AutoGenerate ==> Standard procedure to be executed
            using var stopWatch = new StopWatchLogger(Logger, $"Find Form: {query}", LogLevel.Trace);
            var formAssociations = _formsMethods.GetAllFormAssociations().Select(x => x as IElement).ToList();
            InternalDebug("---");
            InternalDebug("# of FormAssociations: " + formAssociations.Count);
            stopWatch.IntermediateLog("# of FormAssociations: " + formAssociations.Count);

            var foundForms = new List<FoundForm>();
            var queryViewModeIds = query.viewModeId?.Split(' ');

            foreach (var element in formAssociations)
            {
                InternalDebug("-");
                if (element is IHasId hasId)
                {
                    InternalDebug(
                        $"- Handling: ID = {hasId.Id ?? "none"}, Name = {NamedElementMethods.GetFullName(element)}");
                }

                // Checks, if we want to perform a break
                if (query.debugActive && element.getOrDefault<bool>(_DatenMeister._Forms._FormAssociation.debugActive))
                {
                    Debugger.Break();
                }

                var points = 0;
                if (element == null) throw new NullReferenceException("element");

                var associationExtentType =
                    element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.extentType);
                var associationMetaClass =
                    element.getOrDefault<IElement>(_DatenMeister._Forms._FormAssociation.metaClass);
                var associationViewType =
                    element.getOrNull<_DatenMeister._Forms.___FormType>(_DatenMeister._Forms._FormAssociation.formType);
                var associationParentMetaclass =
                    element.getOrDefault<IElement>(_DatenMeister._Forms._FormAssociation.parentMetaClass);
                var associationParentProperty =
                    element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.parentProperty);
                var associationForm = element.getOrDefault<IElement>(_DatenMeister._Forms._FormAssociation.form);
                var associationWorkspaceId = element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.workspaceId);
                var associationExtentUri = element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.extentUri);
                var associationViewModeId =
                    element.getOrDefault<string>(_DatenMeister._Forms._FormAssociation.viewModeId);
                if (string.IsNullOrEmpty(associationExtentType) && associationMetaClass == null
                                                  && associationParentMetaclass == null
                                                  && associationParentProperty == null
                                                  && associationViewModeId == null
                                                  && associationViewType == null
                                                  && string.IsNullOrEmpty(associationWorkspaceId)
                                                  && string.IsNullOrEmpty(associationExtentUri))
                {
                    InternalDebug("- - This item is too unspecific");
                    // Skip item because it is too unspecific
                    continue;
                }

                if (associationForm == null)
                {
                    Logger.Warn(
                        $"Given form of '{NamedElementMethods.GetName(element)}' has null value. " +
                        $"This is not recommended and will lead of unintended behavior of default views.");
                        if(Debugger.IsAttached) Debugger.Break();
                    continue;
                }

                var isMatching = true;

                // Now go through each property and get the points

                // ExtentType
                if (!string.IsNullOrEmpty(associationExtentType) && associationExtentType != null)
                {
                    if (query.extentTypes.Any()
                        && query.extentTypes.All(x=> x.Contains(associationExtentType)))
                    {
                        InternalDebug("-- MATCH: Requested ExtentType: " + string.Join(", ", query.extentTypes) + ", FormAssociation ExtentType: " +
                                      associationExtentType);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: Requested ExtentType: " + string.Join(", ", query.extentTypes) +
                                      ", FormAssociation ExtentType: " +
                                      associationExtentType);
                        isMatching = false;
                    }
                }

                // ViewMode Id
                if (!string.IsNullOrEmpty(associationViewModeId))
                {
                    if (queryViewModeIds != null
                        && queryViewModeIds.Contains(associationViewModeId))
                    {
                        InternalDebug("-- MATCH: Requested ViewMode: " + query.viewModeId + ", FormAssociation ViewModeId: " +
                                      associationViewModeId);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: Requested ViewMode: " + query.viewModeId +
                                      ", FormAssociation ViewMode: " +
                                      associationViewModeId);
                        isMatching = false;
                    }
                }

                // MetaClass
                if (associationMetaClass != null)
                {
                    if (query.metaClass != null && query.metaClass?.equals(associationMetaClass) == true)
                    {
                        InternalDebug("-- MATCH: Requested metaClass: " + NamedElementMethods.GetName(query.metaClass) +
                                      ", FormAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(associationMetaClass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: Requested metaClass: " + NamedElementMethods.GetName(query.metaClass) +
                                      ", FormAssociation innerMetaClass: " +
                                      NamedElementMethods.GetName(associationMetaClass));
                        isMatching = false;
                    }
                }

                // ViewType
                if (!query.FormType.Equals(associationViewType))
                {
                    InternalDebug("-- NO MATCH: Requested viewType: " + query.FormType + ", FormAssociation viewType: " +
                                  associationViewType);
                    isMatching = false;
                }
                else
                {
                    InternalDebug("-- MATCH: Requested viewType: " + query.FormType + ", FormAssociation viewType: " +
                                  associationViewType);
                }

                // ´ParentMetaClass
                if (associationParentMetaclass != null)
                {
                    if (query.parentMetaClass != null &&
                        query.parentMetaClass?.equals(associationParentMetaclass) == true)
                    {
                        InternalDebug("-- MATCH: Requested parentMetaClass: " +
                                      NamedElementMethods.GetName(query.parentMetaClass) +
                                      ", FormAssociation parentMetaClass: " +
                                      NamedElementMethods.GetName(associationParentMetaclass));
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: Requested parentMetaClass: " +
                                      NamedElementMethods.GetName(query.parentMetaClass) +
                                      ", FormAssociation parentMetaClass: " +
                                      NamedElementMethods.GetName(associationParentMetaclass));
                        isMatching = false;
                    }
                }

                // ParentProperty
                if (!string.IsNullOrEmpty(associationParentProperty))
                {
                    if (!string.IsNullOrEmpty(query.parentProperty) &&
                        query.parentProperty?.Equals(associationParentProperty) == true)
                    {
                        InternalDebug("-- MATCH: Requested ParentProperty: " + query.parentProperty +
                                      ", FormAssociation ParentProperty: " +
                                      associationParentProperty);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: Requested ParentProperty: " + query.parentProperty +
                                      ", FormAssociation ParentProperty: " +
                                      associationParentProperty);
                        isMatching = false;
                    }
                }

                // WorkspaceId
                if (!string.IsNullOrEmpty(associationWorkspaceId))
                {
                    if (!string.IsNullOrEmpty(query.workspaceId) &&
                        query.workspaceId?.Equals(associationWorkspaceId) == true)
                    {
                        InternalDebug("-- MATCH: Requested WorkspaceId: " + query.workspaceId +
                                      ", FormAssociation WorkspaceId: " +
                                      associationWorkspaceId);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: Requested WorkspaceId: " + query.workspaceId +
                                      ", FormAssociation WorkspaceId: " +
                                      associationWorkspaceId);
                        isMatching = false;
                    }
                }

                // WorkspaceId
                if (!string.IsNullOrEmpty(associationExtentUri))
                {
                    if (!string.IsNullOrEmpty(query.extentUri) &&
                        query.extentUri?.Equals(associationExtentUri) == true)
                    {
                        InternalDebug("-- MATCH: Requested extentUri: " + query.extentUri +
                                      ", FormAssociation extentUri: " +
                                      associationExtentUri);
                        points++;
                    }
                    else
                    {
                        InternalDebug("-- NO MATCH: Requested extentUri: " + query.extentUri +
                                      ", FormAssociation extentUri: " +
                                      associationExtentUri);
                        isMatching = false;
                    }
                }

                InternalDebug("-- Points: " + points + ", Matched: " + isMatching);

                // The matching view with the maximum points win
                if (isMatching)
                {
                    var foundForm = new FoundForm(
                        associationForm,
                        points);
                    foundForms.Add(foundForm);
                }
            }
            
            stopWatch.IntermediateLog("Forms are evaluated");

            var selectedForms = foundForms
                .OrderByDescending(x => x.Points)
                .Select(x => x.Form)
                .Select(x =>
                {
                    var formType = query.FormType;

                    return FormMethods.ConvertFormToObjectOrCollectionForm(x, formType);
                });

            return selectedForms;
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

        private static readonly ClassLogger Logger = new(typeof(FormFinder));
    }
}