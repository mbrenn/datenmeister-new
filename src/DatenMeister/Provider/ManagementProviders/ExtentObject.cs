﻿using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Provider.ManagementProviders
{
    public class ExtentObject : MappingProviderObject<IUriExtent>
    {
        public ExtentObject(IProvider provider, IUriExtent uriExtent) : base(uriExtent, provider, uriExtent.contextURI())
        {
            AddMapping(
                "uri",
                e => e.contextURI(),
                (e, v) => throw new InvalidOperationException("Uri cannot be set"));

            AddMapping(
                "count",
                e => e.elements().size(),
                (e, v) => throw new InvalidOperationException("Count cannot be set"));
        }
    }
}