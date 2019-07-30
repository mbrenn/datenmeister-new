using System;
using System.IO;
using Autofac;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Plugins;
using DatenMeister.Integration;
using DatenMeister.Modules.TypeSupport;
using DatenMeister.Provider.XMI.ExtentStorage;
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
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ToString(), 
                "StundenMeister");
            var filePath = Path.Combine(directory, "StundenMeister.xmi");

            var xmiStorageData = new XmiStorageConfiguration
            {
                extentUri = "dm:///stundenmeister/",
                filePath = filePath
            };

            StundenMeisterData.TheOne.Data = _extentManager.LoadExtentIfNotAlreadyLoaded(
                xmiStorageData, 
                ExtentCreationFlags.LoadOrCreate);
        }
        
        /// <summary>
        /// Creates a new element containing the time recordings
        /// </summary>
        /// <returns>The element being created</returns>
        public IElement CreateAndAddNewTimeRecoding()
        {
            var factory = new MofFactory(StundenMeisterData.TheOne.Data);
            var createdItem = factory.create(StundenMeisterData.TheOne.ClassTimeRecording);
            StundenMeisterData.TheOne.Data.elements().add(createdItem);
            return createdItem;
        }
    }
}