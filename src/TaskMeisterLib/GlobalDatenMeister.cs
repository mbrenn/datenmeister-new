using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DatenMeister.Core;
using DatenMeister.Core.EMOF.Attributes;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Provider.InMemory;
using DatenMeister.Provider.XMI.EMOF;
using DatenMeister.Provider.XMI.ExtentStorage;
using DatenMeister.Runtime.Workspaces;
using TaskMeister.InMemory;
using TaskMeister.Model;

namespace TaskMeister
{
    public static class GlobalDatenMeister
    {
        private static bool _isInitialized;

        private static Func<Task> _persistFunction;

        public static _TaskMeisterModel TaskMeister { get; private set; }

        private static readonly IUriExtent TypeLayer = new InMemoryUriExtent("dm:///typemaster/types");

        public static IActivityProvider TaskMeisterProvider { get; private set; }

        public static IEnumerable<Type> GetModelTypes()
        {
            return new[]
            {
                typeof(IActivity),
                typeof(IPerson)
            };
        }


        public static async Task Init(InitType initType, string filePath)
        {
            var workspaceLogic = WorkspaceLogic.GetDefaultLogic();

            if (!Enum.IsDefined(typeof(InitType), initType))
                throw new ArgumentOutOfRangeException(nameof(initType), "Value should be defined in the InitType enum.");

            if (_isInitialized)
            {
                Debug.WriteLine("DatenMeister was already initialized");
            }

            _isInitialized = true;
            
            // Performs the initialization of data layers
            WorkspaceData data;
            data = WorkspaceLogic.InitDefault();

            // Creates the types
            TaskMeister = new _TaskMeisterModel();
            TypeLayer.elements().add(TaskMeister.__IActivity);
            data.Types.Set(TaskMeister);
            
            // Loads or creates the extents
            IUriExtent dataExtent;
            IFactory factory;
            var uriTaskMeister = "dm:///taskmeister/example";
            switch (initType)
            {
                case InitType.NonPersistantGeneric:
                    dataExtent = new InMemoryUriExtent(uriTaskMeister);
                    factory = new InMemoryFactory();

                    _persistFunction = null;
                    break;

                case InitType.PersistantXmi:
                    var xmiConfiguration = new XmiStorageConfiguration
                    {
                        Path = filePath,
                        ExtentUri = uriTaskMeister
                    };

                    factory = new XmlFactory();
                    var xmiStorage = new XmiStorage(workspaceLogic);
                    try
                    {
                        dataExtent = xmiStorage.LoadExtent(xmiConfiguration, true);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception.ToString());
                        dataExtent = new XmlUriExtent(workspaceLogic, uriTaskMeister);
                    }

                    _persistFunction = async () =>
                    {

                        var xmi = new XmiStorage(workspaceLogic);
                        await Task.Run(() => xmi.StoreExtent(dataExtent, xmiConfiguration));
                    };

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(initType), initType, null);
            }

            var eap = new ExampleActivityProvider(dataExtent, TaskMeister, factory);
            await eap.Init();
            TaskMeisterProvider = eap;
        }

        public static async Task Persist()
        {
            Debug.WriteLine("Persisting the data");
            if (_persistFunction != null)
            {
                await _persistFunction();
            }

            Debug.WriteLine("Data was persisted");
        }
    }
}

