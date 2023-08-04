using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
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
            result |= IncludeCreationButtonsInTableFormForClassifierOfExtentType(context, form);
            result |= IncludeExtentTypesForTableFormOfExtent(context, form);
            result |= IncludeCreationButtonsInDetailFormOfPackageForClassifierOfExtentType(context, form);
            result |= IncludeJumpToExtentButton(context, form);
            
            return result;
        }

        /// <summary>
        /// If the user shows all items of an extent in the extent overview, the
        /// user can click on the button to get to the extent itself
        /// </summary>
        /// <param name="context"></param>
        /// <param name="form"></param>
        /// <returns>true, if matching has occured</returns>
        private bool IncludeJumpToExtentButton(FormCreationContext context, IElement form)
        {
            if (context.FormType == _DatenMeister._Forms.___FormType.Collection)
            {
                var fields = form.get<IReflectiveCollection>(
                    _DatenMeister._Forms._CollectionForm.field);
                var factory = new MofFactory(form);
                var field = factory.create(_DatenMeister.TheOne.Forms.__ActionFieldData);
                field.set(_DatenMeister._Forms._ActionFieldData.name, "Go to Extent");
                field.set(_DatenMeister._Forms._ActionFieldData.title, "Go to Extent");
                field.set(_DatenMeister._Forms._ActionFieldData.actionName, "Extent.NavigateTo.Extent");

                fields.add(field);
                return true;
            }

            return false;
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
        private bool IncludeCreationButtonsInTableFormForClassifierOfExtentType(FormCreationContext context, IElement form)
        {
            // Finds the extent type fitting to the extent to be shown
            var foundExtentTypes =
                _extentSettings.extentTypeSettings.Where(x => context.ExtentTypes.Contains(x.name)).ToList();

            if (!foundExtentTypes.Any() || context.FormType != _DatenMeister._Forms.___FormType.Collection)
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

                foreach (var foundExtentType in foundExtentTypes)
                {
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
            }

            return true;
        }

        /**
         * Includes the creation buttons for all Properties in SubElementFields for packagedItems for the
         * default root classes of a certain extent type
         */
        private bool IncludeCreationButtonsInDetailFormOfPackageForClassifierOfExtentType(
            FormCreationContext context,
            IElement form)
        {
            var changed = false;

            // Finds the extent type fitting to the extent to be shown
            var foundExtentTypes =
                _extentSettings.extentTypeSettings.Where(x => context.ExtentTypes.Contains(x.name)).ToList();

            if (!foundExtentTypes.Any() || context.FormType != _DatenMeister._Forms.___FormType.Object)
            {
                return false;
            }

            // Check, if the detail element is from type package
            if ((context.DetailElement as IElement)?.metaclass?.equals(_UML.TheOne.Packages.__Package) != true
                && (context.DetailElement as IElement)?.metaclass?.equals(_DatenMeister.TheOne.CommonTypes.Default.__Package) != true)
            {
                return false;
            }

            // Now, go through the forms and look for the subelements of packagedElements
            var rowForms = FormMethods.GetRowForms(form).ToList();
            foreach (var rowForm in rowForms)
            {
                var foundFieldForPackagedElement = FormMethods.GetFieldForProperty(rowForm,
                    _DatenMeister._CommonTypes._Default._Package.packagedElement);
                if (
                    foundFieldForPackagedElement?.metaclass?.equals(_DatenMeister.TheOne.Forms.__SubElementFieldData) ==
                    true)
                {
                    var defaultTypesForNewElements = foundFieldForPackagedElement.get<IReflectiveCollection>(
                        _DatenMeister._Forms._SubElementFieldData.defaultTypesForNewElements);
                    // We found it, so add the stuff

                    foreach (var foundExtentType in foundExtentTypes)
                    {
                        foreach (var rootMetaClass in foundExtentType.rootElementMetaClasses)
                        {
                            var resolvedMetaClass =
                                _workspaceLogic.ResolveElement(rootMetaClass, ResolveType.OnlyMetaWorkspaces);

                            if (resolvedMetaClass == null)
                            {
                                continue;
                            }

                            changed = true;
                            defaultTypesForNewElements.add(resolvedMetaClass);

                            FormMethods.AddToFormCreationProtocol(rowForm,
                                $"ExtentTypeFormsPlugin: Added {NamedElementMethods.GetName(resolvedMetaClass)} by ExtentType '{foundExtentType.name}' to PackagedElement");
                        }
                    }
                }
            }

            return changed;
        }
    }
}