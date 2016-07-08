using System.Collections.Generic;
using System.Linq;
using DatenMeister.Web.Models.Fields;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreationResult
    {
        internal Dictionary<string, FieldData> ColumnsOnProperty { get; }

        public ColumnCreationResult()
        {
            ColumnsOnProperty = new Dictionary<string, FieldData>();
        }

        public IList<FieldData> Columns
        {
            get { return ColumnsOnProperty.Select(x => x.Value).ToList(); }
        }
        public IList<string> Properties
        {
            get { return ColumnsOnProperty.Select(x => x.Key).ToList(); }
        }
    }

}