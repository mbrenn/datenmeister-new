using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Runtime.Copier
{
    public class ExtentCopier
    {
        private IFactory _factory;

        public ExtentCopier(IFactory factory)
        {
            _factory = factory;
        }

        public void Copy(IUriExtent source, IUriExtent target)
        {
            var copier = new ObjectCopier(_factory);
            foreach (var element in source.elements())
            {
                var elementAsElement = element as IElement;
                var copiedElement = copier.Copy(elementAsElement);
                target.elements().add(copiedElement);
            }
        }
    }
}