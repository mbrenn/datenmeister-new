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

    /// <summary>
    /// Gets or sets the view mode being used
    /// </summary>
    public string ViewMode { get; set; } = string.Empty;

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
    public FormCreationContext Create(string viewMode = "")
    {
        var context = new FormCreationContext
        {
            ViewModeId = ViewMode,
            Global = new FormCreationContext.GlobalContext
            {
                Factory = MofFactory ?? _temporaryExtentFactory
            }
        };

        // Build up the CollectionForm Queue
        context.Global.CollectionFormFactories.Add(new EmptyCollectionFormFactory());
        context.Global.CollectionFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        context.Global.CollectionFormFactories.Add(new CollectionFormFromMetaClass());
        context.Global.CollectionFormFactories.Add(new CollectionFormFromData());
        context.Global.CollectionFormFactories.Add(new ValidateObjectOrCollectionForm());

        // Build up the ObjectForm Queue
        context.Global.ObjectFormFactories.Add(new EmptyObjectFormFactory());
        context.Global.ObjectFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        context.Global.ObjectFormFactories.Add(new ObjectFormFromMetaClass());
        context.Global.ObjectFormFactories.Add(new ObjectFormFromData());
        context.Global.ObjectFormFactories.Add(new AddTableFormForPackagedElements());
        context.Global.ObjectFormFactories.Add(new ValidateObjectOrCollectionForm());
        
        // Build up the TableForm Queue
        context.Global.TableFormFactories.Add(new EmptyTableFormFactory());
        context.Global.TableFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        context.Global.TableFormFactories.Add(new TableFormForMetaClass());
        context.Global.TableFormFactories.Add(new TableFormFromData());
        context.Global.TableFormFactories.Add(new ExpandDropDownOfValueReference());
        context.Global.TableFormFactories.Add(new AddDefaultTypeForMetaClassOfForm());
        context.Global.TableFormFactories.Add(new SortFieldsByImportantProperties());
        context.Global.TableFormFactories.Add(new RemoveDuplicateDefaultTypes());
        
        context.Global.RowFormFactories.Add(new EmptyRowFormFactory());
        context.Global.RowFormFactories.Add(new FormFinderFactory(_workspaceLogic));
        context.Global.RowFormFactories.Add(new ExpandDropDownOfValueReference());
        context.Global.RowFormFactories.Add(new RowFormFromData());
        
        // Build up the FieldForm Queue
        context.Global.FieldFormFactories.Add(new FieldFromData(_workspaceLogic));
        
        // Now go through the available Form Modification Plugins
        foreach (var plugin in State.NewFormModificationPlugins)
        {
            plugin.CreateContext(context);
        }
        
        return context;
    }
}