using System;
using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Web.Helper;

namespace DatenMeister.Web.Modules.ViewFinder
{
    public class ViewFinderImpl : IViewFinder
    {
        private readonly FormCreator _formCreator;

        public ViewFinderImpl(FormCreator formCreator)
        {
            _formCreator = formCreator;
        }

        public IObject FindView(IUriExtent extent, string viewname)
        {
            var table = _formCreator.CreateFields(extent);
            throw new NotImplementedException();
        }

        public IObject FindView(IObject value, string viewname)
        {
            var view = _formCreator.CreateFields(value);
            throw new NotImplementedException();
        }
    }
}