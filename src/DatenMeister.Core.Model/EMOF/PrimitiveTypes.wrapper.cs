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
    public class Boolean_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#Boolean");

        public static Boolean_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#Integer",
        TypeKind = TypeKind.WrappedClass)]
    public class Integer_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#Integer");

        public static Integer_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#Real",
        TypeKind = TypeKind.WrappedClass)]
    public class Real_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#Real");

        public static Real_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#String",
        TypeKind = TypeKind.WrappedClass)]
    public class String_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#String");

        public static String_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

    [TypeUri(Uri = "dm:///_internal/model/primitivetypes#UnlimitedNatural",
        TypeKind = TypeKind.WrappedClass)]
    public class UnlimitedNatural_Wrapper(IElement innerDmElement) : IElementWrapper
    {
        public IElement GetWrappedElement() => innerDmElement;

        private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/primitivetypes#UnlimitedNatural");

        public static UnlimitedNatural_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

    }

}

