using Autofac;
using DatenMeister.Models.Modules;
using DatenMeister.Models.Modules.ViewFinder;

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
            
            // Adds the view finder
            kernel.RegisterType<ViewFinderImpl>().As<IViewFinder>();
        }
    }
}