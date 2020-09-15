#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Models
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

        public static readonly _CommonTypes TheOne = new _CommonTypes();

    }

    public class _Actions
    {
        public class _Action
        {
            public static string @name = "name";
            public IElement? @_name = null;

        }

        public _Action @Action = new _Action();
        public IElement @__Action = new MofObjectShadow("dm:///_internal/types/internal#Actions.Action");

        public class _ActionSet
        {
            public static string @name = "name";
            public IElement? @_name = null;

            public static string @action = "action";
            public IElement? @_action = null;

        }

        public _ActionSet @ActionSet = new _ActionSet();
        public IElement @__ActionSet = new MofObjectShadow("dm:///_internal/types/internal#Actions.ActionSet");

        public class _LoggingWriterAction
        {
            public static string @message = "message";
            public IElement? @_message = null;

            public static string @name = "name";
            public IElement? @_name = null;

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

        }

        public _PowershellExecutionAction @PowershellExecutionAction = new _PowershellExecutionAction();
        public IElement @__PowershellExecutionAction = new MofObjectShadow("dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb");

        public class _LoadExtentAction
        {
            public static string @configuration = "configuration";
            public IElement? @_configuration = null;

            public static string @name = "name";
            public IElement? @_name = null;

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

        }

        public _CreateWorkspaceAction @CreateWorkspaceAction = new _CreateWorkspaceAction();
        public IElement @__CreateWorkspaceAction = new MofObjectShadow("dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe");

        public class _DropWorkspaceAction
        {
            public static string @workspace = "workspace";
            public IElement? @_workspace = null;

            public static string @name = "name";
            public IElement? @_name = null;

        }

        public _DropWorkspaceAction @DropWorkspaceAction = new _DropWorkspaceAction();
        public IElement @__DropWorkspaceAction = new MofObjectShadow("dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8");

        public static readonly _Actions TheOne = new _Actions();

    }

}
