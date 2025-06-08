using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.FormModifications;

namespace DatenMeister.Forms.FormFactory;

public class TableFormFactory : FormFactoryBase, ITableFormFactory
{
    public TableFormFactory(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    :base(workspaceLogic, scopeStorage)
    {
            
    }
    
    /// <summary>
    ///     Defines the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(TableFormFactory));


    /// <summary>
    /// Evaluates the given collection and creates a table form out of it.
    /// This is a quite slow routine, but at least, it works
    /// </summary>
    /// <param name="collection">Collection to be evaluated</param>
    /// <param name="configuration">Configuration of the form</param>
    /// <returns>The created form</returns>
    public IElement? CreateTableFormForCollection(
        IReflectiveCollection collection,
        FormFactoryConfiguration configuration)
    {
        using var _ = new StopWatchLogger(Logger, "Timing for CreateTableFormForCollection: ", LogLevel.Trace);
        configuration = configuration with { IsForTableForm = true };
        IElement? foundForm = null;
        if (configuration.ViaFormCreator)
        {
            // Ok, now perform the creation...
            var formCreator = new TableFormFactory(WorkspaceLogic, ScopeStorage);
            foundForm = formCreator.CreateTableFormForCollection(collection,
                configuration with { AllowFormModifications = false });
            FormMethods.AddToFormCreationProtocol(foundForm,
                "[FormFactory.CreateTableFormForCollection] Created Form via FormCreator");
        }

        if (foundForm != null)
        {
            foundForm = FormMethods.CloneForm(foundForm);
                
            AddDefaultTypesForTableFormByExtentInformation(collection as IHasExtent, foundForm);

            FormsState.CallFormsModificationPlugins(
                configuration, new FormCreationContext
                {
                    FormType = _Forms.___FormType.Table,
                    ViewMode = configuration.ViewModeId,
                    IsReadOnly = configuration.IsReadOnly
                },
                ref foundForm);

            FormMethods.CleanupTableForm(foundForm);
        }

        return foundForm;
    }

    public IElement? CreateTableFormForMetaClass(
        IElement? metaClass,
        FormFactoryConfiguration configuration)
    {
        using var _ = new StopWatchLogger(Logger, "Timing for CreateTableFormForMetaClass: ", LogLevel.Trace);
        configuration = configuration with { IsForTableForm = true };
        IElement? foundForm = null;

        if (configuration.ViaFormFinder)
        {
            // Tries to find the form
            var viewFinder = new FormFinder.FormFinder(FormMethods);
            foundForm = viewFinder.FindFormsFor(
                new FindFormQuery
                {
                    metaClass = metaClass,
                    FormType = _Forms.___FormType.Table,
                    viewModeId = configuration.ViewModeId
                }).FirstOrDefault();

            if (foundForm != null)
            {
                foundForm = FormMethods.CloneForm(foundForm);
                Logger.Info("CreateTableFormForMetaClass: Found form: " +
                            NamedElementMethods.GetFullName(foundForm));

                FormMethods.AddToFormCreationProtocol(foundForm,
                    "[FormFactory.CreateTableFormForMetaClass] Found Form via FormFinder" + foundForm.GetUri());
            }
        }

        if (foundForm == null && configuration.ViaFormCreator)
        {
            // Ok, we have not found the form. So create one
            var formCreator = new TableFormFactory(WorkspaceLogic, ScopeStorage);
            foundForm = formCreator.CreateTableFormForMetaClass(metaClass, configuration);

            FormMethods.AddToFormCreationProtocol(
                foundForm,
                "[FormFactory.CreateTableFormForMetaClass] Created Form via FormCreator");
        }

        if (foundForm != null)
        {   
            FormsState.CallFormsModificationPlugins(
                configuration,
                new FormCreationContext
                {
                    MetaClass = metaClass,
                    FormType = _Forms.___FormType.Table,
                    IsReadOnly = configuration.IsReadOnly
                },
                ref foundForm);

            FormMethods.CleanupTableForm(foundForm);
        }

        return foundForm;
    }

    /// <summary>
    /// Creates a list form for a certain property which contains a collection
    /// </summary>
    /// <param name="parentElement">The element to which the property belows</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="propertyType">The type of the property</param>
    /// <param name="configuration">The configuration being used to creates the list form</param>
    /// <returns>The created listform</returns>
    public IElement? CreateTableFormForProperty(
        IObject? parentElement,
        string propertyName,
        IElement? propertyType,
        FormFactoryConfiguration configuration)
    {
        using var _ = new StopWatchLogger(Logger, "Timing for CreateTableFormForProperty: ", LogLevel.Trace);
        configuration = configuration with { IsForTableForm = true };
        IElement? foundForm = null;
        propertyType ??=
            ClassifierMethods.GetPropertyTypeOfValuesProperty(parentElement as IElement, propertyName);

        if (configuration.ViaFormFinder)
        {
            var viewFinder = new FormFinder.FormFinder(FormMethods);
            foundForm = viewFinder.FindFormsFor(
                new FindFormQuery
                {
                    extentTypes = (parentElement as IHasExtent)?.Extent?.GetConfiguration().ExtentTypes ??
                                  [],
                    parentMetaClass = (parentElement as IElement)?.metaclass,
                    metaClass = propertyType,
                    FormType = _Forms.___FormType.Table,
                    parentProperty = propertyName
                }).FirstOrDefault();

            if (foundForm != null)
            {
                foundForm = FormMethods.CloneForm(foundForm);
                Logger.Info("CreateTableFormForProperty: Found form: " +
                            NamedElementMethods.GetFullName(foundForm));
                FormMethods.AddToFormCreationProtocol(foundForm,
                    "[FormFactory.CreateTableFormForProperty] Found Form via FormFinder: " + foundForm.GetUri());
            }
        }

        if (foundForm == null && configuration.ViaFormCreator)
        {
            var formCreator = new TableFormFactory(WorkspaceLogic, ScopeStorage);

            // Checks, if an explicit property type is set: 
            if (propertyType == null)
            {
                foundForm = formCreator.CreateTableFormForCollection(
                                parentElement.get<IReflectiveCollection>(propertyName),
                                new FormFactoryConfiguration { IncludeOnlyCommonProperties = true })
                            ?? throw new InvalidOperationException("foundForm == null");
            }
            else
            {
                foundForm = formCreator.CreateTableFormForMetaClass(
                    propertyType,
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true });

                foundForm?.set(
                    _Forms._TableForm.title,
                    "Property: packagedElements of type " + NamedElementMethods.GetName(propertyType));
            }

            foundForm?.set(_Forms._TableForm.property, propertyName);
            FormMethods.AddToFormCreationProtocol(
                foundForm,
                "[FormFactory.CreateTableFormForProperty] Found Form via FormCreator");
        }

        if (foundForm != null)
        {
            AddDefaultTypesForTableFormByExtentInformation(parentElement as IHasExtent, foundForm);

            FormsState.CallFormsModificationPlugins(
                configuration,
                new FormCreationContext
                {
                    FormType = _Forms.___FormType.Table,
                    MetaClass = (parentElement as IElement)?.metaclass,
                    ParentPropertyName = propertyName,
                    DetailElement = parentElement,
                    IsReadOnly = configuration.IsReadOnly
                },
                ref foundForm);

            FormMethods.CleanupTableForm(foundForm);
        }

        return foundForm;
    }
    

    /// <summary>
    /// Some helper method which creates the button to create new elements by the extent being connected
    /// to the enumeration of elements 
    /// </summary>
    /// <param name="hasExtent">The extent containing the default types</param>
    /// <param name="listForm">the table form to which the elements will be added</param>
    void AddDefaultTypesForTableFormByExtentInformation(IHasExtent? hasExtent, IObject listForm)
    {
        if (hasExtent is null) return;

        var extent = hasExtent.Extent;
        var defaultTypes = extent?.GetConfiguration().GetDefaultTypes();
        if (defaultTypes != null)
        {
            // Now go through the packages and pick the classifier and add them to the list
            foreach (var package in defaultTypes)
            {
                var childItems =
                    package.getOrDefault<IReflectiveCollection>(_UML._Packages._Package.packagedElement);
                if (childItems == null) continue;

                foreach (var type in childItems.OfType<IElement>())
                {
                    if (type.@equals(_UML.TheOne.StructuredClassifiers.__Class))
                    {
                        FormMethods.AddDefaultTypeForNewElement(listForm, package);

                        FormMethods.AddToFormCreationProtocol(
                            listForm,
                            "[FormCreator.SetDefaultTypesByPackages]: Add DefaultTypeForNewElement driven by ExtentType: " +
                            NamedElementMethods.GetName(package));
                    }
                }
            }
        }
    }
}