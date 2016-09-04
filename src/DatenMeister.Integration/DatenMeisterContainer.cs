using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Autofac.Core.Lifetime;
using Autofac.Core.Resolving;

namespace DatenMeister.Integration
{
    public class DatenMeisterContainer : IContainer
    {
        private readonly IContainer _containerImplementation;

        public DatenMeisterContainer(IContainer containerImplementation)
        {
            _containerImplementation = containerImplementation;
        }

        public object ResolveComponent(IComponentRegistration registration, IEnumerable<Parameter> parameters)
        {
            return _containerImplementation.ResolveComponent(registration, parameters);
        }

        public IComponentRegistry ComponentRegistry => _containerImplementation.ComponentRegistry;

        public void Dispose()
        {
            _containerImplementation.Dispose();
        }

        public ILifetimeScope BeginLifetimeScope()
        {
            return _containerImplementation.BeginLifetimeScope();
        }

        public ILifetimeScope BeginLifetimeScope(object tag)
        {
            return _containerImplementation.BeginLifetimeScope(tag);
        }

        public ILifetimeScope BeginLifetimeScope(Action<ContainerBuilder> configurationAction)
        {
            return _containerImplementation.BeginLifetimeScope(configurationAction);
        }

        public ILifetimeScope BeginLifetimeScope(object tag, Action<ContainerBuilder> configurationAction)
        {
            return _containerImplementation.BeginLifetimeScope(tag, configurationAction);
        }

        public IDisposer Disposer => _containerImplementation.Disposer;

        public object Tag => _containerImplementation.Tag;

        public event EventHandler<LifetimeScopeBeginningEventArgs> ChildLifetimeScopeBeginning
        {
            add { _containerImplementation.ChildLifetimeScopeBeginning += value; }
            remove { _containerImplementation.ChildLifetimeScopeBeginning -= value; }
        }

        public event EventHandler<LifetimeScopeEndingEventArgs> CurrentScopeEnding
        {
            add { _containerImplementation.CurrentScopeEnding += value; }
            remove { _containerImplementation.CurrentScopeEnding -= value; }
        }

        public event EventHandler<ResolveOperationBeginningEventArgs> ResolveOperationBeginning
        {
            add { _containerImplementation.ResolveOperationBeginning += value; }
            remove { _containerImplementation.ResolveOperationBeginning -= value; }
        }
    }
}