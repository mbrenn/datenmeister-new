using Autofac;
using DatenMeister.Web.Models.Modules;

namespace DatenMeister.Web.Modules
{
    public static class Integrate
    {
        /// <summary>
        /// Registers the web modules
        /// </summary>
        /// <param name="kernel">Kernel to be usesd</param>
        public static void RegisterWebModules(this ContainerBuilder kernel)
        {
            kernel.RegisterInstance(new ClientModulePlugin()).As<IClientModulePlugin>();
        }
    }
}