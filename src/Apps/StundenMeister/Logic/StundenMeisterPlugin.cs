using System;
using System.IO;
using Autofac;
using DatenMeister.Integration;
using DatenMeister.Modules.ChangeEvents;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime;
using DatenMeister.Runtime.ExtentStorage;
using DatenMeister.Runtime.Plugins;
using StundenMeister.Model;

namespace StundenMeister.Logic
{
    public class StundenMeisterPlugin : IDatenMeisterPlugin
    {
        /// <summary>
        /// Defines the configuration of the StundenMeister
        /// </summary>
        public StundenMeisterConfiguration Configuration { get; }
            = new StundenMeisterConfiguration();

        /// <summary>
        /// Gets the StundenMeisterLogic for the application
        /// </summary>
        /// <returns></returns>
        public static StundenMeisterPlugin Get()
        {
            return GiveMe.Scope.Resolve<StundenMeisterPlugin>();
        }

        private readonly LocalTypeSupport _localTypeSupport;

        private readonly ExtentManager _extentManager;

        /// <summary>
        /// Gets the change event manager
        /// </summary>
        public ChangeEventManager EventManager { get; }

        public StundenMeisterPlugin(
            LocalTypeSupport localTypeSupport,
            ExtentManager extentManager,
            ChangeEventManager changeEventManager)
        {
            _localTypeSupport = localTypeSupport;
            _extentManager = extentManager;
            EventManager = changeEventManager;
        }

        public void Start(PluginLoadingPosition position)
        {
            var types = _localTypeSupport.AddInternalTypes(TypeList.Types);
            StundenMeisterData.TheOne.ClassCostCenter =
                types[Array.IndexOf(TypeList.Types, typeof(CostCenter))];
            StundenMeisterData.TheOne.ClassTimeRecording =
                types[Array.IndexOf(TypeList.Types, typeof(TimeRecording))];
            
            _localTypeSupport.InternalTypes.GetWorkspace().DynamicFunctionManager.AddDerviceProperty(
                StundenMeisterData.TheOne.ClassTimeRecording, 
                nameof(TimeRecording.timeSpanSeconds),  
                x=> TimeRecording.GetTimeSpanSeconds(x));
            _localTypeSupport.InternalTypes.GetWorkspace().DynamicFunctionManager.AddDerviceProperty(
                StundenMeisterData.TheOne.ClassTimeRecording, 
                nameof(TimeRecording.timeSpanHours),  
                x=> TimeRecording.GetTimeSpanHours(x));

            var directory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "StundenMeister");
            var filePath = Path.Combine(directory, "StundenMeister.xmi");

            var storageData = new XmiStorageLoaderConfig("dm:///stundenmeister/")
            {
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