#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models.Reports
{
    public class _Reports
    {
        public class _ReportDefinition
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @title = "title";
            public IElement? @_title = null;

            public static string @elements = "elements";
            public IElement? @_elements = null;

        }

        public _ReportDefinition @ReportDefinition = new _ReportDefinition();
        public IElement @__ReportDefinition = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition");

        public class _ReportElement
        {
            public static string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportElement @ReportElement = new _ReportElement();
        public IElement @__ReportElement = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement");

        public class _ReportHeadline
        {
            public static string @title = "title";
            public IElement? @_title = null;

            public static string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportHeadline @ReportHeadline = new _ReportHeadline();
        public IElement @__ReportHeadline = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline");

        public class _ReportParagraph
        {
            public static string @paragraph = "paragraph";
            public IElement? @_paragraph = null;

            public static string @cssClass = "cssClass";
            public IElement? @_cssClass = null;

            public static string @evalParagraph = "evalParagraph";
            public IElement? @_evalParagraph = null;

            public static string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static string @evalProperties = "evalProperties";
            public IElement? @_evalProperties = null;

            public static string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportParagraph @ReportParagraph = new _ReportParagraph();
        public IElement @__ReportParagraph = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph");

        public class _ReportTable
        {
            public static string @cssClass = "cssClass";
            public IElement? @_cssClass = null;

            public static string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static string @form = "form";
            public IElement? @_form = null;

            public static string @evalProperties = "evalProperties";
            public IElement? @_evalProperties = null;

            public static string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportTable @ReportTable = new _ReportTable();
        public IElement @__ReportTable = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable");

        public class _ReportInstanceSource
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static string @source = "source";
            public IElement? @_source = null;

        }

        public _ReportInstanceSource @ReportInstanceSource = new _ReportInstanceSource();
        public IElement @__ReportInstanceSource = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource");

        public class _HtmlReportInstance
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @reportDefinition = "reportDefinition";
            public IElement? @_reportDefinition = null;

            public static string @sources = "sources";
            public IElement? @_sources = null;

        }

        public _HtmlReportInstance @HtmlReportInstance = new _HtmlReportInstance();
        public IElement @__HtmlReportInstance = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.HtmlReportInstance");

        public class _DescendentMode
        {
            public static string @None = "None";
            public IElement @__None = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode-None");
            public static string @Inline = "Inline";
            public IElement @__Inline = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode-Inline");
            public static string @PerPackage = "PerPackage";
            public IElement @__PerPackage = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode-PerPackage");

        }

        public _DescendentMode @DescendentMode = new _DescendentMode();
        public IElement @__DescendentMode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode");

        public class _ReportTableForTypeMode
        {
            public static string @PerType = "PerType";
            public IElement @__PerType = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode-PerType");
            public static string @AllTypes = "AllTypes";
            public IElement @__AllTypes = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode-AllTypes");

        }

        public _ReportTableForTypeMode @ReportTableForTypeMode = new _ReportTableForTypeMode();
        public IElement @__ReportTableForTypeMode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode");

        public class _SimpleReportConfiguration
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @showDescendents = "showDescendents";
            public IElement? @_showDescendents = null;

            public static string @rootElement = "rootElement";
            public IElement? @_rootElement = null;

            public static string @showRootElement = "showRootElement";
            public IElement? @_showRootElement = null;

            public static string @showMetaClasses = "showMetaClasses";
            public IElement? @_showMetaClasses = null;

            public static string @showFullName = "showFullName";
            public IElement? @_showFullName = null;

            public static string @form = "form";
            public IElement? @_form = null;

            public static string @descendentMode = "descendentMode";
            public IElement? @_descendentMode = null;

            public static string @typeMode = "typeMode";
            public IElement? @_typeMode = null;

        }

        public _SimpleReportConfiguration @SimpleReportConfiguration = new _SimpleReportConfiguration();
        public IElement @__SimpleReportConfiguration = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration");

        public static readonly _Reports TheOne = new _Reports();

    }

}
