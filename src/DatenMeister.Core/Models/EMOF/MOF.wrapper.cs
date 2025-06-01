using DatenMeister.Core.EMOF.Interface.Reflection;
using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.2.0.0
namespace DatenMeister.Core.Models.EMOF
{
    public class MOF
    {
        public class Identifiers
        {
            public class URIExtent_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

            public class Extent_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

        }

        public class EMOF
        {
        }

        public class CMOFExtension
        {
            public class Tag_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public object? @tagOwner
                {
                    get =>
                        innerDmElement.get("tagOwner");
                    set => 
                        innerDmElement.set("tagOwner", value);
                }

                public object? @ownedComment
                {
                    get =>
                        innerDmElement.get("ownedComment");
                    set => 
                        innerDmElement.set("ownedComment", value);
                }

                public object? @ownedElement
                {
                    get =>
                        innerDmElement.get("ownedElement");
                    set => 
                        innerDmElement.set("ownedElement", value);
                }

                public object? @owner
                {
                    get =>
                        innerDmElement.get("owner");
                    set => 
                        innerDmElement.set("owner", value);
                }

            }

        }

        public class Extension
        {
            public class Tag_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @name
                {
                    get =>
                        innerDmElement.getOrDefault<string>("name");
                    set => 
                        innerDmElement.set("name", value);
                }

                public string @value
                {
                    get =>
                        innerDmElement.getOrDefault<string>("value");
                    set => 
                        innerDmElement.set("value", value);
                }

                public object? @element
                {
                    get =>
                        innerDmElement.get("element");
                    set => 
                        innerDmElement.set("element", value);
                }

            }

        }

        public class Common
        {
            public class ReflectiveSequence_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

            public class ReflectiveCollection_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

        }

        public class CMOF
        {
        }

        public class CMOFReflection
        {
            public class Factory_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

            public class Element_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

            public class Argument_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public string @name
                {
                    get =>
                        innerDmElement.getOrDefault<string>("name");
                    set => 
                        innerDmElement.set("name", value);
                }

                public object? @value
                {
                    get =>
                        innerDmElement.get("value");
                    set => 
                        innerDmElement.set("value", value);
                }

            }

            public class Extent_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

            public class Link_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public object? @firstElement
                {
                    get =>
                        innerDmElement.get("firstElement");
                    set => 
                        innerDmElement.set("firstElement", value);
                }

                public object? @secondElement
                {
                    get =>
                        innerDmElement.get("secondElement");
                    set => 
                        innerDmElement.set("secondElement", value);
                }

                public object? @association
                {
                    get =>
                        innerDmElement.get("association");
                    set => 
                        innerDmElement.set("association", value);
                }

            }

            public class Exception_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public object? @objectInError
                {
                    get =>
                        innerDmElement.get("objectInError");
                    set => 
                        innerDmElement.set("objectInError", value);
                }

                public object? @elementInError
                {
                    get =>
                        innerDmElement.get("elementInError");
                    set => 
                        innerDmElement.set("elementInError", value);
                }

                public string @description
                {
                    get =>
                        innerDmElement.getOrDefault<string>("description");
                    set => 
                        innerDmElement.set("description", value);
                }

            }

        }

        public class Reflection
        {
            public class Factory_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public object? @package
                {
                    get =>
                        innerDmElement.get("package");
                    set => 
                        innerDmElement.set("package", value);
                }

            }

            public class Type_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

            public class Object_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

            }

            public class Element_Wrapper(IElement innerDmElement)
            {
                public IElement GetWrappedElement() => innerDmElement;

                public object? @metaclass
                {
                    get =>
                        innerDmElement.get("metaclass");
                    set => 
                        innerDmElement.set("metaclass", value);
                }

            }

        }

    }

}
