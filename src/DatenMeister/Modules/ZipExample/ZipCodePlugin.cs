using System.Linq;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Models.Example.ZipCode;
using DatenMeister.Models.ManagementProvider;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Modules.UserInteractions;
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
        private readonly IScopeStorage _scopeStorage;
        private readonly ExtentSettings _extentSettings;

        /// <summary>
        /// Initializes a new instance of the ZipCodePlugin
        /// </summary>
        /// <param name="localTypeSupport">The local type support being used</param>
        /// <param name="zipCodeModel">The zip code model</param>
        /// <param name="scopeStorage">Scope storage</param>
        public ZipCodePlugin(
            LocalTypeSupport localTypeSupport,
            IScopeStorage scopeStorage
        )
        {
            _localTypeSupport = localTypeSupport;
            _scopeStorage = scopeStorage;
            _extentSettings = scopeStorage.Get<ExtentSettings>();
        }

        public void Start(PluginLoadingPosition position)
        {
            switch (position)
            {
                case PluginLoadingPosition.AfterInitialization:
                {
                    var zipCodeModel = new ZipCodeModel();
                    _scopeStorage.Add(zipCodeModel);
                    
                    // Load Resource
                    var types = _localTypeSupport.AddInternalTypes(
                        ZipCodeModel.PackagePath,
                        new[] {typeof(ZipCode), typeof(ZipCodeWithState)});
                    zipCodeModel.ZipCode = types.ElementAt(0);
                    zipCodeModel.ZipCodeWithState = types.ElementAt(1);

                    _scopeStorage
                        .Get<UserInteractionState>()
                        .ElementInteractionHandler
                        .Add(
                            new ZipCodeInteractionHandler());
                    
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