using Autofac;
using DatenMeister.DependencyInjection;

namespace DatenMeister.WebServer.Library;

public interface IWebserverStartupPhases
{
    /// <summary>
    /// This event is called after the webserver is initialized. This point can be used to finalize the configuration
    /// </summary>
    event EventHandler<LifeTimeScopeEventArgs> AfterInitialization;

    void OnAfterInitialization(ILifetimeScope scope);
}