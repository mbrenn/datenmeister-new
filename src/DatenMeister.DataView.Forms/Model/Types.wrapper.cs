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

    [TypeUri(Uri = "dm:///types.forms.dataview.datenmeister/#0e2986e0-2982-426e-8024-71a100f1e7d0",
        TypeKind = TypeKind.WrappedClass)]
    public class ViewDataTable_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ViewDataTable_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ViewDataTable_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///types.forms.dataview.datenmeister/#0e2986e0-2982-426e-8024-71a100f1e7d0");

        public static ViewDataTable_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

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

        // DatenMeister.Core.Models.Forms.FormTypes.TableForm_Wrapper
        public DatenMeister.Core.Models.Forms.FormTypes.TableForm_Wrapper? @tableForm
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("tableForm");
                return foundElement == null ? null : new DatenMeister.Core.Models.Forms.FormTypes.TableForm_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("tableForm", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("tableForm", value);
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

}

