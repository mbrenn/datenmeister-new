using System.Web;
using BurnSystems.Logging;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;
using DatenMeister.Core.Models;
using DatenMeister.Core.Models.EMOF;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Core.Uml.Helper;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.FormModifications;
using static DatenMeister.Core.Models._Forms;

namespace DatenMeister.Forms;

/// <summary>
/// This formfactory can be used to get the forms for specific elements, collections or extents
/// It will call the automatic form finder or the form creator, dependent on the configuration.
/// </summary>
public class FormFactory(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    : FormFactoryBase(workspaceLogic, scopeStorage), IFormFactory
{
    /// <summary>
    ///     Defines the logger
    /// </summary>
    private static readonly ILogger Logger = new ClassLogger(typeof(FormFactory));

    /// <summary>
    /// Creates a row form by the given metaclass 
    /// </summary>
    /// <param name="metaClass">Metaclass to be evaluated</param>
    /// <param name="configuration">Form configuration to be used</param>
    /// <returns></returns>
    public IElement? CreateRowFormByMetaClass(IElement metaClass, FormFactoryConfiguration? configuration)
    {
        using var _ = new StopWatchLogger(Logger, "Timing for CreateRowFormByMetaClass: ", LogLevel.Trace);
        // Ok, not an extent now do the right things
        IElement? rowForm = null;
        configuration ??= new FormFactoryConfiguration();

        if (configuration.ViaFormFinder)
        {
            var viewFinder = CreateFormFinder();
            rowForm = viewFinder.FindFormsFor(new FindFormQuery
            {
                metaClass = metaClass,
                FormType = ___FormType.Row,
                viewModeId = configuration.ViewModeId ?? ViewModes.Default
            }).FirstOrDefault();

            if (rowForm != null)
            {
                rowForm = FormMethods.CloneForm(rowForm);
                Logger.Info("CreateRowFormByMetaClass: Found form: " + NamedElementMethods.GetFullName(rowForm));
                FormMethods.AddToFormCreationProtocol(rowForm,
                    "[FormFactory.CreateRowFormByMetaClass] Found Form via FormFinder: " + rowForm.GetUri());
            }
        }

        if (rowForm == null && configuration.ViaFormCreator)
        {
            var formCreator = CreateFormCreator();
            rowForm = formCreator.CreateRowFormByMetaClass(
                metaClass,
                new FormFactoryConfiguration { IncludeOnlyCommonProperties = true, AllowFormModifications = false});

            FormMethods.AddToFormCreationProtocol(rowForm, "[FormFactory.CreateRowFormByMetaClass] Created Form via FormCreator");
        }

        if (rowForm != null)
        {
            // Adds the extension forms to the found extent
            AddExtensionFieldsToRowOrTableForm(
                rowForm,
                new FindFormQuery
                {
                    metaClass = metaClass,
                    FormType = ___FormType.RowExtension,
                    viewModeId = configuration.ViewModeId ?? ViewModes.Default
                });
                
            var formCreationContext = new FormCreationContext
            {
                FormType = ___FormType.Row,
                MetaClass = metaClass,
                IsReadOnly = configuration.IsReadOnly
            };

            CallPluginsForRowOrTableForm(configuration, formCreationContext, ref rowForm);
                
            CleanupRowForm(rowForm);
        }

        // No Form
        return rowForm;
    }

    /// <summary>
    /// Creates a row form for the specific item
    /// Here, the metaclass or the included properties are used
    /// </summary>
    /// <param name="element">Element to be evaluted</param>
    /// <param name="configuration">Configuration how this element shall be evaluated</param>
    /// <returns>The created element</returns>
    public IElement? CreateRowFormForItem(IObject element, FormFactoryConfiguration configuration)
    {
        using var _ = new StopWatchLogger(Logger, "Timing for CreateRowFormForItem: ", LogLevel.Trace);
        IElement? foundForm = null;
        var extent = (element as IHasExtent)?.Extent;

        if (element == null) throw new ArgumentNullException(nameof(element));

        if (configuration.ViaFormFinder)
        {
            // Tries to find the form
            var viewFinder = new FormFinder.FormFinder(FormMethods);
            foundForm = viewFinder.FindFormsFor(
                new FindFormQuery
                {
                    extentUri = extent?.GetUri() ?? string.Empty,
                    workspaceId = extent?.GetWorkspace()?.id ?? string.Empty,
                    metaClass = (element as IElement)?.getMetaClass(),
                    FormType = ___FormType.Row,
                    extentTypes = extent == null ? Array.Empty<string>() : extent.GetConfiguration().ExtentTypes,
                    viewModeId = configuration.ViewModeId
                }).FirstOrDefault();

            if (foundForm != null)
            {
                foundForm = FormMethods.CloneForm(foundForm);
                Logger.Info("CreateRowFormForItem: Found form: " + NamedElementMethods.GetFullName(foundForm));
                FormMethods.AddToFormCreationProtocol(foundForm,
                    "[FormFactory.CreateRowFormForItem] Found Form via FormFinder: " + foundForm.GetUri());
            }
        }

        if (foundForm == null && configuration.ViaFormCreator)
        {
            // Ok, we have not found the form. So create one
            var formCreator = CreateFormCreator();
            foundForm = formCreator.CreateRowFormForItem(element);
            FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateRowFormForItem] Created Form via FormCreator");
        }

        if (foundForm != null)
        {
            FormsState.CallFormsModificationPlugins(
                configuration,
                new FormCreationContext
                {
                    MetaClass = (element as IElement)?.getMetaClass(),
                    FormType = ___FormType.Row,
                    ExtentTypes = extent?.GetConfiguration().ExtentTypes ?? Array.Empty<string>(),
                    DetailElement = element,
                    IsReadOnly = configuration.IsReadOnly
                },
                ref foundForm);
                
            CleanupRowForm(foundForm);
        }

        return foundForm;
    }

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
            var formCreator = CreateFormCreator();
            foundForm = formCreator.CreateTableFormForCollection(collection,
                configuration with { AllowFormModifications = false });
            FormMethods.AddToFormCreationProtocol(foundForm,
                "[FormFactory.CreateTableFormForCollection] Created Form via FormCreator");
        }

        if (foundForm != null)
        {
            foundForm = FormMethods.CloneForm(foundForm);
                
            SetDefaultTypesByPackages(collection as IHasExtent, foundForm);

            FormsState.CallFormsModificationPlugins(
                configuration, new FormCreationContext
                {
                    FormType = ___FormType.Table,
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
                    FormType = ___FormType.Table,
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
            var formCreator = CreateFormCreator();
            foundForm = formCreator.CreateTableFormForMetaClass(metaClass, configuration);

            FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateTableFormForMetaClass] Created Form via FormCreator");
        }

        if (foundForm != null)
        {   
            FormsState.CallFormsModificationPlugins(
                configuration,
                new FormCreationContext
                {
                    MetaClass = metaClass,
                    FormType = ___FormType.Table,
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
                                  Array.Empty<string>(),
                    parentMetaClass = (parentElement as IElement)?.metaclass,
                    metaClass = propertyType,
                    FormType = ___FormType.Table,
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
            var formCreator = CreateFormCreator();

            // Checks, if an explicit property type is set: 
            if (propertyType == null)
            {
                foundForm = formCreator.CreateTableFormForCollection(
                    parentElement.get<IReflectiveCollection>(propertyName),
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true });
            }
            else
            {
                foundForm = formCreator.CreateTableFormForMetaClass(
                    propertyType,
                    new FormFactoryConfiguration { IncludeOnlyCommonProperties = true });

                foundForm.set(
                    _TableForm.title,
                    "Property: packagedElements of type " + NamedElementMethods.GetName(propertyType));
            }

            foundForm.set(_TableForm.property, propertyName);
            FormMethods.AddToFormCreationProtocol(foundForm, "[FormFactory.CreateTableFormForProperty] Found Form via FormCreator");
        }

        if (foundForm != null)
        {
            SetDefaultTypesByPackages(parentElement as IHasExtent, foundForm);

            FormsState.CallFormsModificationPlugins(configuration,
                new FormCreationContext
                {
                    FormType = ___FormType.Table,
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
    ///     Calls all the plugins for a possible row or table form
    /// </summary>
    /// <param name="configuration">Configuration of the formfactory</param>
    /// <param name="formCreationContext">Form Creation context to be used</param>
    /// <param name="foundForm">The form being found</param>
    /// <returns>Element being called</returns>
    private void CallPluginsForRowOrTableForm(
        FormFactoryConfiguration configuration,
        FormCreationContext formCreationContext,
        ref IElement foundForm)
    {
        FormsState.CallFormsModificationPlugins(
            configuration,
            formCreationContext,
            ref foundForm);
    }

    // Some helper method which creates the button to create new elements by the extent being connected
    // to the enumeration of elements
    void SetDefaultTypesByPackages(IHasExtent? hasExtent, IObject listForm)
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