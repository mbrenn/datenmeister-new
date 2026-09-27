using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Web.StaticPages.Model;

public class Root
{
    [TypeUri(Uri = "dm:///types.forms.staticwebpages.datenmeister/#f63bbf10-19fe-455a-a952-74be1e3d05f6",
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

        private static readonly MofObjectShadow _metaClass = new ("dm:///types.forms.staticwebpages.datenmeister/#f63bbf10-19fe-455a-a952-74be1e3d05f6");

        public static CreateStaticWebPageAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @htmlFilename
        {
            get =>
                _wrappedElement.getOrDefault<string?>("htmlFilename");
            set => 
                _wrappedElement.set("htmlFilename", value);
        }

    }

}

