using DatenMeister.Core;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Runtime.Workspaces;
using DatenMeister.Forms.CollectionForms;
using DatenMeister.Forms.Fields;
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
            context.Global.CollectionFormFactories.Add(new EmptyCollectionFormFactory());
            context.Global.CollectionFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        }

        context.Global.CollectionFormFactories.Add(new CollectionFormFromMetaClass());
        context.Global.CollectionFormFactories.Add(new CollectionFormFromData());
        context.Global.CollectionFormFactories.Add(new ValidateObjectOrCollectionForm());

        // Build up the ObjectForm Queue
        if (viewMode != ViewModes.AutoGenerate)
        {
            context.Global.ObjectFormFactories.Add(new EmptyObjectFormFactory());
            context.Global.ObjectFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        }

        context.Global.ObjectFormFactories.Add(new ObjectFormFromMetaClass());
        context.Global.ObjectFormFactories.Add(new ObjectFormFromData());
        context.Global.ObjectFormFactories.Add(new AddTableFormForPackagedElements());
        context.Global.ObjectFormFactories.Add(new ValidateObjectOrCollectionForm());
        
        // Build up the TableForm Queue
        if (viewMode != ViewModes.AutoGenerate)
        {
            context.Global.TableFormFactories.Add(new EmptyTableFormFactory());
            context.Global.TableFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        }

        context.Global.TableFormFactories.Add(new TableFormForMetaClass());
        context.Global.TableFormFactories.Add(new TableFormFromData());
        context.Global.TableFormFactories.Add(new ExpandDropDownOfValueReference());
        context.Global.TableFormFactories.Add(new AddDefaultTypeForMetaClassOfForm());
        context.Global.TableFormFactories.Add(new SortFieldsByImportantProperties());
        context.Global.TableFormFactories.Add(new RemoveDuplicateDefaultTypes());

        if (viewMode != ViewModes.AutoGenerate)
        {
            context.Global.RowFormFactories.Add(new EmptyRowFormFactory());
            context.Global.RowFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        }

        context.Global.RowFormFactories.Add(new RowFormFromData());
        context.Global.RowFormFactories.Add(new RowFormFromMetaClass());
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