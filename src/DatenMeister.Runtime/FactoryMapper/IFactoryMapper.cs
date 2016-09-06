﻿using System;
using Autofac;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.FactoryMapper
{
    public interface IFactoryMapper
    {
        IFactory FindFactoryFor(ILifetimeScope scope, Type extentType);
    }
}