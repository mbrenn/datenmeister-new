using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;

namespace DatenMeister.Integration
{
    public class DatenMeisterScope : IDatenMeisterScope
    {
        private ILifetimeScope _lifetimeScopeImplementation;

        public DatenMeisterScope(ILifetimeScope lifetimeScopeImplementation)
        {
            _lifetimeScopeImplementation = lifetimeScopeImplementation;
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            return _lifetimeScopeImplementation.ResolveComponent(registration, parameters);
        }

        public IComponentRegistry ComponentRegistry
        {
            get { return _lifetimeScopeImplementation.ComponentRegistry; }
        }

        public void Dispose()
        {
            _lifetimeScopeImplementation.Dispose();
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _lifetimeScopeImplementation.BeginLifetimeScope();
        }

        public IDisposer Disposer
        {
            get { return _lifetimeScopeImplementation.Disposer; }
        }

        public object Tag
        {
            get { return _lifetimeScopeImplementation.Tag; }
        }

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
        {
            add { _lifetimeScopeImplementation.ChildLifetimeScopeBeginning += value; }
            remove { _lifetimeScopeImplementation.ChildLifetimeScopeBeginning -= value; }
        }

        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
        {
            add { _lifetimeScopeImplementation.CurrentScopeEnding += value; }
            remove { _lifetimeScopeImplementation.CurrentScopeEnding -= value; }
        }

        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
        {
            add { _lifetimeScopeImplementation.ResolveOperationBeginning += value; }
            remove { _lifetimeScopeImplementation.ResolveOperationBeginning -= value; }
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
    }
}