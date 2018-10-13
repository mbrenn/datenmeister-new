using System.IO;
using System.Reflection;
using Autofac;
using DatenMeister.DotNet;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
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
            var zipCodeType = localTypeSupport.AddInternalType(ZipCodeModel.PackagePath,
                typeof(ZipCode)
            );

            // Load Resource, if possible:
            using (var stream = typeof(Integrate).Assembly.GetManifestResourceStream("DatenMeister.Modules.ZipExample.zip_type_definition.xmi"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    var packageDefinition = reader.ReadToEnd();
                }
            }

            var zipPackage = ResourceHelper.LoadElementFromResource(
                typeof(Integrate), "DatenMeister.Modules.ZipExample.zip_type_definition.xmi");

                var zipCodeModel = scope.Resolve<ZipCodeModel>();
            zipCodeModel.ZipCode = zipCodeType;
        }
    }
}