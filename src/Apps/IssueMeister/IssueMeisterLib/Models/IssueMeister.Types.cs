using DatenMeister.Core.Interfaces;
using DatenMeister.Core.Interfaces.MOF.Reflection;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace IssueMeisterLib.Models;

public class _IssueMeister
{
    [TypeUri(Uri = "dm:///intern.types.issues.datenmeister/#IssueMeister.Issue",
        TypeKind = TypeKind.ClassTree)]
    public class _Issue
    {
        public static readonly string @id = "id";
        public IElement? @_id = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @state = "state";
        public IElement? @_state = null;

        public static readonly string @description = "description";
        public IElement? @_description = null;

        public static readonly string @iteration = "iteration";
        public IElement? @_iteration = null;

    }

    public _Issue @Issue = new ();
    public MofObjectShadow @__Issue = new ("dm:///intern.types.issues.datenmeister/#IssueMeister.Issue");

    public class _IssueState
    {
        public static string @Open = "Open";
        public IElement? @__Open = null;
        public static string @InWork = "InWork";
        public IElement? @__InWork = null;
        public static string @Closed = "Closed";
        public IElement? @__Closed = null;

    }

    public _IssueState @IssueState = new _IssueState();
    public IElement @__IssueState = new MofObjectShadow("dm:///intern.types.issues.datenmeister/#IssueMeister.IssueState");


    public enum ___IssueState
    {
        @Open,
        @InWork,
        @Closed
    }

    [TypeUri(Uri = "dm:///intern.types.issues.datenmeister/#IssueMeister.Iteration",
        TypeKind = TypeKind.ClassTree)]
    public class _Iteration
    {
        public static readonly string @id = "id";
        public IElement? @_id = null;

        public static readonly string @name = "name";
        public IElement? @_name = null;

        public static readonly string @endDate = "endDate";
        public IElement? @_endDate = null;

    }

    public _Iteration @Iteration = new ();
    public MofObjectShadow @__Iteration = new ("dm:///intern.types.issues.datenmeister/#IssueMeister.Iteration");

    public static readonly _IssueMeister TheOne = new ();

}

