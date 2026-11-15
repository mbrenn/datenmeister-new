using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Core.Models;

public class _CommonTypes
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DateTime",
        TypeKind = TypeKind.ClassTree)]
    public class _DateTime
    {
    }

    public _DateTime @DateTime = new ();
    public MofObjectShadow @__DateTime = new ("dm:///_internal/types/internal#DateTime");

    public class _Default
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package",
            TypeKind = TypeKind.ClassTree)]
        public class _Package
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @packagedElement = "packagedElement";
            public IElement? @_packagedElement = null;

            public static readonly string @preferredType = "preferredType";
            public IElement? @_preferredType = null;

            public static readonly string @preferredPackage = "preferredPackage";
            public IElement? @_preferredPackage = null;

            public static readonly string @defaultViewMode = "defaultViewMode";
            public IElement? @_defaultViewMode = null;

        }

        public _Package @Package = new ();
        public MofObjectShadow @__Package = new ("dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package");

        [TypeUri(Uri = "dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1",
            TypeKind = TypeKind.ClassTree)]
        public class _XmiExportContainer
        {
            public static readonly string @xmi = "xmi";
            public IElement? @_xmi = null;

        }

        public _XmiExportContainer @XmiExportContainer = new ();
        public MofObjectShadow @__XmiExportContainer = new ("dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1");

        [TypeUri(Uri = "dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a",
            TypeKind = TypeKind.ClassTree)]
        public class _XmiImportContainer
        {
            public static readonly string @xmi = "xmi";
            public IElement? @_xmi = null;

            public static readonly string @property = "property";
            public IElement? @_property = null;

            public static readonly string @addToCollection = "addToCollection";
            public IElement? @_addToCollection = null;

        }

        public _XmiImportContainer @XmiImportContainer = new ();
        public MofObjectShadow @__XmiImportContainer = new ("dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a");

    }

    public _Default Default = new ();

    public class _ExtentManager
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings",
            TypeKind = TypeKind.ClassTree)]
        public class _ImportSettings
        {
            public static readonly string @filePath = "filePath";
            public IElement? @_filePath = null;

            public static readonly string @extentUri = "extentUri";
            public IElement? @_extentUri = null;

            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

        }

        public _ImportSettings @ImportSettings = new ();
        public MofObjectShadow @__ImportSettings = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException",
            TypeKind = TypeKind.ClassTree)]
        public class _ImportException
        {
            public static readonly string @message = "message";
            public IElement? @_message = null;

        }

        public _ImportException @ImportException = new ();
        public MofObjectShadow @__ImportException = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException");

    }

    public _ExtentManager ExtentManager = new ();

    public class _OSIntegration
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication",
            TypeKind = TypeKind.ClassTree)]
        public class _CommandLineApplication
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @applicationPath = "applicationPath";
            public IElement? @_applicationPath = null;

        }

        public _CommandLineApplication @CommandLineApplication = new ();
        public MofObjectShadow @__CommandLineApplication = new ("dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication");

        [TypeUri(Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable",
            TypeKind = TypeKind.ClassTree)]
        public class _EnvironmentalVariable
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @value = "value";
            public IElement? @_value = null;

        }

        public _EnvironmentalVariable @EnvironmentalVariable = new ();
        public MofObjectShadow @__EnvironmentalVariable = new ("dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable");

    }

    public _OSIntegration OSIntegration = new ();

    public static readonly _CommonTypes TheOne = new ();

}

public class _DataViews
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView",
        TypeKind = TypeKind.ClassTree)]
    public class _DataView
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @uri = "uri";
        public IElement? @_uri = null;

        public static readonly string @viewNode = "viewNode";
        public IElement? @_viewNode = null;

    }

    public _DataView @DataView = new ();
    public MofObjectShadow @__DataView = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode",
        TypeKind = TypeKind.ClassTree)]
    public class _ViewNode
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

    }

    public _ViewNode @ViewNode = new ();
    public MofObjectShadow @__ViewNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode");

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
        public static string @RegexMatch = "RegexMatch";
        public IElement? @__RegexMatch = null;
        public static string @RegexNoMatch = "RegexNoMatch";
        public IElement? @__RegexNoMatch = null;

    }

    public _ComparisonMode @ComparisonMode = new _ComparisonMode();
    public IElement @__ComparisonMode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode");


    public enum ___ComparisonMode
    {
        @Equal,
        @NotEqual,
        @Contains,
        @DoesNotContain,
        @GreaterThan,
        @GreaterOrEqualThan,
        @LighterThan,
        @LighterOrEqualThan,
        @RegexMatch,
        @RegexNoMatch
    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement",
        TypeKind = TypeKind.ClassTree)]
    public class _QueryStatement
    {
        public static readonly string @nodes = "nodes";
        public IElement? @_nodes = null;

        public static readonly string @resultNode = "resultNode";
        public IElement? @_resultNode = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

    }

    public _QueryStatement @QueryStatement = new ();
    public MofObjectShadow @__QueryStatement = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement");

    public class _Row
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#5f66ff9a-0a68-4c87-856b-5921c7cae628",
            TypeKind = TypeKind.ClassTree)]
        public class _RowFilterByFreeTextAnywhere
        {
            public static readonly string @freeText = "freeText";
            public IElement? @_freeText = null;

            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @propertyName = "propertyName";
            public IElement? @_propertyName = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RowFilterByFreeTextAnywhere @RowFilterByFreeTextAnywhere = new ();
        public MofObjectShadow @__RowFilterByFreeTextAnywhere = new ("dm:///_internal/types/internal#5f66ff9a-0a68-4c87-856b-5921c7cae628");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode",
            TypeKind = TypeKind.ClassTree)]
        public class _RowFilterByPropertyValueNode
        {
            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @property = "property";
            public IElement? @_property = null;

            public static readonly string @value = "value";
            public IElement? @_value = null;

            public static readonly string @comparisonMode = "comparisonMode";
            public IElement? @_comparisonMode = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RowFilterByPropertyValueNode @RowFilterByPropertyValueNode = new ();
        public MofObjectShadow @__RowFilterByPropertyValueNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode");

        [TypeUri(Uri = "dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9",
            TypeKind = TypeKind.ClassTree)]
        public class _RowOrderByNode
        {
            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @propertyName = "propertyName";
            public IElement? @_propertyName = null;

            public static readonly string @orderDescending = "orderDescending";
            public IElement? @_orderDescending = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RowOrderByNode @RowOrderByNode = new ();
        public MofObjectShadow @__RowOrderByNode = new ("dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9");

        [TypeUri(Uri = "dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f",
            TypeKind = TypeKind.ClassTree)]
        public class _RowFilterOnPositionNode
        {
            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @amount = "amount";
            public IElement? @_amount = null;

            public static readonly string @position = "position";
            public IElement? @_position = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RowFilterOnPositionNode @RowFilterOnPositionNode = new ();
        public MofObjectShadow @__RowFilterOnPositionNode = new ("dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode",
            TypeKind = TypeKind.ClassTree)]
        public class _RowFlattenNode
        {
            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RowFlattenNode @RowFlattenNode = new ();
        public MofObjectShadow @__RowFlattenNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode",
            TypeKind = TypeKind.ClassTree)]
        public class _RowFilterByMetaclassNode
        {
            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @metaClass = "metaClass";
            public IElement? @_metaClass = null;

            public static readonly string @includeInherits = "includeInherits";
            public IElement? @_includeInherits = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RowFilterByMetaclassNode @RowFilterByMetaclassNode = new ();
        public MofObjectShadow @__RowFilterByMetaclassNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode");

    }

    public _Row Row = new ();

    public class _Column
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d",
            TypeKind = TypeKind.ClassTree)]
        public class _ColumnFilterIncludeOnlyNode
        {
            public static readonly string @columnNamesComma = "columnNamesComma";
            public IElement? @_columnNamesComma = null;

            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ColumnFilterIncludeOnlyNode @ColumnFilterIncludeOnlyNode = new ();
        public MofObjectShadow @__ColumnFilterIncludeOnlyNode = new ("dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d");

        [TypeUri(Uri = "dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7",
            TypeKind = TypeKind.ClassTree)]
        public class _ColumnFilterExcludeNode
        {
            public static readonly string @columnNamesComma = "columnNamesComma";
            public IElement? @_columnNamesComma = null;

            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ColumnFilterExcludeNode @ColumnFilterExcludeNode = new ();
        public MofObjectShadow @__ColumnFilterExcludeNode = new ("dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7");

    }

    public _Column Column = new ();

    public class _Source
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode",
            TypeKind = TypeKind.ClassTree)]
        public class _SelectByExtentNode
        {
            public static readonly string @extentUri = "extentUri";
            public IElement? @_extentUri = null;

            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _SelectByExtentNode @SelectByExtentNode = new ();
        public MofObjectShadow @__SelectByExtentNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode",
            TypeKind = TypeKind.ClassTree)]
        public class _SelectByPathNode
        {
            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @path = "path";
            public IElement? @_path = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _SelectByPathNode @SelectByPathNode = new ();
        public MofObjectShadow @__SelectByPathNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode",
            TypeKind = TypeKind.ClassTree)]
        public class _DynamicSourceNode
        {
            public static readonly string @nodeName = "nodeName";
            public IElement? @_nodeName = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _DynamicSourceNode @DynamicSourceNode = new ();
        public MofObjectShadow @__DynamicSourceNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode");

        [TypeUri(Uri = "dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db",
            TypeKind = TypeKind.ClassTree)]
        public class _SelectByWorkspaceNode
        {
            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _SelectByWorkspaceNode @SelectByWorkspaceNode = new ();
        public MofObjectShadow @__SelectByWorkspaceNode = new ("dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db");

        [TypeUri(Uri = "dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a",
            TypeKind = TypeKind.ClassTree)]
        public class _SelectFromAllWorkspacesNode
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _SelectFromAllWorkspacesNode @SelectFromAllWorkspacesNode = new ();
        public MofObjectShadow @__SelectFromAllWorkspacesNode = new ("dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a");

    }

    public _Source Source = new ();

    public class _Node
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#e80d4c64-a68e-44a7-893d-1a5100a80370",
            TypeKind = TypeKind.ClassTree)]
        public class _ReferenceViewNode
        {
            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @itemUri = "itemUri";
            public IElement? @_itemUri = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ReferenceViewNode @ReferenceViewNode = new ();
        public MofObjectShadow @__ReferenceViewNode = new ("dm:///_internal/types/internal#e80d4c64-a68e-44a7-893d-1a5100a80370");

    }

    public _Node Node = new ();

    [TypeUri(Uri = "dm:///_internal/types/internal#4394a28a-0def-4030-b5d0-7a1b5b01c91b",
        TypeKind = TypeKind.ClassTree)]
    public class _ValueItem
    {
        public static readonly string @value = "value";
        public IElement? @_value = null;

    }

    public _ValueItem @ValueItem = new ();
    public MofObjectShadow @__ValueItem = new ("dm:///_internal/types/internal#4394a28a-0def-4030-b5d0-7a1b5b01c91b");

    public class _Transformation
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode",
            TypeKind = TypeKind.ClassTree)]
        public class _SelectByFullNameNode
        {
            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @path = "path";
            public IElement? @_path = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _SelectByFullNameNode @SelectByFullNameNode = new ();
        public MofObjectShadow @__SelectByFullNameNode = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode");

        [TypeUri(Uri = "dm:///_internal/types/internal#2a5ac57c-e5d2-498b-b575-7b07354e2645",
            TypeKind = TypeKind.ClassTree)]
        public class _SelectByProperty
        {
            public static readonly string @input = "input";
            public IElement? @_input = null;

            public static readonly string @propertyName = "propertyName";
            public IElement? @_propertyName = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _SelectByProperty @SelectByProperty = new ();
        public MofObjectShadow @__SelectByProperty = new ("dm:///_internal/types/internal#2a5ac57c-e5d2-498b-b575-7b07354e2645");

    }

    public _Transformation Transformation = new ();

    public static readonly _DataViews TheOne = new ();

}

public class _Reports
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition",
        TypeKind = TypeKind.ClassTree)]
    public class _ReportDefinition
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @title = "title";
        public IElement? @_title = null;

        public static readonly string @elements = "elements";
        public IElement? @_elements = null;

    }

    public _ReportDefinition @ReportDefinition = new ();
    public MofObjectShadow @__ReportDefinition = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource",
        TypeKind = TypeKind.ClassTree)]
    public class _ReportInstanceSource
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @path = "path";
        public IElement? @_path = null;

    }

    public _ReportInstanceSource @ReportInstanceSource = new ();
    public MofObjectShadow @__ReportInstanceSource = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance",
        TypeKind = TypeKind.ClassTree)]
    public class _ReportInstance
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @reportDefinition = "reportDefinition";
        public IElement? @_reportDefinition = null;

        public static readonly string @sources = "sources";
        public IElement? @_sources = null;

    }

    public _ReportInstance @ReportInstance = new ();
    public MofObjectShadow @__ReportInstance = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance",
        TypeKind = TypeKind.ClassTree)]
    public class _AdocReportInstance
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @reportDefinition = "reportDefinition";
        public IElement? @_reportDefinition = null;

        public static readonly string @sources = "sources";
        public IElement? @_sources = null;

    }

    public _AdocReportInstance @AdocReportInstance = new ();
    public MofObjectShadow @__AdocReportInstance = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance",
        TypeKind = TypeKind.ClassTree)]
    public class _HtmlReportInstance
    {
        public static readonly string @cssFile = "cssFile";
        public IElement? @_cssFile = null;

        public static readonly string @cssStyleSheet = "cssStyleSheet";
        public IElement? @_cssStyleSheet = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @reportDefinition = "reportDefinition";
        public IElement? @_reportDefinition = null;

        public static readonly string @sources = "sources";
        public IElement? @_sources = null;

    }

    public _HtmlReportInstance @HtmlReportInstance = new ();
    public MofObjectShadow @__HtmlReportInstance = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance");

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


    public enum ___DescendentMode
    {
        @None,
        @Inline,
        @PerPackage
    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration",
        TypeKind = TypeKind.ClassTree)]
    public class _SimpleReportConfiguration
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @showDescendents = "showDescendents";
        public IElement? @_showDescendents = null;

        public static readonly string @rootElement = "rootElement";
        public IElement? @_rootElement = null;

        public static readonly string @showRootElement = "showRootElement";
        public IElement? @_showRootElement = null;

        public static readonly string @showMetaClasses = "showMetaClasses";
        public IElement? @_showMetaClasses = null;

        public static readonly string @showFullName = "showFullName";
        public IElement? @_showFullName = null;

        public static readonly string @form = "form";
        public IElement? @_form = null;

        public static readonly string @descendentMode = "descendentMode";
        public IElement? @_descendentMode = null;

        public static readonly string @typeMode = "typeMode";
        public IElement? @_typeMode = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

    }

    public _SimpleReportConfiguration @SimpleReportConfiguration = new ();
    public MofObjectShadow @__SimpleReportConfiguration = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration");

    public class _Elements
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement",
            TypeKind = TypeKind.ClassTree)]
        public class _ReportElement
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportElement @ReportElement = new ();
        public MofObjectShadow @__ReportElement = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline",
            TypeKind = TypeKind.ClassTree)]
        public class _ReportHeadline
        {
            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportHeadline @ReportHeadline = new ();
        public MofObjectShadow @__ReportHeadline = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph",
            TypeKind = TypeKind.ClassTree)]
        public class _ReportParagraph
        {
            public static readonly string @paragraph = "paragraph";
            public IElement? @_paragraph = null;

            public static readonly string @cssClass = "cssClass";
            public IElement? @_cssClass = null;

            public static readonly string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static readonly string @evalProperties = "evalProperties";
            public IElement? @_evalProperties = null;

            public static readonly string @evalParagraph = "evalParagraph";
            public IElement? @_evalParagraph = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportParagraph @ReportParagraph = new ();
        public MofObjectShadow @__ReportParagraph = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable",
            TypeKind = TypeKind.ClassTree)]
        public class _ReportTable
        {
            public static readonly string @cssClass = "cssClass";
            public IElement? @_cssClass = null;

            public static readonly string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static readonly string @form = "form";
            public IElement? @_form = null;

            public static readonly string @evalProperties = "evalProperties";
            public IElement? @_evalProperties = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportTable @ReportTable = new ();
        public MofObjectShadow @__ReportTable = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable");

        public class _ReportTableForTypeMode
        {
            public static string @PerType = "PerType";
            public IElement @__PerType = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode-PerType");
            public static string @AllTypes = "AllTypes";
            public IElement @__AllTypes = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode-AllTypes");

        }

        public _ReportTableForTypeMode @ReportTableForTypeMode = new _ReportTableForTypeMode();
        public IElement @__ReportTableForTypeMode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode");


        public enum ___ReportTableForTypeMode
        {
            @PerType,
            @AllTypes
        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop",
            TypeKind = TypeKind.ClassTree)]
        public class _ReportLoop
        {
            public static readonly string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static readonly string @elements = "elements";
            public IElement? @_elements = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ReportLoop @ReportLoop = new ();
        public MofObjectShadow @__ReportLoop = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop");

    }

    public _Elements Elements = new ();

    public static readonly _Reports TheOne = new ();

}

public class _ExtentLoaderConfigs
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _ExtentLoaderConfig
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _ExtentLoaderConfig @ExtentLoaderConfig = new ();
    public MofObjectShadow @__ExtentLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _ExcelLoaderConfig
    {
        public static readonly string @fixRowCount = "fixRowCount";
        public IElement? @_fixRowCount = null;

        public static readonly string @fixColumnCount = "fixColumnCount";
        public IElement? @_fixColumnCount = null;

        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @sheetName = "sheetName";
        public IElement? @_sheetName = null;

        public static readonly string @offsetRow = "offsetRow";
        public IElement? @_offsetRow = null;

        public static readonly string @offsetColumn = "offsetColumn";
        public IElement? @_offsetColumn = null;

        public static readonly string @countRows = "countRows";
        public IElement? @_countRows = null;

        public static readonly string @countColumns = "countColumns";
        public IElement? @_countColumns = null;

        public static readonly string @hasHeader = "hasHeader";
        public IElement? @_hasHeader = null;

        public static readonly string @tryMergedHeaderCells = "tryMergedHeaderCells";
        public IElement? @_tryMergedHeaderCells = null;

        public static readonly string @onlySetColumns = "onlySetColumns";
        public IElement? @_onlySetColumns = null;

        public static readonly string @idColumnName = "idColumnName";
        public IElement? @_idColumnName = null;

        public static readonly string @skipEmptyRowsCount = "skipEmptyRowsCount";
        public IElement? @_skipEmptyRowsCount = null;

        public static readonly string @columns = "columns";
        public IElement? @_columns = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _ExcelLoaderConfig @ExcelLoaderConfig = new ();
    public MofObjectShadow @__ExcelLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _ExcelReadOnlyLoaderConfig
    {
        public static readonly string @fixRowCount = "fixRowCount";
        public IElement? @_fixRowCount = null;

        public static readonly string @fixColumnCount = "fixColumnCount";
        public IElement? @_fixColumnCount = null;

        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @sheetName = "sheetName";
        public IElement? @_sheetName = null;

        public static readonly string @offsetRow = "offsetRow";
        public IElement? @_offsetRow = null;

        public static readonly string @offsetColumn = "offsetColumn";
        public IElement? @_offsetColumn = null;

        public static readonly string @countRows = "countRows";
        public IElement? @_countRows = null;

        public static readonly string @countColumns = "countColumns";
        public IElement? @_countColumns = null;

        public static readonly string @hasHeader = "hasHeader";
        public IElement? @_hasHeader = null;

        public static readonly string @tryMergedHeaderCells = "tryMergedHeaderCells";
        public IElement? @_tryMergedHeaderCells = null;

        public static readonly string @onlySetColumns = "onlySetColumns";
        public IElement? @_onlySetColumns = null;

        public static readonly string @idColumnName = "idColumnName";
        public IElement? @_idColumnName = null;

        public static readonly string @skipEmptyRowsCount = "skipEmptyRowsCount";
        public IElement? @_skipEmptyRowsCount = null;

        public static readonly string @columns = "columns";
        public IElement? @_columns = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _ExcelReadOnlyLoaderConfig @ExcelReadOnlyLoaderConfig = new ();
    public MofObjectShadow @__ExcelReadOnlyLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _ExcelConvertToXmiOnceConfig
    {
        public static readonly string @xmiFilePath = "xmiFilePath";
        public IElement? @_xmiFilePath = null;

        public static readonly string @fixRowCount = "fixRowCount";
        public IElement? @_fixRowCount = null;

        public static readonly string @fixColumnCount = "fixColumnCount";
        public IElement? @_fixColumnCount = null;

        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @sheetName = "sheetName";
        public IElement? @_sheetName = null;

        public static readonly string @offsetRow = "offsetRow";
        public IElement? @_offsetRow = null;

        public static readonly string @offsetColumn = "offsetColumn";
        public IElement? @_offsetColumn = null;

        public static readonly string @countRows = "countRows";
        public IElement? @_countRows = null;

        public static readonly string @countColumns = "countColumns";
        public IElement? @_countColumns = null;

        public static readonly string @hasHeader = "hasHeader";
        public IElement? @_hasHeader = null;

        public static readonly string @tryMergedHeaderCells = "tryMergedHeaderCells";
        public IElement? @_tryMergedHeaderCells = null;

        public static readonly string @onlySetColumns = "onlySetColumns";
        public IElement? @_onlySetColumns = null;

        public static readonly string @idColumnName = "idColumnName";
        public IElement? @_idColumnName = null;

        public static readonly string @skipEmptyRowsCount = "skipEmptyRowsCount";
        public IElement? @_skipEmptyRowsCount = null;

        public static readonly string @columns = "columns";
        public IElement? @_columns = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _ExcelConvertToXmiOnceConfig @ExcelConvertToXmiOnceConfig = new ();
    public MofObjectShadow @__ExcelConvertToXmiOnceConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _ExcelFullSyncLoaderConfig
    {
        public static readonly string @fixRowCount = "fixRowCount";
        public IElement? @_fixRowCount = null;

        public static readonly string @fixColumnCount = "fixColumnCount";
        public IElement? @_fixColumnCount = null;

        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @sheetName = "sheetName";
        public IElement? @_sheetName = null;

        public static readonly string @offsetRow = "offsetRow";
        public IElement? @_offsetRow = null;

        public static readonly string @offsetColumn = "offsetColumn";
        public IElement? @_offsetColumn = null;

        public static readonly string @countRows = "countRows";
        public IElement? @_countRows = null;

        public static readonly string @countColumns = "countColumns";
        public IElement? @_countColumns = null;

        public static readonly string @hasHeader = "hasHeader";
        public IElement? @_hasHeader = null;

        public static readonly string @tryMergedHeaderCells = "tryMergedHeaderCells";
        public IElement? @_tryMergedHeaderCells = null;

        public static readonly string @onlySetColumns = "onlySetColumns";
        public IElement? @_onlySetColumns = null;

        public static readonly string @idColumnName = "idColumnName";
        public IElement? @_idColumnName = null;

        public static readonly string @skipEmptyRowsCount = "skipEmptyRowsCount";
        public IElement? @_skipEmptyRowsCount = null;

        public static readonly string @columns = "columns";
        public IElement? @_columns = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _ExcelFullSyncLoaderConfig @ExcelFullSyncLoaderConfig = new ();
    public MofObjectShadow @__ExcelFullSyncLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _InMemoryLoaderConfig
    {
        public static readonly string @isLinkedList = "isLinkedList";
        public IElement? @_isLinkedList = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _InMemoryLoaderConfig @InMemoryLoaderConfig = new ();
    public MofObjectShadow @__InMemoryLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _XmlReferenceLoaderConfig
    {
        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @keepNamespaces = "keepNamespaces";
        public IElement? @_keepNamespaces = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _XmlReferenceLoaderConfig @XmlReferenceLoaderConfig = new ();
    public MofObjectShadow @__XmlReferenceLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _ExtentFileLoaderConfig
    {
        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _ExtentFileLoaderConfig @ExtentFileLoaderConfig = new ();
    public MofObjectShadow @__ExtentFileLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _XmiStorageLoaderConfig
    {
        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _XmiStorageLoaderConfig @XmiStorageLoaderConfig = new ();
    public MofObjectShadow @__XmiStorageLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _CsvExtentLoaderConfig
    {
        public static readonly string @settings = "settings";
        public IElement? @_settings = null;

        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _CsvExtentLoaderConfig @CsvExtentLoaderConfig = new ();
    public MofObjectShadow @__CsvExtentLoaderConfig = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings",
        TypeKind = TypeKind.ClassTree)]
    public class _CsvSettings
    {
        public static readonly string @encoding = "encoding";
        public IElement? @_encoding = null;

        public static readonly string @hasHeader = "hasHeader";
        public IElement? @_hasHeader = null;

        public static readonly string @separator = "separator";
        public IElement? @_separator = null;

        public static readonly string @columns = "columns";
        public IElement? @_columns = null;

        public static readonly string @metaclassUri = "metaclassUri";
        public IElement? @_metaclassUri = null;

        public static readonly string @trimCells = "trimCells";
        public IElement? @_trimCells = null;

    }

    public _CsvSettings @CsvSettings = new ();
    public MofObjectShadow @__CsvSettings = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings");

    [TypeUri(Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition",
        TypeKind = TypeKind.ClassTree)]
    public class _ExcelHierarchicalColumnDefinition
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @metaClass = "metaClass";
        public IElement? @_metaClass = null;

        public static readonly string @property = "property";
        public IElement? @_property = null;

    }

    public _ExcelHierarchicalColumnDefinition @ExcelHierarchicalColumnDefinition = new ();
    public MofObjectShadow @__ExcelHierarchicalColumnDefinition = new ("dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition");

    [TypeUri(Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _ExcelHierarchicalLoaderConfig
    {
        public static readonly string @hierarchicalColumns = "hierarchicalColumns";
        public IElement? @_hierarchicalColumns = null;

        public static readonly string @skipElementsForLastLevel = "skipElementsForLastLevel";
        public IElement? @_skipElementsForLastLevel = null;

        public static readonly string @fixRowCount = "fixRowCount";
        public IElement? @_fixRowCount = null;

        public static readonly string @fixColumnCount = "fixColumnCount";
        public IElement? @_fixColumnCount = null;

        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @sheetName = "sheetName";
        public IElement? @_sheetName = null;

        public static readonly string @offsetRow = "offsetRow";
        public IElement? @_offsetRow = null;

        public static readonly string @offsetColumn = "offsetColumn";
        public IElement? @_offsetColumn = null;

        public static readonly string @countRows = "countRows";
        public IElement? @_countRows = null;

        public static readonly string @countColumns = "countColumns";
        public IElement? @_countColumns = null;

        public static readonly string @hasHeader = "hasHeader";
        public IElement? @_hasHeader = null;

        public static readonly string @tryMergedHeaderCells = "tryMergedHeaderCells";
        public IElement? @_tryMergedHeaderCells = null;

        public static readonly string @onlySetColumns = "onlySetColumns";
        public IElement? @_onlySetColumns = null;

        public static readonly string @idColumnName = "idColumnName";
        public IElement? @_idColumnName = null;

        public static readonly string @skipEmptyRowsCount = "skipEmptyRowsCount";
        public IElement? @_skipEmptyRowsCount = null;

        public static readonly string @columns = "columns";
        public IElement? @_columns = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _ExcelHierarchicalLoaderConfig @ExcelHierarchicalLoaderConfig = new ();
    public MofObjectShadow @__ExcelHierarchicalLoaderConfig = new ("dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig");

    [TypeUri(Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a",
        TypeKind = TypeKind.ClassTree)]
    public class _ExcelColumn
    {
        public static readonly string @header = "header";
        public IElement? @_header = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

    }

    public _ExcelColumn @ExcelColumn = new ();
    public MofObjectShadow @__ExcelColumn = new ("dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a");

    [TypeUri(Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3",
        TypeKind = TypeKind.ClassTree)]
    public class _EnvironmentalVariableLoaderConfig
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _EnvironmentalVariableLoaderConfig @EnvironmentalVariableLoaderConfig = new ();
    public MofObjectShadow @__EnvironmentalVariableLoaderConfig = new ("dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3");

    public class _ExtentLoaderImportType
    {
        public static string @NoSync = "NoSync";
        public IElement? @__NoSync = null;
        public static string @ReadOnlySync = "ReadOnlySync";
        public IElement? @__ReadOnlySync = null;
        public static string @BidirectionalSync = "BidirectionalSync";
        public IElement? @__BidirectionalSync = null;

    }

    public _ExtentLoaderImportType @ExtentLoaderImportType = new _ExtentLoaderImportType();
    public IElement @__ExtentLoaderImportType = new MofObjectShadow("dm:///_internal/types/internal#9e879c8d-b7a0-4ef8-aaad-a0cbbf0245a9");


    public enum ___ExtentLoaderImportType
    {
        @NoSync,
        @ReadOnlySync,
        @BidirectionalSync
    }

    public static readonly _ExtentLoaderConfigs TheOne = new ();

}

public class _Forms
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder",
        TypeKind = TypeKind.ClassTree)]
    public class _SortingOrder
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDescending = "isDescending";
        public IElement? @_isDescending = null;

    }

    public _SortingOrder @SortingOrder = new ();
    public MofObjectShadow @__SortingOrder = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement",
        TypeKind = TypeKind.ClassTree)]
    public class _DefaultTypeForNewElement
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @metaClass = "metaClass";
        public IElement? @_metaClass = null;

        public static readonly string @parentProperty = "parentProperty";
        public IElement? @_parentProperty = null;

    }

    public _DefaultTypeForNewElement @DefaultTypeForNewElement = new ();
    public MofObjectShadow @__DefaultTypeForNewElement = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode",
        TypeKind = TypeKind.ClassTree)]
    public class _ViewMode
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @id = "id";
        public IElement? @_id = null;

        public static readonly string @defaultExtentType = "defaultExtentType";
        public IElement? @_defaultExtentType = null;

    }

    public _ViewMode @ViewMode = new ();
    public MofObjectShadow @__ViewMode = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode");

    [TypeUri(Uri = "dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0",
        TypeKind = TypeKind.ClassTree)]
    public class _NavigateToFieldsForTestAction
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _NavigateToFieldsForTestAction @NavigateToFieldsForTestAction = new ();
    public MofObjectShadow @__NavigateToFieldsForTestAction = new ("dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0");

    public class _FormTypes
    {
        public class _FormType
        {
            public static string @Object = "Object";
            public IElement @__Object = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-Object");
            public static string @Collection = "Collection";
            public IElement @__Collection = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-Collection");
            public static string @Row = "Row";
            public IElement @__Row = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-RowForm");
            public static string @Table = "Table";
            public IElement @__Table = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-TableForm");
            public static string @ObjectExtension = "ObjectExtension";
            public IElement @__ObjectExtension = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-ObjectExtension");
            public static string @CollectionExtension = "CollectionExtension";
            public IElement @__CollectionExtension = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-CollectionExtension");
            public static string @RowExtension = "RowExtension";
            public IElement @__RowExtension = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-RowExtension");
            public static string @TableExtension = "TableExtension";
            public IElement @__TableExtension = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-TableExtension");
            public static string @Decoupled = "Decoupled";
            public IElement? @__Decoupled = null;

        }

        public _FormType @FormType = new _FormType();
        public IElement @__FormType = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType");


        public enum ___FormType
        {
            @Object,
            @Collection,
            @Row,
            @Table,
            @ObjectExtension,
            @CollectionExtension,
            @RowExtension,
            @TableExtension,
            @Decoupled
        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form",
            TypeKind = TypeKind.ClassTree)]
        public class _Form
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

            public static readonly string @isAutoGenerated = "isAutoGenerated";
            public IElement? @_isAutoGenerated = null;

            public static readonly string @hideMetaInformation = "hideMetaInformation";
            public IElement? @_hideMetaInformation = null;

            public static readonly string @originalUri = "originalUri";
            public IElement? @_originalUri = null;

            public static readonly string @originalWorkspace = "originalWorkspace";
            public IElement? @_originalWorkspace = null;

            public static readonly string @creationProtocol = "creationProtocol";
            public IElement? @_creationProtocol = null;

            public static readonly string @hideReferencingItems = "hideReferencingItems";
            public IElement? @_hideReferencingItems = null;

        }

        public _Form @Form = new ();
        public MofObjectShadow @__Form = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.Form");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm",
            TypeKind = TypeKind.ClassTree)]
        public class _RowForm
        {
            public static readonly string @buttonApplyText = "buttonApplyText";
            public IElement? @_buttonApplyText = null;

            public static readonly string @allowNewProperties = "allowNewProperties";
            public IElement? @_allowNewProperties = null;

            public static readonly string @defaultWidth = "defaultWidth";
            public IElement? @_defaultWidth = null;

            public static readonly string @defaultHeight = "defaultHeight";
            public IElement? @_defaultHeight = null;

            public static readonly string @field = "field";
            public IElement? @_field = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

            public static readonly string @isAutoGenerated = "isAutoGenerated";
            public IElement? @_isAutoGenerated = null;

            public static readonly string @hideMetaInformation = "hideMetaInformation";
            public IElement? @_hideMetaInformation = null;

            public static readonly string @originalUri = "originalUri";
            public IElement? @_originalUri = null;

            public static readonly string @originalWorkspace = "originalWorkspace";
            public IElement? @_originalWorkspace = null;

            public static readonly string @creationProtocol = "creationProtocol";
            public IElement? @_creationProtocol = null;

            public static readonly string @hideReferencingItems = "hideReferencingItems";
            public IElement? @_hideReferencingItems = null;

        }

        public _RowForm @RowForm = new ();
        public MofObjectShadow @__RowForm = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm",
            TypeKind = TypeKind.ClassTree)]
        public class _TableForm
        {
            public static readonly string @property = "property";
            public IElement? @_property = null;

            public static readonly string @metaClass = "metaClass";
            public IElement? @_metaClass = null;

            public static readonly string @includeDescendents = "includeDescendents";
            public IElement? @_includeDescendents = null;

            public static readonly string @noItemsWithMetaClass = "noItemsWithMetaClass";
            public IElement? @_noItemsWithMetaClass = null;

            public static readonly string @inhibitNewItems = "inhibitNewItems";
            public IElement? @_inhibitNewItems = null;

            public static readonly string @inhibitDeleteItems = "inhibitDeleteItems";
            public IElement? @_inhibitDeleteItems = null;

            public static readonly string @inhibitEditItems = "inhibitEditItems";
            public IElement? @_inhibitEditItems = null;

            public static readonly string @defaultTypesForNewElements = "defaultTypesForNewElements";
            public IElement? @_defaultTypesForNewElements = null;

            public static readonly string @fastViewFilters = "fastViewFilters";
            public IElement? @_fastViewFilters = null;

            public static readonly string @field = "field";
            public IElement? @_field = null;

            public static readonly string @sortingOrder = "sortingOrder";
            public IElement? @_sortingOrder = null;

            public static readonly string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static readonly string @autoGenerateFields = "autoGenerateFields";
            public IElement? @_autoGenerateFields = null;

            public static readonly string @duplicatePerType = "duplicatePerType";
            public IElement? @_duplicatePerType = null;

            public static readonly string @dataUrl = "dataUrl";
            public IElement? @_dataUrl = null;

            public static readonly string @inhibitNewUnclassifiedItems = "inhibitNewUnclassifiedItems";
            public IElement? @_inhibitNewUnclassifiedItems = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

            public static readonly string @isAutoGenerated = "isAutoGenerated";
            public IElement? @_isAutoGenerated = null;

            public static readonly string @hideMetaInformation = "hideMetaInformation";
            public IElement? @_hideMetaInformation = null;

            public static readonly string @originalUri = "originalUri";
            public IElement? @_originalUri = null;

            public static readonly string @originalWorkspace = "originalWorkspace";
            public IElement? @_originalWorkspace = null;

            public static readonly string @creationProtocol = "creationProtocol";
            public IElement? @_creationProtocol = null;

            public static readonly string @hideReferencingItems = "hideReferencingItems";
            public IElement? @_hideReferencingItems = null;

        }

        public _TableForm @TableForm = new ();
        public MofObjectShadow @__TableForm = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm",
            TypeKind = TypeKind.ClassTree)]
        public class _CollectionForm
        {
            public static readonly string @tab = "tab";
            public IElement? @_tab = null;

            public static readonly string @autoTabs = "autoTabs";
            public IElement? @_autoTabs = null;

            public static readonly string @field = "field";
            public IElement? @_field = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

            public static readonly string @isAutoGenerated = "isAutoGenerated";
            public IElement? @_isAutoGenerated = null;

            public static readonly string @hideMetaInformation = "hideMetaInformation";
            public IElement? @_hideMetaInformation = null;

            public static readonly string @originalUri = "originalUri";
            public IElement? @_originalUri = null;

            public static readonly string @originalWorkspace = "originalWorkspace";
            public IElement? @_originalWorkspace = null;

            public static readonly string @creationProtocol = "creationProtocol";
            public IElement? @_creationProtocol = null;

            public static readonly string @hideReferencingItems = "hideReferencingItems";
            public IElement? @_hideReferencingItems = null;

        }

        public _CollectionForm @CollectionForm = new ();
        public MofObjectShadow @__CollectionForm = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm",
            TypeKind = TypeKind.ClassTree)]
        public class _ObjectForm
        {
            public static readonly string @tab = "tab";
            public IElement @_tab = new MofObjectShadow("dm:///_internal/types/internal#c19bbfec-6afb-4%23c19cbfec-6afb-4017-94c2-d2992853a25c017-94c2-d2992853a25c");

            public static readonly string @autoTabs = "autoTabs";
            public IElement? @_autoTabs = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

            public static readonly string @isAutoGenerated = "isAutoGenerated";
            public IElement? @_isAutoGenerated = null;

            public static readonly string @hideMetaInformation = "hideMetaInformation";
            public IElement? @_hideMetaInformation = null;

            public static readonly string @originalUri = "originalUri";
            public IElement? @_originalUri = null;

            public static readonly string @originalWorkspace = "originalWorkspace";
            public IElement? @_originalWorkspace = null;

            public static readonly string @creationProtocol = "creationProtocol";
            public IElement? @_creationProtocol = null;

            public static readonly string @hideReferencingItems = "hideReferencingItems";
            public IElement? @_hideReferencingItems = null;

        }

        public _ObjectForm @ObjectForm = new ();
        public MofObjectShadow @__ObjectForm = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation",
            TypeKind = TypeKind.ClassTree)]
        public class _FormAssociation
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @formType = "formType";
            public IElement? @_formType = null;

            public static readonly string @metaClass = "metaClass";
            public IElement? @_metaClass = null;

            public static readonly string @extentType = "extentType";
            public IElement? @_extentType = null;

            public static readonly string @viewModeId = "viewModeId";
            public IElement? @_viewModeId = null;

            public static readonly string @parentMetaClass = "parentMetaClass";
            public IElement? @_parentMetaClass = null;

            public static readonly string @parentProperty = "parentProperty";
            public IElement? @_parentProperty = null;

            public static readonly string @form = "form";
            public IElement? @_form = null;

            public static readonly string @debugActive = "debugActive";
            public IElement? @_debugActive = null;

            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @extentUri = "extentUri";
            public IElement? @_extentUri = null;

            public static readonly string @includeGeneralization = "includeGeneralization";
            public IElement? @_includeGeneralization = null;

        }

        public _FormAssociation @FormAssociation = new ();
        public MofObjectShadow @__FormAssociation = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation");

    }

    public _FormTypes FormTypes = new ();

    public class _FieldTypes
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _FieldData
        {
            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _FieldData @FieldData = new ();
        public MofObjectShadow @__FieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2",
            TypeKind = TypeKind.ClassTree)]
        public class _DropDownByQueryData
        {
            public static readonly string @query = "query";
            public IElement? @_query = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _DropDownByQueryData @DropDownByQueryData = new ();
        public MofObjectShadow @__DropDownByQueryData = new ("dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection",
            TypeKind = TypeKind.ClassTree)]
        public class _DropDownByCollection
        {
            public static readonly string @defaultWorkspace = "defaultWorkspace";
            public IElement? @_defaultWorkspace = null;

            public static readonly string @collection = "collection";
            public IElement? @_collection = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _DropDownByCollection @DropDownByCollection = new ();
        public MofObjectShadow @__DropDownByCollection = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection");

        [TypeUri(Uri = "dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00",
            TypeKind = TypeKind.ClassTree)]
        public class _UriReferenceFieldData
        {
            public static readonly string @defaultWorkspace = "defaultWorkspace";
            public IElement? @_defaultWorkspace = null;

            public static readonly string @defaultExtent = "defaultExtent";
            public IElement? @_defaultExtent = null;

        }

        public _UriReferenceFieldData @UriReferenceFieldData = new ();
        public MofObjectShadow @__UriReferenceFieldData = new ("dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _FullNameFieldData
        {
            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _FullNameFieldData @FullNameFieldData = new ();
        public MofObjectShadow @__FullNameFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _CheckboxListTaggingFieldData
        {
            public static readonly string @values = "values";
            public IElement? @_values = null;

            public static readonly string @separator = "separator";
            public IElement? @_separator = null;

            public static readonly string @containsFreeText = "containsFreeText";
            public IElement? @_containsFreeText = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _CheckboxListTaggingFieldData @CheckboxListTaggingFieldData = new ();
        public MofObjectShadow @__CheckboxListTaggingFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _NumberFieldData
        {
            public static readonly string @format = "format";
            public IElement? @_format = null;

            public static readonly string @isInteger = "isInteger";
            public IElement? @_isInteger = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _NumberFieldData @NumberFieldData = new ();
        public MofObjectShadow @__NumberFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _DropDownFieldData
        {
            public static readonly string @values = "values";
            public IElement? @_values = null;

            public static readonly string @valuesByEnumeration = "valuesByEnumeration";
            public IElement? @_valuesByEnumeration = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _DropDownFieldData @DropDownFieldData = new ();
        public MofObjectShadow @__DropDownFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair",
            TypeKind = TypeKind.ClassTree)]
        public class _ValuePair
        {
            public static readonly string @value = "value";
            public IElement? @_value = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ValuePair @ValuePair = new ();
        public MofObjectShadow @__ValuePair = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _MetaClassElementFieldData
        {
            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _MetaClassElementFieldData @MetaClassElementFieldData = new ();
        public MofObjectShadow @__MetaClassElementFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _ReferenceFieldData
        {
            public static readonly string @isSelectionInline = "isSelectionInline";
            public IElement? @_isSelectionInline = null;

            public static readonly string @defaultWorkspace = "defaultWorkspace";
            public IElement? @_defaultWorkspace = null;

            public static readonly string @defaultItemUri = "defaultItemUri";
            public IElement? @_defaultItemUri = null;

            public static readonly string @showAllChildren = "showAllChildren";
            public IElement? @_showAllChildren = null;

            public static readonly string @showWorkspaceSelection = "showWorkspaceSelection";
            public IElement? @_showWorkspaceSelection = null;

            public static readonly string @showExtentSelection = "showExtentSelection";
            public IElement? @_showExtentSelection = null;

            public static readonly string @metaClassFilter = "metaClassFilter";
            public IElement? @_metaClassFilter = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _ReferenceFieldData @ReferenceFieldData = new ();
        public MofObjectShadow @__ReferenceFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _SubElementFieldData
        {
            public static readonly string @metaClass = "metaClass";
            public IElement? @_metaClass = null;

            public static readonly string @form = "form";
            public IElement? @_form = null;

            public static readonly string @allowOnlyExistingElements = "allowOnlyExistingElements";
            public IElement? @_allowOnlyExistingElements = null;

            public static readonly string @defaultTypesForNewElements = "defaultTypesForNewElements";
            public IElement? @_defaultTypesForNewElements = null;

            public static readonly string @includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";
            public IElement? @_includeSpecializationsForDefaultTypes = null;

            public static readonly string @defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";
            public IElement? @_defaultWorkspaceOfNewElements = null;

            public static readonly string @defaultExtentOfNewElements = "defaultExtentOfNewElements";
            public IElement? @_defaultExtentOfNewElements = null;

            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _SubElementFieldData @SubElementFieldData = new ();
        public MofObjectShadow @__SubElementFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _TextFieldData
        {
            public static readonly string @lineHeight = "lineHeight";
            public IElement? @_lineHeight = null;

            public static readonly string @width = "width";
            public IElement? @_width = null;

            public static readonly string @shortenTextLength = "shortenTextLength";
            public IElement? @_shortenTextLength = null;

            public static readonly string @supportClipboardCopy = "supportClipboardCopy";
            public IElement? @_supportClipboardCopy = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _TextFieldData @TextFieldData = new ();
        public MofObjectShadow @__TextFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _EvalTextFieldData
        {
            public static readonly string @evalCellProperties = "evalCellProperties";
            public IElement? @_evalCellProperties = null;

            public static readonly string @lineHeight = "lineHeight";
            public IElement? @_lineHeight = null;

            public static readonly string @width = "width";
            public IElement? @_width = null;

            public static readonly string @shortenTextLength = "shortenTextLength";
            public IElement? @_shortenTextLength = null;

            public static readonly string @supportClipboardCopy = "supportClipboardCopy";
            public IElement? @_supportClipboardCopy = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _EvalTextFieldData @EvalTextFieldData = new ();
        public MofObjectShadow @__EvalTextFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _SeparatorLineFieldData
        {
            public static readonly string @Height = "Height";
            public IElement? @_Height = null;

        }

        public _SeparatorLineFieldData @SeparatorLineFieldData = new ();
        public MofObjectShadow @__SeparatorLineFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _FileSelectionFieldData
        {
            public static readonly string @defaultExtension = "defaultExtension";
            public IElement? @_defaultExtension = null;

            public static readonly string @isSaving = "isSaving";
            public IElement? @_isSaving = null;

            public static readonly string @initialPathToDirectory = "initialPathToDirectory";
            public IElement? @_initialPathToDirectory = null;

            public static readonly string @filter = "filter";
            public IElement? @_filter = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _FileSelectionFieldData @FileSelectionFieldData = new ();
        public MofObjectShadow @__FileSelectionFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _AnyDataFieldData
        {
            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _AnyDataFieldData @AnyDataFieldData = new ();
        public MofObjectShadow @__AnyDataFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _CheckboxFieldData
        {
            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _CheckboxFieldData @CheckboxFieldData = new ();
        public MofObjectShadow @__CheckboxFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _ActionFieldData
        {
            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @buttonText = "buttonText";
            public IElement? @_buttonText = null;

            public static readonly string @bindingKey = "bindingKey";
            public IElement? @_bindingKey = null;

            public static readonly string @bindingKeyModifierCtrl = "bindingKeyModifierCtrl";
            public IElement? @_bindingKeyModifierCtrl = null;

            public static readonly string @bindingKeyModifierShift = "bindingKeyModifierShift";
            public IElement? @_bindingKeyModifierShift = null;

            public static readonly string @bindingKeyModifierAlt = "bindingKeyModifierAlt";
            public IElement? @_bindingKeyModifierAlt = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _ActionFieldData @ActionFieldData = new ();
        public MofObjectShadow @__ActionFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData",
            TypeKind = TypeKind.ClassTree)]
        public class _DateTimeFieldData
        {
            public static readonly string @hideDate = "hideDate";
            public IElement? @_hideDate = null;

            public static readonly string @hideTime = "hideTime";
            public IElement? @_hideTime = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _DateTimeFieldData @DateTimeFieldData = new ();
        public MofObjectShadow @__DateTimeFieldData = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData");

        [TypeUri(Uri = "dm:///_internal/types/internal#b11ab14e-b620-486e-b795-fb559479674b",
            TypeKind = TypeKind.ClassTree)]
        public class _MergedFieldsInCellData
        {
            public static readonly string @fields = "fields";
            public IElement? @_fields = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _MergedFieldsInCellData @MergedFieldsInCellData = new ();
        public MofObjectShadow @__MergedFieldsInCellData = new ("dm:///_internal/types/internal#b11ab14e-b620-486e-b795-fb559479674b");

        [TypeUri(Uri = "dm:///_internal/types/internal#3769ef41-6cb7-4cc5-99e9-4de89198e644",
            TypeKind = TypeKind.ClassTree)]
        public class _CompositeFieldData
        {
            public static readonly string @metaclass = "metaclass";
            public IElement? @_metaclass = null;

            public static readonly string @isAttached = "isAttached";
            public IElement? @_isAttached = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @title = "title";
            public IElement? @_title = null;

            public static readonly string @isEnumeration = "isEnumeration";
            public IElement? @_isEnumeration = null;

            public static readonly string @defaultValue = "defaultValue";
            public IElement? @_defaultValue = null;

            public static readonly string @isReadOnly = "isReadOnly";
            public IElement? @_isReadOnly = null;

        }

        public _CompositeFieldData @CompositeFieldData = new ();
        public MofObjectShadow @__CompositeFieldData = new ("dm:///_internal/types/internal#3769ef41-6cb7-4cc5-99e9-4de89198e644");

    }

    public _FieldTypes FieldTypes = new ();

    public static readonly _Forms TheOne = new ();

}

public class _AttachedExtent
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration",
        TypeKind = TypeKind.ClassTree)]
    public class _AttachedExtentConfiguration
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @referencedWorkspace = "referencedWorkspace";
        public IElement? @_referencedWorkspace = null;

        public static readonly string @referencedExtent = "referencedExtent";
        public IElement? @_referencedExtent = null;

        public static readonly string @referenceType = "referenceType";
        public IElement? @_referenceType = null;

        public static readonly string @referenceProperty = "referenceProperty";
        public IElement? @_referenceProperty = null;

    }

    public _AttachedExtentConfiguration @AttachedExtentConfiguration = new ();
    public MofObjectShadow @__AttachedExtentConfiguration = new ("dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration");

    public static readonly _AttachedExtent TheOne = new ();

}

public class _Management
{
    public class _ExtentLoadingState
    {
        public static string @Unknown = "Unknown";
        public IElement @__Unknown = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Unknown");
        public static string @Unloaded = "Unloaded";
        public IElement @__Unloaded = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Unloaded");
        public static string @Loaded = "Loaded";
        public IElement @__Loaded = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Loaded");
        public static string @Failed = "Failed";
        public IElement @__Failed = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Failed");
        public static string @LoadedReadOnly = "LoadedReadOnly";
        public IElement @__LoadedReadOnly = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-LoadedReadOnly");

    }

    public _ExtentLoadingState @ExtentLoadingState = new _ExtentLoadingState();
    public IElement @__ExtentLoadingState = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState");


    public enum ___ExtentLoadingState
    {
        @Unknown,
        @Unloaded,
        @Loaded,
        @Failed,
        @LoadedReadOnly
    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent",
        TypeKind = TypeKind.ClassTree)]
    public class _Extent
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @uri = "uri";
        public IElement? @_uri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @count = "count";
        public IElement? @_count = null;

        public static readonly string @totalCount = "totalCount";
        public IElement? @_totalCount = null;

        public static readonly string @type = "type";
        public IElement? @_type = null;

        public static readonly string @extentType = "extentType";
        public IElement? @_extentType = null;

        public static readonly string @isModified = "isModified";
        public IElement? @_isModified = null;

        public static readonly string @alternativeUris = "alternativeUris";
        public IElement? @_alternativeUris = null;

        public static readonly string @autoEnumerateType = "autoEnumerateType";
        public IElement? @_autoEnumerateType = null;

        public static readonly string @state = "state";
        public IElement? @_state = null;

        public static readonly string @failMessage = "failMessage";
        public IElement? @_failMessage = null;

        public static readonly string @properties = "properties";
        public IElement? @_properties = null;

        public static readonly string @loadingConfiguration = "loadingConfiguration";
        public IElement? @_loadingConfiguration = null;

    }

    public _Extent @Extent = new ();
    public MofObjectShadow @__Extent = new ("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace",
        TypeKind = TypeKind.ClassTree)]
    public class _Workspace
    {
        public static readonly string @id = "id";
        public IElement? @_id = null;

        public static readonly string @annotation = "annotation";
        public IElement? @_annotation = null;

        public static readonly string @extents = "extents";
        public IElement? @_extents = null;

    }

    public _Workspace @Workspace = new ();
    public MofObjectShadow @__Workspace = new ("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel",
        TypeKind = TypeKind.ClassTree)]
    public class _CreateNewWorkspaceModel
    {
        public static readonly string @id = "id";
        public IElement? @_id = null;

        public static readonly string @annotation = "annotation";
        public IElement? @_annotation = null;

    }

    public _CreateNewWorkspaceModel @CreateNewWorkspaceModel = new ();
    public MofObjectShadow @__CreateNewWorkspaceModel = new ("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting",
        TypeKind = TypeKind.ClassTree)]
    public class _ExtentTypeSetting
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @rootElementMetaClasses = "rootElementMetaClasses";
        public IElement? @_rootElementMetaClasses = null;

    }

    public _ExtentTypeSetting @ExtentTypeSetting = new ();
    public MofObjectShadow @__ExtentTypeSetting = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties",
        TypeKind = TypeKind.ClassTree)]
    public class _ExtentProperties
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @uri = "uri";
        public IElement? @_uri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @count = "count";
        public IElement? @_count = null;

        public static readonly string @totalCount = "totalCount";
        public IElement? @_totalCount = null;

        public static readonly string @type = "type";
        public IElement? @_type = null;

        public static readonly string @extentType = "extentType";
        public IElement? @_extentType = null;

        public static readonly string @isModified = "isModified";
        public IElement? @_isModified = null;

        public static readonly string @alternativeUris = "alternativeUris";
        public IElement? @_alternativeUris = null;

        public static readonly string @autoEnumerateType = "autoEnumerateType";
        public IElement? @_autoEnumerateType = null;

        public static readonly string @state = "state";
        public IElement? @_state = null;

        public static readonly string @failMessage = "failMessage";
        public IElement? @_failMessage = null;

        public static readonly string @properties = "properties";
        public IElement? @_properties = null;

        public static readonly string @loadingConfiguration = "loadingConfiguration";
        public IElement? @_loadingConfiguration = null;

    }

    public _ExtentProperties @ExtentProperties = new ();
    public MofObjectShadow @__ExtentProperties = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition",
        TypeKind = TypeKind.ClassTree)]
    public class _ExtentPropertyDefinition
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @title = "title";
        public IElement? @_title = null;

        public static readonly string @metaClass = "metaClass";
        public IElement? @_metaClass = null;

    }

    public _ExtentPropertyDefinition @ExtentPropertyDefinition = new ();
    public MofObjectShadow @__ExtentPropertyDefinition = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings",
        TypeKind = TypeKind.ClassTree)]
    public class _ExtentSettings
    {
        public static readonly string @extentTypeSettings = "extentTypeSettings";
        public IElement? @_extentTypeSettings = null;

        public static readonly string @propertyDefinitions = "propertyDefinitions";
        public IElement? @_propertyDefinitions = null;

    }

    public _ExtentSettings @ExtentSettings = new ();
    public MofObjectShadow @__ExtentSettings = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings");

    public static readonly _Management TheOne = new ();

}

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


    public enum ___ComparisonType
    {
        @Equal,
        @GreaterThan,
        @LighterThan,
        @GreaterOrEqualThan,
        @LighterOrEqualThan
    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter",
        TypeKind = TypeKind.ClassTree)]
    public class _PropertyComparisonFilter
    {
        public static readonly string @Property = "Property";
        public IElement? @_Property = null;

        public static readonly string @ComparisonType = "ComparisonType";
        public IElement? @_ComparisonType = null;

        public static readonly string @Value = "Value";
        public IElement? @_Value = null;

    }

    public _PropertyComparisonFilter @PropertyComparisonFilter = new ();
    public MofObjectShadow @__PropertyComparisonFilter = new ("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter");

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter",
        TypeKind = TypeKind.ClassTree)]
    public class _PropertyContainsFilter
    {
        public static readonly string @Property = "Property";
        public IElement? @_Property = null;

        public static readonly string @Value = "Value";
        public IElement? @_Value = null;

    }

    public _PropertyContainsFilter @PropertyContainsFilter = new ();
    public MofObjectShadow @__PropertyContainsFilter = new ("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter");

    public static readonly _FastViewFilters TheOne = new ();

}

public class _DynamicRuntimeProvider
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig",
        TypeKind = TypeKind.ClassTree)]
    public class _DynamicRuntimeLoaderConfig
    {
        public static readonly string @runtimeClass = "runtimeClass";
        public IElement? @_runtimeClass = null;

        public static readonly string @configuration = "configuration";
        public IElement? @_configuration = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

    }

    public _DynamicRuntimeLoaderConfig @DynamicRuntimeLoaderConfig = new ();
    public MofObjectShadow @__DynamicRuntimeLoaderConfig = new ("dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig");

    public class _Examples
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a",
            TypeKind = TypeKind.ClassTree)]
        public class _NumberProviderSettings
        {
            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @start = "start";
            public IElement? @_start = null;

            public static readonly string @end = "end";
            public IElement? @_end = null;

        }

        public _NumberProviderSettings @NumberProviderSettings = new ();
        public MofObjectShadow @__NumberProviderSettings = new ("dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a");

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation",
            TypeKind = TypeKind.ClassTree)]
        public class _NumberRepresentation
        {
            public static readonly string @binary = "binary";
            public IElement? @_binary = null;

            public static readonly string @octal = "octal";
            public IElement? @_octal = null;

            public static readonly string @decimal = "decimal";
            public IElement? @_decimal = null;

            public static readonly string @hexadecimal = "hexadecimal";
            public IElement? @_hexadecimal = null;

        }

        public _NumberRepresentation @NumberRepresentation = new ();
        public MofObjectShadow @__NumberRepresentation = new ("dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation");

    }

    public _Examples Examples = new ();

    public static readonly _DynamicRuntimeProvider TheOne = new ();

}

public class _Verifier
{
    [TypeUri(Uri = "dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768",
        TypeKind = TypeKind.ClassTree)]
    public class _VerifyEntry
    {
        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @itemUri = "itemUri";
        public IElement? @_itemUri = null;

        public static readonly string @category = "category";
        public IElement? @_category = null;

        public static readonly string @message = "message";
        public IElement? @_message = null;

    }

    public _VerifyEntry @VerifyEntry = new ();
    public MofObjectShadow @__VerifyEntry = new ("dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768");

    public static readonly _Verifier TheOne = new ();

}

