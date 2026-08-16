using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.DataView.Forms.Model;

public class Root
{
    [TypeUri(Uri = "dm:///types.forms.dataview.datenmeister/#06f21d5d-3813-4810-a63c-16f12ef5d175",
        TypeKind = TypeKind.WrappedClass)]
    public class ViewNodeRenderAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ViewNodeRenderAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ViewNodeRenderAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///types.forms.dataview.datenmeister/#06f21d5d-3813-4810-a63c-16f12ef5d175");

        public static ViewNodeRenderAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

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

