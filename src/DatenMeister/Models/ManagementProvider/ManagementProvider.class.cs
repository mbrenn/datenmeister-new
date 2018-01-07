using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Provider.ManagementProviders.Model
{
    public class _ManagementProvider
    {
        public class _Extent
        {
            public static string @uri = "uri";
            public IElement _uri = null;

            public static string @count = "count";
            public IElement _count = null;

            public static string @type = "type";
            public IElement _type = null;

            public static string @extentType = "extentType";
            public IElement _extentType = null;

            public static string @alternativeUris = "alternativeUris";
            public IElement _alternativeUris = null;

        }

        public _Extent @Extent = new _Extent();
        public IElement @__Extent = new MofObjectShadow("dm:///DatenMeister/Types/ManagementProvider#DatenMeister.Provider.ManagementProviders.Model.Extent");

        public class _Workspace
        {
            public static string @id = "id";
            public IElement _id = null;

            public static string @annotation = "annotation";
            public IElement _annotation = null;

        }

        public _Workspace @Workspace = new _Workspace();
        public IElement @__Workspace = new MofObjectShadow("dm:///DatenMeister/Types/ManagementProvider#DatenMeister.Provider.ManagementProviders.Model.Workspace");

        public static _ManagementProvider TheOne = new _ManagementProvider();

    }

}
