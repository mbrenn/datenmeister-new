using System;
using Autofac;

namespace DatenMeister.Modules
{
    public class WebserverStartupPhases : IWebserverStartupPhases
    {
        public void OnAfterInitialization(ILifetimeScope scope)
        {
            var e = AfterInitialization;
            e?.Invoke(this, new LifeTimeScopeEventArgs(scope));
        }

        public event EventHandler<LifeTimeScopeEventArgs> AfterInitialization;
    }
}