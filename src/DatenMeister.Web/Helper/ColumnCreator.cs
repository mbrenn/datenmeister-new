using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Models;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreator
    {
        public List<object>  Properties { get; private set; }

        public ColumnCreator()
        {
            Properties = new List<object>();
        }

        public IEnumerable<DataTableColumn> GetColumnsForTable(IUriExtent extent)
        {
            return GetColumnsForTable(extent.elements());
        }

        private IEnumerable<DataTableColumn> GetColumnsForTable(IReflectiveSequence elements)
        {
            var result = new Dictionary<object, DataTableColumn>();
            foreach (var item in elements)
            {
                if (item is IObjectAllProperties)
                {
                    var itemAsObjectExt = item as IObjectAllProperties;
                    var properties = itemAsObjectExt.getPropertiesBeingSet();

                    foreach (var property in properties)
                    {
                        DataTableColumn column;
                        if (!result.TryGetValue(property, out column))
                        {
                            column = new DataTableColumn
                            {
                                name = property.ToString(),
                                title = property.ToString()
                            };

                            Properties.Add(property);

                            result[property] = column;
                        }

                        var value = ((IObject) item).get(property);
                        column.isEnumeration |= value is IEnumerable && !(value is string);
                    }
                }
            }

            return result.Select(x => x.Value);
        }
    }
}