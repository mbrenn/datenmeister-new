using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Core.Models.EMOF;

public class PrimitiveTypes
{
    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#Boolean",
        TypeKind = TypeKind.WrappedClass)]
    public class Boolean_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Boolean_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Boolean_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#Boolean");

        public static Boolean_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#Integer",
        TypeKind = TypeKind.WrappedClass)]
    public class Integer_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Integer_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Integer_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#Integer");

        public static Integer_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#Real",
        TypeKind = TypeKind.WrappedClass)]
    public class Real_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public Real_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public Real_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#Real");

        public static Real_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#String",
        TypeKind = TypeKind.WrappedClass)]
    public class String_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public String_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public String_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#String");

        public static String_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#UnlimitedNatural",
        TypeKind = TypeKind.WrappedClass)]
    public class UnlimitedNatural_Wrapper : IElementWrapper
    {
        private readonly IElement _wrappedElement;

        public UnlimitedNatural_Wrapper(IElement innerDmElement)
        {
            _wrappedElement = innerDmElement;
        }

        public UnlimitedNatural_Wrapper(IFactory factory)
        {
            _wrappedElement = factory.create(_metaClass);
        }

        public IElement GetWrappedElement() => _wrappedElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#UnlimitedNatural");

        public static UnlimitedNatural_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

}

