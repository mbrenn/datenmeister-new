using System.Linq;
using DatenMeister.Core;
using DatenMeister.Core.Models;
using DatenMeister.Extent.Manager.Extents.Configuration;
using DatenMeister.Forms;
using DatenMeister.Forms.Helper;
using DatenMeister.Modules.ZipCodeExample.Model;
using DatenMeister.Plugins;
using DatenMeister.Types;

namespace DatenMeister.Modules.ZipCodeExample
{
    /// <summary>
    /// Integrates the zip code example into the DatenMeister framework
    /// </summary>
    [PluginLoading(PluginLoadingPosition.AfterInitialization | PluginLoadingPosition.AfterBootstrapping)]
    public class ZipCodePlugin : IDatenMeisterPlugin
    {
        public const string ExtentType = "DatenMeister.Example.ZipCodes";
        
        public const string CreateZipExample = "ZipExample.CreateExample";
        
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly IScopeStorage _scopeStorage;
        private readonly ExtentSettings _extentSettings;

        /// <summary>
        /// Initializes a new instance of the ZipCodePlugin
        /// </summary>
        /// <param name="localTypeSupport">The local type support being used</param>
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
                    
                    ActionButtonToFormAdder.AddActionButton(
                        _scopeStorage.Get<FormsPluginState>(),
                        new ActionButtonAdderParameter(CreateZipExample, "Create Zip-Example")
                        {
                            MetaClass = _DatenMeister.TheOne.Management.__Workspace
                        });
                    
                    break;
                }
                case PluginLoadingPosition.AfterBootstrapping:
                    _extentSettings.extentTypeSettings.Add(
                        new ExtentType(ExtentType));
                    break;
            }
        }
    }
}