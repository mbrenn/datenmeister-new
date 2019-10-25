using System;
using System.Collections.Generic;
using System.Linq;
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
            _factory = factory ?? throw new InvalidOperationException(nameof(factory));
        }

        public void Copy(IExtent source, IExtent target, CopyOption copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;
            var sourceSequence = source.elements();
            var targetSequence = target.elements();

            Copy(sourceSequence, targetSequence, copyOptions);
        }

        public void Copy(IReflectiveCollection sourceSequence, IReflectiveCollection targetSequence, CopyOption copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;

            var copier = new ObjectCopier(_factory);
            foreach (var copiedElement in sourceSequence
                .Select(element => element as IElement)
                .Select(elementAsElement => copier.Copy(elementAsElement, copyOptions)))
            {
                targetSequence.add(copiedElement);
            }
        }

        public void Copy(IEnumerable<object> sourceSequence, IReflectiveCollection targetSequence, CopyOption copyOptions = null)
        {
            copyOptions ??= CopyOptions.None;

            var copier = new ObjectCopier(_factory);
            foreach (var copiedElement in sourceSequence
                .Select(element => element as IElement)
                .Select(elementAsElement => copier.Copy(elementAsElement, copyOptions)))
            {
                targetSequence.add(copiedElement);
            }
        }
    }
}