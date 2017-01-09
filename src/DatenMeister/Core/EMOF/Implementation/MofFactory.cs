using System;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Core.EMOF.Implementation
{
    public class MofFactory : IFactory
    {
        private readonly Extent _extent;

        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(Extent extent)
        {
            _extent = extent;
        }

        /// <summary>
        /// Initializes a new instance of the Factory
        /// </summary>
        /// <param name="extent"></param>
        public MofFactory(IExtent extent)
        {
            _extent = (Extent) extent;
        }

        /// <inheritdoc />
        public IElement package
        {
            get { throw new NotImplementedException(); }
        }

        /// <inheritdoc />
        public IElement create(IElement metaClass)
        {
            var elementAsMetaClass = (MofElement) metaClass;
            var uriMetaClass = (elementAsMetaClass?.Extent as UriExtent)?.uri(metaClass);
            return new MofElement(
                _extent.Provider.CreateElement(uriMetaClass),
                _extent);
        }

        /// <inheritdoc />
        public IObject createFromString(IElement dataType, string value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string convertToString(IElement dataType, IObject value)
        {
            throw new NotImplementedException();
        }
    }
}