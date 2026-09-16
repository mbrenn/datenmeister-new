using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces.Workspace;
using DatenMeister.Forms.ViewModeConfigurators;
using DatenMeister.TemporaryExtent;

namespace DatenMeister.Forms;

public class FormCreationContextFactory(IWorkspaceLogic workspaceLogic, IScopeStorage scopeStorage)
{
    private readonly TemporaryExtentLogic _temporaryLogic = new(workspaceLogic, scopeStorage);
    private static readonly IReadOnlyList<IViewModeConfigurator> Configurators =
    [
        new AutoGenerateViewModeConfigurator(),
        new ByPropertiesViewModeConfigurator(),
        new DefaultViewModeConfigurator() // Fallback
    ];

    public IFactory? MofFactory { get; set; }

    public FormsState State { get; set; } = scopeStorage.Get<FormsState>();

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
                Factory = MofFactory ?? new TemporaryExtentFactory(_temporaryLogic),
                FactoryForForms = MofFactory ?? new TemporaryExtentFactory(_temporaryLogic, true)
            }
        };

        var configurator = Configurators.FirstOrDefault(c => c.CanHandle(viewMode))
            ?? Configurators.Last();

        configurator.Configure(context, workspaceLogic);

        // Now go through the available Form Modification Plugins
        foreach (var plugin in State.FormModificationPlugins)
        {
            plugin.CreateContext(context);
        }

        return context;
    }
}