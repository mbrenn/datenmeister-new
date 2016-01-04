﻿using System;
using System.Reflection;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.FactoryMapper
{
    public static class Extensions
    {
        public static IFactory FindFactoryFor(this IFactoryMapper mapper, IUriExtent extent)
        {
            return mapper.FindFactoryFor(extent.GetType());
        }
    }
}