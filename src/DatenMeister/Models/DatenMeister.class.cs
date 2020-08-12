#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Provider.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Model
{
    public class _CommonTypes
    {
        public class _Default
        {
            public class _Package
            {
                public static string @name = "name";
                public IElement? _name = null;

                public static string @packagedElement = "packagedElement";
                public IElement? _packagedElement = null;

            }

            public _Package @Package = new _Package();
            public IElement @__Package = new MofObjectShadow("dm:///datenmeister.types#DatenMeister.Models.DefaultTypes.Package");

        }

        public _Default Default = new _Default();

        public class _ExtentManager
        {
            public class _ImportSettings
            {
                public static string @filePath = "filePath";
                public IElement? _filePath = null;

                public static string @extentUri = "extentUri";
                public IElement? _extentUri = null;

                public static string @workspace = "workspace";
                public IElement? _workspace = null;

            }

            public _ImportSettings @ImportSettings = new _ImportSettings();
            public IElement @__ImportSettings = new MofObjectShadow("dm:///datenmeister.types#DatenMeister.Models.ExtentManager.ImportSettings");

            public class _ImportException
            {
                public static string @message = "message";
                public IElement? _message = null;

            }

            public _ImportException @ImportException = new _ImportException();
            public IElement @__ImportException = new MofObjectShadow("dm:///datenmeister.types#DatenMeister.Models.ExtentManager.ImportException");

        }

        public _ExtentManager ExtentManager = new _ExtentManager();

        public static _CommonTypes TheOne = new _CommonTypes();

    }

}
