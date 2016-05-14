using System.Collections.Generic;
using System.Linq;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreationResult
    {
        internal Dictionary<object, DataField> ColumnsOnProperty { get; }

        public ColumnCreationResult()
        {
            ColumnsOnProperty = new Dictionary<object, DataField>();
        }

        public IList<DataField> Columns
        {
            get { return ColumnsOnProperty.Select(x => x.Value).ToList(); }
        }
        public IList<object> Properties
        {
            get { return ColumnsOnProperty.Select(x => x.Key).ToList(); }
        }
    }

}