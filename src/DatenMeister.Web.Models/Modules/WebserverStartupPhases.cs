using System;
using Autofac;

namespace DatenMeister.Web.Models.Modules
{
    public class WebserverStartupPhases : IWebserverStartupPhases
    {
        public void OnAfterInitialization(ILifetimeScope scope)
        {
            AfterInitialization(this, new LifeTimeScopeEventArgs(scope));
        }

        public event EventHandler<LifeTimeScopeEventArgs> AfterInitialization;
    }
}