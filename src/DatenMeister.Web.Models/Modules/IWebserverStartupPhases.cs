using System;
using Autofac;

namespace DatenMeister.Web.Models.Modules
{
    public interface IWebserverStartupPhases
    {
        /// <summary>
        /// This event is called after the webserver is initialized. This point can be used to finalize the configuration
        /// </summary>
        event EventHandler<LifeTimeScopeEventArgs> AfterInitialization;

        void OnAfterInitialization(ILifetimeScope scope);
    }
}