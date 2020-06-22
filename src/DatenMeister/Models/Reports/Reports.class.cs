#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models.Reports
{
    public class _Reports
    {
        public class _ReportDefinition
        {
            public static string @name = "name";
            public IElement? _name = null;

            public static string @title = "title";
            public IElement? _title = null;

            public static string @elements = "elements";
            public IElement? _elements = null;

        }

        public _ReportDefinition @ReportDefinition = new _ReportDefinition();
        public IElement @__ReportDefinition = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition");

        public class _ReportElement
        {
            public static string @name = "name";
            public IElement? _name = null;

        }

        public _ReportElement @ReportElement = new _ReportElement();
        public IElement @__ReportElement = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement");

        public class _ReportHeadline
        {
            public static string @title = "title";
            public IElement? _title = null;

            public static string @name = "name";
            public IElement? _name = null;

        }

        public _ReportHeadline @ReportHeadline = new _ReportHeadline();
        public IElement @__ReportHeadline = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline");

        public class _ReportParagraph
        {
            public static string @paragraph = "paragraph";
            public IElement? _paragraph = null;

            public static string @name = "name";
            public IElement? _name = null;

        }

        public _ReportParagraph @ReportParagraph = new _ReportParagraph();
        public IElement @__ReportParagraph = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph");

        public class _ReportTable
        {
            public static string @viewNode = "viewNode";
            public IElement? _viewNode = null;

            public static string @form = "form";
            public IElement? _form = null;

            public static string @name = "name";
            public IElement? _name = null;

        }

        public _ReportTable @ReportTable = new _ReportTable();
        public IElement @__ReportTable = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable");

        public static _Reports TheOne = new _Reports();

    }

}
