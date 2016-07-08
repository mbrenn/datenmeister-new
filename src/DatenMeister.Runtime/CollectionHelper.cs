using System;
using System.Collections.Generic;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.Runtime.Proxies;

namespace DatenMeister.Runtime
{
    public static class CollectionHelper
    {
        public static IList<T> ToList<T>(this IReflectiveCollection collection)
        {
            return new ReflectiveList<T>(collection);
        }

        public static IList<T> ToList<T>(this IReflectiveCollection collection, Func<object, T> wrapFunc, Func<T, object> unwrapFunc)
        {
            return new ReflectiveList<T>(collection, wrapFunc, unwrapFunc);
        }
    }
}