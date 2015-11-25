// Created by DatenMeister.SourcecodeGenerator.ClassTreeGenerator Version 1.0.0.0
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
            public object @URIExtentInstance = new object();

            public class _Extent
            {
            }

            public _Extent @Extent = new _Extent();
            public object @ExtentInstance = new object();

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
                public object @tagOwner = new object();

            }

            public _Tag @Tag = new _Tag();
            public object @TagInstance = new object();

        }

        public _CMOFExtension CMOFExtension = new _CMOFExtension();

        public class _Extension
        {
            public class _Tag
            {
                public object @name = new object();

                public object @value = new object();

                public object @element = new object();

            }

            public _Tag @Tag = new _Tag();
            public object @TagInstance = new object();

        }

        public _Extension Extension = new _Extension();

        public class _Common
        {
            public class _ReflectiveSequence
            {
            }

            public _ReflectiveSequence @ReflectiveSequence = new _ReflectiveSequence();
            public object @ReflectiveSequenceInstance = new object();

            public class _ReflectiveCollection
            {
            }

            public _ReflectiveCollection @ReflectiveCollection = new _ReflectiveCollection();
            public object @ReflectiveCollectionInstance = new object();

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
            public object @FactoryInstance = new object();

            public class _Element
            {
            }

            public _Element @Element = new _Element();
            public object @ElementInstance = new object();

            public class _Argument
            {
                public object @name = new object();

                public object @value = new object();

            }

            public _Argument @Argument = new _Argument();
            public object @ArgumentInstance = new object();

            public class _Extent
            {
            }

            public _Extent @Extent = new _Extent();
            public object @ExtentInstance = new object();

            public class _Link
            {
                public object @firstElement = new object();

                public object @secondElement = new object();

                public object @association = new object();

            }

            public _Link @Link = new _Link();
            public object @LinkInstance = new object();

            public class _Exception
            {
                public object @objectInError = new object();

                public object @elementInError = new object();

                public object @description = new object();

            }

            public _Exception @Exception = new _Exception();
            public object @ExceptionInstance = new object();

        }

        public _CMOFReflection CMOFReflection = new _CMOFReflection();

        public class _Reflection
        {
            public class _Factory
            {
                public object @package = new object();

            }

            public _Factory @Factory = new _Factory();
            public object @FactoryInstance = new object();

            public class _Type
            {
            }

            public _Type @Type = new _Type();
            public object @TypeInstance = new object();

            public class _Object
            {
            }

            public _Object @Object = new _Object();
            public object @ObjectInstance = new object();

            public class _Element
            {
                public object @metaclass = new object();

            }

            public _Element @Element = new _Element();
            public object @ElementInstance = new object();

        }

        public _Reflection Reflection = new _Reflection();

        public static _MOF TheOne = new _MOF();

    }

}
