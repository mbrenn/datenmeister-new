using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DatenMeister.EMOF.Interface.Common;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Uml.Helper;
using DatenMeister.Web.Models;

namespace DatenMeister.Web.Helper
{
    public class ColumnCreator
    {
        private readonly IUmlNameResolution _nameResolution;

        public ColumnCreator(IUmlNameResolution nameResolution)
        {
            _nameResolution = nameResolution;
        }

        public ColumnCreationResult FindColumnsForTable(IUriExtent extent)
        {
            return FindColumnsForTable(extent.elements());
        }

        public ColumnCreationResult FindColumnsForTable(IReflectiveSequence elements)
        {
            var result = new ColumnCreationResult();
            foreach (var item in elements)
            {
                EvaluateColumnsForItem(result, item);
            }

            return result;
        }

        public ColumnCreationResult FindColumnsForItem(object item)
        {
            var result = new ColumnCreationResult();
            EvaluateColumnsForItem(result, item);
            return result;
        }

        private void EvaluateColumnsForItem(ColumnCreationResult result, object item)
        {
            if (item is IObjectAllProperties)
            {
                var itemAsObjectExt = item as IObjectAllProperties;
                var properties = itemAsObjectExt.getPropertiesBeingSet();

                foreach (var property in properties)
                {
                    DataTableColumn column;
                    if (!result.ColumnsOnProperty.TryGetValue(property, out column))
                    {
                        column = new DataTableColumn
                        {
                            name = property.ToString(),
                            title = _nameResolution == null ? property.ToString() : _nameResolution.GetName(property)
                        };

                        result.ColumnsOnProperty[property] = column;
                    }

                    var value = ((IObject) item).get(property);
                    column.isEnumeration |= value is IEnumerable && !(value is string);
                }
            }
        }
    }
}