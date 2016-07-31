using System;

namespace DatenMeister.Runtime
{
    /// <summary>
    /// This class stores a set of helper methods to access, evaluate and work on
    /// real .Net objects
    /// </summary>
    public static class DotNetHelper
    {
        /// <summary>
        /// Evaluates whether the given type is a primitive type. 
        /// A primitive type is considered all numbers, strings, timespan and DateTime
        /// </summary>
        /// <param name="type">Type to be evaluated</param>
        /// <returns>true, if the given type is a primitive type</returns>
        public static bool IsPrimitiveType(Type type)
        {
            return type == typeof(bool)
                   || type == typeof(float)
                   || type == typeof(double)
                   || type == typeof(byte)
                   || type == typeof(short)
                   || type == typeof(int)
                   || type == typeof(long)
                   || type == typeof(string)
                   || type == typeof(TimeSpan)
                   || type == typeof(DateTime);
        }
    }
}