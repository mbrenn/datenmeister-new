using DatenMeister.DataLayer;
using DatenMeister.EMOF.InMemory;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.Provider.DotNet;

namespace DatenMeister.Apps.ZipCode
{
    public class Integrate
    {
        private readonly IDataLayerLogic _dataLayerLogic;

        public Integrate(IDataLayerLogic dataLayerLogic)
        {
            _dataLayerLogic = dataLayerLogic;
        }

        /// <summary>
        /// Integrates the Types into the extent
        /// </summary>
        /// <param name="typeExtent">Extent to be used</param>
        public void Into(IExtent typeExtent)
        {
            var layerOfTypes = _dataLayerLogic.GetDataLayerOfExtent(typeExtent);
            var layerOfUml = _dataLayerLogic.GetMetaLayerFor(layerOfTypes);
            var uml = _dataLayerLogic.Get<_UML>(layerOfUml);
            var factory = new MofFactory();

            var typeProvider = new DotNetTypeGenerator(factory, uml);
            var typeForZipCodes = typeProvider.CreateTypeFor(typeof (Model.ZipCode));
            typeExtent.elements().add(typeForZipCodes);
        }
    }
}