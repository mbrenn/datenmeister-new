using Autofac;
using DatenMeister.Core.EMOF.Implementation.DotNet;
using DatenMeister.Integration;
using DatenMeister.Provider.DotNet;
using DatenMeister.Provider.InMemory;
using DatenMeister.Runtime.Extents.Configuration;

namespace DatenMeister.Provider.ManagementProviders.Settings
{
    public class ManagementSettingsProvider : DotNetProvider
    {
        public ManagementSettingsProvider(IDotNetTypeLookup typeLookup) : base(typeLookup)
        {
        }
    }
}