#nullable enable

using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;
using DatenMeister.Core;
using DatenMeister.Core.Runtime.Workspaces;

namespace DatenMeister.DependencyInjection
{
    public class DatenMeisterScope : IDatenMeisterScope
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

        private readonly ILifetimeScope _lifetimeScopeImplementation;
        private IScopeStorage? _scopeStorage ;

        /// <summary>
        /// This event will be called before the items are actually disposed
        /// </summary>
        public event EventHandler? BeforeDisposing;

        public DatenMeisterScope(ILifetimeScope lifetimeScopeImplementation)
        {
            _lifetimeScopeImplementation = lifetimeScopeImplementation;
        }

        public object ResolveComponent(ResolveRequest request)
        {
            return _lifetimeScopeImplementation.ResolveComponent(request);
        }

        public IComponentRegistry ComponentRegistry => _lifetimeScopeImplementation.ComponentRegistry;

        public void Dispose()
        {
            BeforeDisposing?.Invoke(this, EventArgs.Empty);

            _lifetimeScopeImplementation.Dispose();
        }

        public ILifetimeScope BeginLifetimeScope() =>
            _lifetimeScopeImplementation.BeginLifetimeScope();

        public IDisposer Disposer => _lifetimeScopeImplementation.Disposer;

        public object Tag => _lifetimeScopeImplementation.Tag;

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
        {
            add => _lifetimeScopeImplementation.ChildLifetimeScopeBeginning += value;
            remove => _lifetimeScopeImplementation.ChildLifetimeScopeBeginning -= value;
        }

        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
        {
            add => _lifetimeScopeImplementation.CurrentScopeEnding += value;
            remove => _lifetimeScopeImplementation.CurrentScopeEnding -= value;
        }

        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
        {
            add => _lifetimeScopeImplementation.ResolveOperationBeginning += value;
            remove => _lifetimeScopeImplementation.ResolveOperationBeginning -= value;
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            return _lifetimeScopeImplementation.BeginLifetimeScope(tag, configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return _lifetimeScopeImplementation.BeginLifetimeScope(configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(object tag)
        {
            return _lifetimeScopeImplementation.BeginLifetimeScope(tag);
        }

        public ValueTask DisposeAsync()
        {
            return _lifetimeScopeImplementation.DisposeAsync();
        }
    }
}