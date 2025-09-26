// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.3.0.0
export namespace _CommonTypes
{
        export class _DateTime
        {
        }

        export const __DateTime_Uri = "dm:///_internal/types/internal#DateTime";
        export namespace _Default
        {
                export class _Package
                {
                    static _name_ = "name";
                    static packagedElement = "packagedElement";
                    static preferredType = "preferredType";
                    static preferredPackage = "preferredPackage";
                    static defaultViewMode = "defaultViewMode";
                }

                export const __Package_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package";
                export class _XmiExportContainer
                {
                    static xmi = "xmi";
                }

                export const __XmiExportContainer_Uri = "dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1";
                export class _XmiImportContainer
                {
                    static xmi = "xmi";
                    static property = "property";
                    static addToCollection = "addToCollection";
                }

                export const __XmiImportContainer_Uri = "dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a";
        }

        export namespace _ExtentManager
        {
                export class _ImportSettings
                {
                    static filePath = "filePath";
                    static extentUri = "extentUri";
                    static workspaceId = "workspaceId";
                }

                export const __ImportSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings";
                export class _ImportException
                {
                    static message = "message";
                }

                export const __ImportException_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException";
        }

        export namespace _OSIntegration
        {
                export class _CommandLineApplication
                {
                    static _name_ = "name";
                    static applicationPath = "applicationPath";
                }

                export const __CommandLineApplication_Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication";
                export class _EnvironmentalVariable
                {
                    static _name_ = "name";
                    static value = "value";
                }

                export const __EnvironmentalVariable_Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable";
        }

}

export namespace _Actions
{
        export class _ActionSet
        {
            static action = "action";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __ActionSet_Uri = "dm:///_internal/types/internal#Actions.ActionSet";
        export class _LoggingWriterAction
        {
            static message = "message";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __LoggingWriterAction_Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction";
        export class _CommandExecutionAction
        {
            static command = "command";
            static _arguments_ = "arguments";
            static workingDirectory = "workingDirectory";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __CommandExecutionAction_Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82";
        export class _PowershellExecutionAction
        {
            static script = "script";
            static workingDirectory = "workingDirectory";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __PowershellExecutionAction_Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb";
        export class _LoadExtentAction
        {
            static configuration = "configuration";
            static dropExisting = "dropExisting";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __LoadExtentAction_Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee";
        export class _DropExtentAction
        {
            static workspaceId = "workspaceId";
            static extentUri = "extentUri";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __DropExtentAction_Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09";
        export class _CreateWorkspaceAction
        {
            static workspaceId = "workspaceId";
            static annotation = "annotation";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __CreateWorkspaceAction_Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe";
        export class _DropWorkspaceAction
        {
            static workspaceId = "workspaceId";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __DropWorkspaceAction_Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8";
        export class _CopyElementsAction
        {
            static sourcePath = "sourcePath";
            static targetPath = "targetPath";
            static moveOnly = "moveOnly";
            static sourceWorkspace = "sourceWorkspace";
            static targetWorkspace = "targetWorkspace";
            static emptyTarget = "emptyTarget";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __CopyElementsAction_Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed";
        export class _ExportToXmiAction
        {
            static sourcePath = "sourcePath";
            static filePath = "filePath";
            static sourceWorkspaceId = "sourceWorkspaceId";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __ExportToXmiAction_Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863";
        export class _ClearCollectionAction
        {
            static workspaceId = "workspaceId";
            static path = "path";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __ClearCollectionAction_Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae";
        export class _TransformItemsAction
        {
            static metaClass = "metaClass";
            static runtimeClass = "runtimeClass";
            static workspaceId = "workspaceId";
            static path = "path";
            static excludeDescendents = "excludeDescendents";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __TransformItemsAction_Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler";
        export class _EchoAction
        {
            static shallSuccess = "shallSuccess";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __EchoAction_Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction";
        export class _DocumentOpenAction
        {
            static filePath = "filePath";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __DocumentOpenAction_Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74";
        export class _CreateFormByMetaClass
        {
            static metaClass = "metaClass";
            static creationMode = "creationMode";
            static targetContainer = "targetContainer";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __CreateFormByMetaClass_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.CreateFormByMetaclass";
        export namespace _Reports
        {
                export class _SimpleReportAction
                {
                    static path = "path";
                    static configuration = "configuration";
                    static workspaceId = "workspaceId";
                    static filePath = "filePath";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __SimpleReportAction_Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction";
                export class _AdocReportAction
                {
                    static filePath = "filePath";
                    static reportInstance = "reportInstance";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __AdocReportAction_Uri = "dm:///_internal/types/internal#Actions.AdocReportAction";
                export class _HtmlReportAction
                {
                    static filePath = "filePath";
                    static reportInstance = "reportInstance";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __HtmlReportAction_Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction";
        }

        export class _Action
        {
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __Action_Uri = "dm:///_internal/types/internal#Actions.Action";
        export class _MoveOrCopyAction
        {
            static copyMode = "copyMode";
            static target = "target";
            static source = "source";
        }

        export const __MoveOrCopyAction_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.MoveOrCopyAction";
        export module _MoveOrCopyType
        {
            export const Copy = "Copy";
            export const Move = "Move";
        }

        export enum ___MoveOrCopyType
        {
            Copy,
            Move
        }

        export module _MoveDirectionType
        {
            export const Up = "Up";
            export const Down = "Down";
        }

        export enum ___MoveDirectionType
        {
            Up,
            Down
        }

        export class _MoveUpDownAction
        {
            static element = "element";
            static direction = "direction";
            static container = "container";
            static property = "property";
        }

        export const __MoveUpDownAction_Uri = "dm:///_internal/types/internal#bc4952bf-a3f5-4516-be26-5b773e38bd54";
        export class _StoreExtentAction
        {
            static workspaceId = "workspaceId";
            static extentUri = "extentUri";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __StoreExtentAction_Uri = "dm:///_internal/types/internal#43b0764e-b70f-42bb-b37d-ae8586ec45f1";
        export class _ImportXmiAction
        {
            static workspaceId = "workspaceId";
            static itemUri = "itemUri";
            static xmi = "xmi";
            static property = "property";
            static addToCollection = "addToCollection";
        }

        export const __ImportXmiAction_Uri = "dm:///_internal/types/internal#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d";
        export class _DeletePropertyFromCollectionAction
        {
            static propertyName = "propertyName";
            static metaclass = "metaclass";
            static collectionUrl = "collectionUrl";
        }

        export const __DeletePropertyFromCollectionAction_Uri = "dm:///_internal/types/internal#b631bb00-ab11-4a8a-a148-e28abc398503";
        export class _MoveOrCopyActionResult
        {
            static targetUrl = "targetUrl";
            static targetWorkspace = "targetWorkspace";
        }

        export const __MoveOrCopyActionResult_Uri = "dm:///_internal/types/internal#3223e13a-bbb7-4785-8b81-7275be23b0a1";
        export namespace _ParameterTypes
        {
                export class _NavigationDefineActionParameter
                {
                    static actionName = "actionName";
                    static formUrl = "formUrl";
                    static metaClassUrl = "metaClassUrl";
                }

                export const __NavigationDefineActionParameter_Uri = "dm:///_internal/types/internal#90f61e4e-a5ea-42eb-9caa-912d010fbccd";
                export class _LoadExtentActionResult
                {
                    static workspaceId = "workspaceId";
                    static extentUri = "extentUri";
                }

                export const __LoadExtentActionResult_Uri = "dm:///_internal/types/internal#2863f928-fe69-4d35-8c67-f4f3533b7ae5";
        }

        export class _ActionResult
        {
            static isSuccess = "isSuccess";
            static clientActions = "clientActions";
        }

        export const __ActionResult_Uri = "dm:///_internal/types/internal#899324b1-85dc-40a1-ba95-dec50509040d";
        export namespace _ClientActions
        {
                export class _ClientAction
                {
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                }

                export const __ClientAction_Uri = "dm:///_internal/types/internal#e07ca80e-2540-4f91-8214-60dbd464e998";
                export class _AlertClientAction
                {
                    static messageText = "messageText";
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                }

                export const __AlertClientAction_Uri = "dm:///_internal/types/internal#0ee17f2a-5407-4d38-b1b4-34ead2186971";
                export class _NavigateToExtentClientAction
                {
                    static workspaceId = "workspaceId";
                    static extentUri = "extentUri";
                }

                export const __NavigateToExtentClientAction_Uri = "dm:///_internal/types/internal#3251783f-2683-4c24-bad5-828930028462";
                export class _NavigateToItemClientAction
                {
                    static workspaceId = "workspaceId";
                    static itemUrl = "itemUrl";
                    static formUri = "formUri";
                }

                export const __NavigateToItemClientAction_Uri = "dm:///_internal/types/internal#5f69675e-df58-4ad7-84bf-359cdfba5db4";
        }

        export class _ConsoleWriteAction
        {
            static text = "text";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __ConsoleWriteAction_Uri = "dm:///_internal/types/internal#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a";
}

export namespace _DataViews
{
        export class _DataView
        {
            static _name_ = "name";
            static workspaceId = "workspaceId";
            static uri = "uri";
            static viewNode = "viewNode";
        }

        export const __DataView_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView";
        export class _ViewNode
        {
            static _name_ = "name";
        }

        export const __ViewNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode";
        export class _SelectByExtentNode
        {
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static _name_ = "name";
        }

        export const __SelectByExtentNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode";
        export class _FlattenNode
        {
            static input = "input";
            static _name_ = "name";
        }

        export const __FlattenNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode";
        export class _RowFilterByPropertyValueNode
        {
            static input = "input";
            static property = "property";
            static value = "value";
            static comparisonMode = "comparisonMode";
            static _name_ = "name";
        }

        export const __RowFilterByPropertyValueNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode";
        export class _RowFilterByMetaclassNode
        {
            static input = "input";
            static metaClass = "metaClass";
            static includeInherits = "includeInherits";
            static _name_ = "name";
        }

        export const __RowFilterByMetaclassNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode";
        export module _ComparisonMode
        {
            export const Equal = "Equal";
            export const NotEqual = "NotEqual";
            export const Contains = "Contains";
            export const DoesNotContain = "DoesNotContain";
            export const GreaterThan = "GreaterThan";
            export const GreaterOrEqualThan = "GreaterOrEqualThan";
            export const LighterThan = "LighterThan";
            export const LighterOrEqualThan = "LighterOrEqualThan";
            export const RegexMatch = "RegexMatch";
            export const RegexNoMatch = "RegexNoMatch";
        }

        export enum ___ComparisonMode
        {
            Equal,
            NotEqual,
            Contains,
            DoesNotContain,
            GreaterThan,
            GreaterOrEqualThan,
            LighterThan,
            LighterOrEqualThan,
            RegexMatch,
            RegexNoMatch
        }

        export class _SelectByFullNameNode
        {
            static input = "input";
            static path = "path";
            static _name_ = "name";
        }

        export const __SelectByFullNameNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode";
        export class _DynamicSourceNode
        {
            static nodeName = "nodeName";
            static _name_ = "name";
        }

        export const __DynamicSourceNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode";
        export class _SelectByPathNode
        {
            static workspaceId = "workspaceId";
            static path = "path";
        }

        export const __SelectByPathNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode";
        export class _QueryStatement
        {
            static nodes = "nodes";
            static resultNode = "resultNode";
        }

        export const __QueryStatement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement";
        export class _SelectFromAllWorkspacesNode
        {
            static _name_ = "name";
        }

        export const __SelectFromAllWorkspacesNode_Uri = "dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a";
        export class _SelectByWorkspaceNode
        {
            static workspaceId = "workspaceId";
            static _name_ = "name";
        }

        export const __SelectByWorkspaceNode_Uri = "dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db";
        export class _ColumnFilterExcludeNode
        {
            static columnNamesComma = "columnNamesComma";
            static input = "input";
            static _name_ = "name";
        }

        export const __ColumnFilterExcludeNode_Uri = "dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7";
        export class _ColumnFilterIncludeOnlyNode
        {
            static columnNamesComma = "columnNamesComma";
            static input = "input";
            static _name_ = "name";
        }

        export const __ColumnFilterIncludeOnlyNode_Uri = "dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d";
        export class _RowFilterOnPositionNode
        {
            static input = "input";
            static amount = "amount";
            static position = "position";
        }

        export const __RowFilterOnPositionNode_Uri = "dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f";
        export class _RowOrderByNode
        {
            static input = "input";
            static propertyName = "propertyName";
            static orderDescending = "orderDescending";
        }

        export const __RowOrderByNode_Uri = "dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9";
        export class _RowFilterByFreeTextAnywhere
        {
            static freeText = "freeText";
            static input = "input";
            static _name_ = "name";
        }

        export const __RowFilterByFreeTextAnywhere_Uri = "dm:///_internal/types/internal#5f66ff9a-0a68-4c87-856b-5921c7cae628";
}

export namespace _Reports
{
        export class _ReportDefinition
        {
            static _name_ = "name";
            static title = "title";
            static elements = "elements";
        }

        export const __ReportDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition";
        export class _ReportInstanceSource
        {
            static _name_ = "name";
            static workspaceId = "workspaceId";
            static path = "path";
        }

        export const __ReportInstanceSource_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource";
        export class _ReportInstance
        {
            static _name_ = "name";
            static reportDefinition = "reportDefinition";
            static sources = "sources";
        }

        export const __ReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance";
        export class _AdocReportInstance
        {
            static _name_ = "name";
            static reportDefinition = "reportDefinition";
            static sources = "sources";
        }

        export const __AdocReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance";
        export class _HtmlReportInstance
        {
            static cssFile = "cssFile";
            static cssStyleSheet = "cssStyleSheet";
            static _name_ = "name";
            static reportDefinition = "reportDefinition";
            static sources = "sources";
        }

        export const __HtmlReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance";
        export module _DescendentMode
        {
            export const None = "None";
            export const Inline = "Inline";
            export const PerPackage = "PerPackage";
        }

        export enum ___DescendentMode
        {
            None,
            Inline,
            PerPackage
        }

        export class _SimpleReportConfiguration
        {
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

        export const __SimpleReportConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration";
        export namespace _Elements
        {
                export class _ReportElement
                {
                    static _name_ = "name";
                }

                export const __ReportElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement";
                export class _ReportHeadline
                {
                    static title = "title";
                    static _name_ = "name";
                }

                export const __ReportHeadline_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline";
                export class _ReportParagraph
                {
                    static paragraph = "paragraph";
                    static cssClass = "cssClass";
                    static viewNode = "viewNode";
                    static evalProperties = "evalProperties";
                    static evalParagraph = "evalParagraph";
                    static _name_ = "name";
                }

                export const __ReportParagraph_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph";
                export class _ReportTable
                {
                    static cssClass = "cssClass";
                    static viewNode = "viewNode";
                    static form = "form";
                    static evalProperties = "evalProperties";
                    static _name_ = "name";
                }

                export const __ReportTable_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable";
                export module _ReportTableForTypeMode
                {
                    export const PerType = "PerType";
                    export const AllTypes = "AllTypes";
                }

                export enum ___ReportTableForTypeMode
                {
                    PerType,
                    AllTypes
                }

                export class _ReportLoop
                {
                    static viewNode = "viewNode";
                    static elements = "elements";
                    static _name_ = "name";
                }

                export const __ReportLoop_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop";
        }

}

export namespace _ExtentLoaderConfigs
{
        export class _ExtentLoaderConfig
        {
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __ExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig";
        export class _ExcelLoaderConfig
        {
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

        export const __ExcelLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig";
        export class _ExcelReferenceLoaderConfig
        {
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

        export const __ExcelReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig";
        export class _ExcelImportLoaderConfig
        {
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

        export const __ExcelImportLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig";
        export class _ExcelExtentLoaderConfig
        {
            static filePath = "filePath";
            static idColumnName = "idColumnName";
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __ExcelExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig";
        export class _InMemoryLoaderConfig
        {
            static isLinkedList = "isLinkedList";
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __InMemoryLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig";
        export class _XmlReferenceLoaderConfig
        {
            static filePath = "filePath";
            static keepNamespaces = "keepNamespaces";
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __XmlReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig";
        export class _ExtentFileLoaderConfig
        {
            static filePath = "filePath";
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __ExtentFileLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig";
        export class _XmiStorageLoaderConfig
        {
            static filePath = "filePath";
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __XmiStorageLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig";
        export class _CsvExtentLoaderConfig
        {
            static settings = "settings";
            static filePath = "filePath";
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __CsvExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig";
        export class _CsvSettings
        {
            static encoding = "encoding";
            static hasHeader = "hasHeader";
            static separator = "separator";
            static columns = "columns";
            static metaclassUri = "metaclassUri";
            static trimCells = "trimCells";
        }

        export const __CsvSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings";
        export class _ExcelHierarchicalColumnDefinition
        {
            static _name_ = "name";
            static metaClass = "metaClass";
            static property = "property";
        }

        export const __ExcelHierarchicalColumnDefinition_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition";
        export class _ExcelHierarchicalLoaderConfig
        {
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

        export const __ExcelHierarchicalLoaderConfig_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig";
        export class _ExcelColumn
        {
            static header = "header";
            static _name_ = "name";
        }

        export const __ExcelColumn_Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a";
        export class _EnvironmentalVariableLoaderConfig
        {
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __EnvironmentalVariableLoaderConfig_Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3";
}

export namespace _Forms
{
        export class _FieldData
        {
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __FieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData";
        export class _SortingOrder
        {
            static _name_ = "name";
            static isDescending = "isDescending";
        }

        export const __SortingOrder_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder";
        export class _AnyDataFieldData
        {
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __AnyDataFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData";
        export class _CheckboxFieldData
        {
            static lineHeight = "lineHeight";
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __CheckboxFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData";
        export class _ActionFieldData
        {
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

        export const __ActionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData";
        export class _DateTimeFieldData
        {
            static hideDate = "hideDate";
            static hideTime = "hideTime";
            static showOffsetButtons = "showOffsetButtons";
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __DateTimeFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData";
        export class _FormAssociation
        {
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

        export const __FormAssociation_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation";
        export class _DropDownFieldData
        {
            static values = "values";
            static valuesByEnumeration = "valuesByEnumeration";
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __DropDownFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData";
        export class _ValuePair
        {
            static value = "value";
            static _name_ = "name";
        }

        export const __ValuePair_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair";
        export class _MetaClassElementFieldData
        {
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __MetaClassElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData";
        export class _ReferenceFieldData
        {
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

        export const __ReferenceFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData";
        export class _SubElementFieldData
        {
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

        export const __SubElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData";
        export class _TextFieldData
        {
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

        export const __TextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData";
        export class _EvalTextFieldData
        {
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

        export const __EvalTextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData";
        export class _SeparatorLineFieldData
        {
            static Height = "Height";
        }

        export const __SeparatorLineFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData";
        export class _FileSelectionFieldData
        {
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

        export const __FileSelectionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData";
        export class _DefaultTypeForNewElement
        {
            static _name_ = "name";
            static metaClass = "metaClass";
            static parentProperty = "parentProperty";
        }

        export const __DefaultTypeForNewElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement";
        export class _FullNameFieldData
        {
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __FullNameFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData";
        export class _CheckboxListTaggingFieldData
        {
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

        export const __CheckboxListTaggingFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData";
        export class _NumberFieldData
        {
            static format = "format";
            static isInteger = "isInteger";
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __NumberFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData";
        export module _FormType
        {
            export const Object = "Object";
            export const Collection = "Collection";
            export const Row = "Row";
            export const Table = "Table";
            export const ObjectExtension = "ObjectExtension";
            export const CollectionExtension = "CollectionExtension";
            export const RowExtension = "RowExtension";
            export const TableExtension = "TableExtension";
        }

        export enum ___FormType
        {
            Object,
            Collection,
            Row,
            Table,
            ObjectExtension,
            CollectionExtension,
            RowExtension,
            TableExtension
        }

        export class _Form
        {
            static _name_ = "name";
            static title = "title";
            static isReadOnly = "isReadOnly";
            static isAutoGenerated = "isAutoGenerated";
            static hideMetaInformation = "hideMetaInformation";
            static originalUri = "originalUri";
            static originalWorkspace = "originalWorkspace";
            static creationProtocol = "creationProtocol";
        }

        export const __Form_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form";
        export class _RowForm
        {
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

        export const __RowForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm";
        export class _TableForm
        {
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

        export const __TableForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm";
        export class _CollectionForm
        {
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

        export const __CollectionForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm";
        export class _ObjectForm
        {
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

        export const __ObjectForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm";
        export class _ViewMode
        {
            static _name_ = "name";
            static id = "id";
            static defaultExtentType = "defaultExtentType";
        }

        export const __ViewMode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode";
        export class _DropDownByCollection
        {
            static defaultWorkspace = "defaultWorkspace";
            static collection = "collection";
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __DropDownByCollection_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection";
        export class _UriReferenceFieldData
        {
            static defaultWorkspace = "defaultWorkspace";
            static defaultExtent = "defaultExtent";
        }

        export const __UriReferenceFieldData_Uri = "dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00";
        export class _NavigateToFieldsForTestAction
        {
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __NavigateToFieldsForTestAction_Uri = "dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0";
        export class _DropDownByQueryData
        {
            static query = "query";
            static isAttached = "isAttached";
            static _name_ = "name";
            static title = "title";
            static isEnumeration = "isEnumeration";
            static defaultValue = "defaultValue";
            static isReadOnly = "isReadOnly";
        }

        export const __DropDownByQueryData_Uri = "dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2";
}

export namespace _AttachedExtent
{
        export class _AttachedExtentConfiguration
        {
            static _name_ = "name";
            static referencedWorkspace = "referencedWorkspace";
            static referencedExtent = "referencedExtent";
            static referenceType = "referenceType";
            static referenceProperty = "referenceProperty";
        }

        export const __AttachedExtentConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration";
}

export namespace _Management
{
        export module _ExtentLoadingState
        {
            export const Unknown = "Unknown";
            export const Unloaded = "Unloaded";
            export const Loaded = "Loaded";
            export const Failed = "Failed";
            export const LoadedReadOnly = "LoadedReadOnly";
        }

        export enum ___ExtentLoadingState
        {
            Unknown,
            Unloaded,
            Loaded,
            Failed,
            LoadedReadOnly
        }

        export class _Extent
        {
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

        export const __Extent_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent";
        export class _Workspace
        {
            static id = "id";
            static annotation = "annotation";
            static extents = "extents";
        }

        export const __Workspace_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace";
        export class _CreateNewWorkspaceModel
        {
            static id = "id";
            static annotation = "annotation";
        }

        export const __CreateNewWorkspaceModel_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel";
        export class _ExtentTypeSetting
        {
            static _name_ = "name";
            static rootElementMetaClasses = "rootElementMetaClasses";
        }

        export const __ExtentTypeSetting_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting";
        export class _ExtentProperties
        {
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

        export const __ExtentProperties_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties";
        export class _ExtentPropertyDefinition
        {
            static _name_ = "name";
            static title = "title";
            static metaClass = "metaClass";
        }

        export const __ExtentPropertyDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition";
        export class _ExtentSettings
        {
            static extentTypeSettings = "extentTypeSettings";
            static propertyDefinitions = "propertyDefinitions";
        }

        export const __ExtentSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings";
}

export namespace _FastViewFilters
{
        export module _ComparisonType
        {
            export const Equal = "Equal";
            export const GreaterThan = "GreaterThan";
            export const LighterThan = "LighterThan";
            export const GreaterOrEqualThan = "GreaterOrEqualThan";
            export const LighterOrEqualThan = "LighterOrEqualThan";
        }

        export enum ___ComparisonType
        {
            Equal,
            GreaterThan,
            LighterThan,
            GreaterOrEqualThan,
            LighterOrEqualThan
        }

        export class _PropertyComparisonFilter
        {
            static Property = "Property";
            static ComparisonType = "ComparisonType";
            static Value = "Value";
        }

        export const __PropertyComparisonFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter";
        export class _PropertyContainsFilter
        {
            static Property = "Property";
            static Value = "Value";
        }

        export const __PropertyContainsFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter";
}

export namespace _DynamicRuntimeProvider
{
        export class _DynamicRuntimeLoaderConfig
        {
            static runtimeClass = "runtimeClass";
            static configuration = "configuration";
            static _name_ = "name";
            static extentUri = "extentUri";
            static workspaceId = "workspaceId";
            static dropExisting = "dropExisting";
        }

        export const __DynamicRuntimeLoaderConfig_Uri = "dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig";
        export namespace _Examples
        {
                export class _NumberProviderSettings
                {
                    static _name_ = "name";
                    static start = "start";
                    static end = "end";
                }

                export const __NumberProviderSettings_Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a";
                export class _NumberRepresentation
                {
                    static binary = "binary";
                    static octal = "octal";
                    static decimal = "decimal";
                    static hexadecimal = "hexadecimal";
                }

                export const __NumberRepresentation_Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation";
        }

}

export namespace _Verifier
{
        export class _VerifyEntry
        {
            static workspaceId = "workspaceId";
            static itemUri = "itemUri";
            static category = "category";
            static message = "message";
        }

        export const __VerifyEntry_Uri = "dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768";
}

