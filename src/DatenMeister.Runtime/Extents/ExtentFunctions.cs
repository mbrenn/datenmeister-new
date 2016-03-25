﻿using System.Collections.Generic;
using System.Linq;
using DatenMeister.DataLayer;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.Queries;

namespace DatenMeister.Runtime.Extents
{
    public class ExtentFunctions
    {
        private readonly IDataLayerLogic _dataLayerLogic;
        public ExtentFunctions(IDataLayerLogic dataLayerLogic)
        {
            _dataLayerLogic = dataLayerLogic;
        }

        /// <summary>
        /// Gets an enumeration of creatable types for a given extent. 
        /// It navigates to the meta extent and looks for all classes
        /// </summary>
        /// <param name="extent">The extent into which a new instance shall be created</param>
        /// <returns>Enumeration of types</returns>
        public CreateableTypeResult GetCreatableTypes(IUriExtent extent)
        {
            var dataLayer = _dataLayerLogic.GetDataLayerOfExtent(extent);
            var typeLayer = _dataLayerLogic.GetMetaLayerFor(dataLayer);
            var umlLayer= _dataLayerLogic.GetMetaLayerFor(typeLayer);

            var uml = _dataLayerLogic.Get<_UML>(umlLayer);
            var classType = uml.StructuredClassifiers.__Class;

            var result = new CreateableTypeResult
            {
                MetaLayer = typeLayer,
                CreatableTypes = _dataLayerLogic.GetExtentsForDatalayer(typeLayer)
                    .SelectMany(x => x.elements().WhenMetaClassIs(classType))
                    .Cast<IElement>()
                    .ToList()
            };

            return result;
        }

        public class CreateableTypeResult
        {
            public IDataLayer MetaLayer { get; set; }
            public IList<IElement> CreatableTypes { get; set; }
        }
    }
}