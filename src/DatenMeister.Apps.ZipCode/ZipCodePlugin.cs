using DatenMeister.Core;
using DatenMeister.Core.DataLayer;
using DatenMeister.Core.EMOF.InMemory;
using DatenMeister.Core.Filler;
using DatenMeister.Core.Plugins;
using DatenMeister.Provider.DotNet;
using DatenMeister.Runtime.Workspaces;

namespace DatenMeister.Apps.ZipCode
{
    public class ZipCodePlugin : IDatenMeisterPlugin
    {
        private readonly IDataLayerLogic _dataLayerLogic;
        private readonly IWorkspaceCollection _workspaceCollection;

        public ZipCodePlugin(IDataLayerLogic dataLayerLogic, IWorkspaceCollection workspaceCollection)
        {
            _dataLayerLogic = dataLayerLogic;
            _workspaceCollection = workspaceCollection;
        }

        public void Start()
        {
            var typeExtent = _workspaceCollection.FindExtent(Locations.UriTypes);
            var layerOfTypes = _dataLayerLogic.GetDataLayerOfExtent(typeExtent);
            var layerOfUml = _dataLayerLogic.GetMetaLayerFor(layerOfTypes);
            var uml = _dataLayerLogic.Get<_UML>(layerOfUml);
            var factory = new MofFactory();

            var typeProvider = new DotNetTypeGenerator(factory, uml);
            var typeForZipCodes = typeProvider.CreateTypeFor(typeof(Model.ZipCode));
            typeExtent.elements().add(typeForZipCodes);
        }
    }
}