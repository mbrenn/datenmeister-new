using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;

namespace DatenMeister.Provider.HelpingExtents
{
    public class ExtentObject : MappingProviderObject<IUriExtent>, IProviderObject
    {
        public ExtentObject(IProvider provider, IUriExtent uriExtent) : base(uriExtent, provider, uriExtent.contextURI())
        {
            AddMapping(
                "uri",
                E => E.contextURI(),
                (E, v) => throw new InvalidOperationException("Uri cannot be set"));

            AddMapping(
                "count",
                E => E.elements().size(),
                (E, v) => throw new InvalidOperationException("Count cannot be set"));
        }

    }
}