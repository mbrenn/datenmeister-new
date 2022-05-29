// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.0.0.0
export module _PrimitiveTypes
{
    export module _DateTime
    {
    }

    export const __DateTime_Uri = "dm:///_internal/types/internal#PrimitiveTypes.DateTime";
}

export module _DatenMeister
{
    export module _CommonTypes
    {
        export module _Default
        {
            export module _Package
            {
                export const name = "name";
                export const packagedElement = "packagedElement";
                export const preferredType = "preferredType";
                export const preferredPackage = "preferredPackage";
                export const defaultViewMode = "defaultViewMode";
            }

            export const __Package_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package";
        }

        export module _ExtentManager
        {
            export module _ImportSettings
            {
                export const filePath = "filePath";
                export const extentUri = "extentUri";
                export const workspace = "workspace";
            }

            export const __ImportSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings";
            export module _ImportException
            {
                export const message = "message";
            }

            export const __ImportException_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException";
        }

        export module _OSIntegration
        {
            export module _CommandLineApplication
            {
                export const name = "name";
                export const applicationPath = "applicationPath";
            }

            export const __CommandLineApplication_Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication";
            export module _EnvironmentalVariable
            {
                export const name = "name";
                export const value = "value";
            }

            export const __EnvironmentalVariable_Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable";
        }

    }

    export module _Actions
    {
        export module _ActionSet
        {
            export const action = "action";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __ActionSet_Uri = "dm:///_internal/types/internal#Actions.ActionSet";
        export module _LoggingWriterAction
        {
            export const message = "message";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __LoggingWriterAction_Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction";
        export module _CommandExecutionAction
        {
            export const command = "command";
            export const _arguments_ = "arguments";
            export const workingDirectory = "workingDirectory";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __CommandExecutionAction_Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82";
        export module _PowershellExecutionAction
        {
            export const script = "script";
            export const workingDirectory = "workingDirectory";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __PowershellExecutionAction_Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb";
        export module _LoadExtentAction
        {
            export const configuration = "configuration";
            export const dropExisting = "dropExisting";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __LoadExtentAction_Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee";
        export module _DropExtentAction
        {
            export const workspace = "workspace";
            export const extentUri = "extentUri";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __DropExtentAction_Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09";
        export module _CreateWorkspaceAction
        {
            export const workspace = "workspace";
            export const annotation = "annotation";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __CreateWorkspaceAction_Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe";
        export module _DropWorkspaceAction
        {
            export const workspace = "workspace";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __DropWorkspaceAction_Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8";
        export module _CopyElementsAction
        {
            export const sourcePath = "sourcePath";
            export const targetPath = "targetPath";
            export const moveOnly = "moveOnly";
            export const sourceWorkspace = "sourceWorkspace";
            export const targetWorkspace = "targetWorkspace";
            export const emptyTarget = "emptyTarget";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __CopyElementsAction_Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed";
        export module _ExportToXmiAction
        {
            export const sourcePath = "sourcePath";
            export const filePath = "filePath";
            export const sourceWorkspaceId = "sourceWorkspaceId";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __ExportToXmiAction_Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863";
        export module _ClearCollectionAction
        {
            export const workspace = "workspace";
            export const path = "path";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __ClearCollectionAction_Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae";
        export module _TransformItemsAction
        {
            export const metaClass = "metaClass";
            export const runtimeClass = "runtimeClass";
            export const workspace = "workspace";
            export const path = "path";
            export const excludeDescendents = "excludeDescendents";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __TransformItemsAction_Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler";
        export module _EchoAction
        {
            export const shallSuccess = "shallSuccess";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __EchoAction_Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction";
        export module _DocumentOpenAction
        {
            export const filePath = "filePath";
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __DocumentOpenAction_Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74";
        export module _Reports
        {
            export module _SimpleReportAction
            {
                export const path = "path";
                export const configuration = "configuration";
                export const workspaceId = "workspaceId";
                export const filePath = "filePath";
                export const name = "name";
                export const isDisabled = "isDisabled";
            }

            export const __SimpleReportAction_Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction";
            export module _AdocReportAction
            {
                export const filePath = "filePath";
                export const reportInstance = "reportInstance";
                export const name = "name";
                export const isDisabled = "isDisabled";
            }

            export const __AdocReportAction_Uri = "dm:///_internal/types/internal#Actions.AdocReportAction";
            export module _HtmlReportAction
            {
                export const filePath = "filePath";
                export const reportInstance = "reportInstance";
                export const name = "name";
                export const isDisabled = "isDisabled";
            }

            export const __HtmlReportAction_Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction";
        }

        export module _Action
        {
            export const name = "name";
            export const isDisabled = "isDisabled";
        }

        export const __Action_Uri = "dm:///_internal/types/internal#Actions.Action";
    }

    export module _DataViews
    {
        export module _DataView
        {
            export const name = "name";
            export const workspace = "workspace";
            export const uri = "uri";
            export const viewNode = "viewNode";
        }

        export const __DataView_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView";
        export module _ViewNode
        {
            export const name = "name";
        }

        export const __ViewNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode";
        export module _SourceExtentNode
        {
            export const extentUri = "extentUri";
            export const workspace = "workspace";
            export const name = "name";
        }

        export const __SourceExtentNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SourceExtentNode";
        export module _FlattenNode
        {
            export const input = "input";
            export const name = "name";
        }

        export const __FlattenNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode";
        export module _FilterPropertyNode
        {
            export const input = "input";
            export const property = "property";
            export const value = "value";
            export const comparisonMode = "comparisonMode";
            export const name = "name";
        }

        export const __FilterPropertyNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterPropertyNode";
        export module _FilterTypeNode
        {
            export const input = "input";
            export const type = "type";
            export const includeInherits = "includeInherits";
            export const name = "name";
        }

        export const __FilterTypeNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterTypeNode";
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
            LighterOrEqualThan
        }

        export module _SelectByFullNameNode
        {
            export const input = "input";
            export const path = "path";
            export const name = "name";
        }

        export const __SelectByFullNameNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode";
        export module _DynamicSourceNode
        {
            export const nodeName = "nodeName";
            export const name = "name";
        }

        export const __DynamicSourceNode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode";
    }

    export module _Reports
    {
        export module _ReportDefinition
        {
            export const name = "name";
            export const title = "title";
            export const elements = "elements";
        }

        export const __ReportDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition";
        export module _ReportInstanceSource
        {
            export const name = "name";
            export const workspaceId = "workspaceId";
            export const path = "path";
        }

        export const __ReportInstanceSource_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource";
        export module _ReportInstance
        {
            export const name = "name";
            export const reportDefinition = "reportDefinition";
            export const sources = "sources";
        }

        export const __ReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance";
        export module _AdocReportInstance
        {
            export const name = "name";
            export const reportDefinition = "reportDefinition";
            export const sources = "sources";
        }

        export const __AdocReportInstance_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance";
        export module _HtmlReportInstance
        {
            export const name = "name";
            export const reportDefinition = "reportDefinition";
            export const sources = "sources";
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

        export module _SimpleReportConfiguration
        {
            export const name = "name";
            export const showDescendents = "showDescendents";
            export const rootElement = "rootElement";
            export const showRootElement = "showRootElement";
            export const showMetaClasses = "showMetaClasses";
            export const showFullName = "showFullName";
            export const form = "form";
            export const descendentMode = "descendentMode";
            export const typeMode = "typeMode";
            export const workspaceId = "workspaceId";
        }

        export const __SimpleReportConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration";
        export module _Elements
        {
            export module _ReportElement
            {
                export const name = "name";
            }

            export const __ReportElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement";
            export module _ReportHeadline
            {
                export const title = "title";
                export const name = "name";
            }

            export const __ReportHeadline_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline";
            export module _ReportParagraph
            {
                export const paragraph = "paragraph";
                export const cssClass = "cssClass";
                export const viewNode = "viewNode";
                export const evalProperties = "evalProperties";
                export const evalParagraph = "evalParagraph";
                export const name = "name";
            }

            export const __ReportParagraph_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph";
            export module _ReportTable
            {
                export const cssClass = "cssClass";
                export const viewNode = "viewNode";
                export const form = "form";
                export const evalProperties = "evalProperties";
                export const name = "name";
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

            export module _ReportLoop
            {
                export const viewNode = "viewNode";
                export const elements = "elements";
                export const name = "name";
            }

            export const __ReportLoop_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop";
        }

    }

    export module _ExtentLoaderConfigs
    {
        export module _ExtentLoaderConfig
        {
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __ExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig";
        export module _ExcelLoaderConfig
        {
            export const fixRowCount = "fixRowCount";
            export const fixColumnCount = "fixColumnCount";
            export const filePath = "filePath";
            export const sheetName = "sheetName";
            export const offsetRow = "offsetRow";
            export const offsetColumn = "offsetColumn";
            export const countRows = "countRows";
            export const countColumns = "countColumns";
            export const hasHeader = "hasHeader";
            export const tryMergedHeaderCells = "tryMergedHeaderCells";
            export const onlySetColumns = "onlySetColumns";
            export const idColumnName = "idColumnName";
            export const skipEmptyRowsCount = "skipEmptyRowsCount";
            export const columns = "columns";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __ExcelLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig";
        export module _ExcelReferenceLoaderConfig
        {
            export const fixRowCount = "fixRowCount";
            export const fixColumnCount = "fixColumnCount";
            export const filePath = "filePath";
            export const sheetName = "sheetName";
            export const offsetRow = "offsetRow";
            export const offsetColumn = "offsetColumn";
            export const countRows = "countRows";
            export const countColumns = "countColumns";
            export const hasHeader = "hasHeader";
            export const tryMergedHeaderCells = "tryMergedHeaderCells";
            export const onlySetColumns = "onlySetColumns";
            export const idColumnName = "idColumnName";
            export const skipEmptyRowsCount = "skipEmptyRowsCount";
            export const columns = "columns";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __ExcelReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig";
        export module _ExcelImportLoaderConfig
        {
            export const extentPath = "extentPath";
            export const fixRowCount = "fixRowCount";
            export const fixColumnCount = "fixColumnCount";
            export const filePath = "filePath";
            export const sheetName = "sheetName";
            export const offsetRow = "offsetRow";
            export const offsetColumn = "offsetColumn";
            export const countRows = "countRows";
            export const countColumns = "countColumns";
            export const hasHeader = "hasHeader";
            export const tryMergedHeaderCells = "tryMergedHeaderCells";
            export const onlySetColumns = "onlySetColumns";
            export const idColumnName = "idColumnName";
            export const skipEmptyRowsCount = "skipEmptyRowsCount";
            export const columns = "columns";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __ExcelImportLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig";
        export module _ExcelExtentLoaderConfig
        {
            export const filePath = "filePath";
            export const idColumnName = "idColumnName";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __ExcelExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig";
        export module _InMemoryLoaderConfig
        {
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __InMemoryLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig";
        export module _XmlReferenceLoaderConfig
        {
            export const filePath = "filePath";
            export const keepNamespaces = "keepNamespaces";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __XmlReferenceLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig";
        export module _ExtentFileLoaderConfig
        {
            export const filePath = "filePath";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __ExtentFileLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig";
        export module _XmiStorageLoaderConfig
        {
            export const filePath = "filePath";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __XmiStorageLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig";
        export module _CsvExtentLoaderConfig
        {
            export const settings = "settings";
            export const filePath = "filePath";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __CsvExtentLoaderConfig_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig";
        export module _CsvSettings
        {
            export const encoding = "encoding";
            export const hasHeader = "hasHeader";
            export const separator = "separator";
            export const columns = "columns";
            export const metaclassUri = "metaclassUri";
        }

        export const __CsvSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings";
        export module _ExcelHierarchicalColumnDefinition
        {
            export const name = "name";
            export const metaClass = "metaClass";
            export const property = "property";
        }

        export const __ExcelHierarchicalColumnDefinition_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition";
        export module _ExcelHierarchicalLoaderConfig
        {
            export const hierarchicalColumns = "hierarchicalColumns";
            export const skipElementsForLastLevel = "skipElementsForLastLevel";
            export const fixRowCount = "fixRowCount";
            export const fixColumnCount = "fixColumnCount";
            export const filePath = "filePath";
            export const sheetName = "sheetName";
            export const offsetRow = "offsetRow";
            export const offsetColumn = "offsetColumn";
            export const countRows = "countRows";
            export const countColumns = "countColumns";
            export const hasHeader = "hasHeader";
            export const tryMergedHeaderCells = "tryMergedHeaderCells";
            export const onlySetColumns = "onlySetColumns";
            export const idColumnName = "idColumnName";
            export const skipEmptyRowsCount = "skipEmptyRowsCount";
            export const columns = "columns";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __ExcelHierarchicalLoaderConfig_Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig";
        export module _ExcelColumn
        {
            export const header = "header";
            export const name = "name";
        }

        export const __ExcelColumn_Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a";
        export module _EnvironmentalVariableLoaderConfig
        {
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __EnvironmentalVariableLoaderConfig_Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3";
    }

    export module _Forms
    {
        export module _FieldData
        {
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __FieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData";
        export module _SortingOrder
        {
            export const name = "name";
            export const isDescending = "isDescending";
        }

        export const __SortingOrder_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder";
        export module _AnyDataFieldData
        {
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __AnyDataFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData";
        export module _CheckboxFieldData
        {
            export const lineHeight = "lineHeight";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __CheckboxFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData";
        export module _ActionFieldData
        {
            export const actionName = "actionName";
            export const parameter = "parameter";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __ActionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData";
        export module _DateTimeFieldData
        {
            export const hideDate = "hideDate";
            export const hideTime = "hideTime";
            export const showOffsetButtons = "showOffsetButtons";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __DateTimeFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData";
        export module _FormAssociation
        {
            export const name = "name";
            export const formType = "formType";
            export const metaClass = "metaClass";
            export const extentType = "extentType";
            export const viewModeId = "viewModeId";
            export const parentMetaClass = "parentMetaClass";
            export const parentProperty = "parentProperty";
            export const form = "form";
        }

        export const __FormAssociation_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation";
        export module _DropDownFieldData
        {
            export const values = "values";
            export const valuesByEnumeration = "valuesByEnumeration";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __DropDownFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData";
        export module _ValuePair
        {
            export const value = "value";
            export const name = "name";
        }

        export const __ValuePair_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair";
        export module _MetaClassElementFieldData
        {
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __MetaClassElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData";
        export module _ReferenceFieldData
        {
            export const isSelectionInline = "isSelectionInline";
            export const defaultWorkspace = "defaultWorkspace";
            export const defaultItemUri = "defaultItemUri";
            export const showAllChildren = "showAllChildren";
            export const showWorkspaceSelection = "showWorkspaceSelection";
            export const showExtentSelection = "showExtentSelection";
            export const metaClassFilter = "metaClassFilter";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __ReferenceFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData";
        export module _SubElementFieldData
        {
            export const metaClass = "metaClass";
            export const form = "form";
            export const allowOnlyExistingElements = "allowOnlyExistingElements";
            export const defaultTypesForNewElements = "defaultTypesForNewElements";
            export const includeSpecializationsForDefaultTypes = "includeSpecializationsForDefaultTypes";
            export const defaultWorkspaceOfNewElements = "defaultWorkspaceOfNewElements";
            export const defaultExtentOfNewElements = "defaultExtentOfNewElements";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __SubElementFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData";
        export module _TextFieldData
        {
            export const lineHeight = "lineHeight";
            export const width = "width";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __TextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData";
        export module _EvalTextFieldData
        {
            export const evalCellProperties = "evalCellProperties";
            export const lineHeight = "lineHeight";
            export const width = "width";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __EvalTextFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData";
        export module _SeparatorLineFieldData
        {
            export const Height = "Height";
        }

        export const __SeparatorLineFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData";
        export module _FileSelectionFieldData
        {
            export const defaultExtension = "defaultExtension";
            export const isSaving = "isSaving";
            export const initialPathToDirectory = "initialPathToDirectory";
            export const filter = "filter";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __FileSelectionFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData";
        export module _DefaultTypeForNewElement
        {
            export const name = "name";
            export const metaClass = "metaClass";
            export const parentProperty = "parentProperty";
        }

        export const __DefaultTypeForNewElement_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement";
        export module _FullNameFieldData
        {
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __FullNameFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData";
        export module _CheckboxListTaggingFieldData
        {
            export const values = "values";
            export const separator = "separator";
            export const containsFreeText = "containsFreeText";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __CheckboxListTaggingFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData";
        export module _NumberFieldData
        {
            export const format = "format";
            export const isInteger = "isInteger";
            export const isAttached = "isAttached";
            export const name = "name";
            export const title = "title";
            export const isEnumeration = "isEnumeration";
            export const defaultValue = "defaultValue";
            export const isReadOnly = "isReadOnly";
        }

        export const __NumberFieldData_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData";
        export module _FormType
        {
            export const Detail = "Detail";
            export const TreeItemExtent = "TreeItemExtent";
            export const TreeItemDetail = "TreeItemDetail";
            export const ObjectList = "ObjectList";
            export const TreeItemExtentExtension = "TreeItemExtentExtension";
            export const TreeItemDetailExtension = "TreeItemDetailExtension";
        }

        export enum ___FormType
        {
            Detail,
            TreeItemExtent,
            TreeItemDetail,
            ObjectList,
            TreeItemExtentExtension,
            TreeItemDetailExtension
        }

        export module _Form
        {
            export const name = "name";
            export const title = "title";
            export const isReadOnly = "isReadOnly";
            export const hideMetaInformation = "hideMetaInformation";
            export const originalUri = "originalUri";
            export const creationProtocol = "creationProtocol";
        }

        export const __Form_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form";
        export module _DetailForm
        {
            export const buttonApplyText = "buttonApplyText";
            export const allowNewProperties = "allowNewProperties";
            export const defaultWidth = "defaultWidth";
            export const defaultHeight = "defaultHeight";
            export const tab = "tab";
            export const field = "field";
            export const name = "name";
            export const title = "title";
            export const isReadOnly = "isReadOnly";
            export const hideMetaInformation = "hideMetaInformation";
            export const originalUri = "originalUri";
            export const creationProtocol = "creationProtocol";
        }

        export const __DetailForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DetailForm";
        export module _ListForm
        {
            export const property = "property";
            export const metaClass = "metaClass";
            export const includeDescendents = "includeDescendents";
            export const noItemsWithMetaClass = "noItemsWithMetaClass";
            export const inhibitNewItems = "inhibitNewItems";
            export const inhibitDeleteItems = "inhibitDeleteItems";
            export const inhibitEditItems = "inhibitEditItems";
            export const defaultTypesForNewElements = "defaultTypesForNewElements";
            export const fastViewFilters = "fastViewFilters";
            export const field = "field";
            export const sortingOrder = "sortingOrder";
            export const viewNode = "viewNode";
            export const autoGenerateFields = "autoGenerateFields";
            export const duplicatePerType = "duplicatePerType";
            export const name = "name";
            export const title = "title";
            export const isReadOnly = "isReadOnly";
            export const hideMetaInformation = "hideMetaInformation";
            export const originalUri = "originalUri";
            export const creationProtocol = "creationProtocol";
        }

        export const __ListForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ListForm";
        export module _ExtentForm
        {
            export const tab = "tab";
            export const autoTabs = "autoTabs";
            export const name = "name";
            export const title = "title";
            export const isReadOnly = "isReadOnly";
            export const hideMetaInformation = "hideMetaInformation";
            export const originalUri = "originalUri";
            export const creationProtocol = "creationProtocol";
        }

        export const __ExtentForm_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ExtentForm";
        export module _ViewMode
        {
            export const name = "name";
            export const id = "id";
            export const defaultExtentType = "defaultExtentType";
        }

        export const __ViewMode_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode";
    }

    export module _AttachedExtent
    {
        export module _AttachedExtentConfiguration
        {
            export const name = "name";
            export const referencedWorkspace = "referencedWorkspace";
            export const referencedExtent = "referencedExtent";
            export const referenceType = "referenceType";
            export const referenceProperty = "referenceProperty";
        }

        export const __AttachedExtentConfiguration_Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration";
    }

    export module _Management
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

        export module _Extent
        {
            export const name = "name";
            export const uri = "uri";
            export const workspaceId = "workspaceId";
            export const count = "count";
            export const totalCount = "totalCount";
            export const type = "type";
            export const extentType = "extentType";
            export const isModified = "isModified";
            export const alternativeUris = "alternativeUris";
            export const autoEnumerateType = "autoEnumerateType";
            export const state = "state";
            export const failMessage = "failMessage";
            export const properties = "properties";
            export const loadingConfiguration = "loadingConfiguration";
        }

        export const __Extent_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent";
        export module _Workspace
        {
            export const id = "id";
            export const annotation = "annotation";
            export const extents = "extents";
        }

        export const __Workspace_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace";
        export module _CreateNewWorkspaceModel
        {
            export const id = "id";
            export const annotation = "annotation";
        }

        export const __CreateNewWorkspaceModel_Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel";
        export module _ExtentTypeSetting
        {
            export const name = "name";
            export const rootElementMetaClasses = "rootElementMetaClasses";
        }

        export const __ExtentTypeSetting_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting";
        export module _ExtentProperties
        {
            export const name = "name";
            export const uri = "uri";
            export const workspaceId = "workspaceId";
            export const count = "count";
            export const totalCount = "totalCount";
            export const type = "type";
            export const extentType = "extentType";
            export const isModified = "isModified";
            export const alternativeUris = "alternativeUris";
            export const autoEnumerateType = "autoEnumerateType";
            export const state = "state";
            export const failMessage = "failMessage";
            export const properties = "properties";
            export const loadingConfiguration = "loadingConfiguration";
        }

        export const __ExtentProperties_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties";
        export module _ExtentPropertyDefinition
        {
            export const name = "name";
            export const title = "title";
            export const metaClass = "metaClass";
        }

        export const __ExtentPropertyDefinition_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition";
        export module _ExtentSettings
        {
            export const extentTypeSettings = "extentTypeSettings";
            export const propertyDefinitions = "propertyDefinitions";
        }

        export const __ExtentSettings_Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings";
    }

    export module _FastViewFilters
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

        export module _PropertyComparisonFilter
        {
            export const Property = "Property";
            export const ComparisonType = "ComparisonType";
            export const Value = "Value";
        }

        export const __PropertyComparisonFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter";
        export module _PropertyContainsFilter
        {
            export const Property = "Property";
            export const Value = "Value";
        }

        export const __PropertyContainsFilter_Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter";
    }

    export module _DynamicRuntimeProvider
    {
        export module _DynamicRuntimeLoaderConfig
        {
            export const runtimeClass = "runtimeClass";
            export const configuration = "configuration";
            export const name = "name";
            export const extentUri = "extentUri";
            export const workspaceId = "workspaceId";
            export const dropExisting = "dropExisting";
        }

        export const __DynamicRuntimeLoaderConfig_Uri = "dm:///_internal/types/internal#8be3c0ea-ef40-4b4a-a4ea-9262e924d7b8";
        export module _Examples
        {
            export module _NumberProviderSettings
            {
                export const name = "name";
                export const start = "start";
                export const end = "end";
            }

            export const __NumberProviderSettings_Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a";
            export module _NumberRepresentation
            {
                export const binary = "binary";
                export const octal = "octal";
                export const decimal = "decimal";
                export const hexadecimal = "hexadecimal";
            }

            export const __NumberRepresentation_Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation";
        }

    }

}

