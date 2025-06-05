using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.3.0.0
namespace DatenMeister.Core.Models.EMOF
{
    public class _MOF
    {
        public class _Identifiers
        {
            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Identifiers-URIExtent",
                TypeKind = TypeKind.ClassTree)]
            public class _URIExtent
            {
            }

            public _URIExtent @URIExtent = new _URIExtent();
            public MofObjectShadow @__URIExtent = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Identifiers-URIExtent");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Identifiers-Extent",
                TypeKind = TypeKind.ClassTree)]
            public class _Extent
            {
            }

            public _Extent @Extent = new _Extent();
            public MofObjectShadow @__Extent = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Identifiers-Extent");

        }

        public _Identifiers Identifiers = new _Identifiers();

        public class _EMOF
        {
        }

        public _EMOF EMOF = new _EMOF();

        public class _CMOFExtension
        {
            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFExtension-Tag",
                TypeKind = TypeKind.ClassTree)]
            public class _Tag
            {
                public static string @tagOwner = "tagOwner";
                public IElement @_tagOwner = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFExtension-Tag-tagOwner");

                public static string @ownedComment = "ownedComment";
                public IElement @_ownedComment = new MofObjectShadow("dm:///_internal/model/uml#Element-ownedComment");

                public static string @ownedElement = "ownedElement";
                public IElement @_ownedElement = new MofObjectShadow("dm:///_internal/model/uml#Element-ownedElement");

                public static string @owner = "owner";
                public IElement @_owner = new MofObjectShadow("dm:///_internal/model/uml#Element-owner");

            }

            public _Tag @Tag = new _Tag();
            public MofObjectShadow @__Tag = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFExtension-Tag");

        }

        public _CMOFExtension CMOFExtension = new _CMOFExtension();

        public class _Extension
        {
            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Extension-Tag",
                TypeKind = TypeKind.ClassTree)]
            public class _Tag
            {
                public static string @name = "name";
                public IElement @_name = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag-name");

                public static string @value = "value";
                public IElement @_value = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag-value");

                public static string @element = "element";
                public IElement @_element = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag-element");

            }

            public _Tag @Tag = new _Tag();
            public MofObjectShadow @__Tag = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag");

        }

        public _Extension Extension = new _Extension();

        public class _Common
        {
            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence",
                TypeKind = TypeKind.ClassTree)]
            public class _ReflectiveSequence
            {
            }

            public _ReflectiveSequence @ReflectiveSequence = new _ReflectiveSequence();
            public MofObjectShadow @__ReflectiveSequence = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection",
                TypeKind = TypeKind.ClassTree)]
            public class _ReflectiveCollection
            {
            }

            public _ReflectiveCollection @ReflectiveCollection = new _ReflectiveCollection();
            public MofObjectShadow @__ReflectiveCollection = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection");

        }

        public _Common Common = new _Common();

        public class _CMOF
        {
        }

        public _CMOF CMOF = new _CMOF();

        public class _CMOFReflection
        {
            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Factory",
                TypeKind = TypeKind.ClassTree)]
            public class _Factory
            {
            }

            public _Factory @Factory = new _Factory();
            public MofObjectShadow @__Factory = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Factory");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Element",
                TypeKind = TypeKind.ClassTree)]
            public class _Element
            {
            }

            public _Element @Element = new _Element();
            public MofObjectShadow @__Element = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Element");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Argument",
                TypeKind = TypeKind.ClassTree)]
            public class _Argument
            {
                public static string @name = "name";
                public IElement @_name = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument-name");

                public static string @value = "value";
                public IElement @_value = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument-value");

            }

            public _Argument @Argument = new _Argument();
            public MofObjectShadow @__Argument = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Extent",
                TypeKind = TypeKind.ClassTree)]
            public class _Extent
            {
            }

            public _Extent @Extent = new _Extent();
            public MofObjectShadow @__Extent = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Extent");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Link",
                TypeKind = TypeKind.ClassTree)]
            public class _Link
            {
                public static string @firstElement = "firstElement";
                public IElement @_firstElement = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link-firstElement");

                public static string @secondElement = "secondElement";
                public IElement @_secondElement = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link-secondElement");

                public static string @association = "association";
                public IElement @_association = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link-association");

            }

            public _Link @Link = new _Link();
            public MofObjectShadow @__Link = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Exception",
                TypeKind = TypeKind.ClassTree)]
            public class _Exception
            {
                public static string @objectInError = "objectInError";
                public IElement @_objectInError = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception-objectInError");

                public static string @elementInError = "elementInError";
                public IElement @_elementInError = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception-elementInError");

                public static string @description = "description";
                public IElement @_description = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception-description");

            }

            public _Exception @Exception = new _Exception();
            public MofObjectShadow @__Exception = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception");

        }

        public _CMOFReflection CMOFReflection = new _CMOFReflection();

        public class _Reflection
        {
            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Factory",
                TypeKind = TypeKind.ClassTree)]
            public class _Factory
            {
                public static string @package = "package";
                public IElement @_package = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Factory-package");

            }

            public _Factory @Factory = new _Factory();
            public MofObjectShadow @__Factory = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Factory");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Type",
                TypeKind = TypeKind.ClassTree)]
            public class _Type
            {
            }

            public _Type @Type = new _Type();
            public MofObjectShadow @__Type = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Type");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Object",
                TypeKind = TypeKind.ClassTree)]
            public class _Object
            {
            }

            public _Object @Object = new _Object();
            public MofObjectShadow @__Object = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Object");

            [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Element",
                TypeKind = TypeKind.ClassTree)]
            public class _Element
            {
                public static string @metaclass = "metaclass";
                public IElement @_metaclass = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Element-metaclass");

            }

            public _Element @Element = new _Element();
            public MofObjectShadow @__Element = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Element");

        }

        public _Reflection Reflection = new _Reflection();

        public static readonly _MOF TheOne = new _MOF();

    }

}
