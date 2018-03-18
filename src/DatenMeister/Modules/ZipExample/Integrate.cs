using Autofac;
using DatenMeister.Integration;
using DatenMeister.UserInteractions;

namespace DatenMeister.Modules.ZipExample
{
    /// <summary>
    /// Integrates the zip code example into the DatenMeister framework
    /// </summary>
    public static class Integrate
    {
        /// <summary>
        /// Integrates the zipcodes into the container builder
        /// </summary>
        /// <param name="builder">Container builder to be used</param>
        public static void Into(ContainerBuilder builder)
        {
            builder.RegisterType<ZipCodeInteractionHandler>().As<IElementInteractionsHandler>();
        }

        /// <summary>
        /// Integrates the zip code example into the DatenMeister framework
        /// </summary>
        /// <param name="scope">Scope to which the Zip Code shall be included</param>
        public static void Into(ILifetimeScope scope)
        {
            var exampleController = scope.Resolve<ZipExampleController>();
            exampleController.Initialize();
        }
    }
}