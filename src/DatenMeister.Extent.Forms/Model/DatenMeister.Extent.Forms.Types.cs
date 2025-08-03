using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Extent.Forms.Model;

public class _Root
{
    [TypeUri(Uri = "dm:///_internal/types/internal#b5e9f945-6c33-4b26-837b-38a5ad2f65fc",
        TypeKind = TypeKind.ClassTree)]
    public class _MassImportDefinitionAction
    {
        public static string @item = "item";
        public IElement? @_item = null;

        public static string @text = "text";
        public IElement? @_text = null;

        public static string @name = "name";
        public IElement? @_name = null;

        public static string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _MassImportDefinitionAction @MassImportDefinitionAction = new _MassImportDefinitionAction();
    public MofObjectShadow @__MassImportDefinitionAction = new MofObjectShadow("dm:///_internal/types/internal#b5e9f945-6c33-4b26-837b-38a5ad2f65fc");

    public static readonly _Root TheOne = new _Root();

}

