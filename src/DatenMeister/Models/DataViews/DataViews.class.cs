using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models.DataViews
{
    public class _DataViews
    {
        public class _DataView
        {
            public static string @name = "name";
            public IElement _name = null;

            public static string @workspace = "workspace";
            public IElement _workspace = null;

            public static string @uri = "uri";
            public IElement _uri = null;

            public static string @viewNode = "viewNode";
            public IElement _viewNode = null;

        }

        public _DataView @DataView = new _DataView();
        public IElement @__DataView = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.DataView");

        public class _ViewNode
        {
            public static string @name = "name";
            public IElement _name = null;

        }

        public _ViewNode @ViewNode = new _ViewNode();
        public IElement @__ViewNode = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ViewNode");

        public class _SourceExtentNode
        {
            public static string @extentUri = "extentUri";
            public IElement _extentUri = null;

            public static string @workspace = "workspace";
            public IElement _workspace = null;

            public static string @name = "name";
            public IElement _name = null;

        }

        public _SourceExtentNode @SourceExtentNode = new _SourceExtentNode();
        public IElement @__SourceExtentNode = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.SourceExtentNode");

        public class _FlattenNode
        {
            public static string @input = "input";
            public IElement _input = null;

            public static string @name = "name";
            public IElement _name = null;

        }

        public _FlattenNode @FlattenNode = new _FlattenNode();
        public IElement @__FlattenNode = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.FlattenNode");

        public class _FilterPropertyNode
        {
            public static string @input = "input";
            public IElement _input = null;

            public static string @property = "property";
            public IElement _property = null;

            public static string @value = "value";
            public IElement _value = null;

            public static string @comparisonMode = "comparisonMode";
            public IElement _comparisonMode = null;

            public static string @name = "name";
            public IElement _name = null;

        }

        public _FilterPropertyNode @FilterPropertyNode = new _FilterPropertyNode();
        public IElement @__FilterPropertyNode = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.FilterPropertyNode");

        public class _FilterTypeNode
        {
            public static string @input = "input";
            public IElement _input = null;

            public static string @type = "type";
            public IElement _type = null;

            public static string @includeInherits = "includeInherits";
            public IElement _includeInherits = null;

            public static string @name = "name";
            public IElement _name = null;

        }

        public _FilterTypeNode @FilterTypeNode = new _FilterTypeNode();
        public IElement @__FilterTypeNode = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.FilterTypeNode");

        public class _ComparisonMode
        {
            public static string @Equal = "Equal";
            public IElement @__Equal = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-Equal");
            public static string @NotEqual = "NotEqual";
            public IElement @__NotEqual = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-NotEqual");
            public static string @Contains = "Contains";
            public IElement @__Contains = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-Contains");
            public static string @DoesNotContain = "DoesNotContain";
            public IElement @__DoesNotContain = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-DoesNotContain");
            public static string @GreaterThan = "GreaterThan";
            public IElement @__GreaterThan = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-GreaterThan");
            public static string @GreaterOrEqualThan = "GreaterOrEqualThan";
            public IElement @__GreaterOrEqualThan = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-GreaterOrEqualThan");
            public static string @LighterThan = "LighterThan";
            public IElement @__LighterThan = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-LighterThan");
            public static string @LighterOrEqualThan = "LighterOrEqualThan";
            public IElement @__LighterOrEqualThan = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode-LighterOrEqualThan");

        }

        public _ComparisonMode @ComparisonMode = new _ComparisonMode();
        public IElement @__ComparisonMode = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.ComparisonMode");

        public class _SelectPathNode
        {
            public static string @input = "input";
            public IElement _input = null;

            public static string @path = "path";
            public IElement _path = null;

            public static string @name = "name";
            public IElement _name = null;

        }

        public _SelectPathNode @SelectPathNode = new _SelectPathNode();
        public IElement @__SelectPathNode = new MofObjectShadow("datenmeister:///_internal/types/internal#DatenMeister.Modules.DataViews.Model.SelectPathNode");

        public static _DataViews TheOne = new _DataViews();

    }

}
