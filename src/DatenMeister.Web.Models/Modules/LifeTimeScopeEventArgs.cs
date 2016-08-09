﻿using System;
using Autofac;

namespace DatenMeister.Web.Models.Modules
{
    public class LifeTimeScopeEventArgs : EventArgs
    {
        public LifeTimeScopeEventArgs(ILifetimeScope scope)
        {
            Scope = scope;
        }

        public ILifetimeScope Scope { get; set; }
    }
}