using DatenMeister.EMOF.Interface.Identifiers;
using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.Provider.DotNet;
using DatenMeister.Web.Models.Modules.ViewFinder.Helper;

namespace DatenMeister.Web.Models.Modules.ViewFinder
{
    public class ViewFinderImpl : IViewFinder
    {
        private readonly FormCreator _formCreator;
        private readonly IDotNetTypeLookup _dotNetTypeLookup;

        public ViewFinderImpl(IDotNetTypeLookup dotNetTypeLookup)
        {
            _formCreator = new FormCreator();
            _dotNetTypeLookup = dotNetTypeLookup;
        }

        public IObject FindView(IUriExtent extent, string viewname)
        {
            var view = _formCreator.CreateFields(extent);
            return _dotNetTypeLookup.CreateDotNetElement(view);
        }

        public IObject FindView(IObject value, string viewname)
        {
            var view = _formCreator.CreateFields(value);
            return _dotNetTypeLookup.CreateDotNetElement(view);
        }
    }
}