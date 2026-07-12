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

    }

    [TypeUri(Uri = "dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c",
        TypeKind = TypeKind.WrappedClass)]
    public class SwimlaneViewdefinition_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public SwimlaneViewdefinition_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public SwimlaneViewdefinition_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c");

        public static SwimlaneViewdefinition_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

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

    }

}

