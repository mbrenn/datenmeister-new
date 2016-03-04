using DatenMeister.EMOF.Interface.Reflection;
using DatenMeister.EMOF.InMemory;

// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.1.0.0
namespace DatenMeister
{
    public class _MOF
    {
        public class _Identifiers
        {
            public class _URIExtent
            {
            }

            public _URIExtent @URIExtent = new _URIExtent();
            public IElement @__URIExtent = new MofElement();

            public class _Extent
            {
            }

            public _Extent @Extent = new _Extent();
            public IElement @__Extent = new MofElement();

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
                public object @tagOwner = "tagOwner";

            }

            public _Tag @Tag = new _Tag();
            public IElement @__Tag = new MofElement();

        }

        public _CMOFExtension CMOFExtension = new _CMOFExtension();

        public class _Extension
        {
            public class _Tag
            {
                public object @name = "name";

                public object @value = "value";

                public object @element = "element";

            }

            public _Tag @Tag = new _Tag();
            public IElement @__Tag = new MofElement();

        }

        public _Extension Extension = new _Extension();

        public class _Common
        {
            public class _ReflectiveSequence
            {
            }

            public _ReflectiveSequence @ReflectiveSequence = new _ReflectiveSequence();
            public IElement @__ReflectiveSequence = new MofElement();

            public class _ReflectiveCollection
            {
            }

            public _ReflectiveCollection @ReflectiveCollection = new _ReflectiveCollection();
            public IElement @__ReflectiveCollection = new MofElement();

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

            public _Factory @Factory = new _Factory();
            public IElement @__Factory = new MofElement();

            public class _Element
            {
            }

            public _Element @Element = new _Element();
            public IElement @__Element = new MofElement();

            public class _Argument
            {
                public object @name = "name";

                public object @value = "value";

            }

            public _Argument @Argument = new _Argument();
            public IElement @__Argument = new MofElement();

            public class _Extent
            {
            }

            public _Extent @Extent = new _Extent();
            public IElement @__Extent = new MofElement();

            public class _Link
            {
                public object @firstElement = "firstElement";

                public object @secondElement = "secondElement";

                public object @association = "association";

            }

            public _Link @Link = new _Link();
            public IElement @__Link = new MofElement();

            public class _Exception
            {
                public object @objectInError = "objectInError";

                public object @elementInError = "elementInError";

                public object @description = "description";

            }

            public _Exception @Exception = new _Exception();
            public IElement @__Exception = new MofElement();

        }

        public _CMOFReflection CMOFReflection = new _CMOFReflection();

        public class _Reflection
        {
            public class _Factory
            {
                public object @package = "package";

            }

            public _Factory @Factory = new _Factory();
            public IElement @__Factory = new MofElement();

            public class _Type
            {
            }

            public _Type @Type = new _Type();
            public IElement @__Type = new MofElement();

            public class _Object
            {
            }

            public _Object @Object = new _Object();
            public IElement @__Object = new MofElement();

            public class _Element
            {
                public object @metaclass = "metaclass";

            }

            public _Element @Element = new _Element();
            public IElement @__Element = new MofElement();

        }

        public _Reflection Reflection = new _Reflection();

        public static _MOF TheOne = new _MOF();

    }

}
