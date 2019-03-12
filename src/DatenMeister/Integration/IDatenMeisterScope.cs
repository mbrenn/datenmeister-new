using Autofac;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Integration
{
    public interface IDatenMeisterScope : ILifetimeScope
    {
        /// <summary>
        /// Gets the logic for the workspaces
        /// </summary>
        IWorkspaceLogic WorkspaceLogic { get; }
    }
}