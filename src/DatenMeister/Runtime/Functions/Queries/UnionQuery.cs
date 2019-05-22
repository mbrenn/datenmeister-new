using System.Linq;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Common;

namespace DatenMeister.Runtime.Functions.Queries
{
    /// <summary>
    /// Performs a unionization of two reflective collections
    /// </summary>
    public class UnionQuery : TemporaryReflectiveCollection
    {
        /// <summary>
        /// The first reflective collection
        /// </summary>
        private readonly IReflectiveCollection _first;

        /// <summary>
        /// The second reflective collection being added to the first one
        /// </summary>
        private readonly IReflectiveCollection _second;

        public UnionQuery(IReflectiveCollection first, IReflectiveCollection second)
        {
            _first = first;
            _second = second;

            Values = Enumerable.Union(first, second);
        }

        /// <summary>
        /// Clears both reflective collections, so the unionized collection is also empty.
        /// Be aware, that the sources will also be cleared
        /// </summary>
        public override void clear()
        {
            _first.clear();
            _second.clear();
        }

    }
}