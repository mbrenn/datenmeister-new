using System.Linq;
using Autofac;
using DatenMeister.Models.Example.ZipCode;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Modules.UserInteractions;
using DatenMeister.Runtime.Extents.Configuration;
using DatenMeister.Runtime.Plugins;

namespace DatenMeister.Modules.ZipExample
{
    /// <summary>
    /// Integrates the zip code example into the DatenMeister framework
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterInitialization | PluginLoadingPosition.AfterBootstrapping)]
    public class ZipCodePlugin : IDatenMeisterPlugin
    {
        public const string ExtentType = "DatenMeister.Example.ZipCodes";
        
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly ZipCodeModel _zipCodeModel;
        private readonly ExtentSettings _extentSettings;

        /// <summary>
        /// Initializes a new instance of the ZipCodePlugin
        /// </summary>
        /// <param name="localTypeSupport">The local type support being used</param>
        /// <param name="zipCodeModel">The zip code model</param>
        /// <param name="extentSettings"></param>
        public ZipCodePlugin(
            LocalTypeSupport localTypeSupport,
            ZipCodeModel zipCodeModel,
            ExtentSettings extentSettings
            )
        {
            _localTypeSupport = localTypeSupport;
            _zipCodeModel = zipCodeModel;
            _extentSettings = extentSettings;
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
            switch (position)
            {
                case PluginLoadingPosition.AfterInitialization:
                {
                    // Load Resource
                    var types = _localTypeSupport.AddInternalTypes(
                        ZipCodeModel.PackagePath,
                        new[] {typeof(ZipCode), typeof(ZipCodeWithState)});
                    _zipCodeModel.ZipCode = types.ElementAt(0);
                    _zipCodeModel.ZipCodeWithState = types.ElementAt(1);
                    break;
                }
                case PluginLoadingPosition.AfterBootstrapping:
                    _extentSettings.extentTypeSettings.Add(
                        new ExtentTypeSetting(ExtentType));
                    break;
            }
        }
    }
}