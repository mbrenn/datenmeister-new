#nullable enable
using DatenMeister.Core.EMOF.Implementation;
using DatenMeister.Core.EMOF.Interface.Reflection;

// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.2.0.0
namespace DatenMeister.Core.Models.EMOF
{
    public class _MOF
    {
        public class _Identifiers
        {
            public class _URIExtent
            {
            }

            public _URIExtent URIExtent = new _URIExtent();
            public IElement __URIExtent = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Identifiers-URIExtent");

            public class _Extent
            {
            }

            public _Extent Extent = new _Extent();
            public IElement __Extent = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Identifiers-Extent");

        }

        public _Identifiers Identifiers = new _Identifiers();

        public class _EMOF
        {
        }

        public _EMOF EMOF = new _EMOF();

        public class _CMOFExtension
        {
            public class _Tag
            {
                public static string tagOwner = "tagOwner";
                public IElement _tagOwner = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFExtension-Tag-tagOwner");

            }

            public _Tag Tag = new _Tag();
            public IElement __Tag = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFExtension-Tag");

        }

        public _CMOFExtension CMOFExtension = new _CMOFExtension();

        public class _Extension
        {
            public class _Tag
            {
                public static string name = "name";
                public IElement _name = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag-name");

                public static string value = "value";
                public IElement _value = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag-value");

                public static string element = "element";
                public IElement _element = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag-element");

                public static string metaclass = "metaclass";
                public IElement _metaclass = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Element-metaclass");

            }

            public _Tag Tag = new _Tag();
            public IElement __Tag = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Extension-Tag");

        }

        public _Extension Extension = new _Extension();

        public class _Common
        {
            public class _ReflectiveSequence
            {
            }

            public _ReflectiveSequence ReflectiveSequence = new _ReflectiveSequence();
            public IElement __ReflectiveSequence = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence");

            public class _ReflectiveCollection
            {
            }

            public _ReflectiveCollection ReflectiveCollection = new _ReflectiveCollection();
            public IElement __ReflectiveCollection = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection");

        }

        public _Common Common = new _Common();

        public class _CMOF
        {
        }

        public _CMOF CMOF = new _CMOF();

        public class _CMOFReflection
        {
            public class _Factory
            {
            }

            public _Factory Factory = new _Factory();
            public IElement __Factory = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Factory");

            public class _Element
            {
            }

            public _Element Element = new _Element();
            public IElement __Element = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Element");

            public class _Argument
            {
                public static string name = "name";
                public IElement _name = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument-name");

                public static string value = "value";
                public IElement _value = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument-value");

            }

            public _Argument Argument = new _Argument();
            public IElement __Argument = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument");

            public class _Extent
            {
            }

            public _Extent Extent = new _Extent();
            public IElement __Extent = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Extent");

            public class _Link
            {
                public static string firstElement = "firstElement";
                public IElement _firstElement = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link-firstElement");

                public static string secondElement = "secondElement";
                public IElement _secondElement = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link-secondElement");

                public static string association = "association";
                public IElement _association = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link-association");

            }

            public _Link Link = new _Link();
            public IElement __Link = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Link");

            public class _Exception
            {
                public static string objectInError = "objectInError";
                public IElement _objectInError = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception-objectInError");

                public static string elementInError = "elementInError";
                public IElement _elementInError = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception-elementInError");

                public static string description = "description";
                public IElement _description = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception-description");

            }

            public _Exception Exception = new _Exception();
            public IElement __Exception = new MofObjectShadow("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception");

        }

        public _CMOFReflection CMOFReflection = new _CMOFReflection();

        public class _Reflection
        {
            public class _Factory
            {
                public static string package = "package";
                public IElement _package = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Factory-package");

                public static string metaclass = "metaclass";
                public IElement _metaclass = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Element-metaclass");

            }

            public _Factory Factory = new _Factory();
            public IElement __Factory = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Factory");

            public class _Type
            {
            }

            public _Type Type = new _Type();
            public IElement __Type = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Type");

            public class _Object
            {
            }

            public _Object Object = new _Object();
            public IElement __Object = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Object");

            public class _Element
            {
                public static string metaclass = "metaclass";
                public IElement _metaclass = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Element-metaclass");

            }

            public _Element Element = new _Element();
            public IElement __Element = new MofObjectShadow("dm:///_internal/model/mof#_MOF-Reflection-Element");

        }

        public _Reflection Reflection = new _Reflection();

        public static readonly _MOF TheOne = new _MOF();

    }

}
