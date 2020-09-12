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
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#dc761a1a-351d-488f-afd2-de7ee8630b45");

            public static string @title = "title";
            public IElement? _title = new MofObjectShadow("dm:///_internal/types/internal#32b9e343-de03-49f9-ad3c-0a982553ab2f");

            public static string @elements = "elements";
            public IElement? _elements = new MofObjectShadow("dm:///_internal/types/internal#3dc78856-f2e7-4d6d-b7d3-bdfff57b5133");

        }

        public _ReportDefinition @ReportDefinition = new _ReportDefinition();
        public IElement @__ReportDefinition = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition");

        public class _ReportElement
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#aa15cb71-46a2-49eb-abec-35977d1bb0d4");

        }

        public _ReportElement @ReportElement = new _ReportElement();
        public IElement @__ReportElement = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement");

        public class _ReportHeadline
        {
            public static string @title = "title";
            public IElement? _title = new MofObjectShadow("dm:///_internal/types/internal#5fb3d338-abee-4e0b-8a56-5dc597b6b85c");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#328a2d7a-7c67-4e35-8837-8e738f789927");

        }

        public _ReportHeadline @ReportHeadline = new _ReportHeadline();
        public IElement @__ReportHeadline = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline");

        public class _ReportParagraph
        {
            public static string @paragraph = "paragraph";
            public IElement? _paragraph = new MofObjectShadow("dm:///_internal/types/internal#346a321d-f010-4748-b427-d72fd14fa613");

            public static string @cssClass = "cssClass";
            public IElement? _cssClass = new MofObjectShadow("dm:///_internal/types/internal#91d5f61e-cf03-40f4-b413-4866a2207bfb");

            public static string @evalParagraph = "evalParagraph";
            public IElement? _evalParagraph = new MofObjectShadow("dm:///_internal/types/internal#0c259bd9-476b-4698-aa85-d763b3f28681");

            public static string @viewNode = "viewNode";
            public IElement? _viewNode = new MofObjectShadow("dm:///_internal/types/internal#2c0f323d-38df-4789-98da-6f28cf9cdded");

            public static string @evalProperties = "evalProperties";
            public IElement? _evalProperties = new MofObjectShadow("dm:///_internal/types/internal#074bf4c0-16b6-461a-b68d-bad6439324ea");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#02f5ac0b-dec3-4ba5-90d2-9e59efbf57ac");

        }

        public _ReportParagraph @ReportParagraph = new _ReportParagraph();
        public IElement @__ReportParagraph = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph");

        public class _ReportTable
        {
            public static string @cssClass = "cssClass";
            public IElement? _cssClass = new MofObjectShadow("dm:///_internal/types/internal#ec7a801b-e6ef-4e39-bed7-eb1e9bfbcbc2");

            public static string @viewNode = "viewNode";
            public IElement? _viewNode = new MofObjectShadow("dm:///_internal/types/internal#6699b6fb-fc55-4f99-8561-63081f0ea7a2");

            public static string @form = "form";
            public IElement? _form = new MofObjectShadow("dm:///_internal/types/internal#17dba98c-91e7-4ab6-badf-72bfdfcf89d2");

            public static string @evalProperties = "evalProperties";
            public IElement? _evalProperties = new MofObjectShadow("dm:///_internal/types/internal#8769d280-d4f8-4030-8557-aeb4a6de4544");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#44217769-5c2b-46da-a040-d45e0bcb8bba");

        }

        public _ReportTable @ReportTable = new _ReportTable();
        public IElement @__ReportTable = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable");

        public class _ReportInstanceSource
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#8cd13537-6cde-4768-8f52-db4285871339");

            public static string @workspaceId = "workspaceId";
            public IElement? _workspaceId = new MofObjectShadow("dm:///_internal/types/internal#f8167e14-b35e-47b9-a66e-4489123b957b");

            public static string @source = "source";
            public IElement? _source = new MofObjectShadow("dm:///_internal/types/internal#3cee5971-43f1-4e7c-894a-c39435cbd166");

        }

        public _ReportInstanceSource @ReportInstanceSource = new _ReportInstanceSource();
        public IElement @__ReportInstanceSource = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource");

        public class _HtmlReportInstance
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#ae83d465-7da3-476d-9c45-669ffda41978");

            public static string @reportDefinition = "reportDefinition";
            public IElement? _reportDefinition = new MofObjectShadow("dm:///_internal/types/internal#a3972e81-9eab-40e4-91dd-a34f86a4e94f");

            public static string @sources = "sources";
            public IElement? _sources = new MofObjectShadow("dm:///_internal/types/internal#b8aed39b-9e15-448d-96ae-8764e829d847");

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
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#e54e2ba8-8119-4674-942d-3ce7afdb792e");

            public static string @showDescendents = "showDescendents";
            public IElement? _showDescendents = new MofObjectShadow("dm:///_internal/types/internal#b9d54c96-25f2-43bf-b322-c84e3c4e5d6e");

            public static string @rootElement = "rootElement";
            public IElement? _rootElement = new MofObjectShadow("dm:///_internal/types/internal#bd6c0962-5f5a-4e50-89b1-b31babd20197");

            public static string @showRootElement = "showRootElement";
            public IElement? _showRootElement = new MofObjectShadow("dm:///_internal/types/internal#d88c6053-3dea-4e6e-bae9-7a2c416009df");

            public static string @showMetaClasses = "showMetaClasses";
            public IElement? _showMetaClasses = new MofObjectShadow("dm:///_internal/types/internal#925eff5f-8733-4ae2-bd05-2cdbd94a343b");

            public static string @showFullName = "showFullName";
            public IElement? _showFullName = new MofObjectShadow("dm:///_internal/types/internal#83f02786-7c16-44be-9cbb-46329973ea0b");

            public static string @form = "form";
            public IElement? _form = new MofObjectShadow("dm:///_internal/types/internal#8b882371-5e9c-4e49-a1fd-c99c4ce95b08");

            public static string @descendentMode = "descendentMode";
            public IElement? _descendentMode = new MofObjectShadow("dm:///_internal/types/internal#b74ca9b1-5b13-47bd-8d59-f5c1eb50b1b6");

            public static string @typeMode = "typeMode";
            public IElement? _typeMode = new MofObjectShadow("dm:///_internal/types/internal#f8afa68b-d054-4853-9ca9-1e40150dfff6");

        }

        public _SimpleReportConfiguration @SimpleReportConfiguration = new _SimpleReportConfiguration();
        public IElement @__SimpleReportConfiguration = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration");

        public static _Reports TheOne = new _Reports();

    }

}
