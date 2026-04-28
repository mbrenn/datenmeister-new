using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace IssueMeisterLib.Models;

public class IssueMeister
{
    [TypeUri(Uri = "dm:///intern.types.issues.datenmeister/#IssueMeister.Issue",
        TypeKind = TypeKind.WrappedClass)]
    public class Issue_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Issue_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Issue_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.issues.datenmeister/#IssueMeister.Issue");

        public static Issue_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public int @id
        {
            get =>
                _wrappedElement.getOrDefault<int>("id");
            set => 
                _wrappedElement.set("id", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // Not found
        public object? @state
        {
            get =>
                _wrappedElement.getOrDefault<object?>("state");
            set => 
                _wrappedElement.set("state", value);
        }

        public string? @description
        {
            get =>
                _wrappedElement.getOrDefault<string?>("description");
            set => 
                _wrappedElement.set("description", value);
        }

        // Not found
        public object? @iteration
        {
            get =>
                _wrappedElement.getOrDefault<object?>("iteration");
            set => 
                _wrappedElement.set("iteration", value);
        }

    }

    [TypeUri(Uri = "dm:///intern.types.issues.datenmeister/#IssueMeister.Iteration",
        TypeKind = TypeKind.WrappedClass)]
    public class Iteration_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Iteration_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Iteration_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.issues.datenmeister/#IssueMeister.Iteration");

        public static Iteration_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @id
        {
            get =>
                _wrappedElement.getOrDefault<string?>("id");
            set => 
                _wrappedElement.set("id", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        // Not found
        public object? @endDate
        {
            get =>
                _wrappedElement.getOrDefault<object?>("endDate");
            set => 
                _wrappedElement.set("endDate", value);
        }

    }

}

