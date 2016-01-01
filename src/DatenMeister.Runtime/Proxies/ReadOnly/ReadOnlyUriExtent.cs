﻿using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Proxy;

namespace DatenMeister.Runtime.Proxies.ReadOnly
{
    public class ReadOnlyUriExtent : ProxyUriExtent
    {
        public ReadOnlyUriExtent(IUriExtent extent) : base(extent)
        {
            ActivateObjectConversion(
                x => new ReadOnlyElement(x),
                x => new ReadOnlyReflectiveSequence(x),
                x => x.GetProxiedElement());
        }
    }
}