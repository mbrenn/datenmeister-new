using Autofac;

namespace DatenMeister.Integration
{
    /// <summary>
    /// Returns a complete DatenMeister instance
    /// </summary>
    public static class GiveMe
    {
        public static IContainer DatenMeister(IntegrationSettings settings = null)
        {
            if (settings == null)
            {
                settings = new IntegrationSettings()
                {
                    PathToXmiFiles = "App_Data/Xmi",
                    EstablishDataEnvironment = true
                };
            }

            var kernel = new ContainerBuilder();
            var container = kernel.UseDatenMeister(settings);
            
            return new DatenMeisterContainer(container);
        }
    }
}
