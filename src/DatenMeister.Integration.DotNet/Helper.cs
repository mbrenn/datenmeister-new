using Autofac;

namespace DatenMeister.Integration.DotNet
{
    public static class Helper
    {
        /// <summary>
        /// Out of an integrationsettings and a container, a complete dot net integration 
        /// shall be performed
        /// </summary>
        /// <param name="kernel">Kernel being used</param>
        /// <param name="settings">The integration settings</param>
        /// <returns>Container being used</returns>
        public static IContainer UseDatenMeisterDotNet(this ContainerBuilder kernel, IntegrationSettings settings)
        {
            settings.Hooks = new DotNetIntegrationHooks();

            var integration = new Provider.DotNet.Integration(settings);
            return integration.UseDatenMeister(kernel);
        }
    }
}