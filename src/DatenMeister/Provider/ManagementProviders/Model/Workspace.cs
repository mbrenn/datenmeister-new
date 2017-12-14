using System.Collections.Generic;
using DatenMeister.Core.EMOF.Interface.Reflection;

namespace DatenMeister.Provider.ManagementProviders.Model
{
    public class Workspace
    {
        public string id { get; set; }
        public string annotation { get; set; }
        public IEnumerable<Extent> extents;

        /// <summary>
        /// Conversts the given workspace to a string with id and annotation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({id}) {annotation}";
        }
    }
}