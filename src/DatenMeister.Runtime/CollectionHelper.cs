﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    }
}