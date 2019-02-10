using System.IO;
using System.Reflection;
using Autofac;
using DatenMeister.DotNet;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Uml.Helper;
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
            builder.RegisterType<ZipCodeExampleManager>().As<ZipCodeExampleManager>();
            builder.RegisterType<ZipCodeModel>().As<ZipCodeModel>().SingleInstance();
        }

        /// <summary>
        /// Integrates the zip code example into the DatenMeister framework
        /// </summary>
        /// <param name="scope">Scope to which the Zip Code shall be included</param>
        public static void Into(ILifetimeScope scope)
        {
            var localTypeSupport = scope.Resolve<LocalTypeSupport>();

            // Load Resource 
            var zipPackage = ResourceHelper.LoadElementFromResource(
                typeof(Integrate), "DatenMeister.Modules.ZipExample.zip_type_definition.xmi");

            var packageMethods = scope.Resolve<PackageMethods>();
            packageMethods.ImportPackage(
                zipPackage, localTypeSupport.InternalTypes.elements(), "Apps::ZipCode");

            var zipCodeModel = scope.Resolve<ZipCodeModel>();
            zipCodeModel.ZipCode = localTypeSupport.InternalTypes.element(
                "#DatenMeister.Modules.ZipExample.ZipCode");
        }
    }
}