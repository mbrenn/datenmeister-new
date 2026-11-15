// Created by DatenMeister.SourcecodeGenerator.TypeScriptInterfaceGenerator Version 1.3.0.0
export namespace _Actions
{
        export class _Action
        {
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __Action_Uri = "dm:///intern.types.actions.datenmeister/#Actions.Action";
        export class _ActionSet
        {
            static action = "action";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __ActionSet_Uri = "dm:///intern.types.actions.datenmeister/#Actions.ActionSet";
        export class _ActionResult
        {
            static isSuccess = "isSuccess";
            static clientActions = "clientActions";
            static reason = "reason";
        }

        export const __ActionResult_Uri = "dm:///intern.types.actions.datenmeister/#899324b1-85dc-40a1-ba95-dec50509040d";
        export class _LoggingWriterAction
        {
            static message = "message";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __LoggingWriterAction_Uri = "dm:///intern.types.actions.datenmeister/#Actions.LoggingWriterAction";
        export class _LoadExtentAction
        {
            static configuration = "configuration";
            static dropExisting = "dropExisting";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __LoadExtentAction_Uri = "dm:///intern.types.actions.datenmeister/#241b550d-835a-41ea-a32a-bea5d388c6ee";
        export class _DropExtentAction
        {
            static workspaceId = "workspaceId";
            static extentUri = "extentUri";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __DropExtentAction_Uri = "dm:///intern.types.actions.datenmeister/#c870f6e8-2b70-415c-afaf-b78776b42a09";
        export class _CreateWorkspaceAction
        {
            static workspaceId = "workspaceId";
            static annotation = "annotation";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __CreateWorkspaceAction_Uri = "dm:///intern.types.actions.datenmeister/#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe";
        export class _DropWorkspaceAction
        {
            static workspaceId = "workspaceId";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __DropWorkspaceAction_Uri = "dm:///intern.types.actions.datenmeister/#db6cc8eb-011c-43e5-b966-cc0e3a1855e8";
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

        export const __CopyElementsAction_Uri = "dm:///intern.types.actions.datenmeister/#8b576580-0f75-4159-ad16-afb7c2268aed";
        export class _ExportToXmiAction
        {
            static sourcePath = "sourcePath";
            static filePath = "filePath";
            static sourceWorkspaceId = "sourceWorkspaceId";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __ExportToXmiAction_Uri = "dm:///intern.types.actions.datenmeister/#3c3595a4-026e-4c07-83ec-8a90607b8863";
        export class _ClearCollectionAction
        {
            static workspaceId = "workspaceId";
            static path = "path";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __ClearCollectionAction_Uri = "dm:///intern.types.actions.datenmeister/#b70b736b-c9b0-4986-8d92-240fcabc95ae";
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

        export const __TransformItemsAction_Uri = "dm:///intern.types.actions.datenmeister/#Actions.ItemTransformationActionHandler";
        export class _EchoAction
        {
            static shallSuccess = "shallSuccess";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __EchoAction_Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Actions.EchoAction";
        export class _DocumentOpenAction
        {
            static filePath = "filePath";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __DocumentOpenAction_Uri = "dm:///intern.types.actions.datenmeister/#04878741-802e-4b7f-8003-21d25f38ac74";
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

                export const __SimpleReportAction_Uri = "dm:///intern.types.actions.datenmeister/#Actions.SimpleReportAction";
                export class _AdocReportAction
                {
                    static filePath = "filePath";
                    static reportInstance = "reportInstance";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __AdocReportAction_Uri = "dm:///intern.types.actions.datenmeister/#Actions.AdocReportAction";
                export class _HtmlReportAction
                {
                    static filePath = "filePath";
                    static reportInstance = "reportInstance";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __HtmlReportAction_Uri = "dm:///intern.types.actions.datenmeister/#Actions.HtmlReportAction";
        }

        export class _MoveOrCopyAction
        {
            static copyMode = "copyMode";
            static target = "target";
            static source = "source";
        }

        export const __MoveOrCopyAction_Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.MoveOrCopyAction";
        export namespace _MoveOrCopyType
        {
            export const Copy = "Copy";
            export const Move = "Move";
        }

        export enum ___MoveOrCopyType
        {
            Copy,
            Move
        }

        export namespace _MoveDirectionType
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

        export const __MoveUpDownAction_Uri = "dm:///intern.types.actions.datenmeister/#bc4952bf-a3f5-4516-be26-5b773e38bd54";
        export class _StoreExtentAction
        {
            static workspaceId = "workspaceId";
            static extentUri = "extentUri";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __StoreExtentAction_Uri = "dm:///intern.types.actions.datenmeister/#43b0764e-b70f-42bb-b37d-ae8586ec45f1";
        export class _ImportXmiAction
        {
            static workspaceId = "workspaceId";
            static itemUri = "itemUri";
            static xmi = "xmi";
            static property = "property";
            static addToCollection = "addToCollection";
        }

        export const __ImportXmiAction_Uri = "dm:///intern.types.actions.datenmeister/#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d";
        export class _DeletePropertyFromCollectionAction
        {
            static propertyName = "propertyName";
            static metaclass = "metaclass";
            static collectionUrl = "collectionUrl";
        }

        export const __DeletePropertyFromCollectionAction_Uri = "dm:///intern.types.actions.datenmeister/#b631bb00-ab11-4a8a-a148-e28abc398503";
        export class _TargetReferenceResult
        {
            static targetUrl = "targetUrl";
            static targetWorkspace = "targetWorkspace";
        }

        export const __TargetReferenceResult_Uri = "dm:///intern.types.actions.datenmeister/#3223e13a-bbb7-4785-8b81-7275be23b0a1";
        export namespace _ParameterTypes
        {
                export class _NavigationDefineActionParameter
                {
                    static actionName = "actionName";
                    static formUrl = "formUrl";
                    static metaClassUrl = "metaClassUrl";
                }

                export const __NavigationDefineActionParameter_Uri = "dm:///intern.types.actions.datenmeister/#90f61e4e-a5ea-42eb-9caa-912d010fbccd";
                export class _LoadExtentActionResult
                {
                    static workspaceId = "workspaceId";
                    static extentUri = "extentUri";
                }

                export const __LoadExtentActionResult_Uri = "dm:///intern.types.actions.datenmeister/#2863f928-fe69-4d35-8c67-f4f3533b7ae5";
        }

        export namespace _ClientActions
        {
                export class _ClientAction
                {
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                    static _name_ = "name";
                }

                export const __ClientAction_Uri = "dm:///intern.types.actions.datenmeister/#e07ca80e-2540-4f91-8214-60dbd464e998";
                export class _AlertClientAction
                {
                    static messageText = "messageText";
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                    static _name_ = "name";
                }

                export const __AlertClientAction_Uri = "dm:///intern.types.actions.datenmeister/#0ee17f2a-5407-4d38-b1b4-34ead2186971";
                export class _NavigateToExtentClientAction
                {
                    static workspaceId = "workspaceId";
                    static extentUri = "extentUri";
                }

                export const __NavigateToExtentClientAction_Uri = "dm:///intern.types.actions.datenmeister/#3251783f-2683-4c24-bad5-828930028462";
                export class _NavigateToItemClientAction
                {
                    static workspaceId = "workspaceId";
                    static itemUrl = "itemUrl";
                    static formUri = "formUri";
                }

                export const __NavigateToItemClientAction_Uri = "dm:///intern.types.actions.datenmeister/#5f69675e-df58-4ad7-84bf-359cdfba5db4";
                export class _NavigateToUrlClientAction
                {
                    static url = "url";
                }

                export const __NavigateToUrlClientAction_Uri = "dm:///intern.types.actions.datenmeister/#f1b5a13d-14fa-4ae8-8850-0feddf7ba0e5";
                export class _NavigateOpenWindow
                {
                    static url = "url";
                    static isAbsoluteUrl = "isAbsoluteUrl";
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                    static _name_ = "name";
                }

                export const __NavigateOpenWindow_Uri = "dm:///intern.types.actions.datenmeister/#746ecf7c-63d6-4fdf-95ae-837a13a032ed";
                export class _NavigateOpenActionInWindow
                {
                    static context = "context";
                    static actionUrl = "actionUrl";
                    static workspaceId = "workspaceId";
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                    static _name_ = "name";
                }

                export const __NavigateOpenActionInWindow_Uri = "dm:///intern.types.actions.datenmeister/#77ff9c01-0651-4657-823a-ce41bbe0e5e5";
                export class _RenderFormClientAction
                {
                    static dataWorkspaceId = "dataWorkspaceId";
                    static dataUri = "dataUri";
                    static formAutoGenerate = "formAutoGenerate";
                    static formType = "formType";
                    static form = "form";
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                    static _name_ = "name";
                }

                export const __RenderFormClientAction_Uri = "dm:///intern.types.actions.datenmeister/#ffa8c9f8-9454-423d-882d-a44b98eaf4ea";
                export class _RenderHtmlClientAction
                {
                    static html = "html";
                    static text = "text";
                    static renderCompletePage = "renderCompletePage";
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                    static _name_ = "name";
                }

                export const __RenderHtmlClientAction_Uri = "dm:///intern.types.actions.datenmeister/#30f7e380-73e1-4ade-b797-1cc948b63c4f";
                export class _DownloadHtmlClientAction
                {
                    static html = "html";
                    static filename = "filename";
                    static actionName = "actionName";
                    static element = "element";
                    static parameter = "parameter";
                    static _name_ = "name";
                }

                export const __DownloadHtmlClientAction_Uri = "dm:///intern.types.actions.datenmeister/#7059ce1e-f299-45c5-b536-a7cb22c7225e";
        }

        export namespace _Forms
        {
                export class _CreateFormByMetaClass
                {
                    static metaClass = "metaClass";
                    static creationMode = "creationMode";
                    static targetContainer = "targetContainer";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __CreateFormByMetaClass_Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.CreateFormByMetaclass";
                export class _GetAttributeOfItem
                {
                    static itemUri = "itemUri";
                    static workspace = "workspace";
                    static propertyName = "propertyName";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __GetAttributeOfItem_Uri = "dm:///intern.types.actions.datenmeister/#d9838e98-fd94-4dc1-883a-4362a2d732d3";
                export class _GetAttributeOfItemResult
                {
                    static isComposite = "isComposite";
                    static metaClassUri = "metaClassUri";
                    static isMultiple = "isMultiple";
                }

                export const __GetAttributeOfItemResult_Uri = "dm:///intern.types.actions.datenmeister/#e144d177-70e2-4418-991a-8262523c62b7";
        }

        export namespace _OSIntegration
        {
                export class _CommandExecutionAction
                {
                    static command = "command";
                    static _arguments_ = "arguments";
                    static workingDirectory = "workingDirectory";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __CommandExecutionAction_Uri = "dm:///intern.types.actions.datenmeister/#6f2ea2cd-6218-483c-90a3-4db255e84e82";
                export class _PowershellExecutionAction
                {
                    static script = "script";
                    static workingDirectory = "workingDirectory";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __PowershellExecutionAction_Uri = "dm:///intern.types.actions.datenmeister/#4090ce13-6718-466c-96df-52d51024aadb";
                export class _ConsoleWriteAction
                {
                    static text = "text";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __ConsoleWriteAction_Uri = "dm:///intern.types.actions.datenmeister/#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a";
        }

        export class _RefreshTypeIndexAction
        {
            static waitForRefresh = "waitForRefresh";
        }

        export const __RefreshTypeIndexAction_Uri = "dm:///intern.types.actions.datenmeister/#9d43decb-aa2f-4461-b680-3ec595b518d1";
        export class _StoreElementAction
        {
            static workspace = "workspace";
            static url = "url";
            static element = "element";
            static _name_ = "name";
            static isDisabled = "isDisabled";
        }

        export const __StoreElementAction_Uri = "dm:///intern.types.actions.datenmeister/#c7dcb24c-e53c-46f9-9e8a-3704095193a8";
        export namespace _Data
        {
                export class _FreezeViewResultAction
                {
                    static viewNode = "viewNode";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __FreezeViewResultAction_Uri = "dm:///intern.types.actions.datenmeister/#fb1fcea9-c1d5-489d-9bd1-f64c6b71a171";
                export class _FreezeViewResultInMemory
                {
                    static extentUri = "extentUri";
                    static viewNode = "viewNode";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __FreezeViewResultInMemory_Uri = "dm:///intern.types.actions.datenmeister/#1286c17c-3286-4e8c-93fc-1a4c3e6df48a";
                export class _FreezeViewResultInExtent
                {
                    static extentLoaderConfig = "extentLoaderConfig";
                    static viewNode = "viewNode";
                    static _name_ = "name";
                    static isDisabled = "isDisabled";
                }

                export const __FreezeViewResultInExtent_Uri = "dm:///intern.types.actions.datenmeister/#45cd9f30-fdfa-4f1b-a87c-24c088c075be";
        }

}

