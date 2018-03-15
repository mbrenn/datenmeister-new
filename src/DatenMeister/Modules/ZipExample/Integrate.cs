using Autofac;
using DatenMeister.Integration;

namespace DatenMeister.Modules.ZipExample
{
    /// <summary>
    /// Integrates the zip code example into the DatenMeister framework
    /// </summary>
    public static class Integrate
    {
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