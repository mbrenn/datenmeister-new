using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Core.Models;

public class CommonTypes
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DateTime",
        TypeKind = TypeKind.WrappedClass)]
    public class DateTime_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

    }

    public class Default
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DefaultTypes.Package",
            TypeKind = TypeKind.WrappedClass)]
        public class Package_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            // Not found
            public object? @packagedElement
            {
                get =>
                    innerDmElement.getOrDefault<object?>("packagedElement");
                set => 
                    innerDmElement.set("packagedElement", value);
            }

            // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @preferredType
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("preferredType");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("preferredType", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("preferredType", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper? @preferredPackage
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("preferredPackage");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("preferredPackage", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("preferredPackage", value);
                    }
                }
            }

            public string? @defaultViewMode
            {
                get =>
                    innerDmElement.getOrDefault<string?>("defaultViewMode");
                set => 
                    innerDmElement.set("defaultViewMode", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#1c21ea5b-a9ce-4793-b2f9-590ab2c4e4f1",
            TypeKind = TypeKind.WrappedClass)]
        public class XmiExportContainer_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @xmi
            {
                get =>
                    innerDmElement.getOrDefault<string?>("xmi");
                set => 
                    innerDmElement.set("xmi", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#73c8c24e-6040-4700-b11d-c60f2379523a",
            TypeKind = TypeKind.WrappedClass)]
        public class XmiImportContainer_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @xmi
            {
                get =>
                    innerDmElement.getOrDefault<string?>("xmi");
                set => 
                    innerDmElement.set("xmi", value);
            }

            public string? @property
            {
                get =>
                    innerDmElement.getOrDefault<string?>("property");
                set => 
                    innerDmElement.set("property", value);
            }

            public bool @addToCollection
            {
                get =>
                    innerDmElement.getOrDefault<bool>("addToCollection");
                set => 
                    innerDmElement.set("addToCollection", value);
            }

        }

    }

    public class ExtentManager
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportSettings",
            TypeKind = TypeKind.WrappedClass)]
        public class ImportSettings_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string?>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string? @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string?>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

            public string? @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string?>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentManager.ImportException",
            TypeKind = TypeKind.WrappedClass)]
        public class ImportException_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @message
            {
                get =>
                    innerDmElement.getOrDefault<string?>("message");
                set => 
                    innerDmElement.set("message", value);
            }

        }

    }

    public class OSIntegration
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#CommonTypes.OSIntegration.CommandLineApplication",
            TypeKind = TypeKind.WrappedClass)]
        public class CommandLineApplication_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string? @applicationPath
            {
                get =>
                    innerDmElement.getOrDefault<string?>("applicationPath");
                set => 
                    innerDmElement.set("applicationPath", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#OSIntegration.EnvironmentalVariable",
            TypeKind = TypeKind.WrappedClass)]
        public class EnvironmentalVariable_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string? @value
            {
                get =>
                    innerDmElement.getOrDefault<string?>("value");
                set => 
                    innerDmElement.set("value", value);
            }

        }

    }

}

public class Actions
{
    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.ActionSet",
        TypeKind = TypeKind.WrappedClass)]
    public class ActionSet_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.Actions.Action_Wrapper
        public DatenMeister.Core.Models.Actions.Action_Wrapper? @action
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("action");
                return foundElement == null ? null : new DatenMeister.Core.Models.Actions.Action_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("action", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("action", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.LoggingWriterAction",
        TypeKind = TypeKind.WrappedClass)]
    public class LoggingWriterAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @message
        {
            get =>
                innerDmElement.getOrDefault<string?>("message");
            set => 
                innerDmElement.set("message", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#6f2ea2cd-6218-483c-90a3-4db255e84e82",
        TypeKind = TypeKind.WrappedClass)]
    public class CommandExecutionAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @command
        {
            get =>
                innerDmElement.getOrDefault<string?>("command");
            set => 
                innerDmElement.set("command", value);
        }

        public string? @arguments
        {
            get =>
                innerDmElement.getOrDefault<string?>("arguments");
            set => 
                innerDmElement.set("arguments", value);
        }

        public string? @workingDirectory
        {
            get =>
                innerDmElement.getOrDefault<string?>("workingDirectory");
            set => 
                innerDmElement.set("workingDirectory", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#4090ce13-6718-466c-96df-52d51024aadb",
        TypeKind = TypeKind.WrappedClass)]
    public class PowershellExecutionAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @script
        {
            get =>
                innerDmElement.getOrDefault<string?>("script");
            set => 
                innerDmElement.set("script", value);
        }

        public string? @workingDirectory
        {
            get =>
                innerDmElement.getOrDefault<string?>("workingDirectory");
            set => 
                innerDmElement.set("workingDirectory", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#241b550d-835a-41ea-a32a-bea5d388c6ee",
        TypeKind = TypeKind.WrappedClass)]
    public class LoadExtentAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @configuration
        {
            get =>
                innerDmElement.getOrDefault<object?>("configuration");
            set => 
                innerDmElement.set("configuration", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#c870f6e8-2b70-415c-afaf-b78776b42a09",
        TypeKind = TypeKind.WrappedClass)]
    public class DropExtentAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe",
        TypeKind = TypeKind.WrappedClass)]
    public class CreateWorkspaceAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @annotation
        {
            get =>
                innerDmElement.getOrDefault<string?>("annotation");
            set => 
                innerDmElement.set("annotation", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#db6cc8eb-011c-43e5-b966-cc0e3a1855e8",
        TypeKind = TypeKind.WrappedClass)]
    public class DropWorkspaceAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#8b576580-0f75-4159-ad16-afb7c2268aed",
        TypeKind = TypeKind.WrappedClass)]
    public class CopyElementsAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @sourcePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("sourcePath");
            set => 
                innerDmElement.set("sourcePath", value);
        }

        public string? @targetPath
        {
            get =>
                innerDmElement.getOrDefault<string?>("targetPath");
            set => 
                innerDmElement.set("targetPath", value);
        }

        public bool @moveOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("moveOnly");
            set => 
                innerDmElement.set("moveOnly", value);
        }

        public string? @sourceWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("sourceWorkspace");
            set => 
                innerDmElement.set("sourceWorkspace", value);
        }

        public string? @targetWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("targetWorkspace");
            set => 
                innerDmElement.set("targetWorkspace", value);
        }

        public bool @emptyTarget
        {
            get =>
                innerDmElement.getOrDefault<bool>("emptyTarget");
            set => 
                innerDmElement.set("emptyTarget", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#3c3595a4-026e-4c07-83ec-8a90607b8863",
        TypeKind = TypeKind.WrappedClass)]
    public class ExportToXmiAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @sourcePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("sourcePath");
            set => 
                innerDmElement.set("sourcePath", value);
        }

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @sourceWorkspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("sourceWorkspaceId");
            set => 
                innerDmElement.set("sourceWorkspaceId", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#b70b736b-c9b0-4986-8d92-240fcabc95ae",
        TypeKind = TypeKind.WrappedClass)]
    public class ClearCollectionAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @path
        {
            get =>
                innerDmElement.getOrDefault<string?>("path");
            set => 
                innerDmElement.set("path", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.ItemTransformationActionHandler",
        TypeKind = TypeKind.WrappedClass)]
    public class TransformItemsAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaClass
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("metaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("metaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("metaClass", value);
                }
            }
        }

        public string? @runtimeClass
        {
            get =>
                innerDmElement.getOrDefault<string?>("runtimeClass");
            set => 
                innerDmElement.set("runtimeClass", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @path
        {
            get =>
                innerDmElement.getOrDefault<string?>("path");
            set => 
                innerDmElement.set("path", value);
        }

        public bool @excludeDescendents
        {
            get =>
                innerDmElement.getOrDefault<bool>("excludeDescendents");
            set => 
                innerDmElement.set("excludeDescendents", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Actions.EchoAction",
        TypeKind = TypeKind.WrappedClass)]
    public class EchoAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @shallSuccess
        {
            get =>
                innerDmElement.getOrDefault<string?>("shallSuccess");
            set => 
                innerDmElement.set("shallSuccess", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#04878741-802e-4b7f-8003-21d25f38ac74",
        TypeKind = TypeKind.WrappedClass)]
    public class DocumentOpenAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.CreateFormByMetaclass",
        TypeKind = TypeKind.WrappedClass)]
    public class CreateFormByMetaClass_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @metaClass
        {
            get =>
                innerDmElement.getOrDefault<object?>("metaClass");
            set => 
                innerDmElement.set("metaClass", value);
        }

        public string? @creationMode
        {
            get =>
                innerDmElement.getOrDefault<string?>("creationMode");
            set => 
                innerDmElement.set("creationMode", value);
        }

        // Not found
        public object? @targetContainer
        {
            get =>
                innerDmElement.getOrDefault<object?>("targetContainer");
            set => 
                innerDmElement.set("targetContainer", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    public class Reports
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#Actions.SimpleReportAction",
            TypeKind = TypeKind.WrappedClass)]
        public class SimpleReportAction_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @path
            {
                get =>
                    innerDmElement.getOrDefault<string?>("path");
                set => 
                    innerDmElement.set("path", value);
            }

            // DatenMeister.Core.Models.Reports.SimpleReportConfiguration_Wrapper
            public DatenMeister.Core.Models.Reports.SimpleReportConfiguration_Wrapper? @configuration
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("configuration");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.SimpleReportConfiguration_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("configuration", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("configuration", value);
                    }
                }
            }

            public string? @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string?>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string? @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string?>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#Actions.AdocReportAction",
            TypeKind = TypeKind.WrappedClass)]
        public class AdocReportAction_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string?>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            // DatenMeister.Core.Models.Reports.AdocReportInstance_Wrapper
            public DatenMeister.Core.Models.Reports.AdocReportInstance_Wrapper? @reportInstance
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("reportInstance");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.AdocReportInstance_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("reportInstance", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("reportInstance", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#Actions.HtmlReportAction",
            TypeKind = TypeKind.WrappedClass)]
        public class HtmlReportAction_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @filePath
            {
                get =>
                    innerDmElement.getOrDefault<string?>("filePath");
                set => 
                    innerDmElement.set("filePath", value);
            }

            // DatenMeister.Core.Models.Reports.HtmlReportInstance_Wrapper
            public DatenMeister.Core.Models.Reports.HtmlReportInstance_Wrapper? @reportInstance
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("reportInstance");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.HtmlReportInstance_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("reportInstance", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("reportInstance", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public bool @isDisabled
            {
                get =>
                    innerDmElement.getOrDefault<bool>("isDisabled");
                set => 
                    innerDmElement.set("isDisabled", value);
            }

        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#Actions.Action",
        TypeKind = TypeKind.WrappedClass)]
    public class Action_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Actions.MoveOrCopyAction",
        TypeKind = TypeKind.WrappedClass)]
    public class MoveOrCopyAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @copyMode
        {
            get =>
                innerDmElement.getOrDefault<string?>("copyMode");
            set => 
                innerDmElement.set("copyMode", value);
        }

        // Not found
        public object? @target
        {
            get =>
                innerDmElement.getOrDefault<object?>("target");
            set => 
                innerDmElement.set("target", value);
        }

        // Not found
        public object? @source
        {
            get =>
                innerDmElement.getOrDefault<object?>("source");
            set => 
                innerDmElement.set("source", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#bc4952bf-a3f5-4516-be26-5b773e38bd54",
        TypeKind = TypeKind.WrappedClass)]
    public class MoveUpDownAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @element
        {
            get =>
                innerDmElement.getOrDefault<object?>("element");
            set => 
                innerDmElement.set("element", value);
        }

        // Not found
        public object? @direction
        {
            get =>
                innerDmElement.getOrDefault<object?>("direction");
            set => 
                innerDmElement.set("direction", value);
        }

        // Not found
        public object? @container
        {
            get =>
                innerDmElement.getOrDefault<object?>("container");
            set => 
                innerDmElement.set("container", value);
        }

        public string? @property
        {
            get =>
                innerDmElement.getOrDefault<string?>("property");
            set => 
                innerDmElement.set("property", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#43b0764e-b70f-42bb-b37d-ae8586ec45f1",
        TypeKind = TypeKind.WrappedClass)]
    public class StoreExtentAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d",
        TypeKind = TypeKind.WrappedClass)]
    public class ImportXmiAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @itemUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("itemUri");
            set => 
                innerDmElement.set("itemUri", value);
        }

        public string? @xmi
        {
            get =>
                innerDmElement.getOrDefault<string?>("xmi");
            set => 
                innerDmElement.set("xmi", value);
        }

        public string? @property
        {
            get =>
                innerDmElement.getOrDefault<string?>("property");
            set => 
                innerDmElement.set("property", value);
        }

        public bool @addToCollection
        {
            get =>
                innerDmElement.getOrDefault<bool>("addToCollection");
            set => 
                innerDmElement.set("addToCollection", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#b631bb00-ab11-4a8a-a148-e28abc398503",
        TypeKind = TypeKind.WrappedClass)]
    public class DeletePropertyFromCollectionAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @propertyName
        {
            get =>
                innerDmElement.getOrDefault<string?>("propertyName");
            set => 
                innerDmElement.set("propertyName", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaclass
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("metaclass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("metaclass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("metaclass", value);
                }
            }
        }

        public string? @collectionUrl
        {
            get =>
                innerDmElement.getOrDefault<string?>("collectionUrl");
            set => 
                innerDmElement.set("collectionUrl", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#3223e13a-bbb7-4785-8b81-7275be23b0a1",
        TypeKind = TypeKind.WrappedClass)]
    public class MoveOrCopyActionResult_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @targetUrl
        {
            get =>
                innerDmElement.getOrDefault<string?>("targetUrl");
            set => 
                innerDmElement.set("targetUrl", value);
        }

        public string? @targetWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("targetWorkspace");
            set => 
                innerDmElement.set("targetWorkspace", value);
        }

    }

    public class ParameterTypes
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#90f61e4e-a5ea-42eb-9caa-912d010fbccd",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigationDefineActionParameter_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @actionName
            {
                get =>
                    innerDmElement.getOrDefault<string?>("actionName");
                set => 
                    innerDmElement.set("actionName", value);
            }

            public string? @formUrl
            {
                get =>
                    innerDmElement.getOrDefault<string?>("formUrl");
                set => 
                    innerDmElement.set("formUrl", value);
            }

            public string? @metaClassUrl
            {
                get =>
                    innerDmElement.getOrDefault<string?>("metaClassUrl");
                set => 
                    innerDmElement.set("metaClassUrl", value);
            }

        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#899324b1-85dc-40a1-ba95-dec50509040d",
        TypeKind = TypeKind.WrappedClass)]
    public class ActionResult_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @isSuccess
        {
            get =>
                innerDmElement.getOrDefault<object?>("isSuccess");
            set => 
                innerDmElement.set("isSuccess", value);
        }

        // Not found
        public object? @clientActions
        {
            get =>
                innerDmElement.getOrDefault<object?>("clientActions");
            set => 
                innerDmElement.set("clientActions", value);
        }

    }

    public class ClientActions
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#e07ca80e-2540-4f91-8214-60dbd464e998",
            TypeKind = TypeKind.WrappedClass)]
        public class ClientAction_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @actionName
            {
                get =>
                    innerDmElement.getOrDefault<string?>("actionName");
                set => 
                    innerDmElement.set("actionName", value);
            }

            // Not found
            public object? @element
            {
                get =>
                    innerDmElement.getOrDefault<object?>("element");
                set => 
                    innerDmElement.set("element", value);
            }

            // Not found
            public object? @parameter
            {
                get =>
                    innerDmElement.getOrDefault<object?>("parameter");
                set => 
                    innerDmElement.set("parameter", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#0ee17f2a-5407-4d38-b1b4-34ead2186971",
            TypeKind = TypeKind.WrappedClass)]
        public class AlertClientAction_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @messageText
            {
                get =>
                    innerDmElement.getOrDefault<string?>("messageText");
                set => 
                    innerDmElement.set("messageText", value);
            }

            public string? @actionName
            {
                get =>
                    innerDmElement.getOrDefault<string?>("actionName");
                set => 
                    innerDmElement.set("actionName", value);
            }

            // Not found
            public object? @element
            {
                get =>
                    innerDmElement.getOrDefault<object?>("element");
                set => 
                    innerDmElement.set("element", value);
            }

            // Not found
            public object? @parameter
            {
                get =>
                    innerDmElement.getOrDefault<object?>("parameter");
                set => 
                    innerDmElement.set("parameter", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#3251783f-2683-4c24-bad5-828930028462",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigateToExtentClientAction_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string?>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string? @extentUri
            {
                get =>
                    innerDmElement.getOrDefault<string?>("extentUri");
                set => 
                    innerDmElement.set("extentUri", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#5f69675e-df58-4ad7-84bf-359cdfba5db4",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigateToItemClientAction_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @workspaceId
            {
                get =>
                    innerDmElement.getOrDefault<string?>("workspaceId");
                set => 
                    innerDmElement.set("workspaceId", value);
            }

            public string? @itemUrl
            {
                get =>
                    innerDmElement.getOrDefault<string?>("itemUrl");
                set => 
                    innerDmElement.set("itemUrl", value);
            }

            public string? @formUri
            {
                get =>
                    innerDmElement.getOrDefault<string?>("formUri");
                set => 
                    innerDmElement.set("formUri", value);
            }

        }

    }

}

public class DataViews
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DataView",
        TypeKind = TypeKind.WrappedClass)]
    public class DataView_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @uri
        {
            get =>
                innerDmElement.getOrDefault<string?>("uri");
            set => 
                innerDmElement.set("uri", value);
        }

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @viewNode
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("viewNode");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("viewNode", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("viewNode", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.ViewNode",
        TypeKind = TypeKind.WrappedClass)]
    public class ViewNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByExtentNode",
        TypeKind = TypeKind.WrappedClass)]
    public class SelectByExtentNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FlattenNode",
        TypeKind = TypeKind.WrappedClass)]
    public class FlattenNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByPropertyValueNode",
        TypeKind = TypeKind.WrappedClass)]
    public class RowFilterByPropertyValueNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        public string? @property
        {
            get =>
                innerDmElement.getOrDefault<string?>("property");
            set => 
                innerDmElement.set("property", value);
        }

        public string? @value
        {
            get =>
                innerDmElement.getOrDefault<string?>("value");
            set => 
                innerDmElement.set("value", value);
        }

        // Not found
        public object? @comparisonMode
        {
            get =>
                innerDmElement.getOrDefault<object?>("comparisonMode");
            set => 
                innerDmElement.set("comparisonMode", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.FilterByMetaclassNode",
        TypeKind = TypeKind.WrappedClass)]
    public class RowFilterByMetaclassNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        // Not found
        public object? @metaClass
        {
            get =>
                innerDmElement.getOrDefault<object?>("metaClass");
            set => 
                innerDmElement.set("metaClass", value);
        }

        public bool @includeInherits
        {
            get =>
                innerDmElement.getOrDefault<bool>("includeInherits");
            set => 
                innerDmElement.set("includeInherits", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByFullNameNode",
        TypeKind = TypeKind.WrappedClass)]
    public class SelectByFullNameNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        public string? @path
        {
            get =>
                innerDmElement.getOrDefault<string?>("path");
            set => 
                innerDmElement.set("path", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.DynamicSourceNode",
        TypeKind = TypeKind.WrappedClass)]
    public class DynamicSourceNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @nodeName
        {
            get =>
                innerDmElement.getOrDefault<string?>("nodeName");
            set => 
                innerDmElement.set("nodeName", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.SelectByPathNode",
        TypeKind = TypeKind.WrappedClass)]
    public class SelectByPathNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @path
        {
            get =>
                innerDmElement.getOrDefault<string?>("path");
            set => 
                innerDmElement.set("path", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.DataViews.QueryStatement",
        TypeKind = TypeKind.WrappedClass)]
    public class QueryStatement_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @nodes
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("nodes");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("nodes", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("nodes", value);
                }
            }
        }

        // Not found
        public object? @resultNode
        {
            get =>
                innerDmElement.getOrDefault<object?>("resultNode");
            set => 
                innerDmElement.set("resultNode", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#a890d5ec-2686-4f18-9f9f-7037c7fe226a",
        TypeKind = TypeKind.WrappedClass)]
    public class SelectFromAllWorkspacesNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#a7276e99-351c-4aed-8ff1-a4b5ee45b0db",
        TypeKind = TypeKind.WrappedClass)]
    public class SelectByWorkspaceNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#abca8647-18d7-4322-a803-2e3e1cd123d7",
        TypeKind = TypeKind.WrappedClass)]
    public class ColumnFilterExcludeNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @columnNamesComma
        {
            get =>
                innerDmElement.getOrDefault<string?>("columnNamesComma");
            set => 
                innerDmElement.set("columnNamesComma", value);
        }

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#00d223b8-4335-4ee3-9359-92354e2d669d",
        TypeKind = TypeKind.WrappedClass)]
    public class ColumnFilterIncludeOnlyNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @columnNamesComma
        {
            get =>
                innerDmElement.getOrDefault<string?>("columnNamesComma");
            set => 
                innerDmElement.set("columnNamesComma", value);
        }

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#d705b34b-369f-4b44-9a00-013e1daa759f",
        TypeKind = TypeKind.WrappedClass)]
    public class RowFilterOnPositionNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        public int @amount
        {
            get =>
                innerDmElement.getOrDefault<int>("amount");
            set => 
                innerDmElement.set("amount", value);
        }

        public int @number
        {
            get =>
                innerDmElement.getOrDefault<int>("number");
            set => 
                innerDmElement.set("number", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#e6948145-e1b7-4542-84e5-269dab1aa4c9",
        TypeKind = TypeKind.WrappedClass)]
    public class RowOrderByNode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
        public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @input
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("input");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("input", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("input", value);
                }
            }
        }

        public string? @propertyName
        {
            get =>
                innerDmElement.getOrDefault<string?>("propertyName");
            set => 
                innerDmElement.set("propertyName", value);
        }

        public bool @orderDescending
        {
            get =>
                innerDmElement.getOrDefault<bool>("orderDescending");
            set => 
                innerDmElement.set("orderDescending", value);
        }

    }

}

public class Reports
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportDefinition",
        TypeKind = TypeKind.WrappedClass)]
    public class ReportDefinition_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        // DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper
        public DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper? @elements
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("elements");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("elements", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("elements", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstanceSource",
        TypeKind = TypeKind.WrappedClass)]
    public class ReportInstanceSource_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @path
        {
            get =>
                innerDmElement.getOrDefault<string?>("path");
            set => 
                innerDmElement.set("path", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportInstance",
        TypeKind = TypeKind.WrappedClass)]
    public class ReportInstance_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper
        public DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper? @reportDefinition
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("reportDefinition");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("reportDefinition", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("reportDefinition", value);
                }
            }
        }

        // DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper
        public DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper? @sources
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("sources");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("sources", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("sources", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Adoc.AdocReportInstance",
        TypeKind = TypeKind.WrappedClass)]
    public class AdocReportInstance_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper
        public DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper? @reportDefinition
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("reportDefinition");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("reportDefinition", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("reportDefinition", value);
                }
            }
        }

        // DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper
        public DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper? @sources
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("sources");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("sources", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("sources", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Html.HtmlReportInstance",
        TypeKind = TypeKind.WrappedClass)]
    public class HtmlReportInstance_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @cssFile
        {
            get =>
                innerDmElement.getOrDefault<string?>("cssFile");
            set => 
                innerDmElement.set("cssFile", value);
        }

        public string? @cssStyleSheet
        {
            get =>
                innerDmElement.getOrDefault<string?>("cssStyleSheet");
            set => 
                innerDmElement.set("cssStyleSheet", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper
        public DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper? @reportDefinition
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("reportDefinition");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("reportDefinition", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("reportDefinition", value);
                }
            }
        }

        // DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper
        public DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper? @sources
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("sources");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.ReportInstanceSource_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("sources", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("sources", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.Simple.SimpleReportConfiguration",
        TypeKind = TypeKind.WrappedClass)]
    public class SimpleReportConfiguration_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @showDescendents
        {
            get =>
                innerDmElement.getOrDefault<bool>("showDescendents");
            set => 
                innerDmElement.set("showDescendents", value);
        }

        public string? @rootElement
        {
            get =>
                innerDmElement.getOrDefault<string?>("rootElement");
            set => 
                innerDmElement.set("rootElement", value);
        }

        public bool @showRootElement
        {
            get =>
                innerDmElement.getOrDefault<bool>("showRootElement");
            set => 
                innerDmElement.set("showRootElement", value);
        }

        public bool @showMetaClasses
        {
            get =>
                innerDmElement.getOrDefault<bool>("showMetaClasses");
            set => 
                innerDmElement.set("showMetaClasses", value);
        }

        public bool @showFullName
        {
            get =>
                innerDmElement.getOrDefault<bool>("showFullName");
            set => 
                innerDmElement.set("showFullName", value);
        }

        // Not found
        public object? @form
        {
            get =>
                innerDmElement.getOrDefault<object?>("form");
            set => 
                innerDmElement.set("form", value);
        }

        // Not found
        public object? @descendentMode
        {
            get =>
                innerDmElement.getOrDefault<object?>("descendentMode");
            set => 
                innerDmElement.set("descendentMode", value);
        }

        // Not found
        public object? @typeMode
        {
            get =>
                innerDmElement.getOrDefault<object?>("typeMode");
            set => 
                innerDmElement.set("typeMode", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

    }

    public class Elements
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportElement",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportElement_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportHeadline",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportHeadline_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @title
            {
                get =>
                    innerDmElement.getOrDefault<string?>("title");
                set => 
                    innerDmElement.set("title", value);
            }

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportParagraph",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportParagraph_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @paragraph
            {
                get =>
                    innerDmElement.getOrDefault<string?>("paragraph");
                set => 
                    innerDmElement.set("paragraph", value);
            }

            public string? @cssClass
            {
                get =>
                    innerDmElement.getOrDefault<string?>("cssClass");
                set => 
                    innerDmElement.set("cssClass", value);
            }

            // Not found
            public object? @viewNode
            {
                get =>
                    innerDmElement.getOrDefault<object?>("viewNode");
                set => 
                    innerDmElement.set("viewNode", value);
            }

            public string? @evalProperties
            {
                get =>
                    innerDmElement.getOrDefault<string?>("evalProperties");
                set => 
                    innerDmElement.set("evalProperties", value);
            }

            public string? @evalParagraph
            {
                get =>
                    innerDmElement.getOrDefault<string?>("evalParagraph");
                set => 
                    innerDmElement.set("evalParagraph", value);
            }

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportTable",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportTable_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @cssClass
            {
                get =>
                    innerDmElement.getOrDefault<string?>("cssClass");
                set => 
                    innerDmElement.set("cssClass", value);
            }

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @viewNode
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("viewNode");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("viewNode", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("viewNode", value);
                    }
                }
            }

            // DatenMeister.Core.Models.Forms.TableForm_Wrapper
            public DatenMeister.Core.Models.Forms.TableForm_Wrapper? @form
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("form");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Forms.TableForm_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("form", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("form", value);
                    }
                }
            }

            public string? @evalProperties
            {
                get =>
                    innerDmElement.getOrDefault<string?>("evalProperties");
                set => 
                    innerDmElement.set("evalProperties", value);
            }

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Reports.ReportLoop",
            TypeKind = TypeKind.WrappedClass)]
        public class ReportLoop_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            // DatenMeister.Core.Models.DataViews.ViewNode_Wrapper
            public DatenMeister.Core.Models.DataViews.ViewNode_Wrapper? @viewNode
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("viewNode");
                    return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.ViewNode_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("viewNode", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("viewNode", value);
                    }
                }
            }

            // DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper
            public DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper? @elements
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("elements");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("elements", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("elements", value);
                    }
                }
            }

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

        }

    }

}

public class ExtentLoaderConfigs
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @fixRowCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixRowCount");
            set => 
                innerDmElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixColumnCount");
            set => 
                innerDmElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                innerDmElement.getOrDefault<string?>("sheetName");
            set => 
                innerDmElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetRow");
            set => 
                innerDmElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetColumn");
            set => 
                innerDmElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                innerDmElement.getOrDefault<int>("countRows");
            set => 
                innerDmElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                innerDmElement.getOrDefault<int>("countColumns");
            set => 
                innerDmElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                innerDmElement.getOrDefault<bool>("hasHeader");
            set => 
                innerDmElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                innerDmElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                innerDmElement.getOrDefault<bool>("onlySetColumns");
            set => 
                innerDmElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                innerDmElement.getOrDefault<string?>("idColumnName");
            set => 
                innerDmElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                innerDmElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelReferenceLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelReferenceLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @fixRowCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixRowCount");
            set => 
                innerDmElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixColumnCount");
            set => 
                innerDmElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                innerDmElement.getOrDefault<string?>("sheetName");
            set => 
                innerDmElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetRow");
            set => 
                innerDmElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetColumn");
            set => 
                innerDmElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                innerDmElement.getOrDefault<int>("countRows");
            set => 
                innerDmElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                innerDmElement.getOrDefault<int>("countColumns");
            set => 
                innerDmElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                innerDmElement.getOrDefault<bool>("hasHeader");
            set => 
                innerDmElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                innerDmElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                innerDmElement.getOrDefault<bool>("onlySetColumns");
            set => 
                innerDmElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                innerDmElement.getOrDefault<string?>("idColumnName");
            set => 
                innerDmElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                innerDmElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelImportLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelImportLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @extentPath
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentPath");
            set => 
                innerDmElement.set("extentPath", value);
        }

        public bool @fixRowCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixRowCount");
            set => 
                innerDmElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixColumnCount");
            set => 
                innerDmElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                innerDmElement.getOrDefault<string?>("sheetName");
            set => 
                innerDmElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetRow");
            set => 
                innerDmElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetColumn");
            set => 
                innerDmElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                innerDmElement.getOrDefault<int>("countRows");
            set => 
                innerDmElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                innerDmElement.getOrDefault<int>("countColumns");
            set => 
                innerDmElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                innerDmElement.getOrDefault<bool>("hasHeader");
            set => 
                innerDmElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                innerDmElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                innerDmElement.getOrDefault<bool>("onlySetColumns");
            set => 
                innerDmElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                innerDmElement.getOrDefault<string?>("idColumnName");
            set => 
                innerDmElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                innerDmElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExcelExtentLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelExtentLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @idColumnName
        {
            get =>
                innerDmElement.getOrDefault<string?>("idColumnName");
            set => 
                innerDmElement.set("idColumnName", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.InMemoryLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class InMemoryLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @isLinkedList
        {
            get =>
                innerDmElement.getOrDefault<bool>("isLinkedList");
            set => 
                innerDmElement.set("isLinkedList", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmlReferenceLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class XmlReferenceLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public bool @keepNamespaces
        {
            get =>
                innerDmElement.getOrDefault<bool>("keepNamespaces");
            set => 
                innerDmElement.set("keepNamespaces", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.ExtentFileLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentFileLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.XmiStorageLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class XmiStorageLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvExtentLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class CsvExtentLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.ExtentLoaderConfigs.CsvSettings_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.CsvSettings_Wrapper? @settings
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("settings");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.CsvSettings_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("settings", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("settings", value);
                }
            }
        }

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ExtentLoaderConfigs.CsvSettings",
        TypeKind = TypeKind.WrappedClass)]
    public class CsvSettings_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @encoding
        {
            get =>
                innerDmElement.getOrDefault<string?>("encoding");
            set => 
                innerDmElement.set("encoding", value);
        }

        public bool @hasHeader
        {
            get =>
                innerDmElement.getOrDefault<bool>("hasHeader");
            set => 
                innerDmElement.set("hasHeader", value);
        }

        // Not found
        public object? @separator
        {
            get =>
                innerDmElement.getOrDefault<object?>("separator");
            set => 
                innerDmElement.set("separator", value);
        }

        public string? @columns
        {
            get =>
                innerDmElement.getOrDefault<string?>("columns");
            set => 
                innerDmElement.set("columns", value);
        }

        public string? @metaclassUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("metaclassUri");
            set => 
                innerDmElement.set("metaclassUri", value);
        }

        public bool @trimCells
        {
            get =>
                innerDmElement.getOrDefault<bool>("trimCells");
            set => 
                innerDmElement.set("trimCells", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelHierarchicalColumnDefinition_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaClass
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("metaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("metaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("metaClass", value);
                }
            }
        }

        public string? @property
        {
            get =>
                innerDmElement.getOrDefault<string?>("property");
            set => 
                innerDmElement.set("property", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#ExtentLoaderConfigs.ExcelHierarchicalLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelHierarchicalLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition_Wrapper? @hierarchicalColumns
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("hierarchicalColumns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelHierarchicalColumnDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("hierarchicalColumns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("hierarchicalColumns", value);
                }
            }
        }

        public bool @skipElementsForLastLevel
        {
            get =>
                innerDmElement.getOrDefault<bool>("skipElementsForLastLevel");
            set => 
                innerDmElement.set("skipElementsForLastLevel", value);
        }

        public bool @fixRowCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixRowCount");
            set => 
                innerDmElement.set("fixRowCount", value);
        }

        public bool @fixColumnCount
        {
            get =>
                innerDmElement.getOrDefault<bool>("fixColumnCount");
            set => 
                innerDmElement.set("fixColumnCount", value);
        }

        public string? @filePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("filePath");
            set => 
                innerDmElement.set("filePath", value);
        }

        public string? @sheetName
        {
            get =>
                innerDmElement.getOrDefault<string?>("sheetName");
            set => 
                innerDmElement.set("sheetName", value);
        }

        public int @offsetRow
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetRow");
            set => 
                innerDmElement.set("offsetRow", value);
        }

        public int @offsetColumn
        {
            get =>
                innerDmElement.getOrDefault<int>("offsetColumn");
            set => 
                innerDmElement.set("offsetColumn", value);
        }

        public int @countRows
        {
            get =>
                innerDmElement.getOrDefault<int>("countRows");
            set => 
                innerDmElement.set("countRows", value);
        }

        public int @countColumns
        {
            get =>
                innerDmElement.getOrDefault<int>("countColumns");
            set => 
                innerDmElement.set("countColumns", value);
        }

        public bool @hasHeader
        {
            get =>
                innerDmElement.getOrDefault<bool>("hasHeader");
            set => 
                innerDmElement.set("hasHeader", value);
        }

        public bool @tryMergedHeaderCells
        {
            get =>
                innerDmElement.getOrDefault<bool>("tryMergedHeaderCells");
            set => 
                innerDmElement.set("tryMergedHeaderCells", value);
        }

        public bool @onlySetColumns
        {
            get =>
                innerDmElement.getOrDefault<bool>("onlySetColumns");
            set => 
                innerDmElement.set("onlySetColumns", value);
        }

        public string? @idColumnName
        {
            get =>
                innerDmElement.getOrDefault<string?>("idColumnName");
            set => 
                innerDmElement.set("idColumnName", value);
        }

        public int @skipEmptyRowsCount
        {
            get =>
                innerDmElement.getOrDefault<int>("skipEmptyRowsCount");
            set => 
                innerDmElement.set("skipEmptyRowsCount", value);
        }

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper? @columns
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("columns");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExcelColumn_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("columns", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("columns", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#6ff62c94-2eaf-4bd3-aa98-16e3d9b0be0a",
        TypeKind = TypeKind.WrappedClass)]
    public class ExcelColumn_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @header
        {
            get =>
                innerDmElement.getOrDefault<string?>("header");
            set => 
                innerDmElement.set("header", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#10151dfc-f18b-4a58-9434-da1be1e030a3",
        TypeKind = TypeKind.WrappedClass)]
    public class EnvironmentalVariableLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

}

public class Forms
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class FieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SortingOrder",
        TypeKind = TypeKind.WrappedClass)]
    public class SortingOrder_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDescending
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDescending");
            set => 
                innerDmElement.set("isDescending", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.AnyDataFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class AnyDataFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class CheckboxFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public int @lineHeight
        {
            get =>
                innerDmElement.getOrDefault<int>("lineHeight");
            set => 
                innerDmElement.set("lineHeight", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ActionFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class ActionFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @actionName
        {
            get =>
                innerDmElement.getOrDefault<string?>("actionName");
            set => 
                innerDmElement.set("actionName", value);
        }

        // Not found
        public object? @parameter
        {
            get =>
                innerDmElement.getOrDefault<object?>("parameter");
            set => 
                innerDmElement.set("parameter", value);
        }

        public string? @buttonText
        {
            get =>
                innerDmElement.getOrDefault<string?>("buttonText");
            set => 
                innerDmElement.set("buttonText", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DateTimeFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class DateTimeFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @hideDate
        {
            get =>
                innerDmElement.getOrDefault<bool>("hideDate");
            set => 
                innerDmElement.set("hideDate", value);
        }

        public bool @hideTime
        {
            get =>
                innerDmElement.getOrDefault<bool>("hideTime");
            set => 
                innerDmElement.set("hideTime", value);
        }

        public bool @showOffsetButtons
        {
            get =>
                innerDmElement.getOrDefault<bool>("showOffsetButtons");
            set => 
                innerDmElement.set("showOffsetButtons", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FormAssociation",
        TypeKind = TypeKind.WrappedClass)]
    public class FormAssociation_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // Not found
        public object? @formType
        {
            get =>
                innerDmElement.getOrDefault<object?>("formType");
            set => 
                innerDmElement.set("formType", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper? @metaClass
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("metaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("metaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("metaClass", value);
                }
            }
        }

        public string? @extentType
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentType");
            set => 
                innerDmElement.set("extentType", value);
        }

        public string? @viewModeId
        {
            get =>
                innerDmElement.getOrDefault<string?>("viewModeId");
            set => 
                innerDmElement.set("viewModeId", value);
        }

        // DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper
        public DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper? @parentMetaClass
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("parentMetaClass");
                return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Classification.Classifier_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("parentMetaClass", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("parentMetaClass", value);
                }
            }
        }

        public string? @parentProperty
        {
            get =>
                innerDmElement.getOrDefault<string?>("parentProperty");
            set => 
                innerDmElement.set("parentProperty", value);
        }

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @form
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("form");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("form", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("form", value);
                }
            }
        }

        public bool @debugActive
        {
            get =>
                innerDmElement.getOrDefault<bool>("debugActive");
            set => 
                innerDmElement.set("debugActive", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class DropDownFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.Forms.ValuePair_Wrapper
        public DatenMeister.Core.Models.Forms.ValuePair_Wrapper? @values
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("values");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.ValuePair_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("values", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("values", value);
                }
            }
        }

        // Not found
        public object? @valuesByEnumeration
        {
            get =>
                innerDmElement.getOrDefault<object?>("valuesByEnumeration");
            set => 
                innerDmElement.set("valuesByEnumeration", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ValuePair",
        TypeKind = TypeKind.WrappedClass)]
    public class ValuePair_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @value
        {
            get =>
                innerDmElement.getOrDefault<object?>("value");
            set => 
                innerDmElement.set("value", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.MetaClassElementFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class MetaClassElementFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ReferenceFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class ReferenceFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @isSelectionInline
        {
            get =>
                innerDmElement.getOrDefault<bool>("isSelectionInline");
            set => 
                innerDmElement.set("isSelectionInline", value);
        }

        public string? @defaultWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("defaultWorkspace");
            set => 
                innerDmElement.set("defaultWorkspace", value);
        }

        public string? @defaultItemUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("defaultItemUri");
            set => 
                innerDmElement.set("defaultItemUri", value);
        }

        public bool @showAllChildren
        {
            get =>
                innerDmElement.getOrDefault<bool>("showAllChildren");
            set => 
                innerDmElement.set("showAllChildren", value);
        }

        public bool @showWorkspaceSelection
        {
            get =>
                innerDmElement.getOrDefault<bool>("showWorkspaceSelection");
            set => 
                innerDmElement.set("showWorkspaceSelection", value);
        }

        public bool @showExtentSelection
        {
            get =>
                innerDmElement.getOrDefault<bool>("showExtentSelection");
            set => 
                innerDmElement.set("showExtentSelection", value);
        }

        // Not found
        public object? @metaClassFilter
        {
            get =>
                innerDmElement.getOrDefault<object?>("metaClassFilter");
            set => 
                innerDmElement.set("metaClassFilter", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SubElementFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class SubElementFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @metaClass
        {
            get =>
                innerDmElement.getOrDefault<object?>("metaClass");
            set => 
                innerDmElement.set("metaClass", value);
        }

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @form
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("form");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("form", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("form", value);
                }
            }
        }

        public bool @allowOnlyExistingElements
        {
            get =>
                innerDmElement.getOrDefault<bool>("allowOnlyExistingElements");
            set => 
                innerDmElement.set("allowOnlyExistingElements", value);
        }

        // Not found
        public object? @defaultTypesForNewElements
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultTypesForNewElements");
            set => 
                innerDmElement.set("defaultTypesForNewElements", value);
        }

        public bool @includeSpecializationsForDefaultTypes
        {
            get =>
                innerDmElement.getOrDefault<bool>("includeSpecializationsForDefaultTypes");
            set => 
                innerDmElement.set("includeSpecializationsForDefaultTypes", value);
        }

        public string? @defaultWorkspaceOfNewElements
        {
            get =>
                innerDmElement.getOrDefault<string?>("defaultWorkspaceOfNewElements");
            set => 
                innerDmElement.set("defaultWorkspaceOfNewElements", value);
        }

        public string? @defaultExtentOfNewElements
        {
            get =>
                innerDmElement.getOrDefault<string?>("defaultExtentOfNewElements");
            set => 
                innerDmElement.set("defaultExtentOfNewElements", value);
        }

        public string? @actionName
        {
            get =>
                innerDmElement.getOrDefault<string?>("actionName");
            set => 
                innerDmElement.set("actionName", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TextFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class TextFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public int @lineHeight
        {
            get =>
                innerDmElement.getOrDefault<int>("lineHeight");
            set => 
                innerDmElement.set("lineHeight", value);
        }

        public int @width
        {
            get =>
                innerDmElement.getOrDefault<int>("width");
            set => 
                innerDmElement.set("width", value);
        }

        public int @shortenTextLength
        {
            get =>
                innerDmElement.getOrDefault<int>("shortenTextLength");
            set => 
                innerDmElement.set("shortenTextLength", value);
        }

        public bool @supportClipboardCopy
        {
            get =>
                innerDmElement.getOrDefault<bool>("supportClipboardCopy");
            set => 
                innerDmElement.set("supportClipboardCopy", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.EvalTextFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class EvalTextFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @evalCellProperties
        {
            get =>
                innerDmElement.getOrDefault<string?>("evalCellProperties");
            set => 
                innerDmElement.set("evalCellProperties", value);
        }

        public int @lineHeight
        {
            get =>
                innerDmElement.getOrDefault<int>("lineHeight");
            set => 
                innerDmElement.set("lineHeight", value);
        }

        public int @width
        {
            get =>
                innerDmElement.getOrDefault<int>("width");
            set => 
                innerDmElement.set("width", value);
        }

        public int @shortenTextLength
        {
            get =>
                innerDmElement.getOrDefault<int>("shortenTextLength");
            set => 
                innerDmElement.set("shortenTextLength", value);
        }

        public bool @supportClipboardCopy
        {
            get =>
                innerDmElement.getOrDefault<bool>("supportClipboardCopy");
            set => 
                innerDmElement.set("supportClipboardCopy", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.SeparatorLineFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class SeparatorLineFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public int @Height
        {
            get =>
                innerDmElement.getOrDefault<int>("Height");
            set => 
                innerDmElement.set("Height", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FileSelectionFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class FileSelectionFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @defaultExtension
        {
            get =>
                innerDmElement.getOrDefault<string?>("defaultExtension");
            set => 
                innerDmElement.set("defaultExtension", value);
        }

        public bool @isSaving
        {
            get =>
                innerDmElement.getOrDefault<bool>("isSaving");
            set => 
                innerDmElement.set("isSaving", value);
        }

        public string? @initialPathToDirectory
        {
            get =>
                innerDmElement.getOrDefault<string?>("initialPathToDirectory");
            set => 
                innerDmElement.set("initialPathToDirectory", value);
        }

        public string? @filter
        {
            get =>
                innerDmElement.getOrDefault<string?>("filter");
            set => 
                innerDmElement.set("filter", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DefaultTypeForNewElement",
        TypeKind = TypeKind.WrappedClass)]
    public class DefaultTypeForNewElement_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // Not found
        public object? @metaClass
        {
            get =>
                innerDmElement.getOrDefault<object?>("metaClass");
            set => 
                innerDmElement.set("metaClass", value);
        }

        public string? @parentProperty
        {
            get =>
                innerDmElement.getOrDefault<string?>("parentProperty");
            set => 
                innerDmElement.set("parentProperty", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.FullNameFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class FullNameFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CheckboxListTaggingFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class CheckboxListTaggingFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.Forms.ValuePair_Wrapper
        public DatenMeister.Core.Models.Forms.ValuePair_Wrapper? @values
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("values");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.ValuePair_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("values", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("values", value);
                }
            }
        }

        public string? @separator
        {
            get =>
                innerDmElement.getOrDefault<string?>("separator");
            set => 
                innerDmElement.set("separator", value);
        }

        public bool @containsFreeText
        {
            get =>
                innerDmElement.getOrDefault<bool>("containsFreeText");
            set => 
                innerDmElement.set("containsFreeText", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.NumberFieldData",
        TypeKind = TypeKind.WrappedClass)]
    public class NumberFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @format
        {
            get =>
                innerDmElement.getOrDefault<string?>("format");
            set => 
                innerDmElement.set("format", value);
        }

        public bool @isInteger
        {
            get =>
                innerDmElement.getOrDefault<bool>("isInteger");
            set => 
                innerDmElement.set("isInteger", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.Form",
        TypeKind = TypeKind.WrappedClass)]
    public class Form_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                innerDmElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                innerDmElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                innerDmElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalUri");
            set => 
                innerDmElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalWorkspace");
            set => 
                innerDmElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                innerDmElement.getOrDefault<string?>("creationProtocol");
            set => 
                innerDmElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.RowForm",
        TypeKind = TypeKind.WrappedClass)]
    public class RowForm_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @buttonApplyText
        {
            get =>
                innerDmElement.getOrDefault<string?>("buttonApplyText");
            set => 
                innerDmElement.set("buttonApplyText", value);
        }

        public bool @allowNewProperties
        {
            get =>
                innerDmElement.getOrDefault<bool>("allowNewProperties");
            set => 
                innerDmElement.set("allowNewProperties", value);
        }

        public int @defaultWidth
        {
            get =>
                innerDmElement.getOrDefault<int>("defaultWidth");
            set => 
                innerDmElement.set("defaultWidth", value);
        }

        public int @defaultHeight
        {
            get =>
                innerDmElement.getOrDefault<int>("defaultHeight");
            set => 
                innerDmElement.set("defaultHeight", value);
        }

        // DatenMeister.Core.Models.Forms.FieldData_Wrapper
        public DatenMeister.Core.Models.Forms.FieldData_Wrapper? @field
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("field");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FieldData_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("field", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("field", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                innerDmElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                innerDmElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                innerDmElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalUri");
            set => 
                innerDmElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalWorkspace");
            set => 
                innerDmElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                innerDmElement.getOrDefault<string?>("creationProtocol");
            set => 
                innerDmElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.TableForm",
        TypeKind = TypeKind.WrappedClass)]
    public class TableForm_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @property
        {
            get =>
                innerDmElement.getOrDefault<string?>("property");
            set => 
                innerDmElement.set("property", value);
        }

        // Not found
        public object? @metaClass
        {
            get =>
                innerDmElement.getOrDefault<object?>("metaClass");
            set => 
                innerDmElement.set("metaClass", value);
        }

        public bool @includeDescendents
        {
            get =>
                innerDmElement.getOrDefault<bool>("includeDescendents");
            set => 
                innerDmElement.set("includeDescendents", value);
        }

        public bool @noItemsWithMetaClass
        {
            get =>
                innerDmElement.getOrDefault<bool>("noItemsWithMetaClass");
            set => 
                innerDmElement.set("noItemsWithMetaClass", value);
        }

        public bool @inhibitNewItems
        {
            get =>
                innerDmElement.getOrDefault<bool>("inhibitNewItems");
            set => 
                innerDmElement.set("inhibitNewItems", value);
        }

        public bool @inhibitDeleteItems
        {
            get =>
                innerDmElement.getOrDefault<bool>("inhibitDeleteItems");
            set => 
                innerDmElement.set("inhibitDeleteItems", value);
        }

        public bool @inhibitEditItems
        {
            get =>
                innerDmElement.getOrDefault<bool>("inhibitEditItems");
            set => 
                innerDmElement.set("inhibitEditItems", value);
        }

        // DatenMeister.Core.Models.Forms.DefaultTypeForNewElement_Wrapper
        public DatenMeister.Core.Models.Forms.DefaultTypeForNewElement_Wrapper? @defaultTypesForNewElements
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("defaultTypesForNewElements");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.DefaultTypeForNewElement_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("defaultTypesForNewElements", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("defaultTypesForNewElements", value);
                }
            }
        }

        // Not found
        public object? @fastViewFilters
        {
            get =>
                innerDmElement.getOrDefault<object?>("fastViewFilters");
            set => 
                innerDmElement.set("fastViewFilters", value);
        }

        // DatenMeister.Core.Models.Forms.FieldData_Wrapper
        public DatenMeister.Core.Models.Forms.FieldData_Wrapper? @field
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("field");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FieldData_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("field", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("field", value);
                }
            }
        }

        // DatenMeister.Core.Models.Forms.SortingOrder_Wrapper
        public DatenMeister.Core.Models.Forms.SortingOrder_Wrapper? @sortingOrder
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("sortingOrder");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.SortingOrder_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("sortingOrder", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("sortingOrder", value);
                }
            }
        }

        // Not found
        public object? @viewNode
        {
            get =>
                innerDmElement.getOrDefault<object?>("viewNode");
            set => 
                innerDmElement.set("viewNode", value);
        }

        public bool @autoGenerateFields
        {
            get =>
                innerDmElement.getOrDefault<bool>("autoGenerateFields");
            set => 
                innerDmElement.set("autoGenerateFields", value);
        }

        public bool @duplicatePerType
        {
            get =>
                innerDmElement.getOrDefault<bool>("duplicatePerType");
            set => 
                innerDmElement.set("duplicatePerType", value);
        }

        public string? @dataUrl
        {
            get =>
                innerDmElement.getOrDefault<string?>("dataUrl");
            set => 
                innerDmElement.set("dataUrl", value);
        }

        public bool @inhibitNewUnclassifiedItems
        {
            get =>
                innerDmElement.getOrDefault<bool>("inhibitNewUnclassifiedItems");
            set => 
                innerDmElement.set("inhibitNewUnclassifiedItems", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                innerDmElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                innerDmElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                innerDmElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalUri");
            set => 
                innerDmElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalWorkspace");
            set => 
                innerDmElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                innerDmElement.getOrDefault<string?>("creationProtocol");
            set => 
                innerDmElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.CollectionForm",
        TypeKind = TypeKind.WrappedClass)]
    public class CollectionForm_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @tab
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("tab");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("tab", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("tab", value);
                }
            }
        }

        public bool @autoTabs
        {
            get =>
                innerDmElement.getOrDefault<bool>("autoTabs");
            set => 
                innerDmElement.set("autoTabs", value);
        }

        // DatenMeister.Core.Models.Forms.FieldData_Wrapper
        public DatenMeister.Core.Models.Forms.FieldData_Wrapper? @field
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("field");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FieldData_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("field", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("field", value);
                }
            }
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                innerDmElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                innerDmElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                innerDmElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalUri");
            set => 
                innerDmElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalWorkspace");
            set => 
                innerDmElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                innerDmElement.getOrDefault<string?>("creationProtocol");
            set => 
                innerDmElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ObjectForm",
        TypeKind = TypeKind.WrappedClass)]
    public class ObjectForm_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.Forms.Form_Wrapper
        public DatenMeister.Core.Models.Forms.Form_Wrapper? @tab
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("tab");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.Form_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("tab", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("tab", value);
                }
            }
        }

        public bool @autoTabs
        {
            get =>
                innerDmElement.getOrDefault<bool>("autoTabs");
            set => 
                innerDmElement.set("autoTabs", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

        public bool @isAutoGenerated
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAutoGenerated");
            set => 
                innerDmElement.set("isAutoGenerated", value);
        }

        public bool @hideMetaInformation
        {
            get =>
                innerDmElement.getOrDefault<bool>("hideMetaInformation");
            set => 
                innerDmElement.set("hideMetaInformation", value);
        }

        public string? @originalUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalUri");
            set => 
                innerDmElement.set("originalUri", value);
        }

        public string? @originalWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("originalWorkspace");
            set => 
                innerDmElement.set("originalWorkspace", value);
        }

        public string? @creationProtocol
        {
            get =>
                innerDmElement.getOrDefault<string?>("creationProtocol");
            set => 
                innerDmElement.set("creationProtocol", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.ViewModes.ViewMode",
        TypeKind = TypeKind.WrappedClass)]
    public class ViewMode_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @id
        {
            get =>
                innerDmElement.getOrDefault<string?>("id");
            set => 
                innerDmElement.set("id", value);
        }

        public string? @defaultExtentType
        {
            get =>
                innerDmElement.getOrDefault<string?>("defaultExtentType");
            set => 
                innerDmElement.set("defaultExtentType", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Forms.DropDownByCollection",
        TypeKind = TypeKind.WrappedClass)]
    public class DropDownByCollection_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @defaultWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("defaultWorkspace");
            set => 
                innerDmElement.set("defaultWorkspace", value);
        }

        public string? @collection
        {
            get =>
                innerDmElement.getOrDefault<string?>("collection");
            set => 
                innerDmElement.set("collection", value);
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#26a9c433-ead8-414b-9a8e-bb5a1a8cca00",
        TypeKind = TypeKind.WrappedClass)]
    public class UriReferenceFieldData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // Not found
        public object? @defaultWorkspace
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultWorkspace");
            set => 
                innerDmElement.set("defaultWorkspace", value);
        }

        // Not found
        public object? @defaultExtent
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultExtent");
            set => 
                innerDmElement.set("defaultExtent", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#ba1403c9-20cd-487d-8147-3937889deeb0",
        TypeKind = TypeKind.WrappedClass)]
    public class NavigateToFieldsForTestAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                innerDmElement.getOrDefault<bool>("isDisabled");
            set => 
                innerDmElement.set("isDisabled", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#8bb3e235-beed-4eb7-a95e-b5cfa4417bd2",
        TypeKind = TypeKind.WrappedClass)]
    public class DropDownByQueryData_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper
        public DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper? @query
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("query");
                return foundElement == null ? null : new DatenMeister.Core.Models.DataViews.QueryStatement_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("query", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("query", value);
                }
            }
        }

        public bool @isAttached
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAttached");
            set => 
                innerDmElement.set("isAttached", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        public bool @isEnumeration
        {
            get =>
                innerDmElement.getOrDefault<bool>("isEnumeration");
            set => 
                innerDmElement.set("isEnumeration", value);
        }

        // Not found
        public object? @defaultValue
        {
            get =>
                innerDmElement.getOrDefault<object?>("defaultValue");
            set => 
                innerDmElement.set("defaultValue", value);
        }

        public bool @isReadOnly
        {
            get =>
                innerDmElement.getOrDefault<bool>("isReadOnly");
            set => 
                innerDmElement.set("isReadOnly", value);
        }

    }

}

public class AttachedExtent
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.AttachedExtent.AttachedExtentConfiguration",
        TypeKind = TypeKind.WrappedClass)]
    public class AttachedExtentConfiguration_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @referencedWorkspace
        {
            get =>
                innerDmElement.getOrDefault<string?>("referencedWorkspace");
            set => 
                innerDmElement.set("referencedWorkspace", value);
        }

        public string? @referencedExtent
        {
            get =>
                innerDmElement.getOrDefault<string?>("referencedExtent");
            set => 
                innerDmElement.set("referencedExtent", value);
        }

        // Not found
        public object? @referenceType
        {
            get =>
                innerDmElement.getOrDefault<object?>("referenceType");
            set => 
                innerDmElement.set("referenceType", value);
        }

        public string? @referenceProperty
        {
            get =>
                innerDmElement.getOrDefault<string?>("referenceProperty");
            set => 
                innerDmElement.set("referenceProperty", value);
        }

    }

}

public class Management
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Extent",
        TypeKind = TypeKind.WrappedClass)]
    public class Extent_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @uri
        {
            get =>
                innerDmElement.getOrDefault<string?>("uri");
            set => 
                innerDmElement.set("uri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public int @count
        {
            get =>
                innerDmElement.getOrDefault<int>("count");
            set => 
                innerDmElement.set("count", value);
        }

        public int @totalCount
        {
            get =>
                innerDmElement.getOrDefault<int>("totalCount");
            set => 
                innerDmElement.set("totalCount", value);
        }

        public string? @type
        {
            get =>
                innerDmElement.getOrDefault<string?>("type");
            set => 
                innerDmElement.set("type", value);
        }

        public string? @extentType
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentType");
            set => 
                innerDmElement.set("extentType", value);
        }

        public bool @isModified
        {
            get =>
                innerDmElement.getOrDefault<bool>("isModified");
            set => 
                innerDmElement.set("isModified", value);
        }

        public string? @alternativeUris
        {
            get =>
                innerDmElement.getOrDefault<string?>("alternativeUris");
            set => 
                innerDmElement.set("alternativeUris", value);
        }

        public string? @autoEnumerateType
        {
            get =>
                innerDmElement.getOrDefault<string?>("autoEnumerateType");
            set => 
                innerDmElement.set("autoEnumerateType", value);
        }

        // Not found
        public object? @state
        {
            get =>
                innerDmElement.getOrDefault<object?>("state");
            set => 
                innerDmElement.set("state", value);
        }

        public string? @failMessage
        {
            get =>
                innerDmElement.getOrDefault<string?>("failMessage");
            set => 
                innerDmElement.set("failMessage", value);
        }

        // Not found
        public object? @properties
        {
            get =>
                innerDmElement.getOrDefault<object?>("properties");
            set => 
                innerDmElement.set("properties", value);
        }

        // Not found
        public object? @loadingConfiguration
        {
            get =>
                innerDmElement.getOrDefault<object?>("loadingConfiguration");
            set => 
                innerDmElement.set("loadingConfiguration", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.Workspace",
        TypeKind = TypeKind.WrappedClass)]
    public class Workspace_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @id
        {
            get =>
                innerDmElement.getOrDefault<string?>("id");
            set => 
                innerDmElement.set("id", value);
        }

        public string? @annotation
        {
            get =>
                innerDmElement.getOrDefault<string?>("annotation");
            set => 
                innerDmElement.set("annotation", value);
        }

        // DatenMeister.Core.Models.Management.Extent_Wrapper
        public DatenMeister.Core.Models.Management.Extent_Wrapper? @extents
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("extents");
                return foundElement == null ? null : new DatenMeister.Core.Models.Management.Extent_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("extents", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("extents", value);
                }
            }
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.ManagementProvider.FormViewModels.CreateNewWorkspaceModel",
        TypeKind = TypeKind.WrappedClass)]
    public class CreateNewWorkspaceModel_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @id
        {
            get =>
                innerDmElement.getOrDefault<string?>("id");
            set => 
                innerDmElement.set("id", value);
        }

        public string? @annotation
        {
            get =>
                innerDmElement.getOrDefault<string?>("annotation");
            set => 
                innerDmElement.set("annotation", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentTypeSetting",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentTypeSetting_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        // Not found
        public object? @rootElementMetaClasses
        {
            get =>
                innerDmElement.getOrDefault<object?>("rootElementMetaClasses");
            set => 
                innerDmElement.set("rootElementMetaClasses", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentProperties",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentProperties_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @uri
        {
            get =>
                innerDmElement.getOrDefault<string?>("uri");
            set => 
                innerDmElement.set("uri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public int @count
        {
            get =>
                innerDmElement.getOrDefault<int>("count");
            set => 
                innerDmElement.set("count", value);
        }

        public int @totalCount
        {
            get =>
                innerDmElement.getOrDefault<int>("totalCount");
            set => 
                innerDmElement.set("totalCount", value);
        }

        public string? @type
        {
            get =>
                innerDmElement.getOrDefault<string?>("type");
            set => 
                innerDmElement.set("type", value);
        }

        public string? @extentType
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentType");
            set => 
                innerDmElement.set("extentType", value);
        }

        public bool @isModified
        {
            get =>
                innerDmElement.getOrDefault<bool>("isModified");
            set => 
                innerDmElement.set("isModified", value);
        }

        public string? @alternativeUris
        {
            get =>
                innerDmElement.getOrDefault<string?>("alternativeUris");
            set => 
                innerDmElement.set("alternativeUris", value);
        }

        public string? @autoEnumerateType
        {
            get =>
                innerDmElement.getOrDefault<string?>("autoEnumerateType");
            set => 
                innerDmElement.set("autoEnumerateType", value);
        }

        // Not found
        public object? @state
        {
            get =>
                innerDmElement.getOrDefault<object?>("state");
            set => 
                innerDmElement.set("state", value);
        }

        public string? @failMessage
        {
            get =>
                innerDmElement.getOrDefault<string?>("failMessage");
            set => 
                innerDmElement.set("failMessage", value);
        }

        // Not found
        public object? @properties
        {
            get =>
                innerDmElement.getOrDefault<object?>("properties");
            set => 
                innerDmElement.set("properties", value);
        }

        // Not found
        public object? @loadingConfiguration
        {
            get =>
                innerDmElement.getOrDefault<object?>("loadingConfiguration");
            set => 
                innerDmElement.set("loadingConfiguration", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentPropertyDefinition",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentPropertyDefinition_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @title
        {
            get =>
                innerDmElement.getOrDefault<string?>("title");
            set => 
                innerDmElement.set("title", value);
        }

        // Not found
        public object? @metaClass
        {
            get =>
                innerDmElement.getOrDefault<object?>("metaClass");
            set => 
                innerDmElement.set("metaClass", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.Runtime.ExtentSettings",
        TypeKind = TypeKind.WrappedClass)]
    public class ExtentSettings_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        // DatenMeister.Core.Models.Management.ExtentTypeSetting_Wrapper
        public DatenMeister.Core.Models.Management.ExtentTypeSetting_Wrapper? @extentTypeSettings
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("extentTypeSettings");
                return foundElement == null ? null : new DatenMeister.Core.Models.Management.ExtentTypeSetting_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("extentTypeSettings", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("extentTypeSettings", value);
                }
            }
        }

        // DatenMeister.Core.Models.Management.ExtentPropertyDefinition_Wrapper
        public DatenMeister.Core.Models.Management.ExtentPropertyDefinition_Wrapper? @propertyDefinitions
        {
            get
            {
                var foundElement = innerDmElement.getOrDefault<IElement>("propertyDefinitions");
                return foundElement == null ? null : new DatenMeister.Core.Models.Management.ExtentPropertyDefinition_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    innerDmElement.set("propertyDefinitions", wrappedElement.GetWrappedElement());
                }
                else
                {
                    innerDmElement.set("propertyDefinitions", value);
                }
            }
        }

    }

}

public class FastViewFilters
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyComparisonFilter",
        TypeKind = TypeKind.WrappedClass)]
    public class PropertyComparisonFilter_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @Property
        {
            get =>
                innerDmElement.getOrDefault<string?>("Property");
            set => 
                innerDmElement.set("Property", value);
        }

        // Not found
        public object? @ComparisonType
        {
            get =>
                innerDmElement.getOrDefault<object?>("ComparisonType");
            set => 
                innerDmElement.set("ComparisonType", value);
        }

        public string? @Value
        {
            get =>
                innerDmElement.getOrDefault<string?>("Value");
            set => 
                innerDmElement.set("Value", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.Models.FastViewFilter.PropertyContainsFilter",
        TypeKind = TypeKind.WrappedClass)]
    public class PropertyContainsFilter_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @Property
        {
            get =>
                innerDmElement.getOrDefault<string?>("Property");
            set => 
                innerDmElement.set("Property", value);
        }

        public string? @Value
        {
            get =>
                innerDmElement.getOrDefault<string?>("Value");
            set => 
                innerDmElement.set("Value", value);
        }

    }

}

public class DynamicRuntimeProvider
{
    [TypeUri(Uri = "dm:///_internal/types/internal#DynamicRuntimeProvider.DynamicRuntimeLoaderConfig",
        TypeKind = TypeKind.WrappedClass)]
    public class DynamicRuntimeLoaderConfig_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @runtimeClass
        {
            get =>
                innerDmElement.getOrDefault<string?>("runtimeClass");
            set => 
                innerDmElement.set("runtimeClass", value);
        }

        // Not found
        public object? @configuration
        {
            get =>
                innerDmElement.getOrDefault<object?>("configuration");
            set => 
                innerDmElement.set("configuration", value);
        }

        public string? @name
        {
            get =>
                innerDmElement.getOrDefault<string?>("name");
            set => 
                innerDmElement.set("name", value);
        }

        public string? @extentUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("extentUri");
            set => 
                innerDmElement.set("extentUri", value);
        }

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public bool @dropExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("dropExisting");
            set => 
                innerDmElement.set("dropExisting", value);
        }

    }

    public class Examples
    {
        [TypeUri(Uri = "dm:///_internal/types/internal#f264ab67-ab6a-4462-8088-d3d6c9e2763a",
            TypeKind = TypeKind.WrappedClass)]
        public class NumberProviderSettings_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public int @start
            {
                get =>
                    innerDmElement.getOrDefault<int>("start");
                set => 
                    innerDmElement.set("start", value);
            }

            public int @end
            {
                get =>
                    innerDmElement.getOrDefault<int>("end");
                set => 
                    innerDmElement.set("end", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/types/internal#DatenMeister.DynamicRuntimeProviders.Examples.NumberRepresentation",
            TypeKind = TypeKind.WrappedClass)]
        public class NumberRepresentation_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            public string? @binary
            {
                get =>
                    innerDmElement.getOrDefault<string?>("binary");
                set => 
                    innerDmElement.set("binary", value);
            }

            public string? @octal
            {
                get =>
                    innerDmElement.getOrDefault<string?>("octal");
                set => 
                    innerDmElement.set("octal", value);
            }

            public string? @decimal
            {
                get =>
                    innerDmElement.getOrDefault<string?>("decimal");
                set => 
                    innerDmElement.set("decimal", value);
            }

            public string? @hexadecimal
            {
                get =>
                    innerDmElement.getOrDefault<string?>("hexadecimal");
                set => 
                    innerDmElement.set("hexadecimal", value);
            }

        }

    }

}

public class Verifier
{
    [TypeUri(Uri = "dm:///_internal/types/internal#d19d742f-9bba-4bef-b310-05ef96153768",
        TypeKind = TypeKind.WrappedClass)]
    public class VerifyEntry_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @workspaceId
        {
            get =>
                innerDmElement.getOrDefault<string?>("workspaceId");
            set => 
                innerDmElement.set("workspaceId", value);
        }

        public string? @itemUri
        {
            get =>
                innerDmElement.getOrDefault<string?>("itemUri");
            set => 
                innerDmElement.set("itemUri", value);
        }

        public string? @category
        {
            get =>
                innerDmElement.getOrDefault<string?>("category");
            set => 
                innerDmElement.set("category", value);
        }

        public string? @message
        {
            get =>
                innerDmElement.getOrDefault<string?>("message");
            set => 
                innerDmElement.set("message", value);
        }

    }

}

