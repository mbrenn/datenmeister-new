using System;

namespace DatenMeister.Runtime
{
    public class ExtentHelper
    {
        /// <summary>
        /// Creates a temporary uri for an extent. 
        /// It starts with 'dm:///temp/' and finishes 
        /// </summary>
        /// <returns>The created uri</returns>
        public static string CreateTemporaryExtentUri()
        {
            return $"dm:///{Guid.NewGuid()}";
        }
    }
}