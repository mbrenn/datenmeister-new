using Autofac;

namespace DatenMeister.DependencyInjection;

public class LifeTimeScopeEventArgs : EventArgs
{
    public LifeTimeScopeEventArgs(ILifetimeScope scope)
    {
        Scope = scope;
    }

    public ILifetimeScope Scope { get; set; }
}