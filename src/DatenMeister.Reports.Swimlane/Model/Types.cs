using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Reports.Swimlane.Model;

public class _Root
{
    [TypeUri(Uri = "dm:///intern.types.swimlane.datenmeister/#e92b2227-9b93-4033-ac2e-772a2230a869",
        TypeKind = TypeKind.ClassTree)]
    public class _SwimlaneConfiguration
    {
        public static readonly string @verticalSwimlaneProperty = "verticalSwimlaneProperty";
        public IElement? @_verticalSwimlaneProperty = null;

        public static readonly string @horizontalSwimlaneProperty = "horizontalSwimlaneProperty";
        public IElement? @_horizontalSwimlaneProperty = null;

        public static readonly string @cellContent = "cellContent";
        public IElement? @_cellContent = null;

        public static readonly string @linkContent = "linkContent";
        public IElement? @_linkContent = null;

    }

    public _SwimlaneConfiguration @SwimlaneConfiguration = new ();
    public MofObjectShadow @__SwimlaneConfiguration = new ("dm:///intern.types.swimlane.datenmeister/#e92b2227-9b93-4033-ac2e-772a2230a869");

    [TypeUri(Uri = "dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c",
        TypeKind = TypeKind.ClassTree)]
    public class _SwimlaneViewdefinition
    {
        public static readonly string @swimlaneConfiguration = "swimlaneConfiguration";
        public IElement? @_swimlaneConfiguration = null;

        public static readonly string @viewNode = "viewNode";
        public IElement? @_viewNode = null;

    }

    public _SwimlaneViewdefinition @SwimlaneViewdefinition = new ();
    public MofObjectShadow @__SwimlaneViewdefinition = new ("dm:///intern.types.swimlane.datenmeister/#1c8b02e8-1988-4c3b-9d74-8739c400d56c");

    public static readonly _Root TheOne = new ();

}

