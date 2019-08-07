using System;
using System.IO;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Plugins;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using StundenMeister.Model;

namespace StundenMeister.Logic
{
    public class StundenMeisterLogic : IDatenMeisterPlugin
    {
        /// <summary>
        /// Gets the StundenMeisterLogic for the application
        /// </summary>
        /// <returns></returns>
        public static StundenMeisterLogic Get()
        {
            return GiveMe.Scope.Resolve<StundenMeisterLogic>();
        }
        
        private readonly LocalTypeSupport _localTypeSupport;
        private readonly ExtentManager _extentManager;

        public StundenMeisterLogic(LocalTypeSupport localTypeSupport, ExtentManager extentManager)
        {
            _localTypeSupport = localTypeSupport;
            _extentManager = extentManager;
        }

        public void Start(PluginLoadingPosition position)
        {
            var types = _localTypeSupport.AddInternalTypes(
                TypeList.Types);
            StundenMeisterData.TheOne.ClassCostCenter =
                types[Array.IndexOf(TypeList.Types, typeof(CostCenter))];
            StundenMeisterData.TheOne.ClassTimeRecording =
                types[Array.IndexOf(TypeList.Types, typeof(TimeRecording))];

            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "StundenMeister");
            var filePath = Path.Combine(directory, "StundenMeister.xmi");

            var storageData = new XmiStorageConfiguration
            {
                extentUri = "dm:///stundenmeister/",
                filePath = filePath
            };

            StundenMeisterData.TheOne.Extent =
                _extentManager.LoadExtentIfNotAlreadyLoaded(
                    storageData,
                    ExtentCreationFlags.LoadOrCreate);
            
            var recordingLogic = new TimeRecordingLogic(this);
            recordingLogic.Initialize();
        }

        /// <summary>
        /// Stores the information of the extent into the file
        /// </summary>
        public void StoreExtent()
        {
            _extentManager.StoreExtent(Data.Extent); 
        }

        /// <summary>
        /// Gets the data for the Stundenmeister
        /// </summary>
        public StundenMeisterData Data => StundenMeisterData.TheOne;
    }
}