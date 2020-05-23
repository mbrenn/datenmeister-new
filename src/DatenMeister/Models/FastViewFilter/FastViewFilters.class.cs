#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models.FastViewFilter
{
    public class _FastViewFilters
    {
        public class _ComparisonType
        {
            public static string @Equal = "Equal";
            public IElement @__Equal = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-Equal");
            public static string @GreaterThan = "GreaterThan";
            public IElement @__GreaterThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-GreaterThan");
            public static string @LighterThan = "LighterThan";
            public IElement @__LighterThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-LighterThan");
            public static string @GreaterOrEqualThan = "GreaterOrEqualThan";
            public IElement @__GreaterOrEqualThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-GreaterOrEqualThan");
            public static string @LighterOrEqualThan = "LighterOrEqualThan";
            public IElement @__LighterOrEqualThan = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-LighterOrEqualThan");

        }

        public _ComparisonType @ComparisonType = new _ComparisonType();
        public IElement @__ComparisonType = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType");

        public class _PropertyComparisonFilter
        {
            public static string @Property = "Property";
            public IElement? _Property = null;

            public static string @ComparisonType = "ComparisonType";
            public IElement? _ComparisonType = null;

            public static string @Value = "Value";
            public IElement? _Value = null;

        }

        public _PropertyComparisonFilter @PropertyComparisonFilter = new _PropertyComparisonFilter();
        public IElement @__PropertyComparisonFilter = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter");

        public class _PropertyContainsFilter
        {
            public static string @Property = "Property";
            public IElement? _Property = null;

            public static string @Value = "Value";
            public IElement? _Value = null;

        }

        public _PropertyContainsFilter @PropertyContainsFilter = new _PropertyContainsFilter();
        public IElement @__PropertyContainsFilter = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter");

        public static _FastViewFilters TheOne = new _FastViewFilters();

    }

}
