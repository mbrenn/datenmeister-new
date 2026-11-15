using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Actions.Model;

public class _Actions
{
    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.Action",
        TypeKind = TypeKind.ClassTree)]
    public class _Action
    {
        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _Action @Action = new ();
    public MofObjectShadow @__Action = new ("dm:///intern.types.actions.datenmeister/#Actions.Action");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.ActionSet",
        TypeKind = TypeKind.ClassTree)]
    public class _ActionSet
    {
        public static readonly string @action = "action";
        public IElement? @_action = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _ActionSet @ActionSet = new ();
    public MofObjectShadow @__ActionSet = new ("dm:///intern.types.actions.datenmeister/#Actions.ActionSet");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#899324b1-85dc-40a1-ba95-dec50509040d",
        TypeKind = TypeKind.ClassTree)]
    public class _ActionResult
    {
        public static readonly string @isSuccess = "isSuccess";
        public IElement? @_isSuccess = null;

        public static readonly string @clientActions = "clientActions";
        public IElement? @_clientActions = null;

        public static readonly string @reason = "reason";
        public IElement? @_reason = null;

    }

    public _ActionResult @ActionResult = new ();
    public MofObjectShadow @__ActionResult = new ("dm:///intern.types.actions.datenmeister/#899324b1-85dc-40a1-ba95-dec50509040d");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.LoggingWriterAction",
        TypeKind = TypeKind.ClassTree)]
    public class _LoggingWriterAction
    {
        public static readonly string @message = "message";
        public IElement? @_message = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _LoggingWriterAction @LoggingWriterAction = new ();
    public MofObjectShadow @__LoggingWriterAction = new ("dm:///intern.types.actions.datenmeister/#Actions.LoggingWriterAction");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#241b550d-835a-41ea-a32a-bea5d388c6ee",
        TypeKind = TypeKind.ClassTree)]
    public class _LoadExtentAction
    {
        public static readonly string @configuration = "configuration";
        public IElement? @_configuration = null;

        public static readonly string @dropExisting = "dropExisting";
        public IElement? @_dropExisting = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _LoadExtentAction @LoadExtentAction = new ();
    public MofObjectShadow @__LoadExtentAction = new ("dm:///intern.types.actions.datenmeister/#241b550d-835a-41ea-a32a-bea5d388c6ee");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#c870f6e8-2b70-415c-afaf-b78776b42a09",
        TypeKind = TypeKind.ClassTree)]
    public class _DropExtentAction
    {
        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _DropExtentAction @DropExtentAction = new ();
    public MofObjectShadow @__DropExtentAction = new ("dm:///intern.types.actions.datenmeister/#c870f6e8-2b70-415c-afaf-b78776b42a09");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe",
        TypeKind = TypeKind.ClassTree)]
    public class _CreateWorkspaceAction
    {
        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @annotation = "annotation";
        public IElement? @_annotation = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _CreateWorkspaceAction @CreateWorkspaceAction = new ();
    public MofObjectShadow @__CreateWorkspaceAction = new ("dm:///intern.types.actions.datenmeister/#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#db6cc8eb-011c-43e5-b966-cc0e3a1855e8",
        TypeKind = TypeKind.ClassTree)]
    public class _DropWorkspaceAction
    {
        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _DropWorkspaceAction @DropWorkspaceAction = new ();
    public MofObjectShadow @__DropWorkspaceAction = new ("dm:///intern.types.actions.datenmeister/#db6cc8eb-011c-43e5-b966-cc0e3a1855e8");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#8b576580-0f75-4159-ad16-afb7c2268aed",
        TypeKind = TypeKind.ClassTree)]
    public class _CopyElementsAction
    {
        public static readonly string @sourcePath = "sourcePath";
        public IElement? @_sourcePath = null;

        public static readonly string @targetPath = "targetPath";
        public IElement? @_targetPath = null;

        public static readonly string @moveOnly = "moveOnly";
        public IElement? @_moveOnly = null;

        public static readonly string @sourceWorkspace = "sourceWorkspace";
        public IElement? @_sourceWorkspace = null;

        public static readonly string @targetWorkspace = "targetWorkspace";
        public IElement? @_targetWorkspace = null;

        public static readonly string @emptyTarget = "emptyTarget";
        public IElement? @_emptyTarget = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _CopyElementsAction @CopyElementsAction = new ();
    public MofObjectShadow @__CopyElementsAction = new ("dm:///intern.types.actions.datenmeister/#8b576580-0f75-4159-ad16-afb7c2268aed");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#3c3595a4-026e-4c07-83ec-8a90607b8863",
        TypeKind = TypeKind.ClassTree)]
    public class _ExportToXmiAction
    {
        public static readonly string @sourcePath = "sourcePath";
        public IElement? @_sourcePath = null;

        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @sourceWorkspaceId = "sourceWorkspaceId";
        public IElement? @_sourceWorkspaceId = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _ExportToXmiAction @ExportToXmiAction = new ();
    public MofObjectShadow @__ExportToXmiAction = new ("dm:///intern.types.actions.datenmeister/#3c3595a4-026e-4c07-83ec-8a90607b8863");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#b70b736b-c9b0-4986-8d92-240fcabc95ae",
        TypeKind = TypeKind.ClassTree)]
    public class _ClearCollectionAction
    {
        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @path = "path";
        public IElement? @_path = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _ClearCollectionAction @ClearCollectionAction = new ();
    public MofObjectShadow @__ClearCollectionAction = new ("dm:///intern.types.actions.datenmeister/#b70b736b-c9b0-4986-8d92-240fcabc95ae");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.ItemTransformationActionHandler",
        TypeKind = TypeKind.ClassTree)]
    public class _TransformItemsAction
    {
        public static readonly string @metaClass = "metaClass";
        public IElement? @_metaClass = null;

        public static readonly string @runtimeClass = "runtimeClass";
        public IElement? @_runtimeClass = null;

        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @path = "path";
        public IElement? @_path = null;

        public static readonly string @excludeDescendents = "excludeDescendents";
        public IElement? @_excludeDescendents = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _TransformItemsAction @TransformItemsAction = new ();
    public MofObjectShadow @__TransformItemsAction = new ("dm:///intern.types.actions.datenmeister/#Actions.ItemTransformationActionHandler");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Actions.EchoAction",
        TypeKind = TypeKind.ClassTree)]
    public class _EchoAction
    {
        public static readonly string @shallSuccess = "shallSuccess";
        public IElement? @_shallSuccess = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _EchoAction @EchoAction = new ();
    public MofObjectShadow @__EchoAction = new ("dm:///intern.types.actions.datenmeister/#DatenMeister.Actions.EchoAction");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#04878741-802e-4b7f-8003-21d25f38ac74",
        TypeKind = TypeKind.ClassTree)]
    public class _DocumentOpenAction
    {
        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _DocumentOpenAction @DocumentOpenAction = new ();
    public MofObjectShadow @__DocumentOpenAction = new ("dm:///intern.types.actions.datenmeister/#04878741-802e-4b7f-8003-21d25f38ac74");

    public class _Reports
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.SimpleReportAction",
            TypeKind = TypeKind.ClassTree)]
        public class _SimpleReportAction
        {
            public static readonly string @path = "path";
            public IElement? @_path = null;

            public static readonly string @configuration = "configuration";
            public IElement? @_configuration = null;

            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @filePath = "filePath";
            public IElement? @_filePath = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _SimpleReportAction @SimpleReportAction = new ();
        public MofObjectShadow @__SimpleReportAction = new ("dm:///intern.types.actions.datenmeister/#Actions.SimpleReportAction");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.AdocReportAction",
            TypeKind = TypeKind.ClassTree)]
        public class _AdocReportAction
        {
            public static readonly string @filePath = "filePath";
            public IElement? @_filePath = null;

            public static readonly string @reportInstance = "reportInstance";
            public IElement? @_reportInstance = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _AdocReportAction @AdocReportAction = new ();
        public MofObjectShadow @__AdocReportAction = new ("dm:///intern.types.actions.datenmeister/#Actions.AdocReportAction");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.HtmlReportAction",
            TypeKind = TypeKind.ClassTree)]
        public class _HtmlReportAction
        {
            public static readonly string @filePath = "filePath";
            public IElement? @_filePath = null;

            public static readonly string @reportInstance = "reportInstance";
            public IElement? @_reportInstance = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _HtmlReportAction @HtmlReportAction = new ();
        public MofObjectShadow @__HtmlReportAction = new ("dm:///intern.types.actions.datenmeister/#Actions.HtmlReportAction");

    }

    public _Reports Reports = new ();

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.MoveOrCopyAction",
        TypeKind = TypeKind.ClassTree)]
    public class _MoveOrCopyAction
    {
        public static readonly string @copyMode = "copyMode";
        public IElement? @_copyMode = null;

        public static readonly string @target = "target";
        public IElement? @_target = null;

        public static readonly string @source = "source";
        public IElement? @_source = null;

    }

    public _MoveOrCopyAction @MoveOrCopyAction = new ();
    public MofObjectShadow @__MoveOrCopyAction = new ("dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.MoveOrCopyAction");

    public class _MoveOrCopyType
    {
        public static string @Copy = "Copy";
        public IElement? @__Copy = null;
        public static string @Move = "Move";
        public IElement? @__Move = null;

    }

    public _MoveOrCopyType @MoveOrCopyType = new _MoveOrCopyType();
    public IElement @__MoveOrCopyType = new MofObjectShadow("dm:///intern.types.actions.datenmeister/#bb497efd-5acc-4c00-b996-00375efdb41a");


    public enum ___MoveOrCopyType
    {
        @Copy,
        @Move
    }

    public class _MoveDirectionType
    {
        public static string @Up = "Up";
        public IElement? @__Up = null;
        public static string @Down = "Down";
        public IElement? @__Down = null;

    }

    public _MoveDirectionType @MoveDirectionType = new _MoveDirectionType();
    public IElement @__MoveDirectionType = new MofObjectShadow("dm:///intern.types.actions.datenmeister/#45928e05-c8fc-4f90-a726-ecffebc2a566");


    public enum ___MoveDirectionType
    {
        @Up,
        @Down
    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#bc4952bf-a3f5-4516-be26-5b773e38bd54",
        TypeKind = TypeKind.ClassTree)]
    public class _MoveUpDownAction
    {
        public static readonly string @element = "element";
        public IElement? @_element = null;

        public static readonly string @direction = "direction";
        public IElement? @_direction = null;

        public static readonly string @container = "container";
        public IElement? @_container = null;

        public static readonly string @property = "property";
        public IElement? @_property = null;

    }

    public _MoveUpDownAction @MoveUpDownAction = new ();
    public MofObjectShadow @__MoveUpDownAction = new ("dm:///intern.types.actions.datenmeister/#bc4952bf-a3f5-4516-be26-5b773e38bd54");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#43b0764e-b70f-42bb-b37d-ae8586ec45f1",
        TypeKind = TypeKind.ClassTree)]
    public class _StoreExtentAction
    {
        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @extentUri = "extentUri";
        public IElement? @_extentUri = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _StoreExtentAction @StoreExtentAction = new ();
    public MofObjectShadow @__StoreExtentAction = new ("dm:///intern.types.actions.datenmeister/#43b0764e-b70f-42bb-b37d-ae8586ec45f1");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d",
        TypeKind = TypeKind.ClassTree)]
    public class _ImportXmiAction
    {
        public static readonly string @workspaceId = "workspaceId";
        public IElement? @_workspaceId = null;

        public static readonly string @itemUri = "itemUri";
        public IElement? @_itemUri = null;

        public static readonly string @xmi = "xmi";
        public IElement? @_xmi = null;

        public static readonly string @property = "property";
        public IElement? @_property = null;

        public static readonly string @addToCollection = "addToCollection";
        public IElement? @_addToCollection = null;

    }

    public _ImportXmiAction @ImportXmiAction = new ();
    public MofObjectShadow @__ImportXmiAction = new ("dm:///intern.types.actions.datenmeister/#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#b631bb00-ab11-4a8a-a148-e28abc398503",
        TypeKind = TypeKind.ClassTree)]
    public class _DeletePropertyFromCollectionAction
    {
        public static readonly string @propertyName = "propertyName";
        public IElement? @_propertyName = null;

        public static readonly string @metaclass = "metaclass";
        public IElement? @_metaclass = null;

        public static readonly string @collectionUrl = "collectionUrl";
        public IElement? @_collectionUrl = null;

    }

    public _DeletePropertyFromCollectionAction @DeletePropertyFromCollectionAction = new ();
    public MofObjectShadow @__DeletePropertyFromCollectionAction = new ("dm:///intern.types.actions.datenmeister/#b631bb00-ab11-4a8a-a148-e28abc398503");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#3223e13a-bbb7-4785-8b81-7275be23b0a1",
        TypeKind = TypeKind.ClassTree)]
    public class _TargetReferenceResult
    {
        public static readonly string @targetUrl = "targetUrl";
        public IElement? @_targetUrl = null;

        public static readonly string @targetWorkspace = "targetWorkspace";
        public IElement? @_targetWorkspace = null;

    }

    public _TargetReferenceResult @TargetReferenceResult = new ();
    public MofObjectShadow @__TargetReferenceResult = new ("dm:///intern.types.actions.datenmeister/#3223e13a-bbb7-4785-8b81-7275be23b0a1");

    public class _ParameterTypes
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#90f61e4e-a5ea-42eb-9caa-912d010fbccd",
            TypeKind = TypeKind.ClassTree)]
        public class _NavigationDefineActionParameter
        {
            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @formUrl = "formUrl";
            public IElement? @_formUrl = null;

            public static readonly string @metaClassUrl = "metaClassUrl";
            public IElement? @_metaClassUrl = null;

        }

        public _NavigationDefineActionParameter @NavigationDefineActionParameter = new ();
        public MofObjectShadow @__NavigationDefineActionParameter = new ("dm:///intern.types.actions.datenmeister/#90f61e4e-a5ea-42eb-9caa-912d010fbccd");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#2863f928-fe69-4d35-8c67-f4f3533b7ae5",
            TypeKind = TypeKind.ClassTree)]
        public class _LoadExtentActionResult
        {
            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @extentUri = "extentUri";
            public IElement? @_extentUri = null;

        }

        public _LoadExtentActionResult @LoadExtentActionResult = new ();
        public MofObjectShadow @__LoadExtentActionResult = new ("dm:///intern.types.actions.datenmeister/#2863f928-fe69-4d35-8c67-f4f3533b7ae5");

    }

    public _ParameterTypes ParameterTypes = new ();

    public class _ClientActions
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#e07ca80e-2540-4f91-8214-60dbd464e998",
            TypeKind = TypeKind.ClassTree)]
        public class _ClientAction
        {
            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @element = "element";
            public IElement? @_element = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _ClientAction @ClientAction = new ();
        public MofObjectShadow @__ClientAction = new ("dm:///intern.types.actions.datenmeister/#e07ca80e-2540-4f91-8214-60dbd464e998");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#0ee17f2a-5407-4d38-b1b4-34ead2186971",
            TypeKind = TypeKind.ClassTree)]
        public class _AlertClientAction
        {
            public static readonly string @messageText = "messageText";
            public IElement? @_messageText = null;

            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @element = "element";
            public IElement? @_element = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _AlertClientAction @AlertClientAction = new ();
        public MofObjectShadow @__AlertClientAction = new ("dm:///intern.types.actions.datenmeister/#0ee17f2a-5407-4d38-b1b4-34ead2186971");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#3251783f-2683-4c24-bad5-828930028462",
            TypeKind = TypeKind.ClassTree)]
        public class _NavigateToExtentClientAction
        {
            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @extentUri = "extentUri";
            public IElement? @_extentUri = null;

        }

        public _NavigateToExtentClientAction @NavigateToExtentClientAction = new ();
        public MofObjectShadow @__NavigateToExtentClientAction = new ("dm:///intern.types.actions.datenmeister/#3251783f-2683-4c24-bad5-828930028462");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#5f69675e-df58-4ad7-84bf-359cdfba5db4",
            TypeKind = TypeKind.ClassTree)]
        public class _NavigateToItemClientAction
        {
            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @itemUrl = "itemUrl";
            public IElement? @_itemUrl = null;

            public static readonly string @formUri = "formUri";
            public IElement? @_formUri = null;

        }

        public _NavigateToItemClientAction @NavigateToItemClientAction = new ();
        public MofObjectShadow @__NavigateToItemClientAction = new ("dm:///intern.types.actions.datenmeister/#5f69675e-df58-4ad7-84bf-359cdfba5db4");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#f1b5a13d-14fa-4ae8-8850-0feddf7ba0e5",
            TypeKind = TypeKind.ClassTree)]
        public class _NavigateToUrlClientAction
        {
            public static readonly string @url = "url";
            public IElement? @_url = null;

        }

        public _NavigateToUrlClientAction @NavigateToUrlClientAction = new ();
        public MofObjectShadow @__NavigateToUrlClientAction = new ("dm:///intern.types.actions.datenmeister/#f1b5a13d-14fa-4ae8-8850-0feddf7ba0e5");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#746ecf7c-63d6-4fdf-95ae-837a13a032ed",
            TypeKind = TypeKind.ClassTree)]
        public class _NavigateOpenWindow
        {
            public static readonly string @url = "url";
            public IElement? @_url = null;

            public static readonly string @isAbsoluteUrl = "isAbsoluteUrl";
            public IElement? @_isAbsoluteUrl = null;

            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @element = "element";
            public IElement? @_element = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _NavigateOpenWindow @NavigateOpenWindow = new ();
        public MofObjectShadow @__NavigateOpenWindow = new ("dm:///intern.types.actions.datenmeister/#746ecf7c-63d6-4fdf-95ae-837a13a032ed");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#77ff9c01-0651-4657-823a-ce41bbe0e5e5",
            TypeKind = TypeKind.ClassTree)]
        public class _NavigateOpenActionInWindow
        {
            public static readonly string @context = "context";
            public IElement? @_context = null;

            public static readonly string @actionUrl = "actionUrl";
            public IElement? @_actionUrl = null;

            public static readonly string @workspaceId = "workspaceId";
            public IElement? @_workspaceId = null;

            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @element = "element";
            public IElement? @_element = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _NavigateOpenActionInWindow @NavigateOpenActionInWindow = new ();
        public MofObjectShadow @__NavigateOpenActionInWindow = new ("dm:///intern.types.actions.datenmeister/#77ff9c01-0651-4657-823a-ce41bbe0e5e5");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#ffa8c9f8-9454-423d-882d-a44b98eaf4ea",
            TypeKind = TypeKind.ClassTree)]
        public class _RenderFormClientAction
        {
            public static readonly string @dataWorkspaceId = "dataWorkspaceId";
            public IElement? @_dataWorkspaceId = null;

            public static readonly string @dataUri = "dataUri";
            public IElement? @_dataUri = null;

            public static readonly string @formAutoGenerate = "formAutoGenerate";
            public IElement? @_formAutoGenerate = null;

            public static readonly string @formType = "formType";
            public IElement? @_formType = null;

            public static readonly string @form = "form";
            public IElement? @_form = null;

            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @element = "element";
            public IElement? @_element = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RenderFormClientAction @RenderFormClientAction = new ();
        public MofObjectShadow @__RenderFormClientAction = new ("dm:///intern.types.actions.datenmeister/#ffa8c9f8-9454-423d-882d-a44b98eaf4ea");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#30f7e380-73e1-4ade-b797-1cc948b63c4f",
            TypeKind = TypeKind.ClassTree)]
        public class _RenderHtmlClientAction
        {
            public static readonly string @html = "html";
            public IElement? @_html = null;

            public static readonly string @text = "text";
            public IElement? @_text = null;

            public static readonly string @renderCompletePage = "renderCompletePage";
            public IElement? @_renderCompletePage = null;

            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @element = "element";
            public IElement? @_element = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _RenderHtmlClientAction @RenderHtmlClientAction = new ();
        public MofObjectShadow @__RenderHtmlClientAction = new ("dm:///intern.types.actions.datenmeister/#30f7e380-73e1-4ade-b797-1cc948b63c4f");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#7059ce1e-f299-45c5-b536-a7cb22c7225e",
            TypeKind = TypeKind.ClassTree)]
        public class _DownloadHtmlClientAction
        {
            public static readonly string @html = "html";
            public IElement? @_html = null;

            public static readonly string @filename = "filename";
            public IElement? @_filename = null;

            public static readonly string @actionName = "actionName";
            public IElement? @_actionName = null;

            public static readonly string @element = "element";
            public IElement? @_element = null;

            public static readonly string @parameter = "parameter";
            public IElement? @_parameter = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

        }

        public _DownloadHtmlClientAction @DownloadHtmlClientAction = new ();
        public MofObjectShadow @__DownloadHtmlClientAction = new ("dm:///intern.types.actions.datenmeister/#7059ce1e-f299-45c5-b536-a7cb22c7225e");

    }

    public _ClientActions ClientActions = new ();

    public class _Forms
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.CreateFormByMetaclass",
            TypeKind = TypeKind.ClassTree)]
        public class _CreateFormByMetaClass
        {
            public static readonly string @metaClass = "metaClass";
            public IElement? @_metaClass = null;

            public static readonly string @creationMode = "creationMode";
            public IElement? @_creationMode = null;

            public static readonly string @targetContainer = "targetContainer";
            public IElement? @_targetContainer = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _CreateFormByMetaClass @CreateFormByMetaClass = new ();
        public MofObjectShadow @__CreateFormByMetaClass = new ("dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.CreateFormByMetaclass");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#d9838e98-fd94-4dc1-883a-4362a2d732d3",
            TypeKind = TypeKind.ClassTree)]
        public class _GetAttributeOfItem
        {
            public static readonly string @itemUri = "itemUri";
            public IElement? @_itemUri = null;

            public static readonly string @workspace = "workspace";
            public IElement? @_workspace = null;

            public static readonly string @propertyName = "propertyName";
            public IElement? @_propertyName = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _GetAttributeOfItem @GetAttributeOfItem = new ();
        public MofObjectShadow @__GetAttributeOfItem = new ("dm:///intern.types.actions.datenmeister/#d9838e98-fd94-4dc1-883a-4362a2d732d3");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#e144d177-70e2-4418-991a-8262523c62b7",
            TypeKind = TypeKind.ClassTree)]
        public class _GetAttributeOfItemResult
        {
            public static readonly string @isComposite = "isComposite";
            public IElement? @_isComposite = null;

            public static readonly string @metaClassUri = "metaClassUri";
            public IElement? @_metaClassUri = null;

            public static readonly string @isMultiple = "isMultiple";
            public IElement? @_isMultiple = null;

        }

        public _GetAttributeOfItemResult @GetAttributeOfItemResult = new ();
        public MofObjectShadow @__GetAttributeOfItemResult = new ("dm:///intern.types.actions.datenmeister/#e144d177-70e2-4418-991a-8262523c62b7");

    }

    public _Forms Forms = new ();

    public class _OSIntegration
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#6f2ea2cd-6218-483c-90a3-4db255e84e82",
            TypeKind = TypeKind.ClassTree)]
        public class _CommandExecutionAction
        {
            public static readonly string @command = "command";
            public IElement? @_command = null;

            public static readonly string @arguments = "arguments";
            public IElement? @_arguments = null;

            public static readonly string @workingDirectory = "workingDirectory";
            public IElement? @_workingDirectory = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _CommandExecutionAction @CommandExecutionAction = new ();
        public MofObjectShadow @__CommandExecutionAction = new ("dm:///intern.types.actions.datenmeister/#6f2ea2cd-6218-483c-90a3-4db255e84e82");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#4090ce13-6718-466c-96df-52d51024aadb",
            TypeKind = TypeKind.ClassTree)]
        public class _PowershellExecutionAction
        {
            public static readonly string @script = "script";
            public IElement? @_script = null;

            public static readonly string @workingDirectory = "workingDirectory";
            public IElement? @_workingDirectory = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _PowershellExecutionAction @PowershellExecutionAction = new ();
        public MofObjectShadow @__PowershellExecutionAction = new ("dm:///intern.types.actions.datenmeister/#4090ce13-6718-466c-96df-52d51024aadb");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a",
            TypeKind = TypeKind.ClassTree)]
        public class _ConsoleWriteAction
        {
            public static readonly string @text = "text";
            public IElement? @_text = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _ConsoleWriteAction @ConsoleWriteAction = new ();
        public MofObjectShadow @__ConsoleWriteAction = new ("dm:///intern.types.actions.datenmeister/#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a");

    }

    public _OSIntegration OSIntegration = new ();

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#9d43decb-aa2f-4461-b680-3ec595b518d1",
        TypeKind = TypeKind.ClassTree)]
    public class _RefreshTypeIndexAction
    {
        public static readonly string @waitForRefresh = "waitForRefresh";
        public IElement? @_waitForRefresh = null;

    }

    public _RefreshTypeIndexAction @RefreshTypeIndexAction = new ();
    public MofObjectShadow @__RefreshTypeIndexAction = new ("dm:///intern.types.actions.datenmeister/#9d43decb-aa2f-4461-b680-3ec595b518d1");

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#c7dcb24c-e53c-46f9-9e8a-3704095193a8",
        TypeKind = TypeKind.ClassTree)]
    public class _StoreElementAction
    {
        public static readonly string @workspace = "workspace";
        public IElement? @_workspace = null;

        public static readonly string @url = "url";
        public IElement? @_url = null;

        public static readonly string @element = "element";
        public IElement? @_element = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _StoreElementAction @StoreElementAction = new ();
    public MofObjectShadow @__StoreElementAction = new ("dm:///intern.types.actions.datenmeister/#c7dcb24c-e53c-46f9-9e8a-3704095193a8");

    public class _Data
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#fb1fcea9-c1d5-489d-9bd1-f64c6b71a171",
            TypeKind = TypeKind.ClassTree)]
        public class _FreezeViewResultAction
        {
            public static readonly string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _FreezeViewResultAction @FreezeViewResultAction = new ();
        public MofObjectShadow @__FreezeViewResultAction = new ("dm:///intern.types.actions.datenmeister/#fb1fcea9-c1d5-489d-9bd1-f64c6b71a171");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#1286c17c-3286-4e8c-93fc-1a4c3e6df48a",
            TypeKind = TypeKind.ClassTree)]
        public class _FreezeViewResultInMemory
        {
            public static readonly string @extentUri = "extentUri";
            public IElement? @_extentUri = null;

            public static readonly string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _FreezeViewResultInMemory @FreezeViewResultInMemory = new ();
        public MofObjectShadow @__FreezeViewResultInMemory = new ("dm:///intern.types.actions.datenmeister/#1286c17c-3286-4e8c-93fc-1a4c3e6df48a");

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#45cd9f30-fdfa-4f1b-a87c-24c088c075be",
            TypeKind = TypeKind.ClassTree)]
        public class _FreezeViewResultInExtent
        {
            public static readonly string @extentLoaderConfig = "extentLoaderConfig";
            public IElement? @_extentLoaderConfig = null;

            public static readonly string @viewNode = "viewNode";
            public IElement? @_viewNode = null;

            public static readonly string @name = "name";
            public IElement? @_name = null;

            public static readonly string @isDisabled = "isDisabled";
            public IElement? @_isDisabled = null;

        }

        public _FreezeViewResultInExtent @FreezeViewResultInExtent = new ();
        public MofObjectShadow @__FreezeViewResultInExtent = new ("dm:///intern.types.actions.datenmeister/#45cd9f30-fdfa-4f1b-a87c-24c088c075be");

    }

    public _Data Data = new ();

    public static readonly _Actions TheOne = new ();

}

