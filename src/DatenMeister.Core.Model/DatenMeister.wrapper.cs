using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Core.Models;

public class CommonTypes
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DateTime",
        TypeKind = TypeKind.WrappedClass)]
    public class DateTime_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DateTime_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DateTime_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DateTime");

        public static DateTime_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    public class Default
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package",
            TypeKind = TypeKind.WrappedClass)]
        public class Package_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public Package_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public Package_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package");

            public static Package_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            // Not found
            public object? @packagedElement
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("packagedElement");
                set => 
                    _wrappedElement.set("packagedElement", value);
            }

            // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @preferredType
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("preferredType");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("preferredType", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("preferredType", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper? @preferredPackage
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("preferredPackage");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("preferredPackage", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("preferredPackage", value);
                    }
                }
            }

            public string? @defaultViewMode
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("defaultViewMode");
                set => 
                    _wrappedElement.set("defaultViewMode", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1",
            TypeKind = TypeKind.WrappedClass)]
        public class XmiExportContainer_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public XmiExportContainer_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public XmiExportContainer_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1");

            public static XmiExportContainer_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @xmi
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("xmi");
                set => 
                    _wrappedElement.set("xmi", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a",
            TypeKind = TypeKind.WrappedClass)]
        public class XmiImportContainer_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public XmiImportContainer_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public XmiImportContainer_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a");

            public static XmiImportContainer_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @xmi
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("xmi");
                set => 
                    _wrappedElement.set("xmi", value);
            }

            public string? @property
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("property");
                set => 
                    _wrappedElement.set("property", value);
            }

            public bool @addToCollection
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("addToCollection");
                set => 
                    _wrappedElement.set("addToCollection", value);
            }

        }

    }

    public class ExtentManager
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings",
            TypeKind = TypeKind.WrappedClass)]
        public class ImportSettings_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ImportSettings_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ImportSettings_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings");

            public static ImportSettings_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @filePath
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("filePath");
                set => 
                    _wrappedElement.set("filePath", value);
            }

            public string? @extentUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("extentUri");
                set => 
                    _wrappedElement.set("extentUri", value);
            }

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException",
            TypeKind = TypeKind.WrappedClass)]
        public class ImportException_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ImportException_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ImportException_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException");

            public static ImportException_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @message
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("message");
                set => 
                    _wrappedElement.set("message", value);
            }

        }

    }

    public class OSIntegration
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication",
            TypeKind = TypeKind.WrappedClass)]
        public class CommandLineApplication_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public CommandLineApplication_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public CommandLineApplication_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication");

            public static CommandLineApplication_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public string? @applicationPath
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("applicationPath");
                set => 
                    _wrappedElement.set("applicationPath", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable",
            TypeKind = TypeKind.WrappedClass)]
        public class EnvironmentalVariable_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public EnvironmentalVariable_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public EnvironmentalVariable_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable");

            public static EnvironmentalVariable_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public string? @value
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("value");
                set => 
                    _wrappedElement.set("value", value);
            }

        }

    }

}

public class Actions
{
    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.ActionSet",
        TypeKind = TypeKind.WrappedClass)]
    public class ActionSet_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ActionSet_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ActionSet_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#Actions.ActionSet");

        public static ActionSet_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.Actions.Action_Wrapper
        public DatenMeister.Core.Models.Actions.Action_Wrapper? @action
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("action");
                return foundElement == null ? null : new DatenMeister.Core.Models.Actions.Action_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("action", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("action", value);
                }
            }
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction",
        TypeKind = TypeKind.WrappedClass)]
    public class LoggingWriterAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public LoggingWriterAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public LoggingWriterAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#Actions.LoggingWriterAction");

        public static LoggingWriterAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @message
        {
            get =>
                _wrappedElement.getOrDefault<string?>("message");
            set => 
                _wrappedElement.set("message", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee",
        TypeKind = TypeKind.WrappedClass)]
    public class LoadExtentAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public LoadExtentAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public LoadExtentAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee");

        public static LoadExtentAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @configuration
        {
            get =>
                _wrappedElement.getOrDefault<object?>("configuration");
            set => 
                _wrappedElement.set("configuration", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09",
        TypeKind = TypeKind.WrappedClass)]
    public class DropExtentAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DropExtentAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DropExtentAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09");

        public static DropExtentAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe",
        TypeKind = TypeKind.WrappedClass)]
    public class CreateWorkspaceAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CreateWorkspaceAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CreateWorkspaceAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe");

        public static CreateWorkspaceAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @annotation
        {
            get =>
                _wrappedElement.getOrDefault<string?>("annotation");
            set => 
                _wrappedElement.set("annotation", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8",
        TypeKind = TypeKind.WrappedClass)]
    public class DropWorkspaceAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DropWorkspaceAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DropWorkspaceAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8");

        public static DropWorkspaceAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed",
        TypeKind = TypeKind.WrappedClass)]
    public class CopyElementsAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CopyElementsAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CopyElementsAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed");

        public static CopyElementsAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @sourcePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sourcePath");
            set => 
                _wrappedElement.set("sourcePath", value);
        }

        public string? @targetPath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("targetPath");
            set => 
                _wrappedElement.set("targetPath", value);
        }

        public bool @moveOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("moveOnly");
            set => 
                _wrappedElement.set("moveOnly", value);
        }

        public string? @sourceWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sourceWorkspace");
            set => 
                _wrappedElement.set("sourceWorkspace", value);
        }

        public string? @targetWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("targetWorkspace");
            set => 
                _wrappedElement.set("targetWorkspace", value);
        }

        public bool @emptyTarget
        {
            get =>
                _wrappedElement.getOrDefault<bool>("emptyTarget");
            set => 
                _wrappedElement.set("emptyTarget", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863",
        TypeKind = TypeKind.WrappedClass)]
    public class ExportToXmiAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExportToXmiAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExportToXmiAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863");

        public static ExportToXmiAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @sourcePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sourcePath");
            set => 
                _wrappedElement.set("sourcePath", value);
        }

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @sourceWorkspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sourceWorkspaceId");
            set => 
                _wrappedElement.set("sourceWorkspaceId", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae",
        TypeKind = TypeKind.WrappedClass)]
    public class ClearCollectionAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ClearCollectionAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ClearCollectionAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae");

        public static ClearCollectionAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @path
        {
            get =>
                _wrappedElement.getOrDefault<string?>("path");
            set => 
                _wrappedElement.set("path", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler",
        TypeKind = TypeKind.WrappedClass)]
    public class TransformItemsAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public TransformItemsAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public TransformItemsAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#Actions.ItemTransformationActionHandler");

        public static TransformItemsAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaClass
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("metaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("metaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("metaClass", value);
                }
            }
        }

        public string? @runtimeClass
        {
            get =>
                _wrappedElement.getOrDefault<string?>("runtimeClass");
            set => 
                _wrappedElement.set("runtimeClass", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @path
        {
            get =>
                _wrappedElement.getOrDefault<string?>("path");
            set => 
                _wrappedElement.set("path", value);
        }

        public bool @excludeDescendents
        {
            get =>
                _wrappedElement.getOrDefault<bool>("excludeDescendents");
            set => 
                _wrappedElement.set("excludeDescendents", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction",
        TypeKind = TypeKind.WrappedClass)]
    public class EchoAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public EchoAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public EchoAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Actions.EchoAction");

        public static EchoAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @shallSuccess
        {
            get =>
                _wrappedElement.getOrDefault<string?>("shallSuccess");
            set => 
                _wrappedElement.set("shallSuccess", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74",
        TypeKind = TypeKind.WrappedClass)]
    public class DocumentOpenAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DocumentOpenAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DocumentOpenAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74");

        public static DocumentOpenAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    public class Reports
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction",
            TypeKind = TypeKind.WrappedClass)]
        public class SimpleReportAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public SimpleReportAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public SimpleReportAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#Actions.SimpleReportAction");

            public static SimpleReportAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @path
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("path");
                set => 
                    _wrappedElement.set("path", value);
            }

            // DatenMeister.Core.Models.Reports.SimpleReportConfiguration_Wrapper
            public DatenMeister.Core.Models.Reports.SimpleReportConfiguration_Wrapper? @configuration
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("configuration");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.SimpleReportConfiguration_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("configuration", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("configuration", value);
                    }
                }
            }

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @filePath
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("filePath");
                set => 
                    _wrappedElement.set("filePath", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#Actions.AdocReportAction",
            TypeKind = TypeKind.WrappedClass)]
        public class AdocReportAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public AdocReportAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public AdocReportAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#Actions.AdocReportAction");

            public static AdocReportAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @filePath
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("filePath");
                set => 
                    _wrappedElement.set("filePath", value);
            }

            // DatenMeister.Core.Models.Reports.AdocReportInstance_Wrapper
            public DatenMeister.Core.Models.Reports.AdocReportInstance_Wrapper? @reportInstance
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("reportInstance");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.AdocReportInstance_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("reportInstance", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("reportInstance", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction",
            TypeKind = TypeKind.WrappedClass)]
        public class HtmlReportAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public HtmlReportAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public HtmlReportAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#Actions.HtmlReportAction");

            public static HtmlReportAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @filePath
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("filePath");
                set => 
                    _wrappedElement.set("filePath", value);
            }

            // DatenMeister.Core.Models.Reports.HtmlReportInstance_Wrapper
            public DatenMeister.Core.Models.Reports.HtmlReportInstance_Wrapper? @reportInstance
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("reportInstance");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.HtmlReportInstance_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("reportInstance", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("reportInstance", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.Action",
        TypeKind = TypeKind.WrappedClass)]
    public class Action_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Action_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Action_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#Actions.Action");

        public static Action_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.MoveOrCopyAction",
        TypeKind = TypeKind.WrappedClass)]
    public class MoveOrCopyAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public MoveOrCopyAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public MoveOrCopyAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Actions.MoveOrCopyAction");

        public static MoveOrCopyAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @copyMode
        {
            get =>
                _wrappedElement.getOrDefault<string?>("copyMode");
            set => 
                _wrappedElement.set("copyMode", value);
        }

        // Not found
        public object? @target
        {
            get =>
                _wrappedElement.getOrDefault<object?>("target");
            set => 
                _wrappedElement.set("target", value);
        }

        // Not found
        public object? @source
        {
            get =>
                _wrappedElement.getOrDefault<object?>("source");
            set => 
                _wrappedElement.set("source", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#bc4952bf-a3f5-4516-be26-5b773e38bd54",
        TypeKind = TypeKind.WrappedClass)]
    public class MoveUpDownAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public MoveUpDownAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public MoveUpDownAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#bc4952bf-a3f5-4516-be26-5b773e38bd54");

        public static MoveUpDownAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @element
        {
            get =>
                _wrappedElement.getOrDefault<object?>("element");
            set => 
                _wrappedElement.set("element", value);
        }

        // Not found
        public object? @direction
        {
            get =>
                _wrappedElement.getOrDefault<object?>("direction");
            set => 
                _wrappedElement.set("direction", value);
        }

        // Not found
        public object? @container
        {
            get =>
                _wrappedElement.getOrDefault<object?>("container");
            set => 
                _wrappedElement.set("container", value);
        }

        public string? @property
        {
            get =>
                _wrappedElement.getOrDefault<string?>("property");
            set => 
                _wrappedElement.set("property", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#43b0764e-b70f-42bb-b37d-ae8586ec45f1",
        TypeKind = TypeKind.WrappedClass)]
    public class StoreExtentAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public StoreExtentAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public StoreExtentAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#43b0764e-b70f-42bb-b37d-ae8586ec45f1");

        public static StoreExtentAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d",
        TypeKind = TypeKind.WrappedClass)]
    public class ImportXmiAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ImportXmiAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ImportXmiAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d");

        public static ImportXmiAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @itemUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("itemUri");
            set => 
                _wrappedElement.set("itemUri", value);
        }

        public string? @xmi
        {
            get =>
                _wrappedElement.getOrDefault<string?>("xmi");
            set => 
                _wrappedElement.set("xmi", value);
        }

        public string? @property
        {
            get =>
                _wrappedElement.getOrDefault<string?>("property");
            set => 
                _wrappedElement.set("property", value);
        }

        public bool @addToCollection
        {
            get =>
                _wrappedElement.getOrDefault<bool>("addToCollection");
            set => 
                _wrappedElement.set("addToCollection", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#b631bb00-ab11-4a8a-a148-e28abc398503",
        TypeKind = TypeKind.WrappedClass)]
    public class DeletePropertyFromCollectionAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DeletePropertyFromCollectionAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DeletePropertyFromCollectionAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#b631bb00-ab11-4a8a-a148-e28abc398503");

        public static DeletePropertyFromCollectionAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @propertyName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("propertyName");
            set => 
                _wrappedElement.set("propertyName", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaclass
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("metaclass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("metaclass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("metaclass", value);
                }
            }
        }

        public string? @collectionUrl
        {
            get =>
                _wrappedElement.getOrDefault<string?>("collectionUrl");
            set => 
                _wrappedElement.set("collectionUrl", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#3223e13a-bbb7-4785-8b81-7275be23b0a1",
        TypeKind = TypeKind.WrappedClass)]
    public class MoveOrCopyActionResult_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public MoveOrCopyActionResult_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public MoveOrCopyActionResult_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#3223e13a-bbb7-4785-8b81-7275be23b0a1");

        public static MoveOrCopyActionResult_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @targetUrl
        {
            get =>
                _wrappedElement.getOrDefault<string?>("targetUrl");
            set => 
                _wrappedElement.set("targetUrl", value);
        }

        public string? @targetWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("targetWorkspace");
            set => 
                _wrappedElement.set("targetWorkspace", value);
        }

    }

    public class ParameterTypes
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#90f61e4e-a5ea-42eb-9caa-912d010fbccd",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigationDefineActionParameter_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NavigationDefineActionParameter_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NavigationDefineActionParameter_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#90f61e4e-a5ea-42eb-9caa-912d010fbccd");

            public static NavigationDefineActionParameter_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @actionName
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("actionName");
                set => 
                    _wrappedElement.set("actionName", value);
            }

            public string? @formUrl
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("formUrl");
                set => 
                    _wrappedElement.set("formUrl", value);
            }

            public string? @metaClassUrl
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("metaClassUrl");
                set => 
                    _wrappedElement.set("metaClassUrl", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#2863f928-fe69-4d35-8c67-f4f3533b7ae5",
            TypeKind = TypeKind.WrappedClass)]
        public class LoadExtentActionResult_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public LoadExtentActionResult_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public LoadExtentActionResult_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#2863f928-fe69-4d35-8c67-f4f3533b7ae5");

            public static LoadExtentActionResult_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @extentUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("extentUri");
                set => 
                    _wrappedElement.set("extentUri", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#124e202d-e8b3-4d39-bbc2-4c95896e811b",
            TypeKind = TypeKind.WrappedClass)]
        public class CreateFormUponViewResult_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public CreateFormUponViewResult_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public CreateFormUponViewResult_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#124e202d-e8b3-4d39-bbc2-4c95896e811b");

            public static CreateFormUponViewResult_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @resultingPackageUrl
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("resultingPackageUrl");
                set => 
                    _wrappedElement.set("resultingPackageUrl", value);
            }

        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#899324b1-85dc-40a1-ba95-dec50509040d",
        TypeKind = TypeKind.WrappedClass)]
    public class ActionResult_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ActionResult_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ActionResult_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#899324b1-85dc-40a1-ba95-dec50509040d");

        public static ActionResult_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @isSuccess
        {
            get =>
                _wrappedElement.getOrDefault<object?>("isSuccess");
            set => 
                _wrappedElement.set("isSuccess", value);
        }

        // Not found
        public object? @clientActions
        {
            get =>
                _wrappedElement.getOrDefault<object?>("clientActions");
            set => 
                _wrappedElement.set("clientActions", value);
        }

    }

    public class ClientActions
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#e07ca80e-2540-4f91-8214-60dbd464e998",
            TypeKind = TypeKind.WrappedClass)]
        public class ClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#e07ca80e-2540-4f91-8214-60dbd464e998");

            public static ClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @actionName
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("actionName");
                set => 
                    _wrappedElement.set("actionName", value);
            }

            // Not found
            public object? @element
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("element");
                set => 
                    _wrappedElement.set("element", value);
            }

            // Not found
            public object? @parameter
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("parameter");
                set => 
                    _wrappedElement.set("parameter", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#0ee17f2a-5407-4d38-b1b4-34ead2186971",
            TypeKind = TypeKind.WrappedClass)]
        public class AlertClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public AlertClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public AlertClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#0ee17f2a-5407-4d38-b1b4-34ead2186971");

            public static AlertClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @messageText
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("messageText");
                set => 
                    _wrappedElement.set("messageText", value);
            }

            public string? @actionName
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("actionName");
                set => 
                    _wrappedElement.set("actionName", value);
            }

            // Not found
            public object? @element
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("element");
                set => 
                    _wrappedElement.set("element", value);
            }

            // Not found
            public object? @parameter
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("parameter");
                set => 
                    _wrappedElement.set("parameter", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#3251783f-2683-4c24-bad5-828930028462",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigateToExtentClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NavigateToExtentClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NavigateToExtentClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#3251783f-2683-4c24-bad5-828930028462");

            public static NavigateToExtentClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @extentUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("extentUri");
                set => 
                    _wrappedElement.set("extentUri", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#5f69675e-df58-4ad7-84bf-359cdfba5db4",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigateToItemClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NavigateToItemClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NavigateToItemClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#5f69675e-df58-4ad7-84bf-359cdfba5db4");

            public static NavigateToItemClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @itemUrl
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("itemUrl");
                set => 
                    _wrappedElement.set("itemUrl", value);
            }

            public string? @formUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("formUri");
                set => 
                    _wrappedElement.set("formUri", value);
            }

        }

    }

    public class Forms
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#b8333b8d-ac49-4a4e-a7f4-c3745e0a0237",
            TypeKind = TypeKind.WrappedClass)]
        public class CreateFormUponViewAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public CreateFormUponViewAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public CreateFormUponViewAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#b8333b8d-ac49-4a4e-a7f4-c3745e0a0237");

            public static CreateFormUponViewAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper
            public DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper? @query
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("query");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("query", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("query", value);
                    }
                }
            }

            public string? @targetPackageUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("targetPackageUri");
                set => 
                    _wrappedElement.set("targetPackageUri", value);
            }

            public string? @targetPackageWorkspace
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("targetPackageWorkspace");
                set => 
                    _wrappedElement.set("targetPackageWorkspace", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.CreateFormByMetaclass",
            TypeKind = TypeKind.WrappedClass)]
        public class CreateFormByMetaClass_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public CreateFormByMetaClass_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public CreateFormByMetaClass_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Actions.CreateFormByMetaclass");

            public static CreateFormByMetaClass_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // Not found
            public object? @metaClass
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("metaClass");
                set => 
                    _wrappedElement.set("metaClass", value);
            }

            public string? @creationMode
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("creationMode");
                set => 
                    _wrappedElement.set("creationMode", value);
            }

            // Not found
            public object? @targetContainer
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("targetContainer");
                set => 
                    _wrappedElement.set("targetContainer", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

    }

    public class OSIntegration
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82",
            TypeKind = TypeKind.WrappedClass)]
        public class CommandExecutionAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public CommandExecutionAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public CommandExecutionAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82");

            public static CommandExecutionAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @command
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("command");
                set => 
                    _wrappedElement.set("command", value);
            }

            public string? @arguments
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("arguments");
                set => 
                    _wrappedElement.set("arguments", value);
            }

            public string? @workingDirectory
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workingDirectory");
                set => 
                    _wrappedElement.set("workingDirectory", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb",
            TypeKind = TypeKind.WrappedClass)]
        public class PowershellExecutionAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public PowershellExecutionAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public PowershellExecutionAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb");

            public static PowershellExecutionAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @script
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("script");
                set => 
                    _wrappedElement.set("script", value);
            }

            public string? @workingDirectory
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workingDirectory");
                set => 
                    _wrappedElement.set("workingDirectory", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a",
            TypeKind = TypeKind.WrappedClass)]
        public class ConsoleWriteAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ConsoleWriteAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ConsoleWriteAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a");

            public static ConsoleWriteAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @text
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("text");
                set => 
                    _wrappedElement.set("text", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isDisabled");
                set => 
                    _wrappedElement.set("isDisabled", value);
            }

        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#9d43decb-aa2f-4461-b680-3ec595b518d1",
        TypeKind = TypeKind.WrappedClass)]
    public class RefreshTypeIndexAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public RefreshTypeIndexAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public RefreshTypeIndexAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#9d43decb-aa2f-4461-b680-3ec595b518d1");

        public static RefreshTypeIndexAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @waitForRefresh
        {
            get =>
                _wrappedElement.getOrDefault<bool>("waitForRefresh");
            set => 
                _wrappedElement.set("waitForRefresh", value);
        }

    }

}

public class DataViews
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView",
        TypeKind = TypeKind.WrappedClass)]
    public class DataView_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DataView_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DataView_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView");

        public static DataView_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @uri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("uri");
            set => 
                _wrappedElement.set("uri", value);
        }

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @viewNode
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("viewNode");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("viewNode", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("viewNode", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode",
        TypeKind = TypeKind.WrappedClass)]
    public class ViewNode_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ViewNode_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ViewNode_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode");

        public static ViewNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement",
        TypeKind = TypeKind.WrappedClass)]
    public class QueryStatement_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public QueryStatement_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public QueryStatement_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement");

        public static QueryStatement_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @nodes
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("nodes");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("nodes", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("nodes", value);
                }
            }
        }

        // Not found
        public object? @resultNode
        {
            get =>
                _wrappedElement.getOrDefault<object?>("resultNode");
            set => 
                _wrappedElement.set("resultNode", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

    }

    public class Row
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#5f66ff9a-0a68-4c87-856b-5921c7cae628",
            TypeKind = TypeKind.WrappedClass)]
        public class RowFilterByFreeTextAnywhere_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RowFilterByFreeTextAnywhere_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RowFilterByFreeTextAnywhere_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#5f66ff9a-0a68-4c87-856b-5921c7cae628");

            public static RowFilterByFreeTextAnywhere_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @freeText
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("freeText");
                set => 
                    _wrappedElement.set("freeText", value);
            }

            // Not found
            public object? @input
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("input");
                set => 
                    _wrappedElement.set("input", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode",
            TypeKind = TypeKind.WrappedClass)]
        public class RowFilterByPropertyValueNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RowFilterByPropertyValueNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RowFilterByPropertyValueNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode");

            public static RowFilterByPropertyValueNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            public string? @property
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("property");
                set => 
                    _wrappedElement.set("property", value);
            }

            public string? @value
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("value");
                set => 
                    _wrappedElement.set("value", value);
            }

            // Not found
            public object? @comparisonMode
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("comparisonMode");
                set => 
                    _wrappedElement.set("comparisonMode", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9",
            TypeKind = TypeKind.WrappedClass)]
        public class RowOrderByNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RowOrderByNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RowOrderByNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9");

            public static RowOrderByNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            public string? @propertyName
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("propertyName");
                set => 
                    _wrappedElement.set("propertyName", value);
            }

            public bool @orderDescending
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("orderDescending");
                set => 
                    _wrappedElement.set("orderDescending", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f",
            TypeKind = TypeKind.WrappedClass)]
        public class RowFilterOnPositionNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RowFilterOnPositionNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RowFilterOnPositionNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f");

            public static RowFilterOnPositionNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            public int @amount
            {
                get =>
                    _wrappedElement.getOrDefault<int>("amount");
                set => 
                    _wrappedElement.set("amount", value);
            }

            public int @position
            {
                get =>
                    _wrappedElement.getOrDefault<int>("position");
                set => 
                    _wrappedElement.set("position", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode",
            TypeKind = TypeKind.WrappedClass)]
        public class RowFlattenNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RowFlattenNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RowFlattenNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode");

            public static RowFlattenNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode",
            TypeKind = TypeKind.WrappedClass)]
        public class RowFilterByMetaclassNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RowFilterByMetaclassNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RowFilterByMetaclassNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode");

            public static RowFilterByMetaclassNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            // Not found
            public object? @metaClass
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("metaClass");
                set => 
                    _wrappedElement.set("metaClass", value);
            }

            public bool @includeInherits
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("includeInherits");
                set => 
                    _wrappedElement.set("includeInherits", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

    }

    public class Column
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d",
            TypeKind = TypeKind.WrappedClass)]
        public class ColumnFilterIncludeOnlyNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ColumnFilterIncludeOnlyNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ColumnFilterIncludeOnlyNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d");

            public static ColumnFilterIncludeOnlyNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @columnNamesComma
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("columnNamesComma");
                set => 
                    _wrappedElement.set("columnNamesComma", value);
            }

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7",
            TypeKind = TypeKind.WrappedClass)]
        public class ColumnFilterExcludeNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ColumnFilterExcludeNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ColumnFilterExcludeNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7");

            public static ColumnFilterExcludeNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @columnNamesComma
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("columnNamesComma");
                set => 
                    _wrappedElement.set("columnNamesComma", value);
            }

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

    }

    public class Source
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode",
            TypeKind = TypeKind.WrappedClass)]
        public class SelectByExtentNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public SelectByExtentNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public SelectByExtentNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode");

            public static SelectByExtentNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @extentUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("extentUri");
                set => 
                    _wrappedElement.set("extentUri", value);
            }

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode",
            TypeKind = TypeKind.WrappedClass)]
        public class SelectByPathNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public SelectByPathNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public SelectByPathNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode");

            public static SelectByPathNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @path
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("path");
                set => 
                    _wrappedElement.set("path", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode",
            TypeKind = TypeKind.WrappedClass)]
        public class DynamicSourceNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public DynamicSourceNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public DynamicSourceNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode");

            public static DynamicSourceNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @nodeName
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("nodeName");
                set => 
                    _wrappedElement.set("nodeName", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db",
            TypeKind = TypeKind.WrappedClass)]
        public class SelectByWorkspaceNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public SelectByWorkspaceNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public SelectByWorkspaceNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db");

            public static SelectByWorkspaceNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode",
            TypeKind = TypeKind.WrappedClass)]
        public class SelectByFullNameNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public SelectByFullNameNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public SelectByFullNameNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode");

            public static SelectByFullNameNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("input");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("input", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("input", value);
                    }
                }
            }

            public string? @path
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("path");
                set => 
                    _wrappedElement.set("path", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a",
            TypeKind = TypeKind.WrappedClass)]
        public class SelectFromAllWorkspacesNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public SelectFromAllWorkspacesNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public SelectFromAllWorkspacesNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a");

            public static SelectFromAllWorkspacesNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

    }

    public class Node
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#e80d4c64-a68e-44a7-893d-1a5100a80370",
            TypeKind = TypeKind.WrappedClass)]
        public class ReferenceViewNode_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReferenceViewNode_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReferenceViewNode_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#e80d4c64-a68e-44a7-893d-1a5100a80370");

            public static ReferenceViewNode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

            public string? @itemUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("itemUri");
                set => 
                    _wrappedElement.set("itemUri", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

    }

}

public class Reports
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition",
        TypeKind = TypeKind.WrappedClass)]
    public class ReportDefinition_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ReportDefinition_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ReportDefinition_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition");

        public static ReportDefinition_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        // DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper
        public DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper? @elements
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("elements");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("elements", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("elements", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource",
        TypeKind = TypeKind.WrappedClass)]
    public class ReportInstanceSource_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ReportInstanceSource_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ReportInstanceSource_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource");

        public static ReportInstanceSource_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @path
        {
            get =>
                _wrappedElement.getOrDefault<string?>("path");
            set => 
                _wrappedElement.set("path", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance",
        TypeKind = TypeKind.WrappedClass)]
    public class ReportInstance_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ReportInstance_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ReportInstance_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance");

        public static ReportInstance_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper
        public DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper? @reportDefinition
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("reportDefinition");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("reportDefinition", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("reportDefinition", value);
                }
            }
        }

        // DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper
        public DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper? @sources
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("sources");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("sources", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("sources", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance",
        TypeKind = TypeKind.WrappedClass)]
    public class AdocReportInstance_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public AdocReportInstance_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public AdocReportInstance_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance");

        public static AdocReportInstance_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper
        public DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper? @reportDefinition
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("reportDefinition");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("reportDefinition", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("reportDefinition", value);
                }
            }
        }

        // DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper
        public DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper? @sources
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("sources");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("sources", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("sources", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance",
        TypeKind = TypeKind.WrappedClass)]
    public class HtmlReportInstance_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public HtmlReportInstance_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public HtmlReportInstance_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance");

        public static HtmlReportInstance_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @cssFile
        {
            get =>
                _wrappedElement.getOrDefault<string?>("cssFile");
            set => 
                _wrappedElement.set("cssFile", value);
        }

        public string? @cssStyleSheet
        {
            get =>
                _wrappedElement.getOrDefault<string?>("cssStyleSheet");
            set => 
                _wrappedElement.set("cssStyleSheet", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper
        public DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper? @reportDefinition
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("reportDefinition");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("reportDefinition", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("reportDefinition", value);
                }
            }
        }

        // DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper
        public DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper? @sources
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("sources");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("sources", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("sources", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration",
        TypeKind = TypeKind.WrappedClass)]
    public class SimpleReportConfiguration_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SimpleReportConfiguration_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SimpleReportConfiguration_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration");

        public static SimpleReportConfiguration_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @showDescendents
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showDescendents");
            set => 
                _wrappedElement.set("showDescendents", value);
        }

        public string? @rootElement
        {
            get =>
                _wrappedElement.getOrDefault<string?>("rootElement");
            set => 
                _wrappedElement.set("rootElement", value);
        }

        public bool @showRootElement
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showRootElement");
            set => 
                _wrappedElement.set("showRootElement", value);
        }

        public bool @showMetaClasses
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showMetaClasses");
            set => 
                _wrappedElement.set("showMetaClasses", value);
        }

        public bool @showFullName
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showFullName");
            set => 
                _wrappedElement.set("showFullName", value);
        }

        // Not found
        public object? @form
        {
            get =>
                _wrappedElement.getOrDefault<object?>("form");
            set => 
                _wrappedElement.set("form", value);
        }

        // Not found
        public object? @descendentMode
        {
            get =>
                _wrappedElement.getOrDefault<object?>("descendentMode");
            set => 
                _wrappedElement.set("descendentMode", value);
        }

        // Not found
        public object? @typeMode
        {
            get =>
                _wrappedElement.getOrDefault<object?>("typeMode");
            set => 
                _wrappedElement.set("typeMode", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

    }

    public class Elements
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportElement_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReportElement_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReportElement_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement");

            public static ReportElement_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportHeadline_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReportHeadline_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReportHeadline_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline");

            public static ReportHeadline_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @title
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("title");
                set => 
                    _wrappedElement.set("title", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportParagraph_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReportParagraph_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReportParagraph_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph");

            public static ReportParagraph_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @paragraph
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("paragraph");
                set => 
                    _wrappedElement.set("paragraph", value);
            }

            public string? @cssClass
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("cssClass");
                set => 
                    _wrappedElement.set("cssClass", value);
            }

            // Not found
            public object? @viewNode
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("viewNode");
                set => 
                    _wrappedElement.set("viewNode", value);
            }

            public string? @evalProperties
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("evalProperties");
                set => 
                    _wrappedElement.set("evalProperties", value);
            }

            public string? @evalParagraph
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("evalParagraph");
                set => 
                    _wrappedElement.set("evalParagraph", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportTable_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReportTable_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReportTable_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable");

            public static ReportTable_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @cssClass
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("cssClass");
                set => 
                    _wrappedElement.set("cssClass", value);
            }

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @viewNode
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("viewNode");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("viewNode", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("viewNode", value);
                    }
                }
            }

            // DatenMeister.Core.Models.Forms.TableForm_Wrapper
            public DatenMeister.Core.Models.Forms.TableForm_Wrapper? @form
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("form");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Forms.TableForm_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("form", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("form", value);
                    }
                }
            }

            public string? @evalProperties
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("evalProperties");
                set => 
                    _wrappedElement.set("evalProperties", value);
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportLoop_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public ReportLoop_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public ReportLoop_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop");

            public static ReportLoop_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @viewNode
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("viewNode");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("viewNode", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("viewNode", value);
                    }
                }
            }

            // DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper
            public DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper? @elements
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("elements");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("elements", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("elements", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

    }

}

public class ExtentLoaderConfigs
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExtentLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExtentLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig");

        public static ExtentLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExcelLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExcelLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig");

        public static ExcelLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @fixRowCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixRowCount");
            set => 
                _wrappedElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixColumnCount");
            set => 
                _wrappedElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sheetName");
            set => 
                _wrappedElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetRow");
            set => 
                _wrappedElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetColumn");
            set => 
                _wrappedElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                _wrappedElement.getOrDefault<int>("countRows");
            set => 
                _wrappedElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                _wrappedElement.getOrDefault<int>("countColumns");
            set => 
                _wrappedElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hasHeader");
            set => 
                _wrappedElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                _wrappedElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                _wrappedElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                _wrappedElement.getOrDefault<bool>("onlySetColumns");
            set => 
                _wrappedElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("idColumnName");
            set => 
                _wrappedElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                _wrappedElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                _wrappedElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelReferenceLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExcelReferenceLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExcelReferenceLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig");

        public static ExcelReferenceLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @fixRowCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixRowCount");
            set => 
                _wrappedElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixColumnCount");
            set => 
                _wrappedElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sheetName");
            set => 
                _wrappedElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetRow");
            set => 
                _wrappedElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetColumn");
            set => 
                _wrappedElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                _wrappedElement.getOrDefault<int>("countRows");
            set => 
                _wrappedElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                _wrappedElement.getOrDefault<int>("countColumns");
            set => 
                _wrappedElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hasHeader");
            set => 
                _wrappedElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                _wrappedElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                _wrappedElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                _wrappedElement.getOrDefault<bool>("onlySetColumns");
            set => 
                _wrappedElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("idColumnName");
            set => 
                _wrappedElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                _wrappedElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                _wrappedElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelImportLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExcelImportLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExcelImportLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig");

        public static ExcelImportLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @extentPath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentPath");
            set => 
                _wrappedElement.set("extentPath", value);
        }

        public bool @fixRowCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixRowCount");
            set => 
                _wrappedElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixColumnCount");
            set => 
                _wrappedElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sheetName");
            set => 
                _wrappedElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetRow");
            set => 
                _wrappedElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetColumn");
            set => 
                _wrappedElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                _wrappedElement.getOrDefault<int>("countRows");
            set => 
                _wrappedElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                _wrappedElement.getOrDefault<int>("countColumns");
            set => 
                _wrappedElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hasHeader");
            set => 
                _wrappedElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                _wrappedElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                _wrappedElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                _wrappedElement.getOrDefault<bool>("onlySetColumns");
            set => 
                _wrappedElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("idColumnName");
            set => 
                _wrappedElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                _wrappedElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                _wrappedElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelExtentLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExcelExtentLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExcelExtentLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig");

        public static ExcelExtentLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @idColumnName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("idColumnName");
            set => 
                _wrappedElement.set("idColumnName", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class InMemoryLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public InMemoryLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public InMemoryLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig");

        public static InMemoryLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @isLinkedList
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isLinkedList");
            set => 
                _wrappedElement.set("isLinkedList", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class XmlReferenceLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public XmlReferenceLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public XmlReferenceLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig");

        public static XmlReferenceLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public bool @keepNamespaces
        {
            get =>
                _wrappedElement.getOrDefault<bool>("keepNamespaces");
            set => 
                _wrappedElement.set("keepNamespaces", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentFileLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExtentFileLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExtentFileLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig");

        public static ExtentFileLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class XmiStorageLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public XmiStorageLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public XmiStorageLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig");

        public static XmiStorageLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class CsvExtentLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CsvExtentLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CsvExtentLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig");

        public static CsvExtentLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.ExtentLoaderConfigs.CsvSettings_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.CsvSettings_Wrapper? @settings
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("settings");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.CsvSettings_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("settings", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("settings", value);
                }
            }
        }

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings",
        TypeKind = TypeKind.WrappedClass)]
    public class CsvSettings_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CsvSettings_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CsvSettings_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings");

        public static CsvSettings_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @encoding
        {
            get =>
                _wrappedElement.getOrDefault<string?>("encoding");
            set => 
                _wrappedElement.set("encoding", value);
        }

        public bool @hasHeader
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hasHeader");
            set => 
                _wrappedElement.set("hasHeader", value);
        }

        // Not found
        public object? @separator
        {
            get =>
                _wrappedElement.getOrDefault<object?>("separator");
            set => 
                _wrappedElement.set("separator", value);
        }

        public string? @columns
        {
            get =>
                _wrappedElement.getOrDefault<string?>("columns");
            set => 
                _wrappedElement.set("columns", value);
        }

        public string? @metaclassUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("metaclassUri");
            set => 
                _wrappedElement.set("metaclassUri", value);
        }

        public bool @trimCells
        {
            get =>
                _wrappedElement.getOrDefault<bool>("trimCells");
            set => 
                _wrappedElement.set("trimCells", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelHierarchicalColumnDefinition_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExcelHierarchicalColumnDefinition_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExcelHierarchicalColumnDefinition_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition");

        public static ExcelHierarchicalColumnDefinition_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaClass
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("metaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("metaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("metaClass", value);
                }
            }
        }

        public string? @property
        {
            get =>
                _wrappedElement.getOrDefault<string?>("property");
            set => 
                _wrappedElement.set("property", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelHierarchicalLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExcelHierarchicalLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExcelHierarchicalLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig");

        public static ExcelHierarchicalLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition_Wrapper? @hierarchicalColumns
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("hierarchicalColumns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("hierarchicalColumns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("hierarchicalColumns", value);
                }
            }
        }

        public bool @skipElementsForLastLevel
        {
            get =>
                _wrappedElement.getOrDefault<bool>("skipElementsForLastLevel");
            set => 
                _wrappedElement.set("skipElementsForLastLevel", value);
        }

        public bool @fixRowCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixRowCount");
            set => 
                _wrappedElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                _wrappedElement.getOrDefault<bool>("fixColumnCount");
            set => 
                _wrappedElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("sheetName");
            set => 
                _wrappedElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetRow");
            set => 
                _wrappedElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                _wrappedElement.getOrDefault<int>("offsetColumn");
            set => 
                _wrappedElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                _wrappedElement.getOrDefault<int>("countRows");
            set => 
                _wrappedElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                _wrappedElement.getOrDefault<int>("countColumns");
            set => 
                _wrappedElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hasHeader");
            set => 
                _wrappedElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                _wrappedElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                _wrappedElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                _wrappedElement.getOrDefault<bool>("onlySetColumns");
            set => 
                _wrappedElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("idColumnName");
            set => 
                _wrappedElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                _wrappedElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                _wrappedElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelColumn_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExcelColumn_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExcelColumn_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a");

        public static ExcelColumn_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @header
        {
            get =>
                _wrappedElement.getOrDefault<string?>("header");
            set => 
                _wrappedElement.set("header", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3",
        TypeKind = TypeKind.WrappedClass)]
    public class EnvironmentalVariableLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public EnvironmentalVariableLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public EnvironmentalVariableLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3");

        public static EnvironmentalVariableLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

}

public class Forms
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class FieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public FieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public FieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData");

        public static FieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder",
        TypeKind = TypeKind.WrappedClass)]
    public class SortingOrder_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SortingOrder_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SortingOrder_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder");

        public static SortingOrder_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDescending
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDescending");
            set => 
                _wrappedElement.set("isDescending", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class AnyDataFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public AnyDataFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public AnyDataFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData");

        public static AnyDataFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class CheckboxFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CheckboxFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CheckboxFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData");

        public static CheckboxFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public int @lineHeight
        {
            get =>
                _wrappedElement.getOrDefault<int>("lineHeight");
            set => 
                _wrappedElement.set("lineHeight", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class ActionFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ActionFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ActionFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData");

        public static ActionFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @actionName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("actionName");
            set => 
                _wrappedElement.set("actionName", value);
        }

        // Not found
        public object? @parameter
        {
            get =>
                _wrappedElement.getOrDefault<object?>("parameter");
            set => 
                _wrappedElement.set("parameter", value);
        }

        public string? @buttonText
        {
            get =>
                _wrappedElement.getOrDefault<string?>("buttonText");
            set => 
                _wrappedElement.set("buttonText", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class DateTimeFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DateTimeFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DateTimeFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData");

        public static DateTimeFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @hideDate
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hideDate");
            set => 
                _wrappedElement.set("hideDate", value);
        }

        public bool @hideTime
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hideTime");
            set => 
                _wrappedElement.set("hideTime", value);
        }

        public bool @showOffsetButtons
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showOffsetButtons");
            set => 
                _wrappedElement.set("showOffsetButtons", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation",
        TypeKind = TypeKind.WrappedClass)]
    public class FormAssociation_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public FormAssociation_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public FormAssociation_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation");

        public static FormAssociation_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // Not found
        public object? @formType
        {
            get =>
                _wrappedElement.getOrDefault<object?>("formType");
            set => 
                _wrappedElement.set("formType", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper? @metaClass
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("metaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("metaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("metaClass", value);
                }
            }
        }

        public string? @extentType
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentType");
            set => 
                _wrappedElement.set("extentType", value);
        }

        public string? @viewModeId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("viewModeId");
            set => 
                _wrappedElement.set("viewModeId", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper? @parentMetaClass
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("parentMetaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("parentMetaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("parentMetaClass", value);
                }
            }
        }

        public string? @parentProperty
        {
            get =>
                _wrappedElement.getOrDefault<string?>("parentProperty");
            set => 
                _wrappedElement.set("parentProperty", value);
        }

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @form
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("form");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("form", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("form", value);
                }
            }
        }

        public bool @debugActive
        {
            get =>
                _wrappedElement.getOrDefault<bool>("debugActive");
            set => 
                _wrappedElement.set("debugActive", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class DropDownFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DropDownFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DropDownFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData");

        public static DropDownFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.Forms.ValuePair_Wrapper
        public DatenMeister.Core.Models.Forms.ValuePair_Wrapper? @values
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("values");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.ValuePair_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("values", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("values", value);
                }
            }
        }

        // Not found
        public object? @valuesByEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<object?>("valuesByEnumeration");
            set => 
                _wrappedElement.set("valuesByEnumeration", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair",
        TypeKind = TypeKind.WrappedClass)]
    public class ValuePair_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ValuePair_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ValuePair_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair");

        public static ValuePair_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @value
        {
            get =>
                _wrappedElement.getOrDefault<object?>("value");
            set => 
                _wrappedElement.set("value", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class MetaClassElementFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public MetaClassElementFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public MetaClassElementFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData");

        public static MetaClassElementFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class ReferenceFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ReferenceFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ReferenceFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData");

        public static ReferenceFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @isSelectionInline
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isSelectionInline");
            set => 
                _wrappedElement.set("isSelectionInline", value);
        }

        public string? @defaultWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("defaultWorkspace");
            set => 
                _wrappedElement.set("defaultWorkspace", value);
        }

        public string? @defaultItemUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("defaultItemUri");
            set => 
                _wrappedElement.set("defaultItemUri", value);
        }

        public bool @showAllChildren
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showAllChildren");
            set => 
                _wrappedElement.set("showAllChildren", value);
        }

        public bool @showWorkspaceSelection
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showWorkspaceSelection");
            set => 
                _wrappedElement.set("showWorkspaceSelection", value);
        }

        public bool @showExtentSelection
        {
            get =>
                _wrappedElement.getOrDefault<bool>("showExtentSelection");
            set => 
                _wrappedElement.set("showExtentSelection", value);
        }

        // Not found
        public object? @metaClassFilter
        {
            get =>
                _wrappedElement.getOrDefault<object?>("metaClassFilter");
            set => 
                _wrappedElement.set("metaClassFilter", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class SubElementFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SubElementFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SubElementFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData");

        public static SubElementFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @metaClass
        {
            get =>
                _wrappedElement.getOrDefault<object?>("metaClass");
            set => 
                _wrappedElement.set("metaClass", value);
        }

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @form
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("form");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("form", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("form", value);
                }
            }
        }

        public bool @allowOnlyExistingElements
        {
            get =>
                _wrappedElement.getOrDefault<bool>("allowOnlyExistingElements");
            set => 
                _wrappedElement.set("allowOnlyExistingElements", value);
        }

        // Not found
        public object? @defaultTypesForNewElements
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultTypesForNewElements");
            set => 
                _wrappedElement.set("defaultTypesForNewElements", value);
        }

        public bool @includeSpecializationsForDefaultTypes
        {
            get =>
                _wrappedElement.getOrDefault<bool>("includeSpecializationsForDefaultTypes");
            set => 
                _wrappedElement.set("includeSpecializationsForDefaultTypes", value);
        }

        public string? @defaultWorkspaceOfNewElements
        {
            get =>
                _wrappedElement.getOrDefault<string?>("defaultWorkspaceOfNewElements");
            set => 
                _wrappedElement.set("defaultWorkspaceOfNewElements", value);
        }

        public string? @defaultExtentOfNewElements
        {
            get =>
                _wrappedElement.getOrDefault<string?>("defaultExtentOfNewElements");
            set => 
                _wrappedElement.set("defaultExtentOfNewElements", value);
        }

        public string? @actionName
        {
            get =>
                _wrappedElement.getOrDefault<string?>("actionName");
            set => 
                _wrappedElement.set("actionName", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class TextFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public TextFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public TextFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData");

        public static TextFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public int @lineHeight
        {
            get =>
                _wrappedElement.getOrDefault<int>("lineHeight");
            set => 
                _wrappedElement.set("lineHeight", value);
        }

        public int @width
        {
            get =>
                _wrappedElement.getOrDefault<int>("width");
            set => 
                _wrappedElement.set("width", value);
        }

        public int @shortenTextLength
        {
            get =>
                _wrappedElement.getOrDefault<int>("shortenTextLength");
            set => 
                _wrappedElement.set("shortenTextLength", value);
        }

        public bool @supportClipboardCopy
        {
            get =>
                _wrappedElement.getOrDefault<bool>("supportClipboardCopy");
            set => 
                _wrappedElement.set("supportClipboardCopy", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class EvalTextFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public EvalTextFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public EvalTextFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData");

        public static EvalTextFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @evalCellProperties
        {
            get =>
                _wrappedElement.getOrDefault<string?>("evalCellProperties");
            set => 
                _wrappedElement.set("evalCellProperties", value);
        }

        public int @lineHeight
        {
            get =>
                _wrappedElement.getOrDefault<int>("lineHeight");
            set => 
                _wrappedElement.set("lineHeight", value);
        }

        public int @width
        {
            get =>
                _wrappedElement.getOrDefault<int>("width");
            set => 
                _wrappedElement.set("width", value);
        }

        public int @shortenTextLength
        {
            get =>
                _wrappedElement.getOrDefault<int>("shortenTextLength");
            set => 
                _wrappedElement.set("shortenTextLength", value);
        }

        public bool @supportClipboardCopy
        {
            get =>
                _wrappedElement.getOrDefault<bool>("supportClipboardCopy");
            set => 
                _wrappedElement.set("supportClipboardCopy", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class SeparatorLineFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SeparatorLineFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SeparatorLineFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData");

        public static SeparatorLineFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public int @Height
        {
            get =>
                _wrappedElement.getOrDefault<int>("Height");
            set => 
                _wrappedElement.set("Height", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class FileSelectionFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public FileSelectionFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public FileSelectionFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData");

        public static FileSelectionFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @defaultExtension
        {
            get =>
                _wrappedElement.getOrDefault<string?>("defaultExtension");
            set => 
                _wrappedElement.set("defaultExtension", value);
        }

        public bool @isSaving
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isSaving");
            set => 
                _wrappedElement.set("isSaving", value);
        }

        public string? @initialPathToDirectory
        {
            get =>
                _wrappedElement.getOrDefault<string?>("initialPathToDirectory");
            set => 
                _wrappedElement.set("initialPathToDirectory", value);
        }

        public string? @filter
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filter");
            set => 
                _wrappedElement.set("filter", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement",
        TypeKind = TypeKind.WrappedClass)]
    public class DefaultTypeForNewElement_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DefaultTypeForNewElement_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DefaultTypeForNewElement_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement");

        public static DefaultTypeForNewElement_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // Not found
        public object? @metaClass
        {
            get =>
                _wrappedElement.getOrDefault<object?>("metaClass");
            set => 
                _wrappedElement.set("metaClass", value);
        }

        public string? @parentProperty
        {
            get =>
                _wrappedElement.getOrDefault<string?>("parentProperty");
            set => 
                _wrappedElement.set("parentProperty", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class FullNameFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public FullNameFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public FullNameFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData");

        public static FullNameFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class CheckboxListTaggingFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CheckboxListTaggingFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CheckboxListTaggingFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData");

        public static CheckboxListTaggingFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.Forms.ValuePair_Wrapper
        public DatenMeister.Core.Models.Forms.ValuePair_Wrapper? @values
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("values");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.ValuePair_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("values", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("values", value);
                }
            }
        }

        public string? @separator
        {
            get =>
                _wrappedElement.getOrDefault<string?>("separator");
            set => 
                _wrappedElement.set("separator", value);
        }

        public bool @containsFreeText
        {
            get =>
                _wrappedElement.getOrDefault<bool>("containsFreeText");
            set => 
                _wrappedElement.set("containsFreeText", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class NumberFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public NumberFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public NumberFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData");

        public static NumberFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @format
        {
            get =>
                _wrappedElement.getOrDefault<string?>("format");
            set => 
                _wrappedElement.set("format", value);
        }

        public bool @isInteger
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isInteger");
            set => 
                _wrappedElement.set("isInteger", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form",
        TypeKind = TypeKind.WrappedClass)]
    public class Form_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Form_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Form_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.Form");

        public static Form_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                _wrappedElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                _wrappedElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalUri");
            set => 
                _wrappedElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalWorkspace");
            set => 
                _wrappedElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                _wrappedElement.getOrDefault<string?>("creationProtocol");
            set => 
                _wrappedElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm",
        TypeKind = TypeKind.WrappedClass)]
    public class RowForm_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public RowForm_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public RowForm_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm");

        public static RowForm_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @buttonApplyText
        {
            get =>
                _wrappedElement.getOrDefault<string?>("buttonApplyText");
            set => 
                _wrappedElement.set("buttonApplyText", value);
        }

        public bool @allowNewProperties
        {
            get =>
                _wrappedElement.getOrDefault<bool>("allowNewProperties");
            set => 
                _wrappedElement.set("allowNewProperties", value);
        }

        public int @defaultWidth
        {
            get =>
                _wrappedElement.getOrDefault<int>("defaultWidth");
            set => 
                _wrappedElement.set("defaultWidth", value);
        }

        public int @defaultHeight
        {
            get =>
                _wrappedElement.getOrDefault<int>("defaultHeight");
            set => 
                _wrappedElement.set("defaultHeight", value);
        }

        // DatenMeister.Core.Models.Forms.FieldData_Wrapper
        public DatenMeister.Core.Models.Forms.FieldData_Wrapper? @field
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("field");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FieldData_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("field", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("field", value);
                }
            }
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                _wrappedElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                _wrappedElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalUri");
            set => 
                _wrappedElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalWorkspace");
            set => 
                _wrappedElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                _wrappedElement.getOrDefault<string?>("creationProtocol");
            set => 
                _wrappedElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm",
        TypeKind = TypeKind.WrappedClass)]
    public class TableForm_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public TableForm_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public TableForm_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm");

        public static TableForm_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @property
        {
            get =>
                _wrappedElement.getOrDefault<string?>("property");
            set => 
                _wrappedElement.set("property", value);
        }

        // Not found
        public object? @metaClass
        {
            get =>
                _wrappedElement.getOrDefault<object?>("metaClass");
            set => 
                _wrappedElement.set("metaClass", value);
        }

        public bool @includeDescendents
        {
            get =>
                _wrappedElement.getOrDefault<bool>("includeDescendents");
            set => 
                _wrappedElement.set("includeDescendents", value);
        }

        public bool @noItemsWithMetaClass
        {
            get =>
                _wrappedElement.getOrDefault<bool>("noItemsWithMetaClass");
            set => 
                _wrappedElement.set("noItemsWithMetaClass", value);
        }

        public bool @inhibitNewItems
        {
            get =>
                _wrappedElement.getOrDefault<bool>("inhibitNewItems");
            set => 
                _wrappedElement.set("inhibitNewItems", value);
        }

        public bool @inhibitDeleteItems
        {
            get =>
                _wrappedElement.getOrDefault<bool>("inhibitDeleteItems");
            set => 
                _wrappedElement.set("inhibitDeleteItems", value);
        }

        public bool @inhibitEditItems
        {
            get =>
                _wrappedElement.getOrDefault<bool>("inhibitEditItems");
            set => 
                _wrappedElement.set("inhibitEditItems", value);
        }

        // DatenMeister.Core.Models.Forms.DefaultTypeForNewElement_Wrapper
        public DatenMeister.Core.Models.Forms.DefaultTypeForNewElement_Wrapper? @defaultTypesForNewElements
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("defaultTypesForNewElements");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.DefaultTypeForNewElement_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("defaultTypesForNewElements", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("defaultTypesForNewElements", value);
                }
            }
        }

        // Not found
        public object? @fastViewFilters
        {
            get =>
                _wrappedElement.getOrDefault<object?>("fastViewFilters");
            set => 
                _wrappedElement.set("fastViewFilters", value);
        }

        // DatenMeister.Core.Models.Forms.FieldData_Wrapper
        public DatenMeister.Core.Models.Forms.FieldData_Wrapper? @field
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("field");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FieldData_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("field", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("field", value);
                }
            }
        }

        // DatenMeister.Core.Models.Forms.SortingOrder_Wrapper
        public DatenMeister.Core.Models.Forms.SortingOrder_Wrapper? @sortingOrder
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("sortingOrder");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.SortingOrder_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("sortingOrder", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("sortingOrder", value);
                }
            }
        }

        // Not found
        public object? @viewNode
        {
            get =>
                _wrappedElement.getOrDefault<object?>("viewNode");
            set => 
                _wrappedElement.set("viewNode", value);
        }

        public bool @autoGenerateFields
        {
            get =>
                _wrappedElement.getOrDefault<bool>("autoGenerateFields");
            set => 
                _wrappedElement.set("autoGenerateFields", value);
        }

        public bool @duplicatePerType
        {
            get =>
                _wrappedElement.getOrDefault<bool>("duplicatePerType");
            set => 
                _wrappedElement.set("duplicatePerType", value);
        }

        public string? @dataUrl
        {
            get =>
                _wrappedElement.getOrDefault<string?>("dataUrl");
            set => 
                _wrappedElement.set("dataUrl", value);
        }

        public bool @inhibitNewUnclassifiedItems
        {
            get =>
                _wrappedElement.getOrDefault<bool>("inhibitNewUnclassifiedItems");
            set => 
                _wrappedElement.set("inhibitNewUnclassifiedItems", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                _wrappedElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                _wrappedElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalUri");
            set => 
                _wrappedElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalWorkspace");
            set => 
                _wrappedElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                _wrappedElement.getOrDefault<string?>("creationProtocol");
            set => 
                _wrappedElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm",
        TypeKind = TypeKind.WrappedClass)]
    public class CollectionForm_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CollectionForm_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CollectionForm_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm");

        public static CollectionForm_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @tab
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("tab");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("tab", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("tab", value);
                }
            }
        }

        public bool @autoTabs
        {
            get =>
                _wrappedElement.getOrDefault<bool>("autoTabs");
            set => 
                _wrappedElement.set("autoTabs", value);
        }

        // DatenMeister.Core.Models.Forms.FieldData_Wrapper
        public DatenMeister.Core.Models.Forms.FieldData_Wrapper? @field
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("field");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FieldData_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("field", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("field", value);
                }
            }
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                _wrappedElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                _wrappedElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalUri");
            set => 
                _wrappedElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalWorkspace");
            set => 
                _wrappedElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                _wrappedElement.getOrDefault<string?>("creationProtocol");
            set => 
                _wrappedElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm",
        TypeKind = TypeKind.WrappedClass)]
    public class ObjectForm_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ObjectForm_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ObjectForm_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm");

        public static ObjectForm_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @tab
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("tab");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("tab", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("tab", value);
                }
            }
        }

        public bool @autoTabs
        {
            get =>
                _wrappedElement.getOrDefault<bool>("autoTabs");
            set => 
                _wrappedElement.set("autoTabs", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                _wrappedElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                _wrappedElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                _wrappedElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalUri");
            set => 
                _wrappedElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("originalWorkspace");
            set => 
                _wrappedElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                _wrappedElement.getOrDefault<string?>("creationProtocol");
            set => 
                _wrappedElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode",
        TypeKind = TypeKind.WrappedClass)]
    public class ViewMode_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ViewMode_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ViewMode_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode");

        public static ViewMode_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @id
        {
            get =>
                _wrappedElement.getOrDefault<string?>("id");
            set => 
                _wrappedElement.set("id", value);
        }

        public string? @defaultExtentType
        {
            get =>
                _wrappedElement.getOrDefault<string?>("defaultExtentType");
            set => 
                _wrappedElement.set("defaultExtentType", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection",
        TypeKind = TypeKind.WrappedClass)]
    public class DropDownByCollection_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DropDownByCollection_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DropDownByCollection_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection");

        public static DropDownByCollection_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @defaultWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("defaultWorkspace");
            set => 
                _wrappedElement.set("defaultWorkspace", value);
        }

        public string? @collection
        {
            get =>
                _wrappedElement.getOrDefault<string?>("collection");
            set => 
                _wrappedElement.set("collection", value);
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00",
        TypeKind = TypeKind.WrappedClass)]
    public class UriReferenceFieldData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public UriReferenceFieldData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public UriReferenceFieldData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00");

        public static UriReferenceFieldData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @defaultWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultWorkspace");
            set => 
                _wrappedElement.set("defaultWorkspace", value);
        }

        // Not found
        public object? @defaultExtent
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultExtent");
            set => 
                _wrappedElement.set("defaultExtent", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0",
        TypeKind = TypeKind.WrappedClass)]
    public class NavigateToFieldsForTestAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public NavigateToFieldsForTestAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public NavigateToFieldsForTestAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0");

        public static NavigateToFieldsForTestAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2",
        TypeKind = TypeKind.WrappedClass)]
    public class DropDownByQueryData_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DropDownByQueryData_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DropDownByQueryData_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2");

        public static DropDownByQueryData_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper
        public DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper? @query
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("query");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("query", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("query", value);
                }
            }
        }

        public bool @isAttached
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAttached");
            set => 
                _wrappedElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isEnumeration");
            set => 
                _wrappedElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                _wrappedElement.getOrDefault<object?>("defaultValue");
            set => 
                _wrappedElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isReadOnly");
            set => 
                _wrappedElement.set("isReadOnly", value);
        }

    }

}

public class AttachedExtent
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration",
        TypeKind = TypeKind.WrappedClass)]
    public class AttachedExtentConfiguration_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public AttachedExtentConfiguration_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public AttachedExtentConfiguration_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration");

        public static AttachedExtentConfiguration_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @referencedWorkspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("referencedWorkspace");
            set => 
                _wrappedElement.set("referencedWorkspace", value);
        }

        public string? @referencedExtent
        {
            get =>
                _wrappedElement.getOrDefault<string?>("referencedExtent");
            set => 
                _wrappedElement.set("referencedExtent", value);
        }

        // Not found
        public object? @referenceType
        {
            get =>
                _wrappedElement.getOrDefault<object?>("referenceType");
            set => 
                _wrappedElement.set("referenceType", value);
        }

        public string? @referenceProperty
        {
            get =>
                _wrappedElement.getOrDefault<string?>("referenceProperty");
            set => 
                _wrappedElement.set("referenceProperty", value);
        }

    }

}

public class Management
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent",
        TypeKind = TypeKind.WrappedClass)]
    public class Extent_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Extent_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Extent_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent");

        public static Extent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @uri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("uri");
            set => 
                _wrappedElement.set("uri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public int @count
        {
            get =>
                _wrappedElement.getOrDefault<int>("count");
            set => 
                _wrappedElement.set("count", value);
        }

        public int @totalCount
        {
            get =>
                _wrappedElement.getOrDefault<int>("totalCount");
            set => 
                _wrappedElement.set("totalCount", value);
        }

        public string? @type
        {
            get =>
                _wrappedElement.getOrDefault<string?>("type");
            set => 
                _wrappedElement.set("type", value);
        }

        public string? @extentType
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentType");
            set => 
                _wrappedElement.set("extentType", value);
        }

        public bool @isModified
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isModified");
            set => 
                _wrappedElement.set("isModified", value);
        }

        public string? @alternativeUris
        {
            get =>
                _wrappedElement.getOrDefault<string?>("alternativeUris");
            set => 
                _wrappedElement.set("alternativeUris", value);
        }

        public string? @autoEnumerateType
        {
            get =>
                _wrappedElement.getOrDefault<string?>("autoEnumerateType");
            set => 
                _wrappedElement.set("autoEnumerateType", value);
        }

        // Not found
        public object? @state
        {
            get =>
                _wrappedElement.getOrDefault<object?>("state");
            set => 
                _wrappedElement.set("state", value);
        }

        public string? @failMessage
        {
            get =>
                _wrappedElement.getOrDefault<string?>("failMessage");
            set => 
                _wrappedElement.set("failMessage", value);
        }

        // Not found
        public object? @properties
        {
            get =>
                _wrappedElement.getOrDefault<object?>("properties");
            set => 
                _wrappedElement.set("properties", value);
        }

        // Not found
        public object? @loadingConfiguration
        {
            get =>
                _wrappedElement.getOrDefault<object?>("loadingConfiguration");
            set => 
                _wrappedElement.set("loadingConfiguration", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace",
        TypeKind = TypeKind.WrappedClass)]
    public class Workspace_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Workspace_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Workspace_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace");

        public static Workspace_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @id
        {
            get =>
                _wrappedElement.getOrDefault<string?>("id");
            set => 
                _wrappedElement.set("id", value);
        }

        public string? @annotation
        {
            get =>
                _wrappedElement.getOrDefault<string?>("annotation");
            set => 
                _wrappedElement.set("annotation", value);
        }

        // DatenMeister.Core.Models.Management.Extent_Wrapper
        public DatenMeister.Core.Models.Management.Extent_Wrapper? @extents
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("extents");
                return foundElement == null ? null : new DatenMeister.Core.Models.Management.Extent_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("extents", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("extents", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel",
        TypeKind = TypeKind.WrappedClass)]
    public class CreateNewWorkspaceModel_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CreateNewWorkspaceModel_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CreateNewWorkspaceModel_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel");

        public static CreateNewWorkspaceModel_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @id
        {
            get =>
                _wrappedElement.getOrDefault<string?>("id");
            set => 
                _wrappedElement.set("id", value);
        }

        public string? @annotation
        {
            get =>
                _wrappedElement.getOrDefault<string?>("annotation");
            set => 
                _wrappedElement.set("annotation", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentTypeSetting_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExtentTypeSetting_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExtentTypeSetting_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting");

        public static ExtentTypeSetting_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // Not found
        public object? @rootElementMetaClasses
        {
            get =>
                _wrappedElement.getOrDefault<object?>("rootElementMetaClasses");
            set => 
                _wrappedElement.set("rootElementMetaClasses", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentProperties_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExtentProperties_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExtentProperties_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties");

        public static ExtentProperties_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @uri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("uri");
            set => 
                _wrappedElement.set("uri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public int @count
        {
            get =>
                _wrappedElement.getOrDefault<int>("count");
            set => 
                _wrappedElement.set("count", value);
        }

        public int @totalCount
        {
            get =>
                _wrappedElement.getOrDefault<int>("totalCount");
            set => 
                _wrappedElement.set("totalCount", value);
        }

        public string? @type
        {
            get =>
                _wrappedElement.getOrDefault<string?>("type");
            set => 
                _wrappedElement.set("type", value);
        }

        public string? @extentType
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentType");
            set => 
                _wrappedElement.set("extentType", value);
        }

        public bool @isModified
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isModified");
            set => 
                _wrappedElement.set("isModified", value);
        }

        public string? @alternativeUris
        {
            get =>
                _wrappedElement.getOrDefault<string?>("alternativeUris");
            set => 
                _wrappedElement.set("alternativeUris", value);
        }

        public string? @autoEnumerateType
        {
            get =>
                _wrappedElement.getOrDefault<string?>("autoEnumerateType");
            set => 
                _wrappedElement.set("autoEnumerateType", value);
        }

        // Not found
        public object? @state
        {
            get =>
                _wrappedElement.getOrDefault<object?>("state");
            set => 
                _wrappedElement.set("state", value);
        }

        public string? @failMessage
        {
            get =>
                _wrappedElement.getOrDefault<string?>("failMessage");
            set => 
                _wrappedElement.set("failMessage", value);
        }

        // Not found
        public object? @properties
        {
            get =>
                _wrappedElement.getOrDefault<object?>("properties");
            set => 
                _wrappedElement.set("properties", value);
        }

        // Not found
        public object? @loadingConfiguration
        {
            get =>
                _wrappedElement.getOrDefault<object?>("loadingConfiguration");
            set => 
                _wrappedElement.set("loadingConfiguration", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentPropertyDefinition_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExtentPropertyDefinition_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExtentPropertyDefinition_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition");

        public static ExtentPropertyDefinition_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @title
        {
            get =>
                _wrappedElement.getOrDefault<string?>("title");
            set => 
                _wrappedElement.set("title", value);
        }

        // Not found
        public object? @metaClass
        {
            get =>
                _wrappedElement.getOrDefault<object?>("metaClass");
            set => 
                _wrappedElement.set("metaClass", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentSettings_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ExtentSettings_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ExtentSettings_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings");

        public static ExtentSettings_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.Management.ExtentTypeSetting_Wrapper
        public DatenMeister.Core.Models.Management.ExtentTypeSetting_Wrapper? @extentTypeSettings
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("extentTypeSettings");
                return foundElement == null ? null : new DatenMeister.Core.Models.Management.ExtentTypeSetting_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("extentTypeSettings", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("extentTypeSettings", value);
                }
            }
        }

        // DatenMeister.Core.Models.Management.ExtentPropertyDefinition_Wrapper
        public DatenMeister.Core.Models.Management.ExtentPropertyDefinition_Wrapper? @propertyDefinitions
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("propertyDefinitions");
                return foundElement == null ? null : new DatenMeister.Core.Models.Management.ExtentPropertyDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("propertyDefinitions", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("propertyDefinitions", value);
                }
            }
        }

    }

}

public class FastViewFilters
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter",
        TypeKind = TypeKind.WrappedClass)]
    public class PropertyComparisonFilter_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public PropertyComparisonFilter_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public PropertyComparisonFilter_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter");

        public static PropertyComparisonFilter_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @Property
        {
            get =>
                _wrappedElement.getOrDefault<string?>("Property");
            set => 
                _wrappedElement.set("Property", value);
        }

        // Not found
        public object? @ComparisonType
        {
            get =>
                _wrappedElement.getOrDefault<object?>("ComparisonType");
            set => 
                _wrappedElement.set("ComparisonType", value);
        }

        public string? @Value
        {
            get =>
                _wrappedElement.getOrDefault<string?>("Value");
            set => 
                _wrappedElement.set("Value", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter",
        TypeKind = TypeKind.WrappedClass)]
    public class PropertyContainsFilter_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public PropertyContainsFilter_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public PropertyContainsFilter_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter");

        public static PropertyContainsFilter_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @Property
        {
            get =>
                _wrappedElement.getOrDefault<string?>("Property");
            set => 
                _wrappedElement.set("Property", value);
        }

        public string? @Value
        {
            get =>
                _wrappedElement.getOrDefault<string?>("Value");
            set => 
                _wrappedElement.set("Value", value);
        }

    }

}

public class DynamicRuntimeProvider
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class DynamicRuntimeLoaderConfig_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DynamicRuntimeLoaderConfig_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DynamicRuntimeLoaderConfig_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig");

        public static DynamicRuntimeLoaderConfig_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @runtimeClass
        {
            get =>
                _wrappedElement.getOrDefault<string?>("runtimeClass");
            set => 
                _wrappedElement.set("runtimeClass", value);
        }

        // Not found
        public object? @configuration
        {
            get =>
                _wrappedElement.getOrDefault<object?>("configuration");
            set => 
                _wrappedElement.set("configuration", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUri");
            set => 
                _wrappedElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    public class Examples
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a",
            TypeKind = TypeKind.WrappedClass)]
        public class NumberProviderSettings_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NumberProviderSettings_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NumberProviderSettings_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a");

            public static NumberProviderSettings_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

            public int @start
            {
                get =>
                    _wrappedElement.getOrDefault<int>("start");
                set => 
                    _wrappedElement.set("start", value);
            }

            public int @end
            {
                get =>
                    _wrappedElement.getOrDefault<int>("end");
                set => 
                    _wrappedElement.set("end", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation",
            TypeKind = TypeKind.WrappedClass)]
        public class NumberRepresentation_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NumberRepresentation_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NumberRepresentation_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation");

            public static NumberRepresentation_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @binary
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("binary");
                set => 
                    _wrappedElement.set("binary", value);
            }

            public string? @octal
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("octal");
                set => 
                    _wrappedElement.set("octal", value);
            }

            public string? @decimal
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("decimal");
                set => 
                    _wrappedElement.set("decimal", value);
            }

            public string? @hexadecimal
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("hexadecimal");
                set => 
                    _wrappedElement.set("hexadecimal", value);
            }

        }

    }

}

public class Verifier
{
    [TypeUri(Uri = "dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768",
        TypeKind = TypeKind.WrappedClass)]
    public class VerifyEntry_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public VerifyEntry_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public VerifyEntry_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768");

        public static VerifyEntry_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

        public string? @itemUri
        {
            get =>
                _wrappedElement.getOrDefault<string?>("itemUri");
            set => 
                _wrappedElement.set("itemUri", value);
        }

        public string? @category
        {
            get =>
                _wrappedElement.getOrDefault<string?>("category");
            set => 
                _wrappedElement.set("category", value);
        }

        public string? @message
        {
            get =>
                _wrappedElement.getOrDefault<string?>("message");
            set => 
                _wrappedElement.set("message", value);
        }

    }

}

