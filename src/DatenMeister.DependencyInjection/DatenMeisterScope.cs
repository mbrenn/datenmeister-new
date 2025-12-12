using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.Workspace;

namespace DatenMeister.DependencyInjection;

public class DatenMeisterScope(ILifetimeScope lifetimeScopeImplementation) : IDatenMeisterScope
{
    /// <summary>
    /// Gets the workspace logic
    /// </summary>
    public IWorkspaceLogic WorkspaceLogic => this.Resolve<IWorkspaceLogic>();

    /// <summary>
    /// Gets the scope storage being used to store data throughout the running application. 
    /// </summary>
    public IScopeStorage ScopeStorage
    {
        get => _scopeStorage ?? throw new InvalidOperationException("ScopeStorage is null");
        set => _scopeStorage  = value;
    }

    private IScopeStorage? _scopeStorage ;

    /// <summary>
    /// This event will be called before the items are actually disposed
    /// </summary>
    public event EventHandler? BeforeDisposing;

    public object ResolveComponent(ResolveRequest request)
    {
        return lifetimeScopeImplementation.ResolveComponent(request);
    }

    public IComponentRegistry ComponentRegistry => lifetimeScopeImplementation.ComponentRegistry;

    public void Dispose()
    {
        BeforeDisposing?.Invoke(this, EventArgs.Empty);

        lifetimeScopeImplementation.Dispose();
    }

    public ILifetimeScope BeginLifetimeScope() =>
        lifetimeScopeImplementation.BeginLifetimeScope();

    public IDisposer Disposer => lifetimeScopeImplementation.Disposer;

    public object Tag => lifetimeScopeImplementation.Tag;

    public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
    {
        add => lifetimeScopeImplementation.ChildLifetimeScopeBeginning += value;
        remove => lifetimeScopeImplementation.ChildLifetimeScopeBeginning -= value;
    }

    public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
    {
        add => lifetimeScopeImplementation.CurrentScopeEnding += value;
        remove => lifetimeScopeImplementation.CurrentScopeEnding -= value;
    }

    public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
    {
        add => lifetimeScopeImplementation.ResolveOperationBeginning += value;
        remove => lifetimeScopeImplementation.ResolveOperationBeginning -= value;
    }

    public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
    {
        return lifetimeScopeImplementation.BeginLifetimeScope(tag, configurationAction);
    }

    public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
    {
        return lifetimeScopeImplementation.BeginLifetimeScope(configurationAction);
    }

    public ILifetimeScope BeginLifetimeScope(object tag)
    {
        return lifetimeScopeImplementation.BeginLifetimeScope(tag);
    }

    public ValueTask DisposeAsync()
    {
        return lifetimeScopeImplementation.DisposeAsync();
    }
}