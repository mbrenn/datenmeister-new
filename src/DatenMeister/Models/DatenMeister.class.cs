#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models
{
    public class _PrimitiveTypes
    {
        public class _DateTime
        {
        }

        public _DateTime @DateTime = new _DateTime();
        public IElement @__DateTime = new MofObjectShadow("dm:///_internal/types/internal#PrimitiveTypes.DateTime");

        public static readonly _PrimitiveTypes TheOne = new _PrimitiveTypes();

    }

    public class _DatenMeister
    {
        public class _CommonTypes
        {
            public class _Default
            {
                public class _Package
                {
                    public static string @name = "name";
                    public IElement? @_name = null;

                    public static string @packagedElement = "packagedElement";
                    public IElement? @_packagedElement = null;

                    public static string @preferredType = "preferredType";
                    public IElement? @_preferredType = null;

                    public static string @preferredPackage = "preferredPackage";
                    public IElement? @_preferredPackage = null;

                    public static string @defaultViewMode = "defaultViewMode";
                    public IElement? @_defaultViewMode = null;

                }

                public _Package @Package = new _Package();
                public IElement @__Package = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package");

            }

            public _Default Default = new _Default();

            public class _ExtentManager
            {
                public class _ImportSettings
                {
                    public static string @filePath = "filePath";
                    public IElement? @_filePath = null;

                    public static string @extentUri = "extentUri";
                    public IElement? @_extentUri = null;

                    public static string @workspace = "workspace";
                    public IElement? @_workspace = null;

                }

                public _ImportSettings @ImportSettings = new _ImportSettings();
                public IElement @__ImportSettings = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings");

                public class _ImportException
                {
                    public static string @message = "message";
                    public IElement? @_message = null;

                }

                public _ImportException @ImportException = new _ImportException();
                public IElement @__ImportException = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException");

            }

            public _ExtentManager ExtentManager = new _ExtentManager();

            public class _OSIntegration
            {
                public class _CommandLineApplication
                {
                    public static string @name = "name";
                    public IElement? @_name = null;

                    public static string @applicationPath = "applicationPath";
                    public IElement? @_applicationPath = null;

                }

                public _CommandLineApplication @CommandLineApplication = new _CommandLineApplication();
                public IElement @__CommandLineApplication = new MofObjectShadow("dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication");

            }

            public _OSIntegration OSIntegration = new _OSIntegration();

        }

        public _CommonTypes CommonTypes = new _CommonTypes();

        public class _Actions
        {
            public class _Action
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _Action @Action = new _Action();
            public IElement @__Action = new MofObjectShadow("dm:///_internal/types/internal#Actions.Action");

            public class _ActionSet
            {
                public static string @action = "action";
                public IElement? @_action = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _ActionSet @ActionSet = new _ActionSet();
            public IElement @__ActionSet = new MofObjectShadow("dm:///_internal/types/internal#Actions.ActionSet");

            public class _LoggingWriterAction
            {
                public static string @message = "message";
                public IElement? @_message = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _LoggingWriterAction @LoggingWriterAction = new _LoggingWriterAction();
            public IElement @__LoggingWriterAction = new MofObjectShadow("dm:///_internal/types/internal#Actions.LoggingWriterAction");

            public class _CommandExecutionAction
            {
                public static string @command = "command";
                public IElement? @_command = null;

                public static string @arguments = "arguments";
                public IElement? @_arguments = null;

                public static string @workingDirectory = "workingDirectory";
                public IElement? @_workingDirectory = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _CommandExecutionAction @CommandExecutionAction = new _CommandExecutionAction();
            public IElement @__CommandExecutionAction = new MofObjectShadow("dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82");

            public class _PowershellExecutionAction
            {
                public static string @script = "script";
                public IElement? @_script = null;

                public static string @workingDirectory = "workingDirectory";
                public IElement? @_workingDirectory = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _PowershellExecutionAction @PowershellExecutionAction = new _PowershellExecutionAction();
            public IElement @__PowershellExecutionAction = new MofObjectShadow("dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb");

            public class _LoadExtentAction
            {
                public static string @configuration = "configuration";
                public IElement? @_configuration = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _LoadExtentAction @LoadExtentAction = new _LoadExtentAction();
            public IElement @__LoadExtentAction = new MofObjectShadow("dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee");

            public class _DropExtentAction
            {
                public static string @workspace = "workspace";
                public IElement? @_workspace = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _DropExtentAction @DropExtentAction = new _DropExtentAction();
            public IElement @__DropExtentAction = new MofObjectShadow("dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09");

            public class _CreateWorkspaceAction
            {
                public static string @workspace = "workspace";
                public IElement? @_workspace = null;

                public static string @annotation = "annotation";
                public IElement? @_annotation = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _CreateWorkspaceAction @CreateWorkspaceAction = new _CreateWorkspaceAction();
            public IElement @__CreateWorkspaceAction = new MofObjectShadow("dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe");

            public class _DropWorkspaceAction
            {
                public static string @workspace = "workspace";
                public IElement? @_workspace = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _DropWorkspaceAction @DropWorkspaceAction = new _DropWorkspaceAction();
            public IElement @__DropWorkspaceAction = new MofObjectShadow("dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8");

            public class _CopyElementsAction
            {
                public static string @sourcePath = "sourcePath";
                public IElement? @_sourcePath = null;

                public static string @targetPath = "targetPath";
                public IElement? @_targetPath = null;

                public static string @moveOnly = "moveOnly";
                public IElement? @_moveOnly = null;

                public static string @sourceWorkspace = "sourceWorkspace";
                public IElement? @_sourceWorkspace = null;

                public static string @targetWorkspace = "targetWorkspace";
                public IElement? @_targetWorkspace = null;

                public static string @emptyTarget = "emptyTarget";
                public IElement? @_emptyTarget = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _CopyElementsAction @CopyElementsAction = new _CopyElementsAction();
            public IElement @__CopyElementsAction = new MofObjectShadow("dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed");

            public class _ExportToXmiAction
            {
                public static string @sourcePath = "sourcePath";
                public IElement? @_sourcePath = null;

                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @sourceWorkspaceId = "sourceWorkspaceId";
                public IElement? @_sourceWorkspaceId = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _ExportToXmiAction @ExportToXmiAction = new _ExportToXmiAction();
            public IElement @__ExportToXmiAction = new MofObjectShadow("dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863");

            public class _ClearCollectionAction
            {
                public static string @workspace = "workspace";
                public IElement? @_workspace = null;

                public static string @path = "path";
                public IElement? @_path = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;

            }

            public _ClearCollectionAction @ClearCollectionAction = new _ClearCollectionAction();
            public IElement @__ClearCollectionAction = new MofObjectShadow("dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae");

        }

        public _Actions Actions = new _Actions();

        public class _DataViews
        {
            public class _DataView
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @workspace = "workspace";
                public IElement? @_workspace = null;

                public static string @uri = "uri";
                public IElement? @_uri = null;

                public static string @viewNode = "viewNode";
                public IElement? @_viewNode = null;

            }

            public _DataView @DataView = new _DataView();
            public IElement @__DataView = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView");

            public class _ViewNode
            {
                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _ViewNode @ViewNode = new _ViewNode();
            public IElement @__ViewNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode");

            public class _SourceExtentNode
            {
                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspace = "workspace";
                public IElement? @_workspace = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _SourceExtentNode @SourceExtentNode = new _SourceExtentNode();
            public IElement @__SourceExtentNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SourceExtentNode");

            public class _FlattenNode
            {
                public static string @input = "input";
                public IElement? @_input = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _FlattenNode @FlattenNode = new _FlattenNode();
            public IElement @__FlattenNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode");

            public class _FilterPropertyNode
            {
                public static string @input = "input";
                public IElement? @_input = null;

                public static string @property = "property";
                public IElement? @_property = null;

                public static string @value = "value";
                public IElement? @_value = null;

                public static string @comparisonMode = "comparisonMode";
                public IElement? @_comparisonMode = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _FilterPropertyNode @FilterPropertyNode = new _FilterPropertyNode();
            public IElement @__FilterPropertyNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterPropertyNode");

            public class _FilterTypeNode
            {
                public static string @input = "input";
                public IElement? @_input = null;

                public static string @type = "type";
                public IElement? @_type = null;

                public static string @includeInherits = "includeInherits";
                public IElement? @_includeInherits = null;

                public static string @name = "name";
                public IElement? @_name = null;

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
                public IElement? @_input = null;

                public static string @path = "path";
                public IElement? @_path = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _SelectByFullNameNode @SelectByFullNameNode = new _SelectByFullNameNode();
            public IElement @__SelectByFullNameNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode");

            public class _DynamicSourceNode
            {
                public static string @nodeName = "nodeName";
                public IElement? @_nodeName = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _DynamicSourceNode @DynamicSourceNode = new _DynamicSourceNode();
            public IElement @__DynamicSourceNode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode");

        }

        public _DataViews DataViews = new _DataViews();

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

                public static string @viewNode = "viewNode";
                public IElement? @_viewNode = null;

                public static string @evalProperties = "evalProperties";
                public IElement? @_evalProperties = null;

                public static string @evalParagraph = "evalParagraph";
                public IElement? @_evalParagraph = null;

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

            public class _ReportInstance
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @reportDefinition = "reportDefinition";
                public IElement? @_reportDefinition = null;

                public static string @sources = "sources";
                public IElement? @_sources = null;

            }

            public _ReportInstance @ReportInstance = new _ReportInstance();
            public IElement @__ReportInstance = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance");

            public class _AdocReportInstance
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @reportDefinition = "reportDefinition";
                public IElement? @_reportDefinition = null;

                public static string @sources = "sources";
                public IElement? @_sources = null;

            }

            public _AdocReportInstance @AdocReportInstance = new _AdocReportInstance();
            public IElement @__AdocReportInstance = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance");

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
            public IElement @__HtmlReportInstance = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance");

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

        }

        public _Reports Reports = new _Reports();

        public class _ExtentLoaderConfigs
        {
            public class _ExtentLoaderConfig
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _ExtentLoaderConfig @ExtentLoaderConfig = new _ExtentLoaderConfig();
            public IElement @__ExtentLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig");

            public class _ExcelLoaderConfig
            {
                public static string @fixRowCount = "fixRowCount";
                public IElement? @_fixRowCount = null;

                public static string @fixColumnCount = "fixColumnCount";
                public IElement? @_fixColumnCount = null;

                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @sheetName = "sheetName";
                public IElement? @_sheetName = null;

                public static string @offsetRow = "offsetRow";
                public IElement? @_offsetRow = null;

                public static string @offsetColumn = "offsetColumn";
                public IElement? @_offsetColumn = null;

                public static string @countRows = "countRows";
                public IElement? @_countRows = null;

                public static string @countColumns = "countColumns";
                public IElement? @_countColumns = null;

                public static string @hasHeader = "hasHeader";
                public IElement? @_hasHeader = null;

                public static string @idColumnName = "idColumnName";
                public IElement? @_idColumnName = null;

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";
                public IElement? @_skipEmptyRowsCount = null;

                public static string @columns = "columns";
                public IElement? @_columns = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _ExcelLoaderConfig @ExcelLoaderConfig = new _ExcelLoaderConfig();
            public IElement @__ExcelLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig");

            public class _ExcelReferenceLoaderConfig
            {
                public static string @fixRowCount = "fixRowCount";
                public IElement? @_fixRowCount = null;

                public static string @fixColumnCount = "fixColumnCount";
                public IElement? @_fixColumnCount = null;

                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @sheetName = "sheetName";
                public IElement? @_sheetName = null;

                public static string @offsetRow = "offsetRow";
                public IElement? @_offsetRow = null;

                public static string @offsetColumn = "offsetColumn";
                public IElement? @_offsetColumn = null;

                public static string @countRows = "countRows";
                public IElement? @_countRows = null;

                public static string @countColumns = "countColumns";
                public IElement? @_countColumns = null;

                public static string @hasHeader = "hasHeader";
                public IElement? @_hasHeader = null;

                public static string @idColumnName = "idColumnName";
                public IElement? @_idColumnName = null;

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";
                public IElement? @_skipEmptyRowsCount = null;

                public static string @columns = "columns";
                public IElement? @_columns = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _ExcelReferenceLoaderConfig @ExcelReferenceLoaderConfig = new _ExcelReferenceLoaderConfig();
            public IElement @__ExcelReferenceLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig");

            public class _ExcelImportLoaderConfig
            {
                public static string @extentPath = "extentPath";
                public IElement? @_extentPath = null;

                public static string @fixRowCount = "fixRowCount";
                public IElement? @_fixRowCount = null;

                public static string @fixColumnCount = "fixColumnCount";
                public IElement? @_fixColumnCount = null;

                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @sheetName = "sheetName";
                public IElement? @_sheetName = null;

                public static string @offsetRow = "offsetRow";
                public IElement? @_offsetRow = null;

                public static string @offsetColumn = "offsetColumn";
                public IElement? @_offsetColumn = null;

                public static string @countRows = "countRows";
                public IElement? @_countRows = null;

                public static string @countColumns = "countColumns";
                public IElement? @_countColumns = null;

                public static string @hasHeader = "hasHeader";
                public IElement? @_hasHeader = null;

                public static string @idColumnName = "idColumnName";
                public IElement? @_idColumnName = null;

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";
                public IElement? @_skipEmptyRowsCount = null;

                public static string @columns = "columns";
                public IElement? @_columns = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _ExcelImportLoaderConfig @ExcelImportLoaderConfig = new _ExcelImportLoaderConfig();
            public IElement @__ExcelImportLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig");

            public class _ExcelExtentLoaderConfig
            {
                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @idColumnName = "idColumnName";
                public IElement? @_idColumnName = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _ExcelExtentLoaderConfig @ExcelExtentLoaderConfig = new _ExcelExtentLoaderConfig();
            public IElement @__ExcelExtentLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig");

            public class _InMemoryLoaderConfig
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _InMemoryLoaderConfig @InMemoryLoaderConfig = new _InMemoryLoaderConfig();
            public IElement @__InMemoryLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig");

            public class _XmlReferenceLoaderConfig
            {
                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @keepNamespaces = "keepNamespaces";
                public IElement? @_keepNamespaces = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _XmlReferenceLoaderConfig @XmlReferenceLoaderConfig = new _XmlReferenceLoaderConfig();
            public IElement @__XmlReferenceLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig");

            public class _ExtentFileLoaderConfig
            {
                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _ExtentFileLoaderConfig @ExtentFileLoaderConfig = new _ExtentFileLoaderConfig();
            public IElement @__ExtentFileLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig");

            public class _XmiStorageLoaderConfig
            {
                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _XmiStorageLoaderConfig @XmiStorageLoaderConfig = new _XmiStorageLoaderConfig();
            public IElement @__XmiStorageLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");

            public class _CsvExtentLoaderConfig
            {
                public static string @settings = "settings";
                public IElement? @_settings = null;

                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _CsvExtentLoaderConfig @CsvExtentLoaderConfig = new _CsvExtentLoaderConfig();
            public IElement @__CsvExtentLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig");

            public class _CsvSettings
            {
                public static string @encoding = "encoding";
                public IElement? @_encoding = null;

                public static string @hasHeader = "hasHeader";
                public IElement? @_hasHeader = null;

                public static string @separator = "separator";
                public IElement? @_separator = null;

                public static string @columns = "columns";
                public IElement? @_columns = null;

                public static string @metaclassUri = "metaclassUri";
                public IElement? @_metaclassUri = null;

            }

            public _CsvSettings @CsvSettings = new _CsvSettings();
            public IElement @__CsvSettings = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings");

            public class _ExcelHierarchicalColumnDefinition
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @metaClass = "metaClass";
                public IElement? @_metaClass = null;

                public static string @property = "property";
                public IElement? @_property = null;

            }

            public _ExcelHierarchicalColumnDefinition @ExcelHierarchicalColumnDefinition = new _ExcelHierarchicalColumnDefinition();
            public IElement @__ExcelHierarchicalColumnDefinition = new MofObjectShadow("dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition");

            public class _ExcelHierarchicalLoaderConfig
            {
                public static string @hierarchicalColumns = "hierarchicalColumns";
                public IElement? @_hierarchicalColumns = null;

                public static string @skipElementsForLastLevel = "skipElementsForLastLevel";
                public IElement? @_skipElementsForLastLevel = null;

                public static string @fixRowCount = "fixRowCount";
                public IElement? @_fixRowCount = null;

                public static string @fixColumnCount = "fixColumnCount";
                public IElement? @_fixColumnCount = null;

                public static string @filePath = "filePath";
                public IElement? @_filePath = null;

                public static string @sheetName = "sheetName";
                public IElement? @_sheetName = null;

                public static string @offsetRow = "offsetRow";
                public IElement? @_offsetRow = null;

                public static string @offsetColumn = "offsetColumn";
                public IElement? @_offsetColumn = null;

                public static string @countRows = "countRows";
                public IElement? @_countRows = null;

                public static string @countColumns = "countColumns";
                public IElement? @_countColumns = null;

                public static string @hasHeader = "hasHeader";
                public IElement? @_hasHeader = null;

                public static string @idColumnName = "idColumnName";
                public IElement? @_idColumnName = null;

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";
                public IElement? @_skipEmptyRowsCount = null;

                public static string @columns = "columns";
                public IElement? @_columns = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @extentUri = "extentUri";
                public IElement? @_extentUri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;

            }

            public _ExcelHierarchicalLoaderConfig @ExcelHierarchicalLoaderConfig = new _ExcelHierarchicalLoaderConfig();
            public IElement @__ExcelHierarchicalLoaderConfig = new MofObjectShadow("dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig");

            public class _ExcelColumn
            {
                public static string @header = "header";
                public IElement? @_header = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _ExcelColumn @ExcelColumn = new _ExcelColumn();
            public IElement @__ExcelColumn = new MofObjectShadow("dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a");

        }

        public _ExtentLoaderConfigs ExtentLoaderConfigs = new _ExtentLoaderConfigs();

        public class _Forms
        {
            public class _FieldData
            {
                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _FieldData @FieldData = new _FieldData();
            public IElement @__FieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData");

            public class _SortingOrder
            {
                public static string @field = "field";
                public IElement? @_field = null;

                public static string @isDescending = "isDescending";
                public IElement? @_isDescending = null;

            }

            public _SortingOrder @SortingOrder = new _SortingOrder();
            public IElement @__SortingOrder = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder");

            public class _AnyDataFieldData
            {
                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _AnyDataFieldData @AnyDataFieldData = new _AnyDataFieldData();
            public IElement @__AnyDataFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData");

            public class _CheckboxFieldData
            {
                public static string @lineHeight = "lineHeight";
                public IElement? @_lineHeight = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _CheckboxFieldData @CheckboxFieldData = new _CheckboxFieldData();
            public IElement @__CheckboxFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData");

            public class _DateTimeFieldData
            {
                public static string @hideDate = "hideDate";
                public IElement? @_hideDate = null;

                public static string @hideTime = "hideTime";
                public IElement? @_hideTime = null;

                public static string @showOffsetButtons = "showOffsetButtons";
                public IElement? @_showOffsetButtons = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _DateTimeFieldData @DateTimeFieldData = new _DateTimeFieldData();
            public IElement @__DateTimeFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData");

            public class _FormAssociation
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @formType = "formType";
                public IElement? @_formType = null;

                public static string @metaClass = "metaClass";
                public IElement? @_metaClass = null;

                public static string @extentType = "extentType";
                public IElement? @_extentType = null;

                public static string @viewModeId = "viewModeId";
                public IElement? @_viewModeId = null;

                public static string @parentMetaClass = "parentMetaClass";
                public IElement? @_parentMetaClass = null;

                public static string @parentProperty = "parentProperty";
                public IElement? @_parentProperty = null;

                public static string @form = "form";
                public IElement? @_form = null;

            }

            public _FormAssociation @FormAssociation = new _FormAssociation();
            public IElement @__FormAssociation = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation");

            public class _DropDownFieldData
            {
                public static string @values = "values";
                public IElement? @_values = null;

                public static string @valuesByEnumeration = "valuesByEnumeration";
                public IElement? @_valuesByEnumeration = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _DropDownFieldData @DropDownFieldData = new _DropDownFieldData();
            public IElement @__DropDownFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData");

            public class _ValuePair
            {
                public static string @value = "value";
                public IElement? @_value = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _ValuePair @ValuePair = new _ValuePair();
            public IElement @__ValuePair = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair");

            public class _MetaClassElementFieldData
            {
                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _MetaClassElementFieldData @MetaClassElementFieldData = new _MetaClassElementFieldData();
            public IElement @__MetaClassElementFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData");

            public class _ReferenceFieldData
            {
                public static string @isSelectionInline = "isSelectionInline";
                public IElement? @_isSelectionInline = null;

                public static string @defaultExtentUri = "defaultExtentUri";
                public IElement? @_defaultExtentUri = null;

                public static string @defaultWorkspace = "defaultWorkspace";
                public IElement? @_defaultWorkspace = null;

                public static string @showAllChildren = "showAllChildren";
                public IElement? @_showAllChildren = null;

                public static string @showWorkspaceSelection = "showWorkspaceSelection";
                public IElement? @_showWorkspaceSelection = null;

                public static string @showExtentSelection = "showExtentSelection";
                public IElement? @_showExtentSelection = null;

                public static string @metaClassFilter = "metaClassFilter";
                public IElement? @_metaClassFilter = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _ReferenceFieldData @ReferenceFieldData = new _ReferenceFieldData();
            public IElement @__ReferenceFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData");

            public class _SubElementFieldData
            {
                public static string @metaClass = "metaClass";
                public IElement? @_metaClass = null;

                public static string @form = "form";
                public IElement? @_form = null;

                public static string @allowOnlyExistingElements = "allowOnlyExistingElements";
                public IElement? @_allowOnlyExistingElements = null;

                public static string @defaultTypesForNewElements = "defaultTypesForNewElements";
                public IElement? @_defaultTypesForNewElements = null;

                public static string @includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";
                public IElement? @_includeSpecializationsForDefaultTypes = null;

                public static string @defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";
                public IElement? @_defaultWorkspaceOfNewElements = null;

                public static string @defaultExtentOfNewElements = "defaultExtentOfNewElements";
                public IElement? @_defaultExtentOfNewElements = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _SubElementFieldData @SubElementFieldData = new _SubElementFieldData();
            public IElement @__SubElementFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData");

            public class _TextFieldData
            {
                public static string @lineHeight = "lineHeight";
                public IElement? @_lineHeight = null;

                public static string @width = "width";
                public IElement? @_width = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _TextFieldData @TextFieldData = new _TextFieldData();
            public IElement @__TextFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData");

            public class _EvalTextFieldData
            {
                public static string @evalCellProperties = "evalCellProperties";
                public IElement? @_evalCellProperties = null;

                public static string @lineHeight = "lineHeight";
                public IElement? @_lineHeight = null;

                public static string @width = "width";
                public IElement? @_width = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _EvalTextFieldData @EvalTextFieldData = new _EvalTextFieldData();
            public IElement @__EvalTextFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData");

            public class _SeparatorLineFieldData
            {
                public static string @Height = "Height";
                public IElement? @_Height = null;

            }

            public _SeparatorLineFieldData @SeparatorLineFieldData = new _SeparatorLineFieldData();
            public IElement @__SeparatorLineFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData");

            public class _FileSelectionFieldData
            {
                public static string @defaultExtension = "defaultExtension";
                public IElement? @_defaultExtension = null;

                public static string @isSaving = "isSaving";
                public IElement? @_isSaving = null;

                public static string @initialPathToDirectory = "initialPathToDirectory";
                public IElement? @_initialPathToDirectory = null;

                public static string @filter = "filter";
                public IElement? @_filter = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _FileSelectionFieldData @FileSelectionFieldData = new _FileSelectionFieldData();
            public IElement @__FileSelectionFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData");

            public class _DefaultTypeForNewElement
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @metaClass = "metaClass";
                public IElement? @_metaClass = null;

                public static string @parentProperty = "parentProperty";
                public IElement? @_parentProperty = null;

            }

            public _DefaultTypeForNewElement @DefaultTypeForNewElement = new _DefaultTypeForNewElement();
            public IElement @__DefaultTypeForNewElement = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement");

            public class _FullNameFieldData
            {
                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _FullNameFieldData @FullNameFieldData = new _FullNameFieldData();
            public IElement @__FullNameFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData");

            public class _CheckboxListTaggingFieldData
            {
                public static string @values = "values";
                public IElement? @_values = null;

                public static string @separator = "separator";
                public IElement? @_separator = null;

                public static string @containsFreeText = "containsFreeText";
                public IElement? @_containsFreeText = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _CheckboxListTaggingFieldData @CheckboxListTaggingFieldData = new _CheckboxListTaggingFieldData();
            public IElement @__CheckboxListTaggingFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData");

            public class _NumberFieldData
            {
                public static string @format = "format";
                public IElement? @_format = null;

                public static string @isInteger = "isInteger";
                public IElement? @_isInteger = null;

                public static string @isAttached = "isAttached";
                public IElement? @_isAttached = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isEnumeration = "isEnumeration";
                public IElement? @_isEnumeration = null;

                public static string @defaultValue = "defaultValue";
                public IElement? @_defaultValue = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

            }

            public _NumberFieldData @NumberFieldData = new _NumberFieldData();
            public IElement @__NumberFieldData = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData");

            public class _FormType
            {
                public static string @Detail = "Detail";
                public IElement @__Detail = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-Detail");
                public static string @TreeItemExtent = "TreeItemExtent";
                public IElement @__TreeItemExtent = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-TreeItemExtent");
                public static string @TreeItemDetail = "TreeItemDetail";
                public IElement @__TreeItemDetail = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-TreeItemDetail");
                public static string @ObjectList = "ObjectList";
                public IElement @__ObjectList = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-ObjectList");

            }

            public _FormType @FormType = new _FormType();
            public IElement @__FormType = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType");

            public class _Form
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

                public static string @hideMetaInformation = "hideMetaInformation";
                public IElement? @_hideMetaInformation = null;

                public static string @originalUri = "originalUri";
                public IElement? @_originalUri = null;

            }

            public _Form @Form = new _Form();
            public IElement @__Form = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.Form");

            public class _DetailForm
            {
                public static string @buttonApplyText = "buttonApplyText";
                public IElement? @_buttonApplyText = null;

                public static string @allowNewProperties = "allowNewProperties";
                public IElement? @_allowNewProperties = null;

                public static string @defaultWidth = "defaultWidth";
                public IElement? @_defaultWidth = null;

                public static string @defaultHeight = "defaultHeight";
                public IElement? @_defaultHeight = null;

                public static string @tab = "tab";
                public IElement? @_tab = null;

                public static string @field = "field";
                public IElement? @_field = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

                public static string @hideMetaInformation = "hideMetaInformation";
                public IElement? @_hideMetaInformation = null;

                public static string @originalUri = "originalUri";
                public IElement? @_originalUri = null;

            }

            public _DetailForm @DetailForm = new _DetailForm();
            public IElement @__DetailForm = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.DetailForm");

            public class _ListForm
            {
                public static string @property = "property";
                public IElement? @_property = null;

                public static string @metaClass = "metaClass";
                public IElement? @_metaClass = null;

                public static string @includeDescendents = "includeDescendents";
                public IElement? @_includeDescendents = null;

                public static string @noItemsWithMetaClass = "noItemsWithMetaClass";
                public IElement? @_noItemsWithMetaClass = null;

                public static string @inhibitNewItems = "inhibitNewItems";
                public IElement? @_inhibitNewItems = null;

                public static string @inhibitDeleteItems = "inhibitDeleteItems";
                public IElement? @_inhibitDeleteItems = null;

                public static string @inhibitEditItems = "inhibitEditItems";
                public IElement? @_inhibitEditItems = null;

                public static string @defaultTypesForNewElements = "defaultTypesForNewElements";
                public IElement? @_defaultTypesForNewElements = null;

                public static string @fastViewFilters = "fastViewFilters";
                public IElement? @_fastViewFilters = null;

                public static string @field = "field";
                public IElement? @_field = null;

                public static string @sortingOrder = "sortingOrder";
                public IElement? @_sortingOrder = null;

                public static string @viewNode = "viewNode";
                public IElement? @_viewNode = null;

                public static string @autoGenerateFields = "autoGenerateFields";
                public IElement? @_autoGenerateFields = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

                public static string @hideMetaInformation = "hideMetaInformation";
                public IElement? @_hideMetaInformation = null;

                public static string @originalUri = "originalUri";
                public IElement? @_originalUri = null;

            }

            public _ListForm @ListForm = new _ListForm();
            public IElement @__ListForm = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ListForm");

            public class _ExtentForm
            {
                public static string @tab = "tab";
                public IElement? @_tab = null;

                public static string @autoTabs = "autoTabs";
                public IElement? @_autoTabs = null;

                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_isReadOnly = null;

                public static string @hideMetaInformation = "hideMetaInformation";
                public IElement? @_hideMetaInformation = null;

                public static string @originalUri = "originalUri";
                public IElement? @_originalUri = null;

            }

            public _ExtentForm @ExtentForm = new _ExtentForm();
            public IElement @__ExtentForm = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ExtentForm");

            public class _ViewMode
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @id = "id";
                public IElement? @_id = null;

                public static string @defaultExtentType = "defaultExtentType";
                public IElement? @_defaultExtentType = null;

            }

            public _ViewMode @ViewMode = new _ViewMode();
            public IElement @__ViewMode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode");

        }

        public _Forms Forms = new _Forms();

        public class _AttachedExtent
        {
            public class _AttachedExtentConfiguration
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @referencedWorkspace = "referencedWorkspace";
                public IElement? @_referencedWorkspace = null;

                public static string @referencedExtent = "referencedExtent";
                public IElement? @_referencedExtent = null;

                public static string @referenceType = "referenceType";
                public IElement? @_referenceType = null;

                public static string @referenceProperty = "referenceProperty";
                public IElement? @_referenceProperty = null;

            }

            public _AttachedExtentConfiguration @AttachedExtentConfiguration = new _AttachedExtentConfiguration();
            public IElement @__AttachedExtentConfiguration = new MofObjectShadow("dm:///_internal/types/internal#68b3ddbf-a563-4d91-91fb-81e5bfac46b5");

        }

        public _AttachedExtent AttachedExtent = new _AttachedExtent();

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

            public class _Extent
            {
                public static string @uri = "uri";
                public IElement? @_uri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @count = "count";
                public IElement? @_count = null;

                public static string @totalCount = "totalCount";
                public IElement? @_totalCount = null;

                public static string @type = "type";
                public IElement? @_type = null;

                public static string @extentType = "extentType";
                public IElement? @_extentType = null;

                public static string @isModified = "isModified";
                public IElement? @_isModified = null;

                public static string @alternativeUris = "alternativeUris";
                public IElement? @_alternativeUris = null;

                public static string @autoEnumerateType = "autoEnumerateType";
                public IElement? @_autoEnumerateType = null;

                public static string @state = "state";
                public IElement? @_state = null;

                public static string @failMessage = "failMessage";
                public IElement? @_failMessage = null;

            }

            public _Extent @Extent = new _Extent();
            public IElement @__Extent = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent");

            public class _Workspace
            {
                public static string @id = "id";
                public IElement? @_id = null;

                public static string @annotation = "annotation";
                public IElement? @_annotation = null;

                public static string @extents = "extents";
                public IElement? @_extents = null;

            }

            public _Workspace @Workspace = new _Workspace();
            public IElement @__Workspace = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace");

            public class _CreateNewWorkspaceModel
            {
                public static string @id = "id";
                public IElement? @_id = null;

                public static string @annotation = "annotation";
                public IElement? @_annotation = null;

            }

            public _CreateNewWorkspaceModel @CreateNewWorkspaceModel = new _CreateNewWorkspaceModel();
            public IElement @__CreateNewWorkspaceModel = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel");

            public class _ExtentTypeSetting
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @rootElementMetaClasses = "rootElementMetaClasses";
                public IElement? @_rootElementMetaClasses = null;

            }

            public _ExtentTypeSetting @ExtentTypeSetting = new _ExtentTypeSetting();
            public IElement @__ExtentTypeSetting = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting");

            public class _ExtentProperties
            {
                public static string @uri = "uri";
                public IElement? @_uri = null;

                public static string @workspaceId = "workspaceId";
                public IElement? @_workspaceId = null;

                public static string @count = "count";
                public IElement? @_count = null;

                public static string @totalCount = "totalCount";
                public IElement? @_totalCount = null;

                public static string @type = "type";
                public IElement? @_type = null;

                public static string @extentType = "extentType";
                public IElement? @_extentType = null;

                public static string @isModified = "isModified";
                public IElement? @_isModified = null;

                public static string @alternativeUris = "alternativeUris";
                public IElement? @_alternativeUris = null;

                public static string @autoEnumerateType = "autoEnumerateType";
                public IElement? @_autoEnumerateType = null;

                public static string @state = "state";
                public IElement? @_state = null;

                public static string @failMessage = "failMessage";
                public IElement? @_failMessage = null;

            }

            public _ExtentProperties @ExtentProperties = new _ExtentProperties();
            public IElement @__ExtentProperties = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties");

            public class _ExtentPropertyDefinition
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @title = "title";
                public IElement? @_title = null;

                public static string @metaClass = "metaClass";
                public IElement? @_metaClass = null;

            }

            public _ExtentPropertyDefinition @ExtentPropertyDefinition = new _ExtentPropertyDefinition();
            public IElement @__ExtentPropertyDefinition = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition");

            public class _ExtentSettings
            {
                public static string @extentTypeSettings = "extentTypeSettings";
                public IElement? @_extentTypeSettings = null;

                public static string @propertyDefinitions = "propertyDefinitions";
                public IElement? @_propertyDefinitions = null;

            }

            public _ExtentSettings @ExtentSettings = new _ExtentSettings();
            public IElement @__ExtentSettings = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings");

        }

        public _Management Management = new _Management();

        public class _UserManagement
        {
            public class _User
            {
                public static string @name = "name";
                public IElement? @_name = null;

                public static string @password = "password";
                public IElement? @_password = null;

            }

            public _User @User = new _User();
            public IElement @__User = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Modules.UserManagement.User");

            public class _UserManagementSettings
            {
                public static string @salt = "salt";
                public IElement? @_salt = null;

            }

            public _UserManagementSettings @UserManagementSettings = new _UserManagementSettings();
            public IElement @__UserManagementSettings = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Modules.UserManagement.UserManagementSettings");

        }

        public _UserManagement UserManagement = new _UserManagement();

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
                public IElement? @_Property = null;

                public static string @ComparisonType = "ComparisonType";
                public IElement? @_ComparisonType = null;

                public static string @Value = "Value";
                public IElement? @_Value = null;

            }

            public _PropertyComparisonFilter @PropertyComparisonFilter = new _PropertyComparisonFilter();
            public IElement @__PropertyComparisonFilter = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter");

            public class _PropertyContainsFilter
            {
                public static string @Property = "Property";
                public IElement? @_Property = null;

                public static string @Value = "Value";
                public IElement? @_Value = null;

            }

            public _PropertyContainsFilter @PropertyContainsFilter = new _PropertyContainsFilter();
            public IElement @__PropertyContainsFilter = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter");

        }

        public _FastViewFilters FastViewFilters = new _FastViewFilters();

        public static readonly _DatenMeister TheOne = new _DatenMeister();

    }

    public class _Apps
    {
        public class _ZipCodes
        {
            public class _ZipCode
            {
                public static string @id = "id";
                public IElement? @_id = null;

                public static string @zip = "zip";
                public IElement? @_zip = null;

                public static string @positionLong = "positionLong";
                public IElement? @_positionLong = null;

                public static string @positionLat = "positionLat";
                public IElement? @_positionLat = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _ZipCode @ZipCode = new _ZipCode();
            public IElement @__ZipCode = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Example.ZipCode.ZipCode");

            public class _ZipCodeWithState
            {
                public static string @state = "state";
                public IElement? @_state = null;

                public static string @id = "id";
                public IElement? @_id = null;

                public static string @zip = "zip";
                public IElement? @_zip = null;

                public static string @positionLong = "positionLong";
                public IElement? @_positionLong = null;

                public static string @positionLat = "positionLat";
                public IElement? @_positionLat = null;

                public static string @name = "name";
                public IElement? @_name = null;

            }

            public _ZipCodeWithState @ZipCodeWithState = new _ZipCodeWithState();
            public IElement @__ZipCodeWithState = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Example.ZipCode.ZipCodeWithState");

        }

        public _ZipCodes ZipCodes = new _ZipCodes();

        public static readonly _Apps TheOne = new _Apps();

    }

}
