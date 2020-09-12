#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models.DataViews
{
    public class _DataViews
    {
        public class _DataView
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#e1ada3ca-28d4-4d13-9c15-6d4c52e39c35");

            public static string @workspace = "workspace";
            public IElement? _workspace = new MofObjectShadow("dm:///_internal/types/internal#33979609-c766-43ec-96fe-96cf77117149");

            public static string @uri = "uri";
            public IElement? _uri = new MofObjectShadow("dm:///_internal/types/internal#8df840c5-3b69-40f4-9480-8f020f916640");

            public static string @viewNode = "viewNode";
            public IElement? _viewNode = new MofObjectShadow("dm:///_internal/types/internal#853b581a-df3c-47b5-a029-f3a45ff48ab7");

        }

        public _DataView @DataView = new _DataView();
        public IElement @__DataView = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView");

        public class _ViewNode
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#ddbc89e7-ff5e-4a1a-acaf-dbeeeb653c90");

        }

        public _ViewNode @ViewNode = new _ViewNode();
        public IElement @__ViewNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode");

        public class _SourceExtentNode
        {
            public static string @extentUri = "extentUri";
            public IElement? _extentUri = new MofObjectShadow("dm:///_internal/types/internal#3539b63c-87a4-462c-b461-2b7de460b89a");

            public static string @workspace = "workspace";
            public IElement? _workspace = new MofObjectShadow("dm:///_internal/types/internal#753672d7-f3e0-4cd0-8a55-3ee34892988b");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#f7a547eb-6500-423e-8140-68e6d8d57ff2");

        }

        public _SourceExtentNode @SourceExtentNode = new _SourceExtentNode();
        public IElement @__SourceExtentNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SourceExtentNode");

        public class _FlattenNode
        {
            public static string @input = "input";
            public IElement? _input = new MofObjectShadow("dm:///_internal/types/internal#dc23f353-364c-4ebd-addb-3485c76625cc");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#7650fa20-5dc3-4dd9-ba7e-5413030858c1");

        }

        public _FlattenNode @FlattenNode = new _FlattenNode();
        public IElement @__FlattenNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode");

        public class _FilterPropertyNode
        {
            public static string @input = "input";
            public IElement? _input = new MofObjectShadow("dm:///_internal/types/internal#4f5f1ac1-67c3-4794-a04b-ed4890c3db76");

            public static string @property = "property";
            public IElement? _property = new MofObjectShadow("dm:///_internal/types/internal#b850db4c-26a0-46c6-a85a-e0b287c17d85");

            public static string @value = "value";
            public IElement? _value = new MofObjectShadow("dm:///_internal/types/internal#1278600c-6643-4379-9b5f-cf66b4023a18");

            public static string @comparisonMode = "comparisonMode";
            public IElement? _comparisonMode = new MofObjectShadow("dm:///_internal/types/internal#1e4c6606-4906-48b9-a6d6-cff805365b65");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#508a8fc3-354c-46a7-a92b-e215d49b374f");

        }

        public _FilterPropertyNode @FilterPropertyNode = new _FilterPropertyNode();
        public IElement @__FilterPropertyNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterPropertyNode");

        public class _FilterTypeNode
        {
            public static string @input = "input";
            public IElement? _input = new MofObjectShadow("dm:///_internal/types/internal#86b6c67b-0f83-43d7-bb8d-8a19c14d0f44");

            public static string @type = "type";
            public IElement? _type = new MofObjectShadow("dm:///_internal/types/internal#e2dd3806-f39b-4267-ada7-593330e46a7a");

            public static string @includeInherits = "includeInherits";
            public IElement? _includeInherits = new MofObjectShadow("dm:///_internal/types/internal#ade8fbe1-d45c-4acf-be45-bd9596a2d005");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#a55971f3-47bf-4e93-923c-252cabbdfe9f");

        }

        public _FilterTypeNode @FilterTypeNode = new _FilterTypeNode();
        public IElement @__FilterTypeNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterTypeNode");

        public class _ComparisonMode
        {
            public static string @Equal = "Equal";
            public IElement @__Equal = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-Equal");
            public static string @NotEqual = "NotEqual";
            public IElement @__NotEqual = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-NotEqual");
            public static string @Contains = "Contains";
            public IElement @__Contains = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-Contains");
            public static string @DoesNotContain = "DoesNotContain";
            public IElement @__DoesNotContain = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-DoesNotContain");
            public static string @GreaterThan = "GreaterThan";
            public IElement @__GreaterThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-GreaterThan");
            public static string @GreaterOrEqualThan = "GreaterOrEqualThan";
            public IElement @__GreaterOrEqualThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-GreaterOrEqualThan");
            public static string @LighterThan = "LighterThan";
            public IElement @__LighterThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-LighterThan");
            public static string @LighterOrEqualThan = "LighterOrEqualThan";
            public IElement @__LighterOrEqualThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-LighterOrEqualThan");

        }

        public _ComparisonMode @ComparisonMode = new _ComparisonMode();
        public IElement @__ComparisonMode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode");

        public class _SelectByFullNameNode
        {
            public static string @input = "input";
            public IElement? _input = new MofObjectShadow("dm:///_internal/types/internal#f40704d5-e765-41dc-8e30-b37662eaa7bf");

            public static string @path = "path";
            public IElement? _path = new MofObjectShadow("dm:///_internal/types/internal#31103c8a-a205-4202-ab54-86878aed155d");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#ac95639b-1e20-4626-b026-4bca7d055e3b");

        }

        public _SelectByFullNameNode @SelectByFullNameNode = new _SelectByFullNameNode();
        public IElement @__SelectByFullNameNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode");

        public class _DynamicSourceNode
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#46b405d8-992b-4e2d-86f3-d8e2d0a9d340");

        }

        public _DynamicSourceNode @DynamicSourceNode = new _DynamicSourceNode();
        public IElement @__DynamicSourceNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode");

        public static _DataViews TheOne = new _DataViews();

    }

}
