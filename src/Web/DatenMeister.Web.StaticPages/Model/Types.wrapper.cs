using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Web.StaticPages.Model;

public class HtmlElements
{
}

public class Root
{
    [TypeUri(Uri = "dm:///intern.types.staticwebpages.datenmeister/#f63bbf10-19fe-455a-a952-74be1e3d05f6",
        TypeKind = TypeKind.WrappedClass)]
    public class CreateStaticWebPageAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public CreateStaticWebPageAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public CreateStaticWebPageAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.staticwebpages.datenmeister/#f63bbf10-19fe-455a-a952-74be1e3d05f6");

        public static CreateStaticWebPageAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @htmlFilename
        {
            get =>
                _wrappedElement.getOrDefault<string?>("htmlFilename");
            set => 
                _wrappedElement.set("htmlFilename", value);
        }

        // DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper
        public DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper? @htmlElements
        {
            get
            {
                var foundElement = _wrappedElement.getOrDefault<IElement?>("htmlElements");
                return foundElement == null ? null : new DatenMeister.Core.Models.Reports.Elements.ReportElement_Wrapper(foundElement);
            }
            set 
            {
                if(value is IElementWrapper wrappedElement)
                {
                    _wrappedElement.set("htmlElements", wrappedElement.GetWrappedElement());
                }
                else
                {
                    _wrappedElement.set("htmlElements", value);
                }
            }
        }

        public string? @pageTitle
        {
            get =>
                _wrappedElement.getOrDefault<string?>("pageTitle");
            set => 
                _wrappedElement.set("pageTitle", value);
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

