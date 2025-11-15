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
    public class MassImportDefinitionAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#b5e9f945-6c33-4b26-837b-38a5ad2f65fc");

        public static MassImportDefinitionAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        // Not found
        public object? @item
        {
            get =>
                innerDmElement.getOrDefault<object?>("item");
            set => 
                innerDmElement.set("item", value);
        }

        public string? @text
        {
            get =>
                innerDmElement.getOrDefault<string?>("text");
            set => 
                innerDmElement.set("text", value);
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

