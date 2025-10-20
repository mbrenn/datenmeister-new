using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Zip.Model;

public class Root
{
    [TypeUri(Uri = "dm:///_internal/types/internal#bb766314-6b49-48a6-b80b-08d61c549ff6",
        TypeKind = TypeKind.WrappedClass)]
    public class ZipFileExtractAction_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public string? @sourcePath
        {
            get =>
                innerDmElement.getOrDefault<string?>("sourcePath");
            set => 
                innerDmElement.set("sourcePath", value);
        }

        public string? @targetPath
        {
            get =>
                innerDmElement.getOrDefault<string?>("targetPath");
            set => 
                innerDmElement.set("targetPath", value);
        }

        public bool @overwriteIfExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("overwriteIfExisting");
            set => 
                innerDmElement.set("overwriteIfExisting", value);
        }

        public bool @overwriteOnlyIfNewer
        {
            get =>
                innerDmElement.getOrDefault<bool>("overwriteOnlyIfNewer");
            set => 
                innerDmElement.set("overwriteOnlyIfNewer", value);
        }

    }

    [TypeUri(Uri = "dm:///_internal/types/internal#a03dae8c-4f44-4d08-9cf8-e98627d05e2f",
        TypeKind = TypeKind.WrappedClass)]
    public class ZipFileExtractActionResult_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        public bool @success
        {
            get =>
                innerDmElement.getOrDefault<bool>("success");
            set => 
                innerDmElement.set("success", value);
        }

        public bool @alreadyExisting
        {
            get =>
                innerDmElement.getOrDefault<bool>("alreadyExisting");
            set => 
                innerDmElement.set("alreadyExisting", value);
        }

        public bool @isAlreadyUpToDate
        {
            get =>
                innerDmElement.getOrDefault<bool>("isAlreadyUpToDate");
            set => 
                innerDmElement.set("isAlreadyUpToDate", value);
        }

    }

}

