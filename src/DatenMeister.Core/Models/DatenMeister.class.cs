#nullable enable
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Core.Models
{
    public class _PrimitiveTypes
    {
        public static readonly _PrimitiveTypes TheOne = new _PrimitiveTypes();
        public IElement @__DateTime = new MofObjectShadow("dm:///_internal/types/internal#PrimitiveTypes.DateTime");

        public _DateTime @DateTime = new _DateTime();

        public class _DateTime
        {
        }
    }

    public class _DatenMeister
    {
        public static readonly _DatenMeister TheOne = new _DatenMeister();

        public _Actions Actions = new _Actions();

        public _AttachedExtent AttachedExtent = new _AttachedExtent();

        public _CommonTypes CommonTypes = new _CommonTypes();

        public _DataViews DataViews = new _DataViews();

        public _DynamicRuntimeProvider DynamicRuntimeProvider = new _DynamicRuntimeProvider();

        public _ExtentLoaderConfigs ExtentLoaderConfigs = new _ExtentLoaderConfigs();

        public _FastViewFilters FastViewFilters = new _FastViewFilters();

        public _Forms Forms = new _Forms();

        public _Management Management = new _Management();

        public _Reports Reports = new _Reports();

        public _UserManagement UserManagement = new _UserManagement();

        public class _UserManagement
        {
            public IElement @__User =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Modules.UserManagement.User");

            public IElement @__UserManagementSettings =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Modules.UserManagement.UserManagementSettings");

            public _User @User = new _User();

            public _UserManagementSettings @UserManagementSettings = new _UserManagementSettings();

            public class _User
            {
                public static string @name = "name";

                public static string @password = "password";
                public IElement? @_name = null;
                public IElement? @_password = null;
            }

            public class _UserManagementSettings
            {
                public static string @salt = "salt";
                public IElement? @_salt = null;
            }
        }

        public class _CommonTypes
        {
            public _Default Default = new _Default();

            public _ExtentManager ExtentManager = new _ExtentManager();

            public _OSIntegration OSIntegration = new _OSIntegration();

            public class _Default
            {
                public IElement @__Package =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package");

                public _Package @Package = new _Package();

                public class _Package
                {
                    public static string @name = "name";

                    public static string @packagedElement = "packagedElement";

                    public static string @preferredType = "preferredType";

                    public static string @preferredPackage = "preferredPackage";

                    public static string @defaultViewMode = "defaultViewMode";
                    public IElement? @_defaultViewMode = null;
                    public IElement? @_name = null;
                    public IElement? @_packagedElement = null;
                    public IElement? @_preferredPackage = null;
                    public IElement? @_preferredType = null;
                }
            }

            public class _ExtentManager
            {
                public IElement @__ImportException =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException");

                public IElement @__ImportSettings =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings");

                public _ImportException @ImportException = new _ImportException();

                public _ImportSettings @ImportSettings = new _ImportSettings();

                public class _ImportSettings
                {
                    public static string @filePath = "filePath";

                    public static string @extentUri = "extentUri";

                    public static string @workspace = "workspace";
                    public IElement? @_extentUri = null;
                    public IElement? @_filePath = null;
                    public IElement? @_workspace = null;
                }

                public class _ImportException
                {
                    public static string @message = "message";
                    public IElement? @_message = null;
                }
            }

            public class _OSIntegration
            {
                public IElement @__CommandLineApplication =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication");

                public IElement @__EnvironmentalVariable =
                    new MofObjectShadow("dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable");

                public _CommandLineApplication @CommandLineApplication = new _CommandLineApplication();

                public _EnvironmentalVariable @EnvironmentalVariable = new _EnvironmentalVariable();

                public class _CommandLineApplication
                {
                    public static string @name = "name";

                    public static string @applicationPath = "applicationPath";
                    public IElement? @_applicationPath = null;
                    public IElement? @_name = null;
                }

                public class _EnvironmentalVariable
                {
                    public static string @name = "name";

                    public static string @value = "value";
                    public IElement? @_name = null;
                    public IElement? @_value = null;
                }
            }
        }

        public class _Actions
        {
            public IElement @__Action = new MofObjectShadow("dm:///_internal/types/internal#Actions.Action");
            public IElement @__ActionSet = new MofObjectShadow("dm:///_internal/types/internal#Actions.ActionSet");

            public IElement @__ClearCollectionAction =
                new MofObjectShadow("dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae");

            public IElement @__CommandExecutionAction =
                new MofObjectShadow("dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82");

            public IElement @__CopyElementsAction =
                new MofObjectShadow("dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed");

            public IElement @__CreateWorkspaceAction =
                new MofObjectShadow("dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe");

            public IElement @__DocumentOpenAction =
                new MofObjectShadow("dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74");

            public IElement @__DropExtentAction =
                new MofObjectShadow("dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09");

            public IElement @__DropWorkspaceAction =
                new MofObjectShadow("dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8");

            public IElement @__ExportToXmiAction =
                new MofObjectShadow("dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863");

            public IElement @__LoadExtentAction =
                new MofObjectShadow("dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee");

            public IElement @__LoggingWriterAction =
                new MofObjectShadow("dm:///_internal/types/internal#Actions.LoggingWriterAction");

            public IElement @__PowershellExecutionAction =
                new MofObjectShadow("dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb");

            public IElement @__TransformItemsAction =
                new MofObjectShadow("dm:///_internal/types/internal#Actions.ItemTransformationActionHandler");

            public _Action @Action = new _Action();

            public _ActionSet @ActionSet = new _ActionSet();

            public _ClearCollectionAction @ClearCollectionAction = new _ClearCollectionAction();

            public _CommandExecutionAction @CommandExecutionAction = new _CommandExecutionAction();

            public _CopyElementsAction @CopyElementsAction = new _CopyElementsAction();

            public _CreateWorkspaceAction @CreateWorkspaceAction = new _CreateWorkspaceAction();

            public _DocumentOpenAction @DocumentOpenAction = new _DocumentOpenAction();

            public _DropExtentAction @DropExtentAction = new _DropExtentAction();

            public _DropWorkspaceAction @DropWorkspaceAction = new _DropWorkspaceAction();

            public _ExportToXmiAction @ExportToXmiAction = new _ExportToXmiAction();

            public _LoadExtentAction @LoadExtentAction = new _LoadExtentAction();

            public _LoggingWriterAction @LoggingWriterAction = new _LoggingWriterAction();

            public _PowershellExecutionAction @PowershellExecutionAction = new _PowershellExecutionAction();

            public _Reports Reports = new _Reports();

            public _TransformItemsAction @TransformItemsAction = new _TransformItemsAction();

            public class _ActionSet
            {
                public static string @action = "action";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_action = null;
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
            }

            public class _LoggingWriterAction
            {
                public static string @message = "message";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;
                public IElement? @_message = null;
                public IElement? @_name = null;
            }

            public class _CommandExecutionAction
            {
                public static string @command = "command";

                public static string @arguments = "arguments";

                public static string @workingDirectory = "workingDirectory";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_arguments = null;
                public IElement? @_command = null;
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
                public IElement? @_workingDirectory = null;
            }

            public class _PowershellExecutionAction
            {
                public static string @script = "script";

                public static string @workingDirectory = "workingDirectory";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
                public IElement? @_script = null;
                public IElement? @_workingDirectory = null;
            }

            public class _LoadExtentAction
            {
                public static string @configuration = "configuration";

                public static string @dropExisting = "dropExisting";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_configuration = null;
                public IElement? @_dropExisting = null;
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
            }

            public class _DropExtentAction
            {
                public static string @workspace = "workspace";

                public static string @extentUri = "extentUri";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_extentUri = null;
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
                public IElement? @_workspace = null;
            }

            public class _CreateWorkspaceAction
            {
                public static string @workspace = "workspace";

                public static string @annotation = "annotation";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_annotation = null;
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
                public IElement? @_workspace = null;
            }

            public class _DropWorkspaceAction
            {
                public static string @workspace = "workspace";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
                public IElement? @_workspace = null;
            }

            public class _CopyElementsAction
            {
                public static string @sourcePath = "sourcePath";

                public static string @targetPath = "targetPath";

                public static string @moveOnly = "moveOnly";

                public static string @sourceWorkspace = "sourceWorkspace";

                public static string @targetWorkspace = "targetWorkspace";

                public static string @emptyTarget = "emptyTarget";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_emptyTarget = null;
                public IElement? @_isDisabled = null;
                public IElement? @_moveOnly = null;
                public IElement? @_name = null;
                public IElement? @_sourcePath = null;
                public IElement? @_sourceWorkspace = null;
                public IElement? @_targetPath = null;
                public IElement? @_targetWorkspace = null;
            }

            public class _ExportToXmiAction
            {
                public static string @sourcePath = "sourcePath";

                public static string @filePath = "filePath";

                public static string @sourceWorkspaceId = "sourceWorkspaceId";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_filePath = null;
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
                public IElement? @_sourcePath = null;
                public IElement? @_sourceWorkspaceId = null;
            }

            public class _ClearCollectionAction
            {
                public static string @workspace = "workspace";

                public static string @path = "path";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
                public IElement? @_path = null;
                public IElement? @_workspace = null;
            }

            public class _TransformItemsAction
            {
                public static string @metaClass = "metaClass";

                public static string @runtimeClass = "runtimeClass";

                public static string @workspace = "workspace";

                public static string @path = "path";

                public static string @excludeDescendents = "excludeDescendents";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_excludeDescendents = null;
                public IElement? @_isDisabled = null;
                public IElement? @_metaClass = null;
                public IElement? @_name = null;
                public IElement? @_path = null;
                public IElement? @_runtimeClass = null;
                public IElement? @_workspace = null;
            }

            public class _DocumentOpenAction
            {
                public static string @filePath = "filePath";

                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_filePath = null;
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
            }

            public class _Reports
            {
                public IElement @__AdocReportAction =
                    new MofObjectShadow("dm:///_internal/types/internal#Actions.AdocReportAction");

                public IElement @__HtmlReportAction =
                    new MofObjectShadow("dm:///_internal/types/internal#Actions.HtmlReportAction");

                public IElement @__SimpleReportAction =
                    new MofObjectShadow("dm:///_internal/types/internal#Actions.SimpleReportAction");

                public _AdocReportAction @AdocReportAction = new _AdocReportAction();

                public _HtmlReportAction @HtmlReportAction = new _HtmlReportAction();

                public _SimpleReportAction @SimpleReportAction = new _SimpleReportAction();

                public class _SimpleReportAction
                {
                    public static string @path = "path";

                    public static string @configuration = "configuration";

                    public static string @workspaceId = "workspaceId";

                    public static string @filePath = "filePath";

                    public static string @name = "name";

                    public static string @isDisabled = "isDisabled";
                    public IElement? @_configuration = null;
                    public IElement? @_filePath = null;
                    public IElement? @_isDisabled = null;
                    public IElement? @_name = null;
                    public IElement? @_path = null;
                    public IElement? @_workspaceId = null;
                }

                public class _AdocReportAction
                {
                    public static string @filePath = "filePath";

                    public static string @reportInstance = "reportInstance";

                    public static string @name = "name";

                    public static string @isDisabled = "isDisabled";
                    public IElement? @_filePath = null;
                    public IElement? @_isDisabled = null;
                    public IElement? @_name = null;
                    public IElement? @_reportInstance = null;
                }

                public class _HtmlReportAction
                {
                    public static string @filePath = "filePath";

                    public static string @reportInstance = "reportInstance";

                    public static string @name = "name";

                    public static string @isDisabled = "isDisabled";
                    public IElement? @_filePath = null;
                    public IElement? @_isDisabled = null;
                    public IElement? @_name = null;
                    public IElement? @_reportInstance = null;
                }
            }

            public class _Action
            {
                public static string @name = "name";

                public static string @isDisabled = "isDisabled";
                public IElement? @_isDisabled = null;
                public IElement? @_name = null;
            }
        }

        public class _DataViews
        {
            public enum ___ComparisonMode
            {
                @Equal,
                @NotEqual,
                @Contains,
                @DoesNotContain,
                @GreaterThan,
                @GreaterOrEqualThan,
                @LighterThan,
                @LighterOrEqualThan
            }

            public IElement @__ComparisonMode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode");

            public IElement @__DataView =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView");

            public IElement @__DynamicSourceNode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode");

            public IElement @__FilterPropertyNode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterPropertyNode");

            public IElement @__FilterTypeNode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterTypeNode");

            public IElement @__FlattenNode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode");

            public IElement @__SelectByFullNameNode =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode");

            public IElement @__SourceExtentNode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SourceExtentNode");

            public IElement @__ViewNode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode");

            public _ComparisonMode @ComparisonMode = new _ComparisonMode();

            public _DataView @DataView = new _DataView();

            public _DynamicSourceNode @DynamicSourceNode = new _DynamicSourceNode();

            public _FilterPropertyNode @FilterPropertyNode = new _FilterPropertyNode();

            public _FilterTypeNode @FilterTypeNode = new _FilterTypeNode();

            public _FlattenNode @FlattenNode = new _FlattenNode();

            public _SelectByFullNameNode @SelectByFullNameNode = new _SelectByFullNameNode();

            public _SourceExtentNode @SourceExtentNode = new _SourceExtentNode();

            public _ViewNode @ViewNode = new _ViewNode();

            public class _DataView
            {
                public static string @name = "name";

                public static string @workspace = "workspace";

                public static string @uri = "uri";

                public static string @viewNode = "viewNode";
                public IElement? @_name = null;
                public IElement? @_uri = null;
                public IElement? @_viewNode = null;
                public IElement? @_workspace = null;
            }

            public class _ViewNode
            {
                public static string @name = "name";
                public IElement? @_name = null;
            }

            public class _SourceExtentNode
            {
                public static string @extentUri = "extentUri";

                public static string @workspace = "workspace";

                public static string @name = "name";
                public IElement? @_extentUri = null;
                public IElement? @_name = null;
                public IElement? @_workspace = null;
            }

            public class _FlattenNode
            {
                public static string @input = "input";

                public static string @name = "name";
                public IElement? @_input = null;
                public IElement? @_name = null;
            }

            public class _FilterPropertyNode
            {
                public static string @input = "input";

                public static string @property = "property";

                public static string @value = "value";

                public static string @comparisonMode = "comparisonMode";

                public static string @name = "name";
                public IElement? @_comparisonMode = null;
                public IElement? @_input = null;
                public IElement? @_name = null;
                public IElement? @_property = null;
                public IElement? @_value = null;
            }

            public class _FilterTypeNode
            {
                public static string @input = "input";

                public static string @type = "type";

                public static string @includeInherits = "includeInherits";

                public static string @name = "name";
                public IElement? @_includeInherits = null;
                public IElement? @_input = null;
                public IElement? @_name = null;
                public IElement? @_type = null;
            }

            public class _ComparisonMode
            {
                public static string @Equal = "Equal";
                public static string @NotEqual = "NotEqual";
                public static string @Contains = "Contains";
                public static string @DoesNotContain = "DoesNotContain";
                public static string @GreaterThan = "GreaterThan";
                public static string @GreaterOrEqualThan = "GreaterOrEqualThan";
                public static string @LighterThan = "LighterThan";
                public static string @LighterOrEqualThan = "LighterOrEqualThan";

                public IElement @__Contains =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-Contains");

                public IElement @__DoesNotContain = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-DoesNotContain");

                public IElement @__Equal =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-Equal");

                public IElement @__GreaterOrEqualThan = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-GreaterOrEqualThan");

                public IElement @__GreaterThan =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-GreaterThan");

                public IElement @__LighterOrEqualThan = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-LighterOrEqualThan");

                public IElement @__LighterThan =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-LighterThan");

                public IElement @__NotEqual =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ComparisonMode-NotEqual");
            }

            public class _SelectByFullNameNode
            {
                public static string @input = "input";

                public static string @path = "path";

                public static string @name = "name";
                public IElement? @_input = null;
                public IElement? @_name = null;
                public IElement? @_path = null;
            }

            public class _DynamicSourceNode
            {
                public static string @nodeName = "nodeName";

                public static string @name = "name";
                public IElement? @_name = null;
                public IElement? @_nodeName = null;
            }
        }

        public class _Reports
        {
            public enum ___DescendentMode
            {
                @None,
                @Inline,
                @PerPackage
            }

            public IElement @__AdocReportInstance =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance");

            public IElement @__DescendentMode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode");

            public IElement @__HtmlReportInstance =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance");

            public IElement @__ReportDefinition =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition");

            public IElement @__ReportInstance =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance");

            public IElement @__ReportInstanceSource =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource");

            public IElement @__SimpleReportConfiguration = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration");

            public _AdocReportInstance @AdocReportInstance = new _AdocReportInstance();

            public _DescendentMode @DescendentMode = new _DescendentMode();

            public _Elements Elements = new _Elements();

            public _HtmlReportInstance @HtmlReportInstance = new _HtmlReportInstance();

            public _ReportDefinition @ReportDefinition = new _ReportDefinition();

            public _ReportInstance @ReportInstance = new _ReportInstance();

            public _ReportInstanceSource @ReportInstanceSource = new _ReportInstanceSource();

            public _SimpleReportConfiguration @SimpleReportConfiguration = new _SimpleReportConfiguration();

            public class _ReportDefinition
            {
                public static string @name = "name";

                public static string @title = "title";

                public static string @elements = "elements";
                public IElement? @_elements = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _ReportInstanceSource
            {
                public static string @name = "name";

                public static string @workspaceId = "workspaceId";

                public static string @path = "path";
                public IElement? @_name = null;
                public IElement? @_path = null;
                public IElement? @_workspaceId = null;
            }

            public class _ReportInstance
            {
                public static string @name = "name";

                public static string @reportDefinition = "reportDefinition";

                public static string @sources = "sources";
                public IElement? @_name = null;
                public IElement? @_reportDefinition = null;
                public IElement? @_sources = null;
            }

            public class _AdocReportInstance
            {
                public static string @name = "name";

                public static string @reportDefinition = "reportDefinition";

                public static string @sources = "sources";
                public IElement? @_name = null;
                public IElement? @_reportDefinition = null;
                public IElement? @_sources = null;
            }

            public class _HtmlReportInstance
            {
                public static string @name = "name";

                public static string @reportDefinition = "reportDefinition";

                public static string @sources = "sources";
                public IElement? @_name = null;
                public IElement? @_reportDefinition = null;
                public IElement? @_sources = null;
            }

            public class _DescendentMode
            {
                public static string @None = "None";
                public static string @Inline = "Inline";
                public static string @PerPackage = "PerPackage";

                public IElement @__Inline =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode-Inline");

                public IElement @__None =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode-None");

                public IElement @__PerPackage = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.DescendentMode-PerPackage");
            }

            public class _SimpleReportConfiguration
            {
                public static string @name = "name";

                public static string @showDescendents = "showDescendents";

                public static string @rootElement = "rootElement";

                public static string @showRootElement = "showRootElement";

                public static string @showMetaClasses = "showMetaClasses";

                public static string @showFullName = "showFullName";

                public static string @form = "form";

                public static string @descendentMode = "descendentMode";

                public static string @typeMode = "typeMode";

                public static string @workspaceId = "workspaceId";
                public IElement? @_descendentMode = null;
                public IElement? @_form = null;
                public IElement? @_name = null;
                public IElement? @_rootElement = null;
                public IElement? @_showDescendents = null;
                public IElement? @_showFullName = null;
                public IElement? @_showMetaClasses = null;
                public IElement? @_showRootElement = null;
                public IElement? @_typeMode = null;
                public IElement? @_workspaceId = null;
            }

            public class _Elements
            {
                public enum ___ReportTableForTypeMode
                {
                    @PerType,
                    @AllTypes
                }

                public IElement @__ReportElement =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement");

                public IElement @__ReportHeadline =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline");

                public IElement @__ReportLoop =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop");

                public IElement @__ReportParagraph =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph");

                public IElement @__ReportTable =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable");

                public IElement @__ReportTableForTypeMode = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode");

                public _ReportElement @ReportElement = new _ReportElement();

                public _ReportHeadline @ReportHeadline = new _ReportHeadline();

                public _ReportLoop @ReportLoop = new _ReportLoop();

                public _ReportParagraph @ReportParagraph = new _ReportParagraph();

                public _ReportTable @ReportTable = new _ReportTable();

                public _ReportTableForTypeMode @ReportTableForTypeMode = new _ReportTableForTypeMode();

                public class _ReportElement
                {
                    public static string @name = "name";
                    public IElement? @_name = null;
                }

                public class _ReportHeadline
                {
                    public static string @title = "title";

                    public static string @name = "name";
                    public IElement? @_name = null;
                    public IElement? @_title = null;
                }

                public class _ReportParagraph
                {
                    public static string @paragraph = "paragraph";

                    public static string @cssClass = "cssClass";

                    public static string @viewNode = "viewNode";

                    public static string @evalProperties = "evalProperties";

                    public static string @evalParagraph = "evalParagraph";

                    public static string @name = "name";
                    public IElement? @_cssClass = null;
                    public IElement? @_evalParagraph = null;
                    public IElement? @_evalProperties = null;
                    public IElement? @_name = null;
                    public IElement? @_paragraph = null;
                    public IElement? @_viewNode = null;
                }

                public class _ReportTable
                {
                    public static string @cssClass = "cssClass";

                    public static string @viewNode = "viewNode";

                    public static string @form = "form";

                    public static string @evalProperties = "evalProperties";

                    public static string @name = "name";
                    public IElement? @_cssClass = null;
                    public IElement? @_evalProperties = null;
                    public IElement? @_form = null;
                    public IElement? @_name = null;
                    public IElement? @_viewNode = null;
                }

                public class _ReportTableForTypeMode
                {
                    public static string @PerType = "PerType";
                    public static string @AllTypes = "AllTypes";

                    public IElement @__AllTypes = new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode-AllTypes");

                    public IElement @__PerType = new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.ReportTableForTypeMode-PerType");
                }

                public class _ReportLoop
                {
                    public static string @viewNode = "viewNode";

                    public static string @elements = "elements";

                    public static string @name = "name";
                    public IElement? @_elements = null;
                    public IElement? @_name = null;
                    public IElement? @_viewNode = null;
                }
            }
        }

        public class _ExtentLoaderConfigs
        {
            public IElement @__CsvExtentLoaderConfig = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig");

            public IElement @__CsvSettings =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings");

            public IElement @__EnvironmentalVariableLoaderConfig =
                new MofObjectShadow("dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3");

            public IElement @__ExcelColumn =
                new MofObjectShadow("dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a");

            public IElement @__ExcelExtentLoaderConfig = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig");

            public IElement @__ExcelHierarchicalColumnDefinition =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition");

            public IElement @__ExcelHierarchicalLoaderConfig =
                new MofObjectShadow("dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig");

            public IElement @__ExcelImportLoaderConfig = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig");

            public IElement @__ExcelLoaderConfig =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig");

            public IElement @__ExcelReferenceLoaderConfig = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig");

            public IElement @__ExtentFileLoaderConfig = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig");

            public IElement @__ExtentLoaderConfig =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig");

            public IElement @__InMemoryLoaderConfig =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig");

            public IElement @__XmiStorageLoaderConfig = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");

            public IElement @__XmlReferenceLoaderConfig = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig");

            public _CsvExtentLoaderConfig @CsvExtentLoaderConfig = new _CsvExtentLoaderConfig();

            public _CsvSettings @CsvSettings = new _CsvSettings();

            public _EnvironmentalVariableLoaderConfig @EnvironmentalVariableLoaderConfig =
                new _EnvironmentalVariableLoaderConfig();

            public _ExcelColumn @ExcelColumn = new _ExcelColumn();

            public _ExcelExtentLoaderConfig @ExcelExtentLoaderConfig = new _ExcelExtentLoaderConfig();

            public _ExcelHierarchicalColumnDefinition @ExcelHierarchicalColumnDefinition =
                new _ExcelHierarchicalColumnDefinition();

            public _ExcelHierarchicalLoaderConfig @ExcelHierarchicalLoaderConfig = new _ExcelHierarchicalLoaderConfig();

            public _ExcelImportLoaderConfig @ExcelImportLoaderConfig = new _ExcelImportLoaderConfig();

            public _ExcelLoaderConfig @ExcelLoaderConfig = new _ExcelLoaderConfig();

            public _ExcelReferenceLoaderConfig @ExcelReferenceLoaderConfig = new _ExcelReferenceLoaderConfig();

            public _ExtentFileLoaderConfig @ExtentFileLoaderConfig = new _ExtentFileLoaderConfig();

            public _ExtentLoaderConfig @ExtentLoaderConfig = new _ExtentLoaderConfig();

            public _InMemoryLoaderConfig @InMemoryLoaderConfig = new _InMemoryLoaderConfig();

            public _XmiStorageLoaderConfig @XmiStorageLoaderConfig = new _XmiStorageLoaderConfig();

            public _XmlReferenceLoaderConfig @XmlReferenceLoaderConfig = new _XmlReferenceLoaderConfig();

            public class _ExtentLoaderConfig
            {
                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_name = null;
                public IElement? @_workspaceId = null;
            }

            public class _ExcelLoaderConfig
            {
                public static string @fixRowCount = "fixRowCount";

                public static string @fixColumnCount = "fixColumnCount";

                public static string @filePath = "filePath";

                public static string @sheetName = "sheetName";

                public static string @offsetRow = "offsetRow";

                public static string @offsetColumn = "offsetColumn";

                public static string @countRows = "countRows";

                public static string @countColumns = "countColumns";

                public static string @hasHeader = "hasHeader";

                public static string @tryMergedHeaderCells = "tryMergedHeaderCells";

                public static string @onlySetColumns = "onlySetColumns";

                public static string @idColumnName = "idColumnName";

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";

                public static string @columns = "columns";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_columns = null;
                public IElement? @_countColumns = null;
                public IElement? @_countRows = null;
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_fixColumnCount = null;
                public IElement? @_fixRowCount = null;
                public IElement? @_hasHeader = null;
                public IElement? @_idColumnName = null;
                public IElement? @_name = null;
                public IElement? @_offsetColumn = null;
                public IElement? @_offsetRow = null;
                public IElement? @_onlySetColumns = null;
                public IElement? @_sheetName = null;
                public IElement? @_skipEmptyRowsCount = null;
                public IElement? @_tryMergedHeaderCells = null;
                public IElement? @_workspaceId = null;
            }

            public class _ExcelReferenceLoaderConfig
            {
                public static string @fixRowCount = "fixRowCount";

                public static string @fixColumnCount = "fixColumnCount";

                public static string @filePath = "filePath";

                public static string @sheetName = "sheetName";

                public static string @offsetRow = "offsetRow";

                public static string @offsetColumn = "offsetColumn";

                public static string @countRows = "countRows";

                public static string @countColumns = "countColumns";

                public static string @hasHeader = "hasHeader";

                public static string @tryMergedHeaderCells = "tryMergedHeaderCells";

                public static string @onlySetColumns = "onlySetColumns";

                public static string @idColumnName = "idColumnName";

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";

                public static string @columns = "columns";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_columns = null;
                public IElement? @_countColumns = null;
                public IElement? @_countRows = null;
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_fixColumnCount = null;
                public IElement? @_fixRowCount = null;
                public IElement? @_hasHeader = null;
                public IElement? @_idColumnName = null;
                public IElement? @_name = null;
                public IElement? @_offsetColumn = null;
                public IElement? @_offsetRow = null;
                public IElement? @_onlySetColumns = null;
                public IElement? @_sheetName = null;
                public IElement? @_skipEmptyRowsCount = null;
                public IElement? @_tryMergedHeaderCells = null;
                public IElement? @_workspaceId = null;
            }

            public class _ExcelImportLoaderConfig
            {
                public static string @extentPath = "extentPath";

                public static string @fixRowCount = "fixRowCount";

                public static string @fixColumnCount = "fixColumnCount";

                public static string @filePath = "filePath";

                public static string @sheetName = "sheetName";

                public static string @offsetRow = "offsetRow";

                public static string @offsetColumn = "offsetColumn";

                public static string @countRows = "countRows";

                public static string @countColumns = "countColumns";

                public static string @hasHeader = "hasHeader";

                public static string @tryMergedHeaderCells = "tryMergedHeaderCells";

                public static string @onlySetColumns = "onlySetColumns";

                public static string @idColumnName = "idColumnName";

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";

                public static string @columns = "columns";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_columns = null;
                public IElement? @_countColumns = null;
                public IElement? @_countRows = null;
                public IElement? @_dropExisting = null;
                public IElement? @_extentPath = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_fixColumnCount = null;
                public IElement? @_fixRowCount = null;
                public IElement? @_hasHeader = null;
                public IElement? @_idColumnName = null;
                public IElement? @_name = null;
                public IElement? @_offsetColumn = null;
                public IElement? @_offsetRow = null;
                public IElement? @_onlySetColumns = null;
                public IElement? @_sheetName = null;
                public IElement? @_skipEmptyRowsCount = null;
                public IElement? @_tryMergedHeaderCells = null;
                public IElement? @_workspaceId = null;
            }

            public class _ExcelExtentLoaderConfig
            {
                public static string @filePath = "filePath";

                public static string @idColumnName = "idColumnName";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_idColumnName = null;
                public IElement? @_name = null;
                public IElement? @_workspaceId = null;
            }

            public class _InMemoryLoaderConfig
            {
                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_name = null;
                public IElement? @_workspaceId = null;
            }

            public class _XmlReferenceLoaderConfig
            {
                public static string @filePath = "filePath";

                public static string @keepNamespaces = "keepNamespaces";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_keepNamespaces = null;
                public IElement? @_name = null;
                public IElement? @_workspaceId = null;
            }

            public class _ExtentFileLoaderConfig
            {
                public static string @filePath = "filePath";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_name = null;
                public IElement? @_workspaceId = null;
            }

            public class _XmiStorageLoaderConfig
            {
                public static string @filePath = "filePath";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_name = null;
                public IElement? @_workspaceId = null;
            }

            public class _CsvExtentLoaderConfig
            {
                public static string @settings = "settings";

                public static string @filePath = "filePath";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_name = null;
                public IElement? @_settings = null;
                public IElement? @_workspaceId = null;
            }

            public class _CsvSettings
            {
                public static string @encoding = "encoding";

                public static string @hasHeader = "hasHeader";

                public static string @separator = "separator";

                public static string @columns = "columns";

                public static string @metaclassUri = "metaclassUri";
                public IElement? @_columns = null;
                public IElement? @_encoding = null;
                public IElement? @_hasHeader = null;
                public IElement? @_metaclassUri = null;
                public IElement? @_separator = null;
            }

            public class _ExcelHierarchicalColumnDefinition
            {
                public static string @name = "name";

                public static string @metaClass = "metaClass";

                public static string @property = "property";
                public IElement? @_metaClass = null;
                public IElement? @_name = null;
                public IElement? @_property = null;
            }

            public class _ExcelHierarchicalLoaderConfig
            {
                public static string @hierarchicalColumns = "hierarchicalColumns";

                public static string @skipElementsForLastLevel = "skipElementsForLastLevel";

                public static string @fixRowCount = "fixRowCount";

                public static string @fixColumnCount = "fixColumnCount";

                public static string @filePath = "filePath";

                public static string @sheetName = "sheetName";

                public static string @offsetRow = "offsetRow";

                public static string @offsetColumn = "offsetColumn";

                public static string @countRows = "countRows";

                public static string @countColumns = "countColumns";

                public static string @hasHeader = "hasHeader";

                public static string @tryMergedHeaderCells = "tryMergedHeaderCells";

                public static string @onlySetColumns = "onlySetColumns";

                public static string @idColumnName = "idColumnName";

                public static string @skipEmptyRowsCount = "skipEmptyRowsCount";

                public static string @columns = "columns";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_columns = null;
                public IElement? @_countColumns = null;
                public IElement? @_countRows = null;
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_filePath = null;
                public IElement? @_fixColumnCount = null;
                public IElement? @_fixRowCount = null;
                public IElement? @_hasHeader = null;
                public IElement? @_hierarchicalColumns = null;
                public IElement? @_idColumnName = null;
                public IElement? @_name = null;
                public IElement? @_offsetColumn = null;
                public IElement? @_offsetRow = null;
                public IElement? @_onlySetColumns = null;
                public IElement? @_sheetName = null;
                public IElement? @_skipElementsForLastLevel = null;
                public IElement? @_skipEmptyRowsCount = null;
                public IElement? @_tryMergedHeaderCells = null;
                public IElement? @_workspaceId = null;
            }

            public class _ExcelColumn
            {
                public static string @header = "header";

                public static string @name = "name";
                public IElement? @_header = null;
                public IElement? @_name = null;
            }

            public class _EnvironmentalVariableLoaderConfig
            {
                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_name = null;
                public IElement? @_workspaceId = null;
            }
        }

        public class _Forms
        {
            public enum ___FormType
            {
                @Detail,
                @TreeItemExtent,
                @TreeItemDetail,
                @ObjectList,
                @TreeItemExtentExtension,
                @TreeItemDetailExtension
            }

            public IElement @__ActionFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData");

            public IElement @__AnyDataFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData");

            public IElement @__CheckboxFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData");

            public IElement @__CheckboxListTaggingFieldData =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData");

            public IElement @__DateTimeFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData");

            public IElement @__DefaultTypeForNewElement =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement");

            public IElement @__DetailForm =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.DetailForm");

            public IElement @__DropDownFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData");

            public IElement @__EvalTextFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData");

            public IElement @__ExtentForm =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ExtentForm");

            public IElement @__FieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData");

            public IElement @__FileSelectionFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData");

            public IElement @__Form =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.Form");

            public IElement @__FormAssociation =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation");

            public IElement @__FormType =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType");

            public IElement @__FullNameFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData");

            public IElement @__ListForm =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ListForm");

            public IElement @__MetaClassElementFieldData =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData");

            public IElement @__NumberFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData");

            public IElement @__ReferenceFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData");

            public IElement @__SeparatorLineFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData");

            public IElement @__SortingOrder =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder");

            public IElement @__SubElementFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData");

            public IElement @__TextFieldData =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData");

            public IElement @__ValuePair =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair");

            public IElement @__ViewMode =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode");

            public _ActionFieldData @ActionFieldData = new _ActionFieldData();

            public _AnyDataFieldData @AnyDataFieldData = new _AnyDataFieldData();

            public _CheckboxFieldData @CheckboxFieldData = new _CheckboxFieldData();

            public _CheckboxListTaggingFieldData @CheckboxListTaggingFieldData = new _CheckboxListTaggingFieldData();

            public _DateTimeFieldData @DateTimeFieldData = new _DateTimeFieldData();

            public _DefaultTypeForNewElement @DefaultTypeForNewElement = new _DefaultTypeForNewElement();

            public _DetailForm @DetailForm = new _DetailForm();

            public _DropDownFieldData @DropDownFieldData = new _DropDownFieldData();

            public _EvalTextFieldData @EvalTextFieldData = new _EvalTextFieldData();

            public _ExtentForm @ExtentForm = new _ExtentForm();

            public _FieldData @FieldData = new _FieldData();

            public _FileSelectionFieldData @FileSelectionFieldData = new _FileSelectionFieldData();

            public _Form @Form = new _Form();

            public _FormAssociation @FormAssociation = new _FormAssociation();

            public _FormType @FormType = new _FormType();

            public _FullNameFieldData @FullNameFieldData = new _FullNameFieldData();

            public _ListForm @ListForm = new _ListForm();

            public _MetaClassElementFieldData @MetaClassElementFieldData = new _MetaClassElementFieldData();

            public _NumberFieldData @NumberFieldData = new _NumberFieldData();

            public _ReferenceFieldData @ReferenceFieldData = new _ReferenceFieldData();

            public _SeparatorLineFieldData @SeparatorLineFieldData = new _SeparatorLineFieldData();

            public _SortingOrder @SortingOrder = new _SortingOrder();

            public _SubElementFieldData @SubElementFieldData = new _SubElementFieldData();

            public _TextFieldData @TextFieldData = new _TextFieldData();

            public _ValuePair @ValuePair = new _ValuePair();

            public _ViewMode @ViewMode = new _ViewMode();

            public class _FieldData
            {
                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _SortingOrder
            {
                public static string @name = "name";

                public static string @isDescending = "isDescending";
                public IElement? @_isDescending = null;
                public IElement? @_name = null;
            }

            public class _AnyDataFieldData
            {
                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _CheckboxFieldData
            {
                public static string @lineHeight = "lineHeight";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_lineHeight = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _ActionFieldData
            {
                public static string @actionName = "actionName";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_actionName = null;
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _DateTimeFieldData
            {
                public static string @hideDate = "hideDate";

                public static string @hideTime = "hideTime";

                public static string @showOffsetButtons = "showOffsetButtons";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_hideDate = null;
                public IElement? @_hideTime = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_showOffsetButtons = null;
                public IElement? @_title = null;
            }

            public class _FormAssociation
            {
                public static string @name = "name";

                public static string @formType = "formType";

                public static string @metaClass = "metaClass";

                public static string @extentType = "extentType";

                public static string @viewModeId = "viewModeId";

                public static string @parentMetaClass = "parentMetaClass";

                public static string @parentProperty = "parentProperty";

                public static string @form = "form";
                public IElement? @_extentType = null;
                public IElement? @_form = null;
                public IElement? @_formType = null;
                public IElement? @_metaClass = null;
                public IElement? @_name = null;
                public IElement? @_parentMetaClass = null;
                public IElement? @_parentProperty = null;
                public IElement? @_viewModeId = null;
            }

            public class _DropDownFieldData
            {
                public static string @values = "values";

                public static string @valuesByEnumeration = "valuesByEnumeration";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
                public IElement? @_values = null;
                public IElement? @_valuesByEnumeration = null;
            }

            public class _ValuePair
            {
                public static string @value = "value";

                public static string @name = "name";
                public IElement? @_name = null;
                public IElement? @_value = null;
            }

            public class _MetaClassElementFieldData
            {
                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _ReferenceFieldData
            {
                public static string @isSelectionInline = "isSelectionInline";

                public static string @defaultExtentUri = "defaultExtentUri";

                public static string @defaultWorkspace = "defaultWorkspace";

                public static string @showAllChildren = "showAllChildren";

                public static string @showWorkspaceSelection = "showWorkspaceSelection";

                public static string @showExtentSelection = "showExtentSelection";

                public static string @metaClassFilter = "metaClassFilter";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultExtentUri = null;
                public IElement? @_defaultValue = null;
                public IElement? @_defaultWorkspace = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_isSelectionInline = null;
                public IElement? @_metaClassFilter = null;
                public IElement? @_name = null;
                public IElement? @_showAllChildren = null;
                public IElement? @_showExtentSelection = null;
                public IElement? @_showWorkspaceSelection = null;
                public IElement? @_title = null;
            }

            public class _SubElementFieldData
            {
                public static string @metaClass = "metaClass";

                public static string @form = "form";

                public static string @allowOnlyExistingElements = "allowOnlyExistingElements";

                public static string @defaultTypesForNewElements = "defaultTypesForNewElements";

                public static string @includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";

                public static string @defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";

                public static string @defaultExtentOfNewElements = "defaultExtentOfNewElements";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_allowOnlyExistingElements = null;
                public IElement? @_defaultExtentOfNewElements = null;
                public IElement? @_defaultTypesForNewElements = null;
                public IElement? @_defaultValue = null;
                public IElement? @_defaultWorkspaceOfNewElements = null;
                public IElement? @_form = null;
                public IElement? @_includeSpecializationsForDefaultTypes = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_metaClass = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _TextFieldData
            {
                public static string @lineHeight = "lineHeight";

                public static string @width = "width";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_lineHeight = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
                public IElement? @_width = null;
            }

            public class _EvalTextFieldData
            {
                public static string @evalCellProperties = "evalCellProperties";

                public static string @lineHeight = "lineHeight";

                public static string @width = "width";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_evalCellProperties = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_lineHeight = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
                public IElement? @_width = null;
            }

            public class _SeparatorLineFieldData
            {
                public static string @Height = "Height";
                public IElement? @_Height = null;
            }

            public class _FileSelectionFieldData
            {
                public static string @defaultExtension = "defaultExtension";

                public static string @isSaving = "isSaving";

                public static string @initialPathToDirectory = "initialPathToDirectory";

                public static string @filter = "filter";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultExtension = null;
                public IElement? @_defaultValue = null;
                public IElement? @_filter = null;
                public IElement? @_initialPathToDirectory = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_isSaving = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _DefaultTypeForNewElement
            {
                public static string @name = "name";

                public static string @metaClass = "metaClass";

                public static string @parentProperty = "parentProperty";
                public IElement? @_metaClass = null;
                public IElement? @_name = null;
                public IElement? @_parentProperty = null;
            }

            public class _FullNameFieldData
            {
                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _CheckboxListTaggingFieldData
            {
                public static string @values = "values";

                public static string @separator = "separator";

                public static string @containsFreeText = "containsFreeText";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_containsFreeText = null;
                public IElement? @_defaultValue = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_separator = null;
                public IElement? @_title = null;
                public IElement? @_values = null;
            }

            public class _NumberFieldData
            {
                public static string @format = "format";

                public static string @isInteger = "isInteger";

                public static string @isAttached = "isAttached";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isEnumeration = "isEnumeration";

                public static string @defaultValue = "defaultValue";

                public static string @isReadOnly = "isReadOnly";
                public IElement? @_defaultValue = null;
                public IElement? @_format = null;
                public IElement? @_isAttached = null;
                public IElement? @_isEnumeration = null;
                public IElement? @_isInteger = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _FormType
            {
                public static string @Detail = "Detail";
                public static string @TreeItemExtent = "TreeItemExtent";
                public static string @TreeItemDetail = "TreeItemDetail";
                public static string @ObjectList = "ObjectList";
                public static string @TreeItemExtentExtension = "TreeItemExtentExtension";
                public static string @TreeItemDetailExtension = "TreeItemDetailExtension";

                public IElement @__Detail =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-Detail");

                public IElement @__ObjectList =
                    new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-ObjectList");

                public IElement @__TreeItemDetail =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-TreeItemDetail");

                public IElement? @__TreeItemDetailExtension = null;

                public IElement @__TreeItemExtent =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormType-TreeItemExtent");

                public IElement? @__TreeItemExtentExtension = null;
            }

            public class _Form
            {
                public static string @name = "name";

                public static string @title = "title";

                public static string @isReadOnly = "isReadOnly";

                public static string @hideMetaInformation = "hideMetaInformation";

                public static string @originalUri = "originalUri";
                public IElement? @_hideMetaInformation = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_originalUri = null;
                public IElement? @_title = null;
            }

            public class _DetailForm
            {
                public static string @buttonApplyText = "buttonApplyText";

                public static string @allowNewProperties = "allowNewProperties";

                public static string @defaultWidth = "defaultWidth";

                public static string @defaultHeight = "defaultHeight";

                public static string @tab = "tab";

                public static string @field = "field";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isReadOnly = "isReadOnly";

                public static string @hideMetaInformation = "hideMetaInformation";

                public static string @originalUri = "originalUri";
                public IElement? @_allowNewProperties = null;
                public IElement? @_buttonApplyText = null;
                public IElement? @_defaultHeight = null;
                public IElement? @_defaultWidth = null;
                public IElement? @_field = null;
                public IElement? @_hideMetaInformation = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_originalUri = null;
                public IElement? @_tab = null;
                public IElement? @_title = null;
            }

            public class _ListForm
            {
                public static string @property = "property";

                public static string @metaClass = "metaClass";

                public static string @includeDescendents = "includeDescendents";

                public static string @noItemsWithMetaClass = "noItemsWithMetaClass";

                public static string @inhibitNewItems = "inhibitNewItems";

                public static string @inhibitDeleteItems = "inhibitDeleteItems";

                public static string @inhibitEditItems = "inhibitEditItems";

                public static string @defaultTypesForNewElements = "defaultTypesForNewElements";

                public static string @fastViewFilters = "fastViewFilters";

                public static string @field = "field";

                public static string @sortingOrder = "sortingOrder";

                public static string @viewNode = "viewNode";

                public static string @autoGenerateFields = "autoGenerateFields";

                public static string @duplicatePerType = "duplicatePerType";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isReadOnly = "isReadOnly";

                public static string @hideMetaInformation = "hideMetaInformation";

                public static string @originalUri = "originalUri";
                public IElement? @_autoGenerateFields = null;
                public IElement? @_defaultTypesForNewElements = null;
                public IElement? @_duplicatePerType = null;
                public IElement? @_fastViewFilters = null;
                public IElement? @_field = null;
                public IElement? @_hideMetaInformation = null;
                public IElement? @_includeDescendents = null;
                public IElement? @_inhibitDeleteItems = null;
                public IElement? @_inhibitEditItems = null;
                public IElement? @_inhibitNewItems = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_metaClass = null;
                public IElement? @_name = null;
                public IElement? @_noItemsWithMetaClass = null;
                public IElement? @_originalUri = null;
                public IElement? @_property = null;
                public IElement? @_sortingOrder = null;
                public IElement? @_title = null;
                public IElement? @_viewNode = null;
            }

            public class _ExtentForm
            {
                public static string @tab = "tab";

                public static string @autoTabs = "autoTabs";

                public static string @name = "name";

                public static string @title = "title";

                public static string @isReadOnly = "isReadOnly";

                public static string @hideMetaInformation = "hideMetaInformation";

                public static string @originalUri = "originalUri";
                public IElement? @_autoTabs = null;
                public IElement? @_hideMetaInformation = null;
                public IElement? @_isReadOnly = null;
                public IElement? @_name = null;
                public IElement? @_originalUri = null;
                public IElement? @_tab = null;
                public IElement? @_title = null;
            }

            public class _ViewMode
            {
                public static string @name = "name";

                public static string @id = "id";

                public static string @defaultExtentType = "defaultExtentType";
                public IElement? @_defaultExtentType = null;
                public IElement? @_id = null;
                public IElement? @_name = null;
            }
        }

        public class _AttachedExtent
        {
            public IElement @__AttachedExtentConfiguration = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration");

            public _AttachedExtentConfiguration @AttachedExtentConfiguration = new _AttachedExtentConfiguration();

            public class _AttachedExtentConfiguration
            {
                public static string @name = "name";

                public static string @referencedWorkspace = "referencedWorkspace";

                public static string @referencedExtent = "referencedExtent";

                public static string @referenceType = "referenceType";

                public static string @referenceProperty = "referenceProperty";
                public IElement? @_name = null;
                public IElement? @_referencedExtent = null;
                public IElement? @_referencedWorkspace = null;
                public IElement? @_referenceProperty = null;
                public IElement? @_referenceType = null;
            }
        }

        public class _Management
        {
            public enum ___ExtentLoadingState
            {
                @Unknown,
                @Unloaded,
                @Loaded,
                @Failed,
                @LoadedReadOnly
            }

            public IElement @__CreateNewWorkspaceModel = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel");

            public IElement @__Extent =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent");

            public IElement @__ExtentLoadingState =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState");

            public IElement @__ExtentProperties =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties");

            public IElement @__ExtentPropertyDefinition =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition");

            public IElement @__ExtentSettings =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings");

            public IElement @__ExtentTypeSetting =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting");

            public IElement @__Workspace =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace");

            public _CreateNewWorkspaceModel @CreateNewWorkspaceModel = new _CreateNewWorkspaceModel();

            public _Extent @Extent = new _Extent();

            public _ExtentLoadingState @ExtentLoadingState = new _ExtentLoadingState();

            public _ExtentProperties @ExtentProperties = new _ExtentProperties();

            public _ExtentPropertyDefinition @ExtentPropertyDefinition = new _ExtentPropertyDefinition();

            public _ExtentSettings @ExtentSettings = new _ExtentSettings();

            public _ExtentTypeSetting @ExtentTypeSetting = new _ExtentTypeSetting();

            public _Workspace @Workspace = new _Workspace();

            public class _ExtentLoadingState
            {
                public static string @Unknown = "Unknown";
                public static string @Unloaded = "Unloaded";
                public static string @Loaded = "Loaded";
                public static string @Failed = "Failed";
                public static string @LoadedReadOnly = "LoadedReadOnly";

                public IElement @__Failed = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Failed");

                public IElement @__Loaded = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Loaded");

                public IElement @__LoadedReadOnly = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-LoadedReadOnly");

                public IElement @__Unknown = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Unknown");

                public IElement @__Unloaded = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Runtime.ExtentStorage.ExtentLoadingState-Unloaded");
            }

            public class _Extent
            {
                public static string @name = "name";

                public static string @uri = "uri";

                public static string @workspaceId = "workspaceId";

                public static string @count = "count";

                public static string @totalCount = "totalCount";

                public static string @type = "type";

                public static string @extentType = "extentType";

                public static string @isModified = "isModified";

                public static string @alternativeUris = "alternativeUris";

                public static string @autoEnumerateType = "autoEnumerateType";

                public static string @state = "state";

                public static string @failMessage = "failMessage";

                public static string properties = "properties";
                public IElement? @_alternativeUris = null;
                public IElement? @_autoEnumerateType = null;
                public IElement? @_count = null;
                public IElement? @_extentType = null;
                public IElement? @_failMessage = null;
                public IElement? @_isModified = null;
                public IElement? @_name = null;
                public IElement? _properties = null;
                public IElement? @_state = null;
                public IElement? @_totalCount = null;
                public IElement? @_type = null;
                public IElement? @_uri = null;
                public IElement? @_workspaceId = null;
            }

            public class _Workspace
            {
                public static string @id = "id";

                public static string @annotation = "annotation";

                public static string @extents = "extents";
                public IElement? @_annotation = null;
                public IElement? @_extents = null;
                public IElement? @_id = null;
            }

            public class _CreateNewWorkspaceModel
            {
                public static string @id = "id";

                public static string @annotation = "annotation";
                public IElement? @_annotation = null;
                public IElement? @_id = null;
            }

            public class _ExtentTypeSetting
            {
                public static string @name = "name";

                public static string @rootElementMetaClasses = "rootElementMetaClasses";
                public IElement? @_name = null;
                public IElement? @_rootElementMetaClasses = null;
            }

            public class _ExtentProperties
            {
                public static string @name = "name";

                public static string @uri = "uri";

                public static string @workspaceId = "workspaceId";

                public static string @count = "count";

                public static string @totalCount = "totalCount";

                public static string @type = "type";

                public static string @extentType = "extentType";

                public static string @isModified = "isModified";

                public static string @alternativeUris = "alternativeUris";

                public static string @autoEnumerateType = "autoEnumerateType";

                public static string @state = "state";

                public static string @failMessage = "failMessage";

                public static string properties = "properties";
                public IElement? @_alternativeUris = null;
                public IElement? @_autoEnumerateType = null;
                public IElement? @_count = null;
                public IElement? @_extentType = null;
                public IElement? @_failMessage = null;
                public IElement? @_isModified = null;
                public IElement? @_name = null;
                public IElement? _properties = null;
                public IElement? @_state = null;
                public IElement? @_totalCount = null;
                public IElement? @_type = null;
                public IElement? @_uri = null;
                public IElement? @_workspaceId = null;
            }

            public class _ExtentPropertyDefinition
            {
                public static string @name = "name";

                public static string @title = "title";

                public static string @metaClass = "metaClass";
                public IElement? @_metaClass = null;
                public IElement? @_name = null;
                public IElement? @_title = null;
            }

            public class _ExtentSettings
            {
                public static string @extentTypeSettings = "extentTypeSettings";

                public static string @propertyDefinitions = "propertyDefinitions";
                public IElement? @_extentTypeSettings = null;
                public IElement? @_propertyDefinitions = null;
            }
        }

        public class _FastViewFilters
        {
            public enum ___ComparisonType
            {
                @Equal,
                @GreaterThan,
                @LighterThan,
                @GreaterOrEqualThan,
                @LighterOrEqualThan
            }

            public IElement @__ComparisonType =
                new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType");

            public IElement @__PropertyComparisonFilter = new MofObjectShadow(
                "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter");

            public IElement @__PropertyContainsFilter =
                new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter");

            public _ComparisonType @ComparisonType = new _ComparisonType();

            public _PropertyComparisonFilter @PropertyComparisonFilter = new _PropertyComparisonFilter();

            public _PropertyContainsFilter @PropertyContainsFilter = new _PropertyContainsFilter();

            public class _ComparisonType
            {
                public static string @Equal = "Equal";
                public static string @GreaterThan = "GreaterThan";
                public static string @LighterThan = "LighterThan";
                public static string @GreaterOrEqualThan = "GreaterOrEqualThan";
                public static string @LighterOrEqualThan = "LighterOrEqualThan";

                public IElement @__Equal =
                    new MofObjectShadow(
                        "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-Equal");

                public IElement @__GreaterOrEqualThan = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-GreaterOrEqualThan");

                public IElement @__GreaterThan = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-GreaterThan");

                public IElement @__LighterOrEqualThan = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-LighterOrEqualThan");

                public IElement @__LighterThan = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.ComparisonType-LighterThan");
            }

            public class _PropertyComparisonFilter
            {
                public static string @Property = "Property";

                public static string @ComparisonType = "ComparisonType";

                public static string @Value = "Value";
                public IElement? @_ComparisonType = null;
                public IElement? @_Property = null;
                public IElement? @_Value = null;
            }

            public class _PropertyContainsFilter
            {
                public static string @Property = "Property";

                public static string @Value = "Value";
                public IElement? @_Property = null;
                public IElement? @_Value = null;
            }
        }

        public class _DynamicRuntimeProvider
        {
            public IElement @__DynamicRuntimeLoaderConfig =
                new MofObjectShadow("dm:///_internal/types/internal#8be3c0ea-ef40-4b4a-a4ea-9262e924d7b8");

            public _DynamicRuntimeLoaderConfig @DynamicRuntimeLoaderConfig = new _DynamicRuntimeLoaderConfig();

            public _Examples Examples = new _Examples();

            public class _DynamicRuntimeLoaderConfig
            {
                public static string @runtimeClass = "runtimeClass";

                public static string @configuration = "configuration";

                public static string @name = "name";

                public static string @extentUri = "extentUri";

                public static string @workspaceId = "workspaceId";

                public static string @dropExisting = "dropExisting";
                public IElement? @_configuration = null;
                public IElement? @_dropExisting = null;
                public IElement? @_extentUri = null;
                public IElement? @_name = null;
                public IElement? @_runtimeClass = null;
                public IElement? @_workspaceId = null;
            }

            public class _Examples
            {
                public IElement @__NumberProviderSettings =
                    new MofObjectShadow("dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a");

                public IElement @__NumberRepresentation = new MofObjectShadow(
                    "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation");

                public _NumberProviderSettings @NumberProviderSettings = new _NumberProviderSettings();

                public _NumberRepresentation @NumberRepresentation = new _NumberRepresentation();

                public class _NumberProviderSettings
                {
                    public static string @name = "name";

                    public static string @start = "start";

                    public static string @end = "end";
                    public IElement? @_end = null;
                    public IElement? @_name = null;
                    public IElement? @_start = null;
                }

                public class _NumberRepresentation
                {
                    public static string @binary = "binary";

                    public static string @octal = "octal";

                    public static string @decimal = "decimal";

                    public static string @hexadecimal = "hexadecimal";
                    public IElement? @_binary = null;
                    public IElement? @_decimal = null;
                    public IElement? @_hexadecimal = null;
                    public IElement? @_octal = null;
                }
            }
        }
    }
}