using System;
using Autofac;

namespace DatenMeister.Web.Models.Modules
{
    public class WebserverStartupPhases : IWebserverStartupPhases
    {
        public void OnAfterInitialization(ILifetimeScope scope)
        {
            var e = AfterInitialization;
            if (e != null)
            {
                AfterInitialization(this, new LifeTimeScopeEventArgs(scope));
            }
        }

        public event EventHandler<LifeTimeScopeEventArgs> AfterInitialization;
    }
}