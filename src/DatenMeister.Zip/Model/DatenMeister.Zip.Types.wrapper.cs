using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Zip.Model;

public class Root
{
    [TypeUri(Uri = "dm:///_internal/types/internal#bb766314-6b49-48a6-b80b-08d61c549ff6",
        TypeKind = TypeKind.WrappedClass)]
    public class ZipFileExtractAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ZipFileExtractAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ZipFileExtractAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#bb766314-6b49-48a6-b80b-08d61c549ff6");

        public static ZipFileExtractAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

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

        public bool @overwriteIfExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("overwriteIfExisting");
            set => 
                _wrappedElement.set("overwriteIfExisting", value);
        }

        public bool @overwriteOnlyIfNewer
        {
            get =>
                _wrappedElement.getOrDefault<bool>("overwriteOnlyIfNewer");
            set => 
                _wrappedElement.set("overwriteOnlyIfNewer", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#a03dae8c-4f44-4d08-9cf8-e98627d05e2f",
        TypeKind = TypeKind.WrappedClass)]
    public class ZipFileExtractActionResult_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public ZipFileExtractActionResult_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public ZipFileExtractActionResult_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/types/internal#a03dae8c-4f44-4d08-9cf8-e98627d05e2f");

        public static ZipFileExtractActionResult_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public bool @success
        {
            get =>
                _wrappedElement.getOrDefault<bool>("success");
            set => 
                _wrappedElement.set("success", value);
        }

        public bool @alreadyExisting
        {
            get =>
                _wrappedElement.getOrDefault<bool>("alreadyExisting");
            set => 
                _wrappedElement.set("alreadyExisting", value);
        }

        public bool @isAlreadyUpToDate
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isAlreadyUpToDate");
            set => 
                _wrappedElement.set("isAlreadyUpToDate", value);
        }

    }

}

