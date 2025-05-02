#nullable enable
using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Zip.Model
{
    public class _Root
    {
        public class _ZipFileExtractAction
        {
            public static string @sourcePath = "sourcePath";
            public IElement? @_sourcePath = null;

            public static string @targetPath = "targetPath";
            public IElement? @_targetPath = null;

            public static string @overwriteIfExisting = "overwriteIfExisting";
            public IElement? @_overwriteIfExisting = null;

            public static string @overwriteIfNewer = "overwriteIfNewer";
            public IElement? @_overwriteIfNewer = null;

        }

        public _ZipFileExtractAction @ZipFileExtractAction = new _ZipFileExtractAction();
        public MofObjectShadow @__ZipFileExtractAction = new MofObjectShadow("dm:///_internal/types/internal#bb766314-6b49-48a6-b80b-08d61c549ff6");

        public static readonly _Root TheOne = new _Root();

    }

}
