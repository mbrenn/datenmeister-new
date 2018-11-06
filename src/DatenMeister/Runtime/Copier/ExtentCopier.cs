using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Copier
{
    /// <summary>
    /// Supports the copying of an extent
    /// </summary>
    public class ExtentCopier
    {
        private readonly IFactory _factory;

        public ExtentCopier(IFactory factory)
        {
            _factory = factory;
        }

        public void Copy(IExtent source, IExtent target)
        {
            var sourceSequence = source.elements();
            var targetSequence = target.elements();

            Copy(sourceSequence, targetSequence);
        }

        public void Copy(IReflectiveCollection sourceSequence, IReflectiveCollection targetSequence)
        {
            var copier = new ObjectCopier(_factory);
            foreach (var element in sourceSequence)
            {
                var elementAsElement = element as IElement;
                var copiedElement = copier.Copy(elementAsElement);
                targetSequence.add(copiedElement);
            }
        }

        public void Copy(IEnumerable<object> sourceSequence, IReflectiveCollection targetSequence)
        {
            var copier = new ObjectCopier(_factory);
            foreach (var element in sourceSequence)
            {
                var elementAsElement = element as IElement;
                var copiedElement = copier.Copy(elementAsElement);
                targetSequence.add(copiedElement);
            }
        }
    }
}