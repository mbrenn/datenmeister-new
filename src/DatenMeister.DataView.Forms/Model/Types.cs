using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.DataView.Forms.Model;

public class _Root
{
    [TypeUri(Uri = "dm:///types.forms.dataview.datenmeister/#06f21d5d-3813-4810-a63c-16f12ef5d175",
        TypeKind = TypeKind.ClassTree)]
    public class _ViewNodeRenderAction
    {
        public static readonly string @viewNode = "viewNode";
        public IElement? @_viewNode = null;

    }

    public _ViewNodeRenderAction @ViewNodeRenderAction = new ();
    public MofObjectShadow @__ViewNodeRenderAction = new ("dm:///types.forms.dataview.datenmeister/#06f21d5d-3813-4810-a63c-16f12ef5d175");

    public static readonly _Root TheOne = new ();

}

