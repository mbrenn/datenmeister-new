using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Zip.Model;

public class _Root
{
    [TypeUri(Uri = "dm:///_internal/types/internal#bb766314-6b49-48a6-b80b-08d61c549ff6",
        TypeKind = TypeKind.ClassTree)]
    public class _ZipFileExtractAction
    {
        public static readonly string @sourcePath = "sourcePath";
        public IElement? @_sourcePath = null;

        public static readonly string @targetPath = "targetPath";
        public IElement? @_targetPath = null;

        public static readonly string @overwriteIfExisting = "overwriteIfExisting";
        public IElement? @_overwriteIfExisting = null;

        public static readonly string @overwriteOnlyIfNewer = "overwriteOnlyIfNewer";
        public IElement? @_overwriteOnlyIfNewer = null;

    }

    public _ZipFileExtractAction @ZipFileExtractAction = new _ZipFileExtractAction();
    public MofObjectShadow @__ZipFileExtractAction = new MofObjectShadow("dm:///_internal/types/internal#bb766314-6b49-48a6-b80b-08d61c549ff6");

    [TypeUri(Uri = "dm:///_internal/types/internal#a03dae8c-4f44-4d08-9cf8-e98627d05e2f",
        TypeKind = TypeKind.ClassTree)]
    public class _ZipFileExtractActionResult
    {
        public static readonly string @success = "success";
        public IElement? @_success = null;

        public static readonly string @alreadyExisting = "alreadyExisting";
        public IElement? @_alreadyExisting = null;

        public static readonly string @isAlreadyUpToDate = "isAlreadyUpToDate";
        public IElement? @_isAlreadyUpToDate = null;

    }

    public _ZipFileExtractActionResult @ZipFileExtractActionResult = new _ZipFileExtractActionResult();
    public MofObjectShadow @__ZipFileExtractActionResult = new MofObjectShadow("dm:///_internal/types/internal#a03dae8c-4f44-4d08-9cf8-e98627d05e2f");

    public static readonly _Root TheOne = new _Root();

}

