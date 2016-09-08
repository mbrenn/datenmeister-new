using DatenMeister.Core.EMOF.Interface.Identifiers;

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