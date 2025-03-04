// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.0.0.0
export var _PrimitiveTypes;
(function (_PrimitiveTypes) {
    class _DateTime {
    }
    _PrimitiveTypes._DateTime = _DateTime;
    _PrimitiveTypes.__DateTime_Uri = "dm:///_internal/types/internal#PrimitiveTypes.DateTime";
})(_PrimitiveTypes || (_PrimitiveTypes = {}));
export var _DatenMeister;
(function (_DatenMeister) {
    let _CommonTypes;
    (function (_CommonTypes) {
        let _Default;
        (function (_Default) {
            class _Package {
            }
            _Package._name_ = "name";
            _Package.packagedElement = "packagedElement";
            _Package.preferredType = "preferredType";
            _Package.preferredPackage = "preferredPackage";
            _Package.defaultViewMode = "defaultViewMode";
            _Default._Package = _Package;
            _Default.__Package_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package";
            class _XmiExportContainer {
            }
            _XmiExportContainer.xmi = "xmi";
            _Default._XmiExportContainer = _XmiExportContainer;
            _Default.__XmiExportContainer_Uri = "dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1";
            class _XmiImportContainer {
            }
            _XmiImportContainer.xmi = "xmi";
            _XmiImportContainer.property = "property";
            _XmiImportContainer.addToCollection = "addToCollection";
            _Default._XmiImportContainer = _XmiImportContainer;
            _Default.__XmiImportContainer_Uri = "dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a";
        })(_Default = _CommonTypes._Default || (_CommonTypes._Default = {}));
        let _ExtentManager;
        (function (_ExtentManager) {
            class _ImportSettings {
            }
            _ImportSettings.filePath = "filePath";
            _ImportSettings.extentUri = "extentUri";
            _ImportSettings.workspaceId = "workspaceId";
            _ExtentManager._ImportSettings = _ImportSettings;
            _ExtentManager.__ImportSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings";
            class _ImportException {
            }
            _ImportException.message = "message";
            _ExtentManager._ImportException = _ImportException;
            _ExtentManager.__ImportException_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException";
        })(_ExtentManager = _CommonTypes._ExtentManager || (_CommonTypes._ExtentManager = {}));
        let _OSIntegration;
        (function (_OSIntegration) {
            class _CommandLineApplication {
            }
            _CommandLineApplication._name_ = "name";
            _CommandLineApplication.applicationPath = "applicationPath";
            _OSIntegration._CommandLineApplication = _CommandLineApplication;
            _OSIntegration.__CommandLineApplication_Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication";
            class _EnvironmentalVariable {
            }
            _EnvironmentalVariable._name_ = "name";
            _EnvironmentalVariable.value = "value";
            _OSIntegration._EnvironmentalVariable = _EnvironmentalVariable;
            _OSIntegration.__EnvironmentalVariable_Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable";
        })(_OSIntegration = _CommonTypes._OSIntegration || (_CommonTypes._OSIntegration = {}));
    })(_CommonTypes = _DatenMeister._CommonTypes || (_DatenMeister._CommonTypes = {}));
    let _Actions;
    (function (_Actions) {
        class _ActionSet {
        }
        _ActionSet.action = "action";
        _ActionSet._name_ = "name";
        _ActionSet.isDisabled = "isDisabled";
        _Actions._ActionSet = _ActionSet;
        _Actions.__ActionSet_Uri = "dm:///_internal/types/internal#Actions.ActionSet";
        class _LoggingWriterAction {
        }
        _LoggingWriterAction.message = "message";
        _LoggingWriterAction._name_ = "name";
        _LoggingWriterAction.isDisabled = "isDisabled";
        _Actions._LoggingWriterAction = _LoggingWriterAction;
        _Actions.__LoggingWriterAction_Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction";
        class _CommandExecutionAction {
        }
        _CommandExecutionAction.command = "command";
        _CommandExecutionAction._arguments_ = "arguments";
        _CommandExecutionAction.workingDirectory = "workingDirectory";
        _CommandExecutionAction._name_ = "name";
        _CommandExecutionAction.isDisabled = "isDisabled";
        _Actions._CommandExecutionAction = _CommandExecutionAction;
        _Actions.__CommandExecutionAction_Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82";
        class _PowershellExecutionAction {
        }
        _PowershellExecutionAction.script = "script";
        _PowershellExecutionAction.workingDirectory = "workingDirectory";
        _PowershellExecutionAction._name_ = "name";
        _PowershellExecutionAction.isDisabled = "isDisabled";
        _Actions._PowershellExecutionAction = _PowershellExecutionAction;
        _Actions.__PowershellExecutionAction_Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb";
        class _LoadExtentAction {
        }
        _LoadExtentAction.configuration = "configuration";
        _LoadExtentAction.dropExisting = "dropExisting";
        _LoadExtentAction._name_ = "name";
        _LoadExtentAction.isDisabled = "isDisabled";
        _Actions._LoadExtentAction = _LoadExtentAction;
        _Actions.__LoadExtentAction_Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee";
        class _DropExtentAction {
        }
        _DropExtentAction.workspaceId = "workspaceId";
        _DropExtentAction.extentUri = "extentUri";
        _DropExtentAction._name_ = "name";
        _DropExtentAction.isDisabled = "isDisabled";
        _Actions._DropExtentAction = _DropExtentAction;
        _Actions.__DropExtentAction_Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09";
        class _CreateWorkspaceAction {
        }
        _CreateWorkspaceAction.workspaceId = "workspaceId";
        _CreateWorkspaceAction.annotation = "annotation";
        _CreateWorkspaceAction._name_ = "name";
        _CreateWorkspaceAction.isDisabled = "isDisabled";
        _Actions._CreateWorkspaceAction = _CreateWorkspaceAction;
        _Actions.__CreateWorkspaceAction_Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe";
        class _DropWorkspaceAction {
        }
        _DropWorkspaceAction.workspaceId = "workspaceId";
        _DropWorkspaceAction._name_ = "name";
        _DropWorkspaceAction.isDisabled = "isDisabled";
        _Actions._DropWorkspaceAction = _DropWorkspaceAction;
        _Actions.__DropWorkspaceAction_Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8";
        class _CopyElementsAction {
        }
        _CopyElementsAction.sourcePath = "sourcePath";
        _CopyElementsAction.targetPath = "targetPath";
        _CopyElementsAction.moveOnly = "moveOnly";
        _CopyElementsAction.sourceWorkspace = "sourceWorkspace";
        _CopyElementsAction.targetWorkspace = "targetWorkspace";
        _CopyElementsAction.emptyTarget = "emptyTarget";
        _CopyElementsAction._name_ = "name";
        _CopyElementsAction.isDisabled = "isDisabled";
        _Actions._CopyElementsAction = _CopyElementsAction;
        _Actions.__CopyElementsAction_Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed";
        class _ExportToXmiAction {
        }
        _ExportToXmiAction.sourcePath = "sourcePath";
        _ExportToXmiAction.filePath = "filePath";
        _ExportToXmiAction.sourceWorkspaceId = "sourceWorkspaceId";
        _ExportToXmiAction._name_ = "name";
        _ExportToXmiAction.isDisabled = "isDisabled";
        _Actions._ExportToXmiAction = _ExportToXmiAction;
        _Actions.__ExportToXmiAction_Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863";
        class _ClearCollectionAction {
        }
        _ClearCollectionAction.workspaceId = "workspaceId";
        _ClearCollectionAction.path = "path";
        _ClearCollectionAction._name_ = "name";
        _ClearCollectionAction.isDisabled = "isDisabled";
        _Actions._ClearCollectionAction = _ClearCollectionAction;
        _Actions.__ClearCollectionAction_Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae";
        class _TransformItemsAction {
        }
        _TransformItemsAction.metaClass = "metaClass";
        _TransformItemsAction.runtimeClass = "runtimeClass";
        _TransformItemsAction.workspaceId = "workspaceId";
        _TransformItemsAction.path = "path";
        _TransformItemsAction.excludeDescendents = "excludeDescendents";
        _TransformItemsAction._name_ = "name";
        _TransformItemsAction.isDisabled = "isDisabled";
        _Actions._TransformItemsAction = _TransformItemsAction;
        _Actions.__TransformItemsAction_Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler";
        class _EchoAction {
        }
        _EchoAction.shallSuccess = "shallSuccess";
        _EchoAction._name_ = "name";
        _EchoAction.isDisabled = "isDisabled";
        _Actions._EchoAction = _EchoAction;
        _Actions.__EchoAction_Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction";
        class _DocumentOpenAction {
        }
        _DocumentOpenAction.filePath = "filePath";
        _DocumentOpenAction._name_ = "name";
        _DocumentOpenAction.isDisabled = "isDisabled";
        _Actions._DocumentOpenAction = _DocumentOpenAction;
        _Actions.__DocumentOpenAction_Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74";
        class _CreateFormByMetaClass {
        }
        _CreateFormByMetaClass.metaClass = "metaClass";
        _CreateFormByMetaClass.creationMode = "creationMode";
        _CreateFormByMetaClass.targetContainer = "targetContainer";
        _CreateFormByMetaClass._name_ = "name";
        _CreateFormByMetaClass.isDisabled = "isDisabled";
        _Actions._CreateFormByMetaClass = _CreateFormByMetaClass;
        _Actions.__CreateFormByMetaClass_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.CreateFormByMetaclass";
        let _Reports;
        (function (_Reports) {
            class _SimpleReportAction {
            }
            _SimpleReportAction.path = "path";
            _SimpleReportAction.configuration = "configuration";
            _SimpleReportAction.workspaceId = "workspaceId";
            _SimpleReportAction.filePath = "filePath";
            _SimpleReportAction._name_ = "name";
            _SimpleReportAction.isDisabled = "isDisabled";
            _Reports._SimpleReportAction = _SimpleReportAction;
            _Reports.__SimpleReportAction_Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction";
            class _AdocReportAction {
            }
            _AdocReportAction.filePath = "filePath";
            _AdocReportAction.reportInstance = "reportInstance";
            _AdocReportAction._name_ = "name";
            _AdocReportAction.isDisabled = "isDisabled";
            _Reports._AdocReportAction = _AdocReportAction;
            _Reports.__AdocReportAction_Uri = "dm:///_internal/types/internal#Actions.AdocReportAction";
            class _HtmlReportAction {
            }
            _HtmlReportAction.filePath = "filePath";
            _HtmlReportAction.reportInstance = "reportInstance";
            _HtmlReportAction._name_ = "name";
            _HtmlReportAction.isDisabled = "isDisabled";
            _Reports._HtmlReportAction = _HtmlReportAction;
            _Reports.__HtmlReportAction_Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction";
        })(_Reports = _Actions._Reports || (_Actions._Reports = {}));
        class _Action {
        }
        _Action._name_ = "name";
        _Action.isDisabled = "isDisabled";
        _Actions._Action = _Action;
        _Actions.__Action_Uri = "dm:///_internal/types/internal#Actions.Action";
        class _MoveOrCopyAction {
        }
        _MoveOrCopyAction.copyMode = "copyMode";
        _MoveOrCopyAction.target = "target";
        _MoveOrCopyAction.source = "source";
        _Actions._MoveOrCopyAction = _MoveOrCopyAction;
        _Actions.__MoveOrCopyAction_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.MoveOrCopyAction";
        let _MoveOrCopyType;
        (function (_MoveOrCopyType) {
            _MoveOrCopyType.Copy = "Copy";
            _MoveOrCopyType.Move = "Move";
        })(_MoveOrCopyType = _Actions._MoveOrCopyType || (_Actions._MoveOrCopyType = {}));
        let ___MoveOrCopyType;
        (function (___MoveOrCopyType) {
            ___MoveOrCopyType[___MoveOrCopyType["Copy"] = 0] = "Copy";
            ___MoveOrCopyType[___MoveOrCopyType["Move"] = 1] = "Move";
        })(___MoveOrCopyType = _Actions.___MoveOrCopyType || (_Actions.___MoveOrCopyType = {}));
        let _MoveDirectionType;
        (function (_MoveDirectionType) {
            _MoveDirectionType.Up = "Up";
            _MoveDirectionType.Down = "Down";
        })(_MoveDirectionType = _Actions._MoveDirectionType || (_Actions._MoveDirectionType = {}));
        let ___MoveDirectionType;
        (function (___MoveDirectionType) {
            ___MoveDirectionType[___MoveDirectionType["Up"] = 0] = "Up";
            ___MoveDirectionType[___MoveDirectionType["Down"] = 1] = "Down";
        })(___MoveDirectionType = _Actions.___MoveDirectionType || (_Actions.___MoveDirectionType = {}));
        class _MoveUpDownAction {
        }
        _MoveUpDownAction.element = "element";
        _MoveUpDownAction.direction = "direction";
        _MoveUpDownAction.container = "container";
        _MoveUpDownAction.property = "property";
        _Actions._MoveUpDownAction = _MoveUpDownAction;
        _Actions.__MoveUpDownAction_Uri = "dm:///_internal/types/internal#bc4952bf-a3f5-4516-be26-5b773e38bd54";
        class _StoreExtentAction {
        }
        _StoreExtentAction.workspaceId = "workspaceId";
        _StoreExtentAction.extentUri = "extentUri";
        _StoreExtentAction._name_ = "name";
        _StoreExtentAction.isDisabled = "isDisabled";
        _Actions._StoreExtentAction = _StoreExtentAction;
        _Actions.__StoreExtentAction_Uri = "dm:///_internal/types/internal#43b0764e-b70f-42bb-b37d-ae8586ec45f1";
        class _ImportXmiAction {
        }
        _ImportXmiAction.workspaceId = "workspaceId";
        _ImportXmiAction.itemUri = "itemUri";
        _ImportXmiAction.xmi = "xmi";
        _ImportXmiAction.property = "property";
        _ImportXmiAction.addToCollection = "addToCollection";
        _Actions._ImportXmiAction = _ImportXmiAction;
        _Actions.__ImportXmiAction_Uri = "dm:///_internal/types/internal#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d";
        class _DeletePropertyFromCollectionAction {
        }
        _DeletePropertyFromCollectionAction.propertyName = "propertyName";
        _DeletePropertyFromCollectionAction.metaclass = "metaclass";
        _DeletePropertyFromCollectionAction.collectionUrl = "collectionUrl";
        _Actions._DeletePropertyFromCollectionAction = _DeletePropertyFromCollectionAction;
        _Actions.__DeletePropertyFromCollectionAction_Uri = "dm:///_internal/types/internal#b631bb00-ab11-4a8a-a148-e28abc398503";
        class _MoveOrCopyActionResult {
        }
        _MoveOrCopyActionResult.targetUrl = "targetUrl";
        _MoveOrCopyActionResult.targetWorkspace = "targetWorkspace";
        _Actions._MoveOrCopyActionResult = _MoveOrCopyActionResult;
        _Actions.__MoveOrCopyActionResult_Uri = "dm:///_internal/types/internal#3223e13a-bbb7-4785-8b81-7275be23b0a1";
        let _ParameterTypes;
        (function (_ParameterTypes) {
            class _NavigationDefineActionParameter {
            }
            _NavigationDefineActionParameter.actionName = "actionName";
            _NavigationDefineActionParameter.formUrl = "formUrl";
            _NavigationDefineActionParameter.metaClassUrl = "metaClassUrl";
            _ParameterTypes._NavigationDefineActionParameter = _NavigationDefineActionParameter;
            _ParameterTypes.__NavigationDefineActionParameter_Uri = "dm:///_internal/types/internal#90f61e4e-a5ea-42eb-9caa-912d010fbccd";
        })(_ParameterTypes = _Actions._ParameterTypes || (_Actions._ParameterTypes = {}));
        class _ActionResult {
        }
        _ActionResult.isSuccess = "isSuccess";
        _ActionResult.clientActions = "clientActions";
        _Actions._ActionResult = _ActionResult;
        _Actions.__ActionResult_Uri = "dm:///_internal/types/internal#899324b1-85dc-40a1-ba95-dec50509040d";
        let _ClientActions;
        (function (_ClientActions) {
            class _ClientAction {
            }
            _ClientAction.actionName = "actionName";
            _ClientAction.element = "element";
            _ClientAction.parameter = "parameter";
            _ClientActions._ClientAction = _ClientAction;
            _ClientActions.__ClientAction_Uri = "dm:///_internal/types/internal#e07ca80e-2540-4f91-8214-60dbd464e998";
            class _AlertClientAction {
            }
            _AlertClientAction.messageText = "messageText";
            _AlertClientAction.actionName = "actionName";
            _AlertClientAction.element = "element";
            _AlertClientAction.parameter = "parameter";
            _ClientActions._AlertClientAction = _AlertClientAction;
            _ClientActions.__AlertClientAction_Uri = "dm:///_internal/types/internal#0ee17f2a-5407-4d38-b1b4-34ead2186971";
            class _NavigateToExtentClientAction {
            }
            _NavigateToExtentClientAction.workspaceId = "workspaceId";
            _NavigateToExtentClientAction.extentUri = "extentUri";
            _ClientActions._NavigateToExtentClientAction = _NavigateToExtentClientAction;
            _ClientActions.__NavigateToExtentClientAction_Uri = "dm:///_internal/types/internal#3251783f-2683-4c24-bad5-828930028462";
            class _NavigateToItemClientAction {
            }
            _NavigateToItemClientAction.workspaceId = "workspaceId";
            _NavigateToItemClientAction.itemUrl = "itemUrl";
            _NavigateToItemClientAction.formUri = "formUri";
            _ClientActions._NavigateToItemClientAction = _NavigateToItemClientAction;
            _ClientActions.__NavigateToItemClientAction_Uri = "dm:///_internal/types/internal#5f69675e-df58-4ad7-84bf-359cdfba5db4";
        })(_ClientActions = _Actions._ClientActions || (_Actions._ClientActions = {}));
    })(_Actions = _DatenMeister._Actions || (_DatenMeister._Actions = {}));
    let _DataViews;
    (function (_DataViews) {
        class _DataView {
        }
        _DataView._name_ = "name";
        _DataView.workspaceId = "workspaceId";
        _DataView.uri = "uri";
        _DataView.viewNode = "viewNode";
        _DataViews._DataView = _DataView;
        _DataViews.__DataView_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView";
        class _ViewNode {
        }
        _ViewNode._name_ = "name";
        _DataViews._ViewNode = _ViewNode;
        _DataViews.__ViewNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode";
        class _SelectByExtentNode {
        }
        _SelectByExtentNode.extentUri = "extentUri";
        _SelectByExtentNode.workspaceId = "workspaceId";
        _SelectByExtentNode._name_ = "name";
        _DataViews._SelectByExtentNode = _SelectByExtentNode;
        _DataViews.__SelectByExtentNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode";
        class _FlattenNode {
        }
        _FlattenNode.input = "input";
        _FlattenNode._name_ = "name";
        _DataViews._FlattenNode = _FlattenNode;
        _DataViews.__FlattenNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode";
        class _FilterByPropertyValueNode {
        }
        _FilterByPropertyValueNode.input = "input";
        _FilterByPropertyValueNode.property = "property";
        _FilterByPropertyValueNode.value = "value";
        _FilterByPropertyValueNode.comparisonMode = "comparisonMode";
        _FilterByPropertyValueNode._name_ = "name";
        _DataViews._FilterByPropertyValueNode = _FilterByPropertyValueNode;
        _DataViews.__FilterByPropertyValueNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode";
        class _FilterByMetaclassNode {
        }
        _FilterByMetaclassNode.input = "input";
        _FilterByMetaclassNode.metaClass = "metaClass";
        _FilterByMetaclassNode.includeInherits = "includeInherits";
        _FilterByMetaclassNode._name_ = "name";
        _DataViews._FilterByMetaclassNode = _FilterByMetaclassNode;
        _DataViews.__FilterByMetaclassNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode";
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
        class _SelectByFullNameNode {
        }
        _SelectByFullNameNode.input = "input";
        _SelectByFullNameNode.path = "path";
        _SelectByFullNameNode._name_ = "name";
        _DataViews._SelectByFullNameNode = _SelectByFullNameNode;
        _DataViews.__SelectByFullNameNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode";
        class _DynamicSourceNode {
        }
        _DynamicSourceNode.nodeName = "nodeName";
        _DynamicSourceNode._name_ = "name";
        _DataViews._DynamicSourceNode = _DynamicSourceNode;
        _DataViews.__DynamicSourceNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode";
        class _SelectByPathNode {
        }
        _SelectByPathNode.workspaceId = "workspaceId";
        _SelectByPathNode.path = "path";
        _DataViews._SelectByPathNode = _SelectByPathNode;
        _DataViews.__SelectByPathNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode";
        class _QueryStatement {
        }
        _QueryStatement.nodes = "nodes";
        _QueryStatement.resultNode = "resultNode";
        _DataViews._QueryStatement = _QueryStatement;
        _DataViews.__QueryStatement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement";
    })(_DataViews = _DatenMeister._DataViews || (_DatenMeister._DataViews = {}));
    let _Reports;
    (function (_Reports) {
        class _ReportDefinition {
        }
        _ReportDefinition._name_ = "name";
        _ReportDefinition.title = "title";
        _ReportDefinition.elements = "elements";
        _Reports._ReportDefinition = _ReportDefinition;
        _Reports.__ReportDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition";
        class _ReportInstanceSource {
        }
        _ReportInstanceSource._name_ = "name";
        _ReportInstanceSource.workspaceId = "workspaceId";
        _ReportInstanceSource.path = "path";
        _Reports._ReportInstanceSource = _ReportInstanceSource;
        _Reports.__ReportInstanceSource_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource";
        class _ReportInstance {
        }
        _ReportInstance._name_ = "name";
        _ReportInstance.reportDefinition = "reportDefinition";
        _ReportInstance.sources = "sources";
        _Reports._ReportInstance = _ReportInstance;
        _Reports.__ReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance";
        class _AdocReportInstance {
        }
        _AdocReportInstance._name_ = "name";
        _AdocReportInstance.reportDefinition = "reportDefinition";
        _AdocReportInstance.sources = "sources";
        _Reports._AdocReportInstance = _AdocReportInstance;
        _Reports.__AdocReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance";
        class _HtmlReportInstance {
        }
        _HtmlReportInstance.cssFile = "cssFile";
        _HtmlReportInstance.cssStyleSheet = "cssStyleSheet";
        _HtmlReportInstance._name_ = "name";
        _HtmlReportInstance.reportDefinition = "reportDefinition";
        _HtmlReportInstance.sources = "sources";
        _Reports._HtmlReportInstance = _HtmlReportInstance;
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
        class _SimpleReportConfiguration {
        }
        _SimpleReportConfiguration._name_ = "name";
        _SimpleReportConfiguration.showDescendents = "showDescendents";
        _SimpleReportConfiguration.rootElement = "rootElement";
        _SimpleReportConfiguration.showRootElement = "showRootElement";
        _SimpleReportConfiguration.showMetaClasses = "showMetaClasses";
        _SimpleReportConfiguration.showFullName = "showFullName";
        _SimpleReportConfiguration.form = "form";
        _SimpleReportConfiguration.descendentMode = "descendentMode";
        _SimpleReportConfiguration.typeMode = "typeMode";
        _SimpleReportConfiguration.workspaceId = "workspaceId";
        _Reports._SimpleReportConfiguration = _SimpleReportConfiguration;
        _Reports.__SimpleReportConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration";
        let _Elements;
        (function (_Elements) {
            class _ReportElement {
            }
            _ReportElement._name_ = "name";
            _Elements._ReportElement = _ReportElement;
            _Elements.__ReportElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement";
            class _ReportHeadline {
            }
            _ReportHeadline.title = "title";
            _ReportHeadline._name_ = "name";
            _Elements._ReportHeadline = _ReportHeadline;
            _Elements.__ReportHeadline_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline";
            class _ReportParagraph {
            }
            _ReportParagraph.paragraph = "paragraph";
            _ReportParagraph.cssClass = "cssClass";
            _ReportParagraph.viewNode = "viewNode";
            _ReportParagraph.evalProperties = "evalProperties";
            _ReportParagraph.evalParagraph = "evalParagraph";
            _ReportParagraph._name_ = "name";
            _Elements._ReportParagraph = _ReportParagraph;
            _Elements.__ReportParagraph_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph";
            class _ReportTable {
            }
            _ReportTable.cssClass = "cssClass";
            _ReportTable.viewNode = "viewNode";
            _ReportTable.form = "form";
            _ReportTable.evalProperties = "evalProperties";
            _ReportTable._name_ = "name";
            _Elements._ReportTable = _ReportTable;
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
            class _ReportLoop {
            }
            _ReportLoop.viewNode = "viewNode";
            _ReportLoop.elements = "elements";
            _ReportLoop._name_ = "name";
            _Elements._ReportLoop = _ReportLoop;
            _Elements.__ReportLoop_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop";
        })(_Elements = _Reports._Elements || (_Reports._Elements = {}));
    })(_Reports = _DatenMeister._Reports || (_DatenMeister._Reports = {}));
    let _ExtentLoaderConfigs;
    (function (_ExtentLoaderConfigs) {
        class _ExtentLoaderConfig {
        }
        _ExtentLoaderConfig._name_ = "name";
        _ExtentLoaderConfig.extentUri = "extentUri";
        _ExtentLoaderConfig.workspaceId = "workspaceId";
        _ExtentLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._ExtentLoaderConfig = _ExtentLoaderConfig;
        _ExtentLoaderConfigs.__ExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig";
        class _ExcelLoaderConfig {
        }
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
        _ExcelLoaderConfig._name_ = "name";
        _ExcelLoaderConfig.extentUri = "extentUri";
        _ExcelLoaderConfig.workspaceId = "workspaceId";
        _ExcelLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._ExcelLoaderConfig = _ExcelLoaderConfig;
        _ExtentLoaderConfigs.__ExcelLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig";
        class _ExcelReferenceLoaderConfig {
        }
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
        _ExcelReferenceLoaderConfig._name_ = "name";
        _ExcelReferenceLoaderConfig.extentUri = "extentUri";
        _ExcelReferenceLoaderConfig.workspaceId = "workspaceId";
        _ExcelReferenceLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._ExcelReferenceLoaderConfig = _ExcelReferenceLoaderConfig;
        _ExtentLoaderConfigs.__ExcelReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig";
        class _ExcelImportLoaderConfig {
        }
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
        _ExcelImportLoaderConfig._name_ = "name";
        _ExcelImportLoaderConfig.extentUri = "extentUri";
        _ExcelImportLoaderConfig.workspaceId = "workspaceId";
        _ExcelImportLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._ExcelImportLoaderConfig = _ExcelImportLoaderConfig;
        _ExtentLoaderConfigs.__ExcelImportLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig";
        class _ExcelExtentLoaderConfig {
        }
        _ExcelExtentLoaderConfig.filePath = "filePath";
        _ExcelExtentLoaderConfig.idColumnName = "idColumnName";
        _ExcelExtentLoaderConfig._name_ = "name";
        _ExcelExtentLoaderConfig.extentUri = "extentUri";
        _ExcelExtentLoaderConfig.workspaceId = "workspaceId";
        _ExcelExtentLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._ExcelExtentLoaderConfig = _ExcelExtentLoaderConfig;
        _ExtentLoaderConfigs.__ExcelExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig";
        class _InMemoryLoaderConfig {
        }
        _InMemoryLoaderConfig._name_ = "name";
        _InMemoryLoaderConfig.extentUri = "extentUri";
        _InMemoryLoaderConfig.workspaceId = "workspaceId";
        _InMemoryLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._InMemoryLoaderConfig = _InMemoryLoaderConfig;
        _ExtentLoaderConfigs.__InMemoryLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig";
        class _XmlReferenceLoaderConfig {
        }
        _XmlReferenceLoaderConfig.filePath = "filePath";
        _XmlReferenceLoaderConfig.keepNamespaces = "keepNamespaces";
        _XmlReferenceLoaderConfig._name_ = "name";
        _XmlReferenceLoaderConfig.extentUri = "extentUri";
        _XmlReferenceLoaderConfig.workspaceId = "workspaceId";
        _XmlReferenceLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._XmlReferenceLoaderConfig = _XmlReferenceLoaderConfig;
        _ExtentLoaderConfigs.__XmlReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig";
        class _ExtentFileLoaderConfig {
        }
        _ExtentFileLoaderConfig.filePath = "filePath";
        _ExtentFileLoaderConfig._name_ = "name";
        _ExtentFileLoaderConfig.extentUri = "extentUri";
        _ExtentFileLoaderConfig.workspaceId = "workspaceId";
        _ExtentFileLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._ExtentFileLoaderConfig = _ExtentFileLoaderConfig;
        _ExtentLoaderConfigs.__ExtentFileLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig";
        class _XmiStorageLoaderConfig {
        }
        _XmiStorageLoaderConfig.filePath = "filePath";
        _XmiStorageLoaderConfig._name_ = "name";
        _XmiStorageLoaderConfig.extentUri = "extentUri";
        _XmiStorageLoaderConfig.workspaceId = "workspaceId";
        _XmiStorageLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._XmiStorageLoaderConfig = _XmiStorageLoaderConfig;
        _ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig";
        class _CsvExtentLoaderConfig {
        }
        _CsvExtentLoaderConfig.settings = "settings";
        _CsvExtentLoaderConfig.filePath = "filePath";
        _CsvExtentLoaderConfig._name_ = "name";
        _CsvExtentLoaderConfig.extentUri = "extentUri";
        _CsvExtentLoaderConfig.workspaceId = "workspaceId";
        _CsvExtentLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._CsvExtentLoaderConfig = _CsvExtentLoaderConfig;
        _ExtentLoaderConfigs.__CsvExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig";
        class _CsvSettings {
        }
        _CsvSettings.encoding = "encoding";
        _CsvSettings.hasHeader = "hasHeader";
        _CsvSettings.separator = "separator";
        _CsvSettings.columns = "columns";
        _CsvSettings.metaclassUri = "metaclassUri";
        _CsvSettings.trimCells = "trimCells";
        _ExtentLoaderConfigs._CsvSettings = _CsvSettings;
        _ExtentLoaderConfigs.__CsvSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings";
        class _ExcelHierarchicalColumnDefinition {
        }
        _ExcelHierarchicalColumnDefinition._name_ = "name";
        _ExcelHierarchicalColumnDefinition.metaClass = "metaClass";
        _ExcelHierarchicalColumnDefinition.property = "property";
        _ExtentLoaderConfigs._ExcelHierarchicalColumnDefinition = _ExcelHierarchicalColumnDefinition;
        _ExtentLoaderConfigs.__ExcelHierarchicalColumnDefinition_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition";
        class _ExcelHierarchicalLoaderConfig {
        }
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
        _ExcelHierarchicalLoaderConfig._name_ = "name";
        _ExcelHierarchicalLoaderConfig.extentUri = "extentUri";
        _ExcelHierarchicalLoaderConfig.workspaceId = "workspaceId";
        _ExcelHierarchicalLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._ExcelHierarchicalLoaderConfig = _ExcelHierarchicalLoaderConfig;
        _ExtentLoaderConfigs.__ExcelHierarchicalLoaderConfig_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig";
        class _ExcelColumn {
        }
        _ExcelColumn.header = "header";
        _ExcelColumn._name_ = "name";
        _ExtentLoaderConfigs._ExcelColumn = _ExcelColumn;
        _ExtentLoaderConfigs.__ExcelColumn_Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a";
        class _EnvironmentalVariableLoaderConfig {
        }
        _EnvironmentalVariableLoaderConfig._name_ = "name";
        _EnvironmentalVariableLoaderConfig.extentUri = "extentUri";
        _EnvironmentalVariableLoaderConfig.workspaceId = "workspaceId";
        _EnvironmentalVariableLoaderConfig.dropExisting = "dropExisting";
        _ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig = _EnvironmentalVariableLoaderConfig;
        _ExtentLoaderConfigs.__EnvironmentalVariableLoaderConfig_Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3";
    })(_ExtentLoaderConfigs = _DatenMeister._ExtentLoaderConfigs || (_DatenMeister._ExtentLoaderConfigs = {}));
    let _Forms;
    (function (_Forms) {
        class _FieldData {
        }
        _FieldData.isAttached = "isAttached";
        _FieldData._name_ = "name";
        _FieldData.title = "title";
        _FieldData.isEnumeration = "isEnumeration";
        _FieldData.defaultValue = "defaultValue";
        _FieldData.isReadOnly = "isReadOnly";
        _Forms._FieldData = _FieldData;
        _Forms.__FieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData";
        class _SortingOrder {
        }
        _SortingOrder._name_ = "name";
        _SortingOrder.isDescending = "isDescending";
        _Forms._SortingOrder = _SortingOrder;
        _Forms.__SortingOrder_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder";
        class _AnyDataFieldData {
        }
        _AnyDataFieldData.isAttached = "isAttached";
        _AnyDataFieldData._name_ = "name";
        _AnyDataFieldData.title = "title";
        _AnyDataFieldData.isEnumeration = "isEnumeration";
        _AnyDataFieldData.defaultValue = "defaultValue";
        _AnyDataFieldData.isReadOnly = "isReadOnly";
        _Forms._AnyDataFieldData = _AnyDataFieldData;
        _Forms.__AnyDataFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData";
        class _CheckboxFieldData {
        }
        _CheckboxFieldData.lineHeight = "lineHeight";
        _CheckboxFieldData.isAttached = "isAttached";
        _CheckboxFieldData._name_ = "name";
        _CheckboxFieldData.title = "title";
        _CheckboxFieldData.isEnumeration = "isEnumeration";
        _CheckboxFieldData.defaultValue = "defaultValue";
        _CheckboxFieldData.isReadOnly = "isReadOnly";
        _Forms._CheckboxFieldData = _CheckboxFieldData;
        _Forms.__CheckboxFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData";
        class _ActionFieldData {
        }
        _ActionFieldData.actionName = "actionName";
        _ActionFieldData.parameter = "parameter";
        _ActionFieldData.buttonText = "buttonText";
        _ActionFieldData.isAttached = "isAttached";
        _ActionFieldData._name_ = "name";
        _ActionFieldData.title = "title";
        _ActionFieldData.isEnumeration = "isEnumeration";
        _ActionFieldData.defaultValue = "defaultValue";
        _ActionFieldData.isReadOnly = "isReadOnly";
        _Forms._ActionFieldData = _ActionFieldData;
        _Forms.__ActionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData";
        class _DateTimeFieldData {
        }
        _DateTimeFieldData.hideDate = "hideDate";
        _DateTimeFieldData.hideTime = "hideTime";
        _DateTimeFieldData.showOffsetButtons = "showOffsetButtons";
        _DateTimeFieldData.isAttached = "isAttached";
        _DateTimeFieldData._name_ = "name";
        _DateTimeFieldData.title = "title";
        _DateTimeFieldData.isEnumeration = "isEnumeration";
        _DateTimeFieldData.defaultValue = "defaultValue";
        _DateTimeFieldData.isReadOnly = "isReadOnly";
        _Forms._DateTimeFieldData = _DateTimeFieldData;
        _Forms.__DateTimeFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData";
        class _FormAssociation {
        }
        _FormAssociation._name_ = "name";
        _FormAssociation.formType = "formType";
        _FormAssociation.metaClass = "metaClass";
        _FormAssociation.extentType = "extentType";
        _FormAssociation.viewModeId = "viewModeId";
        _FormAssociation.parentMetaClass = "parentMetaClass";
        _FormAssociation.parentProperty = "parentProperty";
        _FormAssociation.form = "form";
        _FormAssociation.debugActive = "debugActive";
        _FormAssociation.workspaceId = "workspaceId";
        _FormAssociation.extentUri = "extentUri";
        _Forms._FormAssociation = _FormAssociation;
        _Forms.__FormAssociation_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation";
        class _DropDownFieldData {
        }
        _DropDownFieldData.values = "values";
        _DropDownFieldData.valuesByEnumeration = "valuesByEnumeration";
        _DropDownFieldData.isAttached = "isAttached";
        _DropDownFieldData._name_ = "name";
        _DropDownFieldData.title = "title";
        _DropDownFieldData.isEnumeration = "isEnumeration";
        _DropDownFieldData.defaultValue = "defaultValue";
        _DropDownFieldData.isReadOnly = "isReadOnly";
        _Forms._DropDownFieldData = _DropDownFieldData;
        _Forms.__DropDownFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData";
        class _ValuePair {
        }
        _ValuePair.value = "value";
        _ValuePair._name_ = "name";
        _Forms._ValuePair = _ValuePair;
        _Forms.__ValuePair_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair";
        class _MetaClassElementFieldData {
        }
        _MetaClassElementFieldData.isAttached = "isAttached";
        _MetaClassElementFieldData._name_ = "name";
        _MetaClassElementFieldData.title = "title";
        _MetaClassElementFieldData.isEnumeration = "isEnumeration";
        _MetaClassElementFieldData.defaultValue = "defaultValue";
        _MetaClassElementFieldData.isReadOnly = "isReadOnly";
        _Forms._MetaClassElementFieldData = _MetaClassElementFieldData;
        _Forms.__MetaClassElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData";
        class _ReferenceFieldData {
        }
        _ReferenceFieldData.isSelectionInline = "isSelectionInline";
        _ReferenceFieldData.defaultWorkspace = "defaultWorkspace";
        _ReferenceFieldData.defaultItemUri = "defaultItemUri";
        _ReferenceFieldData.showAllChildren = "showAllChildren";
        _ReferenceFieldData.showWorkspaceSelection = "showWorkspaceSelection";
        _ReferenceFieldData.showExtentSelection = "showExtentSelection";
        _ReferenceFieldData.metaClassFilter = "metaClassFilter";
        _ReferenceFieldData.isAttached = "isAttached";
        _ReferenceFieldData._name_ = "name";
        _ReferenceFieldData.title = "title";
        _ReferenceFieldData.isEnumeration = "isEnumeration";
        _ReferenceFieldData.defaultValue = "defaultValue";
        _ReferenceFieldData.isReadOnly = "isReadOnly";
        _Forms._ReferenceFieldData = _ReferenceFieldData;
        _Forms.__ReferenceFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData";
        class _SubElementFieldData {
        }
        _SubElementFieldData.metaClass = "metaClass";
        _SubElementFieldData.form = "form";
        _SubElementFieldData.allowOnlyExistingElements = "allowOnlyExistingElements";
        _SubElementFieldData.defaultTypesForNewElements = "defaultTypesForNewElements";
        _SubElementFieldData.includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";
        _SubElementFieldData.defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";
        _SubElementFieldData.defaultExtentOfNewElements = "defaultExtentOfNewElements";
        _SubElementFieldData.actionName = "actionName";
        _SubElementFieldData.isAttached = "isAttached";
        _SubElementFieldData._name_ = "name";
        _SubElementFieldData.title = "title";
        _SubElementFieldData.isEnumeration = "isEnumeration";
        _SubElementFieldData.defaultValue = "defaultValue";
        _SubElementFieldData.isReadOnly = "isReadOnly";
        _Forms._SubElementFieldData = _SubElementFieldData;
        _Forms.__SubElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData";
        class _TextFieldData {
        }
        _TextFieldData.lineHeight = "lineHeight";
        _TextFieldData.width = "width";
        _TextFieldData.shortenTextLength = "shortenTextLength";
        _TextFieldData.supportClipboardCopy = "supportClipboardCopy";
        _TextFieldData.isAttached = "isAttached";
        _TextFieldData._name_ = "name";
        _TextFieldData.title = "title";
        _TextFieldData.isEnumeration = "isEnumeration";
        _TextFieldData.defaultValue = "defaultValue";
        _TextFieldData.isReadOnly = "isReadOnly";
        _Forms._TextFieldData = _TextFieldData;
        _Forms.__TextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData";
        class _EvalTextFieldData {
        }
        _EvalTextFieldData.evalCellProperties = "evalCellProperties";
        _EvalTextFieldData.lineHeight = "lineHeight";
        _EvalTextFieldData.width = "width";
        _EvalTextFieldData.shortenTextLength = "shortenTextLength";
        _EvalTextFieldData.supportClipboardCopy = "supportClipboardCopy";
        _EvalTextFieldData.isAttached = "isAttached";
        _EvalTextFieldData._name_ = "name";
        _EvalTextFieldData.title = "title";
        _EvalTextFieldData.isEnumeration = "isEnumeration";
        _EvalTextFieldData.defaultValue = "defaultValue";
        _EvalTextFieldData.isReadOnly = "isReadOnly";
        _Forms._EvalTextFieldData = _EvalTextFieldData;
        _Forms.__EvalTextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData";
        class _SeparatorLineFieldData {
        }
        _SeparatorLineFieldData.Height = "Height";
        _Forms._SeparatorLineFieldData = _SeparatorLineFieldData;
        _Forms.__SeparatorLineFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData";
        class _FileSelectionFieldData {
        }
        _FileSelectionFieldData.defaultExtension = "defaultExtension";
        _FileSelectionFieldData.isSaving = "isSaving";
        _FileSelectionFieldData.initialPathToDirectory = "initialPathToDirectory";
        _FileSelectionFieldData.filter = "filter";
        _FileSelectionFieldData.isAttached = "isAttached";
        _FileSelectionFieldData._name_ = "name";
        _FileSelectionFieldData.title = "title";
        _FileSelectionFieldData.isEnumeration = "isEnumeration";
        _FileSelectionFieldData.defaultValue = "defaultValue";
        _FileSelectionFieldData.isReadOnly = "isReadOnly";
        _Forms._FileSelectionFieldData = _FileSelectionFieldData;
        _Forms.__FileSelectionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData";
        class _DefaultTypeForNewElement {
        }
        _DefaultTypeForNewElement._name_ = "name";
        _DefaultTypeForNewElement.metaClass = "metaClass";
        _DefaultTypeForNewElement.parentProperty = "parentProperty";
        _Forms._DefaultTypeForNewElement = _DefaultTypeForNewElement;
        _Forms.__DefaultTypeForNewElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement";
        class _FullNameFieldData {
        }
        _FullNameFieldData.isAttached = "isAttached";
        _FullNameFieldData._name_ = "name";
        _FullNameFieldData.title = "title";
        _FullNameFieldData.isEnumeration = "isEnumeration";
        _FullNameFieldData.defaultValue = "defaultValue";
        _FullNameFieldData.isReadOnly = "isReadOnly";
        _Forms._FullNameFieldData = _FullNameFieldData;
        _Forms.__FullNameFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData";
        class _CheckboxListTaggingFieldData {
        }
        _CheckboxListTaggingFieldData.values = "values";
        _CheckboxListTaggingFieldData.separator = "separator";
        _CheckboxListTaggingFieldData.containsFreeText = "containsFreeText";
        _CheckboxListTaggingFieldData.isAttached = "isAttached";
        _CheckboxListTaggingFieldData._name_ = "name";
        _CheckboxListTaggingFieldData.title = "title";
        _CheckboxListTaggingFieldData.isEnumeration = "isEnumeration";
        _CheckboxListTaggingFieldData.defaultValue = "defaultValue";
        _CheckboxListTaggingFieldData.isReadOnly = "isReadOnly";
        _Forms._CheckboxListTaggingFieldData = _CheckboxListTaggingFieldData;
        _Forms.__CheckboxListTaggingFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData";
        class _NumberFieldData {
        }
        _NumberFieldData.format = "format";
        _NumberFieldData.isInteger = "isInteger";
        _NumberFieldData.isAttached = "isAttached";
        _NumberFieldData._name_ = "name";
        _NumberFieldData.title = "title";
        _NumberFieldData.isEnumeration = "isEnumeration";
        _NumberFieldData.defaultValue = "defaultValue";
        _NumberFieldData.isReadOnly = "isReadOnly";
        _Forms._NumberFieldData = _NumberFieldData;
        _Forms.__NumberFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData";
        let _FormType;
        (function (_FormType) {
            _FormType.Object = "Object";
            _FormType.Collection = "Collection";
            _FormType.Row = "Row";
            _FormType.Table = "Table";
            _FormType.ObjectExtension = "ObjectExtension";
            _FormType.CollectionExtension = "CollectionExtension";
            _FormType.RowExtension = "RowExtension";
            _FormType.TableExtension = "TableExtension";
        })(_FormType = _Forms._FormType || (_Forms._FormType = {}));
        let ___FormType;
        (function (___FormType) {
            ___FormType[___FormType["Object"] = 0] = "Object";
            ___FormType[___FormType["Collection"] = 1] = "Collection";
            ___FormType[___FormType["Row"] = 2] = "Row";
            ___FormType[___FormType["Table"] = 3] = "Table";
            ___FormType[___FormType["ObjectExtension"] = 4] = "ObjectExtension";
            ___FormType[___FormType["CollectionExtension"] = 5] = "CollectionExtension";
            ___FormType[___FormType["RowExtension"] = 6] = "RowExtension";
            ___FormType[___FormType["TableExtension"] = 7] = "TableExtension";
        })(___FormType = _Forms.___FormType || (_Forms.___FormType = {}));
        class _Form {
        }
        _Form._name_ = "name";
        _Form.title = "title";
        _Form.isReadOnly = "isReadOnly";
        _Form.isAutoGenerated = "isAutoGenerated";
        _Form.hideMetaInformation = "hideMetaInformation";
        _Form.originalUri = "originalUri";
        _Form.creationProtocol = "creationProtocol";
        _Forms._Form = _Form;
        _Forms.__Form_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form";
        class _RowForm {
        }
        _RowForm.buttonApplyText = "buttonApplyText";
        _RowForm.allowNewProperties = "allowNewProperties";
        _RowForm.defaultWidth = "defaultWidth";
        _RowForm.defaultHeight = "defaultHeight";
        _RowForm.field = "field";
        _RowForm._name_ = "name";
        _RowForm.title = "title";
        _RowForm.isReadOnly = "isReadOnly";
        _RowForm.isAutoGenerated = "isAutoGenerated";
        _RowForm.hideMetaInformation = "hideMetaInformation";
        _RowForm.originalUri = "originalUri";
        _RowForm.creationProtocol = "creationProtocol";
        _Forms._RowForm = _RowForm;
        _Forms.__RowForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm";
        class _TableForm {
        }
        _TableForm.property = "property";
        _TableForm.metaClass = "metaClass";
        _TableForm.includeDescendents = "includeDescendents";
        _TableForm.noItemsWithMetaClass = "noItemsWithMetaClass";
        _TableForm.inhibitNewItems = "inhibitNewItems";
        _TableForm.inhibitDeleteItems = "inhibitDeleteItems";
        _TableForm.inhibitEditItems = "inhibitEditItems";
        _TableForm.defaultTypesForNewElements = "defaultTypesForNewElements";
        _TableForm.fastViewFilters = "fastViewFilters";
        _TableForm.field = "field";
        _TableForm.sortingOrder = "sortingOrder";
        _TableForm.viewNode = "viewNode";
        _TableForm.autoGenerateFields = "autoGenerateFields";
        _TableForm.duplicatePerType = "duplicatePerType";
        _TableForm.dataUrl = "dataUrl";
        _TableForm.inhibitNewUnclassifiedItems = "inhibitNewUnclassifiedItems";
        _TableForm._name_ = "name";
        _TableForm.title = "title";
        _TableForm.isReadOnly = "isReadOnly";
        _TableForm.isAutoGenerated = "isAutoGenerated";
        _TableForm.hideMetaInformation = "hideMetaInformation";
        _TableForm.originalUri = "originalUri";
        _TableForm.creationProtocol = "creationProtocol";
        _Forms._TableForm = _TableForm;
        _Forms.__TableForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm";
        class _CollectionForm {
        }
        _CollectionForm.tab = "tab";
        _CollectionForm.autoTabs = "autoTabs";
        _CollectionForm.field = "field";
        _CollectionForm._name_ = "name";
        _CollectionForm.title = "title";
        _CollectionForm.isReadOnly = "isReadOnly";
        _CollectionForm.isAutoGenerated = "isAutoGenerated";
        _CollectionForm.hideMetaInformation = "hideMetaInformation";
        _CollectionForm.originalUri = "originalUri";
        _CollectionForm.creationProtocol = "creationProtocol";
        _Forms._CollectionForm = _CollectionForm;
        _Forms.__CollectionForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm";
        class _ObjectForm {
        }
        _ObjectForm.tab = "tab";
        _ObjectForm.autoTabs = "autoTabs";
        _ObjectForm._name_ = "name";
        _ObjectForm.title = "title";
        _ObjectForm.isReadOnly = "isReadOnly";
        _ObjectForm.isAutoGenerated = "isAutoGenerated";
        _ObjectForm.hideMetaInformation = "hideMetaInformation";
        _ObjectForm.originalUri = "originalUri";
        _ObjectForm.creationProtocol = "creationProtocol";
        _Forms._ObjectForm = _ObjectForm;
        _Forms.__ObjectForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm";
        class _ViewMode {
        }
        _ViewMode._name_ = "name";
        _ViewMode.id = "id";
        _ViewMode.defaultExtentType = "defaultExtentType";
        _Forms._ViewMode = _ViewMode;
        _Forms.__ViewMode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode";
        class _ReferenceFieldFromCollectionData {
        }
        _ReferenceFieldFromCollectionData.defaultWorkspace = "defaultWorkspace";
        _ReferenceFieldFromCollectionData.collection = "collection";
        _ReferenceFieldFromCollectionData.isAttached = "isAttached";
        _ReferenceFieldFromCollectionData._name_ = "name";
        _ReferenceFieldFromCollectionData.title = "title";
        _ReferenceFieldFromCollectionData.isEnumeration = "isEnumeration";
        _ReferenceFieldFromCollectionData.defaultValue = "defaultValue";
        _ReferenceFieldFromCollectionData.isReadOnly = "isReadOnly";
        _Forms._ReferenceFieldFromCollectionData = _ReferenceFieldFromCollectionData;
        _Forms.__ReferenceFieldFromCollectionData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldFromCollectionData";
        class _UriReferenceFieldData {
        }
        _UriReferenceFieldData.defaultWorkspace = "defaultWorkspace";
        _UriReferenceFieldData.defaultExtent = "defaultExtent";
        _Forms._UriReferenceFieldData = _UriReferenceFieldData;
        _Forms.__UriReferenceFieldData_Uri = "dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00";
        class _NavigateToFieldsForTestAction {
        }
        _NavigateToFieldsForTestAction._name_ = "name";
        _NavigateToFieldsForTestAction.isDisabled = "isDisabled";
        _Forms._NavigateToFieldsForTestAction = _NavigateToFieldsForTestAction;
        _Forms.__NavigateToFieldsForTestAction_Uri = "dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0";
    })(_Forms = _DatenMeister._Forms || (_DatenMeister._Forms = {}));
    let _AttachedExtent;
    (function (_AttachedExtent) {
        class _AttachedExtentConfiguration {
        }
        _AttachedExtentConfiguration._name_ = "name";
        _AttachedExtentConfiguration.referencedWorkspace = "referencedWorkspace";
        _AttachedExtentConfiguration.referencedExtent = "referencedExtent";
        _AttachedExtentConfiguration.referenceType = "referenceType";
        _AttachedExtentConfiguration.referenceProperty = "referenceProperty";
        _AttachedExtent._AttachedExtentConfiguration = _AttachedExtentConfiguration;
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
        class _Extent {
        }
        _Extent._name_ = "name";
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
        _Management._Extent = _Extent;
        _Management.__Extent_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent";
        class _Workspace {
        }
        _Workspace.id = "id";
        _Workspace.annotation = "annotation";
        _Workspace.extents = "extents";
        _Management._Workspace = _Workspace;
        _Management.__Workspace_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace";
        class _CreateNewWorkspaceModel {
        }
        _CreateNewWorkspaceModel.id = "id";
        _CreateNewWorkspaceModel.annotation = "annotation";
        _Management._CreateNewWorkspaceModel = _CreateNewWorkspaceModel;
        _Management.__CreateNewWorkspaceModel_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel";
        class _ExtentTypeSetting {
        }
        _ExtentTypeSetting._name_ = "name";
        _ExtentTypeSetting.rootElementMetaClasses = "rootElementMetaClasses";
        _Management._ExtentTypeSetting = _ExtentTypeSetting;
        _Management.__ExtentTypeSetting_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting";
        class _ExtentProperties {
        }
        _ExtentProperties._name_ = "name";
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
        _Management._ExtentProperties = _ExtentProperties;
        _Management.__ExtentProperties_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties";
        class _ExtentPropertyDefinition {
        }
        _ExtentPropertyDefinition._name_ = "name";
        _ExtentPropertyDefinition.title = "title";
        _ExtentPropertyDefinition.metaClass = "metaClass";
        _Management._ExtentPropertyDefinition = _ExtentPropertyDefinition;
        _Management.__ExtentPropertyDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition";
        class _ExtentSettings {
        }
        _ExtentSettings.extentTypeSettings = "extentTypeSettings";
        _ExtentSettings.propertyDefinitions = "propertyDefinitions";
        _Management._ExtentSettings = _ExtentSettings;
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
        class _PropertyComparisonFilter {
        }
        _PropertyComparisonFilter.Property = "Property";
        _PropertyComparisonFilter.ComparisonType = "ComparisonType";
        _PropertyComparisonFilter.Value = "Value";
        _FastViewFilters._PropertyComparisonFilter = _PropertyComparisonFilter;
        _FastViewFilters.__PropertyComparisonFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter";
        class _PropertyContainsFilter {
        }
        _PropertyContainsFilter.Property = "Property";
        _PropertyContainsFilter.Value = "Value";
        _FastViewFilters._PropertyContainsFilter = _PropertyContainsFilter;
        _FastViewFilters.__PropertyContainsFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter";
    })(_FastViewFilters = _DatenMeister._FastViewFilters || (_DatenMeister._FastViewFilters = {}));
    let _DynamicRuntimeProvider;
    (function (_DynamicRuntimeProvider) {
        class _DynamicRuntimeLoaderConfig {
        }
        _DynamicRuntimeLoaderConfig.runtimeClass = "runtimeClass";
        _DynamicRuntimeLoaderConfig.configuration = "configuration";
        _DynamicRuntimeLoaderConfig._name_ = "name";
        _DynamicRuntimeLoaderConfig.extentUri = "extentUri";
        _DynamicRuntimeLoaderConfig.workspaceId = "workspaceId";
        _DynamicRuntimeLoaderConfig.dropExisting = "dropExisting";
        _DynamicRuntimeProvider._DynamicRuntimeLoaderConfig = _DynamicRuntimeLoaderConfig;
        _DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig_Uri = "dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig";
        let _Examples;
        (function (_Examples) {
            class _NumberProviderSettings {
            }
            _NumberProviderSettings._name_ = "name";
            _NumberProviderSettings.start = "start";
            _NumberProviderSettings.end = "end";
            _Examples._NumberProviderSettings = _NumberProviderSettings;
            _Examples.__NumberProviderSettings_Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a";
            class _NumberRepresentation {
            }
            _NumberRepresentation.binary = "binary";
            _NumberRepresentation.octal = "octal";
            _NumberRepresentation.decimal = "decimal";
            _NumberRepresentation.hexadecimal = "hexadecimal";
            _Examples._NumberRepresentation = _NumberRepresentation;
            _Examples.__NumberRepresentation_Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation";
        })(_Examples = _DynamicRuntimeProvider._Examples || (_DynamicRuntimeProvider._Examples = {}));
    })(_DynamicRuntimeProvider = _DatenMeister._DynamicRuntimeProvider || (_DatenMeister._DynamicRuntimeProvider = {}));
    let _Verifier;
    (function (_Verifier) {
        class _VerifyEntry {
        }
        _VerifyEntry.workspaceId = "workspaceId";
        _VerifyEntry.itemUri = "itemUri";
        _VerifyEntry.category = "category";
        _VerifyEntry.message = "message";
        _Verifier._VerifyEntry = _VerifyEntry;
        _Verifier.__VerifyEntry_Uri = "dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768";
    })(_Verifier = _DatenMeister._Verifier || (_DatenMeister._Verifier = {}));
})(_DatenMeister || (_DatenMeister = {}));
//# sourceMappingURL=DatenMeister.class.js.map