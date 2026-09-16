using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Forms.ViewModeConfigurators;

/// <summary>
/// Configures form factories for the <see cref="ViewModes.ByProperties"/> view mode,
/// skipping form discovery and metaclass mapping to generate forms purely from runtime properties.
/// </summary>
public class ByPropertiesViewModeConfigurator : DefaultViewModeConfigurator
{
    /// <inheritdoc />
    public override bool CanHandle(string viewMode) => viewMode == ViewModes.ByProperties;

    /// <inheritdoc />
    public override void Configure(FormCreationContext context, IWorkspaceLogic workspaceLogic)
    {
        AddPreparationFactories(context);
        // Skip FormFinderFactories and MetaClassFactories to enforce pure property generation
        AddDataFallbackFactories(context, workspaceLogic);
        AddEnhancementAndCleanupFactories(context);
    }
}
