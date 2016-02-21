using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Models;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreator
    {
        public Dictionary<object, DataTableColumn> ColumnsOnProperty { get; private set; }

        public IList<object> Properties
        {
            get { return ColumnsOnProperty.Select(x => x.Key).ToList(); }
        }

        public IEnumerable<DataTableColumn> FindColumnsForTable(IUriExtent extent)
        {
            return FindColumnsForTable(extent.elements());
        }

        public IEnumerable<DataTableColumn> FindColumnsForTable(IReflectiveSequence elements)
        {
            ColumnsOnProperty = new Dictionary<object, DataTableColumn>();
            foreach (var item in elements)
            {
                EvaluateColumnsForItem(item);
            }

            return ColumnsOnProperty.Select(x => x.Value);
        }

        public IEnumerable<DataTableColumn> FindColumnsForItem(object item)
        {
            ColumnsOnProperty = new Dictionary<object, DataTableColumn>();
            EvaluateColumnsForItem(item);
            return ColumnsOnProperty.Select(x => x.Value);
        }

        private void EvaluateColumnsForItem(object item)
        {
            if (item is IObjectAllProperties)
            {
                var itemAsObjectExt = item as IObjectAllProperties;
                var properties = itemAsObjectExt.getPropertiesBeingSet();

                foreach (var property in properties)
                {
                    DataTableColumn column;
                    if (!ColumnsOnProperty.TryGetValue(property, out column))
                    {
                        column = new DataTableColumn
                        {
                            name = property.ToString(),
                            title = property.ToString()
                        };

                        ColumnsOnProperty[property] = column;
                    }

                    var value = ((IObject) item).get(property);
                    column.isEnumeration |= value is IEnumerable && !(value is string);
                }
            }
        }
    }
}