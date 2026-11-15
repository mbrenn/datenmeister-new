using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Actions.Model;

public class Actions
{
    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.Action",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#Actions.Action");

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

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.ActionSet",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#Actions.ActionSet");

        public static ActionSet_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));
    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#899324b1-85dc-40a1-ba95-dec50509040d",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#899324b1-85dc-40a1-ba95-dec50509040d");

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

        public string? @reason
        {
            get =>
                _wrappedElement.getOrDefault<string?>("reason");
            set => 
                _wrappedElement.set("reason", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.LoggingWriterAction",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#Actions.LoggingWriterAction");

        public static LoggingWriterAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @message
        {
            get =>
                _wrappedElement.getOrDefault<string?>("message");
            set => 
                _wrappedElement.set("message", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#241b550d-835a-41ea-a32a-bea5d388c6ee",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#241b550d-835a-41ea-a32a-bea5d388c6ee");

        public static LoadExtentAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // DatenMeister.Core.Models.ExtentLoaderConfigs.ExtentLoaderConfig_Wrapper
        public DatenMeister.Core.Models.ExtentLoaderConfigs.ExtentLoaderConfig_Wrapper? @configuration
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("configuration");
                return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExtentLoaderConfig_Wrapper(foundElement);
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

        public bool @dropExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("dropExisting");
            set => 
                _wrappedElement.set("dropExisting", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#c870f6e8-2b70-415c-afaf-b78776b42a09",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#c870f6e8-2b70-415c-afaf-b78776b42a09");

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

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#1be0dfb0-be9c-4cb0-b2e5-aaab17118bfe");

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

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#db6cc8eb-011c-43e5-b966-cc0e3a1855e8",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#db6cc8eb-011c-43e5-b966-cc0e3a1855e8");

        public static DropWorkspaceAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspaceId
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspaceId");
            set => 
                _wrappedElement.set("workspaceId", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#8b576580-0f75-4159-ad16-afb7c2268aed",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#8b576580-0f75-4159-ad16-afb7c2268aed");

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

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#3c3595a4-026e-4c07-83ec-8a90607b8863",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#3c3595a4-026e-4c07-83ec-8a90607b8863");

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

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#b70b736b-c9b0-4986-8d92-240fcabc95ae",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#b70b736b-c9b0-4986-8d92-240fcabc95ae");

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

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.ItemTransformationActionHandler",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#Actions.ItemTransformationActionHandler");

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

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Actions.EchoAction",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#DatenMeister.Actions.EchoAction");

        public static EchoAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @shallSuccess
        {
            get =>
                _wrappedElement.getOrDefault<string?>("shallSuccess");
            set => 
                _wrappedElement.set("shallSuccess", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#04878741-802e-4b7f-8003-21d25f38ac74",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#04878741-802e-4b7f-8003-21d25f38ac74");

        public static DocumentOpenAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

    }

    public class Reports
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.SimpleReportAction",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#Actions.SimpleReportAction");

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

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.AdocReportAction",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#Actions.AdocReportAction");

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

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#Actions.HtmlReportAction",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#Actions.HtmlReportAction");

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

        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.MoveOrCopyAction",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.MoveOrCopyAction");

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

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#bc4952bf-a3f5-4516-be26-5b773e38bd54",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#bc4952bf-a3f5-4516-be26-5b773e38bd54");

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

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#43b0764e-b70f-42bb-b37d-ae8586ec45f1",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#43b0764e-b70f-42bb-b37d-ae8586ec45f1");

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

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#0f4b40ec-2f90-4184-80d8-2aa3a8eaef5d");

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

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#b631bb00-ab11-4a8a-a148-e28abc398503",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#b631bb00-ab11-4a8a-a148-e28abc398503");

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

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#3223e13a-bbb7-4785-8b81-7275be23b0a1",
        TypeKind = TypeKind.WrappedClass)]
    public class TargetReferenceResult_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public TargetReferenceResult_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public TargetReferenceResult_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#3223e13a-bbb7-4785-8b81-7275be23b0a1");

        public static TargetReferenceResult_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

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
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#90f61e4e-a5ea-42eb-9caa-912d010fbccd",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#90f61e4e-a5ea-42eb-9caa-912d010fbccd");

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

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#2863f928-fe69-4d35-8c67-f4f3533b7ae5",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#2863f928-fe69-4d35-8c67-f4f3533b7ae5");

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

    }

    public class ClientActions
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#e07ca80e-2540-4f91-8214-60dbd464e998",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#e07ca80e-2540-4f91-8214-60dbd464e998");

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

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#0ee17f2a-5407-4d38-b1b4-34ead2186971",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#0ee17f2a-5407-4d38-b1b4-34ead2186971");

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

            public string? @name
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("name");
                set => 
                    _wrappedElement.set("name", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#3251783f-2683-4c24-bad5-828930028462",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#3251783f-2683-4c24-bad5-828930028462");

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

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#5f69675e-df58-4ad7-84bf-359cdfba5db4",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#5f69675e-df58-4ad7-84bf-359cdfba5db4");

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

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#f1b5a13d-14fa-4ae8-8850-0feddf7ba0e5",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigateToUrlClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NavigateToUrlClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NavigateToUrlClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#f1b5a13d-14fa-4ae8-8850-0feddf7ba0e5");

            public static NavigateToUrlClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @url
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("url");
                set => 
                    _wrappedElement.set("url", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#746ecf7c-63d6-4fdf-95ae-837a13a032ed",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigateOpenWindow_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NavigateOpenWindow_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NavigateOpenWindow_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#746ecf7c-63d6-4fdf-95ae-837a13a032ed");

            public static NavigateOpenWindow_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // Not found
            public object? @url
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("url");
                set => 
                    _wrappedElement.set("url", value);
            }

            public bool @isAbsoluteUrl
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isAbsoluteUrl");
                set => 
                    _wrappedElement.set("isAbsoluteUrl", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#77ff9c01-0651-4657-823a-ce41bbe0e5e5",
            TypeKind = TypeKind.WrappedClass)]
        public class NavigateOpenActionInWindow_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public NavigateOpenActionInWindow_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public NavigateOpenActionInWindow_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#77ff9c01-0651-4657-823a-ce41bbe0e5e5");

            public static NavigateOpenActionInWindow_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @context
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("context");
                set => 
                    _wrappedElement.set("context", value);
            }

            public string? @actionUrl
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("actionUrl");
                set => 
                    _wrappedElement.set("actionUrl", value);
            }

            public string? @workspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspaceId");
                set => 
                    _wrappedElement.set("workspaceId", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#ffa8c9f8-9454-423d-882d-a44b98eaf4ea",
            TypeKind = TypeKind.WrappedClass)]
        public class RenderFormClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RenderFormClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RenderFormClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#ffa8c9f8-9454-423d-882d-a44b98eaf4ea");

            public static RenderFormClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @dataWorkspaceId
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("dataWorkspaceId");
                set => 
                    _wrappedElement.set("dataWorkspaceId", value);
            }

            public string? @dataUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("dataUri");
                set => 
                    _wrappedElement.set("dataUri", value);
            }

            public bool @formAutoGenerate
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("formAutoGenerate");
                set => 
                    _wrappedElement.set("formAutoGenerate", value);
            }

            // Not found
            public object? @formType
            {
                get =>
                    _wrappedElement.getOrDefault<object?>("formType");
                set => 
                    _wrappedElement.set("formType", value);
            }

            // DatenMeister.Core.Models.Forms.FormTypes.Form_Wrapper
            public DatenMeister.Core.Models.Forms.FormTypes.Form_Wrapper? @form
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("form");
                    return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FormTypes.Form_Wrapper(foundElement);
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

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#30f7e380-73e1-4ade-b797-1cc948b63c4f",
            TypeKind = TypeKind.WrappedClass)]
        public class RenderHtmlClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public RenderHtmlClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public RenderHtmlClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#30f7e380-73e1-4ade-b797-1cc948b63c4f");

            public static RenderHtmlClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @html
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("html");
                set => 
                    _wrappedElement.set("html", value);
            }

            public string? @text
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("text");
                set => 
                    _wrappedElement.set("text", value);
            }

            public string? @renderCompletePage
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("renderCompletePage");
                set => 
                    _wrappedElement.set("renderCompletePage", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#7059ce1e-f299-45c5-b536-a7cb22c7225e",
            TypeKind = TypeKind.WrappedClass)]
        public class DownloadHtmlClientAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public DownloadHtmlClientAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public DownloadHtmlClientAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#7059ce1e-f299-45c5-b536-a7cb22c7225e");

            public static DownloadHtmlClientAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @html
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("html");
                set => 
                    _wrappedElement.set("html", value);
            }

            public string? @filename
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("filename");
                set => 
                    _wrappedElement.set("filename", value);
            }

        }

    }

    public class Forms
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.CreateFormByMetaclass",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#DatenMeister.Models.Actions.CreateFormByMetaclass");

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

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#d9838e98-fd94-4dc1-883a-4362a2d732d3",
            TypeKind = TypeKind.WrappedClass)]
        public class GetAttributeOfItem_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public GetAttributeOfItem_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public GetAttributeOfItem_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#d9838e98-fd94-4dc1-883a-4362a2d732d3");

            public static GetAttributeOfItem_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @itemUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("itemUri");
                set => 
                    _wrappedElement.set("itemUri", value);
            }

            public string? @workspace
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("workspace");
                set => 
                    _wrappedElement.set("workspace", value);
            }

            public string? @propertyName
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("propertyName");
                set => 
                    _wrappedElement.set("propertyName", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#e144d177-70e2-4418-991a-8262523c62b7",
            TypeKind = TypeKind.WrappedClass)]
        public class GetAttributeOfItemResult_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public GetAttributeOfItemResult_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public GetAttributeOfItemResult_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#e144d177-70e2-4418-991a-8262523c62b7");

            public static GetAttributeOfItemResult_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public bool @isComposite
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isComposite");
                set => 
                    _wrappedElement.set("isComposite", value);
            }

            public string? @metaClassUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("metaClassUri");
                set => 
                    _wrappedElement.set("metaClassUri", value);
            }

            public bool @isMultiple
            {
                get =>
                    _wrappedElement.getOrDefault<bool>("isMultiple");
                set => 
                    _wrappedElement.set("isMultiple", value);
            }

        }

    }

    public class OSIntegration
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#6f2ea2cd-6218-483c-90a3-4db255e84e82",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#6f2ea2cd-6218-483c-90a3-4db255e84e82");

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

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#4090ce13-6718-466c-96df-52d51024aadb",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#4090ce13-6718-466c-96df-52d51024aadb");

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

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a",
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

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#82f46dd7-b61b-4bc1-b25c-d5d3d244c35a");

            public static ConsoleWriteAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @text
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("text");
                set => 
                    _wrappedElement.set("text", value);
            }

        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#9d43decb-aa2f-4461-b680-3ec595b518d1",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#9d43decb-aa2f-4461-b680-3ec595b518d1");

        public static RefreshTypeIndexAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @waitForRefresh
        {
            get =>
                _wrappedElement.getOrDefault<bool>("waitForRefresh");
            set => 
                _wrappedElement.set("waitForRefresh", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#c7dcb24c-e53c-46f9-9e8a-3704095193a8",
        TypeKind = TypeKind.WrappedClass)]
    public class StoreElementAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public StoreElementAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public StoreElementAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#c7dcb24c-e53c-46f9-9e8a-3704095193a8");

        public static StoreElementAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @workspace
        {
            get =>
                _wrappedElement.getOrDefault<string?>("workspace");
            set => 
                _wrappedElement.set("workspace", value);
        }

        public string? @url
        {
            get =>
                _wrappedElement.getOrDefault<string?>("url");
            set => 
                _wrappedElement.set("url", value);
        }

        // Not found
        public object? @element
        {
            get =>
                _wrappedElement.getOrDefault<object?>("element");
            set => 
                _wrappedElement.set("element", value);
        }

    }

    public class Data
    {
        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#fb1fcea9-c1d5-489d-9bd1-f64c6b71a171",
            TypeKind = TypeKind.WrappedClass)]
        public class FreezeViewResultAction_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public FreezeViewResultAction_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public FreezeViewResultAction_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#fb1fcea9-c1d5-489d-9bd1-f64c6b71a171");

            public static FreezeViewResultAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

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

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#1286c17c-3286-4e8c-93fc-1a4c3e6df48a",
            TypeKind = TypeKind.WrappedClass)]
        public class FreezeViewResultInMemory_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public FreezeViewResultInMemory_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public FreezeViewResultInMemory_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#1286c17c-3286-4e8c-93fc-1a4c3e6df48a");

            public static FreezeViewResultInMemory_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @extentUri
            {
                get =>
                    _wrappedElement.getOrDefault<string?>("extentUri");
                set => 
                    _wrappedElement.set("extentUri", value);
            }

        }

        [TypeUri(Uri = "dm:///intern.types.actions.datenmeister/#45cd9f30-fdfa-4f1b-a87c-24c088c075be",
            TypeKind = TypeKind.WrappedClass)]
        public class FreezeViewResultInExtent_Wrapper : IElementWrapper
        {
            private readonly IElement _wrappedElement;

            public FreezeViewResultInExtent_Wrapper(IElement innerDmElement)
            {
                _wrappedElement = innerDmElement;
            }

            public FreezeViewResultInExtent_Wrapper(IFactory factory)
            {
                _wrappedElement = factory.create(_metaClass);
            }

            public IElement GetWrappedElement() => _wrappedElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.actions.datenmeister/#45cd9f30-fdfa-4f1b-a87c-24c088c075be");

            public static FreezeViewResultInExtent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.ExtentLoaderConfigs.ExtentLoaderConfig_Wrapper
            public DatenMeister.Core.Models.ExtentLoaderConfigs.ExtentLoaderConfig_Wrapper? @extentLoaderConfig
            {
                get
                {
                    var foundElement = _wrappedElement.getOrDefault<IElement?>("extentLoaderConfig");
                    return foundElement == null ? null : new DatenMeister.Core.Models.ExtentLoaderConfigs.ExtentLoaderConfig_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        _wrappedElement.set("extentLoaderConfig", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        _wrappedElement.set("extentLoaderConfig", value);
                    }
                }
            }

        }

    }

}

