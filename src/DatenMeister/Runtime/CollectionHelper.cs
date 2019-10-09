using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.Core.EMOF.Interface.Common;
using DatenMeister.Core.EMOF.Interface.Identifiers;
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime
{
    public static class CollectionHelper
    {
        public static IList<T> ToList<T>(this IReflectiveCollection collection) =>
            new ReflectiveList<T>(collection);

        public static IList<T> ToList<T>(this IReflectiveCollection collection, Func<object, T> wrapFunc,
            Func<T, object> unwrapFunc)
            =>
                new ReflectiveList<T>(collection, wrapFunc, unwrapFunc);

        /// <summary>
        /// Gets the first element of the reflective collection.
        /// Returns null, if no element is existing. If the given element is not
        /// a enumeration, the element itself will be returned
        /// </summary>
        /// <param name="value">Value to be queried</param>
        /// <returns>Returned element</returns>
        public static object MakeSingle(object value)
        {
            if (DotNetHelper.IsOfEnumeration(value))
            {
                var asEnumeration = (IEnumerable) value;
                return asEnumeration.Cast<object>().FirstOrDefault();
            }

            return value;
        }

        public static IEnumerable<IObject> OnlyObjects(this IEnumerable<object> values)
        {
            return values.Select(x => x as IObject).Where(x => x != null);
        }

        /// <summary>
        /// Gets all metaclasses of a reflectivecollection, where each metaclass is returned once
        /// </summary>
        /// <param name="values">Reflective collection to be evaluated</param>
        /// <returns>Enumeration of distincting elements</returns>
        public static IEnumerable<IElement> GetMetaClasses(this IEnumerable<object> values)
        {
            return values.OfType<IElement>().Select(x => x.getMetaClass()).Where(x => x != null).Distinct();
        }

        /// <summary>
        /// Gets the associated extent of the reflective collection
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static IExtent GetAssociatedExtent(this IReflectiveCollection collection)
        {
            var mofReflection = collection as IHasExtent ??
                                throw new ArgumentException(@"Not of type IHasExtent", nameof(collection));
            return mofReflection.Extent;
        }
    }
}