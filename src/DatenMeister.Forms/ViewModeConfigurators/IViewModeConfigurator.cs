using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.Forms.ViewModeConfigurators;

/// <summary>
/// Defines the strategy for configuring form factory pipelines within a <see cref="FormCreationContext"/>
/// based on a specified view mode.
/// </summary>
public interface IViewModeConfigurator
{
    /// <summary>
    /// Determines whether this configurator can handle the specified view mode.
    /// </summary>
    /// <param name="viewMode">The identifier of the view mode.</param>
    /// <returns><c>true</c> if this configurator handles the view mode; otherwise, <c>false</c>.</returns>
    bool CanHandle(string viewMode);

    /// <summary>
    /// Configures the form factory queues and settings on the given creation context.
    /// </summary>
    /// <param name="context">The form creation context to configure.</param>
    /// <param name="workspaceLogic">The workspace logic used for form and type resolution.</param>
    void Configure(FormCreationContext context, IWorkspaceLogic workspaceLogic);
}
