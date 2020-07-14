using System;
using System.Collections.Generic;

namespace DatenMeister.Models.AttachedExtent
{
    public static class AttachedExtentTypes
    {
        public static IEnumerable<Type> GetTypes()
        {
            return new[]
            {
                typeof(AttachedExtentConfiguration)
            };
        }
    }
}