define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports._DatenMeister = exports._PrimitiveTypes = void 0;
    // Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.0.0.0
    var _PrimitiveTypes;
    (function (_PrimitiveTypes) {
        _PrimitiveTypes.__DateTime_Uri = "dm:///_internal/types/internal#PrimitiveTypes.DateTime";
    })(_PrimitiveTypes = exports._PrimitiveTypes || (exports._PrimitiveTypes = {}));
    var _DatenMeister;
    (function (_DatenMeister) {
        let _CommonTypes;
        (function (_CommonTypes) {
            let _Default;
            (function (_Default) {
                let _Package;
                (function (_Package) {
                    _Package.name = "name";
                    _Package.packagedElement = "packagedElement";
                    _Package.preferredType = "preferredType";
                    _Package.preferredPackage = "preferredPackage";
                    _Package.defaultViewMode = "defaultViewMode";
                })(_Package = _Default._Package || (_Default._Package = {}));
                _Default.__Package_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package";
            })(_Default = _CommonTypes._Default || (_CommonTypes._Default = {}));
            let _ExtentManager;
            (function (_ExtentManager) {
                let _ImportSettings;
                (function (_ImportSettings) {
                    _ImportSettings.filePath = "filePath";
                    _ImportSettings.extentUri = "extentUri";
                    _ImportSettings.workspace = "workspace";
                })(_ImportSettings = _ExtentManager._ImportSettings || (_ExtentManager._ImportSettings = {}));
                _ExtentManager.__ImportSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings";
                let _ImportException;
                (function (_ImportException) {
                    _ImportException.message = "message";
                })(_ImportException = _ExtentManager._ImportException || (_ExtentManager._ImportException = {}));
                _ExtentManager.__ImportException_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException";
            })(_ExtentManager = _CommonTypes._ExtentManager || (_CommonTypes._ExtentManager = {}));
            let _OSIntegration;
            (function (_OSIntegration) {
                let _CommandLineApplication;
                (function (_CommandLineApplication) {
                    _CommandLineApplication.name = "name";
                    _CommandLineApplication.applicationPath = "applicationPath";
                })(_CommandLineApplication = _OSIntegration._CommandLineApplication || (_OSIntegration._CommandLineApplication = {}));
                _OSIntegration.__CommandLineApplication_Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication";
                let _EnvironmentalVariable;
                (function (_EnvironmentalVariable) {
                    _EnvironmentalVariable.name = "name";
                    _EnvironmentalVariable.value = "value";
                })(_EnvironmentalVariable = _OSIntegration._EnvironmentalVariable || (_OSIntegration._EnvironmentalVariable = {}));
                _OSIntegration.__EnvironmentalVariable_Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable";
            })(_OSIntegration = _CommonTypes._OSIntegration || (_CommonTypes._OSIntegration = {}));
        })(_CommonTypes = _DatenMeister._CommonTypes || (_DatenMeister._CommonTypes = {}));
        let _Actions;
        (function (_Actions) {
            let _ActionSet;
            (function (_ActionSet) {
                _ActionSet.action = "action";
                _ActionSet.name = "name";
                _ActionSet.isDisabled = "isDisabled";
            })(_ActionSet = _Actions._ActionSet || (_Actions._ActionSet = {}));
            _Actions.__ActionSet_Uri = "dm:///_internal/types/internal#Actions.ActionSet";
            let _LoggingWriterAction;
            (function (_LoggingWriterAction) {
                _LoggingWriterAction.message = "message";
                _LoggingWriterAction.name = "name";
                _LoggingWriterAction.isDisabled = "isDisabled";
            })(_LoggingWriterAction = _Actions._LoggingWriterAction || (_Actions._LoggingWriterAction = {}));
            _Actions.__LoggingWriterAction_Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction";
            let _CommandExecutionAction;
            (function (_CommandExecutionAction) {
                _CommandExecutionAction.command = "command";
                _CommandExecutionAction._arguments_ = "arguments";
                _CommandExecutionAction.workingDirectory = "workingDirectory";
                _CommandExecutionAction.name = "name";
                _CommandExecutionAction.isDisabled = "isDisabled";
            })(_CommandExecutionAction = _Actions._CommandExecutionAction || (_Actions._CommandExecutionAction = {}));
            _Actions.__CommandExecutionAction_Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82";
            let _PowershellExecutionAction;
            (function (_PowershellExecutionAction) {
                _PowershellExecutionAction.script = "script";
                _PowershellExecutionAction.workingDirectory = "workingDirectory";
                _PowershellExecutionAction.name = "name";
                _PowershellExecutionAction.isDisabled = "isDisabled";
            })(_PowershellExecutionAction = _Actions._PowershellExecutionAction || (_Actions._PowershellExecutionAction = {}));
            _Actions.__PowershellExecutionAction_Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb";
            let _LoadExtentAction;
            (function (_LoadExtentAction) {
                _LoadExtentAction.configuration = "configuration";
                _LoadExtentAction.dropExisting = "dropExisting";
                _LoadExtentAction.name = "name";
                _LoadExtentAction.isDisabled = "isDisabled";
            })(_LoadExtentAction = _Actions._LoadExtentAction || (_Actions._LoadExtentAction = {}));
            _Actions.__LoadExtentAction_Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee";
            let _DropExtentAction;
            (function (_DropExtentAction) {
                _DropExtentAction.workspace = "workspace";
                _DropExtentAction.extentUri = "extentUri";
                _DropExtentAction.name = "name";
                _DropExtentAction.isDisabled = "isDisabled";
            })(_DropExtentAction = _Actions._DropExtentAction || (_Actions._DropExtentAction = {}));
            _Actions.__DropExtentAction_Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09";
            let _CreateWorkspaceAction;
            (function (_CreateWorkspaceAction) {
                _CreateWorkspaceAction.workspace = "workspace";
                _CreateWorkspaceAction.annotation = "annotation";
                _CreateWorkspaceAction.name = "name";
                _CreateWorkspaceAction.isDisabled = "isDisabled";
            })(_CreateWorkspaceAction = _Actions._CreateWorkspaceAction || (_Actions._CreateWorkspaceAction = {}));
            _Actions.__CreateWorkspaceAction_Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe";
            let _DropWorkspaceAction;
            (function (_DropWorkspaceAction) {
                _DropWorkspaceAction.workspace = "workspace";
                _DropWorkspaceAction.name = "name";
                _DropWorkspaceAction.isDisabled = "isDisabled";
            })(_DropWorkspaceAction = _Actions._DropWorkspaceAction || (_Actions._DropWorkspaceAction = {}));
            _Actions.__DropWorkspaceAction_Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8";
            let _CopyElementsAction;
            (function (_CopyElementsAction) {
                _CopyElementsAction.sourcePath = "sourcePath";
                _CopyElementsAction.targetPath = "targetPath";
                _CopyElementsAction.moveOnly = "moveOnly";
                _CopyElementsAction.sourceWorkspace = "sourceWorkspace";
                _CopyElementsAction.targetWorkspace = "targetWorkspace";
                _CopyElementsAction.emptyTarget = "emptyTarget";
                _CopyElementsAction.name = "name";
                _CopyElementsAction.isDisabled = "isDisabled";
            })(_CopyElementsAction = _Actions._CopyElementsAction || (_Actions._CopyElementsAction = {}));
            _Actions.__CopyElementsAction_Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed";
            let _ExportToXmiAction;
            (function (_ExportToXmiAction) {
                _ExportToXmiAction.sourcePath = "sourcePath";
                _ExportToXmiAction.filePath = "filePath";
                _ExportToXmiAction.sourceWorkspaceId = "sourceWorkspaceId";
                _ExportToXmiAction.name = "name";
                _ExportToXmiAction.isDisabled = "isDisabled";
            })(_ExportToXmiAction = _Actions._ExportToXmiAction || (_Actions._ExportToXmiAction = {}));
            _Actions.__ExportToXmiAction_Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863";
            let _ClearCollectionAction;
            (function (_ClearCollectionAction) {
                _ClearCollectionAction.workspace = "workspace";
                _ClearCollectionAction.path = "path";
                _ClearCollectionAction.name = "name";
                _ClearCollectionAction.isDisabled = "isDisabled";
            })(_ClearCollectionAction = _Actions._ClearCollectionAction || (_Actions._ClearCollectionAction = {}));
            _Actions.__ClearCollectionAction_Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae";
            let _TransformItemsAction;
            (function (_TransformItemsAction) {
                _TransformItemsAction.metaClass = "metaClass";
                _TransformItemsAction.runtimeClass = "runtimeClass";
                _TransformItemsAction.workspace = "workspace";
                _TransformItemsAction.path = "path";
                _TransformItemsAction.excludeDescendents = "excludeDescendents";
                _TransformItemsAction.name = "name";
                _TransformItemsAction.isDisabled = "isDisabled";
            })(_TransformItemsAction = _Actions._TransformItemsAction || (_Actions._TransformItemsAction = {}));
            _Actions.__TransformItemsAction_Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler";
            let _EchoAction;
            (function (_EchoAction) {
                _EchoAction.shallSuccess = "shallSuccess";
                _EchoAction.name = "name";
                _EchoAction.isDisabled = "isDisabled";
            })(_EchoAction = _Actions._EchoAction || (_Actions._EchoAction = {}));
            _Actions.__EchoAction_Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction";
            let _DocumentOpenAction;
            (function (_DocumentOpenAction) {
                _DocumentOpenAction.filePath = "filePath";
                _DocumentOpenAction.name = "name";
                _DocumentOpenAction.isDisabled = "isDisabled";
            })(_DocumentOpenAction = _Actions._DocumentOpenAction || (_Actions._DocumentOpenAction = {}));
            _Actions.__DocumentOpenAction_Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74";
            let _Reports;
            (function (_Reports) {
                let _SimpleReportAction;
                (function (_SimpleReportAction) {
                    _SimpleReportAction.path = "path";
                    _SimpleReportAction.configuration = "configuration";
                    _SimpleReportAction.workspaceId = "workspaceId";
                    _SimpleReportAction.filePath = "filePath";
                    _SimpleReportAction.name = "name";
                    _SimpleReportAction.isDisabled = "isDisabled";
                })(_SimpleReportAction = _Reports._SimpleReportAction || (_Reports._SimpleReportAction = {}));
                _Reports.__SimpleReportAction_Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction";
                let _AdocReportAction;
                (function (_AdocReportAction) {
                    _AdocReportAction.filePath = "filePath";
                    _AdocReportAction.reportInstance = "reportInstance";
                    _AdocReportAction.name = "name";
                    _AdocReportAction.isDisabled = "isDisabled";
                })(_AdocReportAction = _Reports._AdocReportAction || (_Reports._AdocReportAction = {}));
                _Reports.__AdocReportAction_Uri = "dm:///_internal/types/internal#Actions.AdocReportAction";
                let _HtmlReportAction;
                (function (_HtmlReportAction) {
                    _HtmlReportAction.filePath = "filePath";
                    _HtmlReportAction.reportInstance = "reportInstance";
                    _HtmlReportAction.name = "name";
                    _HtmlReportAction.isDisabled = "isDisabled";
                })(_HtmlReportAction = _Reports._HtmlReportAction || (_Reports._HtmlReportAction = {}));
                _Reports.__HtmlReportAction_Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction";
            })(_Reports = _Actions._Reports || (_Actions._Reports = {}));
            let _Action;
            (function (_Action) {
                _Action.name = "name";
                _Action.isDisabled = "isDisabled";
            })(_Action = _Actions._Action || (_Actions._Action = {}));
            _Actions.__Action_Uri = "dm:///_internal/types/internal#Actions.Action";
        })(_Actions = _DatenMeister._Actions || (_DatenMeister._Actions = {}));
        let _DataViews;
        (function (_DataViews) {
            let _DataView;
            (function (_DataView) {
                _DataView.name = "name";
                _DataView.workspace = "workspace";
                _DataView.uri = "uri";
                _DataView.viewNode = "viewNode";
            })(_DataView = _DataViews._DataView || (_DataViews._DataView = {}));
            _DataViews.__DataView_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView";
            let _ViewNode;
            (function (_ViewNode) {
                _ViewNode.name = "name";
            })(_ViewNode = _DataViews._ViewNode || (_DataViews._ViewNode = {}));
            _DataViews.__ViewNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode";
            let _SourceExtentNode;
            (function (_SourceExtentNode) {
                _SourceExtentNode.extentUri = "extentUri";
                _SourceExtentNode.workspace = "workspace";
                _SourceExtentNode.name = "name";
            })(_SourceExtentNode = _DataViews._SourceExtentNode || (_DataViews._SourceExtentNode = {}));
            _DataViews.__SourceExtentNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SourceExtentNode";
            let _FlattenNode;
            (function (_FlattenNode) {
                _FlattenNode.input = "input";
                _FlattenNode.name = "name";
            })(_FlattenNode = _DataViews._FlattenNode || (_DataViews._FlattenNode = {}));
            _DataViews.__FlattenNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode";
            let _FilterPropertyNode;
            (function (_FilterPropertyNode) {
                _FilterPropertyNode.input = "input";
                _FilterPropertyNode.property = "property";
                _FilterPropertyNode.value = "value";
                _FilterPropertyNode.comparisonMode = "comparisonMode";
                _FilterPropertyNode.name = "name";
            })(_FilterPropertyNode = _DataViews._FilterPropertyNode || (_DataViews._FilterPropertyNode = {}));
            _DataViews.__FilterPropertyNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterPropertyNode";
            let _FilterTypeNode;
            (function (_FilterTypeNode) {
                _FilterTypeNode.input = "input";
                _FilterTypeNode.type = "type";
                _FilterTypeNode.includeInherits = "includeInherits";
                _FilterTypeNode.name = "name";
            })(_FilterTypeNode = _DataViews._FilterTypeNode || (_DataViews._FilterTypeNode = {}));
            _DataViews.__FilterTypeNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterTypeNode";
            let _ComparisonMode;
            (function (_ComparisonMode) {
                _ComparisonMode.Equal = "Equal";
                _ComparisonMode.NotEqual = "NotEqual";
                _ComparisonMode.Contains = "Contains";
                _ComparisonMode.DoesNotContain = "DoesNotContain";
                _ComparisonMode.GreaterThan = "GreaterThan";
                _ComparisonMode.GreaterOrEqualThan = "GreaterOrEqualThan";
                _ComparisonMode.LighterThan = "LighterThan";
                _ComparisonMode.LighterOrEqualThan = "LighterOrEqualThan";
            })(_ComparisonMode = _DataViews._ComparisonMode || (_DataViews._ComparisonMode = {}));
            let ___ComparisonMode;
            (function (___ComparisonMode) {
                ___ComparisonMode[___ComparisonMode["Equal"] = 0] = "Equal";
                ___ComparisonMode[___ComparisonMode["NotEqual"] = 1] = "NotEqual";
                ___ComparisonMode[___ComparisonMode["Contains"] = 2] = "Contains";
                ___ComparisonMode[___ComparisonMode["DoesNotContain"] = 3] = "DoesNotContain";
                ___ComparisonMode[___ComparisonMode["GreaterThan"] = 4] = "GreaterThan";
                ___ComparisonMode[___ComparisonMode["GreaterOrEqualThan"] = 5] = "GreaterOrEqualThan";
                ___ComparisonMode[___ComparisonMode["LighterThan"] = 6] = "LighterThan";
                ___ComparisonMode[___ComparisonMode["LighterOrEqualThan"] = 7] = "LighterOrEqualThan";
            })(___ComparisonMode = _DataViews.___ComparisonMode || (_DataViews.___ComparisonMode = {}));
            let _SelectByFullNameNode;
            (function (_SelectByFullNameNode) {
                _SelectByFullNameNode.input = "input";
                _SelectByFullNameNode.path = "path";
                _SelectByFullNameNode.name = "name";
            })(_SelectByFullNameNode = _DataViews._SelectByFullNameNode || (_DataViews._SelectByFullNameNode = {}));
            _DataViews.__SelectByFullNameNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode";
            let _DynamicSourceNode;
            (function (_DynamicSourceNode) {
                _DynamicSourceNode.nodeName = "nodeName";
                _DynamicSourceNode.name = "name";
            })(_DynamicSourceNode = _DataViews._DynamicSourceNode || (_DataViews._DynamicSourceNode = {}));
            _DataViews.__DynamicSourceNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode";
        })(_DataViews = _DatenMeister._DataViews || (_DatenMeister._DataViews = {}));
        let _Reports;
        (function (_Reports) {
            let _ReportDefinition;
            (function (_ReportDefinition) {
                _ReportDefinition.name = "name";
                _ReportDefinition.title = "title";
                _ReportDefinition.elements = "elements";
            })(_ReportDefinition = _Reports._ReportDefinition || (_Reports._ReportDefinition = {}));
            _Reports.__ReportDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition";
            let _ReportInstanceSource;
            (function (_ReportInstanceSource) {
                _ReportInstanceSource.name = "name";
                _ReportInstanceSource.workspaceId = "workspaceId";
                _ReportInstanceSource.path = "path";
            })(_ReportInstanceSource = _Reports._ReportInstanceSource || (_Reports._ReportInstanceSource = {}));
            _Reports.__ReportInstanceSource_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource";
            let _ReportInstance;
            (function (_ReportInstance) {
                _ReportInstance.name = "name";
                _ReportInstance.reportDefinition = "reportDefinition";
                _ReportInstance.sources = "sources";
            })(_ReportInstance = _Reports._ReportInstance || (_Reports._ReportInstance = {}));
            _Reports.__ReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance";
            let _AdocReportInstance;
            (function (_AdocReportInstance) {
                _AdocReportInstance.name = "name";
                _AdocReportInstance.reportDefinition = "reportDefinition";
                _AdocReportInstance.sources = "sources";
            })(_AdocReportInstance = _Reports._AdocReportInstance || (_Reports._AdocReportInstance = {}));
            _Reports.__AdocReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance";
            let _HtmlReportInstance;
            (function (_HtmlReportInstance) {
                _HtmlReportInstance.name = "name";
                _HtmlReportInstance.reportDefinition = "reportDefinition";
                _HtmlReportInstance.sources = "sources";
            })(_HtmlReportInstance = _Reports._HtmlReportInstance || (_Reports._HtmlReportInstance = {}));
            _Reports.__HtmlReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance";
            let _DescendentMode;
            (function (_DescendentMode) {
                _DescendentMode.None = "None";
                _DescendentMode.Inline = "Inline";
                _DescendentMode.PerPackage = "PerPackage";
            })(_DescendentMode = _Reports._DescendentMode || (_Reports._DescendentMode = {}));
            let ___DescendentMode;
            (function (___DescendentMode) {
                ___DescendentMode[___DescendentMode["None"] = 0] = "None";
                ___DescendentMode[___DescendentMode["Inline"] = 1] = "Inline";
                ___DescendentMode[___DescendentMode["PerPackage"] = 2] = "PerPackage";
            })(___DescendentMode = _Reports.___DescendentMode || (_Reports.___DescendentMode = {}));
            let _SimpleReportConfiguration;
            (function (_SimpleReportConfiguration) {
                _SimpleReportConfiguration.name = "name";
                _SimpleReportConfiguration.showDescendents = "showDescendents";
                _SimpleReportConfiguration.rootElement = "rootElement";
                _SimpleReportConfiguration.showRootElement = "showRootElement";
                _SimpleReportConfiguration.showMetaClasses = "showMetaClasses";
                _SimpleReportConfiguration.showFullName = "showFullName";
                _SimpleReportConfiguration.form = "form";
                _SimpleReportConfiguration.descendentMode = "descendentMode";
                _SimpleReportConfiguration.typeMode = "typeMode";
                _SimpleReportConfiguration.workspaceId = "workspaceId";
            })(_SimpleReportConfiguration = _Reports._SimpleReportConfiguration || (_Reports._SimpleReportConfiguration = {}));
            _Reports.__SimpleReportConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration";
            let _Elements;
            (function (_Elements) {
                let _ReportElement;
                (function (_ReportElement) {
                    _ReportElement.name = "name";
                })(_ReportElement = _Elements._ReportElement || (_Elements._ReportElement = {}));
                _Elements.__ReportElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement";
                let _ReportHeadline;
                (function (_ReportHeadline) {
                    _ReportHeadline.title = "title";
                    _ReportHeadline.name = "name";
                })(_ReportHeadline = _Elements._ReportHeadline || (_Elements._ReportHeadline = {}));
                _Elements.__ReportHeadline_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline";
                let _ReportParagraph;
                (function (_ReportParagraph) {
                    _ReportParagraph.paragraph = "paragraph";
                    _ReportParagraph.cssClass = "cssClass";
                    _ReportParagraph.viewNode = "viewNode";
                    _ReportParagraph.evalProperties = "evalProperties";
                    _ReportParagraph.evalParagraph = "evalParagraph";
                    _ReportParagraph.name = "name";
                })(_ReportParagraph = _Elements._ReportParagraph || (_Elements._ReportParagraph = {}));
                _Elements.__ReportParagraph_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph";
                let _ReportTable;
                (function (_ReportTable) {
                    _ReportTable.cssClass = "cssClass";
                    _ReportTable.viewNode = "viewNode";
                    _ReportTable.form = "form";
                    _ReportTable.evalProperties = "evalProperties";
                    _ReportTable.name = "name";
                })(_ReportTable = _Elements._ReportTable || (_Elements._ReportTable = {}));
                _Elements.__ReportTable_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable";
                let _ReportTableForTypeMode;
                (function (_ReportTableForTypeMode) {
                    _ReportTableForTypeMode.PerType = "PerType";
                    _ReportTableForTypeMode.AllTypes = "AllTypes";
                })(_ReportTableForTypeMode = _Elements._ReportTableForTypeMode || (_Elements._ReportTableForTypeMode = {}));
                let ___ReportTableForTypeMode;
                (function (___ReportTableForTypeMode) {
                    ___ReportTableForTypeMode[___ReportTableForTypeMode["PerType"] = 0] = "PerType";
                    ___ReportTableForTypeMode[___ReportTableForTypeMode["AllTypes"] = 1] = "AllTypes";
                })(___ReportTableForTypeMode = _Elements.___ReportTableForTypeMode || (_Elements.___ReportTableForTypeMode = {}));
                let _ReportLoop;
                (function (_ReportLoop) {
                    _ReportLoop.viewNode = "viewNode";
                    _ReportLoop.elements = "elements";
                    _ReportLoop.name = "name";
                })(_ReportLoop = _Elements._ReportLoop || (_Elements._ReportLoop = {}));
                _Elements.__ReportLoop_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop";
            })(_Elements = _Reports._Elements || (_Reports._Elements = {}));
        })(_Reports = _DatenMeister._Reports || (_DatenMeister._Reports = {}));
        let _ExtentLoaderConfigs;
        (function (_ExtentLoaderConfigs) {
            let _ExtentLoaderConfig;
            (function (_ExtentLoaderConfig) {
                _ExtentLoaderConfig.name = "name";
                _ExtentLoaderConfig.extentUri = "extentUri";
                _ExtentLoaderConfig.workspaceId = "workspaceId";
                _ExtentLoaderConfig.dropExisting = "dropExisting";
            })(_ExtentLoaderConfig = _ExtentLoaderConfigs._ExtentLoaderConfig || (_ExtentLoaderConfigs._ExtentLoaderConfig = {}));
            _ExtentLoaderConfigs.__ExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig";
            let _ExcelLoaderConfig;
            (function (_ExcelLoaderConfig) {
                _ExcelLoaderConfig.fixRowCount = "fixRowCount";
                _ExcelLoaderConfig.fixColumnCount = "fixColumnCount";
                _ExcelLoaderConfig.filePath = "filePath";
                _ExcelLoaderConfig.sheetName = "sheetName";
                _ExcelLoaderConfig.offsetRow = "offsetRow";
                _ExcelLoaderConfig.offsetColumn = "offsetColumn";
                _ExcelLoaderConfig.countRows = "countRows";
                _ExcelLoaderConfig.countColumns = "countColumns";
                _ExcelLoaderConfig.hasHeader = "hasHeader";
                _ExcelLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
                _ExcelLoaderConfig.onlySetColumns = "onlySetColumns";
                _ExcelLoaderConfig.idColumnName = "idColumnName";
                _ExcelLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
                _ExcelLoaderConfig.columns = "columns";
                _ExcelLoaderConfig.name = "name";
                _ExcelLoaderConfig.extentUri = "extentUri";
                _ExcelLoaderConfig.workspaceId = "workspaceId";
                _ExcelLoaderConfig.dropExisting = "dropExisting";
            })(_ExcelLoaderConfig = _ExtentLoaderConfigs._ExcelLoaderConfig || (_ExtentLoaderConfigs._ExcelLoaderConfig = {}));
            _ExtentLoaderConfigs.__ExcelLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig";
            let _ExcelReferenceLoaderConfig;
            (function (_ExcelReferenceLoaderConfig) {
                _ExcelReferenceLoaderConfig.fixRowCount = "fixRowCount";
                _ExcelReferenceLoaderConfig.fixColumnCount = "fixColumnCount";
                _ExcelReferenceLoaderConfig.filePath = "filePath";
                _ExcelReferenceLoaderConfig.sheetName = "sheetName";
                _ExcelReferenceLoaderConfig.offsetRow = "offsetRow";
                _ExcelReferenceLoaderConfig.offsetColumn = "offsetColumn";
                _ExcelReferenceLoaderConfig.countRows = "countRows";
                _ExcelReferenceLoaderConfig.countColumns = "countColumns";
                _ExcelReferenceLoaderConfig.hasHeader = "hasHeader";
                _ExcelReferenceLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
                _ExcelReferenceLoaderConfig.onlySetColumns = "onlySetColumns";
                _ExcelReferenceLoaderConfig.idColumnName = "idColumnName";
                _ExcelReferenceLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
                _ExcelReferenceLoaderConfig.columns = "columns";
                _ExcelReferenceLoaderConfig.name = "name";
                _ExcelReferenceLoaderConfig.extentUri = "extentUri";
                _ExcelReferenceLoaderConfig.workspaceId = "workspaceId";
                _ExcelReferenceLoaderConfig.dropExisting = "dropExisting";
            })(_ExcelReferenceLoaderConfig = _ExtentLoaderConfigs._ExcelReferenceLoaderConfig || (_ExtentLoaderConfigs._ExcelReferenceLoaderConfig = {}));
            _ExtentLoaderConfigs.__ExcelReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig";
            let _ExcelImportLoaderConfig;
            (function (_ExcelImportLoaderConfig) {
                _ExcelImportLoaderConfig.extentPath = "extentPath";
                _ExcelImportLoaderConfig.fixRowCount = "fixRowCount";
                _ExcelImportLoaderConfig.fixColumnCount = "fixColumnCount";
                _ExcelImportLoaderConfig.filePath = "filePath";
                _ExcelImportLoaderConfig.sheetName = "sheetName";
                _ExcelImportLoaderConfig.offsetRow = "offsetRow";
                _ExcelImportLoaderConfig.offsetColumn = "offsetColumn";
                _ExcelImportLoaderConfig.countRows = "countRows";
                _ExcelImportLoaderConfig.countColumns = "countColumns";
                _ExcelImportLoaderConfig.hasHeader = "hasHeader";
                _ExcelImportLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
                _ExcelImportLoaderConfig.onlySetColumns = "onlySetColumns";
                _ExcelImportLoaderConfig.idColumnName = "idColumnName";
                _ExcelImportLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
                _ExcelImportLoaderConfig.columns = "columns";
                _ExcelImportLoaderConfig.name = "name";
                _ExcelImportLoaderConfig.extentUri = "extentUri";
                _ExcelImportLoaderConfig.workspaceId = "workspaceId";
                _ExcelImportLoaderConfig.dropExisting = "dropExisting";
            })(_ExcelImportLoaderConfig = _ExtentLoaderConfigs._ExcelImportLoaderConfig || (_ExtentLoaderConfigs._ExcelImportLoaderConfig = {}));
            _ExtentLoaderConfigs.__ExcelImportLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig";
            let _ExcelExtentLoaderConfig;
            (function (_ExcelExtentLoaderConfig) {
                _ExcelExtentLoaderConfig.filePath = "filePath";
                _ExcelExtentLoaderConfig.idColumnName = "idColumnName";
                _ExcelExtentLoaderConfig.name = "name";
                _ExcelExtentLoaderConfig.extentUri = "extentUri";
                _ExcelExtentLoaderConfig.workspaceId = "workspaceId";
                _ExcelExtentLoaderConfig.dropExisting = "dropExisting";
            })(_ExcelExtentLoaderConfig = _ExtentLoaderConfigs._ExcelExtentLoaderConfig || (_ExtentLoaderConfigs._ExcelExtentLoaderConfig = {}));
            _ExtentLoaderConfigs.__ExcelExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig";
            let _InMemoryLoaderConfig;
            (function (_InMemoryLoaderConfig) {
                _InMemoryLoaderConfig.name = "name";
                _InMemoryLoaderConfig.extentUri = "extentUri";
                _InMemoryLoaderConfig.workspaceId = "workspaceId";
                _InMemoryLoaderConfig.dropExisting = "dropExisting";
            })(_InMemoryLoaderConfig = _ExtentLoaderConfigs._InMemoryLoaderConfig || (_ExtentLoaderConfigs._InMemoryLoaderConfig = {}));
            _ExtentLoaderConfigs.__InMemoryLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig";
            let _XmlReferenceLoaderConfig;
            (function (_XmlReferenceLoaderConfig) {
                _XmlReferenceLoaderConfig.filePath = "filePath";
                _XmlReferenceLoaderConfig.keepNamespaces = "keepNamespaces";
                _XmlReferenceLoaderConfig.name = "name";
                _XmlReferenceLoaderConfig.extentUri = "extentUri";
                _XmlReferenceLoaderConfig.workspaceId = "workspaceId";
                _XmlReferenceLoaderConfig.dropExisting = "dropExisting";
            })(_XmlReferenceLoaderConfig = _ExtentLoaderConfigs._XmlReferenceLoaderConfig || (_ExtentLoaderConfigs._XmlReferenceLoaderConfig = {}));
            _ExtentLoaderConfigs.__XmlReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig";
            let _ExtentFileLoaderConfig;
            (function (_ExtentFileLoaderConfig) {
                _ExtentFileLoaderConfig.filePath = "filePath";
                _ExtentFileLoaderConfig.name = "name";
                _ExtentFileLoaderConfig.extentUri = "extentUri";
                _ExtentFileLoaderConfig.workspaceId = "workspaceId";
                _ExtentFileLoaderConfig.dropExisting = "dropExisting";
            })(_ExtentFileLoaderConfig = _ExtentLoaderConfigs._ExtentFileLoaderConfig || (_ExtentLoaderConfigs._ExtentFileLoaderConfig = {}));
            _ExtentLoaderConfigs.__ExtentFileLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig";
            let _XmiStorageLoaderConfig;
            (function (_XmiStorageLoaderConfig) {
                _XmiStorageLoaderConfig.filePath = "filePath";
                _XmiStorageLoaderConfig.name = "name";
                _XmiStorageLoaderConfig.extentUri = "extentUri";
                _XmiStorageLoaderConfig.workspaceId = "workspaceId";
                _XmiStorageLoaderConfig.dropExisting = "dropExisting";
            })(_XmiStorageLoaderConfig = _ExtentLoaderConfigs._XmiStorageLoaderConfig || (_ExtentLoaderConfigs._XmiStorageLoaderConfig = {}));
            _ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig";
            let _CsvExtentLoaderConfig;
            (function (_CsvExtentLoaderConfig) {
                _CsvExtentLoaderConfig.settings = "settings";
                _CsvExtentLoaderConfig.filePath = "filePath";
                _CsvExtentLoaderConfig.name = "name";
                _CsvExtentLoaderConfig.extentUri = "extentUri";
                _CsvExtentLoaderConfig.workspaceId = "workspaceId";
                _CsvExtentLoaderConfig.dropExisting = "dropExisting";
            })(_CsvExtentLoaderConfig = _ExtentLoaderConfigs._CsvExtentLoaderConfig || (_ExtentLoaderConfigs._CsvExtentLoaderConfig = {}));
            _ExtentLoaderConfigs.__CsvExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig";
            let _CsvSettings;
            (function (_CsvSettings) {
                _CsvSettings.encoding = "encoding";
                _CsvSettings.hasHeader = "hasHeader";
                _CsvSettings.separator = "separator";
                _CsvSettings.columns = "columns";
                _CsvSettings.metaclassUri = "metaclassUri";
            })(_CsvSettings = _ExtentLoaderConfigs._CsvSettings || (_ExtentLoaderConfigs._CsvSettings = {}));
            _ExtentLoaderConfigs.__CsvSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings";
            let _ExcelHierarchicalColumnDefinition;
            (function (_ExcelHierarchicalColumnDefinition) {
                _ExcelHierarchicalColumnDefinition.name = "name";
                _ExcelHierarchicalColumnDefinition.metaClass = "metaClass";
                _ExcelHierarchicalColumnDefinition.property = "property";
            })(_ExcelHierarchicalColumnDefinition = _ExtentLoaderConfigs._ExcelHierarchicalColumnDefinition || (_ExtentLoaderConfigs._ExcelHierarchicalColumnDefinition = {}));
            _ExtentLoaderConfigs.__ExcelHierarchicalColumnDefinition_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition";
            let _ExcelHierarchicalLoaderConfig;
            (function (_ExcelHierarchicalLoaderConfig) {
                _ExcelHierarchicalLoaderConfig.hierarchicalColumns = "hierarchicalColumns";
                _ExcelHierarchicalLoaderConfig.skipElementsForLastLevel = "skipElementsForLastLevel";
                _ExcelHierarchicalLoaderConfig.fixRowCount = "fixRowCount";
                _ExcelHierarchicalLoaderConfig.fixColumnCount = "fixColumnCount";
                _ExcelHierarchicalLoaderConfig.filePath = "filePath";
                _ExcelHierarchicalLoaderConfig.sheetName = "sheetName";
                _ExcelHierarchicalLoaderConfig.offsetRow = "offsetRow";
                _ExcelHierarchicalLoaderConfig.offsetColumn = "offsetColumn";
                _ExcelHierarchicalLoaderConfig.countRows = "countRows";
                _ExcelHierarchicalLoaderConfig.countColumns = "countColumns";
                _ExcelHierarchicalLoaderConfig.hasHeader = "hasHeader";
                _ExcelHierarchicalLoaderConfig.tryMergedHeaderCells = "tryMergedHeaderCells";
                _ExcelHierarchicalLoaderConfig.onlySetColumns = "onlySetColumns";
                _ExcelHierarchicalLoaderConfig.idColumnName = "idColumnName";
                _ExcelHierarchicalLoaderConfig.skipEmptyRowsCount = "skipEmptyRowsCount";
                _ExcelHierarchicalLoaderConfig.columns = "columns";
                _ExcelHierarchicalLoaderConfig.name = "name";
                _ExcelHierarchicalLoaderConfig.extentUri = "extentUri";
                _ExcelHierarchicalLoaderConfig.workspaceId = "workspaceId";
                _ExcelHierarchicalLoaderConfig.dropExisting = "dropExisting";
            })(_ExcelHierarchicalLoaderConfig = _ExtentLoaderConfigs._ExcelHierarchicalLoaderConfig || (_ExtentLoaderConfigs._ExcelHierarchicalLoaderConfig = {}));
            _ExtentLoaderConfigs.__ExcelHierarchicalLoaderConfig_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig";
            let _ExcelColumn;
            (function (_ExcelColumn) {
                _ExcelColumn.header = "header";
                _ExcelColumn.name = "name";
            })(_ExcelColumn = _ExtentLoaderConfigs._ExcelColumn || (_ExtentLoaderConfigs._ExcelColumn = {}));
            _ExtentLoaderConfigs.__ExcelColumn_Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a";
            let _EnvironmentalVariableLoaderConfig;
            (function (_EnvironmentalVariableLoaderConfig) {
                _EnvironmentalVariableLoaderConfig.name = "name";
                _EnvironmentalVariableLoaderConfig.extentUri = "extentUri";
                _EnvironmentalVariableLoaderConfig.workspaceId = "workspaceId";
                _EnvironmentalVariableLoaderConfig.dropExisting = "dropExisting";
            })(_EnvironmentalVariableLoaderConfig = _ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig || (_ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig = {}));
            _ExtentLoaderConfigs.__EnvironmentalVariableLoaderConfig_Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3";
        })(_ExtentLoaderConfigs = _DatenMeister._ExtentLoaderConfigs || (_DatenMeister._ExtentLoaderConfigs = {}));
        let _Forms;
        (function (_Forms) {
            let _FieldData;
            (function (_FieldData) {
                _FieldData.isAttached = "isAttached";
                _FieldData.name = "name";
                _FieldData.title = "title";
                _FieldData.isEnumeration = "isEnumeration";
                _FieldData.defaultValue = "defaultValue";
                _FieldData.isReadOnly = "isReadOnly";
            })(_FieldData = _Forms._FieldData || (_Forms._FieldData = {}));
            _Forms.__FieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData";
            let _SortingOrder;
            (function (_SortingOrder) {
                _SortingOrder.name = "name";
                _SortingOrder.isDescending = "isDescending";
            })(_SortingOrder = _Forms._SortingOrder || (_Forms._SortingOrder = {}));
            _Forms.__SortingOrder_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder";
            let _AnyDataFieldData;
            (function (_AnyDataFieldData) {
                _AnyDataFieldData.isAttached = "isAttached";
                _AnyDataFieldData.name = "name";
                _AnyDataFieldData.title = "title";
                _AnyDataFieldData.isEnumeration = "isEnumeration";
                _AnyDataFieldData.defaultValue = "defaultValue";
                _AnyDataFieldData.isReadOnly = "isReadOnly";
            })(_AnyDataFieldData = _Forms._AnyDataFieldData || (_Forms._AnyDataFieldData = {}));
            _Forms.__AnyDataFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData";
            let _CheckboxFieldData;
            (function (_CheckboxFieldData) {
                _CheckboxFieldData.lineHeight = "lineHeight";
                _CheckboxFieldData.isAttached = "isAttached";
                _CheckboxFieldData.name = "name";
                _CheckboxFieldData.title = "title";
                _CheckboxFieldData.isEnumeration = "isEnumeration";
                _CheckboxFieldData.defaultValue = "defaultValue";
                _CheckboxFieldData.isReadOnly = "isReadOnly";
            })(_CheckboxFieldData = _Forms._CheckboxFieldData || (_Forms._CheckboxFieldData = {}));
            _Forms.__CheckboxFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData";
            let _ActionFieldData;
            (function (_ActionFieldData) {
                _ActionFieldData.actionName = "actionName";
                _ActionFieldData.parameter = "parameter";
                _ActionFieldData.isAttached = "isAttached";
                _ActionFieldData.name = "name";
                _ActionFieldData.title = "title";
                _ActionFieldData.isEnumeration = "isEnumeration";
                _ActionFieldData.defaultValue = "defaultValue";
                _ActionFieldData.isReadOnly = "isReadOnly";
            })(_ActionFieldData = _Forms._ActionFieldData || (_Forms._ActionFieldData = {}));
            _Forms.__ActionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData";
            let _DateTimeFieldData;
            (function (_DateTimeFieldData) {
                _DateTimeFieldData.hideDate = "hideDate";
                _DateTimeFieldData.hideTime = "hideTime";
                _DateTimeFieldData.showOffsetButtons = "showOffsetButtons";
                _DateTimeFieldData.isAttached = "isAttached";
                _DateTimeFieldData.name = "name";
                _DateTimeFieldData.title = "title";
                _DateTimeFieldData.isEnumeration = "isEnumeration";
                _DateTimeFieldData.defaultValue = "defaultValue";
                _DateTimeFieldData.isReadOnly = "isReadOnly";
            })(_DateTimeFieldData = _Forms._DateTimeFieldData || (_Forms._DateTimeFieldData = {}));
            _Forms.__DateTimeFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData";
            let _FormAssociation;
            (function (_FormAssociation) {
                _FormAssociation.name = "name";
                _FormAssociation.formType = "formType";
                _FormAssociation.metaClass = "metaClass";
                _FormAssociation.extentType = "extentType";
                _FormAssociation.viewModeId = "viewModeId";
                _FormAssociation.parentMetaClass = "parentMetaClass";
                _FormAssociation.parentProperty = "parentProperty";
                _FormAssociation.form = "form";
            })(_FormAssociation = _Forms._FormAssociation || (_Forms._FormAssociation = {}));
            _Forms.__FormAssociation_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation";
            let _DropDownFieldData;
            (function (_DropDownFieldData) {
                _DropDownFieldData.values = "values";
                _DropDownFieldData.valuesByEnumeration = "valuesByEnumeration";
                _DropDownFieldData.isAttached = "isAttached";
                _DropDownFieldData.name = "name";
                _DropDownFieldData.title = "title";
                _DropDownFieldData.isEnumeration = "isEnumeration";
                _DropDownFieldData.defaultValue = "defaultValue";
                _DropDownFieldData.isReadOnly = "isReadOnly";
            })(_DropDownFieldData = _Forms._DropDownFieldData || (_Forms._DropDownFieldData = {}));
            _Forms.__DropDownFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData";
            let _ValuePair;
            (function (_ValuePair) {
                _ValuePair.value = "value";
                _ValuePair.name = "name";
            })(_ValuePair = _Forms._ValuePair || (_Forms._ValuePair = {}));
            _Forms.__ValuePair_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair";
            let _MetaClassElementFieldData;
            (function (_MetaClassElementFieldData) {
                _MetaClassElementFieldData.isAttached = "isAttached";
                _MetaClassElementFieldData.name = "name";
                _MetaClassElementFieldData.title = "title";
                _MetaClassElementFieldData.isEnumeration = "isEnumeration";
                _MetaClassElementFieldData.defaultValue = "defaultValue";
                _MetaClassElementFieldData.isReadOnly = "isReadOnly";
            })(_MetaClassElementFieldData = _Forms._MetaClassElementFieldData || (_Forms._MetaClassElementFieldData = {}));
            _Forms.__MetaClassElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData";
            let _ReferenceFieldData;
            (function (_ReferenceFieldData) {
                _ReferenceFieldData.isSelectionInline = "isSelectionInline";
                _ReferenceFieldData.defaultWorkspace = "defaultWorkspace";
                _ReferenceFieldData.defaultItemUri = "defaultItemUri";
                _ReferenceFieldData.showAllChildren = "showAllChildren";
                _ReferenceFieldData.showWorkspaceSelection = "showWorkspaceSelection";
                _ReferenceFieldData.showExtentSelection = "showExtentSelection";
                _ReferenceFieldData.metaClassFilter = "metaClassFilter";
                _ReferenceFieldData.isAttached = "isAttached";
                _ReferenceFieldData.name = "name";
                _ReferenceFieldData.title = "title";
                _ReferenceFieldData.isEnumeration = "isEnumeration";
                _ReferenceFieldData.defaultValue = "defaultValue";
                _ReferenceFieldData.isReadOnly = "isReadOnly";
            })(_ReferenceFieldData = _Forms._ReferenceFieldData || (_Forms._ReferenceFieldData = {}));
            _Forms.__ReferenceFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData";
            let _SubElementFieldData;
            (function (_SubElementFieldData) {
                _SubElementFieldData.metaClass = "metaClass";
                _SubElementFieldData.form = "form";
                _SubElementFieldData.allowOnlyExistingElements = "allowOnlyExistingElements";
                _SubElementFieldData.defaultTypesForNewElements = "defaultTypesForNewElements";
                _SubElementFieldData.includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";
                _SubElementFieldData.defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";
                _SubElementFieldData.defaultExtentOfNewElements = "defaultExtentOfNewElements";
                _SubElementFieldData.isAttached = "isAttached";
                _SubElementFieldData.name = "name";
                _SubElementFieldData.title = "title";
                _SubElementFieldData.isEnumeration = "isEnumeration";
                _SubElementFieldData.defaultValue = "defaultValue";
                _SubElementFieldData.isReadOnly = "isReadOnly";
            })(_SubElementFieldData = _Forms._SubElementFieldData || (_Forms._SubElementFieldData = {}));
            _Forms.__SubElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData";
            let _TextFieldData;
            (function (_TextFieldData) {
                _TextFieldData.lineHeight = "lineHeight";
                _TextFieldData.width = "width";
                _TextFieldData.isAttached = "isAttached";
                _TextFieldData.name = "name";
                _TextFieldData.title = "title";
                _TextFieldData.isEnumeration = "isEnumeration";
                _TextFieldData.defaultValue = "defaultValue";
                _TextFieldData.isReadOnly = "isReadOnly";
            })(_TextFieldData = _Forms._TextFieldData || (_Forms._TextFieldData = {}));
            _Forms.__TextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData";
            let _EvalTextFieldData;
            (function (_EvalTextFieldData) {
                _EvalTextFieldData.evalCellProperties = "evalCellProperties";
                _EvalTextFieldData.lineHeight = "lineHeight";
                _EvalTextFieldData.width = "width";
                _EvalTextFieldData.isAttached = "isAttached";
                _EvalTextFieldData.name = "name";
                _EvalTextFieldData.title = "title";
                _EvalTextFieldData.isEnumeration = "isEnumeration";
                _EvalTextFieldData.defaultValue = "defaultValue";
                _EvalTextFieldData.isReadOnly = "isReadOnly";
            })(_EvalTextFieldData = _Forms._EvalTextFieldData || (_Forms._EvalTextFieldData = {}));
            _Forms.__EvalTextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData";
            let _SeparatorLineFieldData;
            (function (_SeparatorLineFieldData) {
                _SeparatorLineFieldData.Height = "Height";
            })(_SeparatorLineFieldData = _Forms._SeparatorLineFieldData || (_Forms._SeparatorLineFieldData = {}));
            _Forms.__SeparatorLineFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData";
            let _FileSelectionFieldData;
            (function (_FileSelectionFieldData) {
                _FileSelectionFieldData.defaultExtension = "defaultExtension";
                _FileSelectionFieldData.isSaving = "isSaving";
                _FileSelectionFieldData.initialPathToDirectory = "initialPathToDirectory";
                _FileSelectionFieldData.filter = "filter";
                _FileSelectionFieldData.isAttached = "isAttached";
                _FileSelectionFieldData.name = "name";
                _FileSelectionFieldData.title = "title";
                _FileSelectionFieldData.isEnumeration = "isEnumeration";
                _FileSelectionFieldData.defaultValue = "defaultValue";
                _FileSelectionFieldData.isReadOnly = "isReadOnly";
            })(_FileSelectionFieldData = _Forms._FileSelectionFieldData || (_Forms._FileSelectionFieldData = {}));
            _Forms.__FileSelectionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData";
            let _DefaultTypeForNewElement;
            (function (_DefaultTypeForNewElement) {
                _DefaultTypeForNewElement.name = "name";
                _DefaultTypeForNewElement.metaClass = "metaClass";
                _DefaultTypeForNewElement.parentProperty = "parentProperty";
            })(_DefaultTypeForNewElement = _Forms._DefaultTypeForNewElement || (_Forms._DefaultTypeForNewElement = {}));
            _Forms.__DefaultTypeForNewElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement";
            let _FullNameFieldData;
            (function (_FullNameFieldData) {
                _FullNameFieldData.isAttached = "isAttached";
                _FullNameFieldData.name = "name";
                _FullNameFieldData.title = "title";
                _FullNameFieldData.isEnumeration = "isEnumeration";
                _FullNameFieldData.defaultValue = "defaultValue";
                _FullNameFieldData.isReadOnly = "isReadOnly";
            })(_FullNameFieldData = _Forms._FullNameFieldData || (_Forms._FullNameFieldData = {}));
            _Forms.__FullNameFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData";
            let _CheckboxListTaggingFieldData;
            (function (_CheckboxListTaggingFieldData) {
                _CheckboxListTaggingFieldData.values = "values";
                _CheckboxListTaggingFieldData.separator = "separator";
                _CheckboxListTaggingFieldData.containsFreeText = "containsFreeText";
                _CheckboxListTaggingFieldData.isAttached = "isAttached";
                _CheckboxListTaggingFieldData.name = "name";
                _CheckboxListTaggingFieldData.title = "title";
                _CheckboxListTaggingFieldData.isEnumeration = "isEnumeration";
                _CheckboxListTaggingFieldData.defaultValue = "defaultValue";
                _CheckboxListTaggingFieldData.isReadOnly = "isReadOnly";
            })(_CheckboxListTaggingFieldData = _Forms._CheckboxListTaggingFieldData || (_Forms._CheckboxListTaggingFieldData = {}));
            _Forms.__CheckboxListTaggingFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData";
            let _NumberFieldData;
            (function (_NumberFieldData) {
                _NumberFieldData.format = "format";
                _NumberFieldData.isInteger = "isInteger";
                _NumberFieldData.isAttached = "isAttached";
                _NumberFieldData.name = "name";
                _NumberFieldData.title = "title";
                _NumberFieldData.isEnumeration = "isEnumeration";
                _NumberFieldData.defaultValue = "defaultValue";
                _NumberFieldData.isReadOnly = "isReadOnly";
            })(_NumberFieldData = _Forms._NumberFieldData || (_Forms._NumberFieldData = {}));
            _Forms.__NumberFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData";
            let _FormType;
            (function (_FormType) {
                _FormType.Detail = "Detail";
                _FormType.TreeItemExtent = "TreeItemExtent";
                _FormType.TreeItemDetail = "TreeItemDetail";
                _FormType.ObjectList = "ObjectList";
                _FormType.TreeItemExtentExtension = "TreeItemExtentExtension";
                _FormType.TreeItemDetailExtension = "TreeItemDetailExtension";
            })(_FormType = _Forms._FormType || (_Forms._FormType = {}));
            let ___FormType;
            (function (___FormType) {
                ___FormType[___FormType["Detail"] = 0] = "Detail";
                ___FormType[___FormType["TreeItemExtent"] = 1] = "TreeItemExtent";
                ___FormType[___FormType["TreeItemDetail"] = 2] = "TreeItemDetail";
                ___FormType[___FormType["ObjectList"] = 3] = "ObjectList";
                ___FormType[___FormType["TreeItemExtentExtension"] = 4] = "TreeItemExtentExtension";
                ___FormType[___FormType["TreeItemDetailExtension"] = 5] = "TreeItemDetailExtension";
            })(___FormType = _Forms.___FormType || (_Forms.___FormType = {}));
            let _Form;
            (function (_Form) {
                _Form.name = "name";
                _Form.title = "title";
                _Form.isReadOnly = "isReadOnly";
                _Form.hideMetaInformation = "hideMetaInformation";
                _Form.originalUri = "originalUri";
                _Form.creationProtocol = "creationProtocol";
            })(_Form = _Forms._Form || (_Forms._Form = {}));
            _Forms.__Form_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form";
            let _DetailForm;
            (function (_DetailForm) {
                _DetailForm.buttonApplyText = "buttonApplyText";
                _DetailForm.allowNewProperties = "allowNewProperties";
                _DetailForm.defaultWidth = "defaultWidth";
                _DetailForm.defaultHeight = "defaultHeight";
                _DetailForm.tab = "tab";
                _DetailForm.field = "field";
                _DetailForm.name = "name";
                _DetailForm.title = "title";
                _DetailForm.isReadOnly = "isReadOnly";
                _DetailForm.hideMetaInformation = "hideMetaInformation";
                _DetailForm.originalUri = "originalUri";
                _DetailForm.creationProtocol = "creationProtocol";
            })(_DetailForm = _Forms._DetailForm || (_Forms._DetailForm = {}));
            _Forms.__DetailForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DetailForm";
            let _ListForm;
            (function (_ListForm) {
                _ListForm.property = "property";
                _ListForm.metaClass = "metaClass";
                _ListForm.includeDescendents = "includeDescendents";
                _ListForm.noItemsWithMetaClass = "noItemsWithMetaClass";
                _ListForm.inhibitNewItems = "inhibitNewItems";
                _ListForm.inhibitDeleteItems = "inhibitDeleteItems";
                _ListForm.inhibitEditItems = "inhibitEditItems";
                _ListForm.defaultTypesForNewElements = "defaultTypesForNewElements";
                _ListForm.fastViewFilters = "fastViewFilters";
                _ListForm.field = "field";
                _ListForm.sortingOrder = "sortingOrder";
                _ListForm.viewNode = "viewNode";
                _ListForm.autoGenerateFields = "autoGenerateFields";
                _ListForm.duplicatePerType = "duplicatePerType";
                _ListForm.name = "name";
                _ListForm.title = "title";
                _ListForm.isReadOnly = "isReadOnly";
                _ListForm.hideMetaInformation = "hideMetaInformation";
                _ListForm.originalUri = "originalUri";
                _ListForm.creationProtocol = "creationProtocol";
            })(_ListForm = _Forms._ListForm || (_Forms._ListForm = {}));
            _Forms.__ListForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ListForm";
            let _ExtentForm;
            (function (_ExtentForm) {
                _ExtentForm.tab = "tab";
                _ExtentForm.autoTabs = "autoTabs";
                _ExtentForm.name = "name";
                _ExtentForm.title = "title";
                _ExtentForm.isReadOnly = "isReadOnly";
                _ExtentForm.hideMetaInformation = "hideMetaInformation";
                _ExtentForm.originalUri = "originalUri";
                _ExtentForm.creationProtocol = "creationProtocol";
            })(_ExtentForm = _Forms._ExtentForm || (_Forms._ExtentForm = {}));
            _Forms.__ExtentForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ExtentForm";
            let _ViewMode;
            (function (_ViewMode) {
                _ViewMode.name = "name";
                _ViewMode.id = "id";
                _ViewMode.defaultExtentType = "defaultExtentType";
            })(_ViewMode = _Forms._ViewMode || (_Forms._ViewMode = {}));
            _Forms.__ViewMode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode";
        })(_Forms = _DatenMeister._Forms || (_DatenMeister._Forms = {}));
        let _AttachedExtent;
        (function (_AttachedExtent) {
            let _AttachedExtentConfiguration;
            (function (_AttachedExtentConfiguration) {
                _AttachedExtentConfiguration.name = "name";
                _AttachedExtentConfiguration.referencedWorkspace = "referencedWorkspace";
                _AttachedExtentConfiguration.referencedExtent = "referencedExtent";
                _AttachedExtentConfiguration.referenceType = "referenceType";
                _AttachedExtentConfiguration.referenceProperty = "referenceProperty";
            })(_AttachedExtentConfiguration = _AttachedExtent._AttachedExtentConfiguration || (_AttachedExtent._AttachedExtentConfiguration = {}));
            _AttachedExtent.__AttachedExtentConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration";
        })(_AttachedExtent = _DatenMeister._AttachedExtent || (_DatenMeister._AttachedExtent = {}));
        let _Management;
        (function (_Management) {
            let _ExtentLoadingState;
            (function (_ExtentLoadingState) {
                _ExtentLoadingState.Unknown = "Unknown";
                _ExtentLoadingState.Unloaded = "Unloaded";
                _ExtentLoadingState.Loaded = "Loaded";
                _ExtentLoadingState.Failed = "Failed";
                _ExtentLoadingState.LoadedReadOnly = "LoadedReadOnly";
            })(_ExtentLoadingState = _Management._ExtentLoadingState || (_Management._ExtentLoadingState = {}));
            let ___ExtentLoadingState;
            (function (___ExtentLoadingState) {
                ___ExtentLoadingState[___ExtentLoadingState["Unknown"] = 0] = "Unknown";
                ___ExtentLoadingState[___ExtentLoadingState["Unloaded"] = 1] = "Unloaded";
                ___ExtentLoadingState[___ExtentLoadingState["Loaded"] = 2] = "Loaded";
                ___ExtentLoadingState[___ExtentLoadingState["Failed"] = 3] = "Failed";
                ___ExtentLoadingState[___ExtentLoadingState["LoadedReadOnly"] = 4] = "LoadedReadOnly";
            })(___ExtentLoadingState = _Management.___ExtentLoadingState || (_Management.___ExtentLoadingState = {}));
            let _Extent;
            (function (_Extent) {
                _Extent.name = "name";
                _Extent.uri = "uri";
                _Extent.workspaceId = "workspaceId";
                _Extent.count = "count";
                _Extent.totalCount = "totalCount";
                _Extent.type = "type";
                _Extent.extentType = "extentType";
                _Extent.isModified = "isModified";
                _Extent.alternativeUris = "alternativeUris";
                _Extent.autoEnumerateType = "autoEnumerateType";
                _Extent.state = "state";
                _Extent.failMessage = "failMessage";
                _Extent.properties = "properties";
                _Extent.loadingConfiguration = "loadingConfiguration";
            })(_Extent = _Management._Extent || (_Management._Extent = {}));
            _Management.__Extent_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent";
            let _Workspace;
            (function (_Workspace) {
                _Workspace.id = "id";
                _Workspace.annotation = "annotation";
                _Workspace.extents = "extents";
            })(_Workspace = _Management._Workspace || (_Management._Workspace = {}));
            _Management.__Workspace_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace";
            let _CreateNewWorkspaceModel;
            (function (_CreateNewWorkspaceModel) {
                _CreateNewWorkspaceModel.id = "id";
                _CreateNewWorkspaceModel.annotation = "annotation";
            })(_CreateNewWorkspaceModel = _Management._CreateNewWorkspaceModel || (_Management._CreateNewWorkspaceModel = {}));
            _Management.__CreateNewWorkspaceModel_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel";
            let _ExtentTypeSetting;
            (function (_ExtentTypeSetting) {
                _ExtentTypeSetting.name = "name";
                _ExtentTypeSetting.rootElementMetaClasses = "rootElementMetaClasses";
            })(_ExtentTypeSetting = _Management._ExtentTypeSetting || (_Management._ExtentTypeSetting = {}));
            _Management.__ExtentTypeSetting_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting";
            let _ExtentProperties;
            (function (_ExtentProperties) {
                _ExtentProperties.name = "name";
                _ExtentProperties.uri = "uri";
                _ExtentProperties.workspaceId = "workspaceId";
                _ExtentProperties.count = "count";
                _ExtentProperties.totalCount = "totalCount";
                _ExtentProperties.type = "type";
                _ExtentProperties.extentType = "extentType";
                _ExtentProperties.isModified = "isModified";
                _ExtentProperties.alternativeUris = "alternativeUris";
                _ExtentProperties.autoEnumerateType = "autoEnumerateType";
                _ExtentProperties.state = "state";
                _ExtentProperties.failMessage = "failMessage";
                _ExtentProperties.properties = "properties";
                _ExtentProperties.loadingConfiguration = "loadingConfiguration";
            })(_ExtentProperties = _Management._ExtentProperties || (_Management._ExtentProperties = {}));
            _Management.__ExtentProperties_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties";
            let _ExtentPropertyDefinition;
            (function (_ExtentPropertyDefinition) {
                _ExtentPropertyDefinition.name = "name";
                _ExtentPropertyDefinition.title = "title";
                _ExtentPropertyDefinition.metaClass = "metaClass";
            })(_ExtentPropertyDefinition = _Management._ExtentPropertyDefinition || (_Management._ExtentPropertyDefinition = {}));
            _Management.__ExtentPropertyDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition";
            let _ExtentSettings;
            (function (_ExtentSettings) {
                _ExtentSettings.extentTypeSettings = "extentTypeSettings";
                _ExtentSettings.propertyDefinitions = "propertyDefinitions";
            })(_ExtentSettings = _Management._ExtentSettings || (_Management._ExtentSettings = {}));
            _Management.__ExtentSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings";
        })(_Management = _DatenMeister._Management || (_DatenMeister._Management = {}));
        let _FastViewFilters;
        (function (_FastViewFilters) {
            let _ComparisonType;
            (function (_ComparisonType) {
                _ComparisonType.Equal = "Equal";
                _ComparisonType.GreaterThan = "GreaterThan";
                _ComparisonType.LighterThan = "LighterThan";
                _ComparisonType.GreaterOrEqualThan = "GreaterOrEqualThan";
                _ComparisonType.LighterOrEqualThan = "LighterOrEqualThan";
            })(_ComparisonType = _FastViewFilters._ComparisonType || (_FastViewFilters._ComparisonType = {}));
            let ___ComparisonType;
            (function (___ComparisonType) {
                ___ComparisonType[___ComparisonType["Equal"] = 0] = "Equal";
                ___ComparisonType[___ComparisonType["GreaterThan"] = 1] = "GreaterThan";
                ___ComparisonType[___ComparisonType["LighterThan"] = 2] = "LighterThan";
                ___ComparisonType[___ComparisonType["GreaterOrEqualThan"] = 3] = "GreaterOrEqualThan";
                ___ComparisonType[___ComparisonType["LighterOrEqualThan"] = 4] = "LighterOrEqualThan";
            })(___ComparisonType = _FastViewFilters.___ComparisonType || (_FastViewFilters.___ComparisonType = {}));
            let _PropertyComparisonFilter;
            (function (_PropertyComparisonFilter) {
                _PropertyComparisonFilter.Property = "Property";
                _PropertyComparisonFilter.ComparisonType = "ComparisonType";
                _PropertyComparisonFilter.Value = "Value";
            })(_PropertyComparisonFilter = _FastViewFilters._PropertyComparisonFilter || (_FastViewFilters._PropertyComparisonFilter = {}));
            _FastViewFilters.__PropertyComparisonFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter";
            let _PropertyContainsFilter;
            (function (_PropertyContainsFilter) {
                _PropertyContainsFilter.Property = "Property";
                _PropertyContainsFilter.Value = "Value";
            })(_PropertyContainsFilter = _FastViewFilters._PropertyContainsFilter || (_FastViewFilters._PropertyContainsFilter = {}));
            _FastViewFilters.__PropertyContainsFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter";
        })(_FastViewFilters = _DatenMeister._FastViewFilters || (_DatenMeister._FastViewFilters = {}));
        let _DynamicRuntimeProvider;
        (function (_DynamicRuntimeProvider) {
            let _DynamicRuntimeLoaderConfig;
            (function (_DynamicRuntimeLoaderConfig) {
                _DynamicRuntimeLoaderConfig.runtimeClass = "runtimeClass";
                _DynamicRuntimeLoaderConfig.configuration = "configuration";
                _DynamicRuntimeLoaderConfig.name = "name";
                _DynamicRuntimeLoaderConfig.extentUri = "extentUri";
                _DynamicRuntimeLoaderConfig.workspaceId = "workspaceId";
                _DynamicRuntimeLoaderConfig.dropExisting = "dropExisting";
            })(_DynamicRuntimeLoaderConfig = _DynamicRuntimeProvider._DynamicRuntimeLoaderConfig || (_DynamicRuntimeProvider._DynamicRuntimeLoaderConfig = {}));
            _DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig_Uri = "dm:///_internal/types/internal#8be3c0ea-ef40-4b4a-a4ea-9262e924d7b8";
            let _Examples;
            (function (_Examples) {
                let _NumberProviderSettings;
                (function (_NumberProviderSettings) {
                    _NumberProviderSettings.name = "name";
                    _NumberProviderSettings.start = "start";
                    _NumberProviderSettings.end = "end";
                })(_NumberProviderSettings = _Examples._NumberProviderSettings || (_Examples._NumberProviderSettings = {}));
                _Examples.__NumberProviderSettings_Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a";
                let _NumberRepresentation;
                (function (_NumberRepresentation) {
                    _NumberRepresentation.binary = "binary";
                    _NumberRepresentation.octal = "octal";
                    _NumberRepresentation.decimal = "decimal";
                    _NumberRepresentation.hexadecimal = "hexadecimal";
                })(_NumberRepresentation = _Examples._NumberRepresentation || (_Examples._NumberRepresentation = {}));
                _Examples.__NumberRepresentation_Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation";
            })(_Examples = _DynamicRuntimeProvider._Examples || (_DynamicRuntimeProvider._Examples = {}));
        })(_DynamicRuntimeProvider = _DatenMeister._DynamicRuntimeProvider || (_DatenMeister._DynamicRuntimeProvider = {}));
    })(_DatenMeister = exports._DatenMeister || (exports._DatenMeister = {}));
});
//# sourceMappingURL=DatenMeister.class.js.map