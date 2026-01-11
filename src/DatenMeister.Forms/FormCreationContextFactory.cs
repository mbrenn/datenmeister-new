using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Forms.CollectionForms;
using DatenMeister.Forms.Fields;
using DatenMeister.Forms.FormFactory;
using DatenMeister.Forms.FormFinder;
using DatenMeister.Forms.ObjectForm;
using DatenMeister.Forms.RowForm;
using DatenMeister.Forms.TableForms;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Forms;

public class FormCreationContextFactory
{
    private readonly IWorkspaceLogic _workspaceLogic;
    private readonly IScopeStorage _scopeStorage;
    private readonly TemporaryExtentFactory _temporaryExtentFactory;


    public IFactory? MofFactory { get; set; }

    public FormCreationContextFactory(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
    {
        _workspaceLogic = workspaceLogic;
        _scopeStorage = scopeStorage;
        State = scopeStorage.Get<FormsState>();

        var temporaryLogic = new TemporaryExtentLogic(workspaceLogic, scopeStorage);
        _temporaryExtentFactory = new TemporaryExtentFactory(temporaryLogic);
    }

    public FormsState State { get; set; }

    /// <summary>
    /// Creates a new form creation
    /// </summary>
    /// <returns>The created form</returns>
    public FormCreationContext Create(string viewMode)
    {
        var context = new FormCreationContext
        {
            ViewModeId = viewMode,
            Global = new FormCreationContext.GlobalContext
            {
                Factory = MofFactory ?? _temporaryExtentFactory
            }
        };

        // Build up the CollectionForm Queue
        if (viewMode != ViewModes.AutoGenerate)
        {
            context.Global.CollectionFormFactories.Add(new EmptyCollectionFormFactory
                { Priority = FormFactoryPriorities.Preparation });
            context.Global.CollectionFormFactories.Add(new FormFinderFactory(_workspaceLogic)
                { Priority = FormFactoryPriorities.PrimaryBuildUp });
        }

        context.Global.CollectionFormFactories.Add(new CollectionFormFromMetaClass
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.CollectionFormFactories.Add(new CollectionFormFromData
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.CollectionFormFactories.Add(new RemovePropertyFromCollectionFormTabs
            { Priority = FormFactoryPriorities.CleanUp });
        context.Global.CollectionFormFactories.Add(new ValidateObjectOrCollectionForm
            { Priority = FormFactoryPriorities.Miscellaneous });

        // Build up the ObjectForm Queue
        if (viewMode != ViewModes.AutoGenerate)
        {
            context.Global.ObjectFormFactories.Add(new EmptyObjectFormFactory
                { Priority = FormFactoryPriorities.Preparation });
            context.Global.ObjectFormFactories.Add(new FormFinderFactory(_workspaceLogic)
                { Priority = FormFactoryPriorities.PrimaryBuildUp });
        }

        context.Global.ObjectFormFactories.Add(new ObjectFormFromMetaClass(_workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.ObjectFormFactories.Add(new ObjectFormFromData
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.ObjectFormFactories.Add(new AddTableFormForPackagedElements());
        context.Global.ObjectFormFactories.Add(new ValidateObjectOrCollectionForm
            { Priority = FormFactoryPriorities.Miscellaneous });
        context.Global.ObjectFormFactories.Add(new AddEmptyRowFormInCaseItDoesNotExist
            { Priority = FormFactoryPriorities.CleanUp });

        // Build up the TableForm Queue
        if (viewMode != ViewModes.AutoGenerate)
        {
            context.Global.TableFormFactories.Add(new EmptyTableFormFactory
                { Priority = FormFactoryPriorities.Preparation });
            context.Global.TableFormFactories.Add(new FormFinderFactory(_workspaceLogic)
                { Priority = FormFactoryPriorities.PrimaryBuildUp });
        }

        context.Global.TableFormFactories.Add(new TableFormForMetaClass(_workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.TableFormFactories.Add(new TableFormFromData(_workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.TableFormFactories.Add(new ExpandDropDownOfValueReference());
        context.Global.TableFormFactories.Add(new AddDefaultTypeForMetaClassOfForm());
        context.Global.TableFormFactories.Add(new SortFieldsByImportantProperties());
        context.Global.TableFormFactories.Add(new RemoveDuplicateDefaultTypes
            { Priority = FormFactoryPriorities.Miscellaneous });

        // Build up the RowForm Queue
        if (viewMode != ViewModes.AutoGenerate)
        {
            context.Global.RowFormFactories.Add(new EmptyRowFormFactory
                { Priority = FormFactoryPriorities.Preparation });
            context.Global.RowFormFactories.Add(new FormFinderFactory(_workspaceLogic)
                { Priority = FormFactoryPriorities.PrimaryBuildUp });
        }

        context.Global.RowFormFactories.Add(new RowFormFromData(_workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 1 });
        context.Global.RowFormFactories.Add(new RowFormFromMetaClass(_workspaceLogic)
            { Priority = FormFactoryPriorities.PrimaryBuildUp - 2 });
        context.Global.RowFormFactories.Add(new AddMetaClassField());
        context.Global.RowFormFactories.Add(new ExpandDropDownOfValueReference());

        // Build up the FieldForm Queue
        context.Global.FieldFormFactories.Add(new FieldFromPropertyType(_workspaceLogic));
        context.Global.FieldFormFactories.Add(new FieldFromPropertyValue());
        context.Global.FieldFormFactories.Add(new FieldAnyFieldData());

        // Now go through the available Form Modification Plugins
        foreach (var plugin in State.FormModificationPlugins)
        {
            plugin.CreateContext(context);
        }

        return context;
    }
}