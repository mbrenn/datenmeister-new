using DatenMeister.Core;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.Filler;
using DatenMeister.Core.Plugins;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Apps.ZipCode
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ZipCodePlugin : IDatenMeisterPlugin
    {
        private readonly IWorkspaceLogic _dataLayerLogic;

        public ZipCodePlugin(IWorkspaceLogic dataLayerLogic)
        {
            _dataLayerLogic = dataLayerLogic;
        }

        public void Start()
        {
            var typeExtent = _dataLayerLogic.FindExtent(WorkspaceNames.UriInternalTypes);
            var layerOfTypes = _dataLayerLogic.GetWorkspaceOfExtent(typeExtent);
            var layerOfUml = layerOfTypes.MetaWorkspace;
            var uml = layerOfUml.Get<_UML>();
            var factory = new MofFactory();

            var typeProvider = new DotNetTypeGenerator(factory, uml);
            var typeForZipCodes = typeProvider.CreateTypeFor(typeof(Model.ZipCode));
            typeExtent.elements().add(typeForZipCodes);
        }
    }
}