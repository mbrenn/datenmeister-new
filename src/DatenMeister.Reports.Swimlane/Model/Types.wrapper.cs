using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Reports.Swimlane.Model;

public class Root
{
    [TypeUri(Uri = "dm:///intern.types.swimlane.datenmeister/#e92b2227-9b93-4033-ac2e-772a2230a869",
        TypeKind = TypeKind.WrappedClass)]
    public class SwimlaneConfiguration_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SwimlaneConfiguration_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SwimlaneConfiguration_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.swimlane.datenmeister/#e92b2227-9b93-4033-ac2e-772a2230a869");

        public static SwimlaneConfiguration_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @verticalSwimlaneProperty
        {
            get =>
                _wrappedElement.getOrDefault<string?>("verticalSwimlaneProperty");
            set => 
                _wrappedElement.set("verticalSwimlaneProperty", value);
        }

        public string? @horizontalSwimlaneProperty
        {
            get =>
                _wrappedElement.getOrDefault<string?>("horizontalSwimlaneProperty");
            set => 
                _wrappedElement.set("horizontalSwimlaneProperty", value);
        }

        public string? @cellContent
        {
            get =>
                _wrappedElement.getOrDefault<string?>("cellContent");
            set => 
                _wrappedElement.set("cellContent", value);
        }

        public bool @linkContent
        {
            get =>
                _wrappedElement.getOrDefault<bool>("linkContent");
            set => 
                _wrappedElement.set("linkContent", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c",
        TypeKind = TypeKind.WrappedClass)]
    public class SwimlaneViewDefinition_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SwimlaneViewDefinition_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SwimlaneViewDefinition_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c");

        public static SwimlaneViewDefinition_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @swimlaneConfiguration
        {
            get =>
                _wrappedElement.getOrDefault<object?>("swimlaneConfiguration");
            set => 
                _wrappedElement.set("swimlaneConfiguration", value);
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

    [TypeUri(Uri = "dm:///intern.types.swimlane.datenmeister/#142cb4a2-9a07-4e63-a212-32b0a1f0a289",
        TypeKind = TypeKind.WrappedClass)]
    public class SwimlaneForm_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SwimlaneForm_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SwimlaneForm_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.swimlane.datenmeister/#142cb4a2-9a07-4e63-a212-32b0a1f0a289");

        public static SwimlaneForm_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @swimlaneViewDefinition
        {
            get =>
                _wrappedElement.getOrDefault<object?>("swimlaneViewDefinition");
            set => 
                _wrappedElement.set("swimlaneViewDefinition", value);
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

}

