using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Forms.CollectionForms;
using DatenMeister.Forms.Fields;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.ObjectForm;
using DatenMeister.Forms.RowForm;
using DatenMeister.Forms.TableForms;

namespace DatenMeister.Forms.ViewModeConfigurators;

/// <summary>
/// Configures form factories for the default view mode, registering all standard stages
/// including preparation, form finder, metaclass, data fallback, and enhancement/cleanup factories.
/// </summary>
public class DefaultViewModeConfigurator : IViewModeConfigurator
{
    /// <inheritdoc />
    public virtual bool CanHandle(string viewMode) =>
        string.IsNullOrEmpty(viewMode) || viewMode == ViewModes.Default;

    /// <inheritdoc />
    public virtual void Configure(FormCreationContext context, IWorkspaceLogic workspaceLogic)
    {
        AddPreparationFactories(context);
        AddFormFinderFactories(context, workspaceLogic);
        AddMetaClassFactories(context, workspaceLogic);
        AddDataFallbackFactories(context, workspaceLogic);
        AddEnhancementAndCleanupFactories(context);
    }

    /// <summary>
    /// Adds empty preparation factories to initialize form creation.
    /// </summary>
    /// <param name="context">The form creation context.</param>
    protected virtual void AddPreparationFactories(FormCreationContext context)
    {
        context.Global.CollectionFormFactories.Add(new EmptyCollectionFormFactory
            { Priority = FormFactoryPriorities.Preparation });
        context.Global.ObjectFormFactories.Add(new EmptyObjectFormFactory
            { Priority = FormFactoryPriorities.Preparation });
        context.Global.TableFormFactories.Add(new EmptyTableFormFactory
            { Priority = FormFactoryPriorities.Preparation });
        context.Global.RowFormFactories.Add(new EmptyRowFormFactory
            { Priority = FormFactoryPriorities.Preparation });
    }

    /// <summary>
    /// Adds form finder factories that discover predefined forms from workspaces.
    /// </summary>
    /// <param name="context">The form creation context.</param>
    /// <param name="workspaceLogic">The workspace logic.</param>
    protected virtual void AddFormFinderFactories(FormCreationContext context, IWorkspaceLogic workspaceLogic)
    {
        context.Global.CollectionFormFactories.Add(new FormFinderFactory(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp });
        context.Global.ObjectFormFactories.Add(new FormFinderFactory(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp });
        context.Global.TableFormFactories.Add(new FormFinderFactory(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp });
        context.Global.RowFormFactories.Add(new FormFinderFactory(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp });
    }

    /// <summary>
    /// Adds factories that create forms and fields based on metaclass definitions.
    /// </summary>
    /// <param name="context">The form creation context.</param>
    /// <param name="workspaceLogic">The workspace logic.</param>
    protected virtual void AddMetaClassFactories(FormCreationContext context, IWorkspaceLogic workspaceLogic)
    {
        context.Global.CollectionFormFactories.Add(new CollectionFormFromMetaClass
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.ObjectFormFactories.Add(new ObjectFormFromMetaClass(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.TableFormFactories.Add(new TableFormForMetaClass(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.RowFormFactories.Add(new RowFormFromMetaClass(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.FieldFormFactories.Add(new FieldFromPropertyType(workspaceLogic));
    }

    /// <summary>
    /// Adds fallback factories that infer forms and fields directly from runtime data.
    /// </summary>
    /// <param name="context">The form creation context.</param>
    /// <param name="workspaceLogic">The workspace logic.</param>
    protected virtual void AddDataFallbackFactories(FormCreationContext context, IWorkspaceLogic workspaceLogic)
    {
        context.Global.CollectionFormFactories.Add(new CollectionFormFromData
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.ObjectFormFactories.Add(new ObjectFormFromData
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.TableFormFactories.Add(new TableFormFromData(workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.RowFormFactories.Add(new RowFormFromData()
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.FieldFormFactories.Add(new FieldFromPropertyValue());
        context.Global.FieldFormFactories.Add(new FieldAnyFieldData());
    }

    /// <summary>
    /// Adds post-processing, validation, and cleanup factories.
    /// </summary>
    /// <param name="context">The form creation context.</param>
    protected virtual void AddEnhancementAndCleanupFactories(FormCreationContext context)
    {
        context.Global.CollectionFormFactories.Add(new RemovePropertyFromCollectionFormTabs
            { Priority = FormFactoryPriorities.CleanUp });
        context.Global.CollectionFormFactories.Add(new ValidateObjectOrCollectionForm
            { Priority = FormFactoryPriorities.Miscellaneous });

        context.Global.ObjectFormFactories.Add(new AddTableFormForPackagedElements());
        context.Global.ObjectFormFactories.Add(new ValidateObjectOrCollectionForm
            { Priority = FormFactoryPriorities.Miscellaneous });
        context.Global.ObjectFormFactories.Add(new AddEmptyRowFormInCaseItDoesNotExist
            { Priority = FormFactoryPriorities.CleanUp });

        context.Global.TableFormFactories.Add(new ExpandDropDownOfValueReference());
        context.Global.TableFormFactories.Add(new AddDefaultTypeForMetaClassOfForm());
        context.Global.TableFormFactories.Add(new SortFieldsByImportantProperties());
        context.Global.TableFormFactories.Add(new RemoveDuplicateDefaultTypes
            { Priority = FormFactoryPriorities.Miscellaneous });

        context.Global.RowFormFactories.Add(new AddMetaClassField());
        context.Global.RowFormFactories.Add(new ExpandDropDownOfValueReference());
    }
}
