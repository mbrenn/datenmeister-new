using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Functions.Queries;
using DatenMeister.Core.Runtime.Proxies;

namespace DatenMeister.Core.Extensions.Functions.Transformation
{
    public static class FilterHierarchy
    {
        /// <summary>
        /// Takes the given collection and creates a proxied collection which just contains
        /// the elements which have one of the given ids as a parent or as a child.
        /// </summary>
        /// <param name="collection">The collection being queried</param>
        /// <param name="ids">The ids that are looked for</param>
        /// <returns>The reflective collection containing the original elements</returns>
        public static IReflectiveCollection Extract(IReflectiveCollection collection, IEnumerable<string> ids)
        {
            var allElements = AllDescendentsQuery.GetDescendents(collection);
            var found = allElements.OfType<IHasId>().Where(x => ids.Any(y => y == x.Id));

            var result = new PureReflectiveSequence();
            foreach (var element in found)
                result.add(element);

            return result;
        }

        /// <summary>
        /// Takes the given collection and creates a proxied collection which just contains
        /// the elements which have one of the given ids as a parent or as a child.
        /// </summary>
        /// <param name="collection">The collection being queried</param>
        /// <param name="elements">The elements that are looked for</param>
        /// <returns>The reflective collection containing the original elements</returns>
        public static IReflectiveCollection Extract(IReflectiveCollection collection, IEnumerable<IObject> elements)
        {
            var allElements = AllDescendentsQuery.GetDescendents(collection);
            var found = allElements.Where(x => elements.Any(y => Equals(y, x)));

            var result = new PureReflectiveSequence();
            foreach (var element in found)
                result.add(element);

            return result;
        }
    }
}