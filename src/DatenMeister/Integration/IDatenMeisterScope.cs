#nullable enable

using System;
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

        /// <summary>
        /// This event is called before the element is being disposed
        /// </summary>
        event EventHandler BeforeDisposing;
    }
}