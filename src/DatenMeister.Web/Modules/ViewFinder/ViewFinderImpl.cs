using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Helper;

namespace DatenMeister.Web.Modules.ViewFinder
{
    public class ViewFinderImpl : IViewFinder
    {
        private readonly ColumnCreator _columnCreator;

        public ViewFinderImpl(ColumnCreator columnCreator)
        {
            _columnCreator = columnCreator;
        }

        public IObject FindView(IUriExtent extent, string viewname)
        {
            var table = _columnCreator.FindColumnsForTable(extent);
            throw new System.NotImplementedException();
        }

        public IObject FindView(IObject value, string viewname)
        {
            var view = _columnCreator.FindColumnsForItem(value);
            throw new System.NotImplementedException();
        }
    }
}