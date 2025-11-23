using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Extent.Forms.Model;

public class Root
{
    [TypeUri(Uri = "dm:///_internal/types/internal#b5e9f945-6c33-4b26-837b-38a5ad2f65fc",
        TypeKind = TypeKind.WrappedClass)]
    public class MassImportDefinitionAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public MassImportDefinitionAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public MassImportDefinitionAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#b5e9f945-6c33-4b26-837b-38a5ad2f65fc");

        public static MassImportDefinitionAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @item
        {
            get =>
                _wrappedElement.getOrDefault<object?>("item");
            set => 
                _wrappedElement.set("item", value);
        }

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

