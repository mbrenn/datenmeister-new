using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Core.Models;
using DatenMeister.Core.Provider.DotNet;
using DatenMeister.Runtime.Extents.Configuration;

namespace DatenMeister.Provider.ManagementProviders.Settings
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