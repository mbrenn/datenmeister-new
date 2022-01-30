using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.DotNet;
using DatenMeister.Extent.Manager.Extents.Configuration;

namespace DatenMeister.Extent.Manager
{
    public class ManagementSettingsProvider : DotNetProvider
    {
        public ManagementSettingsProvider(IDotNetTypeLookup typeLookup) : base(typeLookup)
        {
            typeLookup.Add(
                _DatenMeister.TheOne.Management.__ExtentTypeSetting,
                typeof(ExtentType));
            typeLookup.Add(
                _DatenMeister.TheOne.Management.__ExtentPropertyDefinition,
                typeof(ExtentPropertyDefinition));
        }
    }
}