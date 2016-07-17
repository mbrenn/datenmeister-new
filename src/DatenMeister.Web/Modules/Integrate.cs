using Autofac;

namespace DatenMeister.Web.Modules
{
    public static class Integrate
    {
        public static void RegisterWebModules(this ContainerBuilder kernel)
        {
            kernel.RegisterInstance(new ClientModulePlugin()).As<IClientModulePlugin>();
        }
    }
}