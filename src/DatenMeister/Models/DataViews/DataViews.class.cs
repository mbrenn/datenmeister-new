using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

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

            public static string @ViewNode = "ViewNode";
            public IElement _ViewNode = null;

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

        public static _DataViews TheOne = new _DataViews();

    }

}
