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
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.Helper;

namespace DatenMeister.Extent.Forms;

/// <summary>
/// Modifies the form according to the Extent Type
/// </summary>
public class ExtentTypeFormModification
{
    public class IncludeJumpToExtentButtonModification : INewCollectionFormFactory
    {
        public void CreateCollectionForm(CollectionFormFactoryParameter parameter, NewFormCreationContext context,
            FormCreationResult result)
        {
            if (result.Form == null)
            {
                throw new InvalidOperationException("Form is null");
            }

            var fields = result.Form.get<IReflectiveCollection>(_Forms._CollectionForm.field);
            var factory = new MofFactory(result.Form);
            var field = factory.create(_Forms.TheOne.__ActionFieldData);
            field.set(_Forms._ActionFieldData.name, "Go to Extent");
            field.set(_Forms._ActionFieldData.title, "Go to Extent");
            field.set(_Forms._ActionFieldData.actionName, "DatenMeister.Navigation.ToExtent");

            fields.add(field);
            result.IsManaged = true;
        }
    }

    public class IncludeExtentTypesForTableFormExtent(ExtentSettings extentSettings) : INewRowFormFactory
    {
        public void CreateRowForm(RowFormFactoryParameter parameter, NewFormCreationContext context, FormCreationResult result)
        {
            var metaClass = parameter.MetaClass;
            
            if (metaClass == null || metaClass.equals(_Management.TheOne.__Extent) != true)
            {
                return;
            }

            if (result.Form == null)
            {
                throw new InvalidOperationException("Form is null");
            }
            
            // Got it, look for the field
            var field =
                FormMethods.GetField(
                    result.Form,
                    _Management._Extent.extentType,
                    _Forms.TheOne.__CheckboxListTaggingFieldData);

            if (field != null)
            {
                var values =
                    field.get<IReflectiveCollection>(_Forms._CheckboxListTaggingFieldData.values);
                var valuesAsList = values
                    .OfType<IElement>()
                    .ToList();

                var extentTypes = extentSettings.extentTypeSettings.Select(x => x.name).ToList();
                var factory = new MofFactory(field);
                foreach (var extentType in extentTypes)
                {
                    if (valuesAsList.Any(x =>
                            x.getOrDefault<string>(_Forms._ValuePair.value) == extentType))
                    {
                        // Already added, otherwise add the value pair
                        continue;
                    }

                    var valuePair = factory.create(_Forms.TheOne.__ValuePair);
                    valuePair.set(_Forms._ValuePair.name, extentType);
                    valuePair.set(_Forms._ValuePair.value, extentType);
                    values.add(valuePair);
                }
                
                result.IsManaged = true;
            }
        }
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
    public class IncludeCreationButtonsInTableFormForClassifierOfExtentType(
        IWorkspaceLogic workspaceLogic,
        ExtentSettings extentSettings)
        : INewCollectionFormFactory
    {
        public void CreateCollectionForm(
            CollectionFormFactoryParameter parameter, NewFormCreationContext context,
            FormCreationResult result)
        {
            if (parameter.Collection == null)
                return;
            
            var collection = parameter.Collection;
            var extentTypes = collection.GetUriExtentOf()?.GetConfiguration().ExtentTypes;
            if (extentTypes == null)
                return;
            
            // Finds the extent type fitting to the extent to be shown
            var foundExtentTypes =
                extentSettings.extentTypeSettings.Where(x => extentTypes.Contains(x.name)).ToList();

            if (foundExtentTypes.Count == 0)
                return;

            if (result.Form == null)
                throw new InvalidOperationException("Form is null");

            var foundListMetaClasses = new List<IElement>();

            var tableForms = FormMethods.GetTableForms(result.Form).ToList();
            // First, figure out which list forms we are having...
            foreach (var listForm in tableForms)
            {
                var metaClass = listForm.getOrDefault<IElement>(_Forms._TableForm.metaClass);
                foundListMetaClasses.Add(metaClass);
            }

            // Second, now create the action buttons for the tableform without classifier
            foreach (var listForm in tableForms)
            {
                // Selects only the listform which do not have a classifier
                if (listForm.getOrDefault<IElement>(_Forms._TableForm.metaClass) != null)
                {
                    continue;
                }

                foreach (var foundExtentType in foundExtentTypes)
                {
                    foreach (var rootMetaClass in foundExtentType.rootElementMetaClasses)
                    {
                        var resolvedMetaClass =
                            workspaceLogic.ResolveElement(rootMetaClass, ResolveType.OnlyMetaWorkspaces);

                        if (resolvedMetaClass == null)
                        {
                            continue;
                        }

                        // The found metaclass already has a list form
                        if (foundListMetaClasses.Contains(resolvedMetaClass))
                        {
                            result.AddToFormCreationProtocol(
                                $"ExtentTypeFormsPlugin: Did not add {NamedElementMethods.GetName(resolvedMetaClass)} for ExtentType '{foundExtentType.name}' since it already got a listform");
                            continue;
                        }

                        FormMethods.AddDefaultTypeForNewElement(result.Form, resolvedMetaClass);

                        result.AddToFormCreationProtocol(
                            $"ExtentTypeFormsPlugin: Added {NamedElementMethods.GetName(resolvedMetaClass)} by ExtentType '{foundExtentType.name}'");
                    }
                }
            }

            result.IsManaged = true;
        }

        public void CreateCollectionFormForMetaClass(IElement metaClass, NewFormCreationContext context,
            FormCreationResult result)
        {
        }
    }

    /**
     * Includes the creation buttons for all Properties in SubElementFields for packagedItems for the
     * default root classes of a certain extent type
     */
    public class NewIncludeCreationButtonsInDetailFormOfPackageForClassifierOfExtentType(
        IWorkspaceLogic workspaceLogic,
        ExtentSettings extentSettings) : INewObjectFormFactory
    {
        public void CreateObjectForm(ObjectFormFactoryParameter paramete, NewFormCreationContext context, FormCreationResult result)
        {
            var element = paramete.Element;
            if (element == null)
                return;
            
            var extentTypes = element.GetUriExtentOf()?.GetConfiguration().ExtentTypes;

            var changed = false;
            if (extentTypes == null)
                return;

            // Finds the extent type fitting to the extent to be shown
            var foundExtentTypes =
                extentSettings.extentTypeSettings.Where(x => extentTypes.Contains(x.name)).ToList();

            if (foundExtentTypes.Count == 0)
                return;

            if (result.Form == null)
                throw new InvalidOperationException("Form is null");

            // Check, if the detail element is from type package
            if ((element as IElement)?.metaclass?.equals(_UML.TheOne.Packages.__Package) != true
                && (element as IElement)?.metaclass?.equals(_CommonTypes.TheOne.Default.__Package) != true)
            {
                return;
            }

            // Now, go through the forms and look for the subelements of packagedElements
            var rowForms = FormMethods.GetRowForms(result.Form).ToList();
            foreach (var rowForm in rowForms)
            {
                var foundFieldForPackagedElement = FormMethods.GetFieldForProperty(rowForm,
                    _CommonTypes._Default._Package.packagedElement);
                if (
                    foundFieldForPackagedElement?.metaclass?.equals(_Forms.TheOne.__SubElementFieldData) ==
                    true)
                {
                    var defaultTypesForNewElements = foundFieldForPackagedElement.get<IReflectiveCollection>(
                        _Forms._SubElementFieldData.defaultTypesForNewElements);
                    // We found it, so add the stuff

                    foreach (var foundExtentType in foundExtentTypes)
                    {
                        foreach (var rootMetaClass in foundExtentType.rootElementMetaClasses)
                        {
                            var resolvedMetaClass =
                                workspaceLogic.ResolveElement(rootMetaClass, ResolveType.OnlyMetaWorkspaces);

                            if (resolvedMetaClass == null)
                            {
                                continue;
                            }

                            changed = true;
                            defaultTypesForNewElements.add(resolvedMetaClass);

                            result.AddToFormCreationProtocol(
                                $"ExtentTypeFormsPlugin: Added {NamedElementMethods.GetName(resolvedMetaClass)} by ExtentType '{foundExtentType.name}' to PackagedElement");
                        }
                    }
                }
            }

            result.IsManaged = changed;
        }

        public void CreateObjectFormForMetaClass(IElement? metaClass, NewFormCreationContext context,
            FormCreationResult result)
        {
            throw new NotImplementedException();
        }
    }
}