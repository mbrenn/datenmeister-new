using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Domains.Model;

public class Root
{
    [TypeUri(Uri = "dm:///intern.types.domains.datenmeister/#9b040443-4675-42b6-8707-0e42450a2102",
        TypeKind = TypeKind.WrappedClass)]
    public class DomainCreateFoundationAction_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public DomainCreateFoundationAction_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public DomainCreateFoundationAction_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///intern.types.domains.datenmeister/#9b040443-4675-42b6-8707-0e42450a2102");

        public static DomainCreateFoundationAction_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        public string? @filePath
        {
            get =>
                _wrappedElement.getOrDefault<string?>("filePath");
            set => 
                _wrappedElement.set("filePath", value);
        }

        public bool @createDataExtent
        {
            get =>
                _wrappedElement.getOrDefault<bool>("createDataExtent");
            set => 
                _wrappedElement.set("createDataExtent", value);
        }

        public string? @extentUriPrefix
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUriPrefix");
            set => 
                _wrappedElement.set("extentUriPrefix", value);
        }

        public string? @extentUriPostfix
        {
            get =>
                _wrappedElement.getOrDefault<string?>("extentUriPostfix");
            set => 
                _wrappedElement.set("extentUriPostfix", value);
        }

        public string? @name
        {
            get =>
                _wrappedElement.getOrDefault<string?>("name");
            set => 
                _wrappedElement.set("name", value);
        }

        public bool @isDisabled
        {
            get =>
                _wrappedElement.getOrDefault<bool>("isDisabled");
            set => 
                _wrappedElement.set("isDisabled", value);
        }

    }

}

