using Autofac;
using DatenMeister.Core.Plugins;
using DatenMeister.DotNet;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.DotNet;
using DatenMeister.Uml.Helper;
using DatenMeister.UserInteractions;

namespace DatenMeister.Modules.ZipExample
{
    /// <summary>
    /// Integrates the zip code example into the DatenMeister framework
    /// </summary>
    public class ZipCodePlugin : IDatenMeisterPlugin
    {
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly PackageMethods _packageMethods;
        private readonly ZipCodeModel _zipCodeModel;

        /// <summary>
        /// Initializes a new instance of the ZipCodePlugin
        /// </summary>
        /// <param name="localTypeSupport">The local type support being used</param>
        /// <param name="packageMethods">Package methods</param>
        /// <param name="zipCodeModel">The zip code model</param>
        public ZipCodePlugin(
            LocalTypeSupport localTypeSupport,
            PackageMethods packageMethods,
            ZipCodeModel zipCodeModel)
        {
            _localTypeSupport = localTypeSupport;
            _packageMethods = packageMethods;
            _zipCodeModel = zipCodeModel;
        }
        
        /// <summary>
        /// Integrates the zipcodes into the container builder
        /// </summary>
        /// <param name="builder">Container builder to be used</param>
        public static void Into(ContainerBuilder builder)
        {
            builder.RegisterType<ZipCodeInteractionHandler>().As<IElementInteractionsHandler>();
            builder.RegisterType<ZipCodeModel>().As<ZipCodeModel>().SingleInstance();
        }

        public void Start(PluginLoadingPosition position)
        {
            // Load Resource 
            _zipCodeModel.ZipCode =_localTypeSupport.AddInternalType(ZipCodeModel.PackagePath, typeof(ZipCode));
        }
    }
}