using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Provider.DotNet;

namespace DatenMeister.Provider.ManagementProviders.Settings
{
    public class ManagementSettingsProvider : DotNetProvider
    {
        public ManagementSettingsProvider(IDotNetTypeLookup typeLookup) : base(typeLookup)
        {
        }
    }
}