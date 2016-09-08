using Autofac;

namespace DatenMeister.Integration
{
    public interface IIntegrationHooks
    {
        void OnStartScope(ILifetimeScope scope);
        void BeforeLoadExtents(ILifetimeScope scope);
    }
}