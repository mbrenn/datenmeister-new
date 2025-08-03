using Autofac;

namespace DatenMeister.DependencyInjection;

public class LifeTimeScopeEventArgs(ILifetimeScope scope) : EventArgs
{
    public ILifetimeScope Scope { get; set; } = scope;
}