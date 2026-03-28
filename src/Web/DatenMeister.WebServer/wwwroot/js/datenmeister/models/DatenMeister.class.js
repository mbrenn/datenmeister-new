// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.3.0.0
export var _CommonTypes;
(function (_CommonTypes) {
    class _DateTime {
    }
    _CommonTypes._DateTime = _DateTime;
    _CommonTypes.__DateTime_Uri = "dm:///_internal/types/internal#DateTime";
    let _Default;
    (function (_Default) {
        class _Package {
            static _name_ = "name";
            static packagedElement = "packagedElement";
            static preferredType = "preferredType";
            static preferredPackage = "preferredPackage";
            static defaultViewMode = "defaultViewMode";
        }
        _Default._Package = _Package;
        _Default.__Package_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package";
        class _XmiExportContainer {
            static xmi = "xmi";
        }
        _Default._XmiExportContainer = _XmiExportContainer;
        _Default.__XmiExportContainer_Uri = "dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1";
        class _XmiImportContainer {
            static xmi = "xmi";
            static property = "property";
            static addToCollection = "addToCollection";
        }
        _Default._XmiImportContainer = _XmiImportContainer;
        _Default.__XmiImportContainer_Uri = "dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a";
    })(_Default = _CommonTypes._Default || (_CommonTypes._Default = {}));
    let _ExtentManager;
    (function (_ExtentManager) {
        class _ImportSettings {
            static filePath = "filePath";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
        }
        _ExtentManager._ImportSettings = _ImportSettings;
        _ExtentManager.__ImportSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings";
        class _ImportException {
            static message = "message";
        }
        _ExtentManager._ImportException = _ImportException;
        _ExtentManager.__ImportException_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException";
    })(_ExtentManager = _CommonTypes._ExtentManager || (_CommonTypes._ExtentManager = {}));
    let _OSIntegration;
    (function (_OSIntegration) {
        class _CommandLineApplication {
            static _name_ = "name";
            static applicationPath = "applicationPath";
        }
        _OSIntegration._CommandLineApplication = _CommandLineApplication;
        _OSIntegration.__CommandLineApplication_Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication";
        class _EnvironmentalVariable {
            static _name_ = "name";
            static value = "value";
        }
        _OSIntegration._EnvironmentalVariable = _EnvironmentalVariable;
        _OSIntegration.__EnvironmentalVariable_Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable";
    })(_OSIntegration = _CommonTypes._OSIntegration || (_CommonTypes._OSIntegration = {}));
})(_CommonTypes || (_CommonTypes = {}));
export var _Actions;
(function (_Actions) {
    class _ActionSet {
        static action = "action";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._ActionSet = _ActionSet;
    _Actions.__ActionSet_Uri = "dm:///_internal/types/internal#Actions.ActionSet";
    class _LoggingWriterAction {
        static message = "message";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._LoggingWriterAction = _LoggingWriterAction;
    _Actions.__LoggingWriterAction_Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction";
    class _LoadExtentAction {
        static configuration = "configuration";
        static dropExisting = "dropExisting";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._LoadExtentAction = _LoadExtentAction;
    _Actions.__LoadExtentAction_Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee";
    class _DropExtentAction {
        static workspaceId = "workspaceId";
        static extentUri = "extentUri";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._DropExtentAction = _DropExtentAction;
    _Actions.__DropExtentAction_Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09";
    class _CreateWorkspaceAction {
        static workspaceId = "workspaceId";
        static annotation = "annotation";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._CreateWorkspaceAction = _CreateWorkspaceAction;
    _Actions.__CreateWorkspaceAction_Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe";
    class _DropWorkspaceAction {
        static workspaceId = "workspaceId";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._DropWorkspaceAction = _DropWorkspaceAction;
    _Actions.__DropWorkspaceAction_Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8";
    class _CopyElementsAction {
        static sourcePath = "sourcePath";
        static targetPath = "targetPath";
        static moveOnly = "moveOnly";
        static sourceWorkspace = "sourceWorkspace";
        static targetWorkspace = "targetWorkspace";
        static emptyTarget = "emptyTarget";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._CopyElementsAction = _CopyElementsAction;
    _Actions.__CopyElementsAction_Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed";
    class _ExportToXmiAction {
        static sourcePath = "sourcePath";
        static filePath = "filePath";
        static sourceWorkspaceId = "sourceWorkspaceId";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._ExportToXmiAction = _ExportToXmiAction;
    _Actions.__ExportToXmiAction_Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863";
    class _ClearCollectionAction {
        static workspaceId = "workspaceId";
        static path = "path";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._ClearCollectionAction = _ClearCollectionAction;
    _Actions.__ClearCollectionAction_Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae";
    class _TransformItemsAction {
        static metaClass = "metaClass";
        static runtimeClass = "runtimeClass";
        static workspaceId = "workspaceId";
        static path = "path";
        static excludeDescendents = "excludeDescendents";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._TransformItemsAction = _TransformItemsAction;
    _Actions.__TransformItemsAction_Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler";
    class _EchoAction {
        static shallSuccess = "shallSuccess";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._EchoAction = _EchoAction;
    _Actions.__EchoAction_Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction";
    class _DocumentOpenAction {
        static filePath = "filePath";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._DocumentOpenAction = _DocumentOpenAction;
    _Actions.__DocumentOpenAction_Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74";
    let _Reports;
    (function (_Reports) {
        class _SimpleReportAction {
            static path = "path";
            static configuration = "configuration";
            static workspaceId = "workspaceId";
            static filePath = "filePath";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _Reports._SimpleReportAction = _SimpleReportAction;
        _Reports.__SimpleReportAction_Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction";
        class _AdocReportAction {
            static filePath = "filePath";
            static reportInstance = "reportInstance";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _Reports._AdocReportAction = _AdocReportAction;
        _Reports.__AdocReportAction_Uri = "dm:///_internal/types/internal#Actions.AdocReportAction";
        class _HtmlReportAction {
            static filePath = "filePath";
            static reportInstance = "reportInstance";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _Reports._HtmlReportAction = _HtmlReportAction;
        _Reports.__HtmlReportAction_Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction";
    })(_Reports = _Actions._Reports || (_Actions._Reports = {}));
    class _Action {
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._Action = _Action;
    _Actions.__Action_Uri = "dm:///_internal/types/internal#Actions.Action";
    class _MoveOrCopyAction {
        static copyMode = "copyMode";
        static target = "target";
        static source = "source";
    }
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
        static element = "element";
        static direction = "direction";
        static container = "container";
        static property = "property";
    }
    _Actions._MoveUpDownAction = _MoveUpDownAction;
    _Actions.__MoveUpDownAction_Uri = "dm:///_internal/types/internal#bc4952bf-a3f5-4516-be26-5b773e38bd54";
    class _StoreExtentAction {
        static workspaceId = "workspaceId";
        static extentUri = "extentUri";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._StoreExtentAction = _StoreExtentAction;
    _Actions.__StoreExtentAction_Uri = "dm:///_internal/types/internal#43b0764e-b70f-42bb-b37d-ae8586ec45f1";
    class _ImportXmiAction {
        static workspaceId = "workspaceId";
        static itemUri = "itemUri";
        static xmi = "xmi";
        static property = "property";
        static addToCollection = "addToCollection";
    }
    _Actions._ImportXmiAction = _ImportXmiAction;
    _Actions.__ImportXmiAction_Uri = "dm:///_internal/types/internal#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d";
    class _DeletePropertyFromCollectionAction {
        static propertyName = "propertyName";
        static metaclass = "metaclass";
        static collectionUrl = "collectionUrl";
    }
    _Actions._DeletePropertyFromCollectionAction = _DeletePropertyFromCollectionAction;
    _Actions.__DeletePropertyFromCollectionAction_Uri = "dm:///_internal/types/internal#b631bb00-ab11-4a8a-a148-e28abc398503";
    class _TargetReferenceResult {
        static targetUrl = "targetUrl";
        static targetWorkspace = "targetWorkspace";
    }
    _Actions._TargetReferenceResult = _TargetReferenceResult;
    _Actions.__TargetReferenceResult_Uri = "dm:///_internal/types/internal#3223e13a-bbb7-4785-8b81-7275be23b0a1";
    let _ParameterTypes;
    (function (_ParameterTypes) {
        class _NavigationDefineActionParameter {
            static actionName = "actionName";
            static formUrl = "formUrl";
            static metaClassUrl = "metaClassUrl";
        }
        _ParameterTypes._NavigationDefineActionParameter = _NavigationDefineActionParameter;
        _ParameterTypes.__NavigationDefineActionParameter_Uri = "dm:///_internal/types/internal#90f61e4e-a5ea-42eb-9caa-912d010fbccd";
        class _LoadExtentActionResult {
            static workspaceId = "workspaceId";
            static extentUri = "extentUri";
        }
        _ParameterTypes._LoadExtentActionResult = _LoadExtentActionResult;
        _ParameterTypes.__LoadExtentActionResult_Uri = "dm:///_internal/types/internal#2863f928-fe69-4d35-8c67-f4f3533b7ae5";
        class _CreateFormUponViewResult {
            static resultingPackageUrl = "resultingPackageUrl";
        }
        _ParameterTypes._CreateFormUponViewResult = _CreateFormUponViewResult;
        _ParameterTypes.__CreateFormUponViewResult_Uri = "dm:///_internal/types/internal#124e202d-e8b3-4d39-bbc2-4c95896e811b";
    })(_ParameterTypes = _Actions._ParameterTypes || (_Actions._ParameterTypes = {}));
    class _ActionResult {
        static isSuccess = "isSuccess";
        static clientActions = "clientActions";
    }
    _Actions._ActionResult = _ActionResult;
    _Actions.__ActionResult_Uri = "dm:///_internal/types/internal#899324b1-85dc-40a1-ba95-dec50509040d";
    let _ClientActions;
    (function (_ClientActions) {
        class _ClientAction {
            static actionName = "actionName";
            static element = "element";
            static parameter = "parameter";
        }
        _ClientActions._ClientAction = _ClientAction;
        _ClientActions.__ClientAction_Uri = "dm:///_internal/types/internal#e07ca80e-2540-4f91-8214-60dbd464e998";
        class _AlertClientAction {
            static messageText = "messageText";
            static actionName = "actionName";
            static element = "element";
            static parameter = "parameter";
        }
        _ClientActions._AlertClientAction = _AlertClientAction;
        _ClientActions.__AlertClientAction_Uri = "dm:///_internal/types/internal#0ee17f2a-5407-4d38-b1b4-34ead2186971";
        class _NavigateToExtentClientAction {
            static workspaceId = "workspaceId";
            static extentUri = "extentUri";
        }
        _ClientActions._NavigateToExtentClientAction = _NavigateToExtentClientAction;
        _ClientActions.__NavigateToExtentClientAction_Uri = "dm:///_internal/types/internal#3251783f-2683-4c24-bad5-828930028462";
        class _NavigateToItemClientAction {
            static workspaceId = "workspaceId";
            static itemUrl = "itemUrl";
            static formUri = "formUri";
        }
        _ClientActions._NavigateToItemClientAction = _NavigateToItemClientAction;
        _ClientActions.__NavigateToItemClientAction_Uri = "dm:///_internal/types/internal#5f69675e-df58-4ad7-84bf-359cdfba5db4";
    })(_ClientActions = _Actions._ClientActions || (_Actions._ClientActions = {}));
    let _Forms;
    (function (_Forms) {
        class _AddQueryInPackageAction {
            static query = "query";
            static targetPackageUri = "targetPackageUri";
            static targetPackageWorkspace = "targetPackageWorkspace";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _Forms._AddQueryInPackageAction = _AddQueryInPackageAction;
        _Forms.__AddQueryInPackageAction_Uri = "dm:///_internal/types/internal#b8333b8d-ac49-4a4e-a7f4-c3745e0a0237";
        class _CreateFormByMetaClass {
            static metaClass = "metaClass";
            static creationMode = "creationMode";
            static targetContainer = "targetContainer";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _Forms._CreateFormByMetaClass = _CreateFormByMetaClass;
        _Forms.__CreateFormByMetaClass_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.CreateFormByMetaclass";
    })(_Forms = _Actions._Forms || (_Actions._Forms = {}));
    let _OSIntegration;
    (function (_OSIntegration) {
        class _CommandExecutionAction {
            static command = "command";
            static _arguments_ = "arguments";
            static workingDirectory = "workingDirectory";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _OSIntegration._CommandExecutionAction = _CommandExecutionAction;
        _OSIntegration.__CommandExecutionAction_Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82";
        class _PowershellExecutionAction {
            static script = "script";
            static workingDirectory = "workingDirectory";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _OSIntegration._PowershellExecutionAction = _PowershellExecutionAction;
        _OSIntegration.__PowershellExecutionAction_Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb";
        class _ConsoleWriteAction {
            static text = "text";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }
        _OSIntegration._ConsoleWriteAction = _ConsoleWriteAction;
        _OSIntegration.__ConsoleWriteAction_Uri = "dm:///_internal/types/internal#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a";
    })(_OSIntegration = _Actions._OSIntegration || (_Actions._OSIntegration = {}));
    class _RefreshTypeIndexAction {
        static waitForRefresh = "waitForRefresh";
    }
    _Actions._RefreshTypeIndexAction = _RefreshTypeIndexAction;
    _Actions.__RefreshTypeIndexAction_Uri = "dm:///_internal/types/internal#9d43decb-aa2f-4461-b680-3ec595b518d1";
    class _StoreElementAction {
        static workspace = "workspace";
        static url = "url";
        static element = "element";
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Actions._StoreElementAction = _StoreElementAction;
    _Actions.__StoreElementAction_Uri = "dm:///_internal/types/internal#c7dcb24c-e53c-46f9-9e8a-3704095193a8";
})(_Actions || (_Actions = {}));
export var _DataViews;
(function (_DataViews) {
    class _DataView {
        static _name_ = "name";
        static workspaceId = "workspaceId";
        static uri = "uri";
        static viewNode = "viewNode";
    }
    _DataViews._DataView = _DataView;
    _DataViews.__DataView_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView";
    class _ViewNode {
        static _name_ = "name";
    }
    _DataViews._ViewNode = _ViewNode;
    _DataViews.__ViewNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode";
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
        _ComparisonMode.RegexMatch = "RegexMatch";
        _ComparisonMode.RegexNoMatch = "RegexNoMatch";
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
        ___ComparisonMode[___ComparisonMode["RegexMatch"] = 8] = "RegexMatch";
        ___ComparisonMode[___ComparisonMode["RegexNoMatch"] = 9] = "RegexNoMatch";
    })(___ComparisonMode = _DataViews.___ComparisonMode || (_DataViews.___ComparisonMode = {}));
    class _QueryStatement {
        static nodes = "nodes";
        static resultNode = "resultNode";
        static _name_ = "name";
    }
    _DataViews._QueryStatement = _QueryStatement;
    _DataViews.__QueryStatement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement";
    let _Row;
    (function (_Row) {
        class _RowFilterByFreeTextAnywhere {
            static freeText = "freeText";
            static input = "input";
            static _name_ = "name";
        }
        _Row._RowFilterByFreeTextAnywhere = _RowFilterByFreeTextAnywhere;
        _Row.__RowFilterByFreeTextAnywhere_Uri = "dm:///_internal/types/internal#5f66ff9a-0a68-4c87-856b-5921c7cae628";
        class _RowFilterByPropertyValueNode {
            static input = "input";
            static property = "property";
            static value = "value";
            static comparisonMode = "comparisonMode";
            static _name_ = "name";
        }
        _Row._RowFilterByPropertyValueNode = _RowFilterByPropertyValueNode;
        _Row.__RowFilterByPropertyValueNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode";
        class _RowOrderByNode {
            static input = "input";
            static propertyName = "propertyName";
            static orderDescending = "orderDescending";
            static _name_ = "name";
        }
        _Row._RowOrderByNode = _RowOrderByNode;
        _Row.__RowOrderByNode_Uri = "dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9";
        class _RowFilterOnPositionNode {
            static input = "input";
            static amount = "amount";
            static position = "position";
            static _name_ = "name";
        }
        _Row._RowFilterOnPositionNode = _RowFilterOnPositionNode;
        _Row.__RowFilterOnPositionNode_Uri = "dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f";
        class _RowFlattenNode {
            static input = "input";
            static _name_ = "name";
        }
        _Row._RowFlattenNode = _RowFlattenNode;
        _Row.__RowFlattenNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode";
        class _RowFilterByMetaclassNode {
            static input = "input";
            static metaClass = "metaClass";
            static includeInherits = "includeInherits";
            static _name_ = "name";
        }
        _Row._RowFilterByMetaclassNode = _RowFilterByMetaclassNode;
        _Row.__RowFilterByMetaclassNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode";
    })(_Row = _DataViews._Row || (_DataViews._Row = {}));
    let _Column;
    (function (_Column) {
        class _ColumnFilterIncludeOnlyNode {
            static columnNamesComma = "columnNamesComma";
            static input = "input";
            static _name_ = "name";
        }
        _Column._ColumnFilterIncludeOnlyNode = _ColumnFilterIncludeOnlyNode;
        _Column.__ColumnFilterIncludeOnlyNode_Uri = "dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d";
        class _ColumnFilterExcludeNode {
            static columnNamesComma = "columnNamesComma";
            static input = "input";
            static _name_ = "name";
        }
        _Column._ColumnFilterExcludeNode = _ColumnFilterExcludeNode;
        _Column.__ColumnFilterExcludeNode_Uri = "dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7";
    })(_Column = _DataViews._Column || (_DataViews._Column = {}));
    let _Source;
    (function (_Source) {
        class _SelectByExtentNode {
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static _name_ = "name";
        }
        _Source._SelectByExtentNode = _SelectByExtentNode;
        _Source.__SelectByExtentNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode";
        class _SelectByPathNode {
            static workspaceId = "workspaceId";
            static path = "path";
            static _name_ = "name";
        }
        _Source._SelectByPathNode = _SelectByPathNode;
        _Source.__SelectByPathNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode";
        class _DynamicSourceNode {
            static nodeName = "nodeName";
            static _name_ = "name";
        }
        _Source._DynamicSourceNode = _DynamicSourceNode;
        _Source.__DynamicSourceNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode";
        class _SelectByWorkspaceNode {
            static workspaceId = "workspaceId";
            static _name_ = "name";
        }
        _Source._SelectByWorkspaceNode = _SelectByWorkspaceNode;
        _Source.__SelectByWorkspaceNode_Uri = "dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db";
        class _SelectByFullNameNode {
            static input = "input";
            static path = "path";
            static _name_ = "name";
        }
        _Source._SelectByFullNameNode = _SelectByFullNameNode;
        _Source.__SelectByFullNameNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode";
        class _SelectFromAllWorkspacesNode {
            static _name_ = "name";
        }
        _Source._SelectFromAllWorkspacesNode = _SelectFromAllWorkspacesNode;
        _Source.__SelectFromAllWorkspacesNode_Uri = "dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a";
    })(_Source = _DataViews._Source || (_DataViews._Source = {}));
    let _Node;
    (function (_Node) {
        class _ReferenceViewNode {
            static workspaceId = "workspaceId";
            static itemUri = "itemUri";
            static _name_ = "name";
        }
        _Node._ReferenceViewNode = _ReferenceViewNode;
        _Node.__ReferenceViewNode_Uri = "dm:///_internal/types/internal#e80d4c64-a68e-44a7-893d-1a5100a80370";
    })(_Node = _DataViews._Node || (_DataViews._Node = {}));
    class _ValueItem {
        static value = "value";
    }
    _DataViews._ValueItem = _ValueItem;
    _DataViews.__ValueItem_Uri = "dm:///_internal/types/internal#4394a28a-0def-4030-b5d0-7a1b5b01c91b";
})(_DataViews || (_DataViews = {}));
export var _Reports;
(function (_Reports) {
    class _ReportDefinition {
        static _name_ = "name";
        static title = "title";
        static elements = "elements";
    }
    _Reports._ReportDefinition = _ReportDefinition;
    _Reports.__ReportDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition";
    class _ReportInstanceSource {
        static _name_ = "name";
        static workspaceId = "workspaceId";
        static path = "path";
    }
    _Reports._ReportInstanceSource = _ReportInstanceSource;
    _Reports.__ReportInstanceSource_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource";
    class _ReportInstance {
        static _name_ = "name";
        static reportDefinition = "reportDefinition";
        static sources = "sources";
    }
    _Reports._ReportInstance = _ReportInstance;
    _Reports.__ReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance";
    class _AdocReportInstance {
        static _name_ = "name";
        static reportDefinition = "reportDefinition";
        static sources = "sources";
    }
    _Reports._AdocReportInstance = _AdocReportInstance;
    _Reports.__AdocReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance";
    class _HtmlReportInstance {
        static cssFile = "cssFile";
        static cssStyleSheet = "cssStyleSheet";
        static _name_ = "name";
        static reportDefinition = "reportDefinition";
        static sources = "sources";
    }
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
        static _name_ = "name";
        static showDescendents = "showDescendents";
        static rootElement = "rootElement";
        static showRootElement = "showRootElement";
        static showMetaClasses = "showMetaClasses";
        static showFullName = "showFullName";
        static form = "form";
        static descendentMode = "descendentMode";
        static typeMode = "typeMode";
        static workspaceId = "workspaceId";
    }
    _Reports._SimpleReportConfiguration = _SimpleReportConfiguration;
    _Reports.__SimpleReportConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration";
    let _Elements;
    (function (_Elements) {
        class _ReportElement {
            static _name_ = "name";
        }
        _Elements._ReportElement = _ReportElement;
        _Elements.__ReportElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement";
        class _ReportHeadline {
            static title = "title";
            static _name_ = "name";
        }
        _Elements._ReportHeadline = _ReportHeadline;
        _Elements.__ReportHeadline_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline";
        class _ReportParagraph {
            static paragraph = "paragraph";
            static cssClass = "cssClass";
            static viewNode = "viewNode";
            static evalProperties = "evalProperties";
            static evalParagraph = "evalParagraph";
            static _name_ = "name";
        }
        _Elements._ReportParagraph = _ReportParagraph;
        _Elements.__ReportParagraph_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph";
        class _ReportTable {
            static cssClass = "cssClass";
            static viewNode = "viewNode";
            static form = "form";
            static evalProperties = "evalProperties";
            static _name_ = "name";
        }
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
            static viewNode = "viewNode";
            static elements = "elements";
            static _name_ = "name";
        }
        _Elements._ReportLoop = _ReportLoop;
        _Elements.__ReportLoop_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop";
    })(_Elements = _Reports._Elements || (_Reports._Elements = {}));
})(_Reports || (_Reports = {}));
export var _ExtentLoaderConfigs;
(function (_ExtentLoaderConfigs) {
    class _ExtentLoaderConfig {
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._ExtentLoaderConfig = _ExtentLoaderConfig;
    _ExtentLoaderConfigs.__ExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig";
    class _ExcelLoaderConfig {
        static fixRowCount = "fixRowCount";
        static fixColumnCount = "fixColumnCount";
        static filePath = "filePath";
        static sheetName = "sheetName";
        static offsetRow = "offsetRow";
        static offsetColumn = "offsetColumn";
        static countRows = "countRows";
        static countColumns = "countColumns";
        static hasHeader = "hasHeader";
        static tryMergedHeaderCells = "tryMergedHeaderCells";
        static onlySetColumns = "onlySetColumns";
        static idColumnName = "idColumnName";
        static skipEmptyRowsCount = "skipEmptyRowsCount";
        static columns = "columns";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._ExcelLoaderConfig = _ExcelLoaderConfig;
    _ExtentLoaderConfigs.__ExcelLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig";
    class _ExcelReferenceLoaderConfig {
        static fixRowCount = "fixRowCount";
        static fixColumnCount = "fixColumnCount";
        static filePath = "filePath";
        static sheetName = "sheetName";
        static offsetRow = "offsetRow";
        static offsetColumn = "offsetColumn";
        static countRows = "countRows";
        static countColumns = "countColumns";
        static hasHeader = "hasHeader";
        static tryMergedHeaderCells = "tryMergedHeaderCells";
        static onlySetColumns = "onlySetColumns";
        static idColumnName = "idColumnName";
        static skipEmptyRowsCount = "skipEmptyRowsCount";
        static columns = "columns";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._ExcelReferenceLoaderConfig = _ExcelReferenceLoaderConfig;
    _ExtentLoaderConfigs.__ExcelReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig";
    class _ExcelImportLoaderConfig {
        static extentPath = "extentPath";
        static fixRowCount = "fixRowCount";
        static fixColumnCount = "fixColumnCount";
        static filePath = "filePath";
        static sheetName = "sheetName";
        static offsetRow = "offsetRow";
        static offsetColumn = "offsetColumn";
        static countRows = "countRows";
        static countColumns = "countColumns";
        static hasHeader = "hasHeader";
        static tryMergedHeaderCells = "tryMergedHeaderCells";
        static onlySetColumns = "onlySetColumns";
        static idColumnName = "idColumnName";
        static skipEmptyRowsCount = "skipEmptyRowsCount";
        static columns = "columns";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._ExcelImportLoaderConfig = _ExcelImportLoaderConfig;
    _ExtentLoaderConfigs.__ExcelImportLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig";
    class _ExcelExtentLoaderConfig {
        static filePath = "filePath";
        static idColumnName = "idColumnName";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._ExcelExtentLoaderConfig = _ExcelExtentLoaderConfig;
    _ExtentLoaderConfigs.__ExcelExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig";
    class _InMemoryLoaderConfig {
        static isLinkedList = "isLinkedList";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._InMemoryLoaderConfig = _InMemoryLoaderConfig;
    _ExtentLoaderConfigs.__InMemoryLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig";
    class _XmlReferenceLoaderConfig {
        static filePath = "filePath";
        static keepNamespaces = "keepNamespaces";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._XmlReferenceLoaderConfig = _XmlReferenceLoaderConfig;
    _ExtentLoaderConfigs.__XmlReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig";
    class _ExtentFileLoaderConfig {
        static filePath = "filePath";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._ExtentFileLoaderConfig = _ExtentFileLoaderConfig;
    _ExtentLoaderConfigs.__ExtentFileLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig";
    class _XmiStorageLoaderConfig {
        static filePath = "filePath";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._XmiStorageLoaderConfig = _XmiStorageLoaderConfig;
    _ExtentLoaderConfigs.__XmiStorageLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig";
    class _CsvExtentLoaderConfig {
        static settings = "settings";
        static filePath = "filePath";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._CsvExtentLoaderConfig = _CsvExtentLoaderConfig;
    _ExtentLoaderConfigs.__CsvExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig";
    class _CsvSettings {
        static encoding = "encoding";
        static hasHeader = "hasHeader";
        static separator = "separator";
        static columns = "columns";
        static metaclassUri = "metaclassUri";
        static trimCells = "trimCells";
    }
    _ExtentLoaderConfigs._CsvSettings = _CsvSettings;
    _ExtentLoaderConfigs.__CsvSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings";
    class _ExcelHierarchicalColumnDefinition {
        static _name_ = "name";
        static metaClass = "metaClass";
        static property = "property";
    }
    _ExtentLoaderConfigs._ExcelHierarchicalColumnDefinition = _ExcelHierarchicalColumnDefinition;
    _ExtentLoaderConfigs.__ExcelHierarchicalColumnDefinition_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition";
    class _ExcelHierarchicalLoaderConfig {
        static hierarchicalColumns = "hierarchicalColumns";
        static skipElementsForLastLevel = "skipElementsForLastLevel";
        static fixRowCount = "fixRowCount";
        static fixColumnCount = "fixColumnCount";
        static filePath = "filePath";
        static sheetName = "sheetName";
        static offsetRow = "offsetRow";
        static offsetColumn = "offsetColumn";
        static countRows = "countRows";
        static countColumns = "countColumns";
        static hasHeader = "hasHeader";
        static tryMergedHeaderCells = "tryMergedHeaderCells";
        static onlySetColumns = "onlySetColumns";
        static idColumnName = "idColumnName";
        static skipEmptyRowsCount = "skipEmptyRowsCount";
        static columns = "columns";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._ExcelHierarchicalLoaderConfig = _ExcelHierarchicalLoaderConfig;
    _ExtentLoaderConfigs.__ExcelHierarchicalLoaderConfig_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig";
    class _ExcelColumn {
        static header = "header";
        static _name_ = "name";
    }
    _ExtentLoaderConfigs._ExcelColumn = _ExcelColumn;
    _ExtentLoaderConfigs.__ExcelColumn_Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a";
    class _EnvironmentalVariableLoaderConfig {
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _ExtentLoaderConfigs._EnvironmentalVariableLoaderConfig = _EnvironmentalVariableLoaderConfig;
    _ExtentLoaderConfigs.__EnvironmentalVariableLoaderConfig_Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3";
})(_ExtentLoaderConfigs || (_ExtentLoaderConfigs = {}));
export var _Forms;
(function (_Forms) {
    class _FieldData {
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._FieldData = _FieldData;
    _Forms.__FieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData";
    class _SortingOrder {
        static _name_ = "name";
        static isDescending = "isDescending";
    }
    _Forms._SortingOrder = _SortingOrder;
    _Forms.__SortingOrder_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder";
    class _AnyDataFieldData {
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._AnyDataFieldData = _AnyDataFieldData;
    _Forms.__AnyDataFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData";
    class _CheckboxFieldData {
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._CheckboxFieldData = _CheckboxFieldData;
    _Forms.__CheckboxFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData";
    class _ActionFieldData {
        static actionName = "actionName";
        static parameter = "parameter";
        static buttonText = "buttonText";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._ActionFieldData = _ActionFieldData;
    _Forms.__ActionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData";
    class _DateTimeFieldData {
        static hideDate = "hideDate";
        static hideTime = "hideTime";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._DateTimeFieldData = _DateTimeFieldData;
    _Forms.__DateTimeFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData";
    class _FormAssociation {
        static _name_ = "name";
        static formType = "formType";
        static metaClass = "metaClass";
        static extentType = "extentType";
        static viewModeId = "viewModeId";
        static parentMetaClass = "parentMetaClass";
        static parentProperty = "parentProperty";
        static form = "form";
        static debugActive = "debugActive";
        static workspaceId = "workspaceId";
        static extentUri = "extentUri";
    }
    _Forms._FormAssociation = _FormAssociation;
    _Forms.__FormAssociation_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation";
    class _DropDownFieldData {
        static values = "values";
        static valuesByEnumeration = "valuesByEnumeration";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._DropDownFieldData = _DropDownFieldData;
    _Forms.__DropDownFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData";
    class _ValuePair {
        static value = "value";
        static _name_ = "name";
    }
    _Forms._ValuePair = _ValuePair;
    _Forms.__ValuePair_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair";
    class _MetaClassElementFieldData {
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._MetaClassElementFieldData = _MetaClassElementFieldData;
    _Forms.__MetaClassElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData";
    class _ReferenceFieldData {
        static isSelectionInline = "isSelectionInline";
        static defaultWorkspace = "defaultWorkspace";
        static defaultItemUri = "defaultItemUri";
        static showAllChildren = "showAllChildren";
        static showWorkspaceSelection = "showWorkspaceSelection";
        static showExtentSelection = "showExtentSelection";
        static metaClassFilter = "metaClassFilter";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._ReferenceFieldData = _ReferenceFieldData;
    _Forms.__ReferenceFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData";
    class _SubElementFieldData {
        static metaClass = "metaClass";
        static form = "form";
        static allowOnlyExistingElements = "allowOnlyExistingElements";
        static defaultTypesForNewElements = "defaultTypesForNewElements";
        static includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";
        static defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";
        static defaultExtentOfNewElements = "defaultExtentOfNewElements";
        static actionName = "actionName";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._SubElementFieldData = _SubElementFieldData;
    _Forms.__SubElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData";
    class _TextFieldData {
        static lineHeight = "lineHeight";
        static width = "width";
        static shortenTextLength = "shortenTextLength";
        static supportClipboardCopy = "supportClipboardCopy";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._TextFieldData = _TextFieldData;
    _Forms.__TextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData";
    class _EvalTextFieldData {
        static evalCellProperties = "evalCellProperties";
        static lineHeight = "lineHeight";
        static width = "width";
        static shortenTextLength = "shortenTextLength";
        static supportClipboardCopy = "supportClipboardCopy";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._EvalTextFieldData = _EvalTextFieldData;
    _Forms.__EvalTextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData";
    class _SeparatorLineFieldData {
        static Height = "Height";
    }
    _Forms._SeparatorLineFieldData = _SeparatorLineFieldData;
    _Forms.__SeparatorLineFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData";
    class _FileSelectionFieldData {
        static defaultExtension = "defaultExtension";
        static isSaving = "isSaving";
        static initialPathToDirectory = "initialPathToDirectory";
        static filter = "filter";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._FileSelectionFieldData = _FileSelectionFieldData;
    _Forms.__FileSelectionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData";
    class _DefaultTypeForNewElement {
        static _name_ = "name";
        static metaClass = "metaClass";
        static parentProperty = "parentProperty";
    }
    _Forms._DefaultTypeForNewElement = _DefaultTypeForNewElement;
    _Forms.__DefaultTypeForNewElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement";
    class _FullNameFieldData {
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._FullNameFieldData = _FullNameFieldData;
    _Forms.__FullNameFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData";
    class _CheckboxListTaggingFieldData {
        static values = "values";
        static separator = "separator";
        static containsFreeText = "containsFreeText";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._CheckboxListTaggingFieldData = _CheckboxListTaggingFieldData;
    _Forms.__CheckboxListTaggingFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData";
    class _NumberFieldData {
        static format = "format";
        static isInteger = "isInteger";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
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
        static _name_ = "name";
        static title = "title";
        static isReadOnly = "isReadOnly";
        static isAutoGenerated = "isAutoGenerated";
        static hideMetaInformation = "hideMetaInformation";
        static originalUri = "originalUri";
        static originalWorkspace = "originalWorkspace";
        static creationProtocol = "creationProtocol";
    }
    _Forms._Form = _Form;
    _Forms.__Form_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form";
    class _RowForm {
        static buttonApplyText = "buttonApplyText";
        static allowNewProperties = "allowNewProperties";
        static defaultWidth = "defaultWidth";
        static defaultHeight = "defaultHeight";
        static field = "field";
        static _name_ = "name";
        static title = "title";
        static isReadOnly = "isReadOnly";
        static isAutoGenerated = "isAutoGenerated";
        static hideMetaInformation = "hideMetaInformation";
        static originalUri = "originalUri";
        static originalWorkspace = "originalWorkspace";
        static creationProtocol = "creationProtocol";
    }
    _Forms._RowForm = _RowForm;
    _Forms.__RowForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm";
    class _TableForm {
        static property = "property";
        static metaClass = "metaClass";
        static includeDescendents = "includeDescendents";
        static noItemsWithMetaClass = "noItemsWithMetaClass";
        static inhibitNewItems = "inhibitNewItems";
        static inhibitDeleteItems = "inhibitDeleteItems";
        static inhibitEditItems = "inhibitEditItems";
        static defaultTypesForNewElements = "defaultTypesForNewElements";
        static fastViewFilters = "fastViewFilters";
        static field = "field";
        static sortingOrder = "sortingOrder";
        static viewNode = "viewNode";
        static autoGenerateFields = "autoGenerateFields";
        static duplicatePerType = "duplicatePerType";
        static dataUrl = "dataUrl";
        static inhibitNewUnclassifiedItems = "inhibitNewUnclassifiedItems";
        static _name_ = "name";
        static title = "title";
        static isReadOnly = "isReadOnly";
        static isAutoGenerated = "isAutoGenerated";
        static hideMetaInformation = "hideMetaInformation";
        static originalUri = "originalUri";
        static originalWorkspace = "originalWorkspace";
        static creationProtocol = "creationProtocol";
    }
    _Forms._TableForm = _TableForm;
    _Forms.__TableForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm";
    class _CollectionForm {
        static tab = "tab";
        static autoTabs = "autoTabs";
        static field = "field";
        static _name_ = "name";
        static title = "title";
        static isReadOnly = "isReadOnly";
        static isAutoGenerated = "isAutoGenerated";
        static hideMetaInformation = "hideMetaInformation";
        static originalUri = "originalUri";
        static originalWorkspace = "originalWorkspace";
        static creationProtocol = "creationProtocol";
    }
    _Forms._CollectionForm = _CollectionForm;
    _Forms.__CollectionForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm";
    class _ObjectForm {
        static tab = "tab";
        static autoTabs = "autoTabs";
        static _name_ = "name";
        static title = "title";
        static isReadOnly = "isReadOnly";
        static isAutoGenerated = "isAutoGenerated";
        static hideMetaInformation = "hideMetaInformation";
        static originalUri = "originalUri";
        static originalWorkspace = "originalWorkspace";
        static creationProtocol = "creationProtocol";
    }
    _Forms._ObjectForm = _ObjectForm;
    _Forms.__ObjectForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm";
    class _ViewMode {
        static _name_ = "name";
        static id = "id";
        static defaultExtentType = "defaultExtentType";
    }
    _Forms._ViewMode = _ViewMode;
    _Forms.__ViewMode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode";
    class _DropDownByCollection {
        static defaultWorkspace = "defaultWorkspace";
        static collection = "collection";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._DropDownByCollection = _DropDownByCollection;
    _Forms.__DropDownByCollection_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection";
    class _UriReferenceFieldData {
        static defaultWorkspace = "defaultWorkspace";
        static defaultExtent = "defaultExtent";
    }
    _Forms._UriReferenceFieldData = _UriReferenceFieldData;
    _Forms.__UriReferenceFieldData_Uri = "dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00";
    class _NavigateToFieldsForTestAction {
        static _name_ = "name";
        static isDisabled = "isDisabled";
    }
    _Forms._NavigateToFieldsForTestAction = _NavigateToFieldsForTestAction;
    _Forms.__NavigateToFieldsForTestAction_Uri = "dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0";
    class _DropDownByQueryData {
        static query = "query";
        static isAttached = "isAttached";
        static _name_ = "name";
        static title = "title";
        static isEnumeration = "isEnumeration";
        static defaultValue = "defaultValue";
        static isReadOnly = "isReadOnly";
    }
    _Forms._DropDownByQueryData = _DropDownByQueryData;
    _Forms.__DropDownByQueryData_Uri = "dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2";
})(_Forms || (_Forms = {}));
export var _AttachedExtent;
(function (_AttachedExtent) {
    class _AttachedExtentConfiguration {
        static _name_ = "name";
        static referencedWorkspace = "referencedWorkspace";
        static referencedExtent = "referencedExtent";
        static referenceType = "referenceType";
        static referenceProperty = "referenceProperty";
    }
    _AttachedExtent._AttachedExtentConfiguration = _AttachedExtentConfiguration;
    _AttachedExtent.__AttachedExtentConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration";
})(_AttachedExtent || (_AttachedExtent = {}));
export var _Management;
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
        static _name_ = "name";
        static uri = "uri";
        static workspaceId = "workspaceId";
        static count = "count";
        static totalCount = "totalCount";
        static type = "type";
        static extentType = "extentType";
        static isModified = "isModified";
        static alternativeUris = "alternativeUris";
        static autoEnumerateType = "autoEnumerateType";
        static state = "state";
        static failMessage = "failMessage";
        static properties = "properties";
        static loadingConfiguration = "loadingConfiguration";
    }
    _Management._Extent = _Extent;
    _Management.__Extent_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent";
    class _Workspace {
        static id = "id";
        static annotation = "annotation";
        static extents = "extents";
    }
    _Management._Workspace = _Workspace;
    _Management.__Workspace_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace";
    class _CreateNewWorkspaceModel {
        static id = "id";
        static annotation = "annotation";
    }
    _Management._CreateNewWorkspaceModel = _CreateNewWorkspaceModel;
    _Management.__CreateNewWorkspaceModel_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel";
    class _ExtentTypeSetting {
        static _name_ = "name";
        static rootElementMetaClasses = "rootElementMetaClasses";
    }
    _Management._ExtentTypeSetting = _ExtentTypeSetting;
    _Management.__ExtentTypeSetting_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting";
    class _ExtentProperties {
        static _name_ = "name";
        static uri = "uri";
        static workspaceId = "workspaceId";
        static count = "count";
        static totalCount = "totalCount";
        static type = "type";
        static extentType = "extentType";
        static isModified = "isModified";
        static alternativeUris = "alternativeUris";
        static autoEnumerateType = "autoEnumerateType";
        static state = "state";
        static failMessage = "failMessage";
        static properties = "properties";
        static loadingConfiguration = "loadingConfiguration";
    }
    _Management._ExtentProperties = _ExtentProperties;
    _Management.__ExtentProperties_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties";
    class _ExtentPropertyDefinition {
        static _name_ = "name";
        static title = "title";
        static metaClass = "metaClass";
    }
    _Management._ExtentPropertyDefinition = _ExtentPropertyDefinition;
    _Management.__ExtentPropertyDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition";
    class _ExtentSettings {
        static extentTypeSettings = "extentTypeSettings";
        static propertyDefinitions = "propertyDefinitions";
    }
    _Management._ExtentSettings = _ExtentSettings;
    _Management.__ExtentSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings";
})(_Management || (_Management = {}));
export var _FastViewFilters;
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
        static Property = "Property";
        static ComparisonType = "ComparisonType";
        static Value = "Value";
    }
    _FastViewFilters._PropertyComparisonFilter = _PropertyComparisonFilter;
    _FastViewFilters.__PropertyComparisonFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter";
    class _PropertyContainsFilter {
        static Property = "Property";
        static Value = "Value";
    }
    _FastViewFilters._PropertyContainsFilter = _PropertyContainsFilter;
    _FastViewFilters.__PropertyContainsFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter";
})(_FastViewFilters || (_FastViewFilters = {}));
export var _DynamicRuntimeProvider;
(function (_DynamicRuntimeProvider) {
    class _DynamicRuntimeLoaderConfig {
        static runtimeClass = "runtimeClass";
        static configuration = "configuration";
        static _name_ = "name";
        static extentUri = "extentUri";
        static workspaceId = "workspaceId";
        static dropExisting = "dropExisting";
    }
    _DynamicRuntimeProvider._DynamicRuntimeLoaderConfig = _DynamicRuntimeLoaderConfig;
    _DynamicRuntimeProvider.__DynamicRuntimeLoaderConfig_Uri = "dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig";
    let _Examples;
    (function (_Examples) {
        class _NumberProviderSettings {
            static _name_ = "name";
            static start = "start";
            static end = "end";
        }
        _Examples._NumberProviderSettings = _NumberProviderSettings;
        _Examples.__NumberProviderSettings_Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a";
        class _NumberRepresentation {
            static binary = "binary";
            static octal = "octal";
            static decimal = "decimal";
            static hexadecimal = "hexadecimal";
        }
        _Examples._NumberRepresentation = _NumberRepresentation;
        _Examples.__NumberRepresentation_Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation";
    })(_Examples = _DynamicRuntimeProvider._Examples || (_DynamicRuntimeProvider._Examples = {}));
})(_DynamicRuntimeProvider || (_DynamicRuntimeProvider = {}));
export var _Verifier;
(function (_Verifier) {
    class _VerifyEntry {
        static workspaceId = "workspaceId";
        static itemUri = "itemUri";
        static category = "category";
        static message = "message";
    }
    _Verifier._VerifyEntry = _VerifyEntry;
    _Verifier.__VerifyEntry_Uri = "dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768";
})(_Verifier || (_Verifier = {}));
//# sourceMappingURL=DatenMeister.class.js.map