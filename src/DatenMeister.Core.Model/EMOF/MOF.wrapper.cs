using DatenMeister.Core.Interfaces.MOF.Reflection;
using DatenMeister.Core.Interfaces;
using DatenMeister.Core.EMOF.Implementation;

using DatenMeister.Core.Helper;

// ReSharper disable InconsistentNaming
// ReSharper disable RedundantNameQualifier
// Created by DatenMeister.SourcecodeGenerator.WrapperTreeGenerator Version 1.3.0.0
namespace DatenMeister.Core.Models.EMOF;

public class MOF
{
    public class Identifiers
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Identifiers-URIExtent",
            TypeKind = TypeKind.WrappedClass)]
        public class URIExtent_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Identifiers-URIExtent");

            public static URIExtent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Identifiers-Extent",
            TypeKind = TypeKind.WrappedClass)]
        public class Extent_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Identifiers-Extent");

            public static Extent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

    }

    public class EMOF
    {
    }

    public class CMOFExtension
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFExtension-Tag",
            TypeKind = TypeKind.WrappedClass)]
        public class Tag_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFExtension-Tag");

            public static Tag_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper? @tagOwner
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("tagOwner");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("tagOwner", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("tagOwner", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Comment_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Comment_Wrapper? @ownedComment
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("ownedComment");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Comment_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("ownedComment", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("ownedComment", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper? @ownedElement
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("ownedElement");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("ownedElement", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("ownedElement", value);
                    }
                }
            }

            // DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper? @owner
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("owner");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.CommonStructure.Element_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("owner", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("owner", value);
                    }
                }
            }

        }

    }

    public class Extension
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Extension-Tag",
            TypeKind = TypeKind.WrappedClass)]
        public class Tag_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Extension-Tag");

            public static Tag_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            public string? @value
            {
                get =>
                    innerDmElement.getOrDefault<string?>("value");
                set => 
                    innerDmElement.set("value", value);
            }

            // Not found
            public object? @element
            {
                get =>
                    innerDmElement.getOrDefault<object?>("element");
                set => 
                    innerDmElement.set("element", value);
            }

        }

    }

    public class Common
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence",
            TypeKind = TypeKind.WrappedClass)]
        public class ReflectiveSequence_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Common-ReflectiveSequence");

            public static ReflectiveSequence_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection",
            TypeKind = TypeKind.WrappedClass)]
        public class ReflectiveCollection_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Common-ReflectiveCollection");

            public static ReflectiveCollection_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

    }

    public class CMOF
    {
    }

    public class CMOFReflection
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Factory",
            TypeKind = TypeKind.WrappedClass)]
        public class Factory_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Factory");

            public static Factory_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Element",
            TypeKind = TypeKind.WrappedClass)]
        public class Element_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Element");

            public static Element_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Argument",
            TypeKind = TypeKind.WrappedClass)]
        public class Argument_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Argument");

            public static Argument_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            public string? @name
            {
                get =>
                    innerDmElement.getOrDefault<string?>("name");
                set => 
                    innerDmElement.set("name", value);
            }

            // Not found
            public object? @value
            {
                get =>
                    innerDmElement.getOrDefault<object?>("value");
                set => 
                    innerDmElement.set("value", value);
            }

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Extent",
            TypeKind = TypeKind.WrappedClass)]
        public class Extent_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Extent");

            public static Extent_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Link",
            TypeKind = TypeKind.WrappedClass)]
        public class Link_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Link");

            public static Link_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // Not found
            public object? @firstElement
            {
                get =>
                    innerDmElement.getOrDefault<object?>("firstElement");
                set => 
                    innerDmElement.set("firstElement", value);
            }

            // Not found
            public object? @secondElement
            {
                get =>
                    innerDmElement.getOrDefault<object?>("secondElement");
                set => 
                    innerDmElement.set("secondElement", value);
            }

            // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Association_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Association_Wrapper? @association
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("association");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Association_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("association", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("association", value);
                    }
                }
            }

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-CMOFReflection-Exception",
            TypeKind = TypeKind.WrappedClass)]
        public class Exception_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-CMOFReflection-Exception");

            public static Exception_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // Not found
            public object? @objectInError
            {
                get =>
                    innerDmElement.getOrDefault<object?>("objectInError");
                set => 
                    innerDmElement.set("objectInError", value);
            }

            // Not found
            public object? @elementInError
            {
                get =>
                    innerDmElement.getOrDefault<object?>("elementInError");
                set => 
                    innerDmElement.set("elementInError", value);
            }

            public string? @description
            {
                get =>
                    innerDmElement.getOrDefault<string?>("description");
                set => 
                    innerDmElement.set("description", value);
            }

        }

    }

    public class Reflection
    {
        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Factory",
            TypeKind = TypeKind.WrappedClass)]
        public class Factory_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Factory");

            public static Factory_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper? @package
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("package");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.Packages.Package_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("package", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("package", value);
                    }
                }
            }

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Type",
            TypeKind = TypeKind.WrappedClass)]
        public class Type_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Type");

            public static Type_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Object",
            TypeKind = TypeKind.WrappedClass)]
        public class Object_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Object");

            public static Object_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

        }

        [TypeUri(Uri = "dm:///_internal/model/mof#_MOF-Reflection-Element",
            TypeKind = TypeKind.WrappedClass)]
        public class Element_Wrapper(IElement innerDmElement) : IElementWrapper
        {
            public IElement GetWrappedElement() => innerDmElement;

            private static readonly MofObjectShadow _metaClass = new ("dm:///_internal/model/mof#_MOF-Reflection-Element");

            public static Element_Wrapper Create(IFactory factory) => new (factory.create(_metaClass));

            // DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper
            public DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper? @metaclass
            {
                get
                {
                    var foundElement = innerDmElement.getOrDefault<IElement>("metaclass");
                    return foundElement == null ? null : new DatenMeister.Core.Models.EMOF.UML.StructuredClassifiers.Class_Wrapper(foundElement);
                }
                set 
                {
                    if(value is IElementWrapper wrappedElement)
                    {
                        innerDmElement.set("metaclass", wrappedElement.GetWrappedElement());
                    }
                    else
                    {
                        innerDmElement.set("metaclass", value);
                    }
                }
            }

        }

    }

}

