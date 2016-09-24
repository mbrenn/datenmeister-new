using DatenMeister.Core;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.Filler;
using DatenMeister.Core.Plugins;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Apps.ZipCode
{
    public class ZipCodePlugin : IDatenMeisterPlugin
    {
        private readonly IWorkspaceLogic _dataLayerLogic;
        private readonly IWorkspaceCollection _workspaceCollection;

        public ZipCodePlugin(IWorkspaceLogic dataLayerLogic, IWorkspaceCollection workspaceCollection)
        {
            _dataLayerLogic = dataLayerLogic;
            _workspaceCollection = workspaceCollection;
        }

        public void Start()
        {
            var typeExtent = _workspaceCollection.FindExtent(Locations.UriInternalTypes);
            var layerOfTypes = _dataLayerLogic.GetDataLayerOfExtent(typeExtent);
            var layerOfUml = _dataLayerLogic.GetMetaLayerFor(layerOfTypes);
            var uml = layerOfUml.Get<_UML>();
            var factory = new MofFactory();

            var typeProvider = new DotNetTypeGenerator(factory, uml);
            var typeForZipCodes = typeProvider.CreateTypeFor(typeof(Model.ZipCode));
            typeExtent.elements().add(typeForZipCodes);
        }
    }
}