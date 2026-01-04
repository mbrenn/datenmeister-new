using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Domains.Model;

public class _Root
{
    [TypeUri(Uri = "dm:///intern.types.domains.datenmeister/#9b040443-4675-42b6-8707-0e42450a2102",
        TypeKind = TypeKind.ClassTree)]
    public class _DomainCreateFoundationAction
    {
        public static readonly string @filePath = "filePath";
        public IElement? @_filePath = null;

        public static readonly string @createDataExtent = "createDataExtent";
        public IElement? @_createDataExtent = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @isDisabled = "isDisabled";
        public IElement? @_isDisabled = null;

    }

    public _DomainCreateFoundationAction @DomainCreateFoundationAction = new ();
    public MofObjectShadow @__DomainCreateFoundationAction = new ("dm:///intern.types.domains.datenmeister/#9b040443-4675-42b6-8707-0e42450a2102");

    public static readonly _Root TheOne = new ();

}

