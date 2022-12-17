﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Extent.Forms
{
    /// <summary>
    /// Modifies the form according to the Extent Type
    /// </summary>
    public class ExtentTypeFormModification : IFormModificationPlugin
    {
        private readonly IWorkspaceLogic _workspaceLogic;
        private readonly ExtentSettings _extentSettings;

        public ExtentTypeFormModification(IWorkspaceLogic workspaceLogic, ExtentSettings extentSettings)
        {
            _workspaceLogic = workspaceLogic;
            _extentSettings = extentSettings;
        }

        /// <summary>
        /// Performs some modifications for the extent forms
        /// </summary>
        /// <param name="context">Context to be used</param>
        /// <param name="form">Form to be sued</param>
        /// <returns></returns>
        public bool ModifyForm(FormCreationContext context, IElement form)
        {
            var result = false;
            result |= IncludeCreationButtonsForClassifierOfExtentType(context, form);
            result |= IncludeExtentTypesForTableFormOfExtent(context, form);

            return result;
        }
        
        /// <summary>
        /// If the user shows the extent, the checkbox tagging field for the extent will receive
        /// additional values for the known extent types 
        /// </summary>
        /// <param name="context">Form creation to be used</param>
        /// <param name="form">Form to be used</param>
        private bool IncludeExtentTypesForTableFormOfExtent(FormCreationContext context, IElement form)
        {
            if (context.FormType != _DatenMeister._Forms.___FormType.Row ||
                context.MetaClass?.equals(_DatenMeister.TheOne.Management.__Extent) != true)
            {
                return false;
            }
            
            // Got it, look for the field
            var field =
                FormMethods.GetField(
                    form,
                    _DatenMeister._Management._Extent.extentType,
                    _DatenMeister.TheOne.Forms.__CheckboxListTaggingFieldData);

            if (field != null)
            {
                var values =
                    field.get<IReflectiveCollection>(_DatenMeister._Forms._CheckboxListTaggingFieldData.values);
                var valuesAsList = values
                    .OfType<IElement>()
                    .ToList();

                var extentTypes = _extentSettings.extentTypeSettings.Select(x => x.name).ToList();
                var factory = new MofFactory(field);
                foreach (var extentType in extentTypes)
                {
                    if (valuesAsList.Any(x =>
                            x.getOrDefault<string>(_DatenMeister._Forms._ValuePair.value) == extentType))
                    {
                        // Already added, otherwise add the value pair
                        continue;
                    }

                    var valuePair = factory.create(_DatenMeister.TheOne.Forms.__ValuePair);
                    valuePair.set(_DatenMeister._Forms._ValuePair.name, extentType);
                    valuePair.set(_DatenMeister._Forms._ValuePair.value, extentType);
                    values.add(valuePair);
                }
            }

            return true;
        }

        /// <summary>
        /// Modifies the form by figuring out the extent type of the currently shown Collection Form.
        /// The extent type includes information about the supported metaclasses of the extent type themselves. 
        /// After that, they are going through each table form and adds the buttons to create new instances.
        ///
        /// The table form which is not associated to a classifier will receive creation buttons for all
        /// to the extenttype's associated classifiers except the ones where a table form is already existing.
        ///  
        /// </summary>
        /// <param name="context">Form Creation Context to be used</param>
        /// <param name="form">Form to be used</param>
        /// <returns>true, if the form has been modified. </returns>
        private bool IncludeCreationButtonsForClassifierOfExtentType(FormCreationContext context, IElement form)
        {
            // Finds the extent type fitting to the extent to be shown
            var foundExtentType =
                _extentSettings.extentTypeSettings.FirstOrDefault(x => x.name == context.ExtentType);

            if (foundExtentType == null || context.FormType != _DatenMeister._Forms.___FormType.Collection)
            {
                return false;
            }
            
            var foundListMetaClasses = new List<IElement>();

            var tableForms = FormMethods.GetTableForms(form).ToList();
            // First, figure out which list forms we are having...
            foreach (var listForm in tableForms)
            {
                var metaClass = listForm.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass);
                foundListMetaClasses.Add(metaClass);
            }

            // Second, now create the action buttons for the tableform without classifier
            foreach (var listForm in tableForms)
            {
                // Selects only the listform which do not have a classifier
                if (listForm.getOrDefault<IElement>(_DatenMeister._Forms._TableForm.metaClass) != null)
                {
                    continue;
                }

                foreach (var rootMetaClass in foundExtentType.rootElementMetaClasses)
                {
                    var resolvedMetaClass =
                        _workspaceLogic.ResolveElement(rootMetaClass, ResolveType.OnlyMetaWorkspaces);

                    if (resolvedMetaClass == null)
                    {
                        continue;
                    }

                    // The found metaclass already has a list form
                    if (foundListMetaClasses.Contains(resolvedMetaClass))
                    {
                        FormMethods.AddToFormCreationProtocol(listForm,
                            $"ExtentTypeFormsPlugin: Did not add {NamedElementMethods.GetName(resolvedMetaClass)} for ExtentType '{foundExtentType.name}' since it already got a listform");
                        continue;
                    }

                    FormMethods.AddDefaultTypeForNewElement(form, resolvedMetaClass);

                    FormMethods.AddToFormCreationProtocol(listForm,
                        $"ExtentTypeFormsPlugin: Added {NamedElementMethods.GetName(resolvedMetaClass)} by ExtentType '{foundExtentType.name}'");
                }
            }

            return true;

        }
    }
}