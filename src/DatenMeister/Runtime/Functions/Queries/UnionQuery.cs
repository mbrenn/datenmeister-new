using System.Collections.Generic;
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
        private readonly IReflectiveCollection _first;
        private readonly IReflectiveCollection _second;

        public UnionQuery(IReflectiveCollection first, IReflectiveCollection second)
        {
            _first = first;
            _second = second;

            Values = Enumerable.Union(first, second);
        }
    }
}