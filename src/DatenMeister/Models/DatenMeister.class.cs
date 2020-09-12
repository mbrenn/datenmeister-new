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
                public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#1f901e57-0e10-488e-9ab6-c58e21f38f2a");

                public static string @packagedElement = "packagedElement";
                public IElement? _packagedElement = new MofObjectShadow("dm:///_internal/types/internal#f59bd4b9-ea44-4dc4-bee9-ba5274ccd299");

                public static string @preferredType = "preferredType";
                public IElement? _preferredType = new MofObjectShadow("dm:///_internal/types/internal#92047310-19b3-4177-9fab-1ffcba5f9dd0");

                public static string @preferredPackage = "preferredPackage";
                public IElement? _preferredPackage = new MofObjectShadow("dm:///_internal/types/internal#eb9214a4-ddc6-4d78-b866-78cf88ec1115");

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
                public IElement? _filePath = new MofObjectShadow("dm:///_internal/types/internal#07f34b89-8ccc-4aea-a003-2c3a75b4a3b7");

                public static string @extentUri = "extentUri";
                public IElement? _extentUri = new MofObjectShadow("dm:///_internal/types/internal#19a9d20f-710f-4865-99af-daf571f9cdcb");

                public static string @workspace = "workspace";
                public IElement? _workspace = new MofObjectShadow("dm:///_internal/types/internal#419416e6-ac3f-493f-a7bc-4cc0fde3fed1");

            }

            public _ImportSettings @ImportSettings = new _ImportSettings();
            public IElement @__ImportSettings = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings");

            public class _ImportException
            {
                public static string @message = "message";
                public IElement? _message = new MofObjectShadow("dm:///_internal/types/internal#da1d7009-925f-438a-96ac-eecb9e8a4f4c");

            }

            public _ImportException @ImportException = new _ImportException();
            public IElement @__ImportException = new MofObjectShadow("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException");

        }

        public _ExtentManager ExtentManager = new _ExtentManager();

        public static _CommonTypes TheOne = new _CommonTypes();

    }

    public class _Actions
    {
        public class _Action
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#df67302d-ae6c-4842-b1f2-ab2129b0fb7e");

        }

        public _Action @Action = new _Action();
        public IElement @__Action = new MofObjectShadow("dm:///_internal/types/internal#Actions.Action");

        public class _ActionSet
        {
            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#431b1911-9660-48ba-a014-e4c44a74b2a6");

            public static string @action = "action";
            public IElement? _action = new MofObjectShadow("dm:///_internal/types/internal#d688f145-d02e-4052-be12-0d06cec9853b");

        }

        public _ActionSet @ActionSet = new _ActionSet();
        public IElement @__ActionSet = new MofObjectShadow("dm:///_internal/types/internal#Actions.ActionSet");

        public class _LoggingWriterAction
        {
            public static string @message = "message";
            public IElement? _message = new MofObjectShadow("dm:///_internal/types/internal#5506180f-2158-423f-a290-ef929e4f956c");

            public static string @name = "name";
            public IElement? _name = new MofObjectShadow("dm:///_internal/types/internal#df67302d-ae6c-4842-b1f2-ab2129b0fb7e");

        }

        public _LoggingWriterAction @LoggingWriterAction = new _LoggingWriterAction();
        public IElement @__LoggingWriterAction = new MofObjectShadow("dm:///_internal/types/internal#Actions.LoggingWriterAction");

        public static _Actions TheOne = new _Actions();

    }

}
