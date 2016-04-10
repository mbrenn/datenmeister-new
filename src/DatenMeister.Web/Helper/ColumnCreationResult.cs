using System.Collections.Generic;
using System.Linq;
using DatenMeister.Web.Models;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreationResult
    {
        internal Dictionary<object, DataTableColumn> ColumnsOnProperty { get; }

        public ColumnCreationResult()
        {
            ColumnsOnProperty = new Dictionary<object, DataTableColumn>();
        }

        public IList<DataTableColumn> Columns
        {
            get { return ColumnsOnProperty.Select(x => x.Value).ToList(); }
        }
        public IList<object> Properties
        {
            get { return ColumnsOnProperty.Select(x => x.Key).ToList(); }
        }
    }

}