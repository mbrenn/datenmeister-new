using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Interfaces.MOF.Identifiers;

namespace DatenMeister.Core.Runtime.Proxies.ReadOnly;

public class ReadOnlyUriExtent : ProxyUriExtent
{
    public ReadOnlyUriExtent(IUriExtent extent) : base(extent)
    {
        ActivateObjectConversion(
            x => x == null ? null : new ReadOnlyElement((MofElement) x),
            x =>  new ReadOnlyReflectiveSequence(x),
            x => x?.GetProxiedElement());
    }
}