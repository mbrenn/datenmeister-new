using Autofac;

namespace DatenMeister.Integration.DotNet
{
    public static class Helper
    {
        public static IContainer UseDatenMeisterDotNet(this ContainerBuilder kernel, IntegrationSettings settings)
        {
            settings.Hooks = new DotNetIntegrationHooks();

            var integration = new Integration(settings);
            return integration.UseDatenMeister(kernel);
        }
    }
}