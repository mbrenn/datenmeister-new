using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Forms.ViewModeConfigurators;

/// <summary>
/// Configures form factories for the <see cref="ViewModes.AutoGenerate"/> view mode,
/// bypassing predefined form discovery to enforce automatic form creation from metaclasses and data.
/// </summary>
public class AutoGenerateViewModeConfigurator : DefaultViewModeConfigurator
{
    /// <inheritdoc />
    public override bool CanHandle(string viewMode) => viewMode == ViewModes.AutoGenerate;

    /// <inheritdoc />
    public override void Configure(FormCreationContext context, IWorkspaceLogic workspaceLogic)
    {
        AddPreparationFactories(context);
        // Skip FormFinderFactories to enforce auto-generation
        AddMetaClassFactories(context, workspaceLogic);
        AddDataFallbackFactories(context, workspaceLogic);
        AddEnhancementAndCleanupFactories(context);
    }
}
