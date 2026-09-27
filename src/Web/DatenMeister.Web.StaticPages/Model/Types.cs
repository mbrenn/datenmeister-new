using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Web.StaticPages.Model;

public class _Root
{
    [TypeUri(Uri = "dm:///types.forms.staticwebpages.datenmeister/#f63bbf10-19fe-455a-a952-74be1e3d05f6",
        TypeKind = TypeKind.ClassTree)]
    public class _CreateStaticWebPageAction
    {
        public static readonly string @htmlFilename = "htmlFilename";
        public IElement? @_htmlFilename = null;

    }

    public _CreateStaticWebPageAction @CreateStaticWebPageAction = new ();
    public MofObjectShadow @__CreateStaticWebPageAction = new ("dm:///types.forms.staticwebpages.datenmeister/#f63bbf10-19fe-455a-a952-74be1e3d05f6");

    public static readonly _Root TheOne = new ();

}

